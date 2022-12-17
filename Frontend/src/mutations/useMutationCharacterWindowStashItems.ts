import { useMutation } from "@tanstack/react-query";
import { AxiosError } from "axios";
import { CharacterWindowStashItemInput, getCharacterWindowStashItems, StashItem } from "../api/getCharacterWindowStashItems";

export function useMutationCharacterWindowStashItems() {
  return useMutation<StashItem[], AxiosError, CharacterWindowStashItemInput>((props) => getCharacterWindowStashItems(props));
}
