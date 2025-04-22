// Bank Account interface
export interface BankAccount {
  id: number;
  accountNumber: string;
  accountNickname: string;
  accountHolderName: string;
  transactionType: number;
  status: number;
  bank: {
    id: number;
    name: string;
  };
  lastUpdated: string;
  lastUpdatedStr: string;
  depositType: number;
  withdrawalType: number;
  depositMatchingMode: number;
  promptPay: boolean;
  mobile: string;
} 