import { ProductGroupIds } from '@/constants/product';
import { GroupItem, ProductGroupDict } from '@/types/new-account';

import { GroupType } from '../../types/new-account/index';
import { PTTemplate } from '../../types/position-taking/index';

export type PTInitData<T> = {
  violateMinMaxPTErr: string[];
  ptSetting: T;
  ptConfig: T;
  selectedPTTemplate: string;
  ptTemplates: PTTemplate[];
};

export interface IPTHandler<T> {
  initPTAndTemplateValues(): PTInitData<T>;
}

export default class PTHandlerContext {
  private handler: IPTHandler<{ [key: number]: [] }>;

  constructor(handler: IPTHandler<{ [key: number]: [] }>) {
    this.handler = handler;
  }

  setHandler(handler: IPTHandler<{ [key: number]: [] }>) {
    this.handler = handler;
  }

  handleInitPTAndTemplateValues() {
    return this.handler.initPTAndTemplateValues();
  }
}

export class BasePTHandler {
  protected productGroups: ProductGroupDict;
  protected ptTemplates: PTTemplate[];
  protected violateMinMaxPTError: string[];

  constructor(productGroups: ProductGroupDict, ptTemplate: PTTemplate[]) {
    this.productGroups = productGroups;
    this.ptTemplates = ptTemplate;
    this.violateMinMaxPTError = [];
  }

  protected groupPTBySubGroupId(ptGroupItems: GroupType): { [key: number]: GroupItem[] } {
    const ptGroups = ptGroupItems.groupItems.reduce((acc: { [key: number | string]: GroupItem[] }, cur) => {
      if (cur.subGroupId >= 0) {
        const subGroupKey = `+${cur.subGroupId}`;
        acc[subGroupKey] = acc[subGroupKey] ?? [];
        acc[subGroupKey].push(cur);
      }
      return acc;
    }, {});

    return ptGroups;
  }

  protected setViolateError(
    productGroupId: number,
    minPTItem: GroupItem,
    minPTValue: number,
    maxPTItem: GroupItem,
    maxPTValue: number,
  ) {
    if (this.violateMinMaxPTError.length === 0) {
      const isSportBookProduct = productGroupId === ProductGroupIds.SportsBook;
      const minPTError = `${minPTItem?.sportTypeName}${
        isSportBookProduct ? ` - ${minPTItem?.betTypeName}` : ''
      } - MinPT: ${minPTValue / 100}`;
      const maxPTError = `${maxPTItem?.sportTypeName}${
        isSportBookProduct ? ` - ${maxPTItem?.betTypeName}` : ''
      } - MaxPT: ${maxPTValue / 100}`;

      this.violateMinMaxPTError = [minPTError, maxPTError];
    }
  }

  protected getValidDefaultPTTemplate() {
    return this.ptTemplates.find((template) => template.isDefault && template.isValid);
  }
}
