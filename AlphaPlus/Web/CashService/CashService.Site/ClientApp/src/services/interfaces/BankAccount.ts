// Bank Account interface
export interface BankAccount {
  id: number;
  accountNumber: string;
  accountNickname: string;
  accountHolderName: string;
  transactionTypes: string;
  status: number;
  bankId: number;
  bankName: string;
  lastUpdated: string;
  lastUpdatedStr: string;
  depositType: number;
  withdrawalType: number;
  depositMatchingMode: number;
  promptPay: boolean;
  mobile: string;
} 