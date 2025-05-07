import React, { useState, useEffect } from 'react';
import { getTransactions } from '../../../../services/transactionsHistoryAPI';
import {
  TransactionItem,
  TransactionsHistoryResponse,
  TransactionsHistoryParams,
} from '../../../../services/interfaces/Transactions/TransactionHistory';
import Filters from './Filters/Filters';
import Search from './Search';
import Table from './Table';
import Pagination from './Pagination';
import { useTranslation } from 'react-i18next';
import '../../../../i18n/i18n';

interface TransactionListProps {
  transactionType: number;
}

const TransactionList: React.FC<TransactionListProps> = ({ transactionType }) => {
  const { t } = useTranslation('transactionHistory');
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [apiData, setApiData] = useState<TransactionsHistoryResponse>({
    items: [],
    totalCount: 0,
  });
  const [apiParams, setApiParams] = useState<
    Omit<TransactionsHistoryParams, 'searchText' | 'column'>
  >(() => ({
    dateRange: ['04/10/2025', '04/16/2025'],
    status: 'All',
    account: '',
    page: 1,
    pageSize: 100,
    transactionType: transactionType,
  }));
  const [columnFilter, setColumnFilter] = useState<string[]>([
    t('table.name'),
    t('table.amount'),
    t('table.currency'),
    t('table.from'),
    t('table.to'),
    t('table.status'),
    t('table.type'),
    t('table.referenceNo'),
    t('table.dateAndTime'),
  ]);
  const [refreshTrigger, setRefreshTrigger] = useState(0);
  const [localSearchText, setLocalSearchText] = useState('');
  const [displayedItems, setDisplayedItems] = useState<TransactionItem[]>([]);

  useEffect(() => {
    setApiParams((prev) => ({
      ...prev,
      transactionType: transactionType,
      page: 1,
    }));
  }, [transactionType]);

  useEffect(() => {
    const fetchData = async () => {
      try {
        setLoading(true);
        const paramsForApi: TransactionsHistoryParams = {
          ...apiParams,
          searchText: '',
        };
        const response = await getTransactions(paramsForApi);
        setApiData(response);
        setError(null);
      } catch (err) {
        setError('Failed to load transaction history');
        console.error('Error loading data:', err);
        setApiData({ items: [], totalCount: 0 });
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, [apiParams, columnFilter, refreshTrigger]);

  useEffect(() => {
    if (!localSearchText) {
      setDisplayedItems(apiData.items);
    } else {
      const lowerCaseSearchText = localSearchText.toLowerCase();
      const filtered = apiData.items.filter(
        (item) =>
          item.username.toLowerCase().includes(lowerCaseSearchText) ||
          item.referenceNumber.toLowerCase().includes(lowerCaseSearchText)
      );
      setDisplayedItems(filtered);
    }
  }, [apiData.items, localSearchText]);

  const handleDateRangeChange = (dates: [string, string]) => {
    setApiParams((prev) => ({ ...prev, dateRange: dates, page: 1 }));
  };

  const handleStatusChange = (value: string) => {
    setApiParams((prev) => ({ ...prev, status: value, page: 1 }));
  };

  const handleColumnChange = (value: string[]) => {
    setColumnFilter(value);
  };

  const handleAccountChange = (value: string) => {
    setApiParams((prev) => ({ ...prev, account: value, page: 1 }));
  };

  const handleLocalSearchChange = (value: string) => {
    setLocalSearchText(value);
  };

  const handleRefresh = () => {
    setRefreshTrigger((prev) => prev + 1);
  };

  const handlePageChange = (newPage: number) => {
    setApiParams((prev) => ({ ...prev, page: newPage }));
  };

  const handlePageSizeChange = (newSize: number) => {
    setApiParams((prev) => ({ ...prev, pageSize: newSize, page: 1 }));
  };

  const pageSizeOptions = [100, 200, 500, 1000];

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
          dateRange={apiParams.dateRange}
          status={apiParams.status}
          column={columnFilter}
          account={apiParams.account}
          transactionType={transactionType === 1 ? 'deposit' : 'withdrawal'}
          onDateRangeChange={handleDateRangeChange}
          onStatusChange={handleStatusChange}
          onColumnChange={handleColumnChange}
          onAccountChange={handleAccountChange}
        />
      </div>

      <div className="search-section mb-2 w-full">
        <Search
          value={localSearchText}
          onChange={handleLocalSearchChange}
          placeholder="Username/Reference No."
        />
      </div>

      <div className="table-section">
        <Table
          items={displayedItems}
          isLoading={loading}
          onRefreshNeeded={handleRefresh}
          columnFilter={columnFilter}
        />
        <Pagination
          currentPage={apiParams.page}
          totalCount={apiData.totalCount}
          pageSize={apiParams.pageSize}
          pageSizeOptions={pageSizeOptions}
          onPageChange={handlePageChange}
          onPageSizeChange={handlePageSizeChange}
        />
      </div>
    </div>
  );
};

export default TransactionList;
