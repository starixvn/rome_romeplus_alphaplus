import React from 'react';
import { describe, it, expect, vi, beforeEach } from 'vitest';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import '@testing-library/jest-dom';
import DataGrid from '../DataGrid';
import { transactionAPI } from '../../../../../services/transactionsHistoryAPI';
import { TransactionItem } from '../../../../../services/interfaces/Transactions/TransactionHistory';

vi.mock('react-i18next', () => ({
  useTranslation: () => ({
    t: (key: string) => key,
  }),
}));

vi.mock('../../../../../services/transactionsHistoryAPI', () => ({
  transactionAPI: {
    getTransactionDetail: vi.fn(),
  },
}));

vi.mock('../PendingDepositModal', () => ({
  default: ({ onClose }: { onClose: () => void }) => (
    <div data-testid="pending-deposit-modal" onClick={onClose}>
      Mock Modal
    </div>
  ),
}));

vi.mock('../../../../../common/commonFunctions', () => ({
  displayAmount: (amount: number) => amount.toString(),
  renderBankAccount: (bank: string) => bank,
  formatDateTime: (date: string) => date,
  CurrentCurrency: 'USD',
}));

describe('DataGrid', () => {
  const mockDeposits: TransactionItem[] = [
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
      statusId: 1,
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

  beforeEach(() => {
    vi.clearAllMocks();
  });

  it('renders the data grid with deposits correctly', () => {
    render(<DataGrid deposits={mockDeposits} />);
    
    expect(screen.getByText('testuser1')).toBeInTheDocument();
    expect(screen.getByText('Test1')).toBeInTheDocument();
    expect(screen.getByText('John')).toBeInTheDocument();
    expect(screen.getByText('Doe')).toBeInTheDocument();
    expect(screen.getByText('Bank A')).toBeInTheDocument();
    expect(screen.getByText('Bank B')).toBeInTheDocument();
    expect(screen.getByText('1000')).toBeInTheDocument();
  });

  it('shows no records found message when deposits array is empty', () => {
    render(<DataGrid deposits={[]} />);
    expect(screen.getByText('dataGrid.noDepositsFound')).toBeInTheDocument();
  });

  it('handles process button click successfully', async () => {
    const mockTransactionDetail = { ...mockDeposits[0] };
    vi.mocked(transactionAPI.getTransactionDetail).mockResolvedValueOnce(mockTransactionDetail);

    render(<DataGrid deposits={mockDeposits} />);

    const processButton = screen.getAllByText('dataGrid.process')[0];
    await fireEvent.click(processButton);

    expect(transactionAPI.getTransactionDetail).toHaveBeenCalledWith(1, 1001);
    await waitFor(() => {
      expect(screen.getByTestId('pending-deposit-modal')).toBeInTheDocument();
    });
  });

  it('displays error message when process button click fails', async () => {
    vi.mocked(transactionAPI.getTransactionDetail).mockRejectedValueOnce(new Error('API Error'));

    render(<DataGrid deposits={mockDeposits} />);

    const processButton = screen.getAllByText('dataGrid.process')[0];
    await fireEvent.click(processButton);

    await waitFor(() => {
      expect(screen.getByText('dataGrid.errorFetchingDetail')).toBeInTheDocument();
    });
  });

  it('closes the modal when clicking close', async () => {
    const mockTransactionDetail = { ...mockDeposits[0] };
    vi.mocked(transactionAPI.getTransactionDetail).mockResolvedValueOnce(mockTransactionDetail);

    render(<DataGrid deposits={mockDeposits} />);

    const processButton = screen.getAllByText('dataGrid.process')[0];
    await fireEvent.click(processButton);

    const modal = await screen.findByTestId('pending-deposit-modal');
    fireEvent.click(modal);

    await waitFor(() => {
      expect(screen.queryByTestId('pending-deposit-modal')).not.toBeInTheDocument();
    });
  });

  it('handles pagination correctly', async () => {
    const manyDeposits = Array(15)
      .fill(null)
      .map((_, index) => ({
        ...mockDeposits[0],
        transId: index + 1,
        custId: 1000 + index + 1,
        username: `testuser${index + 1}`,
        nickname: `Test${index + 1}`,
      }));

    render(<DataGrid deposits={manyDeposits} />);

    await waitFor(() => {
      expect(screen.getByText('testuser1')).toBeInTheDocument();
      expect(screen.queryByText('testuser11')).not.toBeInTheDocument();
    });

    const nextButton = screen.getAllByText('›').find(
      element => !element.getAttribute('style')?.includes('transform: scaleX(-1)')
    );
    expect(nextButton).toBeDefined();
    await fireEvent.click(nextButton!);

    await waitFor(() => {
      expect(screen.queryByText('testuser1')).not.toBeInTheDocument();
      expect(screen.getByText('testuser11')).toBeInTheDocument();
    });
  });

  it('handles page size change', async () => {
    const manyDeposits = Array(15)
      .fill(null)
      .map((_, index) => ({
        ...mockDeposits[0],
        transId: index + 1,
        custId: 1000 + index + 1,
        username: `testuser${index + 1}`,
        nickname: `Test${index + 1}`,
        amount: 1000 + index,
        paymentFrom: `Bank ${index + 1}A`,
        paymentTo: `Bank ${index + 1}B`,
        createdTime: '2024-01-01T10:00:00',
        currency: 'USD',
        statusId: 1,
        betCredit: 0,
        transactionTypeId: 1,
        transactionType: 'Deposit',
        remark: '',
        payslip: '',
        base64Payslip: '',
        lastUpdatedDate: '2024-01-01T10:00:00',
        isAllowRevert: false,
        operatedBy: 'system',
        operatedTime: '2024-01-01T10:00:00',
      }));

    render(<DataGrid deposits={manyDeposits} />);

    const table = await screen.findByRole('table');
    expect(table).toBeInTheDocument();

    const findUsernameCells = async () => {
      const rows = screen.getAllByRole('row');
      const usernameCells = rows
        .slice(1)
        .map(row => row.querySelectorAll('td')[1])
        .map(cell => cell?.querySelector('div')?.textContent || '');
      return [usernameCells[0], usernameCells[usernameCells.length - 1]];
    };

    const initialCells = await findUsernameCells();
    expect(initialCells[0]).toBe('testuser1');

    const pageSizeSelect = screen.getByRole('combobox');
    await fireEvent.change(pageSizeSelect, { target: { value: '20' } });

    const updatedCells = await findUsernameCells();
    expect(updatedCells[0]).toBe('');
    expect(updatedCells[updatedCells.length - 1]).toBe('');
  });
});
