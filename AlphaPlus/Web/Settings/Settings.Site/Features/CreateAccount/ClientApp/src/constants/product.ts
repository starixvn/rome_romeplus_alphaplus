export const MaxNumberInputLength = 18; // 99,999,999,999,999

export const enum ProductGroupIds {
  SportsBook = 1,
  RNGGames = 2,
  LiveRNGGames = 3,
  OthersGames = 4,
}

export const GroupTypeIds = {
  SportsBookCommission: 150,
  SportsBookBetSetting: 151,
  ParlayMaxPayout: 152,
  PositionTaking: 153,
  RNGGamesBetSetting: 154,
  Commission: 156,
  OtherGamesBetSetting: 157,
};

export const BetSettingGroupTypeIds: { [key: number]: number } = {
  [ProductGroupIds.SportsBook]: GroupTypeIds.SportsBookBetSetting,
  [ProductGroupIds.RNGGames]: GroupTypeIds.RNGGamesBetSetting,
  [ProductGroupIds.OthersGames]: GroupTypeIds.OtherGamesBetSetting,
};

export const ParlayMaxPayoutTypeIds: { [key: number]: number } = {
  [ProductGroupIds.SportsBook]: GroupTypeIds.ParlayMaxPayout,
};

export const CommissionTypeIds: { [key: number]: number } = {
  [ProductGroupIds.SportsBook]: GroupTypeIds.SportsBookCommission,
  [ProductGroupIds.LiveRNGGames]: GroupTypeIds.Commission,
  [ProductGroupIds.OthersGames]: GroupTypeIds.Commission,
};

export const PositionTakingIds: { [key: number]: number } = {
  [ProductGroupIds.SportsBook]: GroupTypeIds.PositionTaking,
  [ProductGroupIds.RNGGames]: GroupTypeIds.PositionTaking,
  [ProductGroupIds.LiveRNGGames]: GroupTypeIds.PositionTaking,
  [ProductGroupIds.OthersGames]: GroupTypeIds.PositionTaking,
};
