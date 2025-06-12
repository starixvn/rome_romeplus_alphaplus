import React from 'react';
import { describe, it, expect, vi, beforeEach } from 'vitest';
import { render, screen, fireEvent } from '@testing-library/react';
import '@testing-library/jest-dom';
import AccountFilter from '../AccountFilter';
import { TransactionType } from '../../../../../../utils/commonFunctions';

// Mock the react-i18next
vi.mock('react-i18next', () => ({
  useTranslation: () => ({
    t: (key: string) => key,
  }),
}));

// Mock the CustomDropdown component
vi.mock('../../../../../../components/CustomDropdownComponent', () => ({
  default: ({ options, onSelect, disable, initialValue, showOptionAll, searchable }) => (
    <select
      data-testid="custom-dropdown"
      onChange={(e) => onSelect({ value: e.target.value })}
      disabled={disable}
      value={initialValue}
      className={searchable ? 'searchable' : ''}
    >
      {showOptionAll && <option value="">All</option>}
      {options.map((opt: any) => (
        <option key={opt.value} value={opt.value}>
          {opt.label}
        </option>
      ))}
    </select>
  ),
}));

// Mock the BankIcon component
vi.mock('../../../../../../components/BankComponents', () => ({
  default: ({ code }) => <div data-testid="bank-icon">{code}</div>,
}));

describe('AccountFilter', () => {
  const mockOnAccountChange = vi.fn();
  const mockOnAccountsLoaded = vi.fn();

  const defaultProps = {
    account: '',
    transactionType: TransactionType.Deposit,
    paymentInformations: ['BANK1-123456', 'BANK2-789012'],
    onAccountChange: mockOnAccountChange,
    onAccountsLoaded: mockOnAccountsLoaded,
  };

  beforeEach(() => {
    vi.clearAllMocks();
  });

  it('renders correctly for deposit transaction type', () => {
    render(<AccountFilter {...defaultProps} />);
    expect(screen.getByText('filter.to')).toBeInTheDocument();
    expect(screen.getByTestId('custom-dropdown')).toBeInTheDocument();
  });

  it('renders correctly for withdrawal transaction type', () => {
    render(
      <AccountFilter
        {...defaultProps}
        transactionType={TransactionType.Withdrawal}
      />
    );
    expect(screen.getByText('filter.from')).toBeInTheDocument();
  });

  it('handles account selection change', () => {
    render(<AccountFilter {...defaultProps} />);
    
    const dropdown = screen.getByTestId('custom-dropdown');
    fireEvent.change(dropdown, { target: { value: 'BANK1-123456' } });

    expect(mockOnAccountChange).toHaveBeenCalledWith('BANK1-123456');
  });

  it('calls onAccountsLoaded after initial load', () => {
    render(<AccountFilter {...defaultProps} />);
    expect(mockOnAccountsLoaded).toHaveBeenCalledTimes(1);
  });

  it('renders with initial account value', () => {
    const initialProps = {
      ...defaultProps,
      account: 'BANK1-123456',
    };

    render(<AccountFilter {...initialProps} />);
    const dropdown = screen.getByTestId('custom-dropdown') as HTMLSelectElement;
    expect(dropdown.value).toBe('BANK1-123456');
  });

  it('handles empty payment information array', () => {
    render(
      <AccountFilter
        {...defaultProps}
        paymentInformations={[]}
      />
    );
    
    const dropdown = screen.getByTestId('custom-dropdown');
    expect(dropdown).toBeInTheDocument();
  });

  it('processes payment information correctly', () => {
    render(<AccountFilter {...defaultProps} />);
    
    const options = screen.getAllByRole('option');
    expect(options).toHaveLength(defaultProps.paymentInformations.length + 1);
    
    expect(options[1].textContent).toBe('123456');
    expect(options[2].textContent).toBe('789012');
  });
});
