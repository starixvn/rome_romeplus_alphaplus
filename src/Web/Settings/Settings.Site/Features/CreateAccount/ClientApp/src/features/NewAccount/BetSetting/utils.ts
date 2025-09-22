import { TFunction } from 'i18next';

import {
    OtherGamesBetSettingItem, RNGGamesBetSettingItem, SportsBookBetSettingItem
} from '@/types/new-account';
import { formatThousandNumber } from '@/utils/number-format';

import {
    BetSettingConfigProp, BetSettingProp, BetSettingValidateOptions, Operator, OtherGamesValue,
    RNGGamesValue, SportsBookValue
} from './type';

export function isUnlimitedBetSetting(value: number) {
  return value === 0;
}

export function getSportsBookValue(sportsBookItem: SportsBookBetSettingItem): SportsBookValue {
  return {
    minBet: sportsBookItem.minBet,
    maxBet: sportsBookItem.maxBet,
    maxPerMatch: sportsBookItem.maxPerMatch,
    maxPayoutPerMatch: sportsBookItem.maxPayoutPerMatch ?? null,
  };
}

export function getRNGGamesValue(rngGamesItem: RNGGamesBetSettingItem): RNGGamesValue {
  return {
    minBet: rngGamesItem.minBet,
    maxBet: rngGamesItem.maxBet,
    maxPerBall: rngGamesItem.maxPerBall ?? null,
    maxPerMatch: rngGamesItem.maxPerMatch,
  };
}

export function getOtherGamesValue(otherGamesItem: OtherGamesBetSettingItem): OtherGamesValue {
  return {
    minBet: otherGamesItem.minBet,
    maxBet: otherGamesItem.maxBet,
    maxPerMatch: otherGamesItem.maxPerMatch,
  };
}

export function getGreaterErrorMessage(prop: string, value: number | string) {
  const valueKey = typeof value === 'string' ? `-${value}` : `#${value}`;
  const errorMessage = `${prop}-mustBeGreaterThanOrEqualTo${valueKey}`;
  const mappedErrorMessage = mapSpecificErrorMessage(errorMessage, typeof value);
  return mappedErrorMessage;
} 

export function getLessErrorMessage(prop: string, value: number | string) {
  const valueKey = typeof value === 'string' ? `-${value}` : `#${value}`;
  const errorMessage = `${prop}-mustBeLessThanOrEqualTo${valueKey}`;
  const mappedErrorMessage = mapSpecificErrorMessage(errorMessage, typeof value);
  return mappedErrorMessage;
}

function mapSpecificErrorMessage(messageKey: string, valueType: string) {
  if(messageKey === 'maxPerMatch-mustBeLessThanOrEqualTo-maxPayoutPerMatch') {
    return 'maxPerMatch-mustBeLessThanOrEqualTo-maxWinPerMatch';
  }
  if(messageKey === 'maxPayoutPerMatch-mustBeGreaterThanOrEqualTo-maxPerMatch') {
    return 'maxWinPerMatch-mustBeGreaterThanOrEqualTo-maxPerMatch';
  }
  if(valueType === 'number' && messageKey.includes('maxPayoutPerMatch-mustBeLessThanOrEqualTo')) {
    return messageKey.replace('maxPayoutPerMatch-mustBeLessThanOrEqualTo', 'maxWinPerMatch-mustBeLessThanOrEqualTo');
  }
  return messageKey;
}


export function isViolateBetSettingConfig(current: BetSettingProp, config: BetSettingConfigProp) {
  if (config.operator === Operator.LessEqual && !isUnlimitedBetSetting(config.value) && current.value < config.value)
    return getGreaterErrorMessage(current.name, config.value);
  if (config.operator === Operator.GreaterEqual && !isUnlimitedBetSetting(config.value) && current.value > config.value)
    return getLessErrorMessage(current.name, config.value);
  return '';
}

export function validateBetSettingValue(
  isCheckLessThanOne: boolean,
  betSettingValidateOptions: BetSettingValidateOptions,
) {
  const { current, config, max, min } = betSettingValidateOptions;
  if (current.value === undefined) return `${current.name}#RequiredField`;
  if (isCheckLessThanOne && current.value < 1 && config.value < 1) return getGreaterErrorMessage(current.name, 1);
  const isViolateConfigError = isViolateBetSettingConfig(current, config);
  if (isViolateConfigError) return isViolateConfigError;
  if (max && current.value > max.value) return getLessErrorMessage(current.name, max.name);
  if (min && current.value < min.value) return getGreaterErrorMessage(current.name, min.name);
  return '';
}

export function getErrorMessageTranslate(messageKey: string, t: TFunction) {
  const messageArr = messageKey.split('#');
  if (messageArr.includes('RequiredField')) return t('RequiredField');
  
  var errKey = messageArr[0];
  var terr = t(errKey);
  if(terr.indexOf("{{value1}}") > -1) {
    var valueKey = messageArr[1];
    var tvalue = Number.isNaN(Number(valueKey)) ? t(valueKey) : formatThousandNumber(valueKey);
    return terr.replace("{{value1}}", tvalue);
  }

  return messageArr
    .map((errKey) => (Number.isNaN(Number(errKey)) ? t(errKey) : formatThousandNumber(errKey)))
    .join(' ');
}
