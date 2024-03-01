import { StateCreator } from 'zustand';

import { site789y } from '@/constants';
import { BetSettingGroupTypeIds, GroupTypeIds } from '@/constants/product';
import {
  GroupItem,
  GroupTypeDict,
  OtherGamesBetSettingItem,
  ProductGroupDict,
  RNGGamesBetSettingItem,
  SportsBookBetSettingItem,
} from '@/types/new-account';

import { validateOtherGames } from './BetSettingHandler/otherGamesHandler';
import { copyRNGGamesBetSettings, validateRNGGames } from './BetSettingHandler/rngGamesHandler';
import { copySportsBookBetSettings, validateSportsBook } from './BetSettingHandler/sportsBookHandler';

const intiBetSettingData = (productGroupsDict: ProductGroupDict) => {
  const betSettings = Object.keys(BetSettingGroupTypeIds).reduce((acc: GroupTypeDict, cur) => {
    const betSettingGroup = productGroupsDict[Number(cur)]?.groupTypes.find(
      (groupType) => groupType.id === BetSettingGroupTypeIds[Number(cur)],
    );

    if (betSettingGroup) {
      acc[BetSettingGroupTypeIds[Number(cur)]] = betSettingGroup;
    }

    return acc;
  }, {});

  return betSettings;
};

export type BetSettingSlice = {
  site: string;
  upLineBetSettings: GroupTypeDict;
  betSettings: GroupTypeDict;
  betSettingErrors: string[];
  initBetSettingState: (site: string, productGroups: ProductGroupDict) => void;
  clearBetSettingError: () => void;
  updateBetSettingItem: (groupTypeId: number, betSettingGroupItem: GroupItem) => void;
  handleCopySportsBook: (copyId: string, upLineItem: SportsBookBetSettingItem) => boolean;
  handleCopyRngGames: (copyId: string, upLineItem: RNGGamesBetSettingItem) => boolean;
  validateSportsBookBetSetting: () => boolean;
  validateRNGGamesBetSetting: () => boolean;
  validateOtherGamesBetSetting: () => boolean;
};

