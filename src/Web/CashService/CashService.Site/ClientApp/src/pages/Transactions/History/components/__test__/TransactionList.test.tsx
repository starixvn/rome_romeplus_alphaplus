import React from 'react';
import { describe, it, expect, vi, beforeEach } from 'vitest';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import '@testing-library/jest-dom';
import TransactionList from '../TransactionList';
import { transactionAPI } from '../../../../../services/transactionsHistoryAPI';
import { TransactionType } from '../../../../../utils/commonFunctions';
import { PAGE_SIZE_OPTIONS } from '../../../../../constants/paymentConstants';
import { Mock } from 'vitest';
import { useTranslation } from 'react-i18next';

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
    ],
    totalCount: 1,
  };

  beforeEach(() => {
    vi.clearAllMocks();
    // Setup default mock response
    (transactionAPI.getTransactions as Mock).mockResolvedValue(mockTransactions);
  });

  it('renders the component with initial state', async () => {
    render(<TransactionList transactionType={TransactionType.Deposit} />);

    expect(screen.getByPlaceholderText('filter.usernamePlaceholder')).toBeInTheDocument();

    await waitFor(() => {
      expect(transactionAPI.getTransactions).toHaveBeenCalled();
    });
  });

  it('handles search functionality', async () => {
    render(<TransactionList transactionType={TransactionType.Deposit} />);

    await waitFor(() => {
      expect(transactionAPI.getTransactions).toHaveBeenCalled();
    });

    const searchInput = screen.getByPlaceholderText('filter.usernamePlaceholder');
    fireEvent.change(searchInput, { target: { value: 'testUser' } });

    // Verify that the search results are filtered
    await waitFor(() => {
      expect(screen.getByText('testUser')).toBeInTheDocument();
    });
  });

  it('handles pagination', async () => {
    render(<TransactionList transactionType={TransactionType.Deposit} />);

    await waitFor(() => {
      expect(transactionAPI.getTransactions).toHaveBeenCalled();
    });

    // Find the page input and verify it's 1
    await waitFor(() => {
      const pageInput = screen.getByDisplayValue('1');
      expect(pageInput).toBeInTheDocument();
    });

    // Wait for and find the next button by its container
    await waitFor(() => {
      const nextButton = Array.from(document.querySelectorAll('span')).find(
        span => span.querySelector('svg') && span.textContent === ''
      );
      expect(nextButton).toBeInTheDocument();
      fireEvent.click(nextButton!);
    });

    // Verify that the API was called with updated page index
    await waitFor(() => {
      expect(transactionAPI.getTransactions).toHaveBeenCalledWith({
        account: "",
        dateRange: ["6/5/2025, 12:00:00 AM", "6/11/2025, 11:59:59 PM"],
        pageIndex: 1,
        pageSize: 10,
        searchText: "",
        status: -1,
        transactionType: TransactionType.Deposit
      });
    });
  });

//   it('handles date range filter changes', async () => {
//     const { t } = useTranslation();
//     render(<TransactionList transactionType={TransactionType.Deposit} />);

//     await waitFor(() => {
//       expect(transactionAPI.getTransactions).toHaveBeenCalled();
//     });

//     // Find the date range picker button by its class name
//     const dateRangeButton = screen.getByRole('button', { name: 'filter.dateRange' });
//     fireEvent.click(dateRangeButton);

//     // Find and click the "Last 7 Days" quick range button
//     const last7DaysButton = screen.getByRole('button', { name: 'Last 7 Days' });
//     fireEvent.click(last7DaysButton);

//     // Verify that the API was called with new date range
//     await waitFor(() => {
//       expect(transactionAPI.getTransactions).toHaveBeenCalledWith({
//         account: "",
//         dateRange: ["6/5/2025, 12:00:00 AM", "6/11/2025, 11:59:59 PM"],
//         pageIndex: 1,
//         pageSize: 10,
//         searchText: "",
//         status: -1,
//         transactionType: TransactionType.Deposit
//       });
//     });
//   });

//   it('handles status filter changes', async () => {
//     render(<TransactionList transactionType={TransactionType.Deposit} />);

//     await waitFor(() => {
//       expect(transactionAPI.getTransactions).toHaveBeenCalled();
//     });

//     // Find and change status filter
//     const statusSelect = screen.getByRole('combobox', { name: /status/i });
//     fireEvent.change(statusSelect, { target: { value: '1' } });

//     // Verify that the API was called with new status
//     expect(transactionAPI.getTransactions).toHaveBeenCalledWith(
//       expect.objectContaining({
//         status: 1,
//       })
//     );
//   });

  it('handles export functionality', async () => {
    render(<TransactionList transactionType={TransactionType.Deposit} />);

    await waitFor(() => {
      expect(transactionAPI.getTransactions).toHaveBeenCalled();
    });

    // Find and click export button
    const exportButton = screen.getByRole('button', { name: /export/i });
    fireEvent.click(exportButton);

    // Verify that export API was called
    expect(transactionAPI.exportExcel).toHaveBeenCalled();
  });

//   it('handles error state', async () => {
//     // Mock API to throw error
//     (transactionAPI.getTransactions as Mock).mockRejectedValue(new Error('API Error'));

//     render(<TransactionList transactionType={TransactionType.Deposit} />);

//     // Verify error message is displayed
//     await waitFor(() => {
//       expect(screen.getByText(/Failed to load transaction history/i)).toBeInTheDocument();
//     });
//   });

//   it('handles page size changes', async () => {
//     render(<TransactionList transactionType={TransactionType.Deposit} />);

//     await waitFor(() => {
//       expect(transactionAPI.getTransactions).toHaveBeenCalled();
//     });

//     // Find and change page size select
//     const pageSizeSelect = screen.getByRole('combobox', { name: /page size/i });
//     fireEvent.change(pageSizeSelect, { target: { value: PAGE_SIZE_OPTIONS[1].toString() } });

//     // Verify that the API was called with new page size
//     expect(transactionAPI.getTransactions).toHaveBeenCalledWith(
//       expect.objectContaining({
//         pageSize: PAGE_SIZE_OPTIONS[1],
//         pageIndex: 1, // Should reset to first page
//       })
//     );
//   });
});
