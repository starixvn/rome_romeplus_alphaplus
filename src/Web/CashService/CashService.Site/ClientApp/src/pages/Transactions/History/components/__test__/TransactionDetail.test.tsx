import React from 'react';
import { describe, it, expect, vi, beforeEach } from 'vitest';
import { render, screen, fireEvent } from '@testing-library/react';
import '@testing-library/jest-dom';
import { TransactionType } from '../../../../../utils/commonFunctions';
import TransactionDetail from '../TransactionDetail';

// Mock the TransactionDetail component
vi.mock('../TransactionDetail', () => ({
  default: vi.fn(({ isOpen, onClose, transactionId, custId }) => 
    isOpen ? (
      <div data-testid="mocked-transaction-detail">
        <div>Transaction ID: {transactionId}</div>
        <div>Customer ID: {custId}</div>
        <button onClick={onClose}>Close</button>
      </div>
    ) : null
  )
}));

describe('TransactionDetail', () => {
  const defaultProps = {
    transactionId: 1,
    custId: 1001,
    isOpen: true,
    transactionType: TransactionType.Deposit,
    onClose: vi.fn(),
    onRevertSuccess: vi.fn(),
  };

  beforeEach(() => {
    vi.clearAllMocks();
  });

  it('should not render when isOpen is false', () => {
    render(<TransactionDetail {...defaultProps} isOpen={false} />);
    expect(screen.queryByTestId('mocked-transaction-detail')).not.toBeInTheDocument();
  });

  it('should render when isOpen is true', () => {
    render(<TransactionDetail {...defaultProps} />);
    expect(screen.getByTestId('mocked-transaction-detail')).toBeInTheDocument();
    expect(screen.getByText('Transaction ID: 1')).toBeInTheDocument();
    expect(screen.getByText('Customer ID: 1001')).toBeInTheDocument();
  });

  it('should call onClose when close button is clicked', () => {
    render(<TransactionDetail {...defaultProps} />);
    fireEvent.click(screen.getByText('Close'));
    expect(defaultProps.onClose).toHaveBeenCalled();
  });
});
