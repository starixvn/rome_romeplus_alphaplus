import React from 'react';
import { describe, it, expect, vi, beforeEach, Mock } from 'vitest';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import '@testing-library/jest-dom';
import PendingWithdrawal from '../index';
import { transactionAPI } from '../../../../services/transactionsHistoryAPI';

vi.mock('react-i18next', () => ({
  useTranslation: () => ({
    t: (key: string) => key,
  }),
  initReactI18next: {
    type: '3rdParty',
    init: () => {},
  }
}));

vi.mock('../../../../hooks/useCookieLanguage', () => ({
  useCookieLanguage: vi.fn(),
}));

vi.mock('../../../../services/transactionsHistoryAPI', () => ({
  transactionAPI: {
    getTransactions: vi.fn(),
    getMemberLoginName: vi.fn(),
    getCashAgentBankInfo: vi.fn(),
    getCashAgentProvider: vi.fn(),
  },
}));

vi.mock('../../../../components/LoadingComponent', () => ({
  LoadingFullComponent: () => <div data-testid="loading">Loading...</div>,
}));

vi.mock('../components/DataGrid', () => ({
  default: ({ withdrawals }) => (
    <div data-testid="data-grid">
      {withdrawals.map((withdrawal) => (
        <div key={withdrawal.transId}>{withdrawal.username}</div>
      ))}
    </div>
  )
}));

vi.mock('../components/ManualAddWithdrawalForm', () => ({
  default: ({ onClose }) => (
    <div data-testid="manual-withdrawal-form">
      <button onClick={() => onClose(false)}>Close</button>
    </div>
  ),
}));

describe('PendingWithdrawal', () => {
  const mockTransactions = {
    items: [
      {
        transId: '123',
        username: 'testUser',
        amount: 100,
        status: 2,
        createTime: '2024-03-20T10:00:00',
      },
    ],
    totalCount: 1,
  };

  const mockMemberNames = ['testUser1', 'testUser2'];
  const mockBankInfo = [{ id: 1, bankName: 'TestBank', displayName: 'Test Bank' }];
  const mockProviderInfo = [{ id: 1, name: 'TestProvider' }];

  beforeEach(() => {
    vi.clearAllMocks();
    (transactionAPI.getTransactions as Mock).mockResolvedValue(mockTransactions);
    (transactionAPI.getMemberLoginName as Mock).mockResolvedValue(mockMemberNames);
    (transactionAPI.getCashAgentBankInfo as Mock).mockResolvedValue(mockBankInfo);
    (transactionAPI.getCashAgentProvider as Mock).mockResolvedValue(mockProviderInfo);
  });

  it('renders the component with initial state', async () => {
    render(<PendingWithdrawal />);

    expect(screen.getByText('pendingWithdrawal')).toBeInTheDocument();
    expect(screen.getByText(/addManualWithdrawal/)).toBeInTheDocument();
    expect(screen.getByText('filter.recentWithdrawals')).toBeInTheDocument();

    await waitFor(() => {
      expect(transactionAPI.getTransactions).toHaveBeenCalled();
    });
  });

  it('shows loading state while fetching data', async () => {
    (transactionAPI.getTransactions as Mock).mockImplementation(
      () => new Promise((resolve) => setTimeout(() => resolve(mockTransactions), 100))
    );

    render(<PendingWithdrawal />);

    expect(screen.getByTestId('loading')).toBeInTheDocument();

    await waitFor(() => {
      expect(screen.queryByTestId('loading')).not.toBeInTheDocument();
    });
  });

  it('handles manual withdrawal form toggle', async () => {
    render(<PendingWithdrawal />);

    fireEvent.click(screen.getByText(/addManualWithdrawal/));

    await waitFor(() => {
      expect(screen.getByTestId('manual-withdrawal-form')).toBeInTheDocument();
    });

    const closeButton = screen.getByText('Close');
    fireEvent.click(closeButton);

    await waitFor(() => {
      expect(screen.queryByTestId('manual-withdrawal-form')).not.toBeInTheDocument();
    });
  });

  it('filters withdrawals based on search term', async () => {
    render(<PendingWithdrawal />);

    await waitFor(() => {
      expect(transactionAPI.getTransactions).toHaveBeenCalled();
    });

    const searchInput = screen.getByPlaceholderText('filter.usernamePlaceholder');

    fireEvent.change(searchInput, { target: { value: 'testUser' } });

    await waitFor(() => {
      expect(screen.getByText('testUser')).toBeInTheDocument();
    });
  });

  it('handles API error gracefully', async () => {
    const consoleErrorSpy = vi.spyOn(console, 'error').mockImplementation(() => {});
    (transactionAPI.getTransactions as Mock).mockRejectedValue(new Error('API Error'));

    render(<PendingWithdrawal />);

    await waitFor(() => {
      expect(consoleErrorSpy).toHaveBeenCalledWith('Error fetching withdrawals:', expect.any(Error));
    });

    consoleErrorSpy.mockRestore();
  });

  it('updates withdrawals when recent withdrawals filter changes', async () => {
    render(<PendingWithdrawal />);

    await waitFor(() => {
      expect(transactionAPI.getTransactions).toHaveBeenCalled();
    });

    const select = screen.getByRole('combobox');
    fireEvent.change(select, { target: { value: 'Last30Mins' } });

    await waitFor(() => {
      expect(transactionAPI.getTransactions).toHaveBeenCalledTimes(2);
    });
  });
});
