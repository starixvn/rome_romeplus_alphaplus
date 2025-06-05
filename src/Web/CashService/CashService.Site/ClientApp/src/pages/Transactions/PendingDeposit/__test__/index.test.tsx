import React from 'react';
import { describe, it, expect, vi, beforeEach, Mock } from 'vitest';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import '@testing-library/jest-dom';
import PendingDeposit from '../index';
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
    getCashAgentBankInfo: vi.fn(),
  },
}));

vi.mock('../../../../components/LoadingComponent', () => ({
  LoadingFullComponent: () => <div data-testid="loading">Loading...</div>,
}));

vi.mock('../components/DataGrid', () => ({
  default: ({ deposits }) => (
    <div data-testid="data-grid">
      {deposits.map((deposit) => (
        <div key={deposit.transId}>{deposit.username}</div>
      ))}
    </div>
  )
}));

describe('PendingDeposit', () => {
  const mockTransactions = {
    items: [
      {
        transId: '123',
        username: 'testUser',
        paymentTo: 'BANK1',
        amount: 100,
        status: 2,
        createTime: '2024-03-20T10:00:00',
      },
    ],
    totalCount: 1,
  };

  const mockBankInfo = [
    {
      id: 1,
      bankName: 'BANK1-KRR',
      displayName: 'Bank One',
    },
  ];

  beforeEach(() => {
    vi.clearAllMocks();
    (transactionAPI.getTransactions as Mock).mockResolvedValue(mockTransactions);
    (transactionAPI.getCashAgentBankInfo as Mock).mockResolvedValue(mockBankInfo);
  });

  it('renders the component with initial state', async () => {
    render(<PendingDeposit />);

    expect(screen.getByText('pendingDeposit')).toBeInTheDocument();

    expect(screen.getByText(/addManualDeposit/)).toBeInTheDocument();

    expect(screen.getByText('filter.recentDeposits')).toBeInTheDocument();

    await waitFor(() => {
      expect(transactionAPI.getTransactions).toHaveBeenCalled();
      expect(transactionAPI.getCashAgentBankInfo).toHaveBeenCalled();
    });
  });

  it('shows loading state while fetching data', async () => {
    (transactionAPI.getTransactions as Mock).mockImplementation(
      () => new Promise((resolve) => setTimeout(() => resolve(mockTransactions), 100))
    );

    render(<PendingDeposit />);

    expect(screen.getByTestId('loading')).toBeInTheDocument();

    await waitFor(() => {
      expect(screen.queryByTestId('loading')).not.toBeInTheDocument();
    });
  });

  it('handles manual deposit form toggle', () => {
    render(<PendingDeposit />);

    fireEvent.click(screen.getByText(/addManualDeposit/));

    expect(screen.queryByText('pendingDeposit')).not.toBeInTheDocument();
  });

  it('filters deposits based on search term', async () => {
    render(<PendingDeposit />);

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

    render(<PendingDeposit />);

    await waitFor(() => {
      expect(consoleErrorSpy).toHaveBeenCalledWith('Error fetching deposits:', expect.any(Error));
    });

    consoleErrorSpy.mockRestore();
  });

  it('updates deposits when recent deposits filter changes', async () => {
    render(<PendingDeposit />);

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
