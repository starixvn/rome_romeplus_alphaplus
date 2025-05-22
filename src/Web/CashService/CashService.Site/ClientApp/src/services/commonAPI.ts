import { api } from './axios';

export const baseAPI = {
  getCountryCodes: async (): Promise<string[]> => {
    return await api.get<string[]>('../Payments/GetCountryCodes', []);
  },
};
