import { SportsBookBetSettingItem } from '@/types/new-account';

import { IBetSettingValidator, Operator, SportsBookValue } from './type';
import { getSportsBookValue, validateBetSettingValue } from './utils';

const ESports = '43-1';
const MuayThai = '44-3';
export const SpecialI7SportsTypes: ReadonlyArray<string> = [ESports, MuayThai];

export default class SportsBookValidator implements IBetSettingValidator<SportsBookBetSettingItem> {
  private is789y: boolean;
  private betSettingConfigs: SportsBookBetSettingItem;
  private betSettings: SportsBookValue;
  private error: string = '';

  constructor(is789y: boolean, configs: SportsBookBetSettingItem, settings: SportsBookBetSettingItem) {
    this.is789y = is789y;
    this.betSettingConfigs = configs;
    this.betSettings = getSportsBookValue(settings);
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

  private isSpecialI7SportType() {
    if (!this.is789y) return false;
    const { sportTypeId, betTypeId } = this.betSettingConfigs;
    return SpecialI7SportsTypes.includes(`${sportTypeId}-${betTypeId}`);
  }

  private isUnlimitedOrPreventBetValue() {
    const { minBet, maxBet, maxPerMatch, maxPayoutPerMatch } = this.betSettings;
    if (maxPayoutPerMatch === null) {
      const betSettings = JSON.stringify([minBet, maxBet, maxPerMatch]);
      return (
        betSettings === JSON.stringify([0, 0, 0]) ||
        betSettings === JSON.stringify([0, 0.1, 0.1]) ||
        (minBet === 0 &&
          maxBet <= this.betSettingConfigs.maxBet &&
          maxPerMatch <= this.betSettingConfigs.maxPerMatch &&
          maxBet <= maxPerMatch) ||
        (minBet === 0 && maxBet === 0 && maxPerMatch <= this.betSettingConfigs.maxPerMatch)
      );
    }

    const betSettings = JSON.stringify([minBet, maxBet, maxPerMatch, maxPayoutPerMatch]);
    return (
      betSettings === JSON.stringify([0, 0, 0, 0]) ||
      betSettings === JSON.stringify([0, 0.1, 0.1, 0.1]) ||
      (minBet === 0 &&
        maxPayoutPerMatch !== undefined &&
        this.betSettingConfigs.maxPayoutPerMatch !== undefined &&
        maxBet <= this.betSettingConfigs.maxBet &&
        maxPerMatch <= this.betSettingConfigs.maxPerMatch &&
        maxPayoutPerMatch <= this.betSettingConfigs.maxPayoutPerMatch &&
        maxBet <= maxPerMatch &&
        maxPerMatch <= maxPayoutPerMatch) ||
      (minBet === 0 &&
        maxBet === 0 &&
        maxPayoutPerMatch !== undefined &&
        this.betSettingConfigs.maxPayoutPerMatch !== undefined &&
        maxPerMatch <= this.betSettingConfigs.maxPerMatch &&
        maxPayoutPerMatch <= this.betSettingConfigs.maxPayoutPerMatch &&
        maxPerMatch <= maxPayoutPerMatch) ||
      (minBet === 0 &&
        maxBet === 0 &&
        maxPerMatch === 0 &&
        maxPayoutPerMatch !== undefined &&
        this.betSettingConfigs.maxPayoutPerMatch !== undefined &&
        maxPayoutPerMatch <= this.betSettingConfigs.maxPayoutPerMatch)
    );
  }

  private validateMinBet(prop?: keyof SportsBookBetSettingItem) {
    if (this.error || (prop && prop !== 'minBet')) return this;
    const error = validateBetSettingValue(this.isSpecialI7SportType(), {
      current: { name: 'minBet', value: this.betSettings.minBet },
      config: { operator: Operator.LessEqual, value: this.betSettingConfigs.minBet },
      max: { name: 'maxBet', value: this.betSettings.maxBet },
    });
    if (error) return this.setError(error);
    return this;
  }

  private validateMaxBet(prop?: keyof SportsBookBetSettingItem) {
    if (this.error || (prop && prop !== 'maxBet')) return this;
    const error = validateBetSettingValue(this.isSpecialI7SportType(), {
      current: { name: 'maxBet', value: this.betSettings.maxBet },
      config: { operator: Operator.GreaterEqual, value: this.betSettingConfigs.maxBet },
      max: { name: 'maxPerMatch', value: this.betSettings.maxPerMatch },
      min: { name: 'minBet', value: this.betSettings.minBet },
    });
    if (error) return this.setError(error);

    return this;
  }

  private validateMaxPerMatch(prop?: keyof SportsBookBetSettingItem) {
    if (this.error || (prop && prop !== 'maxPerMatch')) return this;
    const { maxBet, maxPayoutPerMatch } = this.betSettings;
    const error = validateBetSettingValue(this.isSpecialI7SportType(), {
      current: { name: 'maxPerMatch', value: this.betSettings.maxPerMatch },
      config: { operator: Operator.GreaterEqual, value: this.betSettingConfigs.maxPerMatch },
      max: maxPayoutPerMatch !== null ? { name: 'maxPayoutPerMatch', value: maxPayoutPerMatch } : undefined,
      min: { name: 'maxBet', value: maxBet },
    });
    if (error) return this.setError(error);

    return this;
  }

  private validateMaxPayoutPerMatch(prop?: keyof SportsBookBetSettingItem) {
    if (this.error || (prop && prop !== 'maxPayoutPerMatch')) return this;
    const { maxPayoutPerMatch, maxPerMatch } = this.betSettings;
    if (maxPayoutPerMatch === null) return this;
    const error = validateBetSettingValue(this.isSpecialI7SportType(), {
      current: { name: 'maxPayoutPerMatch', value: maxPayoutPerMatch },
      config: { operator: Operator.GreaterEqual, value: this.betSettingConfigs.maxPayoutPerMatch },
      min: { name: 'maxPerMatch', value: maxPerMatch },
    });
    if (error) return this.setError(error);
    return this;
  }

  validate(prop?: keyof SportsBookBetSettingItem) {
    this.error = '';
    if (!this.isSpecialI7SportType() && this.isUnlimitedOrPreventBetValue()) return this.error;

    return this.validateMinBet(prop)
      .validateMaxBet(prop)
      .validateMaxPerMatch(prop)
      .validateMaxPayoutPerMatch(prop)
      .getError();
  }
}
