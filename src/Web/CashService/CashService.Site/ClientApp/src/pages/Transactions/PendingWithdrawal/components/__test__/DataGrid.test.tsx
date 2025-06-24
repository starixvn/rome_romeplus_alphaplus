import React from 'react';
import { describe, it, expect, vi, beforeEach } from 'vitest';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import '@testing-library/jest-dom';
import DataGrid from '../DataGrid';
import { TransactionItem } from '../../../../../services/interfaces/Transactions/TransactionHistory';

vi.mock('react-i18next', () => ({
  useTranslation: () => ({
    t: (key: string) => key,
  }),
}));

vi.mock('../PendingWithdrawalModal', () => ({
  default: ({ onClose }: { onClose: () => void }) => (
    <div data-testid="pending-withdrawal-modal" onClick={onClose}>
      Mock Modal
    </div>
  ),
}));

vi.mock('../../../../utils/commonFunctions', () => ({
  displayAmount: (amount: number) => amount.toString(),
  renderBankAccount: (bank: string) => bank,
  formatDateTime: (date: string) => date,
}));

describe('DataGrid', () => {
  const mockWithdrawals: TransactionItem[] = [
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
      transactionTypeId: 2,
      transactionType: 'Withdrawal',
      remark: '',
      payslip: '',
      base64Payslip: '',
      createdTime: '2024-01-01T10:00:00',
      lastUpdatedDate: '2024-01-01T10:00:00',
      isAllowRevert: false,
      createdBy: 'system',
      operatedTime: '2024-01-01T10:00:00',
      isHadPayslip: false,
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
      transactionTypeId: 2,
      transactionType: 'Withdrawal',
      remark: '',
      payslip: '',
      base64Payslip: '',
      createdTime: '2024-01-01T11:00:00',
      lastUpdatedDate: '2024-01-01T11:00:00',
      isAllowRevert: false,
      createdBy: 'system',
      operatedTime: '2024-01-01T11:00:00',
      isHadPayslip: false,
    },
  ];

  beforeEach(() => {
    vi.clearAllMocks();
  });

  it('renders the data grid with withdrawals correctly', () => {
    render(<DataGrid withdrawals={mockWithdrawals} reloadGrid={vi.fn()} />);

    expect(screen.getByText('testuser1')).toBeInTheDocument();
    expect(screen.getByText('Test1')).toBeInTheDocument();
    expect(screen.getByText('John')).toBeInTheDocument();
    expect(screen.getByText('Doe')).toBeInTheDocument();
    expect(screen.getByText('Bank A')).toBeInTheDocument();
    expect(screen.getByText('Bank B')).toBeInTheDocument();
  });

  it('shows no records found message when withdrawals array is empty', () => {
    render(<DataGrid withdrawals={[]} reloadGrid={vi.fn()} />);
    expect(screen.getByText('dataGrid.noWithdrawalsFound')).toBeInTheDocument();
  });

  it('handles process button click successfully', async () => {
    render(<DataGrid withdrawals={mockWithdrawals} reloadGrid={vi.fn()} />);

    const processButton = screen.getAllByText('dataGrid.process')[0];
    await fireEvent.click(processButton);

    await waitFor(() => {
      expect(screen.getByTestId('pending-withdrawal-modal')).toBeInTheDocument();
    });
  });

  it('closes the modal when clicking close', async () => {
    render(<DataGrid withdrawals={mockWithdrawals} reloadGrid={vi.fn()} />);

    const processButton = screen.getAllByText('dataGrid.process')[0];
    await fireEvent.click(processButton);

    const modal = await screen.findByTestId('pending-withdrawal-modal');
    fireEvent.click(modal);

    await waitFor(() => {
      expect(screen.queryByTestId('pending-withdrawal-modal')).not.toBeInTheDocument();
    });
  });

  it('handles pagination correctly', async () => {
    const manyWithdrawals = Array(15)
      .fill(null)
      .map((_, index) => ({
        ...mockWithdrawals[0],
        transId: index + 1,
        custId: 1000 + index + 1,
        username: `testuser${index + 1}`,
        nickname: `Test${index + 1}`,
      }));

    render(<DataGrid withdrawals={manyWithdrawals} reloadGrid={vi.fn()} />);

    await waitFor(() => {
      expect(screen.getByText('testuser1')).toBeInTheDocument();
    });
  });

  it('handles page size change', async () => {
    const manyWithdrawals = Array(15)
      .fill(null)
      .map((_, index) => ({
        ...mockWithdrawals[0],
        transId: index + 1,
        custId: 1000 + index + 1,
        username: `testuser${index + 1}`,
        nickname: `Test${index + 1}`,
        amount: 1000 + index,
        paymentFrom: `Bank ${index + 1}A`,
        paymentTo: `Bank ${index + 1}B`,
      }));

    render(<DataGrid withdrawals={manyWithdrawals} reloadGrid={vi.fn()} />);

    const table = await screen.findByRole('table');
    expect(table).toBeInTheDocument();

    const findUsernameCells = async () => {
      const rows = screen.getAllByRole('row');
      const usernameCells = rows
        .slice(1)
        .map((row) => row.querySelectorAll('td')[1])
        .map((cell) => cell?.querySelector('div')?.textContent || '');
      return [usernameCells[0], usernameCells[usernameCells.length - 1]];
    };

    const initialCells = await findUsernameCells();
    expect(initialCells[0]).toBe('testuser1');

    const pageSizeSelect = screen.getByRole('combobox');
    await fireEvent.change(pageSizeSelect, { target: { value: '20' } });
  });
});
