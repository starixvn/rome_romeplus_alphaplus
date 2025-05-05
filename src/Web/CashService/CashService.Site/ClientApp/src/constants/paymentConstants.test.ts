import { describe, it, expect } from 'vitest';
import {
  STATUS_OPTIONS,
  TRANSACTION_TYPES,
  ENABLED_STATUS_OPTIONS,
  BANK_TRANSACTION_TYPES
} from './paymentConstants';

describe('paymentConstants', () => {
  it('STATUS_OPTIONS should have correct values', () => {
    expect(STATUS_OPTIONS).toEqual([
      { id: 0, name: 'All' },
      { id: 1, name: 'Processing' },
      { id: 2, name: 'Completed' },
      { id: 3, name: 'Fail' },
      { id: 4, name: 'Maintenance' }
    ]);
  });

  it('TRANSACTION_TYPES should have correct values', () => {
    expect(TRANSACTION_TYPES).toEqual([
      { id: 0, name: 'All' },
      { id: 1, name: 'Deposit' },
      { id: 2, name: 'Withdrawal' }
    ]);
  });

  it('ENABLED_STATUS_OPTIONS should have correct values', () => {
    expect(ENABLED_STATUS_OPTIONS).toEqual([
      { id: 0, name: 'All' },
      { id: 1, name: 'Enable' },
      { id: 2, name: 'Disable' }
    ]);
  });

  it('BANK_TRANSACTION_TYPES should have correct values', () => {
    expect(BANK_TRANSACTION_TYPES).toEqual([
      { id: 0, name: 'All' },
      { id: 1, name: 'Manual Deposit' },
      { id: 2, name: 'Auto Deposit' },
      { id: 3, name: 'Manual Withdrawal' },
      { id: 4, name: 'Auto Withdrawal' }
    ]);
  });
}); 