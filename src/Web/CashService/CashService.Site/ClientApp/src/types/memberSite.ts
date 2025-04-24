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


export interface MemberSiteResponse {
  domains: Domain[];
  generalSettings: Setting[];
  mobile: string;
  lineId: string;
  announcement: string;
  [key: string]: Domain[] | Setting[] | string;
}
