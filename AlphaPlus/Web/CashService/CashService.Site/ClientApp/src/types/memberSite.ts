export interface Domain {
  id: string;
  url: string;
  qrCode?: string;
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

export interface MemberSiteData {
  domains: Domain[];
  settings: Setting[];
  mobile: string;
  lineId: string;
  announcement: string;
  [key: string]: Domain[] | Setting[] | string;
}

export interface ApiData {
  domains: Array<{
    url: string;
    qrCode?: string;
  }>;
  mobile: string;
  lineId: string;
  announcement: string;
  [key: string]: any;
} 