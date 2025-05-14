import axios from 'axios';
import {
  TransactionItem,
  TransactionsHistoryResponse,
  TransactionsHistoryParams,
} from './interfaces/Transactions/TransactionHistory';
import { AccountOption } from './interfaces/Transactions/AccountOption';
import { ResultModel } from './interfaces/ResultModel';
import { MemberLoginName } from './interfaces/Transactions/MemberLoginName';
import { BankInfo } from './interfaces/Transactions/BankInfo';

const api = axios.create({
  baseURL: '',
  headers: {
    'Content-Type': 'application/json',
  },
});

export const getTransactions = async (
  params: TransactionsHistoryParams
): Promise<TransactionsHistoryResponse> => {
  try {
    const queryParams = new URLSearchParams({
      page: params.page.toString(),
      pageSize: params.pageSize.toString(),
      status: params.status,
      searchText: params.searchText,
      account: params.account,
      fromDate: params.dateRange[0],
      toDate: params.dateRange[1],
    });

    if (params.transactionType) {
      queryParams.append('transactionType', params.transactionType.toString());
    }

    const response = await api.get<TransactionsHistoryResponse>(`GetAll?${queryParams.toString()}`);
    return response.data;
  } catch (error) {
    console.error('Error fetching transactions:', error);
    return {
      items: [],
      totalCount: 0,
    };
  }
};

/**
 * Fetches details for a single transaction.
 */
export const getTransactionDetail = async (transactionId: number): Promise<TransactionItem> => {
  try {
    const response = await api.get<TransactionItem>(`Detail?transactionId=${transactionId}`);
    return response.data;
  } catch (error) {
    console.error(`Error fetching transaction detail for ID ${transactionId}:`, error);
    throw error;
  }
};

/**
 * Reverts a transaction to pending status.
 */
export const revertTransaction = async (
  transactionId: number,
  remarks: string
): Promise<ResultModel> => {
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
};

/**
 * Fetches account options.
 */
export const getAccount = async (transactionType: number): Promise<AccountOption[]> => {
  try {
    const response = await api.get<AccountOption[]>(
      `GetFilterAccount?transactionType=${transactionType}`
    );
    return response.data;
  } catch (error) {
    console.error('Error fetching accounts:', error);
    return [];
  }
};

export const getMemberLoginName = async (): Promise<MemberLoginName[]> => {
  try {
    const response = await api.get<MemberLoginName[]>('GetMemberLoginName');
    return response.data;
  } catch (error) {
    console.error('Error fetching member login names:', error);
    return [];
  }
};

export const GetCashAgentBankInfo = async (transactionType: number): Promise<BankInfo[]> => {
  try {
    const response = await api.get<BankInfo[]>(`GetCashAgentBankInfo?transactionType=${transactionType}`);
    return response.data;
  } catch (error) {
    console.error('Error fetching cash agent bank info:', error);
    return [];
  }
};

export const GetCashMemberBankInfo = async (custId: number): Promise<BankInfo[]> => {
  try {
    const response = await api.get<BankInfo[]>(`GetCashMemberBankInfo?custId=${custId}`);
    return response.data;
  } catch (error) {
    console.error('Error fetching cash member bank info:', error);
    return [];
  }
};

export const GetCashAgentProvider = async (transactionType: number): Promise<BankInfo[]> => {
  try {
    const response = await api.get<BankInfo[]>(`GetCashAgentProvider?transactionType=${transactionType}`);
    return response.data;
  } catch (error) {
    console.error('Error fetching cash agent provider info:', error);
    return [];
  }
};