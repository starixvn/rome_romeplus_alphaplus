import { api } from './axios';

export const baseAPI = {
  getCountryCodes: async (): Promise<any> => {
    return await api.get<string[]>('../Payments/GetCountryCodes');
  },
};
