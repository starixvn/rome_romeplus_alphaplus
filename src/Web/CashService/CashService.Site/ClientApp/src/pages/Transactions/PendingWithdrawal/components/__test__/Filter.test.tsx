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

describe('Filter', () => {
  const mockSetRecentWithdrawals = vi.fn();
  const mockSetSearchTerm = vi.fn();

  const defaultProps = {
    recentWithdrawals: 'All',
    setRecentWithdrawals: mockSetRecentWithdrawals,
    searchTerm: '',
    setSearchTerm: mockSetSearchTerm,
  };

  beforeEach(() => {
    vi.clearAllMocks();
  });

  it('renders all components correctly', () => {
    render(<Filter {...defaultProps} />);
  
    expect(screen.getByText('filter.recentWithdrawals')).toBeInTheDocument();
    expect(screen.getByRole('combobox')).toBeInTheDocument();
    expect(screen.getByPlaceholderText('filter.usernamePlaceholder')).toBeInTheDocument();
  });

  it('handles recent withdrawals selection change', () => {
    render(<Filter {...defaultProps} />);

    const select = screen.getByRole('combobox');
    fireEvent.change(select, { target: { value: 'Last30Mins' } });

    expect(mockSetRecentWithdrawals).toHaveBeenCalledWith('Last30Mins');
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
      recentWithdrawals: 'Last60Mins',
      searchTerm: 'initial search',
    };

    render(<Filter {...initialProps} />);

    const select = screen.getByRole('combobox') as HTMLSelectElement;
    expect(select.value).toBe('Last60Mins');

    const searchInput = screen.getByPlaceholderText('filter.usernamePlaceholder') as HTMLInputElement;
    expect(searchInput.value).toBe('initial search');
  });

  it('renders all recent withdrawals options', () => {
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
    expect(select).toHaveClass('control-field');

    const searchInput = screen.getByPlaceholderText('filter.usernamePlaceholder');
    expect(searchInput).toHaveClass('control-field');
  });
});
