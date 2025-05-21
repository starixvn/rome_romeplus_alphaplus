export interface TransactionItem {
  transId: number;
  custId: number;
  username: string;
  nickname: string;
  firstName: string;
  lastName: string;
  amount: number;
  currency: string;
  paymentFrom: string;
  paymentTo: string;
  statusId: number;
  betCredit: number;
  transactionTypeId: number;
  transactionType: string;
  remark: string;
  payslip: string;
  createdTime: string;
  lastUpdatedDate: string;
}

export interface TransactionsHistoryResponse {
  items: TransactionItem[];
  totalCount: number;
}

export interface TransactionsHistoryParams {
  dateRange: [string, string];
  status: number;
  searchText: string;
  account: string;
  pageIndex: number;
  pageSize: number;
  transactionType: number;
}

export interface PaymentInformation {
  paymentFrom: string;
  paymentTo: string;
}
