import { api } from '../config/axios';
import { MobilePrefix } from './interfaces/MobilePrefix';

export const baseAPI = {
  // Get mobile prefixes
  getMobilePrefixes: async (): Promise<MobilePrefix[]> => {
    try {
      const response = await api.get<MobilePrefix[]>('../Payments/GetMobilePrefixes', { method: 'GET' });
      return response.data;
    } catch (error) {
      console.error('Error fetching mobile prefixes:', error);
      return [];
    }
  },
}; 