import { GroupTypeIds } from '@/constants/product';
import {
  GroupItem,
  GroupType,
  OtherGamesBetSettingItem,
  RNGGamesBetSettingItem,
  SportsBookBetSettingItem,
} from '@/types/new-account';
import { getNumberOfCommaString } from '@/utils/number-format';

import { resetName } from './baseSettingHandler';

interface BaseBetSettingHandler {
  handle(groupType: GroupType): GroupItem[];
}

class SportsBookHandler implements BaseBetSettingHandler {
  handle(groupType: GroupType): GroupItem[] {
    return (groupType.groupItems as SportsBookBetSettingItem[]).map((item) => ({
      ...item,
      ...resetName(item),
      minBet: getNumberOfCommaString(item.minBet),
      maxBet: getNumberOfCommaString(item.maxBet),
      maxPerMatch: getNumberOfCommaString(item.maxPerMatch),
      maxPayoutPerMatch: item.maxPayoutPerMatch === null ? null : getNumberOfCommaString(item.maxPayoutPerMatch),
    }));
  }
}

class RNGGamesHandler implements BaseBetSettingHandler {
  handle(groupType: GroupType): GroupItem[] {
    return (groupType.groupItems as RNGGamesBetSettingItem[]).map((item) => ({
      ...item,
      ...resetName(item),
      minBet: getNumberOfCommaString(item.minBet),
      maxBet: getNumberOfCommaString(item.maxBet),
      maxPerMatch: getNumberOfCommaString(item.maxPerMatch),
      maxPerBall: item.maxPerBall === null ? null : getNumberOfCommaString(item.maxPerBall),
    }));
  }
}

class OtherGamesHandler implements BaseBetSettingHandler {
  handle(groupType: GroupType): GroupItem[] {
    return (groupType.groupItems as OtherGamesBetSettingItem[]).map((item) => ({
      ...item,
      ...resetName(item),
      minBet: getNumberOfCommaString(item.minBet),
      maxBet: getNumberOfCommaString(item.maxBet),
      maxPerMatch: getNumberOfCommaString(item.maxPerMatch),
    }));
  }
}

export default function getBetSettingHandler(groupType: GroupType) {
  switch (groupType.id) {
    case GroupTypeIds.SportsBookBetSetting:
      return new SportsBookHandler();
    case GroupTypeIds.RNGGamesBetSetting:
      return new RNGGamesHandler();
    case GroupTypeIds.OtherGamesBetSetting:
      return new OtherGamesHandler();
  }
}
