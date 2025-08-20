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
  originalEnabled?: boolean;
  contents: LanguageInfo[];
  originalContents?: LanguageInfo[];
}

export interface PromotionRequest {
  promotionType: number;
  statusId?: number;
  fromDate: Date | null;
  toDate: Date | null;
  enabled: boolean;
  productSettings: ProductSetting[];
  contents: LanguageInfo[];
}

export interface PromotionTransactionRequest {
  promotionTypeId: number;
  fromDate: string;
  toDate: string;
}

export interface PromotionTransaction {
  promotionId: number;
  transId: string;
  custId: number;
  username: string;
  nickname: string;
  firstName: string;
  lastName: string;
  amount: number;
  currency: string;
  statusId: number;
  status: string;
  title: string;
  promotionType: number;
  promotionTypeName: string;
  fromDate: string;
  toDate: string;
}

export interface PromotionTransactionResponse {
  promotionTransactions: PromotionTransaction[];
  totalCount: number;
}

export interface Language {
  key: string;
  label: string;
  isDefault: boolean;
}
export interface LanguageInfo {
  language: string;
  title: string;
  bannerName: string | null;
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
  minTO: number;
  maxPayout: number;
  unlimited: boolean;
  productIds: number[];
  productList?: string;
}
