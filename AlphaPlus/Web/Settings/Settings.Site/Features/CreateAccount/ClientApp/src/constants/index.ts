export interface Params {
  suggestUsername: string;
  uplineId: number;
  uplineRoleId: number;
}

declare global {
  const params: Params;

  interface Window {
    alpha: {
      urls: {
        settingsRootPath: string;
        oldMainRootPath: string;
      };
    };
    bootboxConfirm: any;
    bootboxAlert: any;
    indexManager: any;
  }
}

export const siteMemberInfoUrl = window.top?.alpha?.urls.settingsRootPath;

export const createAccountRootUrl = `${siteMemberInfoUrl}CreateAccount`;

export const customerSettingInfoRootUrl = `${siteMemberInfoUrl}Customer/SportBookInformation`;

export const copyAccountRootUrl = `${siteMemberInfoUrl}CopyAccount`;

export const bootBoxConfirm = window.top?.bootboxConfirm;
export const bootBoxAlert = window.top?.bootboxAlert;

export const UserLevelName: { [key: number]: string } = {
  1: 'Member',
  2: 'Agent',
  3: 'Master',
  4: 'Super',
};

export enum UserLevel {
  Member = 1,
  Agent = 2,
  Master = 3,
  Super = 4,
}

export const TransferOptions = {
  Daily: '00000000',
};

export const site789y = '789y';
export const site3Decimals = '789y,TH7a5';
