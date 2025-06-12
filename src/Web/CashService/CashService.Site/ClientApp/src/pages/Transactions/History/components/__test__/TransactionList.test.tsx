import React from 'react';
import { describe, it, expect, vi, beforeEach } from 'vitest';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import '@testing-library/jest-dom';
import TransactionList from '../TransactionList';
import { transactionAPI } from '../../../../../services/transactionsHistoryAPI';
import { TransactionType } from '../../../../../utils/commonFunctions';
import { Mock } from 'vitest';

// Mock the transactionAPI
vi.mock('../../../../../services/transactionsHistoryAPI', () => ({
  transactionAPI: {
    getTransactions: vi.fn(),
    exportExcel: vi.fn(),
  },
}));

// Mock react-i18next
vi.mock('react-i18next', () => ({
  useTranslation: () => ({
    t: (key: string) => key,
  }),
}));

// Add this with other mocks
vi.mock('../../../../../components/DateRangePickerComponent', () => ({
  default: ({ onChange }: { onChange: (dates: [Date, Date]) => void }) => (
    <div data-testid="date-range-picker" onClick={() => onChange([new Date('2024-01-01'), new Date('2024-01-31')])} />
  ),
}));

describe('TransactionList', () => {
  const mockTransactions = {
    items: [
      {
        transId: 1,
        username: 'testUser',
        amount: 100,
        paymentFrom: 'Bank A',
        paymentTo: 'Bank B',
        status: 1,
        type: 'Deposit',
        referenceNo: 'REF123',
        dateAndTime: '2024-01-01T10:00:00',
      },
      {
        transId: 2,
        username: 'testUser2',
        amount: 200,
        paymentFrom: 'Bank C',
        paymentTo: 'Bank D',
        status: 2,
        type: 'Deposit',
        referenceNo: 'REF456',
        dateAndTime: '2024-01-02T10:00:00',
      },
    ],
    totalCount: 2,
  };

  beforeEach(() => {
    vi.clearAllMocks();
    (transactionAPI.getTransactions as Mock).mockResolvedValue(mockTransactions);
    (transactionAPI.exportExcel as Mock).mockResolvedValue({ isSuccessful: true });
  });

  it('renders the component with initial state', async () => {
    render(<TransactionList transactionType={TransactionType.Deposit} />);

    await waitFor(() => {
      expect(transactionAPI.getTransactions).toHaveBeenCalledWith(
        expect.objectContaining({
          account: "",
          dateRange: expect.any(Array),
          pageIndex: 1,
          pageSize: 10,
          searchText: "",
          status: -1,
          transactionType: TransactionType.Deposit
        })
      );
    });

    await waitFor(() => {
      expect(screen.getByPlaceholderText('filter.usernamePlaceholder')).toBeInTheDocument();
      expect(screen.getByText('testUser')).toBeInTheDocument();
      expect(screen.getByText('testUser2')).toBeInTheDocument();
    });
  });

  it('handles search functionality', async () => {
    render(<TransactionList transactionType={TransactionType.Deposit} />);

    const searchInput = screen.getByPlaceholderText('filter.usernamePlaceholder');
    fireEvent.change(searchInput, { target: { value: 'testUser' } });

    await waitFor(() => {
      expect(transactionAPI.getTransactions).toHaveBeenCalled();
    });
  });

  it('handles pagination', async () => {
    render(<TransactionList transactionType={TransactionType.Deposit} />);

    await waitFor(() => {
      expect(transactionAPI.getTransactions).toHaveBeenCalledWith(
        expect.objectContaining({
          account: "",
          dateRange: expect.any(Array),
          pageIndex: 1,
          pageSize: 10,
          searchText: "",
          status: -1,
          transactionType: TransactionType.Deposit
        })
      );
    });

    await waitFor(() => {
      const nextButton = Array.from(document.querySelectorAll('span')).find(
        span => span.querySelector('svg') && span.textContent === ''
      );
      expect(nextButton).toBeInTheDocument();
      fireEvent.click(nextButton!);
    });
  });


  it('handles API error during data fetch', async () => {
    (transactionAPI.getTransactions as Mock).mockRejectedValueOnce(new Error('API Error'));
    
    render(<TransactionList transactionType={TransactionType.Deposit} />);

    await waitFor(() => {
      const errorElement = screen.getByRole('alert');
      expect(errorElement).toBeInTheDocument();
    });
  });

  it('handles page size change', async () => {
    render(<TransactionList transactionType={TransactionType.Deposit} />);

    const pageSizeSelect = screen.getByRole('combobox');
    fireEvent.change(pageSizeSelect, { target: { value: '20' } });

    await waitFor(() => {
      expect(transactionAPI.getTransactions).toHaveBeenCalled();
    });
  });

  it('handles date range filter', async () => {
    render(<TransactionList transactionType={TransactionType.Deposit} />);

    await waitFor(() => {
      expect(transactionAPI.getTransactions).toHaveBeenCalled();
    });

    const dateRangePicker = screen.getByTestId('date-range-picker');
    fireEvent.click(dateRangePicker);

    await waitFor(() => {
      expect(transactionAPI.getTransactions).toHaveBeenCalledWith(
        expect.objectContaining({
          dateRange: expect.any(Array)
        })
      );
    });
  });
});
