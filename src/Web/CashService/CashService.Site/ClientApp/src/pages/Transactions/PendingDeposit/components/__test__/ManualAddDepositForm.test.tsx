import React from 'react';
import { describe, it, expect, vi, beforeEach } from 'vitest';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import '@testing-library/jest-dom';
import ManualAddDepositForm from '../ManualAddDepositForm';
import { transactionAPI } from '../../../../../services/transactionsHistoryAPI';
import pendingDepositAPI from '../../../../../services/pendingDepositAPI';

vi.mock('react-i18next', () => ({
  useTranslation: () => ({
    t: (key: string) => key,
  }),
}));

vi.mock('../../../../../services/transactionsHistoryAPI', () => ({
  transactionAPI: {
    getMemberLoginName: vi.fn(),
    getCashMemberBankInfo: vi.fn(),
  },
}));

vi.mock('../../../../../services/pendingDepositAPI', () => ({
  default: {
    getLimitAmtDeposit: vi.fn(),
    createDepositManual: vi.fn(),
  },
}));

vi.mock('../../../Components/UploadImage', () => ({
  default: ({ setImage }) => <div data-testid="upload-image" onClick={() => setImage && setImage('test-image')}>Upload Image Component</div>,
}));

vi.mock('../AccountInformation', () => ({
  default: ({ 
    memberLoginOptions,
    memberId,
    onMemberChange,
    paymentFrom,
    onPaymentFromChange,
    cashMemberOptions,
    paymentTo,
    onPaymentToChange,
    cashAgentOptions,
    depositAmount,
    onDepositAmountChange,
    agentAvailableCredit,
    remarks,
    onRemarksChange 
  }) => (
    <div data-testid="account-information">
      <select 
        data-testid="member-select" 
        value={memberId} 
        onChange={(e) => onMemberChange({ value: e.target.value })}
      >
        {memberLoginOptions.map((option) => (
          <option key={option.custId} value={option.custId}>{option.loginName}</option>
        ))}
      </select>
      <input 
        data-testid="deposit-amount"
        type="number"
        value={depositAmount}
        onChange={(e) => onDepositAmountChange(Number(e.target.value))}
      />
      <input
        data-testid="remarks"
        value={remarks}
        onChange={(e) => onRemarksChange(e)}
      />
    </div>
  ),
}));

vi.mock('../../../../../components/ButtonComponents', () => ({
  ButtonCancelComponent: ({ onClick }) => <button data-testid="cancel-button" onClick={onClick}>Cancel</button>,
  ButtonApplyComponent: ({ onClick }) => <button data-testid="apply-button" onClick={onClick}>Apply</button>,
  BackButtonComponent: ({ onClick }) => <button data-testid="back-button" onClick={onClick}>Back</button>,
}));

vi.mock('../../../../../components/LoadingComponent', () => ({
  LoadingFullComponent: () => <div data-testid="loading-component">Loading...</div>
}));

