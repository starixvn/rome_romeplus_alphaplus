// Bank Account interface
export interface BankAccount {
  id: number;
  accountNumber: string;
  accountNickname: string;
  accountHolderName: string;
  transactionTypes: string;
  status: number;
  code: string;
  bankId: number;
  bankName: string;
  lastUpdated: string;
  lastUpdatedStr: string;
  depositMatchingMode: number;
  promptPay: boolean;
  mobile: string;
  mobilePrefix: string;
} 