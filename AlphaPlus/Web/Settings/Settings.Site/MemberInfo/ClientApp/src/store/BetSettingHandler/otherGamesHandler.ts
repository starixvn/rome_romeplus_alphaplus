import OtherGamesValidator from '@/features/NewAccount/BetSetting/otherGamesValidator';
import { OtherGamesBetSettingItem } from '@/types/new-account';

export const validateOtherGames = (
  is789y: boolean,
  config: OtherGamesBetSettingItem,
  setting: OtherGamesBetSettingItem,
) => {
  const error = new OtherGamesValidator(is789y, config, setting).validate();
  const [errorName] = error.split('-');
  return errorName;
};
