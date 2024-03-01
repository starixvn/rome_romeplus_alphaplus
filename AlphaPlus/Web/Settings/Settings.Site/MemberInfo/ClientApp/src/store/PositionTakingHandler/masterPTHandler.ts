import { PositionTakingIds } from '@/constants/product';
import { getMasterPTByChangeM2, maxM2, minM2, minUM2 } from '@/features/NewAccount/PositionTaking/masterPTRule';
import { GroupItem, MasterPTConfigItem, MasterPTDict, MasterPTSubGroupConfigAndPTSetting } from '@/types/new-account';
import { PTTemplate } from '@/types/position-taking';

import { BasePTHandler, IPTHandler, PTInitData } from './basePTHandler';

export default class MasterPTHandler extends BasePTHandler implements IPTHandler<{ [key: number]: [] }> {
  private initSubGroupPTValues(
    productGroupId: number,
    ptGroupItemBySubGroupId: { [key: number | string]: GroupItem[] },
  ): MasterPTSubGroupConfigAndPTSetting[] {
    const ptSubGroup = Object.keys(ptGroupItemBySubGroupId).map((subGroupId) => {
      const groupItemsBySubGroupId = ptGroupItemBySubGroupId[`+${Number(subGroupId)}`] as MasterPTConfigItem[];
      const minA4 = Math.min(...groupItemsBySubGroupId.map((item) => item.a4));
      const minM3 = Math.min(...groupItemsBySubGroupId.map((item) => item.m3));
      const maxN3 = Math.max(...groupItemsBySubGroupId.map((item) => item.n3));
      const maxN3SubtractA4 = maxN3 - minA4;
      const { subGroupName } = groupItemsBySubGroupId[0];

      if (maxN3 > minM3) {
        const minPTItem = groupItemsBySubGroupId.find((item) => item.n3 === maxN3);
        const maxPTItem = groupItemsBySubGroupId.find((item) => item.m3 === minM3);
        if (minPTItem && maxPTItem) {
          this.setViolateError(productGroupId, minPTItem, minPTItem.n3, maxPTItem, maxPTItem.m3);
        }
      }

      return {
        minA4,
        minM3,
        maxN3,
        maxN3SubtractA4,
        subGroupId: Number(subGroupId),
        subGroupName,
        m2: minM3,
        uM2: minUM2(maxN3SubtractA4),
        iM2: false,
        a2: 0,
        n2: 0,
      };
    });

    return ptSubGroup;
  }

  private initGroupPTValues() {
    const masterPT = Object.keys(PositionTakingIds).reduce((acc: MasterPTDict, cur) => {
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

    return masterPT;
  }

  private updateIsValidTemplateProp(masterPT: MasterPTDict) {
    this.ptTemplates = this.ptTemplates.map((template) => {
      const isValidTemplate = template.settings.every((ptTemplate) => {
        const pt = masterPT[ptTemplate.groupId]?.find((x) => x.subGroupId === ptTemplate.subGroupId);
        if (pt && ptTemplate.pt <= maxM2(pt.minM3) && ptTemplate.pt >= minM2(pt.maxN3SubtractA4)) {
          return true;
        }

        return false;
      });

      return { ...template, isValid: isValidTemplate };
    });
  }

  private updateDefaultPTValue(defaultPTTemplate: PTTemplate, masterPT: MasterPTDict) {
    let masterPTWithDefaultTemplateValues = {};
    Object.entries(masterPT).forEach((item) => {
      const [key, value] = item;
      const newItems = value.map((group) => {
        const defaultSetting = defaultPTTemplate.settings.find((setting) => setting.subGroupId === group.subGroupId);
        if (!defaultSetting) return group;
        return {
          ...group,
          iM2: false,
          ...getMasterPTByChangeM2(group.minA4, group.maxN3, group.minM3, group.a2, group.n2, defaultSetting.pt),
        };
      });

      masterPTWithDefaultTemplateValues = {
        ...masterPTWithDefaultTemplateValues,
        [key]: newItems,
      };
    });

    return masterPTWithDefaultTemplateValues;
  }

  initPTAndTemplateValues(): PTInitData<{ [key: number]: [] }> {
    const masterPTValue = this.initGroupPTValues();
    this.updateIsValidTemplateProp(masterPTValue);
    const defaultPTTemplate = this.getValidDefaultPTTemplate();
    const masterPT = defaultPTTemplate ? this.updateDefaultPTValue(defaultPTTemplate, masterPTValue) : masterPTValue;

    return {
      violateMinMaxPTErr: this.violateMinMaxPTError ?? [],
      ptSetting: masterPT as { [key: number]: [] },
      ptConfig: masterPT as { [key: number]: [] },
      selectedPTTemplate: defaultPTTemplate?.name ?? '',
      ptTemplates: this.ptTemplates,
    };
  }
}
