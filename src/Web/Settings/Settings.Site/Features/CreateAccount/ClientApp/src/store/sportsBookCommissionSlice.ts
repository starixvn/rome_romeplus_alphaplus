import { StateCreator } from 'zustand';

import { ProductGroupIds } from '@/constants/product';
import { GroupTypeDict } from '@/types/new-account';

export type SportsBookCommissionSlice = {
  sportsBookCommissions: GroupTypeDict;
  initSportsBookCommissionState: (sportsBookCommissions: GroupTypeDict) => void;
  updateSportsBookCommissions: (sportTypeId: number, betTypeId: number, discount: number) => void;
};

const createSportsBookCommissionSlice: StateCreator<SportsBookCommissionSlice, [], [], SportsBookCommissionSlice> = (
  set,
) => ({
  sportsBookCommissions: {},
  commissions: {},
  initSportsBookCommissionState: (sportsBookCommissions) => set({ sportsBookCommissions }),
  updateSportsBookCommissions: (sportTypeId, betTypeId, discount) =>
    set((state) => {
      const newCommissions = state.sportsBookCommissions[ProductGroupIds.SportsBook].groupItems.map((commission) => {
        if (commission.sportTypeId === sportTypeId && commission.betTypeId === betTypeId) {
          return { ...commission, discount };
        }
        return commission;
      });

      return {
        sportsBookCommissions: {
          [ProductGroupIds.SportsBook]: {
            ...state.sportsBookCommissions[ProductGroupIds.SportsBook],
            groupItems: newCommissions,
          },
        },
      };
    }),
});

export default createSportsBookCommissionSlice;
