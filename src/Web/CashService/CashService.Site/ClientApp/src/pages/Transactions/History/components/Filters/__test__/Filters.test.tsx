import React from 'react';
import { describe, it, expect, vi } from 'vitest';
import { render, screen } from '@testing-library/react';
import '@testing-library/jest-dom';
import Filters from '../Filters';

// Mock the sub-components to isolate the Filters component testing
vi.mock('../DateRangeFilter', () => ({
  default: ({ dateRange, onDateRangeChange }: any) => (
    <div data-testid="date-range-filter">
      DateRangeFilter Component
    </div>
  ),
}));

vi.mock('../AccountFilter', () => ({
  default: ({ account, transactionType, paymentInformations, onAccountChange }: any) => (
    <div data-testid="account-filter">
      AccountFilter Component
    </div>
  ),
}));

vi.mock('../StatusFilter', () => ({
  default: ({ status, onStatusChange }: any) => (
    <div data-testid="status-filter">
      StatusFilter Component
    </div>
  ),
}));

vi.mock('../ColumnFilter', () => ({
  default: ({ column, onColumnChange }: any) => (
    <div data-testid="column-filter">
      ColumnFilter Component
    </div>
  ),
}));

describe('Filters', () => {
  const defaultProps = {
    dateRange: ['2024-01-01', '2024-01-31'] as [string, string],
    status: 0,
    column: ['column1', 'column2'],
    account: 'test-account',
    paymentInformations: ['payment1', 'payment2'],
    transactionType: 1,
    onDateRangeChange: vi.fn(),
    onStatusChange: vi.fn(),
    onColumnChange: vi.fn(),
    onAccountChange: vi.fn(),
  };

  it('renders all filter components', () => {
    render(<Filters {...defaultProps} />);

    expect(screen.getByTestId('date-range-filter')).toBeInTheDocument();
    expect(screen.getByTestId('account-filter')).toBeInTheDocument();
    expect(screen.getByTestId('status-filter')).toBeInTheDocument();
    expect(screen.getByTestId('column-filter')).toBeInTheDocument();
  });

  it('renders with correct wrapper classes', () => {
    const { container } = render(<Filters {...defaultProps} />);
    
    const wrapper = container.firstChild as HTMLElement;
    expect(wrapper).toHaveClass('flex', 'flex-nowrap', 'gap-[16px]');
  });

  it('passes correct props to DateRangeFilter', () => {
    render(<Filters {...defaultProps} />);
    const dateRangeFilter = screen.getByTestId('date-range-filter');
    expect(dateRangeFilter).toBeInTheDocument();
  });

  it('passes correct props to AccountFilter', () => {
    render(<Filters {...defaultProps} />);
    const accountFilter = screen.getByTestId('account-filter');
    expect(accountFilter).toBeInTheDocument();
  });

  it('passes correct props to StatusFilter', () => {
    render(<Filters {...defaultProps} />);
    const statusFilter = screen.getByTestId('status-filter');
    expect(statusFilter).toBeInTheDocument();
  });

  it('passes correct props to ColumnFilter', () => {
    render(<Filters {...defaultProps} />);
    const columnFilter = screen.getByTestId('column-filter');
    expect(columnFilter).toBeInTheDocument();
  });

  it('renders correctly with minimum required props', () => {
    render(<Filters {...defaultProps} />);
    expect(screen.getAllByTestId(/.*-filter/).length).toBe(4);
  });
});
