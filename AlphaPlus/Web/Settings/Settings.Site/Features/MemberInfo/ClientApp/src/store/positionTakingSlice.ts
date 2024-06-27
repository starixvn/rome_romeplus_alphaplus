import { StateCreator } from 'zustand';

import { UserLevel } from '@/constants';
import {
    AgentPTSetting, MasterPTSetting, ProductGroupDict, SubGroup, SuperPTSetting
} from '@/types/new-account';
import { PTTemplate } from '@/types/position-taking';

import AgentPTHandler from './PositionTakingHandler/agentPTHandler';
import PTHandlerContext from './PositionTakingHandler/basePTHandler';
import MasterPTHandler from './PositionTakingHandler/masterPTHandler';
import {
    handleAgentSelectDefaultTemplate, handleAgentSelectTemplate, handleMasterSelectTemplate,
    handlerMasterSelectDefaultTemplate, handleSuperSelectDefaultTemplate, handleSuperSelectTemplate
} from './PositionTakingHandler/ptTemplateHandler';
import SuperPTHandler from './PositionTakingHandler/superPTHandler';

function getPTHandler(userLevel: UserLevel, productGroups: ProductGroupDict, ptTemplates: PTTemplate[]) {
  switch (userLevel) {
    case UserLevel.Super:
      return new SuperPTHandler(productGroups, ptTemplates);
    case UserLevel.Master:
      return new MasterPTHandler(productGroups, ptTemplates);
    case UserLevel.Agent:
      return new AgentPTHandler(productGroups, ptTemplates);
  }
}

export type PositionTakingSlice = {
  userLevel: number;
  ptConfig: { [key: number]: [] };
  ptSetting: { [key: number]: [] };
  ptTemplates: PTTemplate[];
  selectedPTTemplate: string;
  isSelectingPTTemplate: boolean;
  setIsSelectingPTTemplate: (isSelectingPTTemplate: boolean) => void;
  initPositionTakingState: (userLevel: number, productGroups: ProductGroupDict, ptTemplates: PTTemplate[]) => string[];
  updatePTSetting: (
    productGroupId: number,
    subGroupId: number,
    value: Partial<SuperPTSetting | MasterPTSetting | AgentPTSetting>,
  ) => void;
  handleSelectPTTemplate: (templateName: string) => void;
};

const createPositionTakingSlice: StateCreator<PositionTakingSlice, [], [], PositionTakingSlice> = (set, get) => ({
  userLevel: 0,
  ptConfig: {},
  ptSetting: {},
  ptTemplates: [],
  selectedPTTemplate: '',
  isSelectingPTTemplate: false,
  setIsSelectingPTTemplate: (isSelectingPTTemplate) => set({ isSelectingPTTemplate }),
  initPositionTakingState: (userLevel, productGroups, ptTemplates) => {
    const handler = getPTHandler(userLevel, productGroups, ptTemplates);
    if (handler) {
      const ptHandler = new PTHandlerContext(handler);
      const pt = ptHandler.handleInitPTAndTemplateValues();

      set({ userLevel, ...pt });

      if (pt.violateMinMaxPTErr.length > 0) {
        return pt.violateMinMaxPTErr;
      }
    }

    return [];
  },
  updatePTSetting: (productGroupId, subGroupId, value) =>
    set((state) => {
      const newSuperSettings = [...(state.ptSetting[productGroupId] as SubGroup[])].map((item) => {
        if (item.subGroupId === subGroupId) {
          return {
            ...item,
            ...value,
          };
        }

        return item;
      });

      return {
        ptSetting: {
          ...state.ptSetting,
          [productGroupId]: newSuperSettings as [],
        },
      };
    }),
  handleSelectPTTemplate(templateName) {
    const { userLevel, ptTemplates, ptSetting, updatePTSetting } = get();
    const selectedTemplate = ptTemplates.find((template) => template.name === templateName);
    set({ selectedPTTemplate: selectedTemplate?.name ?? '' });
    if (!selectedTemplate) {
      userLevel === UserLevel.Super && set({ ptSetting: handleSuperSelectDefaultTemplate(ptSetting) });
      userLevel === UserLevel.Master && set({ ptSetting: handlerMasterSelectDefaultTemplate(ptSetting) });
      userLevel === UserLevel.Agent && set({ ptSetting: handleAgentSelectDefaultTemplate(ptSetting) });
      return;
    }

    userLevel === UserLevel.Super && handleSuperSelectTemplate(selectedTemplate, ptSetting, updatePTSetting);
    userLevel === UserLevel.Master && handleMasterSelectTemplate(selectedTemplate, ptSetting, updatePTSetting);
    userLevel === UserLevel.Agent && handleAgentSelectTemplate(selectedTemplate, ptSetting, updatePTSetting);
  },
});

export default createPositionTakingSlice;
