import axios from 'axios';
import { PaymentProvider } from './interfaces/PaymentProvider';
import { PaymentProviderFeeGroup } from './interfaces/PaymentProviderFeeGroup';
import { ResultModel } from './interfaces/ResultModel';

// Declare the cashServiceAPIUrl property on the Window interface
declare global {
  interface Window {
    cashServiceAPIUrl: string;
  }
}

// Create an axios instance with default config
const api = axios.create({
  baseURL: "http://localhost:5252/api",
  headers: {
    'Content-Type': 'application/json'
  },
  withCredentials: true
});

// API service for payment providers
export const paymentProviderService = {
  // Get all payment providers
  getAllProviders: async (): Promise<PaymentProvider[]> => {
    try {
      const response = await api.get<PaymentProvider[]>('/payment-providers', { method: 'GET' });
      return response.data;
    } catch (error) {
      console.error('Error fetching payment providers:', error);
      return [];
    }
  },
  
  // Get fees for a specific payment provider
  getProviderFees: async (providerId: number): Promise<PaymentProviderFeeGroup[]> => {
    try {
      const response = await api.get<PaymentProviderFeeGroup[]>(`/payment-provider-fees/${providerId}`, { method: 'GET' });
      return response.data;
    } catch (error) {
      console.error(`Error fetching fees for provider ${providerId}:`, error);
      return [];
    }
  },
  
  // Set daily withdrawal count for a customer
  setDailyWithdrawalCount: async (custId: number, count: number): Promise<ResultModel> => {
    try {
      const response = await api.post(`/set-daily-withdrawal-count?custId=${custId}&count=${count}`);
      return response.data;
    } catch (error) {
      console.error('Error setting daily withdrawal count:', error);
      throw error;
    }
  },
  
  // Get settings for a specific payment provider
  getProviderSettings: async (providerId: number, method: number, status: number, transactionType: number): Promise<any> => {
    try {
      const params = new URLSearchParams();
      params.append('providerId', providerId.toString());
      params.append('method', method.toString() );
      params.append('status', status.toString());
      params.append('transactionType', transactionType.toString());
      
      const response = await api.get(`/payment-provider-settings?${params.toString()}`, { method: 'GET' });
      return response.data;
    } catch (error) {
      console.error(`Error fetching settings for provider ${providerId}:`, error);
      throw error;
    }
  },
  
  // Update settings for a specific payment provider
  updateProviderSettings: async (settings: any): Promise<ResultModel> => {
    try {
      const params = new URLSearchParams();
      params.append('settings', JSON.stringify(settings));
      
      const response = await api.put(`/update-payment-provider-settings?${params.toString()}`);
      return response.data;
    } catch (error) {
      console.error(`Error updating settings`, error);
      throw error;
    }
  },
};

export default api; 