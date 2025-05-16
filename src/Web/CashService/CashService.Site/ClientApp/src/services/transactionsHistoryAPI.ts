import { api } from '../config/axios';
import {
  TransactionItem,
  TransactionsHistoryResponse,
  TransactionsHistoryParams,
} from './interfaces/Transactions/TransactionHistory';
import { AccountOption } from './interfaces/Transactions/AccountOption';
import { ResultModel } from './interfaces/ResultModel';
import { BankInfo, ProviderInfo } from './interfaces/Transactions/BankInfo';
import { MemberLoginName } from './interfaces/Transactions/MemberLoginName';

export const transactionAPI = {
  getTransactions: async (
    params: TransactionsHistoryParams
  ): Promise<TransactionsHistoryResponse> => {
    try {
      const queryParams = new URLSearchParams({
        pageIndex: params.pageIndex.toString(),
        pageSize: params.pageSize.toString(),
        paymentStatus: params.status,
        searchText: params.searchText,
        account: params.account,
        fromDate: params.dateRange[0],
        toDate: params.dateRange[1],
      });

      if (params.transactionType) {
        queryParams.append('transactionType', params.transactionType.toString());
      }

      const response = await api.get<TransactionsHistoryResponse>(
        `GetAll?${queryParams.toString()}`
      );
      return response.data;
    } catch (error) {
      console.error('Error fetching transactions:', error);
      return {
        items: [],
        totalCount: 0,
      };
    }
  },

  getTransactionDetail: async (transactionId: number): Promise<TransactionItem> => {
    try {
      const response = await api.get<TransactionItem>(`Detail?transactionId=${transactionId}`);
      return response.data;
    } catch (error) {
      console.error(`Error fetching transaction detail for ID ${transactionId}:`, error);
      throw error;
    }
  },

  revertTransaction: async (transactionId: number, remarks: string): Promise<ResultModel> => {
    try {
      const response = await api.post<ResultModel>('RevertTransactionToPending', {
        transactionId: transactionId,
        remarks: remarks,
      });
      return response.data;
    } catch (error) {
      console.error(`Error reverting transaction for ID ${transactionId}:`, error);
      throw error;
    }
  },

  getAccount: async (transactionType: number): Promise<AccountOption[]> => {
    try {
      const response = await api.get<AccountOption[]>(
        `GetFilterAccount?transactionType=${transactionType}`
      );
      return response.data;
    } catch (error) {
      console.error('Error fetching accounts:', error);
      return [];
    }
  },

  getCashMemberBankInfo: async (custId: number): Promise<BankInfo[]> => {
    try {
      const response = await api.get<BankInfo[]>(`GetCashMemberBankInfo?custId=${custId}`);
      return response.data;
    } catch (error) {
      console.error('Error fetching cash member bank info:', error);
      return [];
    }
  },

  getCashAgentProvider: async (transactionType: number): Promise<ProviderInfo[]> => {
    try {
      const response = await api.get<ProviderInfo[]>(
        `GetCashAgentProvider?transactionType=${transactionType}`
      );
      console.log(response.data);
      return response.data;
    } catch (error) {
      console.error('Error fetching cash agent provider info:', error);
      return [];
    }
  },

  getMemberLoginName: async (): Promise<MemberLoginName[]> => {
    try {
      const response = await api.get<MemberLoginName[]>('GetMemberLoginName', { method: 'GET' });
      return response.data;
    } catch (error) {
      console.error('Error fetching member login names:', error);
      return [];
    }
  },

  getCashAgentBankInfo: async (transactionType: number): Promise<BankInfo[]> => {
    try {
      const response = await api.get<BankInfo[]>(
        `GetCashAgentBankInfo?transactionType=${transactionType}`,
        { method: 'GET' }
      );
      return response.data;
    } catch (error) {
      console.error('Error fetching cash agent bank info:', error);
      return [];
    }
  },
};
