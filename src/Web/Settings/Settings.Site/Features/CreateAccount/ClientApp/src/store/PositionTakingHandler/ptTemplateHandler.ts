import { getMasterPTByChangeM2 } from '@/features/NewAccount/PositionTaking/masterPTRule';
import { getPTSettingByChangeS3 } from '@/features/NewAccount/PositionTaking/superPTRule';
import {
    AgentPTDict, AgentPTSetting, MasterPTDict, MasterPTSetting, SuperPTDict, SuperPTSetting
} from '@/types/new-account';
import { PTTemplate } from '@/types/position-taking';

export const handleSuperSelectDefaultTemplate = (ptSettings: SuperPTDict) => {
  let newPositionTakings = {};
  Object.entries(ptSettings).forEach((ptItem) => {
    const [key, value] = ptItem;
    newPositionTakings = {
      ...newPositionTakings,
      [Number(key)]: value.map((subGroupPt) => ({
        ...subGroupPt,
        s3: subGroupPt.minM4,
        iS3: false,
        m3: 0,
        n3: 0,
      })),
    };
  });

  return newPositionTakings;
};

export const handlerMasterSelectDefaultTemplate = (ptSettings: MasterPTDict) => {
  let newPositionTakings = {};
  Object.entries(ptSettings).forEach((ptItem) => {
    const [key, value] = ptItem;
    newPositionTakings = {
      ...newPositionTakings,
      [Number(key)]: value.map((subGroupPt) => ({
        ...subGroupPt,
        m2: subGroupPt.minM3,
        iM2: false,
        a2: 0,
        n2: 0,
      })),
    };
  });

  return newPositionTakings;
};

export const handleAgentSelectDefaultTemplate = (ptSettings: AgentPTDict) => {
  let newPositionTakings = {};
  Object.entries(ptSettings).forEach((ptItem) => {
    const [key, value] = ptItem;
    newPositionTakings = {
      ...newPositionTakings,
      [Number(key)]: value.map((subGroupPt) => ({
        ...subGroupPt,
        a1: subGroupPt.minA2,
      })),
    };
  });

  return newPositionTakings;
};

export const handleSuperSelectTemplate = (
  selected: PTTemplate,
  positionTakings: SuperPTDict,
  updatePTValue: (productGroupId: number, subGroupId: number, value: Partial<SuperPTSetting>) => void,
) => {
  selected.settings.forEach((setting) => {
    const pt = positionTakings[setting.groupId]?.find((x) => x.subGroupId === setting.subGroupId);

    if (pt && pt.minS4 >= setting.pt) {
      updatePTValue(setting.groupId, setting.subGroupId, {
        ...getPTSettingByChangeS3(pt.maxN4, pt.minM4, pt.minS4, pt.m3, pt.n3, setting.pt),
        iS3: false,
      });
    }
  });
};

export const handleMasterSelectTemplate = (
  selected: PTTemplate,
  agentPTAndUpLineConfigs: MasterPTDict,
  updatePTValue: (productGroupId: number, subGroupId: number, value: Partial<MasterPTSetting>) => void,
) => {
  selected.settings.forEach((setting) => {
    const pt = agentPTAndUpLineConfigs[setting.groupId]?.find((x) => x.subGroupId === setting.subGroupId);

    if (pt && pt.minM3 >= setting.pt) {
      updatePTValue(setting.groupId, setting.subGroupId, {
        ...getMasterPTByChangeM2(pt.minA4, pt.maxN3, pt.minM3, pt.a2, pt.n2, setting.pt),
        iM2: false,
      });
    }
  });
};

export const handleAgentSelectTemplate = (
  selected: PTTemplate,
  agentPTAndUpLineConfigs: AgentPTDict,
  updatePTValue: (productGroupId: number, subGroupId: number, value: Partial<AgentPTSetting>) => void,
) => {
  selected.settings.forEach((setting) => {
    const pt = agentPTAndUpLineConfigs[setting.groupId]?.find((x) => x.subGroupId === setting.subGroupId);

    if (pt && pt.minA2 >= setting.pt) {
      updatePTValue(setting.groupId, setting.subGroupId, { a1: setting.pt });
    }
  });
};
