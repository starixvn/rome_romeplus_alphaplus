import axios from 'axios';
import {
  TransactionItem,
  TransactionsHistoryResponse,
  TransactionsHistoryParams,
} from './interfaces/Transactions/TransactionHistory';
import { ResultModel } from './interfaces/ResultModel';

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
      queryParams.append('transactionType', params.transactionType);
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
