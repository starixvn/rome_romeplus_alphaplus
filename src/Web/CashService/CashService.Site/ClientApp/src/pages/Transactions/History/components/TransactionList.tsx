import React, { useState, useEffect, useCallback, useMemo } from 'react';
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

const useTransactionData = () => {
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [apiData, setApiData] = useState<TransactionsHistoryResponse>({
    items: [],
    totalCount: 0,
  });

  const fetchTransactions = useCallback(async (params: TransactionsHistoryParams) => {
    try {
      setLoading(true);
      setError(null);

      const response = await transactionAPI.getTransactions(params);
      setApiData(response);

      return response;
    } catch (err) {
      const errorMessage = 'Failed to load transaction history';
      setError(errorMessage);
      console.error('Error loading data:', err);
      setApiData({ items: [], totalCount: 0 });
      throw err;
    } finally {
      setLoading(false);
    }
  }, []);

  return {
    loading,
    error,
    apiData,
    setApiData,
    fetchTransactions,
    setLoading,
    setError,
  };
};

const useApiParams = (transactionType: number) => {
  const initialParams = useMemo(() => {
    const today = new Date();
    const todayInGMT4 = convertToGMT4(today);
    todayInGMT4.setHours(23, 59, 59, 0);

    const sevenDaysAgo = new Date();
    const sevenDaysAgoInGMT4 = convertToGMT4(sevenDaysAgo);
    sevenDaysAgoInGMT4.setDate(sevenDaysAgoInGMT4.getDate() - 6);
    sevenDaysAgoInGMT4.setHours(0, 0, 0, 0);

    return {
      dateRange: [
        sevenDaysAgoInGMT4.toLocaleString('en-US'),
        todayInGMT4.toLocaleString('en-US'),
      ] as [string, string],
      status: -1,
      account: '',
      pageIndex: 1,
      pageSize: PAGE_SIZE_OPTIONS[2],
      transactionType: transactionType,
    };
  }, [transactionType]);

  const [apiParams, setApiParams] =
    useState<Omit<TransactionsHistoryParams, 'searchText' | 'column'>>(initialParams);

  const updateApiParams = useCallback(
    (updates: Partial<Omit<TransactionsHistoryParams, 'searchText' | 'column'>>) => {
      setApiParams((prev) => ({ ...prev, ...updates }));
    },
    []
  );

  return {
    apiParams,
    setApiParams,
    updateApiParams,
  };
};

const useFilters = () => {
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

  const handleColumnChange = useCallback((value: string[]) => {
    setColumnFilter(value);
  }, []);

  const handleLocalSearchChange = useCallback((value: string) => {
    setLocalSearchText(value);
  }, []);

  return {
    columnFilter,
    localSearchText,
    setLocalSearchText,
    handleColumnChange,
    handleLocalSearchChange,
  };
};

const useDisplayedItems = (
  apiData: TransactionsHistoryResponse,
  apiParams: Omit<TransactionsHistoryParams, 'searchText' | 'column'>,
  localSearchText: string,
  transactionType: number
) => {
  const [displayedItems, setDisplayedItems] = useState<TransactionItem[]>([]);
  const [tempTotalCount, setTempTotalCount] = useState(0);

  const searchFilteredItems = useMemo(() => {
    if (!localSearchText.trim()) {
      return apiData.items;
    }

    const lowerCaseSearchText = localSearchText.toLowerCase();
    return apiData.items.filter(
      (item) =>
        item.username.toLowerCase().includes(lowerCaseSearchText) ||
        item.transId.toString().toLowerCase().includes(lowerCaseSearchText)
    );
  }, [apiData.items, localSearchText]);

  const accountFilteredItems = useMemo(() => {
    if (!apiParams.account) {
      return searchFilteredItems;
    }

    return searchFilteredItems.filter(
      (item) =>
        (transactionType === TransactionType.Deposit ? item.paymentTo : item.paymentFrom) ===
        apiParams.account
    );
  }, [searchFilteredItems, apiParams.account, transactionType]);

  useEffect(() => {
    const startIndex = (apiParams.pageIndex - 1) * apiParams.pageSize;
    const endIndex = startIndex + apiParams.pageSize;

    const itemsToDisplay = accountFilteredItems.slice(startIndex, endIndex);
    setDisplayedItems(itemsToDisplay);
    setTempTotalCount(accountFilteredItems.length);
  }, [accountFilteredItems, apiParams.pageIndex, apiParams.pageSize]);

  return {
    displayedItems,
    tempTotalCount,
  };
};

