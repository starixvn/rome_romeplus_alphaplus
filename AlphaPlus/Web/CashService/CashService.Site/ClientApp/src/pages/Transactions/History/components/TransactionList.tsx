import React, { useState, useEffect } from 'react';
import { useTranslation } from 'react-i18next';
import {
  getTransactions,
  TransactionHistoryItem,
  TransactionsHistoryResponse,
  TransactionsHistoryParams,
} from '../../../../services/transactionsHistoryAPI';
import Filters from './Filters';
import Search from './Search';
import Table from './Table';

interface TransactionListProps {
  transactionType: 'deposit' | 'withdrawal';
}

const TransactionList: React.FC<TransactionListProps> = ({ transactionType }) => {
  const { t } = useTranslation('transactions');
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [data, setData] = useState<TransactionsHistoryResponse>({
    items: [],
    totalCount: 0,
  });
  const [searchParams, setSearchParams] = useState<TransactionsHistoryParams>({
    dateRange: ['04/10/2025', '04/16/2025'],
    status: 'All',
    column: 'All Columns (9)',
    searchText: '',
    page: 1,
    pageSize: 10,
  });

  useEffect(() => {
    setSearchParams((prev) => ({ ...prev, transactionType }));
  }, [transactionType]);

  const fetchData = async () => {
    try {
      setLoading(true);
      const response = await getTransactions(searchParams);
      setData(response);
      setError(null);
    } catch (err) {
      setError('Failed to load transaction history');
      console.error('Error loading data:', err);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchData();
  }, [searchParams]);

  const handleDateRangeChange = (dates: [string, string]) => {
    setSearchParams((prev: TransactionsHistoryParams) => ({ ...prev, dateRange: dates }));
  };

  const handleStatusChange = (value: string) => {
    setSearchParams((prev: TransactionsHistoryParams) => ({ ...prev, status: value }));
  };

  const handleColumnChange = (value: string) => {
    setSearchParams((prev: TransactionsHistoryParams) => ({ ...prev, column: value }));
  };

  const handleSearchChange = (value: string) => {
    setSearchParams((prev: TransactionsHistoryParams) => ({ ...prev, searchText: value }));
  };

  return (
    <div className="transaction-list-container">
      {error && (
        <div
          className="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded relative mb-4"
          role="alert"
        >
          <strong className="font-bold">Error!</strong>
          <span className="block sm:inline"> {error}</span>
        </div>
      )}

      <div className="filters-container mb-4">
        <Filters
          dateRange={searchParams.dateRange}
          status={searchParams.status}
          column={searchParams.column}
          onDateRangeChange={handleDateRangeChange}
          onStatusChange={handleStatusChange}
          onColumnChange={handleColumnChange}
        />
      </div>

      <div className="bg-white p-4 rounded-lg">
        <div className="search-section mb-2 w-full">
          <Search
            value={searchParams.searchText}
            onChange={handleSearchChange}
            placeholder="Username/Reference No."
          />
        </div>

        <div className="table-section">
          <Table items={data.items} isLoading={loading} />
        </div>
      </div>
    </div>
  );
};

export default TransactionList;
