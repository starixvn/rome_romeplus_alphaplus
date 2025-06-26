export interface Promotion {
  promotionId: number;
  promotionTitle: string;
  promotionType: string;
  fromDate: Date | null;
  toDate: Date | null;
  sportsPercentage: number;
  virtualSportsPercentage: number;
  gamesPercentage: number;
  liveCasinoPercentage: number;
  sportsMaxPayout: number;
  virtualSportsMaxPayout: number;
  gamesMaxPayout: number;
  liveCasinoMaxPayout: number;
  provider: string[];
  status: string;
  lastUpdated: string;
}

export interface PromotionParams {
  type: string;
  status: string;
}
