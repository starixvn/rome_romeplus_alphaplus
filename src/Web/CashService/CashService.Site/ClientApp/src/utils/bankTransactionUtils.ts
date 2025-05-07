import { Bank } from '../services/interfaces/Bank';
import { BANK_TRANSACTION_TYPES } from '../constants/paymentConstants';

export const hasDepositTransactionType = (bankId: string, banks: Bank[]): boolean => {
  if (!bankId) return false;
  const bank = banks.find(b => b.id === parseInt(bankId));
  if (!bank || !bank.transactionTypes) return false;
  const transactionTypes = bank.transactionTypes.split(',').map(t => t.trim());
  return transactionTypes.includes(BANK_TRANSACTION_TYPES[1].id.toString()) || // Manual Deposit
         transactionTypes.includes(BANK_TRANSACTION_TYPES[2].id.toString());   // Auto Deposit
};

export const hasManualDepositTransactionType = (bankId: string, banks: Bank[]): boolean => {
  if (!bankId) return false;
  const bank = banks.find(b => b.id === parseInt(bankId));
  if (!bank || !bank.transactionTypes) return false;
  const transactionTypes = bank.transactionTypes.split(',').map(t => t.trim());
  return transactionTypes.includes(BANK_TRANSACTION_TYPES[1].id.toString()); // Manual Deposit
};

export const hasAutoDepositTransactionType = (bankId: string, banks: Bank[]): boolean => {
  if (!bankId) return false;
  const bank = banks.find(b => b.id === parseInt(bankId));
  if (!bank || !bank.transactionTypes) return false;
  const transactionTypes = bank.transactionTypes.split(',').map(t => t.trim());
  return transactionTypes.includes(BANK_TRANSACTION_TYPES[2].id.toString()); // Auto Deposit
};

export const hasWithdrawalTransactionType = (bankId: string, banks: Bank[]): boolean => {
  if (!bankId) return false;
  const bank = banks.find(b => b.id === parseInt(bankId));
  if (!bank || !bank.transactionTypes) return false;
  const transactionTypes = bank.transactionTypes.split(',').map(t => t.trim());
  return transactionTypes.includes(BANK_TRANSACTION_TYPES[3].id.toString());   // Manual Withdrawal
};

export const hasManualWithdrawalTransactionType = (bankId: string, banks: Bank[]): boolean => {
  if (!bankId) return false;
  const bank = banks.find(b => b.id === parseInt(bankId));
  if (!bank || !bank.transactionTypes) return false;
  const transactionTypes = bank.transactionTypes.split(',').map(t => t.trim());
  return transactionTypes.includes(BANK_TRANSACTION_TYPES[3].id.toString()); // Manual Withdrawal
}; 