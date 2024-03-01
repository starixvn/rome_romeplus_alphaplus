import { StateCreator } from 'zustand';

import { GroupTypeIds, ParlayMaxPayoutTypeIds } from '@/constants/product';
import { validateParlayMaxPayoutValue } from '@/features/NewAccount/ParlayMaxPayout/ParlayMaxPayoutRowBlock';
import { GroupItem, GroupTypeDict, ParlayMaxPayoutItem, ProductGroupDict } from '@/types/new-account';
import { getNumberOfCommaString } from '@/utils/number-format';

import { UserLevel } from '../constants/index';

const initParlayMaxPayoutsState = (productGroupsDict: ProductGroupDict) => {
  const parlayMaxPayouts = Object.keys(ParlayMaxPayoutTypeIds).reduce((acc: GroupTypeDict, cur) => {
    const parlayMaxPayoutGroup = productGroupsDict[Number(cur)]?.groupTypes.find(
      (groupType) => groupType.id === ParlayMaxPayoutTypeIds[Number(cur)],
    );

    if (parlayMaxPayoutGroup) {
      const parlayGroupItems = parlayMaxPayoutGroup.groupItems as ParlayMaxPayoutItem[];
      const groupItems = parlayGroupItems.map((item) => ({
        ...item,
        combo2: item.maxCombo2,
        combo35: item.maxCombo35,
        comboOver6: item.maxComboOver6,
        singleMatchParlay: item.maxSingleMatchParlay,
      }));

      acc[ParlayMaxPayoutTypeIds[Number(cur)]] = { ...parlayMaxPayoutGroup, groupItems };
    }

    return acc;
  }, {});

  return parlayMaxPayouts;
};

const validateParlayMaxPayout = (userLevel: UserLevel, parlayMaxPayoutItem: ParlayMaxPayoutItem) => {
  const {
    singleMatchParlay,
    maxSingleMatchParlay,
    isAllowUnlimitSingleMatchParlay,
    combo2,
    maxCombo2,
    isAllowUnlimitCombo2,
    combo35,
    maxCombo35,
    isAllowUnlimitCombo35,
    comboOver6,
    maxComboOver6,
    isAllowUnlimitComboOver6,
  } = parlayMaxPayoutItem;
  const singleMatchParlayValue = getNumberOfCommaString(singleMatchParlay);
  const combo2Value = getNumberOfCommaString(combo2);
  const combo35Value = getNumberOfCommaString(combo35);
  const comboOver6Value = getNumberOfCommaString(comboOver6);

  if (
    !validateParlayMaxPayoutValue(
      userLevel,
      isAllowUnlimitSingleMatchParlay,
      singleMatchParlayValue,
      maxSingleMatchParlay,
    )
  )
    return 'singleMatchParlay';
  if (!validateParlayMaxPayoutValue(userLevel, isAllowUnlimitCombo2, combo2Value, maxCombo2)) return 'combo2';
  if (!validateParlayMaxPayoutValue(userLevel, isAllowUnlimitCombo35, combo35Value, maxCombo35)) return 'combo35';
  if (!validateParlayMaxPayoutValue(userLevel, isAllowUnlimitComboOver6, comboOver6Value, maxComboOver6))
    return 'comboOver6';
  return '';
};

export type ParlayMaxPayoutSlice = {
  parlayMaxPayouts: GroupTypeDict;
  parlayMaxPayoutErrors: string[];
  initParlayMaxPayoutState: (productGroups: ProductGroupDict) => void;
  updateParlayMaxPayoutItem: (groupTypeId: number, parlayMaxPayoutGroupItem: GroupItem) => void;
  validateParlayMaxPayout: (userLevel: UserLevel) => boolean;
};

const createParlayMaxPayoutSlice: StateCreator<ParlayMaxPayoutSlice, [], [], ParlayMaxPayoutSlice> = (set, get) => ({
  parlayMaxPayouts: {},
  parlayMaxPayoutErrors: [],
  initParlayMaxPayoutState: (productGroups) => {
    set({ parlayMaxPayoutErrors: [], parlayMaxPayouts: initParlayMaxPayoutsState(productGroups) });
  },
  updateParlayMaxPayoutItem: (groupTypeId, parlayMaxPayoutGroupItem) =>
    set((state) => {
      const newParlaySettings = [...state.parlayMaxPayouts[groupTypeId].groupItems].map((item) => {
        if (
          item.sportTypeId === parlayMaxPayoutGroupItem.sportTypeId &&
          item.betTypeId === parlayMaxPayoutGroupItem.betTypeId
        ) {
          return parlayMaxPayoutGroupItem;
        }

        return item;
      });

      return {
        parlayMaxPayoutErrors: [],
        parlayMaxPayouts: {
          ...state.parlayMaxPayouts,
          [groupTypeId]: {
            ...state.parlayMaxPayouts[groupTypeId],
            groupItems: newParlaySettings,
          },
        },
      };
    }),
  validateParlayMaxPayout: (userLevel: UserLevel) => {
    const parlayMaxPayouts = get().parlayMaxPayouts[GroupTypeIds.ParlayMaxPayout].groupItems as ParlayMaxPayoutItem[];

    let [i, errorId] = [0, ''];
    for (i; i < parlayMaxPayouts.length; i += 1) {
      const parlayMaxPayoutItem = parlayMaxPayouts[i];
      const { sportTypeId, betTypeId } = parlayMaxPayoutItem;

      errorId = validateParlayMaxPayout(userLevel, parlayMaxPayoutItem);
      errorId = errorId !== '' ? `${sportTypeId}-${betTypeId}-${errorId}` : errorId;
      if (errorId) break;
    }

    set({ parlayMaxPayoutErrors: [errorId] });

    return errorId === '';
  },
});

export default createParlayMaxPayoutSlice;
