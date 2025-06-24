import React from 'react';
import { describe, it, expect, vi, beforeEach } from 'vitest';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import '@testing-library/jest-dom';
import PendingWithdrawalModal from '../PendingWithdrawalModal';
import { approveWithdrawal, rejectWithdrawal } from '../../../../../services/pendingWithdrawalAPI';
import { transactionAPI } from '../../../../../services/transactionsHistoryAPI';

// Mock react-i18next
vi.mock('react-i18next', () => ({
  useTranslation: () => ({
    t: (key: string) => key,
  }),
}));

// Mock withdrawal API
vi.mock('../../../../../services/pendingWithdrawalAPI', () => ({
  approveWithdrawal: vi.fn(),
  rejectWithdrawal: vi.fn(),
}));

// Mock transaction API
vi.mock('../../../../../services/transactionsHistoryAPI', () => ({
  transactionAPI: {
    getTransactionDetail: vi.fn(),
    getCashAgentProvider: vi.fn(),
    getCashAgentBankInfo: vi.fn(),
  },
}));

describe('PendingWithdrawalModal', () => {
  const mockOnClose = vi.fn();
  
  const mockWithdrawal = {
    transId: 123,
    custId: 1,
    username: 'testUser',
    nickname: 'Test Nick',
    firstName: 'John',
    lastName: 'Doe',
    betCredit: 1000,
    amount: 500,
    currency: 'USD',
    paymentFrom: 'BANK1-Test Bank',
    paymentTo: 'BANK2-Destination Bank',
    statusId: 1,
    transactionTypeId: 2,
    transactionType: 'Withdrawal',
    createdTime: '2024-03-15T10:00:00',
    lastUpdatedDate: '2024-03-15T10:00:00',
    isAllowRevert: false,
    operatedBy: 'admin',
    operatedTime: '2024-03-15T10:00:00',
    remark: 'Test remark'
  };

  const mockBankOptions = [
    {
      value: 'BANK1-123456',
      label: '123456',
      code: 'BANK1'
    },
    {
      value: 'BANK2-789012',
      label: '789012',
      code: 'BANK2'
    }
  ];

  const defaultProps = {
    transId: mockWithdrawal.transId,
    custId: mockWithdrawal.custId,
    onClose: mockOnClose,
  };

  beforeEach(() => {
    vi.clearAllMocks();
    vi.mocked(transactionAPI.getTransactionDetail).mockResolvedValue({
      error: false,
      data: mockWithdrawal
    });
    vi.mocked(transactionAPI.getCashAgentProvider).mockResolvedValue([
      { providerId: 1, paymentMethodId: 1, providerName: 'Provider1', methodName: 'Provider1' }
    ]);
    vi.mocked(transactionAPI.getCashAgentBankInfo).mockResolvedValue([
      { bankId: 1, bankCode: 'BANK1', bankAccountNumber: '123456' },
      { bankId: 2, bankCode: 'BANK2', bankAccountNumber: '789012' }
    ]);
  });

  it('renders modal with withdrawal information correctly', async () => {
    render(<PendingWithdrawalModal {...defaultProps} />);

    await waitFor(() => {
      expect(screen.getByText(`pendingWithdrawalModal.title (${mockWithdrawal.transId})`)).toBeInTheDocument();
      expect(screen.getByText(mockWithdrawal.username)).toBeInTheDocument();
      expect(screen.getByText(mockWithdrawal.nickname)).toBeInTheDocument();
      expect(screen.getByText(`${mockWithdrawal.firstName} ${mockWithdrawal.lastName}`)).toBeInTheDocument();
    });
  });

  it('handles approve withdrawal successfully', async () => {
    vi.mocked(approveWithdrawal).mockResolvedValue({
      isSuccessful: true,
      code: 200,
      message: 'Success'
    });

    render(<PendingWithdrawalModal {...defaultProps} />);

    await waitFor(() => {
      expect(screen.getByText(`pendingWithdrawalModal.title (${mockWithdrawal.transId})`)).toBeInTheDocument();
    });

    const bankDropdown = screen.getAllByText('pendingWithdrawalModal.selectBank')[0];
    fireEvent.click(bankDropdown);
    const option = screen.getByText(mockBankOptions[0].label);
    fireEvent.click(option);

    const approveButton = screen.getByText('pendingWithdrawalModal.approve');
    fireEvent.click(approveButton);

    await waitFor(() => {
      expect(screen.getByText('pendingWithdrawalModal.approveSuccess')).toBeInTheDocument();
    });

    expect(approveWithdrawal).toHaveBeenCalledWith({
      transId: mockWithdrawal.transId,
      paymentFrom: mockBankOptions[0].value,
      remark: mockWithdrawal.remark,
      image: null
    });
  });

  it('handles reject withdrawal successfully', async () => {
    vi.mocked(rejectWithdrawal).mockResolvedValue({
      isSuccessful: true,
      code: 200,
      message: 'Success'
    });

    render(<PendingWithdrawalModal {...defaultProps} />);

    await waitFor(() => {
      expect(screen.getByText(`pendingWithdrawalModal.title (${mockWithdrawal.transId})`)).toBeInTheDocument();
    });

    const bankDropdown = screen.getAllByText('pendingWithdrawalModal.selectBank')[0];
    fireEvent.click(bankDropdown);
    const option = screen.getByText(mockBankOptions[0].label);
    fireEvent.click(option);

    const rejectButton = screen.getByText('pendingWithdrawalModal.reject');
    fireEvent.click(rejectButton);

    await waitFor(() => {
      expect(screen.getByText('pendingWithdrawalModal.rejectSuccess')).toBeInTheDocument();
    });

    expect(rejectWithdrawal).toHaveBeenCalledWith({
      transId: mockWithdrawal.transId,
      paymentFrom: mockBankOptions[0].value,
      remark: mockWithdrawal.remark,
      image: null
    });
  });

  it('shows error when trying to approve without selecting bank', async () => {
    render(<PendingWithdrawalModal {...defaultProps} />);

    await waitFor(() => {
      expect(screen.getByText(`pendingWithdrawalModal.title (${mockWithdrawal.transId})`)).toBeInTheDocument();
    });

    const approveButton = screen.getByText('pendingWithdrawalModal.approve');
    fireEvent.click(approveButton);

    expect(screen.getAllByText('pendingWithdrawalModal.selectBank')[0]).toBeInTheDocument();
    expect(approveWithdrawal).not.toHaveBeenCalled();
  });

  it('handles API error responses', async () => {
    vi.mocked(approveWithdrawal).mockResolvedValue({
      isSuccessful: false,
      code: 400,
      message: 'Error message'
    });

    render(<PendingWithdrawalModal {...defaultProps} />);

    await waitFor(() => {
      expect(screen.getByText(`pendingWithdrawalModal.title (${mockWithdrawal.transId})`)).toBeInTheDocument();
    });

    const bankDropdown = screen.getAllByText('pendingWithdrawalModal.selectBank')[0];
    fireEvent.click(bankDropdown);
    const option = screen.getByText(mockBankOptions[0].label);
    fireEvent.click(option);

    const approveButton = screen.getByText('pendingWithdrawalModal.approve');
    fireEvent.click(approveButton);

    await waitFor(() => {
      expect(screen.getByText('pendingWithdrawalModal.approveError')).toBeInTheDocument();
    });
  });

  it('handles close button click', () => {
    render(<PendingWithdrawalModal {...defaultProps} />);

    const closeButton = screen.getByRole('button', { name: '' });
    fireEvent.click(closeButton);

    expect(mockOnClose).toHaveBeenCalled();
  });

  it('disables buttons while submitting', async () => {
    vi.mocked(approveWithdrawal).mockImplementation(
      () => new Promise(resolve => setTimeout(resolve, 100))
    );

    render(<PendingWithdrawalModal {...defaultProps} />);

    await waitFor(() => {
      expect(screen.getByText(`pendingWithdrawalModal.title (${mockWithdrawal.transId})`)).toBeInTheDocument();
    });

    const bankDropdown = screen.getAllByText('pendingWithdrawalModal.selectBank')[0];
    fireEvent.click(bankDropdown);
    const option = screen.getByText(mockBankOptions[0].label);
    fireEvent.click(option);

    const approveButton = screen.getByText('pendingWithdrawalModal.approve');
    const rejectButton = screen.getByText('pendingWithdrawalModal.reject');

    fireEvent.click(approveButton);

    expect(approveButton).toBeDisabled();
    expect(rejectButton).toBeDisabled();
  });
});
