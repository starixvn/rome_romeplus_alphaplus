import { describe, it, expect } from 'vitest';
import { 
  hasDepositTransactionType,
  hasManualDepositTransactionType,
  hasAutoDepositTransactionType,
  hasWithdrawalTransactionType,
  hasManualWithdrawalTransactionType
} from './bankTransactionUtils';
import { Bank } from '../services/interfaces/Bank';

describe('bankTransactionUtils', () => {
  const mockBanks: Bank[] = [
    {
      id: 1,
      name: 'Bank 1',
      transactionTypes: '1,2,3', // Manual Deposit, Auto Deposit, Manual Withdrawal
      depositMatchingMode: {
        sourceType: '1',
        mappingTypes: [1, 2],
        sourceTypeName: 'Decimal'
      }
    },
    {
      id: 2,
      name: 'Bank 2',
      transactionTypes: '1,3', // Manual Deposit, Manual Withdrawal
      depositMatchingMode: {
        sourceType: '1',
        mappingTypes: [1],
        sourceTypeName: 'Decimal'
      }
    },
    {
      id: 3,
      name: 'Bank 3',
      transactionTypes: '2,4', // Auto Deposit, Auto Withdrawal
      depositMatchingMode: {
        sourceType: '2',
        mappingTypes: [2],
        sourceTypeName: 'Bank Matching'
      }
    },
    {
      id: 4,
      name: 'Bank 4',
      transactionTypes: '', // No transaction types
      depositMatchingMode: {
        sourceType: '1',
        mappingTypes: [],
        sourceTypeName: 'None'
      }
    },
    {
      id: 5,
      name: 'Bank 5',
      transactionTypes: ' 1 , 2 ', // Manual Deposit, Auto Deposit with whitespace
      depositMatchingMode: {
        sourceType: '1',
        mappingTypes: [1, 2],
        sourceTypeName: 'Decimal'
      }
    },
    {
      id: 6,
      name: 'Bank 6',
      transactionTypes: '', // Empty transaction types
      depositMatchingMode: {
        sourceType: '1',
        mappingTypes: [],
        sourceTypeName: 'None'
      }
    }
  ];

  describe('hasDepositTransactionType', () => {
    it('should return true for bank with manual deposit', () => {
      expect(hasDepositTransactionType('1', mockBanks)).toBe(true);
    });

    it('should return true for bank with auto deposit', () => {
      expect(hasDepositTransactionType('3', mockBanks)).toBe(true);
    });

    it('should return false for bank without deposit types', () => {
      expect(hasDepositTransactionType('4', mockBanks)).toBe(false);
    });

    it('should return false for invalid bank id', () => {
      expect(hasDepositTransactionType('999', mockBanks)).toBe(false);
    });

    it('should return false for empty bank id', () => {
      expect(hasDepositTransactionType('', mockBanks)).toBe(false);
    });

    it('should handle transaction types with whitespace', () => {
      expect(hasDepositTransactionType('5', mockBanks)).toBe(true);
    });

    it('should handle null transaction types', () => {
      expect(hasDepositTransactionType('6', mockBanks)).toBe(false);
    });
  });

  describe('hasManualDepositTransactionType', () => {
    it('should return true for bank with manual deposit', () => {
      expect(hasManualDepositTransactionType('1', mockBanks)).toBe(true);
    });

    it('should return false for bank without manual deposit', () => {
      expect(hasManualDepositTransactionType('3', mockBanks)).toBe(false);
    });

    it('should return false for invalid bank id', () => {
      expect(hasManualDepositTransactionType('999', mockBanks)).toBe(false);
    });

    it('should handle transaction types with whitespace', () => {
      expect(hasManualDepositTransactionType('5', mockBanks)).toBe(true);
    });

    it('should handle null transaction types', () => {
      expect(hasManualDepositTransactionType('6', mockBanks)).toBe(false);
    });

    it('should return false for empty bank id', () => {
      expect(hasManualDepositTransactionType('', mockBanks)).toBe(false);
    });
  });

  describe('hasAutoDepositTransactionType', () => {
    it('should return true for bank with auto deposit', () => {
      expect(hasAutoDepositTransactionType('3', mockBanks)).toBe(true);
    });

    it('should return false for bank without auto deposit', () => {
      expect(hasAutoDepositTransactionType('2', mockBanks)).toBe(false);
    });

    it('should return false for invalid bank id', () => {
      expect(hasAutoDepositTransactionType('999', mockBanks)).toBe(false);
    });

    it('should handle transaction types with whitespace', () => {
      expect(hasAutoDepositTransactionType('5', mockBanks)).toBe(true);
    });

    it('should handle null transaction types', () => {
      expect(hasAutoDepositTransactionType('6', mockBanks)).toBe(false);
    });

    it('should return false for empty bank id', () => {
      expect(hasAutoDepositTransactionType('', mockBanks)).toBe(false);
    });
  });

  describe('hasWithdrawalTransactionType', () => {
    it('should return true for bank with manual withdrawal', () => {
      expect(hasWithdrawalTransactionType('1', mockBanks)).toBe(true);
    });

    it('should return false for bank without withdrawal', () => {
      expect(hasWithdrawalTransactionType('3', mockBanks)).toBe(false);
    });

    it('should return false for invalid bank id', () => {
      expect(hasWithdrawalTransactionType('999', mockBanks)).toBe(false);
    });

    it('should handle null transaction types', () => {
      expect(hasWithdrawalTransactionType('6', mockBanks)).toBe(false);
    });

    it('should return false for empty bank id', () => {
      expect(hasWithdrawalTransactionType('', mockBanks)).toBe(false);
    });
  });

  describe('hasManualWithdrawalTransactionType', () => {
    it('should return true for bank with manual withdrawal', () => {
      expect(hasManualWithdrawalTransactionType('1', mockBanks)).toBe(true);
    });

    it('should return false for bank without manual withdrawal', () => {
      expect(hasManualWithdrawalTransactionType('3', mockBanks)).toBe(false);
    });

    it('should return false for invalid bank id', () => {
      expect(hasManualWithdrawalTransactionType('999', mockBanks)).toBe(false);
    });

    it('should handle null transaction types', () => {
      expect(hasManualWithdrawalTransactionType('6', mockBanks)).toBe(false);
    });

    it('should return false for empty bank id', () => {
      expect(hasManualWithdrawalTransactionType('', mockBanks)).toBe(false);
    });
  });
}); 