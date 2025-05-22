import { api } from './axios';
import { ResultModel } from './interfaces/ResultModel';
import { TransactionType } from './interfaces/TransactionType';
import { 
  PaymentProvider,
  PaymentProviderFeeGroup,
  PaymentMethod,
  Bank,
  BankAccount,
  PaymentTransactionLimit,
  AddPaymentMethodViewModel,
  createDefaultBankAccount,
  createDefaultPaymentTransactionLimit
} from './interfaces/Payments';

// API service for payment providers
export const paymentProviderService = {
  getProviders: async (): Promise<PaymentProvider[]> => {
    const result = await api.get<PaymentProvider[]>('GetProviders');
    return result || [];
  },

  getDailyWithdrawalCount: async (): Promise<number> => {
    const result = await api.get<number>('GetDailyWithdrawalCount');
    return result || 0;
  },

  setDailyWithdrawalCount: async (count: number): Promise<ResultModel> => {
    const result = await api.post<ResultModel>('SetDailyWithdrawalCount', { count });
    return result || { code: 1, message: 'Failed to set daily withdrawal count', isSuccessful: false };
  }, 

  getProviderFees: async (providerId: number): Promise<PaymentProviderFeeGroup[]> => {
    const result = await api.get<PaymentProviderFeeGroup[]>(`GetProviderFees?providerId=${providerId}`);
    return result || [];
  },

  getProviderSettings: async (
    providerId: number,
    method: number,
    status: number,
    transactionType: number
  ): Promise<any> => {    
    const result = await api.get<any>(`ProviderSettings?providerId=${providerId}&method=${method}&status=${status}&transactionType=${transactionType}`);
    return result || [];
  },

  getAddPaymentMethodInfo: async (
    providerId: number,
    transactionType: number
  ): Promise<AddPaymentMethodViewModel> => {
    const result = await api.get<AddPaymentMethodViewModel>(`GetAddPaymentMethodInfo?providerId=${providerId}&transactionType=${transactionType}`);
    return result || { paymentMethods: [], channelContact: {tele: ''} };
  },

  addPaymentMethod: async (paymentMethod: PaymentMethod): Promise<ResultModel> => {
    const result = await api.post('AddPaymentMethod', paymentMethod);
    return result || { code: 1, message: 'Failed to add payment method', isSuccessful: false };
  },

  updatePaymentMethod: async (paymentMethod: PaymentMethod): Promise<ResultModel> => {
    const result = await api.post('UpdatePaymentMethod', paymentMethod);
    return result || { code: 1, message: 'Failed to update payment method', isSuccessful: false };
  },

  getBanksByTransactionType: async (transactionType: number): Promise<Bank[]> => {
    const result = await api.get<Bank[]>(`GetBanksByTransactionType?transactionType=${transactionType}`);
    return result || [];
  },

  updateProvider: async (provider: any): Promise<ResultModel> => {
    const result = await api.post(`UpdateProvider`, provider);
    return result || { code: 1, message: 'Failed to update provider', isSuccessful: false };
  },

  getBanksByCust: async (): Promise<Bank[]> => {
    const result = await api.get<Bank[]>(`GetBanksByCust`);
    return result || [];
  },

  getTransactionTypes: async (): Promise<TransactionType[]> => {
    const result = await api.get<TransactionType[]>(`GetTransactionTypes`);
    return result || [];
  },

  getBankAccounts: async (
    bankId: number,
    transactionType: number,
    status: number,
    searchText: string
  ): Promise<BankAccount[]> => {
    const result = await api.get<BankAccount[]>(`GetBankAccounts?bankId=${bankId}&transactionType=${transactionType}&status=${status}&searchText=${searchText}`);
    return result || [];     
  },

  getTransactionLimit: async (): Promise<PaymentTransactionLimit> => {
    const result = await api.get<PaymentTransactionLimit>(`GetTransactionLimit`);
    return result || createDefaultPaymentTransactionLimit();
  },

  setTransactionLimit: async (limit: PaymentTransactionLimit): Promise<ResultModel> => {  
    const result = await api.post(`SetTransactionLimit`, limit);
    return result || { code: 1, message: 'Failed to set transaction limit', isSuccessful: false };
  },

  addBankAccount: async (bankAccount: BankAccount): Promise<ResultModel> => {
    const result = await api.post('AddBankAccount', bankAccount);
    return result || { code: 1, message: 'Failed to add bank account', isSuccessful: false };
  },

  getBankAccountById: async (bankAccountId: number): Promise<BankAccount> => {
    const result = await api.get<BankAccount>(`GetBankAccountById?bankAccountId=${bankAccountId}`);
    return result || createDefaultBankAccount();
  },

  updateBankAccount: async (bankAccount: BankAccount): Promise<ResultModel> => {
    const result = await api.put(`UpdateBankAccount`, bankAccount);
    return result || { code: 1, message: 'Failed to update bank account', isSuccessful: false };
  },

  deleteBankAccount: async (bankAccountId: number): Promise<ResultModel> => {
    const result = await api.post(`DeleteBankAccount`, { bankAccountId });
    return result || { code: 1, message: 'Failed to delete bank account', isSuccessful: false };
  },
};
