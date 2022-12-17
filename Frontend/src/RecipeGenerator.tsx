import React, { useEffect, useMemo, useState } from "react";
import _groupBy from "lodash/groupBy";
import Box from "@mui/material/Box";
import Stack from "@mui/material/Stack";
import Typography from "@mui/material/Typography";
import Tooltip from "@mui/material/Tooltip";
import { StashItem } from "./api/getCharacterWindowStashItems";
import { useMutationCharacterWindowStashItems } from "./mutations/useMutationCharacterWindowStashItems";
import TextInput from "./components/TextInput";
import SelectItem from "./components/SelectItem";
import Button from "@mui/material/Button";
import { useLocalStorage } from "usehooks-ts";
import Alert from "@mui/material/Alert";
import LoadingButton from "@mui/lab/LoadingButton";

const qualityRanges: Record<number, string> = {
  20: "Quality 20%",
  16: "Quality 16-19%",
  11: "Quality 11-15%",
  6: "Quality 6-10%",
  1: "Quality 1-5%",
  0: "Quality 0%",
};
const qualityRangesKeys = Object.keys(qualityRanges)
  .map((x) => Number(x))
  .sort((x) => x)
  .reverse();

interface StashItemModel extends StashItem {
  isHighlighted: boolean;
  isHidden: boolean;
}

// TODO: refactor
const RecipeGenerator: React.FC = () => {
  const [sessionId, setSessionId] = useLocalStorage("sessionId", "");
  const [accountName, setAccountName] = useLocalStorage("accountName", "");
  const [realm, setRealm] = useLocalStorage("realm", "pc");
  const [league, setLeague] = useLocalStorage("league", "Sanctum");
  const [stashTabName, setStashTabName] = useLocalStorage("stashTabName", "");
  const [stashType, setStashType] = useLocalStorage("stashType", "Quality");
  const [pageIndex, setPageIndex] = useState(0);
  const [maxPages, setMaxPages] = useState(0);
  const [groupedByQuality, setGroupedByQuality] = useState<[number, string, StashItemModel[]][]>([]);
  const { data, error, isLoading, mutate } = useMutationCharacterWindowStashItems();

  const generate = () => {
    setMaxPages(0);
    setPageIndex(0);
    mutate({ sessionId, accountName, realm, league, stashTabName });
  };

  const itemModels = useMemo(
    () =>
      data &&
      data.map<StashItemModel>((x) => ({
        ...x,
        isHighlighted: false,
        isHidden: false,
      })),
    [data]
  );

  const paginatedHighlightedStashItemModels = useMemo(() => itemModels && getSets(itemModels, 40, 20), [itemModels]);

  const renderData = useMemo(
    () => (
      <div className="stash quality-stash">
        {groupedByQuality &&
          groupedByQuality.map((group) => (
            <div className="quality-group" data-title={group[1]} key={group[1]}>
              {group[2].map((item, index) => {
                return (
                  !item.isHidden && (
                    <Tooltip title={<Typography>{item.name}</Typography>} placement="top" key={index}>
                      <div className="item" key={index}>
                        <img src={item.iconUrl} alt="item-view" />
                        {item.isHighlighted && <div className="highlight" />}
                      </div>
                    </Tooltip>
                  )
                );
              })}
            </div>
          ))}
      </div>
    ),
    [groupedByQuality]
  );

  useEffect(() => {
    if (!itemModels) {
      return;
    }

    console.log("inital models", itemModels);
    const result = Object.entries(_groupBy(itemModels, (x) => qualityRangesKeys.find((r) => r <= x.quality)))
      .map<[number, string, StashItemModel[]]>(([key, value]) => [
        Number(key),
        qualityRanges[Number(key)],
        value.sort((a, b) => a.name.localeCompare(b.name)),
      ])
      .sort(([key]) => key)
      .reverse();

    console.log("grouped quality", result);
    setGroupedByQuality(result);
  }, [itemModels]);

  useEffect(() => {
    if (!paginatedHighlightedStashItemModels) {
      return;
    }
    const maxPages = paginatedHighlightedStashItemModels.length;
    setMaxPages(maxPages);
    if (paginatedHighlightedStashItemModels.length === 0) {
      return;
    }
    paginatedHighlightedStashItemModels[pageIndex].forEach((x) => {
      x.isHighlighted = true;
      x.isHidden = false;
    });
    paginatedHighlightedStashItemModels
      .slice(0, pageIndex)
      .flat()
      .forEach((x) => {
        x.isHighlighted = false;
        x.isHidden = true;
      });
    paginatedHighlightedStashItemModels
      .slice(pageIndex + 1, maxPages)
      .flat()
      .forEach((x) => {
        x.isHighlighted = false;
        x.isHidden = false;
      });
    console.log("highlights", paginatedHighlightedStashItemModels);
    setGroupedByQuality(function (x) {
      return [...x];
    });
  }, [pageIndex, paginatedHighlightedStashItemModels]);

  return (
    <Stack>
      <Box
        component="form"
        sx={{
          "& > *": { m: 1, width: 500 },
          "& > .MuiFormControl-root": { m: 1 },
          "& > .MuiButtonBase-root": { m: 1 },
          display: "flex",
          flexDirection: "column",
        }}
      >
        <TextInput label="POESESSID" value={sessionId} onChange={setSessionId} required />
        <TextInput label="Account Name" value={accountName} onChange={setAccountName} required />
        <SelectItem label="Realm" value={realm} options={["pc", "xbox", "sony"]} onChange={setRealm} required />
        <SelectItem label="League" value={league} options={["Sanctum"]} onChange={setLeague} required />
        <TextInput label="Stash Tab Name" value={stashTabName} onChange={setStashTabName} required />
        <SelectItem label="Stash Type" value={stashType} options={["Quality"]} onChange={setStashType} required />
        <LoadingButton variant="contained" onClick={generate} loading={isLoading}>
          Generate
        </LoadingButton>
        <Stack direction="row" spacing={2} sx={{ m: 1 }}>
          <Button variant="contained" disabled={isLoading || pageIndex <= 0} onClick={() => setPageIndex((p) => p - 1)}>
            Previous
          </Button>
          <Button variant="contained" disabled={isLoading || pageIndex >= maxPages - 1} onClick={() => setPageIndex((p) => p + 1)}>
            Next
          </Button>
        </Stack>
        {error && <Alert severity="error">Failed to retrieve stash items.</Alert>}
      </Box>
      <div style={{ alignSelf: "center" }}>{data && renderData}</div>
    </Stack>
  );
};

