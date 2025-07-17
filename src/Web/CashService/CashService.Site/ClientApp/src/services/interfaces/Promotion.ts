export interface PromotionDetail {
  promotionId: number;
  promotionTitle: string;
  promotionTypeId: number;
  promotionTypeName: string;
  fromDate: Date | null;
  toDate: Date | null;
  productSettings: ProductSetting[];
  languageSettings: LanguageInfo[];
  enabled: boolean;
}

export interface EditPromotionRequest {  
  promotionId: number;
  enabled: boolean;
  contents: LanguageInfo[];
}

export interface PromotionRequest {
  promotionType: number;
  fromDate: Date | null;
  toDate: Date | null;
  enabled: boolean;
  productSettings: ProductSetting[];
  contents: LanguageInfo[];
}
export interface Language {
  key: string;
  label: string;
  isDefault: boolean;
}
export interface LanguageInfo {
  language: string;
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
  productIds: number[];
}
