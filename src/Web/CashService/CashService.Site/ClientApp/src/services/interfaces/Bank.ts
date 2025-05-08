import { DepositMatchingMode } from './DepositMatchingMode';

// Bank interface
export interface Bank {
  id: number;
  name: string;
  transactionTypes: string;
  depositMatchingMode: DepositMatchingMode;
} 