function getSets(items: StashItemModel[], sum: number, instantValue: number): StashItemModel[][] {
  const result: StashItemModel[][] = [];
  const orderedByQuality = items
    .filter((x) => x.quality > 0)
    .sort((x) => x.quality)
    .reverse();
  const instants = orderedByQuality.filter((x) => x.quality >= instantValue && (x.itemLevel === 0 || x.explicitMods.length === 0));

  if (instants.length !== 0) {
    result.push(instants);
  }

  let restSet = orderedByQuality.filter((x) => !instants.find((y) => y.x === x.x && y.y === x.y));

  if (restSet.length !== 0) {
    do {
      const list = rec(restSet, sum, 0);
      if (list === null) {
        break;
      }

      restSet = restSet.filter((x) => list.indexOf(x) < 0);
      result.push(list);
    } while (restSet.length !== 0);
  }

  return result;
}

function rec(items: StashItemModel[], sum: number, index: number): StashItemModel[] | null {
  if (sum < 0) {
    return null;
  }

  let result: StashItemModel[] | null = null;
  let i: StashItemModel;
  do {
    i = items[index++];

    if (sum - i.quality === 0) {
      return [i];
    }

    if (index >= items.length) {
      return null;
    }

    result = rec(items, sum - i.quality, index);
  } while (result === null);

  result.push(i);
  return result;
}

export default RecipeGenerator;
