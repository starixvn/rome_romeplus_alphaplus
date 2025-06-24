import React from 'react';
import { describe, it, expect, vi, beforeEach } from 'vitest';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import '@testing-library/jest-dom';
import ManualAddWithdrawalForm from '../ManualAddWithdrawalForm';
import { transactionAPI } from '../../../../../services/transactionsHistoryAPI';
import * as pendingWithdrawalAPI from '../../../../../services/pendingWithdrawalAPI';

vi.mock('react-i18next', () => ({
  useTranslation: () => ({
    t: (key: string) => key,
  }),
}));

vi.mock('../../../../../services/transactionsHistoryAPI', () => ({
  transactionAPI: {
    getMemberLoginName: vi.fn(),
    getCashMemberBankInfo: vi.fn(),
    getCashAgentBankInfo: vi.fn(),
    getCashAgentProvider: vi.fn(),
  },
}));

vi.mock('../../../../../services/pendingWithdrawalAPI', () => ({
  getSettingLimitAmtWithdrawal: vi.fn(),
  manualAddWithdrawal: vi.fn(),
  getWithdrawalMemberCashBalance: vi.fn(),
}));

vi.mock('../../../Components/UploadImage', () => ({
  default: ({ setImage }) => (
    <div data-testid="upload-image" onClick={() => setImage && setImage('test-image')}>
      Upload Image Component
    </div>
  ),
}));

vi.mock('../../../../../components/ButtonComponents', () => ({
  ButtonCancelComponent: ({ onClick }) => <button data-testid="cancel-button" onClick={onClick}>Cancel</button>,
  ButtonApplyComponent: ({ onClick }) => <button data-testid="apply-button" onClick={onClick}>Apply</button>,
  BackButtonComponent: ({ onClick }) => <button data-testid="back-button" onClick={onClick}>Back</button>,
}));

vi.mock('../../../../../components/CustomDropdownComponent', () => ({
  default: ({ value, onChange, options, placeholder }) => {
    const testId = placeholder ? `dropdown-${placeholder}` : 'dropdown-component';
    return (
      <select
        data-testid={testId}
        value={value}
        onChange={(e) => onChange && onChange({ value: e.target.value })}
      >
        {options.map((option: any) => (
          <option key={option.value} value={option.value}>
            {option.label}
          </option>
        ))}
      </select>
    );
  },
}));

