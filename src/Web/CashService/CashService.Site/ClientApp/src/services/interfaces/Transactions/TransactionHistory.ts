export interface TransactionItem {
  transactionId: number;
  username: string;
  firstName: string;
  lastName: string;
  amount: number;
  betCredit: number;
  currency: string;
  from: string;
  to: string;
  status: 'COMPLETED' | 'REJECTED' | 'PROVIDER_REJECTED' | 'EXPIRED';
  transactionType: string;
  referenceNumber: string;
  remarks: string;
  createdDate: string;
  lastUpdatedDate: string;
}

export interface TransactionsHistoryResponse {
  items: TransactionItem[];
  totalCount: number;
}

export interface TransactionsHistoryParams {
  dateRange: [string, string];
  status: string;
  searchText: string;
  account: string;
  page: number;
  pageSize: number;
  transactionType: 'deposit' | 'withdrawal';
}
