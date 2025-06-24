import React from 'react';
import { describe, it, expect, vi } from 'vitest';
import { render, screen, fireEvent } from '@testing-library/react';
import '@testing-library/jest-dom';
import HistoryTab from '../HistoryTab';
import { TransactionType } from '../../../../../utils/commonFunctions';

// Mock react-i18next
vi.mock('react-i18next', () => ({
  useTranslation: () => ({
    t: (key: string) => key,
  }),
}));

describe('HistoryTab', () => {
  it('renders both deposit and withdrawal buttons', () => {
    const mockOnTabChange = vi.fn();
    render(<HistoryTab activeTab={TransactionType.Deposit} onTabChange={mockOnTabChange} />);

    expect(screen.getByText('deposit')).toBeInTheDocument();
    expect(screen.getByText('withdrawal')).toBeInTheDocument();
  });

  it('applies correct styles when Deposit tab is active', () => {
    const mockOnTabChange = vi.fn();
    render(<HistoryTab activeTab={TransactionType.Deposit} onTabChange={mockOnTabChange} />);

    const depositButton = screen.getByText('deposit').closest('button');
    const withdrawalButton = screen.getByText('withdrawal').closest('button');

    expect(depositButton?.className).toContain('bg-[var(--button-background)]');
    expect(withdrawalButton?.className).toContain('border-[var(--button-outline-border)]');
  });

  it('applies correct styles when Withdrawal tab is active', () => {
    const mockOnTabChange = vi.fn();
    render(<HistoryTab activeTab={TransactionType.Withdrawal} onTabChange={mockOnTabChange} />);

    const depositButton = screen.getByText('deposit').closest('button');
    const withdrawalButton = screen.getByText('withdrawal').closest('button');

    expect(depositButton?.className).toContain('border-[var(--button-outline-border)]');
    expect(withdrawalButton?.className).toContain('bg-[var(--button-background)]');
  });

  it('calls onTabChange with correct value when deposit button is clicked', () => {
    const mockOnTabChange = vi.fn();
    render(<HistoryTab activeTab={TransactionType.Withdrawal} onTabChange={mockOnTabChange} />);

    const depositButton = screen.getByText('deposit');
    fireEvent.click(depositButton);

    expect(mockOnTabChange).toHaveBeenCalledWith(TransactionType.Deposit);
  });

  it('calls onTabChange with correct value when withdrawal button is clicked', () => {
    const mockOnTabChange = vi.fn();
    render(<HistoryTab activeTab={TransactionType.Deposit} onTabChange={mockOnTabChange} />);

    const withdrawalButton = screen.getByText('withdrawal');
    fireEvent.click(withdrawalButton);

    expect(mockOnTabChange).toHaveBeenCalledWith(TransactionType.Withdrawal);
  });

  it('applies minimum width class to button containers', () => {
    const mockOnTabChange = vi.fn();
    render(<HistoryTab activeTab={TransactionType.Deposit} onTabChange={mockOnTabChange} />);

    const buttonContainers = screen.getAllByRole('button').map(button => button.parentElement);
    buttonContainers.forEach(container => {
      expect(container?.className).toContain('[&>*]:min-w-[150px]');
    });
  });
});
