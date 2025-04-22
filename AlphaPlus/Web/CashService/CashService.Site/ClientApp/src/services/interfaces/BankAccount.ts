// Bank Account interface
export interface BankAccount {
  id: number;
  accountNumber: string;
  accountNickname: string;
  accountHolderName: string;
  transactionType: string;
  status: number;
  bank: {
    id: number;
    name: string;
  };
  lastUpdated: string;
  lastUpdatedStr: string;
} 