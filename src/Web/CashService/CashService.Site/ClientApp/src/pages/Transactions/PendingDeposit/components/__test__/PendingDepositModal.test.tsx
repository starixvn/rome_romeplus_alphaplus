import React from 'react';
import { describe, it, expect, vi, beforeEach } from 'vitest';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import '@testing-library/jest-dom';
import PendingDepositModal from '../PendingDepositModal';
import pendingDepositAPI from '../../../../../services/pendingDepositAPI';
import { transactionAPI } from '../../../../../services/transactionsHistoryAPI';

vi.mock('react-i18next', () => ({
  useTranslation: () => ({
    t: (key: string) => key,
  }),
}));

vi.mock('../../../../../services/pendingDepositAPI', () => ({
  default: {
    approveDeposit: vi.fn(),
    rejectDeposit: vi.fn(),
  },
}));

vi.mock('../../../../../services/transactionsHistoryAPI', () => ({
  transactionAPI: {
    getTransactionDetail: vi.fn(),
  },
}));

describe('PendingDepositModal', () => {
  const mockOnClose = vi.fn();
  
  const mockDeposit = {
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
    transactionTypeId: 1,
    transactionType: 'Deposit',
    createdTime: '2024-03-15T10:00:00',
    lastUpdatedDate: '2024-03-15T10:00:00',
    isAllowRevert: false,
    operatedBy: 'admin',
    operatedTime: '2024-03-15T10:00:00',
    remark: 'Test remark',
    payslip: '',
    base64Payslip: 'base64string'
  };

  const defaultProps = {
    transId: mockDeposit.transId,
    custId: mockDeposit.custId,
    onClose: mockOnClose,
  };

  beforeEach(() => {
    vi.clearAllMocks();
    vi.mocked(transactionAPI.getTransactionDetail).mockResolvedValue({
      error: false,
      data: mockDeposit
    });
  });

  it('renders modal with deposit information correctly', async () => {
    render(<PendingDepositModal {...defaultProps} />);

    await waitFor(() => {
      expect(screen.getByText(`pendingDepositModal.title (${mockDeposit.transId})`)).toBeInTheDocument();
      expect(screen.getByText(mockDeposit.username)).toBeInTheDocument();
      expect(screen.getByText(mockDeposit.nickname)).toBeInTheDocument();
      expect(screen.getByText(`${mockDeposit.firstName} ${mockDeposit.lastName}`)).toBeInTheDocument();
    });
  });

  it('handles approve deposit successfully', async () => {
    vi.mocked(pendingDepositAPI.approveDeposit).mockResolvedValue({
      isSuccessful: true,
      code: 200,
      message: 'Success'
    });

    render(<PendingDepositModal {...defaultProps} />);

    // Wait for initial data loading
    await waitFor(() => {
      expect(screen.getByText(`pendingDepositModal.title (${mockDeposit.transId})`)).toBeInTheDocument();
    });

    const approveButton = screen.getByText('pendingDepositModal.approve');
    fireEvent.click(approveButton);

    await waitFor(() => {
      expect(screen.getByText('successMessages.depositApproved')).toBeInTheDocument();
    });

    expect(pendingDepositAPI.approveDeposit).toHaveBeenCalledWith({
      transId: mockDeposit.transId,
      amount: mockDeposit.amount,
      remark: ''
    });
  });

  it('handles reject deposit successfully', async () => {
    vi.mocked(pendingDepositAPI.rejectDeposit).mockResolvedValue({
      isSuccessful: true,
      code: 200,
      message: 'Success'
    });

    render(<PendingDepositModal {...defaultProps} />);

    // Wait for initial data loading
    await waitFor(() => {
      expect(screen.getByText(`pendingDepositModal.title (${mockDeposit.transId})`)).toBeInTheDocument();
    });

    const rejectButton = screen.getByText('pendingDepositModal.reject');
    fireEvent.click(rejectButton);

    await waitFor(() => {
      expect(screen.getByText('successMessages.depositRejected')).toBeInTheDocument();
    });

    expect(pendingDepositAPI.rejectDeposit).toHaveBeenCalledWith({
      transId: mockDeposit.transId,
      amount: mockDeposit.amount,
      remark: ''
    });
  });

  it('handles API error responses', async () => {
    vi.mocked(pendingDepositAPI.approveDeposit).mockResolvedValue({
      isSuccessful: false,
      code: 400,
      message: 'Error message'
    });

    render(<PendingDepositModal {...defaultProps} />);

    const approveButton = screen.getByText('pendingDepositModal.approve');
    fireEvent.click(approveButton);

    await waitFor(() => {
      expect(screen.getByText('Error message')).toBeInTheDocument();
    });
  });

  it('allows amount modification', async () => {
    render(<PendingDepositModal {...defaultProps} />);

    // Wait for initial data loading
    await waitFor(() => {
      expect(screen.getByText(`pendingDepositModal.title (${mockDeposit.transId})`)).toBeInTheDocument();
    });

    const amountInput = screen.getByPlaceholderText('0');
    fireEvent.change(amountInput, { target: { value: '1000' } });

    expect(amountInput).toHaveValue('1000');
  });

  it('handles close button click', () => {
    render(<PendingDepositModal {...defaultProps} />);

    const closeButton = screen.getByRole('button', { name: '' });
    fireEvent.click(closeButton);

    expect(mockOnClose).toHaveBeenCalledWith(false);
  });

  it('renders payslip image when base64Payslip is provided', async () => {
    render(<PendingDepositModal {...defaultProps} />);

    // Wait for initial data loading
    await waitFor(() => {
      expect(screen.getByText(`pendingDepositModal.title (${mockDeposit.transId})`)).toBeInTheDocument();
    });

    const image = screen.getByAltText(mockDeposit.transId.toString());
    expect(image).toBeInTheDocument();
    expect(image).toHaveAttribute('src', `data:image/png;base64,${mockDeposit.base64Payslip}`);
  });

  it('renders without payslip image when base64Payslip is not provided', () => {
    const propsWithoutPayslip = {
      ...defaultProps,
      deposit: { ...mockDeposit, base64Payslip: '' }
    };

    render(<PendingDepositModal {...propsWithoutPayslip} />);

    const image = screen.queryByAltText(mockDeposit.transId.toString());
    expect(image).not.toBeInTheDocument();
  });

  it('disables buttons while submitting', async () => {
    vi.mocked(pendingDepositAPI.approveDeposit).mockImplementation(
      () => new Promise(resolve => setTimeout(resolve, 100))
    );

    render(<PendingDepositModal {...defaultProps} />);

    const approveButton = screen.getByText('pendingDepositModal.approve');
    const rejectButton = screen.getByText('pendingDepositModal.reject');

    fireEvent.click(approveButton);

    expect(approveButton).toBeDisabled();
    expect(rejectButton).toBeDisabled();
  });
});
