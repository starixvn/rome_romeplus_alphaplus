import { Bank } from '../services/interfaces/Bank';
import { BANK_TRANSACTION_TYPES } from '../constants/paymentConstants';

const checkTransactionType = (bankId: string, banks: Bank[], transactionTypeId: number): boolean => {
  if (!bankId) return false;
  const bank = banks.find(b => b.id === parseInt(bankId));
  if (!bank || !bank.transactionTypes) return false;
  const transactionTypes = bank.transactionTypes.split(',').map(t => t.trim());
  return transactionTypes.includes(transactionTypeId.toString());
};

export const hasDepositTransactionType = (bankId: string, banks: Bank[]): boolean => {
  return checkTransactionType(bankId, banks, BANK_TRANSACTION_TYPES[1].id) || // Manual Deposit
         checkTransactionType(bankId, banks, BANK_TRANSACTION_TYPES[2].id);   // Auto Deposit
};

export const hasManualDepositTransactionType = (bankId: string, banks: Bank[]): boolean => {
  return checkTransactionType(bankId, banks, BANK_TRANSACTION_TYPES[1].id); // Manual Deposit
};

export const hasAutoDepositTransactionType = (bankId: string, banks: Bank[]): boolean => {
  return checkTransactionType(bankId, banks, BANK_TRANSACTION_TYPES[2].id); // Auto Deposit
};

export const hasWithdrawalTransactionType = (bankId: string, banks: Bank[]): boolean => {
  return checkTransactionType(bankId, banks, BANK_TRANSACTION_TYPES[3].id); // Manual Withdrawal
};

export const hasManualWithdrawalTransactionType = (bankId: string, banks: Bank[]): boolean => {
  return checkTransactionType(bankId, banks, BANK_TRANSACTION_TYPES[3].id); // Manual Withdrawal
}; 