describe('ManualAddDepositForm', () => {
  const mockCashAgentOptions = [
    { bankId: 1, bankCode: 'BANK1', bankAccountNumber: '1234', bankName: 'Bank One' },
    { bankId: 2, bankCode: 'BANK2', bankAccountNumber: '5678', bankName: 'Bank Two' },
  ];

  const mockMemberLoginOptions = [
    { custId: 1, loginName: 'user1' },
    { custId: 2, loginName: 'user2' },
  ];

  const mockCashMemberBankInfo = [
    { bankId: 3, bankCode: 'BANK3', bankAccountNumber: '9012', bankName: 'Bank Three' },
  ];

  const defaultProps = {
    cashAgentOptions: mockCashAgentOptions,
    onClose: vi.fn(),
  };

  beforeEach(() => {
    vi.clearAllMocks();
    
    (transactionAPI.getMemberLoginName as any).mockResolvedValue(mockMemberLoginOptions);
    (transactionAPI.getCashMemberBankInfo as any).mockResolvedValue(mockCashMemberBankInfo);
    (pendingDepositAPI.getLimitAmtDeposit as any).mockResolvedValue(10000);
    (pendingDepositAPI.createDepositManual as any).mockResolvedValue({ isSuccessful: true });
  });

  it('renders loading state initially', () => {
    render(<ManualAddDepositForm {...defaultProps} />);
    expect(screen.getByTestId('loading-component')).toBeInTheDocument();
  });

  it('loads and displays initial data correctly', async () => {
    render(<ManualAddDepositForm {...defaultProps} />);

    expect(screen.getByTestId('loading-component')).toBeInTheDocument();

    await waitFor(() => {
      expect(screen.queryByTestId('loading-component')).not.toBeInTheDocument();
    });

    expect(transactionAPI.getMemberLoginName).toHaveBeenCalled();
    expect(transactionAPI.getCashMemberBankInfo).toHaveBeenCalled();
    expect(pendingDepositAPI.getLimitAmtDeposit).toHaveBeenCalled();

    expect(screen.getByTestId('account-information')).toBeInTheDocument();
    expect(screen.getByTestId('upload-image')).toBeInTheDocument();
  });

  it('handles form submission with valid data', async () => {
    render(<ManualAddDepositForm {...defaultProps} />);

    await waitFor(() => {
      expect(screen.queryByTestId('loading-component')).not.toBeInTheDocument();
    });

    const depositAmountInput = screen.getByTestId('deposit-amount');
    fireEvent.change(depositAmountInput, { target: { value: '1000' } });

    const remarksInput = screen.getByTestId('remarks');
    fireEvent.change(remarksInput, { target: { value: 'Test remarks' } });

    const submitButton = screen.getByTestId('apply-button');
    fireEvent.click(submitButton);

    await waitFor(() => {
      expect(pendingDepositAPI.createDepositManual).toHaveBeenCalled();
    });
  });

  it('displays validation error for invalid deposit amount', async () => {
    render(<ManualAddDepositForm {...defaultProps} />);

    await waitFor(() => {
      expect(screen.queryByTestId('loading-component')).not.toBeInTheDocument();
    });

    const depositAmountInput = screen.getByTestId('deposit-amount');
    fireEvent.change(depositAmountInput, { target: { value: '-100' } });

    const submitButton = screen.getByTestId('apply-button');
    fireEvent.click(submitButton);

    expect(screen.getByText('manualDepositForm.amountMustBeGreaterThan0AndLessThanMaxLimit')).toBeInTheDocument();
  });

  it('handles API error during form submission', async () => {
    (pendingDepositAPI.createDepositManual as any).mockRejectedValue(new Error('API Error'));

    render(<ManualAddDepositForm {...defaultProps} />);

    await waitFor(() => {
      expect(screen.queryByTestId('loading-component')).not.toBeInTheDocument();
    });

    const depositAmountInput = screen.getByTestId('deposit-amount');
    fireEvent.change(depositAmountInput, { target: { value: '1000' } });

    const submitButton = screen.getByTestId('apply-button');
    fireEvent.click(submitButton);

    await waitFor(() => {
      expect(screen.getByText('errorMessages.failedToSubmit')).toBeInTheDocument();
    });
  });

  it('handles member change correctly', async () => {
    render(<ManualAddDepositForm {...defaultProps} />);

    await waitFor(() => {
      expect(screen.queryByTestId('loading-component')).not.toBeInTheDocument();
    });

    const memberSelect = screen.getByTestId('member-select');
    fireEvent.change(memberSelect, { target: { value: '2' } });

    await waitFor(() => {
      expect(transactionAPI.getCashMemberBankInfo).toHaveBeenCalledWith(2);
      expect(pendingDepositAPI.getLimitAmtDeposit).toHaveBeenCalledWith(2);
    });
  });

  it('closes form when cancel button is clicked', async () => {
    render(<ManualAddDepositForm {...defaultProps} />);

    await waitFor(() => {
      expect(screen.queryByTestId('loading-component')).not.toBeInTheDocument();
    });

    const cancelButton = screen.getByTestId('cancel-button');
    fireEvent.click(cancelButton);

    expect(defaultProps.onClose).toHaveBeenCalled();
  });
});
