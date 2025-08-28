import { DepositMatchingMode } from './Payments/DepositMatchingMode';

// Bank interface
export interface Bank {
  id: number;
  name: string;
  code: string;
  transactionTypes: string;
  depositMatchingMode: DepositMatchingMode;
} 