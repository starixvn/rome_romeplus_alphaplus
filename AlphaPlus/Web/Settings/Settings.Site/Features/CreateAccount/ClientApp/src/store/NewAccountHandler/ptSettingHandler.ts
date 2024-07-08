import { UserLevel } from '@/constants';
import {
  AgentPTSubGroupConfigAndPTSetting,
  GroupItem,
  MasterPTSubGroupConfigAndPTSetting,
  SuperPTSubGroupConfigAndPTSetting,
} from '@/types/new-account';

import { resetName } from './baseSettingHandler';

const getBasePTValues = (item: GroupItem) => {
  return {
    ...resetName(item),
    $type: item.$type,
    sportTypeId: item.sportTypeId,
    betTypeId: item.betTypeId,
    subGroupId: item.subGroupId,
    isLive: item.isLive,
  };
};

interface BasePTSettingHandler {
  handle(ptConfigs: GroupItem[], ptSettings: []): GroupItem[];
}

class SuperPTSettingHandler implements BasePTSettingHandler {
  handle(ptConfigs: GroupItem[], ptSettings: SuperPTSubGroupConfigAndPTSetting[]): GroupItem[] {
    return ptConfigs.map((config) => {
      const ptSetting = ptSettings.find((setting) => setting.subGroupId === config.subGroupId);
      return {
        ...getBasePTValues(config),
        s3: ptSetting?.s3,
        uS3: ptSetting?.uS3,
        iS3: ptSetting?.iS3,
        m3: ptSetting?.m3,
        n3: ptSetting?.n3,
      };
    });
  }
}

class MasterPTSettingHandler implements BasePTSettingHandler {
  handle(ptConfigs: GroupItem[], ptSettings: MasterPTSubGroupConfigAndPTSetting[]): GroupItem[] {
    return ptConfigs.map((config) => {
      const ptSetting = ptSettings.find((setting) => setting.subGroupId === config.subGroupId);
      return {
        ...getBasePTValues(config),
        m2: ptSetting?.m2,
        uM2: ptSetting?.uM2,
        iM2: ptSetting?.iM2,
        a2: ptSetting?.a2,
        n2: ptSetting?.n2,
      };
    });
  }
}

class AgentPTSettingHandler implements BasePTSettingHandler {
  handle(ptConfigs: GroupItem[], ptSettings: AgentPTSubGroupConfigAndPTSetting[]): GroupItem[] {
    return ptConfigs.map((config) => {
      const ptSetting = ptSettings.find((setting) => setting.subGroupId === config.subGroupId);
      return {
        ...getBasePTValues(config),
        a1: ptSetting?.a1,
      };
    });
  }
}

export default function getPTSettingHandler(userLevel: UserLevel) {
  switch (userLevel) {
    case UserLevel.Super:
      return new SuperPTSettingHandler();
    case UserLevel.Master:
      return new MasterPTSettingHandler();
    case UserLevel.Agent:
      return new AgentPTSettingHandler();
  }
}
