import SportsBookValidator from '@/features/NewAccount/BetSetting/sportsBookValidator';
import { SportsBookBetSettingItem } from '@/types/new-account';

export const validateSportsBook = (
  is789y: boolean,
  config: SportsBookBetSettingItem,
  setting: SportsBookBetSettingItem,
) => {
  const error = new SportsBookValidator(is789y, config, setting).validate();
  const [errorName] = error.split('-');
  return errorName;
};

export const copySportsBookBetSettings = (
  sportsBookItems: SportsBookBetSettingItem[],
  sportTypeId: number,
  betTypeId: number,
) => {
  const betSettingItemIndex = sportsBookItems.findIndex(
    (item) => item.sportTypeId === sportTypeId && item.betTypeId === betTypeId,
  );
  const betSettingItem = sportsBookItems[betSettingItemIndex];
  return sportsBookItems.map((item, index) => {
    if (betSettingItemIndex < index) {
      return {
        ...item,
        minBet: betSettingItem.minBet,
        maxBet: betSettingItem.maxBet,
        maxPerMatch: betSettingItem.maxPerMatch,
        maxPayoutPerMatch: item.maxPayoutPerMatch === null ? null : betSettingItem.maxPayoutPerMatch,
      };
    }
    return item;
  });
};
