import { GroupTypeIds } from '@/constants/product';
import {
  AgentPTSetting,
  AgentPTSubGroupConfigAndPTSetting,
  CommissionItem,
  GroupTypeDict,
  MasterPTSetting,
  MasterPTSubGroupConfigAndPTSetting,
  ProductGroupDict,
  ProductGroupUI,
  SuperPTSetting,
  SuperPTSubGroupConfigAndPTSetting,
} from '@/types/new-account';

const GroupLogs: { [key: number]: number[] } = {
  1: [GroupTypeIds.PositionTaking],
  2: [GroupTypeIds.PositionTaking],
  3: [GroupTypeIds.Commission, GroupTypeIds.PositionTaking],
  4: [GroupTypeIds.Commission, GroupTypeIds.PositionTaking],
};

interface UIHandler {
  getPtUIs(pt: { [key: number]: [] }): void;
}

export class BaseUIHandler {
  protected productGroupUIs: ProductGroupUI[] = [];

  constructor(productGroups: ProductGroupDict) {
    this.getBaseUIs(productGroups);
  }

  private getBaseUIs(productGroups: ProductGroupDict) {
    Object.values(productGroups).forEach((productGroup) => {
      if (GroupLogs[productGroup.id].length > 0) {
        this.productGroupUIs.push({
          id: productGroup.id,
          groupTypes: GroupLogs[productGroup.id].map((groupTypeId) => {
            const groupType = productGroup.groupTypes.find((item) => item.id === groupTypeId);
            if (groupType) {
              const { id } = groupType;
              return {
                id,
                subGroups: [],
              };
            }
          }) as [],
        });
      }
    });

    return this;
  }

  protected getProductIndex(productId: number) {
    return this.productGroupUIs.findIndex((product) => product.id === productId);
  }

  protected getGroupTypeIndex(productGroupId: number, groupTypeId: number) {
    return GroupLogs[productGroupId].findIndex((groupType) => groupType === groupTypeId);
  }

  getCommissionUIs(commissions: GroupTypeDict) {
    Object.keys(commissions).forEach((key) => {
      const productId = Number(key);
      const comItems = commissions[productId]?.groupItems;
      if (comItems && comItems.length > 0 && comItems[0].subGroupId !== null) {
        const productIndex = this.getProductIndex(productId);
        const groupTypeIndex = this.getGroupTypeIndex(productId, commissions[productId].id);

        if (groupTypeIndex >= 0) {
          let subGroupId: null | number = null;
          comItems.forEach((com) => {
            if (com.subGroupId !== subGroupId) {
              this.productGroupUIs[productIndex].groupTypes[groupTypeIndex].subGroups.push({
                id: com.subGroupId,
                attributes: [
                  {
                    key: 'discount',
                    value: (com as CommissionItem).discount.toString(),
                  },
                ],
              });
              subGroupId = com.subGroupId;
            }
          });
        }
      }
    });

    return this;
  }

  getProductGroupUIs() {
    return {
      productGroups: this.productGroupUIs,
    };
  }
}

const SuperPTBehavior: ReadonlyArray<keyof SuperPTSetting> = ['s3', 'iS3', 'uS3', 'm3', 'n3'];
const MasterPTBehavior: ReadonlyArray<keyof MasterPTSetting> = ['m2', 'iM2', 'uM2', 'a2', 'n2'];
const AgentPTBehavior: ReadonlyArray<keyof AgentPTSetting> = ['a1'];

export class SuperUIHandler extends BaseUIHandler implements UIHandler {
  getPtUIs(superPT: { [key: number]: SuperPTSubGroupConfigAndPTSetting[] }) {
    Object.keys(superPT).forEach((key) => {
      const productId = Number(key);
      const productIndex = this.getProductIndex(productId);
      const groupTypeIndex = this.getGroupTypeIndex(productId, GroupTypeIds.PositionTaking);

      if (groupTypeIndex >= 0) {
        superPT[Number(key)].forEach((pt) => {
          this.productGroupUIs[productIndex].groupTypes[groupTypeIndex].subGroups.push({
            id: pt.subGroupId,
            attributes: SuperPTBehavior.map((item) => ({
              key: item,
              value: item === 'iS3' ? (+pt[item]).toString() : (pt[item] / 100).toString(),
            })),
          });
        });
      }
    });

    return this;
  }
}

export class MasterUIHandler extends BaseUIHandler {
  getPtUIs(masterPT: { [key: number]: MasterPTSubGroupConfigAndPTSetting[] }) {
    Object.keys(masterPT).forEach((key) => {
      const productId = Number(key);
      const productIndex = this.getProductIndex(productId);
      const groupTypeIndex = this.getGroupTypeIndex(productId, GroupTypeIds.PositionTaking);

      if (groupTypeIndex >= 0) {
        masterPT[Number(key)].forEach((pt) => {
          this.productGroupUIs[productIndex].groupTypes[groupTypeIndex].subGroups.push({
            id: pt.subGroupId,
            attributes: MasterPTBehavior.map((item) => ({
              key: item,
              value: item === 'iM2' ? (+pt[item]).toString() : (pt[item] / 100).toString(),
            })),
          });
        });
      }
    });

    return this;
  }
}

export class AgentUIHandler extends BaseUIHandler {
  getPtUIs(agentPT: { [key: number]: AgentPTSubGroupConfigAndPTSetting[] }) {
    Object.keys(agentPT).forEach((key) => {
      const productId = Number(key);
      const productIndex = this.getProductIndex(productId);
      const groupTypeIndex = this.getGroupTypeIndex(productId, GroupTypeIds.PositionTaking);

      if (groupTypeIndex >= 0) {
        agentPT[Number(key)].forEach((pt) => {
          this.productGroupUIs[productIndex].groupTypes[groupTypeIndex].subGroups.push({
            id: pt.subGroupId,
            attributes: AgentPTBehavior.map((item) => ({
              key: item,
              value: (pt[item] / 100).toString(),
            })),
          });
        });
      }
    });

    return this;
  }
}
