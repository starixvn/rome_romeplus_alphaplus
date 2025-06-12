import React from 'react';
import { describe, it, expect, vi, beforeEach } from 'vitest';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import '@testing-library/jest-dom';
import Search from '../Search';
import { TransactionItem } from '../../../../../services/interfaces/Transactions/TransactionHistory';

// Mock react-i18next
vi.mock('react-i18next', () => ({
  useTranslation: () => ({
    t: (key: string) => key,
  }),
}));

// Mock clipboard API
Object.assign(navigator, {
  clipboard: {
    writeText: vi.fn(),
  },
});

// Mock common functions
vi.mock('../../../../../utils/commonFunctions', () => ({
  CurrentCurrency: 'USD',
  displayAmount: (amount: number) => amount.toString(),
  formatDateTime: (date: string) => date,
  mappingStatus: (statusId: number) => `Status ${statusId}`,
}));

describe('Search', () => {
  const mockTransactions: TransactionItem[] = [
    {
      transId: 1,
      custId: 1001,
      username: 'testuser1',
      nickname: 'Test1',
      firstName: 'John',
      lastName: 'Doe',
      amount: 1000,
      currency: 'USD',
      paymentFrom: 'Bank A',
      paymentTo: 'Bank B',
      statusId: 1,
      betCredit: 0,
      transactionTypeId: 2,
      transactionType: 'Withdrawal',
      remark: '',
      payslip: '',
      base64Payslip: '',
      createdTime: '2024-01-01T10:00:00',
      lastUpdatedDate: '2024-01-01T10:00:00',
      isAllowRevert: false,
      operatedBy: 'system',
      operatedTime: '2024-01-01T10:00:00',
    },
  ];

  const defaultProps = {
    value: '',
    onChange: vi.fn(),
    items: mockTransactions,
    columnFilter: ['name', 'amount', 'from', 'to', 'status', 'type', 'referenceNo', 'dateAndTime'],
    onDownload: vi.fn(),
  };

  beforeEach(() => {
    vi.clearAllMocks();
  });

  it('renders search component correctly', () => {
    render(<Search {...defaultProps} />);
    
    expect(screen.getByPlaceholderText('filter.usernamePlaceholder')).toBeInTheDocument();
    expect(screen.getByTitle('Copy')).toBeInTheDocument();
    expect(screen.getByTitle('Download')).toBeInTheDocument();
  });

  it('handles search input change', () => {
    render(<Search {...defaultProps} />);
    
    const searchInput = screen.getByPlaceholderText('filter.usernamePlaceholder');
    fireEvent.change(searchInput, { target: { value: 'test' } });
    
    expect(defaultProps.onChange).toHaveBeenCalledWith('test');
  });

  it('handles download button click', () => {
    render(<Search {...defaultProps} />);
    
    const downloadButton = screen.getByTitle('Download');
    fireEvent.click(downloadButton);
    
    expect(defaultProps.onDownload).toHaveBeenCalled();
  });

  it('handles copy button click and shows alert', async () => {
    const clipboardWriteText = vi.fn().mockResolvedValue(undefined);
    Object.assign(navigator, {
      clipboard: {
        writeText: clipboardWriteText,
      },
    });

    render(<Search {...defaultProps} />);
    
    const copyButton = screen.getByTitle('Copy');
    fireEvent.click(copyButton);

    const headers = [
      'table.no',
      'table.username & table.nickname',
      'table.name',
      'table.amount (USD)',
      'table.from',
      'table.to',
      'table.status',
      'table.type',
      'table.referenceNo',
      'table.dateAndTime'
    ];

    const row = [
      '1',
      'testuser1 (Test1)',
      'John Doe',
      '1000',
      'Bank A',
      'Bank B',
      'Status 1',
      'Withdrawal',
      '1',
      '2024-01-01T10:00:00'
    ];

    const expectedText = headers.join('\t') + '\n' + row.join('\t');

    await waitFor(() => {
      expect(clipboardWriteText).toHaveBeenCalledWith(expectedText);
    });

    await waitFor(() => {
      expect(screen.getByText('messages.copiedToClipboard')).toBeInTheDocument();
    });

    await waitFor(() => {
      expect(screen.queryByText('messages.copiedToClipboard')).not.toBeInTheDocument();
    }, { timeout: 1100 });
  });

  it('handles copy button click with empty data', async () => {
    const clipboardWriteText = vi.fn().mockResolvedValue(undefined);
    Object.assign(navigator, {
      clipboard: {
        writeText: clipboardWriteText,
      },
    });

    render(<Search {...defaultProps} items={[]} />);
    
    const copyButton = screen.getByTitle('Copy');
    fireEvent.click(copyButton);

    const headers = [
      'table.no',
      'table.username & table.nickname',
      'table.name',
      'table.amount (USD)',
      'table.from',
      'table.to',
      'table.status',
      'table.type',
      'table.referenceNo',
      'table.dateAndTime'
    ];

    const expectedText = headers.join('\t');

    await waitFor(() => {
      expect(clipboardWriteText).toHaveBeenCalledWith(expectedText);
    });

    await waitFor(() => {
      expect(screen.getByText('messages.copiedToClipboard')).toBeInTheDocument();
    });

    await waitFor(() => {
      expect(screen.queryByText('messages.copiedToClipboard')).not.toBeInTheDocument();
    }, { timeout: 1100 });
  });

  it('displays max records message', () => {
    render(<Search {...defaultProps} />);
    expect(screen.getByText('* filter.maxRecords')).toBeInTheDocument();
  });

  it('handles clipboard API failure', async () => {
    const consoleSpy = vi.spyOn(console, 'error').mockImplementation(() => {});
    navigator.clipboard.writeText = vi.fn().mockRejectedValue(new Error('Clipboard error'));
    
    render(<Search {...defaultProps} />);
    
    const copyButton = screen.getByTitle('Copy');
    fireEvent.click(copyButton);

    await waitFor(() => {
      expect(consoleSpy).toHaveBeenCalledWith('Failed to copy text: ', expect.any(Error));
    });

    consoleSpy.mockRestore();
  });
});
