import { api } from '../config/axios';
import { ResultModel } from './interfaces/ResultModel';
import { TransactionType } from './interfaces/TransactionType';
import { 
  PaymentProvider,
  PaymentProviderFeeGroup,
  PaymentMethod,
  Bank,
  BankAccount,
  PaymentTransactionLimit,
  AddPaymentMethodViewModel
} from './interfaces/Payments';

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
      const response = await api.get<PaymentProviderFeeGroup[]>(`GetProviderFees?providerId=${providerId}`, {
        method: 'GET',
      });
      return response.data;
    } catch (error) {
      console.error(`Error fetching fees for provider`, error);
      return [];
    }
  },

  // Get settings for a specific payment provider
  getProviderSettings: async (
    providerId: number,
    method: number,
    status: number,
    transactionType: number
  ): Promise<any> => {
    try {
      const params = new URLSearchParams();
      params.append('providerId', providerId.toString());
      params.append('method', method.toString());
      params.append('status', status.toString());
      params.append('transactionType', transactionType.toString());

      const response = await api.get(`ProviderSettings?${params.toString()}`, { method: 'GET' });
      return response.data;
    } catch (error) {
      console.error(`Error fetching settings for provider ${providerId}:`, error);
      throw error;
    }
  },

  // Get all payment methods and channel contact
  getAddPaymentMethodInfo: async (
    providerId: number,
    transactionType: number
  ): Promise<AddPaymentMethodViewModel> => {
    try {
      const response = await api.get<AddPaymentMethodViewModel>(
        `GetAddPaymentMethodInfo?providerId=${providerId}&transactionType=${transactionType}`,
        { method: 'GET' }
      );
      return response.data;
    } catch (error) {
      console.error('Error fetching payment methods:', error);
      return { paymentMethods: [], channelContact: {tele: ''} };
    }
  },

  // Add a new payment method
  addPaymentMethod: async (paymentMethod: PaymentMethod): Promise<ResultModel> => {
    try {
      const response = await api.post('AddPaymentMethod', paymentMethod);
      return response.data;
    } catch (error) {
      console.error('Error adding payment method:', error);
      throw error;
    }
  },

  // Update a payment method
  updatePaymentMethod: async (paymentMethod: PaymentMethod): Promise<ResultModel> => {
    try {
      const response = await api.post('UpdatePaymentMethod', paymentMethod);
      return response.data;
    } catch (error) {
      console.error('Error updating payment method:', error);
      throw error;
    }
  },

  // Get all banks
  getBanksByTransactionType: async (transactionType: number): Promise<Bank[]> => {
    try {
      const response = await api.get<Bank[]>(
        `GetBanksByTransactionType?transactionType=${transactionType}`,
        { method: 'GET' }
      );
      return response.data;
    } catch (error) {
      console.error('Error fetching banks:', error);
      return [];
    }
  },

  // Update payment provider
  updateProvider: async (provider: any): Promise<ResultModel> => {
    try {
      const response = await api.post(`UpdateProvider`, provider);
      return response.data;
    } catch (error) {
      console.error(`Error updating settings`, error);
      throw error;
    }
  },

  // Get banks for a specific provider
  getBanksByCust: async (): Promise<Bank[]> => {
    try {
      const response = await api.get<Bank[]>(`GetBanksByCust`, { method: 'GET' });
      return response.data;
    } catch (error) {
      console.error('Error fetching provider banks:', error);
      return [];
    }
  },

  // Get transaction types for a specific bank
  getTransactionTypes: async (): Promise<TransactionType[]> => {
    try {
      const response = await api.get<TransactionType[]>(`GetTransactionTypes`, { method: 'GET' });
      return response.data;
    } catch (error) {
      console.error('Error fetching transaction types:', error);
      return [];
    }
  },

  // Get bank accounts with optional filters
  getBankAccounts: async (
    bankId: number,
    transactionType: number,
    status: number,
    searchText: string
  ): Promise<BankAccount[]> => {
    try {
      const params = new URLSearchParams();
      params.append('bankId', bankId.toString());
      params.append('transactionType', transactionType.toString());
      params.append('status', status.toString());
      params.append('searchText', searchText);

      const response = await api.get<BankAccount[]>(`GetBankAccounts?${params.toString()}`, {
        method: 'GET',
      });
      return response.data;
    } catch (error) {
      console.error('Error fetching bank accounts:', error);
      return [];
    }
  },

  // Get transaction limit for a payment provider or bank account
  getTransactionLimit: async (): Promise<PaymentTransactionLimit> => {
    try {
      const response = await api.get<PaymentTransactionLimit>(`GetTransactionLimit`);
      return response.data;
    } catch (error) {
      console.error('Error getting transaction limit:', error);
      throw error;
    }
  },

  // Set transaction limit for a payment provider or bank account
  setTransactionLimit: async (limit: PaymentTransactionLimit): Promise<ResultModel> => {
    try {
      const response = await api.post(`SetTransactionLimit`, limit);
      return response.data;
    } catch (error) {
      console.error('Error setting transaction limit:', error);
      throw error;
    }
  },

  // Add a new bank account
  addBankAccount: async (bankAccount: BankAccount): Promise<ResultModel> => {
    try {
      const response = await api.post('AddBankAccount', bankAccount);
      return response.data;
    } catch (error) {
      console.error('Error adding bank account:', error);
      throw error;
    }
  },

  // Get a bank account by ID
  getBankAccountById: async (bankAccountId: number): Promise<BankAccount> => {
    try {
      const response = await api.get<BankAccount>(
        `GetBankAccountById?bankAccountId=${bankAccountId}`
      );
      return response.data;
    } catch (error) {
      console.error(`Error fetching bank account ${bankAccountId}:`, error);
      throw error;
    }
  },

  // Update an existing bank account
  updateBankAccount: async (bankAccount: BankAccount): Promise<ResultModel> => {
    try {
      const response = await api.put(`UpdateBankAccount`, bankAccount);
      return response.data;
    } catch (error) {
      console.error('Error updating bank account:', error);
      throw error;
    }
  },

  // Delete a bank account
  deleteBankAccount: async (bankAccountId: number): Promise<ResultModel> => {
    try {
      const response = await api.post(`DeleteBankAccount`, { bankAccountId });
      return response.data;
    } catch (error) {
      console.error(`Error deleting bank account ${bankAccountId}:`, error);
      throw error;
    }
  },
};
