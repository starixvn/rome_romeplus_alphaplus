import React from 'react';
import { describe, it, expect, vi, beforeEach } from 'vitest';
import { render, screen, fireEvent } from '@testing-library/react';
import '@testing-library/jest-dom';
import AccountInformation from '../AccountInformation';
import { BankInfo } from '../../../../../services/interfaces/Transactions/BankInfo';
import { MemberLoginName } from '../../../../../services/interfaces/Transactions/MemberLoginName';

vi.mock('react-i18next', () => ({
  useTranslation: () => ({
    t: (key: string) => key,
  }),
}));

vi.mock('../../../../../components/BankComponents', () => ({
  default: () => <div data-testid="bank-icon">Bank Icon</div>,
}));

vi.mock('../../../../../components/ui/AmountInput', () => ({
  AmountInput: ({ id, value, onChange }) => (
    <input
      type="text"
      id={id}
      data-testid="amount-input"
      value={value}
      onChange={(e) => onChange(parseFloat(e.target.value))}
    />
  ),
}));

describe('AccountInformation', () => {
  const mockOnMemberChange = vi.fn();
  const mockOnPaymentFromChange = vi.fn();
  const mockOnPaymentToChange = vi.fn();
  const mockOnDepositAmountChange = vi.fn();
  const mockOnRemarksChange = vi.fn();

  const mockMemberLoginOptions: MemberLoginName[] = [
    { custId: 1, username: 'user1' },
    { custId: 2, username: 'user2' },
  ];

  const mockCashMemberOptions: BankInfo[] = [
    { bankId: 1, bankCode: 'BANK1', bankAccountNumber: '1234567890' },
    { bankId: 2, bankCode: 'BANK2', bankAccountNumber: '0987654321' },
  ];

  const mockCashAgentOptions: BankInfo[] = [
    { bankId: 3, bankCode: 'BANK3', bankAccountNumber: '11111111' },
    { bankId: 4, bankCode: 'BANK4', bankAccountNumber: '22222222' },
  ];

  const defaultProps = {
    memberLoginOptions: mockMemberLoginOptions,
    memberId: 1,
    onMemberChange: mockOnMemberChange,
    paymentFrom: 'BANK1-1234567890',
    onPaymentFromChange: mockOnPaymentFromChange,
    cashMemberOptions: mockCashMemberOptions,
    paymentTo: 'BANK3-11111111',
    onPaymentToChange: mockOnPaymentToChange,
    cashAgentOptions: mockCashAgentOptions,
    depositAmount: 1000,
    onDepositAmountChange: mockOnDepositAmountChange,
    agentAvailableCredit: 5000,
    remarks: '',
    onRemarksChange: mockOnRemarksChange,
  };

  beforeEach(() => {
    vi.clearAllMocks();
  });

  it('renders all components correctly', () => {
    render(<AccountInformation {...defaultProps} />);

    expect(screen.getByText('manualDepositForm.accountInformation')).toBeInTheDocument();
    expect(screen.getByText('manualDepositForm.memberLoginName')).toBeInTheDocument();
    expect(screen.getByText('manualDepositForm.from')).toBeInTheDocument();
    expect(screen.getByText('manualDepositForm.to')).toBeInTheDocument();
    expect(screen.getByText('manualDepositForm.depositAmount')).toBeInTheDocument();
    expect(screen.getByText('manualDepositForm.remarks')).toBeInTheDocument();

    const amountInput = screen.getByTestId('amount-input');
    expect(amountInput).toBeInTheDocument();

    const remarksTextarea = screen.getByPlaceholderText('manualDepositForm.remarksPlaceholder');
    expect(remarksTextarea).toBeInTheDocument();

    expect(screen.getByText('manualDepositForm.agentAvailableCredit:')).toBeInTheDocument();
  });

  it('handles remarks change', () => {
    render(<AccountInformation {...defaultProps} />);

    const remarksTextarea = screen.getByPlaceholderText('manualDepositForm.remarksPlaceholder');
    fireEvent.change(remarksTextarea, { target: { value: 'Test remarks' } });

    expect(mockOnRemarksChange).toHaveBeenCalled();
  });

  it('handles deposit amount change', () => {
    render(<AccountInformation {...defaultProps} />);

    const amountInput = screen.getByTestId('amount-input');
    fireEvent.change(amountInput, { target: { value: '2000' } });

    expect(mockOnDepositAmountChange).toHaveBeenCalledWith(2000);
  });

  it('displays initial values correctly', () => {
    const propsWithValues = {
      ...defaultProps,
      remarks: 'Initial remarks',
      depositAmount: 2500,
    };

    render(<AccountInformation {...propsWithValues} />);

    const remarksTextarea = screen.getByPlaceholderText('manualDepositForm.remarksPlaceholder') as HTMLTextAreaElement;
    expect(remarksTextarea.value).toBe('Initial remarks');

    expect(screen.getByText(`${propsWithValues.agentAvailableCredit}`)).toBeInTheDocument();
  });

  it('shows correct placeholder for remarks', () => {
    render(<AccountInformation {...defaultProps} />);

    const remarksTextarea = screen.getByPlaceholderText('manualDepositForm.remarksPlaceholder');
    expect(remarksTextarea).toBeInTheDocument();
  });
});
