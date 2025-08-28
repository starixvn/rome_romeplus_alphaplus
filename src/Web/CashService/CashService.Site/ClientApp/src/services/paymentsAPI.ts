import { api } from './axios';
import { ResultModel } from './interfaces/ResultModel';
import { TransactionType } from './interfaces/TransactionType';
import * as PaymentsInterfaces from './interfaces/Payments';
import { Bank } from './interfaces/Bank';

// API service for payment providers
export const paymentProviderService = {
  getProviders: async (): Promise<any> => {
    return await api.get<PaymentsInterfaces.PaymentProvider[]>('GetProviders');
  },

  getDailyWithdrawalCount: async (): Promise<any> => {
    return await api.get<number>('GetDailyWithdrawalCount');
  },

  setDailyWithdrawalCount: async (count: number): Promise<any> => {
    return await api.post<ResultModel>('SetDailyWithdrawalCount', { count });
  }, 

  getProviderFees: async (providerId: number): Promise<any> => {
    return await api.get<PaymentsInterfaces.PaymentProviderFeeGroup[]>(`GetProviderFees?providerId=${providerId}`);
  },

  getProviderSettings: async (
    providerId: number,
    method: number,
    status: number,
    transactionType: number
  ): Promise<any> => {    
    return await api.get<any>(`ProviderSettings?providerId=${providerId}&method=${method}&status=${status}&transactionType=${transactionType}`);
  },

  getAddPaymentMethodInfo: async (
    providerId: number,
    transactionType: number
  ): Promise<any> => {
    return await api.get<PaymentsInterfaces.AddPaymentMethodViewModel>(`GetAddPaymentMethodInfo?providerId=${providerId}&transactionType=${transactionType}`);
  },

  addPaymentMethod: async (paymentMethod: PaymentsInterfaces.PaymentMethod): Promise<any> => {
    return await api.post('AddPaymentMethod', paymentMethod);
  },

  updatePaymentMethod: async (paymentMethod: PaymentsInterfaces.PaymentMethod): Promise<any> => {
    return await api.post('UpdatePaymentMethod', paymentMethod);
  },

  getBanksByTransactionType: async (transactionType: number): Promise<any> => {
    return await api.get<Bank[]>(`GetBanksByTransactionType?transactionType=${transactionType}`);
  },

  updateProvider: async (provider: any): Promise<any> => {
    return await api.post(`UpdateProvider`, provider);
  },

  getBanksByCust: async (): Promise<any> => {
    return await api.get<Bank[]>(`GetBanksByCust`);
  },

  getTransactionTypes: async (): Promise<any> => {
    return await api.get<TransactionType[]>(`GetTransactionTypes`);
  },

  getBankAccounts: async (
    bankId: number,
    transactionType: number,
    status: number,
    searchText: string
  ): Promise<any> => {
    return await api.get<PaymentsInterfaces.BankAccount[]>(`GetBankAccounts?bankId=${bankId}&transactionType=${transactionType}&status=${status}&searchText=${searchText}`);
  },

  getTransactionLimit: async (): Promise<any> => {
    return await api.get<PaymentsInterfaces.PaymentTransactionLimit>(`GetTransactionLimit`);
  },

  setTransactionLimit: async (limit: PaymentsInterfaces.PaymentTransactionLimit): Promise<any> => {  
    return await api.post(`SetTransactionLimit`, limit);
  },

  addBankAccount: async (bankAccount: PaymentsInterfaces.BankAccount): Promise<any> => {
    return await api.post('AddBankAccount', bankAccount);
  },

  getBankAccountById: async (bankAccountId: number): Promise<any> => {
    return await api.get<PaymentsInterfaces.BankAccount>(`GetBankAccountById?bankAccountId=${bankAccountId}`);
  },

  updateBankAccount: async (bankAccount: PaymentsInterfaces.BankAccount): Promise<any> => {
    return await api.post(`UpdateBankAccount`, bankAccount);
  },

  deleteBankAccount: async (bankAccountId: number): Promise<any> => {
    return await api.post(`DeleteBankAccount`, { bankAccountId });
  },
};
