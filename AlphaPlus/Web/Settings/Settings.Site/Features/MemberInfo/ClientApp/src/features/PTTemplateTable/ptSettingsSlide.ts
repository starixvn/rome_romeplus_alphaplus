import { StateCreator } from 'zustand';

export interface PTSetting {
  groupId: number;
  groupName: string;
  subGroups: PTSettingDetail[];
}

export interface PTSettingDetail {
  subGroupId: number;
  subGroupName: string;
  min: number;
  max: number;
}

export type PTSettingsSlide = {
  ptSettings: PTSetting[];
  updatePTSettings: (data: PTSetting[]) => void;
  isSpecialSite: boolean;
  is3DecimalSite: boolean;
  updateIsSpecialSite: (value: boolean) => void;
  updateIs3DecimalSite: (value: boolean) => void;
};

export const createPTSettingsSlice: StateCreator<PTSettingsSlide, [], [], PTSettingsSlide> = (set) => ({
  ptSettings: [],
  updatePTSettings: (data) => set(() => ({ ptSettings: data })),
  isSpecialSite: false,
  is3DecimalSite: false,
  updateIsSpecialSite: (value) => set(() => ({ isSpecialSite: value })),
  updateIs3DecimalSite: (value) => set(() => ({ is3DecimalSite: value })),
});
