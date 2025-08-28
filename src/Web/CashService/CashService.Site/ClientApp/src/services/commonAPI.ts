import { api } from './axios';
import { Bank } from './interfaces/Bank';

export const baseAPI = {
  getCountryCodes: async (): Promise<any> => {
    return await api.get<string[]>('../Payments/GetCountryCodes');
  },
  getSetupCheckList: async (): Promise<any> => {
    return await api.get<any>('../Common/GetSetupCheckList');
  },

  getProviderBanks: async (providerId: number): Promise<any> => {
    return await api.get<Bank[]>(`../Common/GetProviderBanks?providerId=${providerId}`);
  },
};
