import { GroupItem } from '@/types/new-account';

export interface IBetSettingValidator<T> {
  validate(prop?: keyof T): string;
}

export type BetSettingProps<T extends GroupItem> = {
  groupTypeId: number;
  groupItems: T[];
  is789y: boolean;
};

export const enum Operator {
  GreaterEqual = '>=',
  LessEqual = '<=',
}

export type SportsBookValue = {
  minBet: number;
  maxBet: number;
  maxPerMatch: number;
  maxPayoutPerMatch: number | null;
};

export type RNGGamesValue = {
  minBet: number;
  maxBet: number;
  maxPerBall: number | null;
  maxPerMatch: number;
};

export type OtherGamesValue = {
  minBet: number;
  maxBet: number;
  maxPerMatch: number;
};

export type BetSettingProp = {
  name: string;
  value: number;
};

export type BetSettingConfigProp = {
  operator: Operator;
  value: number;
};

export type BetSettingValidateOptions = {
  current: BetSettingProp;
  config: BetSettingConfigProp;
  max?: BetSettingProp;
  min?: BetSettingProp;
};
