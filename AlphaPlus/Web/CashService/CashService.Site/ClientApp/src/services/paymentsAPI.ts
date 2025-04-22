import axios from 'axios';
import { PaymentProvider } from './interfaces/PaymentProvider';
import { PaymentProviderFeeGroup } from './interfaces/PaymentProviderFeeGroup';
import { ResultModel } from './interfaces/ResultModel';
import { PaymentMethod } from './interfaces/PaymentMethod';
import { Bank } from './interfaces/Bank';
import { BankAccount } from './interfaces/BankAccount';
import { PaymentTransactionLimit } from './interfaces/PaymentTransactionLimit';

// Declare the cashServiceAPIUrl property on the Window interface
declare global {
  interface Window {
    cashServiceAPIUrl: string;
  }
}

// Create an axios instance with default config
const api = axios.create({
  baseURL: "",
  headers: {
    'Content-Type': 'application/json'
  },
  withCredentials: true
});

// API service for payment providers
export const paymentProviderService = {
  // Get payment providers
  getProviders: async (): Promise<PaymentProvider[]> => {
    try {
      const response = await api.get<PaymentProvider[]>('GetProviders', { method: 'GET' });
      return response.data;
    } catch (error) {
      console.error('Error fetching payment providers:', error);
      return [];
    }
  },
  
  // Get daily withdrawal count for a customer
  getDailyWithdrawalCount: async (): Promise<number> => {
    try {
      const response = await api.get(`GetDailyWithdrawalCount`, { method: 'GET' });
      return response.data;
    } catch (error) {
      console.error('Error getting daily withdrawal count:', error);
      throw error;
    }
  },
  
  // Set daily withdrawal count for a customer
  setDailyWithdrawalCount: async (count: number): Promise<ResultModel> => {
    try {
      const response = await api.post(`SetDailyWithdrawalCount`, { count });
      return response.data;
    } catch (error) {
      console.error('Error setting daily withdrawal count:', error);
      throw error;
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
      const response = await api.put(`/update-payment-provider-settings`, settings );
      return response.data;
    } catch (error) {
      console.error(`Error updating settings`, error);
      throw error;
    }
  },
  
  // Get all payment methods
  getAllPaymentMethods: async (): Promise<PaymentMethod[]> => {
    try {
      const response = await api.get<PaymentMethod[]>('/payment-methods', { method: 'GET' });
      return response.data;
    } catch (error) {
      console.error('Error fetching payment methods:', error);
      return [];
    }
  },
  
  // Get all banks
  getAllBanks: async (): Promise<Bank[]> => {
    try {
      const response = await api.get<Bank[]>('/banks', { method: 'GET' });
      return response.data;
    } catch (error) {
      console.error('Error fetching banks:', error);
      return [];
    }
  },
  
  // Get bank accounts with optional filters
  getBankAccounts: async (
    custId: number,
    bankId: number,
    transactionType: number,
    status: number,
    searchText: string,
  ): Promise<BankAccount[]> => {
    try {
      const params = new URLSearchParams();
      params.append('custId', custId.toString());
      params.append('bankId', bankId.toString());
      params.append('transactionType', transactionType.toString());
      params.append('status', status.toString());
      params.append('searchText', searchText);
      
      const response = await api.get<BankAccount[]>(`/bank-accounts?${params.toString()}`, { method: 'GET' });
      return response.data;
    } catch (error) {
      console.error('Error fetching bank accounts:', error);
      return [];
    }
  },
  
  // Add a new payment method
  addPaymentMethod: async (paymentMethod: PaymentMethod): Promise<ResultModel> => {
    try {
      const response = await api.post('/add-payment-method', paymentMethod);
      return response.data;
    } catch (error) {
      console.error('Error adding payment method:', error);
      throw error;
    }
  },
  
  // Delete a bank account
  deleteBankAccount: async (bankAccountId: number): Promise<ResultModel> => {
    try {
      const response = await api.delete(`/bank-accounts/${bankAccountId}`);
      return response.data;
    } catch (error) {
      console.error(`Error deleting bank account ${bankAccountId}:`, error);
      throw error;
    }
  },

  // Set transaction limit for a payment provider or bank account
  setTransactionLimit: async (custId: number, limit: PaymentTransactionLimit): Promise<ResultModel> => {
    try {
      const response = await api.post(`/set-transaction-limit?custId=${custId}`, limit);
      return response.data;
    } catch (error) {
      console.error('Error setting transaction limit:', error);
      throw error;
    }
  },
  
  // Add a new bank account
  addBankAccount: async (bankAccount: BankAccount): Promise<ResultModel> => {
    try {
      const response = await api.post('/add-bank-account', bankAccount);
      return response.data;
    } catch (error) {
      console.error('Error adding bank account:', error);
      throw error;
    }
  },
  
  // Update an existing bank account
  updateBankAccount: async (bankAccount: BankAccount): Promise<ResultModel> => {
    try {
      const response = await api.put(`/update-bank-account`, bankAccount);
      return response.data;
    } catch (error) {
      console.error('Error updating bank account:', error);
      throw error;
    }
  },
};

export default api; 