import React, { useState, useEffect } from 'react';
import { transactionAPI } from '../../../../services/transactionsHistoryAPI';
import {
  TransactionItem,
  TransactionsHistoryResponse,
  TransactionsHistoryParams,
  ExportExcelParams,
} from '../../../../services/interfaces/Transactions/TransactionHistory';
import Filters from './Filters/Filters';
import Search from './Search';
import DataGrid from './DataGrid';
import { PAGE_SIZE_OPTIONS } from '../../../../constants/paymentConstants';
import { convertToGMT4, TransactionType } from '../../../../utils/commonFunctions';
import Pagination from '../../Components/Pagination';

interface TransactionListProps {
  transactionType: number;
}

const TransactionList: React.FC<TransactionListProps> = ({ transactionType }) => {
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
    const todayInGMT4 = convertToGMT4(today);
    todayInGMT4.setHours(23, 59, 59, 0);
    const sevenDaysAgo = new Date();
    const sevenDaysAgoInGMT4 = convertToGMT4(sevenDaysAgo);
    sevenDaysAgoInGMT4.setDate(sevenDaysAgoInGMT4.getDate() - 6);
    sevenDaysAgoInGMT4.setHours(0, 0, 0, 0);
    return {
      dateRange: [sevenDaysAgoInGMT4.toLocaleString('en-US'), todayInGMT4.toLocaleString('en-US')],
      status: -1,
      account: '',
      pageIndex: 1,
      pageSize: PAGE_SIZE_OPTIONS[2],
      transactionType: transactionType,
    };
  });

  const [columnFilter, setColumnFilter] = useState<string[]>([
    'name',
    'amount',
    'from',
    'to',
    'status',
    'type',
    'referenceNo',
    'dateAndTime',
  ]);
  const [localSearchText, setLocalSearchText] = useState('');
  const [displayedItems, setDisplayedItems] = useState<TransactionItem[]>([]);
  const [paymentInformations, setPaymentInformations] = useState<string[]>([]);
  const [tempTotalCount, setTempTotalCount] = useState(0);

  const fetchTransactions = async (params: TransactionsHistoryParams) => {
    try {
      setLoading(true);
      const response = await transactionAPI.getTransactions(params);
      setApiData(response);
      setTempTotalCount(response.totalCount);

      const uniquePayment = new Set();
      const paymentInfo = response.items
        .filter((item) => {
          const checkItem =
            params.transactionType === TransactionType.Deposit ? item.paymentTo : item.paymentFrom;
          if (uniquePayment.has(checkItem)) {
            return false;
          }
          if (params.transactionType === TransactionType.Deposit) {
            uniquePayment.add(item.paymentTo);
          } else {
            uniquePayment.add(item.paymentFrom);
          }
          return true;
        })
        .map((item) =>
          params.transactionType === TransactionType.Deposit ? item.paymentTo : item.paymentFrom
        );
      setPaymentInformations(paymentInfo);
      setApiParams((prev) => ({ ...prev, account: '', pageIndex: 1 }));
      setLocalSearchText('');

      setError(null);
    } catch (err) {
      setError('Failed to load transaction history');
      console.error('Error loading data:', err);
      setApiData({ items: [], totalCount: 0 });
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    const paramsForFilterOrTypeChange: TransactionsHistoryParams = {
      ...apiParams,
      account: '',
      pageIndex: 1,
      pageSize: PAGE_SIZE_OPTIONS[0],
      searchText: '',
      transactionType: transactionType,
    };
    fetchTransactions(paramsForFilterOrTypeChange);
  }, [transactionType, apiParams.dateRange, apiParams.status]);

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

      setDisplayedItems(filtered.slice(0, apiParams.pageSize));
      setTempTotalCount(filtered.length);
    }
  }, [apiData.items, localSearchText, apiParams.pageIndex, apiParams.pageSize]);

  useEffect(() => {
    const startIndex = (apiParams.pageIndex - 1) * apiParams.pageSize;
    const endIndex = startIndex + apiParams.pageSize;
    if (!apiParams.account) {
      setDisplayedItems(apiData.items.slice(startIndex, endIndex));
      setTempTotalCount(apiData.totalCount);
    } else {
      const filtered = apiData.items.filter(
        (item) =>
          (transactionType === TransactionType.Deposit ? item.paymentTo : item.paymentFrom) ===
          apiParams.account
      );
      setDisplayedItems(filtered.slice(startIndex, endIndex));
      setTempTotalCount(filtered.length);
    }
  }, [apiParams.account, apiParams.pageIndex, apiParams.pageSize]);

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

  const handlePageChange = (newPage: number) => {
    setApiParams((prev) => ({ ...prev, pageIndex: newPage }));
    setLocalSearchText(localSearchText);
  };

  const handlePageSizeChange = (newSize: number) => {
    setApiParams((prev) => ({ ...prev, pageSize: newSize, pageIndex: 1 }));
    setLocalSearchText('');
  };

  const onDownload = () => {
    const paramsForDownload: ExportExcelParams = {
      fromDate: apiParams.dateRange[0],
      toDate: apiParams.dateRange[1],
      status: apiParams.status,
      transactionType: transactionType,
    };
    transactionAPI.exportExcel(paramsForDownload);
  };
  const onRefreshNeeded = () => {
    const paramsForRefresh: TransactionsHistoryParams = {
      ...apiParams,
      searchText: localSearchText,
      transactionType: transactionType,
    };
    fetchTransactions(paramsForRefresh);
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

      <div className="filters-container mb-[5px]">
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

      <div className="search-section w-full border border-[var(--table-border-color)] border-b-0 p-[6px]">
        <Search
          value={localSearchText}
          onChange={handleLocalSearchChange}
          placeholder="Username/Reference No."
          items={displayedItems}
          columnFilter={columnFilter}
          onDownload={onDownload}
        />
      </div>

      <DataGrid
        items={displayedItems}
        pageIndex={apiParams.pageIndex}
        pageSize={apiParams.pageSize}
        isLoading={loading}
        columnFilter={columnFilter}
        transactionType={transactionType}
        onRefreshNeeded={onRefreshNeeded}
      />
      {!loading && (
        <div className="mt-[-1px]">
          <Pagination 
            currentPage={apiParams.pageIndex}
            totalItems={tempTotalCount}
            pageSize={apiParams.pageSize}
            onPageChange={handlePageChange}
            onPageSizeChange={handlePageSizeChange}
          />
        </div>
      )}
    </div>
  );
};

export default TransactionList;
