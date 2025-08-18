export interface TransactionItem {
  transId: string;
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
  status: string;
  betCredit: number;
  transactionTypeId: number;
  transactionType: string;
  remark: string;
  payslip: string;
  base64Payslip: string;
  createdTime: string;
  lastUpdatedDate: string;
  isAllowRevert: boolean;
  createdBy: string;
  operatedTime: string;
  operatedBy: string;
  isHadPayslip: boolean;
  paymentType: number;
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

export interface ExportExcelParams {
  fromDate: string;
  toDate: string;
  transactionType: number;
  status: number;
  account: string;
}
