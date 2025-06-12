import React from 'react';
import { describe, it, expect, vi, beforeEach } from 'vitest';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import '@testing-library/jest-dom';
import DataGrid from '../DataGrid';
import { TransactionItem } from '../../../../../services/interfaces/Transactions/TransactionHistory';

// Mock react-i18next
vi.mock('react-i18next', () => ({
  useTranslation: () => ({
    t: (key: string) => key,
  }),
}));

// Mock TransactionDetail component
vi.mock('./TransactionDetail', () => ({
  default: ({ onClose }: { onClose: () => void }) => (
    <div data-testid="transaction-detail-modal" onClick={onClose}>
      Mock Transaction Detail Modal
    </div>
  ),
}));

// Mock common functions
vi.mock('../../../../utils/commonFunctions', () => ({
  displayAmount: (amount: number) => amount.toString(),
  renderBankAccount: (bank: string) => bank,
  formatDateTime: (date: string) => date,
  CurrentCurrency: 'USD',
  mappingStatus: (statusId: number) => `Status ${statusId}`,
}));

describe('DataGrid', () => {
  const mockTransactions: TransactionItem[] = [
    {
      transId: 1,
      custId: 1001,
      username: 'testuser1',
      nickname: 'Test1',
      firstName: 'John',
      lastName: 'Doe',
      amount: 1000,
      currency: 'USD',
      paymentFrom: 'Bank A',
      paymentTo: 'Bank B',
      statusId: 1,
      betCredit: 0,
      transactionTypeId: 1,
      transactionType: 'Deposit',
      remark: '',
      payslip: '',
      base64Payslip: '',
      createdTime: '2024-01-01T10:00:00',
      lastUpdatedDate: '2024-01-01T10:00:00',
      isAllowRevert: false,
      operatedBy: 'system',
      operatedTime: '2024-01-01T10:00:00',
    },
    {
      transId: 2,
      custId: 1002,
      username: 'testuser2',
      nickname: 'Test2',
      firstName: 'Jane',
      lastName: 'Smith',
      amount: 2000,
      currency: 'USD',
      paymentFrom: 'Bank C',
      paymentTo: 'Bank D',
      statusId: 2,
      betCredit: 0,
      transactionTypeId: 1,
      transactionType: 'Deposit',
      remark: '',
      payslip: '',
      base64Payslip: '',
      createdTime: '2024-01-01T11:00:00',
      lastUpdatedDate: '2024-01-01T11:00:00',
      isAllowRevert: false,
      operatedBy: 'system',
      operatedTime: '2024-01-01T11:00:00',
    },
  ];

  const defaultProps = {
    items: mockTransactions,
    pageIndex: 1,
    pageSize: 10,
    isLoading: false,
    transactionType: 1,
    columnFilter: ['All'],
  };

  beforeEach(() => {
    vi.clearAllMocks();
  });

  it('renders loading component when isLoading is true', () => {
    const { container } = render(<DataGrid {...defaultProps} isLoading={true} />);
    expect(container.querySelector('.loader')).toBeInTheDocument();
  });

  it('renders the data grid with transactions correctly', () => {
    render(<DataGrid {...defaultProps} />);
    
    expect(screen.getByText('testuser1')).toBeInTheDocument();
    expect(screen.getByText('Test1')).toBeInTheDocument();
  });

  it('shows no records found message when transactions array is empty', () => {
    render(<DataGrid {...defaultProps} items={[]} />);
    expect(screen.getByText('table.noRecords')).toBeInTheDocument();
  });

  it('respects column filter settings', () => {
    render(<DataGrid {...defaultProps} columnFilter={['username', 'amount']} />);
    
    expect(screen.getByText('table.username')).toBeInTheDocument();
    expect(screen.getByText(/table\.amount/i)).toBeInTheDocument();
    
    expect(screen.queryByText('table.from')).not.toBeInTheDocument();
    expect(screen.queryByText('table.to')).not.toBeInTheDocument();
  });

  it('displays correct pagination information', () => {
    render(<DataGrid {...defaultProps} pageIndex={2} pageSize={5} />);
    
    const firstRowNumber = screen.getByText('6');
    expect(firstRowNumber).toBeInTheDocument();
  });
});
