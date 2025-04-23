import { ProductGroupIds } from '@/constants/product';
import { GroupItem, ProductGroupDict } from '@/types/new-account';

import { GroupType } from '../../types/new-account/index';
import { PTTemplate } from '../../types/position-taking/index';

export type PTError = {
  isLive: boolean;
  message: string
}

export type PTInitData<T> = {
  violateMinMaxPTErr: PTError[];
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
  protected violateMinMaxPTError: PTError[];

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
      const minPTError = `${
        isSportBookProduct ? `${minPTItem?.betTypeName}` : `${minPTItem?.sportTypeName}`
      } - MinPT: ${minPTValue / 100}`;
      const maxPTError = `${
        isSportBookProduct ? `${maxPTItem?.betTypeName}` : `${maxPTItem?.sportTypeName}`
      } - MaxPT: ${maxPTValue / 100}`;

      this.violateMinMaxPTError = [{isLive: minPTItem.isLive, message: minPTError}, {isLive: maxPTItem.isLive, message: maxPTError}];
    }
  }

  protected getValidDefaultPTTemplate() {
    return this.ptTemplates.find((template) => template.isDefault && template.isValid);
  }
}
