import axios from 'axios';

export interface TransactionHistoryItem {
  id: string;
  username: string;
  firstName: string;
  lastName: string;
  amount: number;
  currency: string;
  from: string;
  to: string;
  status: 'COMPLETED' | 'REJECTED' | 'PROVIDER_REJECTED';
  transactionType: string;
  referenceNumber: string;
  dateTime: string;
}

export interface TransactionsHistoryResponse {
  items: TransactionHistoryItem[];
  totalCount: number;
}

export interface TransactionsHistoryParams {
  dateRange: [string, string];
  status: string;
  column: string;
  searchText: string;
  page: number;
  pageSize: number;
}

const api = axios.create({
  baseURL: 'http://localhost:5252/api',
  headers: {
    'Content-Type': 'application/json',
  },
});

export const getTransactions = async (
  params: TransactionsHistoryParams
): Promise<TransactionsHistoryResponse> => {
  try {
    const queryParams = new URLSearchParams();
    queryParams.append('fromDate', params.dateRange[0]);
    queryParams.append('toDate', params.dateRange[1]);
    queryParams.append('status', params.status);
    queryParams.append('column', params.column);
    queryParams.append('searchText', params.searchText);
    queryParams.append('page', params.page.toString());
    queryParams.append('pageSize', params.pageSize.toString());

    const response = await api.get<TransactionsHistoryResponse>(
      `/transactions-history?${queryParams.toString()}`
    );
    return response.data;
  } catch (error) {
    console.error('Error fetching transactions:', error);
    return {
      items: [],
      totalCount: 0,
    };
  }
};
