import { StateCreator } from 'zustand';

import { GroupTypeDict } from '@/types/new-account';

export type CommissionSlice = {
  commissions: GroupTypeDict;
  initCommissionState: (commissions: GroupTypeDict) => void;
  updateCommissions: (productGroupId: number, subGroupId: number, commission: number) => void;
};

const createCommissionSlice: StateCreator<CommissionSlice, [], [], CommissionSlice> = (set) => ({
  sportsBookCommissions: {},
  commissions: {},
  initCommissionState: (commissions) => set({ commissions }),
  updateCommissions: (productGroupId, subGroupId, discount) =>
    set((state) => {
      const newCommissionSettings = [...state.commissions[productGroupId].groupItems].map((item) => {
        if (item.subGroupId === subGroupId) {
          return {
            ...item,
            discount,
          };
        }
        return item;
      });

      return {
        commissions: {
          ...state.commissions,
          [productGroupId]: {
            ...state.commissions[productGroupId],
            groupItems: newCommissionSettings,
          },
        },
      };
    }),
});

export default createCommissionSlice;
