export interface PromotionDetail {
  promotionId: number;
  promotionTitle: string;
  promotionTypeId: number;
  promotionType: string;
  statusId: number;
  status: string;
  fromDate: Date | null;
  toDate: Date | null;
  productSettings: ProductSetting[];
  promotionInfo: LanguageInfo[];
  enabled: boolean;
  lastUpdated: string;
}

export interface PromotionRequest {
  promotionType: number;
  fromDate: Date | null;
  toDate: Date | null;
  enabled: boolean;
  products: ProductSetting[];
  promotionInfo: LanguageInfo[];
}

export interface LanguageInfo {
  lang: string;
  title: string;
  banner: string | null;
  content: string;
}

export interface PromotionParams {
  type: number;
  status: number;
}

export interface Product {
  productId: number;
  productName: string;
  isSelected: boolean;
  childProducts: Product[];
}

export interface ProductSetting extends Product {
  percentage: number;
  minTurnover: number;
  maxPayout: number;
  unlimited: boolean;
}