const createBetSettingSlice: StateCreator<BetSettingSlice, [], [], BetSettingSlice> = (set, get) => ({
  site: '',
  upLineBetSettings: {},
  betSettings: {},
  betSettingErrors: [],
  initBetSettingState: (site, productGroups) => {
    const betSettings = intiBetSettingData(productGroups);
    set({ site: site.toLocaleLowerCase(), betSettingErrors: [], upLineBetSettings: betSettings, betSettings });
  },
  clearBetSettingError: () => set({ betSettingErrors: [] }),
  updateBetSettingItem: (groupTypeId, betSettingGroupItem) =>
    set((state) => {
      const newBetSettings = [...state.betSettings[groupTypeId].groupItems].map((item) => {
        if (item.sportTypeId === betSettingGroupItem.sportTypeId && item.betTypeId === betSettingGroupItem.betTypeId) {
          return betSettingGroupItem;
        }

        return item;
      });

      return {
        betSettingErrors: [],
        betSettings: {
          ...state.betSettings,
          [groupTypeId]: {
            ...state.betSettings[groupTypeId],
            groupItems: newBetSettings,
          },
        },
      };
    }),
  handleCopySportsBook: (copyId, upLineItem) => {
    const { site, betSettings } = get();
    const [sportTypeId, betTypeId] = copyId.split('-');
    const sportsBookId = GroupTypeIds.SportsBookBetSetting;
    const sportsBookGroupItems = [...betSettings[sportsBookId].groupItems] as SportsBookBetSettingItem[];
    const copyItemIndex = sportsBookGroupItems.findIndex(
      (item) => item.sportTypeId === Number(sportTypeId) && item.betTypeId === Number(betTypeId),
    );

    const errorName = validateSportsBook(site === site789y, upLineItem, sportsBookGroupItems[copyItemIndex]);
    if (errorName) {
      set({ betSettingErrors: [`${sportTypeId}-${betTypeId}-${errorName}`] });
      return false;
    }

    const newBetSettings = copySportsBookBetSettings(sportsBookGroupItems, Number(sportTypeId), Number(betTypeId));
    set({
      betSettingErrors: [],
      betSettings: {
        ...betSettings,
        [sportsBookId]: {
          ...betSettings[sportsBookId],
          groupItems: newBetSettings ? (newBetSettings as GroupItem[]) : betSettings[sportsBookId].groupItems,
        },
      },
    });
    return true;
  },
  handleCopyRngGames: (copyId, upLineItem) => {
    const { site, betSettings } = get();
    const [sportTypeId, betTypeId] = copyId.split('-');
    const rngGamesId = GroupTypeIds.RNGGamesBetSetting;
    const rngGamesGroupItems = [...betSettings[rngGamesId].groupItems] as RNGGamesBetSettingItem[];
    const copyItemIndex = rngGamesGroupItems.findIndex(
      (item) => item.sportTypeId === Number(sportTypeId) && item.betTypeId === Number(betTypeId),
    );

    const errorName = validateRNGGames(site === site789y, upLineItem, rngGamesGroupItems[copyItemIndex]);
    if (errorName) {
      set({ betSettingErrors: [`${sportTypeId}-${betTypeId}-${errorName}`] });
      return false;
    }

    const newBetSettings = copyRNGGamesBetSettings(rngGamesGroupItems, Number(sportTypeId), Number(betTypeId));
    set({
      betSettingErrors: [],
      betSettings: {
        ...betSettings,
        [rngGamesId]: {
          ...betSettings[rngGamesId],
          groupItems: newBetSettings ? (newBetSettings as GroupItem[]) : betSettings[rngGamesId].groupItems,
        },
      },
    });
    return true;
  },
  validateSportsBookBetSetting: () => {
    const currentItems = get().betSettings[GroupTypeIds.SportsBookBetSetting]?.groupItems as SportsBookBetSettingItem[];
    const upLineItems = get().upLineBetSettings[GroupTypeIds.SportsBookBetSetting]
      ?.groupItems as SportsBookBetSettingItem[];
    if (!currentItems || !upLineItems) return true;
    const is789y = get().site === site789y;

    let [i, errorId] = [0, ''];
    for (i; i < currentItems.length; i += 1) {
      const currentItem = currentItems[i];
      const { sportTypeId, betTypeId } = currentItem;

      errorId = validateSportsBook(is789y, upLineItems[i], currentItem);
      errorId = errorId !== '' ? `${sportTypeId}-${betTypeId}-${errorId}` : errorId;
      if (errorId) break;
    }

    if (errorId) {
      set({ betSettingErrors: [errorId, ...get().betSettingErrors] });
    }

    return errorId === '';
  },
  validateRNGGamesBetSetting: () => {
    const currentItems = get().betSettings[GroupTypeIds.RNGGamesBetSetting]?.groupItems as RNGGamesBetSettingItem[];
    const upLineItems = get().upLineBetSettings[GroupTypeIds.RNGGamesBetSetting]
      ?.groupItems as RNGGamesBetSettingItem[];
    if (!currentItems || !upLineItems) return true;
    const is789y = get().site === site789y;

    let [i, errorId] = [0, ''];
    for (i; i < currentItems.length; i += 1) {
      const currentItem = currentItems[i];
      const { sportTypeId, betTypeId } = currentItem;

      errorId = validateRNGGames(is789y, upLineItems[i], currentItem);
      errorId = errorId !== '' ? `${sportTypeId}-${betTypeId}-${errorId}` : errorId;
      if (errorId) break;
    }

    if (errorId) {
      set({ betSettingErrors: [errorId, ...get().betSettingErrors] });
    }

    return errorId === '';
  },
  validateOtherGamesBetSetting: () => {
    const currentItems = get().betSettings[GroupTypeIds.OtherGamesBetSetting]?.groupItems as OtherGamesBetSettingItem[];
    const upLineItems = get().upLineBetSettings[GroupTypeIds.OtherGamesBetSetting]
      ?.groupItems as OtherGamesBetSettingItem[];
    if (!currentItems || !upLineItems) return true;
    const is789y = get().site === site789y;

    let [i, errorId] = [0, ''];
    for (i; i < currentItems.length; i += 1) {
      const currentItem = currentItems[i];
      const { sportTypeId, betTypeId } = currentItem;

      errorId = validateOtherGames(is789y, upLineItems[i], currentItem);
      errorId = errorId !== '' ? `${sportTypeId}-${betTypeId}-${errorId}` : errorId;
      if (errorId) break;
    }

    if (errorId) {
      set({ betSettingErrors: [errorId, ...get().betSettingErrors] });
    }

    return errorId === '';
  },
});

export default createBetSettingSlice;
