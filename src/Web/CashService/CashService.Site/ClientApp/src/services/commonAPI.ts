import { api } from '../config/axios';

export const baseAPI = {
  getCountryCodes: async (): Promise<string[]> => {
    try {
      const response = await api.get<string[]>('../Payments/GetCountryCodes', { method: 'GET' });
      return response.data;
    } catch (error) {
      console.error('Error fetching mobile prefixes:', error);
      return [];
    }
  },
};
