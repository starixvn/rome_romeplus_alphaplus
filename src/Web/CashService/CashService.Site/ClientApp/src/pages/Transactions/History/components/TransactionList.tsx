import React, { useState, useEffect } from 'react';
import { transactionAPI } from '../../../../services/transactionsHistoryAPI';
import {
  TransactionItem,
  TransactionsHistoryResponse,
  TransactionsHistoryParams,
} from '../../../../services/interfaces/Transactions/TransactionHistory';
import Filters from './Filters/Filters';
import Search from './Search';
import DataGrid from './DataGrid';
import Pagination from './Pagination';
import { useTranslation } from 'react-i18next';
import { PAGE_SIZE_OPTIONS } from '../../../../constants/paymentConstants';
import { TransactionType } from '../../../../common/commonFunctions';

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
  >(() => {
    const today = new Date();
    const sevenDaysAgo = new Date();
    sevenDaysAgo.setDate(today.getDate() - 7);
    return {
      dateRange: [sevenDaysAgo.toLocaleDateString('en-US'), today.toLocaleDateString('en-US')],
      status: -1,
      account: '',
      pageIndex: 1,
      pageSize: PAGE_SIZE_OPTIONS[0],
      transactionType: transactionType,
    };
  });
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
  const [paymentInformations, setPaymentInformations] = useState<string[]>([]);

  useEffect(() => {
    setApiParams((prev) => ({
      ...prev,
      transactionType: transactionType,
      pageIndex: 1,
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
        const response = await transactionAPI.getTransactions(paramsForApi);
        setApiData(response);

        // Map payment information from response with distinct paymentFrom
        const uniquePayment = new Set();
        const paymentInfo = response.items
          .filter((item) => {
            const checkItem =
              transactionType === TransactionType.Deposit ? item.paymentTo : item.paymentFrom;
            if (uniquePayment.has(checkItem)) {
              return false;
            }
            if (transactionType === TransactionType.Deposit) {
              uniquePayment.add(item.paymentTo);
            } else {
              uniquePayment.add(item.paymentFrom);
            }
            return true;
          })
          .map((item) =>
            transactionType === TransactionType.Deposit ? item.paymentTo : item.paymentFrom
          );
        setPaymentInformations(paymentInfo);

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
  }, [
        apiParams.transactionType,
        apiParams.dateRange,
        apiParams.account,
        apiParams.status,
        columnFilter,
        refreshTrigger
      ]);

  useEffect(() => {
    const startIndex = (apiParams.pageIndex - 1) * apiParams.pageSize;
    const endIndex = startIndex + apiParams.pageSize;

    if (!localSearchText) {
      setDisplayedItems(apiData.items.slice(startIndex, endIndex));
    } else {
      const lowerCaseSearchText = localSearchText.toLowerCase();
      const filtered = apiData.items.filter(
        (item) =>
          item.username.toLowerCase().includes(lowerCaseSearchText) ||
          item.transId.toString().toLowerCase().includes(lowerCaseSearchText)
      );
    
      setDisplayedItems(filtered.slice(startIndex, endIndex));
    }
  }, [apiData.items, localSearchText, apiParams.pageIndex, apiParams.pageSize]);

  const handleDateRangeChange = (dates: [string, string]) => {
    setApiParams((prev) => ({ ...prev, dateRange: dates, pageIndex: 1 }));
  };

  const handleStatusChange = (value: number) => {
    setApiParams((prev) => ({ ...prev, status: value, pageIndex: 1 }));
  };

  const handleColumnChange = (value: string[]) => {
    setColumnFilter(value);
  };

  const handleAccountChange = (value: string) => {
    setApiParams((prev) => ({ ...prev, account: value, pageIndex: 1 }));
  };

  const handleLocalSearchChange = (value: string) => {
    setLocalSearchText(value);
  };

  const handleRefresh = () => {
    setRefreshTrigger((prev) => prev + 1);
  };

  const handlePageChange = (newPage: number) => {
    setApiParams((prev) => ({ ...prev, pageIndex: newPage }));
  };

  const handlePageSizeChange = (newSize: number) => {
    setApiParams((prev) => ({ ...prev, pageSize: newSize, pageIndex: 1 }));
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
          dateRange={apiParams.dateRange}
          status={apiParams.status}
          column={columnFilter}
          account={apiParams.account}
          paymentInformations={paymentInformations}
          transactionType={transactionType}
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
        <DataGrid
          items={displayedItems}
          pageIndex={apiParams.pageIndex}
          pageSize={apiParams.pageSize}
          isLoading={loading}
          onRefreshNeeded={handleRefresh}
          columnFilter={columnFilter}
        />
        <Pagination
          currentPage={apiParams.pageIndex}
          totalCount={apiData.totalCount}
          pageSize={apiParams.pageSize}
          pageSizeOptions={PAGE_SIZE_OPTIONS}
          onPageChange={handlePageChange}
          onPageSizeChange={handlePageSizeChange}
        />
      </div>
    </div>
  );
};

export default TransactionList;
