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
  provider: number[];
  status: string;
  lastUpdated: string;
}

export interface PromotionDetail {
  promotionId: number;
  promotionTitle: string;
  promotionType: string;
  fromDate: Date | null;
  toDate: Date | null;
  products: Product[];
  enabled: boolean;
  lastUpdated: string;
}

export interface PromotionRequest {
  promotionTitle: string;
  promotionType: string;
  fromDate: Date | null;
  toDate: Date | null;
  enabled: boolean;
  products: Product[];
}

export interface PromotionParams {
  type: string;
  status: string;
}

export interface Product {
  productId: number;
  productName: string;
  isSelected: boolean;
  children: Product[];
  productSettings: ProductSetting;
}

export interface ProductSetting {
  percentage: number;
  minTurnover: number;
  maxPayout: number;
  unlimited: boolean;
}
