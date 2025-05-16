export interface TransactionItem {
  transactionId: number;
  username: string;
  firstName: string;
  lastName: string;
  amount: number;
  currency: string;
  paymentFrom: string;
  paymentTo: string;
  statusId: number;
  status: 'COMPLETED' | 'REJECTED' | 'PROVIDER_REJECTED' | 'EXPIRED';
  betCredit: number;
  transactionTypeId: number;
  transactionType: string;
  transferRefNo: string;
  remark: string;
  createdTime: string;
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
  pageIndex: number;
  pageSize: number;
  transactionType: number;
}
