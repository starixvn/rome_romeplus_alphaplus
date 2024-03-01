import { ReactNode } from 'react';
import { StateCreator } from 'zustand';

import { TransferOptions, UserLevel } from '@/constants';
import { GeneralInfo, ProductGroup, ProductGroupDict } from '@/types/new-account';
import { PTTemplate } from '@/types/position-taking';

import createBetSettingSlice, { BetSettingSlice } from './betSettingSlice';
import { initCommissionState } from './CommissionHandler/commissionHandler';
import createCommissionSlice, { CommissionSlice } from './commissionSlice';
import { GeneralInfoSlice } from './generalInfoSlice';
import BaseSettingHandler from './NewAccountHandler/baseSettingHandler';
import { AgentUIHandler, MasterUIHandler, SuperUIHandler } from './NewAccountHandler/baseUIHandler';
import createParlayMaxPayoutSlice, { ParlayMaxPayoutSlice } from './parlayMaxPayoutSlice';
import createPositionTakingSlice, { PositionTakingSlice } from './positionTakingSlice';
import createSportsBookCommissionSlice, { SportsBookCommissionSlice } from './sportsBookCommissionSlice';

const getUIHandler = (userLevel: UserLevel, productGroups: ProductGroupDict) => {
  switch (userLevel) {
    case UserLevel.Super:
      return new SuperUIHandler(productGroups);
    case UserLevel.Master:
      return new MasterUIHandler(productGroups);
    case UserLevel.Agent:
      return new AgentUIHandler(productGroups);
  }
};

export type NewAccountSlice = {
  theme: string;
  popup: ReactNode;
  productGroupsDict: ProductGroupDict;
  submitCount: number;
  setTheme: (value: string) => void;
  showPopup: (popup: ReactNode) => void;
  intiNewAccountState: (generalInfo: GeneralInfo, productGroups: ProductGroup[], ptTemplates: PTTemplate[]) => string[];
  getProductGroupAndUISettings: () => {
    uiBehavior: object;
    generalInfo: GeneralInfo;
    productGroups: ProductGroup[];
    templateName: string;
  };
  increaseSubmitCount: () => void;
};

type NewAccountState = GeneralInfoSlice &
  NewAccountSlice &
  SportsBookCommissionSlice &
  CommissionSlice &
  BetSettingSlice &
  ParlayMaxPayoutSlice &
  PositionTakingSlice;

const createNewAccountSlice: StateCreator<NewAccountState, [], [], NewAccountSlice> = (set, get, api) => ({
  theme: 'light',
  popup: null,
  productGroupsDict: {},
  submitCount: 0,
  setTheme: (value) => set({ theme: value }),
  showPopup: (popup) => set({ popup }),
  intiNewAccountState: (generalInfo, productGroups, ptTemplates) => {
    const productGroupsDict = productGroups.reduce((acc: ProductGroupDict, cur) => {
      acc[cur.id] = {
        ...cur,
      };
      return acc;
    }, {});

    set(() => ({
      generalInfo: {
        ...generalInfo,
        userName: (generalInfo.uplineUserName + generalInfo.newUserName).toUpperCase(),
        uplineMaxCredit: generalInfo.maxCredit ?? 0,
        uplineMemberMaxCredit: generalInfo.memberMaxCredit ?? 0,
        maxCredit: 0,
        group: 'a',
      },
      disableAutoMPT: generalInfo.disableAutoMPT,
      userLevel: generalInfo.uplineRoleId,
      siteLabel: generalInfo.siteLabel.toLocaleLowerCase(),
      isAutomateTransfer: generalInfo.isAutomateTransfer,
      displayTransferCondition: generalInfo.transferOption === TransferOptions.Daily,
      productGroupsDict,
    }));
    const { sportsBookCommissions, commissions } = initCommissionState(productGroupsDict);
    createSportsBookCommissionSlice(set, get, api).initSportsBookCommissionState(sportsBookCommissions);
    createCommissionSlice(set, get, api).initCommissionState(commissions);
    createBetSettingSlice(set, get, api).initBetSettingState(generalInfo.siteLabel, productGroupsDict);
    createParlayMaxPayoutSlice(set, get, api).initParlayMaxPayoutState(productGroupsDict);
    const error = createPositionTakingSlice(set, get, api).initPositionTakingState(
      generalInfo.uplineRoleId,
      productGroupsDict,
      ptTemplates,
    );

    return error;
  },
  increaseSubmitCount: () => set({ submitCount: get().submitCount + 1 }),
  getProductGroupAndUISettings: () => {
    const clearBetSettingError = get().clearBetSettingError;
    clearBetSettingError();
    const {
      userLevel,
      productGroupsDict,
      sportsBookCommissions,
      commissions,
      betSettings,
      parlayMaxPayouts,
      ptSetting,
    } = get();
    const settingHandler = new BaseSettingHandler(JSON.parse(JSON.stringify(productGroupsDict)), {
      userLevel,
      commissions: {
        ...sportsBookCommissions,
        ...commissions,
      },
      betSettings,
      parlayMaxPayouts,
      ptSettings: ptSetting,
    });
    const settings = settingHandler.getProductGroupSettings();

    const uiHandler = getUIHandler(userLevel, JSON.parse(JSON.stringify(productGroupsDict)));
    const uiSettings = uiHandler?.getCommissionUIs(commissions).getPtUIs(ptSetting).getProductGroupUIs();

    return {
      uiBehavior: uiSettings as object,
      generalInfo: get().generalInfo,
      productGroups: Object.values(settings),
      templateName: get().selectedPTTemplate ?? '',
    };
  },
});

export default createNewAccountSlice;
