import { axiosInstance } from "./axiosInstance";

export interface CharacterWindowStashItemInput {
  sessionId: string;
  accountName: string;
  realm: string;
  league: string;
  stashTabName: string;
}

export interface StashItem {
  iconUrl: string;
  quality: number;
  itemLevel: number;
  explicitMods: string[];
  width: number;
  x: number;
  y: number;
  name: string;
}

export function getCharacterWindowStashItems(props: CharacterWindowStashItemInput) {
  return axiosInstance
    .get<StashItem[]>("/api/character-window/get-stash-items", {
      params: {
        accountName: props.accountName,
        sessionId: props.sessionId,
        realm: props.realm,
        league: props.league,
        stashTabName: props.stashTabName,
      },
    })
    .then((r) => r.data);
}
