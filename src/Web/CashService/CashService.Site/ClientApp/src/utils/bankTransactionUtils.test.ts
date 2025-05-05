import { describe, it, expect, vi } from 'vitest';
import {
  hasDepositTransactionType,
  hasManualDepositTransactionType,
  hasAutoDepositTransactionType,
  hasWithdrawalTransactionType,
  hasManualWithdrawalTransactionType
} from './bankTransactionUtils';

// Mock BANK_TRANSACTION_TYPES (should match the real structure)
vi.mock('../constants/paymentConstants', () => ({
  BANK_TRANSACTION_TYPES: [
    { id: 0, name: 'All' },
    { id: 1, name: 'Manual Deposit' },
    { id: 2, name: 'Auto Deposit' },
    { id: 3, name: 'Manual Withdrawal' },
    { id: 4, name: 'Auto Withdrawal' }
  ]
}));

const banks = [
  { id: 1, name: 'Bank A', transactionTypes: '1,2,3', depositMatchingMode: 0 },
  { id: 2, name: 'Bank B', transactionTypes: '2,3', depositMatchingMode: 0 },
  { id: 3, name: 'Bank C', transactionTypes: '3', depositMatchingMode: 0 },
  { id: 4, name: 'Bank D', transactionTypes: '', depositMatchingMode: 0 },
];

describe('bankTransactionUtils', () => {
  describe('hasDepositTransactionType', () => {
    it('returns true if bank has manual or auto deposit', () => {
      expect(hasDepositTransactionType('1', banks)).toBe(true); // has 1,2
      expect(hasDepositTransactionType('2', banks)).toBe(true); // has 2
    });
    it('returns false if bank does not have deposit types', () => {
      expect(hasDepositTransactionType('3', banks)).toBe(false); // only 3
      expect(hasDepositTransactionType('4', banks)).toBe(false); // none
      expect(hasDepositTransactionType('', banks)).toBe(false); // no id
      expect(hasDepositTransactionType('999', banks)).toBe(false); // not found
    });
  });

  describe('hasManualDepositTransactionType', () => {
    it('returns true if bank has manual deposit', () => {
      expect(hasManualDepositTransactionType('1', banks)).toBe(true);
    });
    it('returns false if bank does not have manual deposit', () => {
      expect(hasManualDepositTransactionType('2', banks)).toBe(false);
      expect(hasManualDepositTransactionType('3', banks)).toBe(false);
      expect(hasManualDepositTransactionType('4', banks)).toBe(false);
      expect(hasManualDepositTransactionType('', banks)).toBe(false);
      expect(hasManualDepositTransactionType('999', banks)).toBe(false);
    });
  });

  describe('hasAutoDepositTransactionType', () => {
    it('returns true if bank has auto deposit', () => {
      expect(hasAutoDepositTransactionType('1', banks)).toBe(true);
      expect(hasAutoDepositTransactionType('2', banks)).toBe(true);
    });
    it('returns false if bank does not have auto deposit', () => {
      expect(hasAutoDepositTransactionType('3', banks)).toBe(false);
      expect(hasAutoDepositTransactionType('4', banks)).toBe(false);
      expect(hasAutoDepositTransactionType('', banks)).toBe(false);
      expect(hasAutoDepositTransactionType('999', banks)).toBe(false);
    });
  });

  describe('hasWithdrawalTransactionType', () => {
    it('returns true if bank has manual withdrawal', () => {
      expect(hasWithdrawalTransactionType('1', banks)).toBe(true);
      expect(hasWithdrawalTransactionType('2', banks)).toBe(true);
      expect(hasWithdrawalTransactionType('3', banks)).toBe(true);
    });
    it('returns false if bank does not have manual withdrawal', () => {
      expect(hasWithdrawalTransactionType('4', banks)).toBe(false);
      expect(hasWithdrawalTransactionType('', banks)).toBe(false);
      expect(hasWithdrawalTransactionType('999', banks)).toBe(false);
    });
  });

  describe('hasManualWithdrawalTransactionType', () => {
    it('returns true if bank has manual withdrawal', () => {
      expect(hasManualWithdrawalTransactionType('1', banks)).toBe(true);
      expect(hasManualWithdrawalTransactionType('2', banks)).toBe(true);
      expect(hasManualWithdrawalTransactionType('3', banks)).toBe(true);
    });
    it('returns false if bank does not have manual withdrawal', () => {
      expect(hasManualWithdrawalTransactionType('4', banks)).toBe(false);
      expect(hasManualWithdrawalTransactionType('', banks)).toBe(false);
      expect(hasManualWithdrawalTransactionType('999', banks)).toBe(false);
    });
  });
}); 