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

export interface ProductDropdown {
  productCategoryId: number;
  productCategoryName: string;
  products: Product[];
}

export interface Product {
  productCategoryId: number;
  productId: number;
  productName: string;
}

export interface ProductSetting extends ProductDropdown {
  percentage: number;
  minTurnover: number;
  maxPayout: number;
  unlimited: boolean;
}
