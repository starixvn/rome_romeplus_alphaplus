import React from 'react';
import { describe, it, expect, vi, beforeEach } from 'vitest';
import { render, screen, fireEvent } from '@testing-library/react';
import '@testing-library/jest-dom';
import Filter from '../Filter';

vi.mock('react-i18next', () => ({
  useTranslation: () => ({
    t: (key: string) => key,
  }),
}));

vi.mock('../../../../../components/BankComponents', () => ({
  default: () => <div data-testid="bank-icon">Bank Icon</div>,
}));

describe('Filter', () => {
  const mockSetRecentDeposits = vi.fn();
  const mockSetCashAgentTo = vi.fn();
  const mockSetSearchTerm = vi.fn();

  const defaultProps = {
    recentDeposits: 'All',
    setRecentDeposits: mockSetRecentDeposits,
    setCashAgentTo: mockSetCashAgentTo,
    searchTerm: '',
    setSearchTerm: mockSetSearchTerm,
    uniquePaymentToOptions: ['BANK1-Bank One', 'BANK2-Bank Two'],
  };

  beforeEach(() => {
    vi.clearAllMocks();
  });

  it('renders all components correctly', () => {
    render(<Filter {...defaultProps} />);
  
    expect(screen.getByText('filter.recentDeposits')).toBeInTheDocument();
    expect(screen.getByRole('combobox')).toBeInTheDocument();
    expect(screen.getByText('filter.to')).toBeInTheDocument();
    expect(screen.getByPlaceholderText('filter.usernamePlaceholder')).toBeInTheDocument();
  });

  it('handles recent deposits selection change', () => {
    render(<Filter {...defaultProps} />);

    const select = screen.getByRole('combobox');
    fireEvent.change(select, { target: { value: 'Last30Mins' } });

    expect(mockSetRecentDeposits).toHaveBeenCalledWith('Last30Mins');
  });

  it('handles search term input change', () => {
    render(<Filter {...defaultProps} />);

    const searchInput = screen.getByPlaceholderText('filter.usernamePlaceholder');
    fireEvent.change(searchInput, { target: { value: 'test search' } });

    expect(mockSetSearchTerm).toHaveBeenCalledWith('test search');
  });

  it('renders with initial values', () => {
    const initialProps = {
      ...defaultProps,
      recentDeposits: 'Last60Mins',
      searchTerm: 'initial search',
    };

    render(<Filter {...initialProps} />);

    const select = screen.getByRole('combobox') as HTMLSelectElement;
    expect(select.value).toBe('Last60Mins');

    const searchInput = screen.getByPlaceholderText('filter.usernamePlaceholder') as HTMLInputElement;
    expect(searchInput.value).toBe('initial search');
  });

  it('renders all recent deposits options', () => {
    render(<Filter {...defaultProps} />);

    const options = screen.getAllByRole('option');
    expect(options).toHaveLength(3);
    expect(options[0]).toHaveValue('All');
    expect(options[1]).toHaveValue('Last30Mins');
    expect(options[2]).toHaveValue('Last60Mins');
  });

  it('applies correct styles to select and input elements', () => {
    render(<Filter {...defaultProps} />);

    const select = screen.getByRole('combobox');
    expect(select).toHaveClass('w-[100%]');

    const searchInput = screen.getByPlaceholderText('filter.usernamePlaceholder');
    expect(searchInput).toHaveClass('w-[100%]');
  });
});
