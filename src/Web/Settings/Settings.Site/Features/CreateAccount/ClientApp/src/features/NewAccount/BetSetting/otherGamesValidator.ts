import { OtherGamesBetSettingItem } from '@/types/new-account';

import { IBetSettingValidator, Operator, OtherGamesValue } from './type';
import { getOtherGamesValue, validateBetSettingValue } from './utils';

export default class OtherGamesValidator implements IBetSettingValidator<OtherGamesBetSettingItem> {
  private is789y: boolean;
  private betSettingConfigs: OtherGamesBetSettingItem;
  private betSettings: OtherGamesValue;
  private error: string = '';

  constructor(is789y: boolean, configs: OtherGamesBetSettingItem, settings: OtherGamesBetSettingItem) {
    this.is789y = is789y;
    this.betSettingConfigs = configs;
    this.betSettings = getOtherGamesValue(settings);
  }

  private setError(error: string) {
    if (error !== '' && this.error === '') {
      this.error = error;
    }

    return this;
  }

  private getError() {
    return this.error;
  }

  private isUnlimitedOrPreventBetValue() {
    const { minBet, maxBet, maxPerMatch } = this.betSettings;
    const betSettings = JSON.stringify([minBet, maxBet, maxPerMatch]);
    return (
      betSettings === JSON.stringify([0, 0, 0]) ||
      betSettings === JSON.stringify([0, 0.1, 0.1]) ||
      (minBet == 0 && maxBet <= this.betSettingConfigs.maxBet && maxPerMatch <= this.betSettingConfigs.maxPerMatch) ||
      (minBet == 0 && maxBet == 0 && maxPerMatch <= this.betSettingConfigs.maxPerMatch)
    );
  }

  private validateMinBet(prop?: keyof OtherGamesBetSettingItem) {
    if (this.error || (prop && prop !== 'minBet')) return this;
    const { minBet, maxBet } = this.betSettings;
    const error = validateBetSettingValue(this.is789y, {
      current: { name: 'minBet', value: minBet },
      config: { operator: Operator.LessEqual, value: this.betSettingConfigs.minBet },
      max: { name: 'maxBet', value: maxBet },
    });
    if (error) return this.setError(error);

    return this;
  }

  private validateMaxBet(prop?: keyof OtherGamesBetSettingItem) {
    if (this.error || (prop && prop !== 'maxBet')) return this;
    const { minBet, maxBet, maxPerMatch } = this.betSettings;
    const error = validateBetSettingValue(this.is789y, {
      current: { name: 'maxBet', value: maxBet },
      config: { operator: Operator.GreaterEqual, value: this.betSettingConfigs.maxBet },
      max: { name: 'maxPerMatch', value: maxPerMatch },
      min: { name: 'minBet', value: minBet },
    });
    if (error) return this.setError(error);

    return this;
  }

  private validateMaxPerMatch(prop?: keyof OtherGamesBetSettingItem) {
    if (this.error || (prop && prop !== 'maxPerMatch')) return this;
    const { maxPerMatch, maxBet } = this.betSettings;
    const error = validateBetSettingValue(this.is789y, {
      current: { name: 'maxPerMatch', value: maxPerMatch },
      config: { operator: Operator.GreaterEqual, value: this.betSettingConfigs.maxPerMatch },
      min: { name: 'maxBet', value: maxBet },
    });
    if (error) return this.setError(error);

    return this;
  }

  validate(prop?: keyof OtherGamesBetSettingItem): string {
    this.error = '';
    if (!this.is789y && this.isUnlimitedOrPreventBetValue()) return this.error;

    return this.validateMinBet(prop).validateMaxBet(prop).validateMaxPerMatch(prop).getError();
  }
}
