import RNGGamesValidator from '@/features/NewAccount/BetSetting/rngGamesValidator';
import { RNGGamesBetSettingItem } from '@/types/new-account';

export const validateRNGGames = (is789y: boolean, config: RNGGamesBetSettingItem, setting: RNGGamesBetSettingItem) => {
  const error = new RNGGamesValidator(is789y, config, setting).validate();
  const [errorName] = error.split('-');
  return errorName;
};

export const copyRNGGamesBetSettings = (
  rngGamesItems: RNGGamesBetSettingItem[],
  sportTypeId: number,
  betTypeId: number,
) => {
  const betSettingItemIndex = rngGamesItems.findIndex(
    (item) => item.sportTypeId === Number(sportTypeId) && item.betTypeId === Number(betTypeId),
  );
  const betSettingItem = rngGamesItems[betSettingItemIndex];
  return rngGamesItems.map((item, index) => {
    if (betSettingItemIndex < index && betSettingItem.subType === item.subType) {
      return {
        ...item,
        minBet: betSettingItem.minBet,
        maxBet: betSettingItem.maxBet,
        maxPerMatch: betSettingItem.maxPerMatch,
        maxPerBall: item.maxPerBall === null ? null : betSettingItem.maxPerBall,
      };
    }
    return item;
  });
};
