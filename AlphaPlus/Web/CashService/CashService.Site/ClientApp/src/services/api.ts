import axios from 'axios';

// Create an axios instance with default config
const api = axios.create({
  baseURL: 'http://localhost:5252/api',
  headers: {
    'Content-Type': 'application/json',
    'Access-Control-Allow-Origin': '*',
    'Access-Control-Allow-Headers': '*',
    'Access-Control-Allow-Credentials': 'true',
    'Access-Control-Allow-Methods': 'GET, PUT, POST, DELETE, OPTIONS'
  },
  withCredentials: true
});

// Payment Provider interface
export interface PaymentProvider {
  id: number;
  name: string;
  fee: number;
  status: number;
  lastUpdated: string;
}

// API service for payment providers
export const paymentProviderService = {
  // Get all payment providers
  getAllProviders: async (): Promise<PaymentProvider[]> => {
    try {
      const response = await api.get<PaymentProvider[]>('/payment-providers');
      return response.data;
    } catch (error) {
      console.error('Error fetching payment providers:', error);
      return [];
    }
  }
};

export default api; 