describe('ManualAddWithdrawalForm', () => {
  const mockMemberLoginOptions = [
    { custId: 1, username: 'user1', nickName: 'nick1' },
    { custId: 2, username: 'user2', nickName: 'nick2' },
  ];

  const mockAgentBankInfo = [
    { bankId: 1, bankCode: 'BANK1', bankAccountNumber: '1234', bankName: 'Bank One' },
  ];

  const mockProviderInfo = [
    { providerId: 1, providerName: 'Provider1', paymentMethodId: 1 },
  ];

  const mockMemberBankInfo = [
    { bankId: 2, bankCode: 'BANK2', bankAccountNumber: '5678', bankName: 'Bank Two' },
  ];

  const defaultProps = {
    onClose: vi.fn(),
  };

  beforeEach(() => {
    vi.clearAllMocks();
    
    (transactionAPI.getMemberLoginName as any).mockResolvedValue(mockMemberLoginOptions);
    (transactionAPI.getCashAgentBankInfo as any).mockResolvedValue([
      { bankId: 1, bankCode: 'BANK1', bankAccountNumber: '1234', bankName: 'Bank One', providerId: 1, paymentMethodId: 1 }
    ]);
    (transactionAPI.getCashAgentProvider as any).mockResolvedValue(mockProviderInfo);
    (transactionAPI.getCashMemberBankInfo as any).mockResolvedValue(mockMemberBankInfo);
    (pendingWithdrawalAPI.getSettingLimitAmtWithdrawal as any).mockResolvedValue({
      minAmt: 100,
      maxAmt: 10000,
    });
    (pendingWithdrawalAPI.getWithdrawalMemberCashBalance as any).mockResolvedValue(5000);
    (pendingWithdrawalAPI.manualAddWithdrawal as any).mockResolvedValue({ isSuccessful: true });
  });

  it('renders and loads initial data correctly', async () => {
    render(<ManualAddWithdrawalForm {...defaultProps} />);

    await waitFor(() => {
      expect(transactionAPI.getMemberLoginName).toHaveBeenCalled();
      expect(transactionAPI.getCashAgentBankInfo).toHaveBeenCalled();
      expect(transactionAPI.getCashAgentProvider).toHaveBeenCalled();
      expect(transactionAPI.getCashMemberBankInfo).toHaveBeenCalled();
    });

    expect(screen.getByTestId('upload-image')).toBeInTheDocument();
  });

  it('handles form submission with valid data', async () => {
    render(<ManualAddWithdrawalForm {...defaultProps} />);

    await waitFor(() => {
      expect(screen.getAllByTestId('dropdown-component')[0]).toBeInTheDocument();
    });

    const memberDropdown = screen.getByTestId('dropdown-manualWithdrawalForm.selectMember');
    fireEvent.change(memberDropdown, { target: { value: mockMemberLoginOptions[0].custId.toString() } });

    const fromDropdown = screen.getByTestId('dropdown-component');
    fireEvent.change(fromDropdown, { target: { value: mockAgentBankInfo[0].bankId.toString() } });

    const toDropdown = screen.getByTestId('dropdown-component');
    fireEvent.change(toDropdown, { target: { value: mockMemberBankInfo[0].bankId.toString() } });

    const amountInput = document.getElementById('withdrawalAmount') as HTMLInputElement;
    fireEvent.change(amountInput, { target: { value: '1000' } });

    const submitButton = screen.getByTestId('apply-button');
    fireEvent.click(submitButton);

    await waitFor(() => {
      expect(pendingWithdrawalAPI.manualAddWithdrawal).toHaveBeenCalledWith(
        expect.objectContaining({
          custId: mockMemberLoginOptions[0].custId,
          paymentFrom: expect.any(String),
          paymentTo: expect.any(String),
          amount: 1000,
        })
      );
    });
  });

  it('displays validation error for amount exceeding member cash balance', async () => {
    render(<ManualAddWithdrawalForm {...defaultProps} />);

    await waitFor(() => {
      expect(screen.getByTestId('dropdown-manualWithdrawalForm.selectMember')).toBeInTheDocument();
    });

    const amountInput = document.getElementById('withdrawalAmount') as HTMLInputElement;
    fireEvent.change(amountInput, { target: { value: '6000' } });

    const submitButton = screen.getByTestId('apply-button');
    fireEvent.click(submitButton);

    expect(screen.getByText('manualWithdrawalForm.amountRangeError')).toBeInTheDocument();
  });

  it('handles API error during form submission', async () => {
    (pendingWithdrawalAPI.manualAddWithdrawal as any).mockRejectedValue(new Error('API Error'));

    render(<ManualAddWithdrawalForm {...defaultProps} />);

    await waitFor(() => {
      expect(screen.getByTestId('dropdown-manualWithdrawalForm.selectMember')).toBeInTheDocument();
    });

    const amountInput = document.getElementById('withdrawalAmount') as HTMLInputElement;
    fireEvent.change(amountInput, { target: { value: '1000' } });

    const submitButton = screen.getByTestId('apply-button');
    fireEvent.click(submitButton);

    await waitFor(() => {
      expect(screen.getByText('manualWithdrawalForm.error')).toBeInTheDocument();
    });
  });

  it('closes form when cancel button is clicked', async () => {
    render(<ManualAddWithdrawalForm {...defaultProps} />);

    const cancelButton = screen.getByTestId('cancel-button');
    fireEvent.click(cancelButton);

    expect(defaultProps.onClose).toHaveBeenCalled();
  });

  it('validates image upload size and type', async () => {
    render(<ManualAddWithdrawalForm {...defaultProps} />);

    await waitFor(() => {
      expect(screen.getByTestId('upload-image')).toBeInTheDocument();
    });

    const uploadComponent = screen.getByTestId('upload-image');
    fireEvent.click(uploadComponent);

    const submitButton = screen.getByTestId('apply-button');
    fireEvent.click(submitButton);

    expect(screen.queryByText('manualWithdrawalForm.imageSizeError')).not.toBeInTheDocument();
  });
});
