import axios from 'axios';
import { PaymentProvider } from './interfaces/PaymentProvider';
import { PaymentProviderFee } from './interfaces/PaymentProviderFee';

// Declare the cashServiceAPIUrl property on the Window interface
declare global {
  interface Window {
    cashServiceAPIUrl: string;
  }
}

// Create an axios instance with default config
const api = axios.create({
  baseURL: window.cashServiceAPIUrl,
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
  getProviderFees: async (providerId: number): Promise<PaymentProviderFee[]> => {
    try {
      const response = await api.get<PaymentProviderFee[]>(`/payment-provider-fees/${providerId}`, { method: 'GET' });
      return response.data;
    } catch (error) {
      console.error(`Error fetching fees for provider ${providerId}:`, error);
      return [];
    }
  }
};

export default api; 