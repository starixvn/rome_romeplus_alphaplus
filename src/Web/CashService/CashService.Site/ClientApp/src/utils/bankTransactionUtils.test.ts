import { describe, it, expect } from 'vitest';
import { hasDepositTransactionType, hasManualDepositTransactionType, hasAutoDepositTransactionType, hasWithdrawalTransactionType, hasManualWithdrawalTransactionType } from './bankTransactionUtils';
import { Bank } from '../services/interfaces/Bank';

describe('bankTransactionUtils', () => {
  const mockBanks: Bank[] = [
    {
      id: 1,
      name: 'Bank 1',
      code: 'BANK1',
      transactionTypes: '1,2,3', // Manual Deposit, Auto Deposit, Manual Withdrawal
      depositMatchingMode: {
        sourceType: 'BANK',
        mappingTypes: [1, 2],
        sourceTypeName: 'Bank Matching'
      }
    },
    {
      id: 2,
      name: 'Bank 2',
      code: 'BANK2',
      transactionTypes: '3,4', // Manual Withdrawal, Auto Withdrawal
      depositMatchingMode: {
        sourceType: 'BANK',
        mappingTypes: [1],
        sourceTypeName: 'Bank Matching'
      }
    },
    {
      id: 3,
      name: 'Bank 3',
      code: 'BANK3',
      transactionTypes: '', // No transaction types
      depositMatchingMode: {
        sourceType: 'BANK',
        mappingTypes: [],
        sourceTypeName: 'Bank Matching'
      }
    }
  ];

  describe('hasDepositTransactionType', () => {
    it('should return true for bank with manual deposit', () => {
      expect(hasDepositTransactionType('1', mockBanks)).toBe(true);
    });

    it('should return true for bank with auto deposit', () => {
      expect(hasDepositTransactionType('1', mockBanks)).toBe(true);
    });

    it('should return false for bank without deposit types', () => {
      expect(hasDepositTransactionType('2', mockBanks)).toBe(false);
    });

    it('should return false for invalid bank id', () => {
      expect(hasDepositTransactionType('999', mockBanks)).toBe(false);
    });

    it('should return false for empty bank id', () => {
      expect(hasDepositTransactionType('', mockBanks)).toBe(false);
    });
  });

  describe('hasManualDepositTransactionType', () => {
    it('should return true for bank with manual deposit', () => {
      expect(hasManualDepositTransactionType('1', mockBanks)).toBe(true);
    });

    it('should return false for bank without manual deposit', () => {
      expect(hasManualDepositTransactionType('2', mockBanks)).toBe(false);
    });

    it('should return false for bank with no transaction types', () => {
      expect(hasManualDepositTransactionType('3', mockBanks)).toBe(false);
    });
  });

  describe('hasAutoDepositTransactionType', () => {
    it('should return true for bank with auto deposit', () => {
      expect(hasAutoDepositTransactionType('1', mockBanks)).toBe(true);
    });

    it('should return false for bank without auto deposit', () => {
      expect(hasAutoDepositTransactionType('2', mockBanks)).toBe(false);
    });
  });

  describe('hasWithdrawalTransactionType', () => {
    it('should return true for bank with manual withdrawal', () => {
      expect(hasWithdrawalTransactionType('1', mockBanks)).toBe(true);
    });

    it('should return true for bank with manual withdrawal', () => {
      expect(hasWithdrawalTransactionType('2', mockBanks)).toBe(true);
    });

    it('should return false for bank without withdrawal', () => {
      expect(hasWithdrawalTransactionType('3', mockBanks)).toBe(false);
    });
  });

  describe('hasManualWithdrawalTransactionType', () => {
    it('should return true for bank with manual withdrawal', () => {
      expect(hasManualWithdrawalTransactionType('1', mockBanks)).toBe(true);
    });

    it('should return true for bank with manual withdrawal', () => {
      expect(hasManualWithdrawalTransactionType('2', mockBanks)).toBe(true);
    });

    it('should return false for bank without manual withdrawal', () => {
      expect(hasManualWithdrawalTransactionType('3', mockBanks)).toBe(false);
    });
  });
}); 