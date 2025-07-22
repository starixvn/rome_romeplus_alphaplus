import { api } from './axios';

export const baseAPI = {
  getCountryCodes: async (): Promise<any> => {
    return await api.get<string[]>('../Payments/GetCountryCodes');
  },
  getSetupCheckList: async (): Promise<any> => {
    return await api.get<any>('../Common/GetSetupCheckList');
  },
};
