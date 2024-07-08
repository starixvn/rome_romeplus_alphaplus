export interface GeneralInfo {
  userName: string;
  uplineCustId?: number;
  uplineRoleId: number;
  uplineUserName: string;
  newUserName: string;
  password: string;
  firstName: string;
  lastName: string;
  phone: string;
  mobilePhone: string;
  uplineMaxCredit: number;
  maxCredit?: number;
  memberMaxCredit?: number;
  uplineMemberMaxCredit: number;
  fax: string;
  disableAutoMPT: boolean;
  group: string;
  transferOption: string;
  isAutomateTransfer: boolean;
  site: string;
  siteLabel: string;
  templateName: string;
  currency: number;

  commissions: {
    discount: number;
    name: string;
  }[];
}

export type ProductGroupDict = { [key: number]: ProductGroup };
export type GroupTypeDict = { [key: number]: GroupType };

export interface ProductGroup {
  id: number;
  name: string;
  groupTypes: GroupType[];
}

export interface GroupType {
  id: number;
  name: string;
  displayType: number;
  groupItems: GroupItem[];
}

export interface GroupItem {
  $type: number;
  sportTypeId: number;
  sportTypeName: string;
  betTypeId: number;
  betTypeName: string;
  displayName: string;
  subGroupId: number;
  subGroupName: string;
  isLive: boolean;
}

export interface CommissionItem extends GroupItem {
  discount: number;
  discountMember: number;
  step: number;
}

export interface SportsBookBetSettingItem extends GroupItem {
  minBet: number;
  maxBet: number;
  maxPerMatch: number;
  maxPayoutPerMatch: number;
  upline: string;
}

export interface RNGGamesBetSettingItem extends GroupItem {
  minBet: number;
  maxBet: number;
  maxPerMatch: number;
  maxPerBall: number;
  subType: number;
  upline: string;
}

export interface OtherGamesBetSettingItem extends GroupItem {
  minBet: number;
  maxBet: number;
  maxPerMatch: number;
}

export interface ParlayMaxPayoutItem extends GroupItem {
  singleMatchParlay: number;
  isAllowUnlimitSingleMatchParlay: boolean;
  combo2: number;
  isAllowUnlimitCombo2: boolean;
  combo35: number;
  isAllowUnlimitCombo35: boolean;
  comboOver6: number;
  isAllowUnlimitComboOver6: boolean;
  upline: ParlayMaxPayoutItem;
}

export interface SubGroup {
  subGroupId: number;
  subGroupName: string;
}

export interface SuperPTConfigItem extends GroupItem {
  s4: number;
  m4: number;
  n4: number;
}

export type SuperPTSubGroupConfig = {
  minS4: number;
  minM4: number;
  maxN4: number;
  maxN4SubtractM4: number;
};

export type SuperPTSetting = {
  s3: number;
  uS3: number;
  iS3: boolean;
  m3: number;
  n3: number;
};

export interface MasterPTConfigItem extends GroupItem {
  a4: number;
  m3: number;
  n3: number;
}

export type MasterPTSubGroupConfig = {
  minA4: number;
  minM3: number;
  maxN3: number;
  maxN3SubtractA4: number;
};

export type MasterPTSetting = {
  m2: number;
  uM2: number;
  iM2: boolean;
  a2: number;
  n2: number;
};

export interface AgentPTConfigItem extends GroupItem {
  a2: number;
  n2: number;
}

export type AgentPTSubGroupConfig = {
  minA2: number;
  maxN2: number;
};

export type AgentPTSetting = {
  a1: number;
};

export type SuperPTDict = {
  [key: number]: SuperPTSubGroupConfigAndPTSetting[];
};

export type MasterPTDict = {
  [key: number]: MasterPTSubGroupConfigAndPTSetting[];
};

export type AgentPTDict = {
  [key: number]: AgentPTSubGroupConfigAndPTSetting[];
};

export type SuperPTSubGroupConfigAndPTSetting = SuperPTSubGroupConfig & SuperPTSetting & SubGroup;

export type MasterPTSubGroupConfigAndPTSetting = MasterPTSubGroupConfig & MasterPTSetting & SubGroup;

export type AgentPTSubGroupConfigAndPTSetting = AgentPTSubGroupConfig & AgentPTSetting & SubGroup;

export type ProductGroupUI = {
  id: number;
  groupTypes: UIGroupType[] | [];
};

export type UIGroupType = {
  id: number;
  subGroups: UISubGroup[];
};

export type UISubGroup = {
  id: number;
  attributes: UIAttribute[];
};

export type UIAttribute = {
  key: string;
  value: string;
};
