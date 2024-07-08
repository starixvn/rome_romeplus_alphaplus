import { PositionTakingIds } from '@/constants/product';
import { AgentPTConfigItem, AgentPTDict, AgentPTSubGroupConfigAndPTSetting, GroupItem } from '@/types/new-account';
import { PTTemplate } from '@/types/position-taking';

import { BasePTHandler, IPTHandler, PTInitData } from './basePTHandler';

export default class AgentPTHandler extends BasePTHandler implements IPTHandler<{ [key: number]: [] }> {
  private initSubGroupPTValues(
    productGroupId: number,
    ptGroupItemBySubGroupId: { [key: number | string]: GroupItem[] },
  ): AgentPTSubGroupConfigAndPTSetting[] {
    const ptSubGroup = Object.keys(ptGroupItemBySubGroupId).map((subGroupId) => {
      const groupItemsBySubGroupId = ptGroupItemBySubGroupId[`+${Number(subGroupId)}`] as AgentPTConfigItem[];
      const minA2 = Math.min(...groupItemsBySubGroupId.map((item) => item.a2));
      const maxN2 = Math.max(...groupItemsBySubGroupId.map((item) => item.n2));
      const { subGroupName } = groupItemsBySubGroupId[0];

      if (maxN2 > minA2) {
        const minPTItem = groupItemsBySubGroupId.find((item) => item.n2 === maxN2);
        const maxPTItem = groupItemsBySubGroupId.find((item) => item.a2 === minA2);
        if (minPTItem && maxPTItem) {
          this.setViolateError(productGroupId, minPTItem, minPTItem.n2, maxPTItem, maxPTItem.a2);
        }
      }

      return {
        minA2,
        maxN2,
        subGroupId: Number(subGroupId),
        subGroupName,
        a1: minA2,
      };
    });

    return ptSubGroup;
  }

  private initGroupPTValues() {
    const agentPT = Object.keys(PositionTakingIds).reduce((acc: AgentPTDict, cur) => {
      const ptGroupItems = this.productGroups[Number(cur)]?.groupTypes.find(
        (groupType) => groupType.id === PositionTakingIds[Number(cur)],
      );

      if (ptGroupItems) {
        const ptGroupItemBySubGroupId = this.groupPTBySubGroupId(ptGroupItems);
        const ptSubGroup = this.initSubGroupPTValues(Number(cur), ptGroupItemBySubGroupId);
        acc[Number(cur)] = ptSubGroup;
      }

      return acc;
    }, {});

    return agentPT;
  }

  private updateIsValidTemplateProp(agentPT: AgentPTDict) {
    this.ptTemplates = this.ptTemplates.map((template) => {
      const isValidTemplate = template.settings.every((ptTemplate) => {
        const pt = agentPT[ptTemplate.groupId]?.find((x) => x.subGroupId === ptTemplate.subGroupId);
        if (pt && pt.minA2 >= ptTemplate.pt && ptTemplate.pt >= pt.maxN2) {
          return true;
        }

        return false;
      });

      return { ...template, isValid: isValidTemplate };
    });
  }

  private updateDefaultPTValue(defaultPTTemplate: PTTemplate, agentPT: AgentPTDict) {
    let agentPTWithDefaultTemplateValues = {};
    Object.entries(agentPT).forEach((item) => {
      const [key, value] = item;
      const newItems = value.map((group) => {
        const defaultSetting = defaultPTTemplate.settings.find((setting) => setting.subGroupId === group.subGroupId);
        if (!defaultSetting) return group;
        return {
          ...group,
          a1: defaultSetting.pt,
        };
      });

      agentPTWithDefaultTemplateValues = {
        ...agentPTWithDefaultTemplateValues,
        [key]: newItems,
      };
    });

    return agentPTWithDefaultTemplateValues;
  }

  initPTAndTemplateValues(): PTInitData<{ [key: number]: [] }> {
    const agentPTValue = this.initGroupPTValues();
    this.updateIsValidTemplateProp(agentPTValue);
    const defaultPTTemplate = this.getValidDefaultPTTemplate();
    const agentPT = defaultPTTemplate ? this.updateDefaultPTValue(defaultPTTemplate, agentPTValue) : agentPTValue;

    return {
      violateMinMaxPTErr: this.violateMinMaxPTError ?? [],
      ptSetting: agentPT as { [key: number]: [] },
      ptConfig: agentPT as { [key: number]: [] },
      selectedPTTemplate: defaultPTTemplate?.name ?? '',
      ptTemplates: this.ptTemplates,
    };
  }
}