const usePaymentInfo = (apiData: TransactionsHistoryResponse, transactionType: number) => {
  const paymentInformations = useMemo(() => {
    const uniquePayment = new Set<string>();

    return apiData.items
      .filter((item) => {
        const checkItem =
          transactionType === TransactionType.Deposit ? item.paymentTo : item.paymentFrom;

        if (uniquePayment.has(checkItem)) {
          return false;
        }

        uniquePayment.add(checkItem);
        return true;
      })
      .map((item) =>
        transactionType === TransactionType.Deposit ? item.paymentTo : item.paymentFrom
      );
  }, [apiData.items, transactionType]);

  return { paymentInformations };
};

const TransactionList: React.FC<TransactionListProps> = ({ transactionType }) => {
  const { loading, error, apiData, fetchTransactions } = useTransactionData();
  const { apiParams, updateApiParams } = useApiParams(transactionType);
  const { columnFilter, localSearchText, handleColumnChange, handleLocalSearchChange } =
    useFilters();
  const { displayedItems, tempTotalCount } = useDisplayedItems(
    apiData,
    apiParams,
    localSearchText,
    transactionType
  );
  const { paymentInformations } = usePaymentInfo(apiData, transactionType);

  const handleDateRangeChange = useCallback(
    (dates: [string, string]) => {
      updateApiParams({ dateRange: dates, pageIndex: 1 });
    },
    [updateApiParams]
  );

  const handleStatusChange = useCallback(
    (value: number) => {
      updateApiParams({ status: value, pageIndex: 1 });
    },
    [updateApiParams]
  );

  const handleAccountChange = useCallback(
    (value: string) => {
      updateApiParams({ account: value, pageIndex: 1 });
    },
    [updateApiParams]
  );

  const handlePageChange = useCallback(
    (newPage: number) => {
      updateApiParams({ pageIndex: newPage });
    },
    [updateApiParams]
  );

  const handlePageSizeChange = useCallback(
    (newSize: number) => {
      updateApiParams({ pageSize: newSize, pageIndex: 1 });
    },
    [updateApiParams]
  );

  const onDownload = useCallback(() => {
    const paramsForDownload: ExportExcelParams = {
      fromDate: apiParams.dateRange[0],
      toDate: apiParams.dateRange[1],
      status: apiParams.status,
      transactionType: transactionType,
      account: apiParams.account,
    };
    transactionAPI.exportExcel(paramsForDownload);
  }, [apiParams, transactionType]);

  const onRefreshNeeded = useCallback(async () => {
    const paramsForRefresh: TransactionsHistoryParams = {
      ...apiParams,
      searchText: localSearchText,
      transactionType: transactionType,
    };

    try {
      const response = await fetchTransactions(paramsForRefresh);
      return response;
    } catch (error) {
      console.error('Refresh failed:', error);
    }
  }, [apiParams, localSearchText, transactionType, fetchTransactions]);

  useEffect(() => {
    const paramsForFilterOrTypeChange: TransactionsHistoryParams = {
      ...apiParams,
      pageIndex: 1,
      pageSize: PAGE_SIZE_OPTIONS[0],
      searchText: '',
      transactionType: transactionType,
    };

    fetchTransactions(paramsForFilterOrTypeChange);
  }, [transactionType, apiParams.dateRange, apiParams.status, fetchTransactions]);

  useEffect(() => {
    updateApiParams({ pageIndex: 1 });
  }, [transactionType, updateApiParams]);

  useEffect(() => {
    updateApiParams({ pageIndex: 1 });
  }, [localSearchText, apiParams.account, updateApiParams]);

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

      {!loading && tempTotalCount > 0 && (
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
