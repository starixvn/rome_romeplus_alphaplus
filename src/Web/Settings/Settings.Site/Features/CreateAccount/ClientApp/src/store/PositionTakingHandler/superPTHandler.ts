import { PositionTakingIds } from '@/constants/product';
import { getPTSettingByChangeS3, maxS3, minS3, minUS3 } from '@/features/NewAccount/PositionTaking/superPTRule';
import { GroupItem, SuperPTConfigItem, SuperPTDict, SuperPTSubGroupConfigAndPTSetting } from '@/types/new-account';
import { PTTemplate } from '@/types/position-taking';

import { BasePTHandler, IPTHandler, PTInitData } from './basePTHandler';

export default class SuperPTHandler extends BasePTHandler implements IPTHandler<{ [key: number]: [] }> {
  private initSubGroupPTValues(
    productGroupId: number,
    ptGroupItemBySubGroupId: { [key: number | string]: GroupItem[] },
  ): SuperPTSubGroupConfigAndPTSetting[] {
    const ptSubGroup = Object.keys(ptGroupItemBySubGroupId).map((subGroupId) => {
      const groupItemsBySubGroupId = ptGroupItemBySubGroupId[`+${Number(subGroupId)}`] as SuperPTConfigItem[];
      const minS4 = Math.min(...groupItemsBySubGroupId.map((item) => item.s4));
      const minM4 = Math.min(...groupItemsBySubGroupId.map((item) => item.m4));
      const maxN4 = Math.max(...groupItemsBySubGroupId.map((item) => item.n4));
      const maxN4SubtractM4 = maxN4 - minM4;
      const { subGroupName, tooltipMessage } = groupItemsBySubGroupId[0];

      if (maxN4 > minS4) {
        const minPTItem = groupItemsBySubGroupId.find((item) => item.n4 === maxN4);
        const maxPTItem = groupItemsBySubGroupId.find((item) => item.s4 === minS4);
        if (minPTItem && maxPTItem) {
          this.setViolateError(productGroupId, minPTItem, minPTItem.n4, maxPTItem, maxPTItem.m4);
        }
      }

      return {
        minS4,
        minM4,
        maxN4,
        maxN4SubtractM4,
        subGroupId: Number(subGroupId),
        subGroupName,
        tooltipMessage,
        s3: minS4,
        uS3: minUS3(maxN4SubtractM4),
        iS3: false,
        m3: 0,
        n3: 0,
      };
    });

    return ptSubGroup;
  }

  private initGroupPTValues() {
    const superPT = Object.keys(PositionTakingIds).reduce((acc: SuperPTDict, cur) => {
      const ptGroupItems = this.productGroups[Number(cur)]?.groupTypes.find(
        (groupType) => groupType.id === PositionTakingIds[Number(cur)],
      );

      if (ptGroupItems) {
        const ptGroup = this.groupPTBySubGroupId(ptGroupItems);
        const ptSubGroup = this.initSubGroupPTValues(Number(cur), ptGroup);
        acc[Number(cur)] = ptSubGroup;
      }

      return acc;
    }, {});

    return superPT;
  }

  private updateIsValidTemplateProp(superPT: SuperPTDict) {
    this.ptTemplates = this.ptTemplates.map((template) => {
      const isValidTemplate = template.settings.every((ptTemplate) => {
        const pt = superPT[ptTemplate.groupId]?.find((x) => x.subGroupId === ptTemplate.subGroupId);
        if (pt && ptTemplate.pt <= maxS3(pt.minS4) && ptTemplate.pt >= minS3(pt.maxN4SubtractM4)) {
          return true;
        }

        return false;
      });

      return { ...template, isValid: isValidTemplate };
    });
  }

  private updateDefaultPTValue(defaultPTTemplate: PTTemplate, superPT: SuperPTDict): SuperPTDict {
    let superPTWithDefaultTemplateValues = {};
    Object.entries(superPT).forEach((item) => {
      const [key, value] = item;
      const newPTItems = value.map((group) => {
        const defaultSetting = defaultPTTemplate.settings.find((setting) => setting.subGroupId === group.subGroupId);
        if (!defaultSetting) return group;

        return {
          ...group,
          iS3: false,
          ...getPTSettingByChangeS3(group.maxN4, group.minM4, group.minS4, group.m3, group.n3, defaultSetting.pt),
        };
      });

      superPTWithDefaultTemplateValues = {
        ...superPTWithDefaultTemplateValues,
        [key]: newPTItems,
      };
    });

    return superPTWithDefaultTemplateValues;
  }

  initPTAndTemplateValues(): PTInitData<{ [key: number]: [] }> {
    const superPTValue = this.initGroupPTValues();
    this.updateIsValidTemplateProp(superPTValue);
    const defaultPTTemplate = this.getValidDefaultPTTemplate();
    const superPT = defaultPTTemplate ? this.updateDefaultPTValue(defaultPTTemplate, superPTValue) : superPTValue;

    return {
      violateMinMaxPTErr: this.violateMinMaxPTError ?? [],
      ptSetting: superPT as { [key: number]: [] },
      ptConfig: superPT as { [key: number]: [] },
      selectedPTTemplate: defaultPTTemplate?.name ?? '',
      ptTemplates: this.ptTemplates,
    };
  }
}
