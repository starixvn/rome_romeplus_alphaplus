export interface Domain {
  id: string;
  url: string;
  name: string;
  status: 'active' | 'blocked';
}

export interface Setting {
  id: string;
  name: string;
  description: string;
  value: boolean;
  category?: string;
}

export interface MessageData {
  mobile?: string;
  lineId?: string;
  announcement?: string;
}

export interface MemberSiteResponse {
  domains: Domain[];
  generalSettings: Setting[];
  countryCode: string;
  countryCodes: string[];
  mobile: string;
  lineId: string;
  announcement: string;
  mobileCode: string;
  mobilePhone: string;
}

export interface ResponseResult {
  code: number;
  message: string;
  isSuccessful: boolean;
  isFailed: boolean;
  isOk: boolean;
  isNotOk: boolean;
}
