import axios from 'axios';

// Create an axios instance with default config
const api = axios.create({
  baseURL: 'http://localhost:5252/api',
  headers: {
    'Content-Type': 'application/json'
  },
  withCredentials: true
});

// Payment Provider interface
export interface PaymentProvider {
  id: number;
  name: string;
  fee: number;
  status: number;
  lastUpdatedStr: string;
}

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
  }
};

export default api; 