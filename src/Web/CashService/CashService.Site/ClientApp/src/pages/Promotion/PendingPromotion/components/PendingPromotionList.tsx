import React, { useState, useEffect, useCallback, useMemo } from 'react';
import { promotionService } from '../../../../services/promotionAPI';
import {
  PromotionTransaction,
  PromotionTransactionRequest,
} from '../../../../services/interfaces/Promotion';
import Filters from './Filters/Filters';
import Search from './Search';
import DataGrid from './DataGrid';
import { PAGE_SIZE_OPTIONS } from '../../../../constants/paymentConstants';
import { convertToGMT4 } from '../../../../utils/commonFunctions';
import Pagination from '../../../Transactions/Components/Pagination';
import { useTranslation } from 'react-i18next';
import ConfirmPopup from '../../../../components/ConfirmPopup';

interface PendingPromotionListProps {
  isCashServiceEnabled: boolean;
}

const usePendingPromotionData = () => {
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [apiData, setApiData] = useState<PromotionTransaction[]>([]);
  const [selectedItems, setSelectedItems] = useState<PromotionTransaction[]>([]);
  const fetchPendingPromotions = useCallback(async (params: PromotionTransactionRequest) => {
    try {
      setLoading(true);
      setError(null);

      const response = await promotionService.getPendingPromotionTransactions(params);
      setApiData(response.promotionTransactions ?? []);

      return response;
    } catch (err) {
      const errorMessage = 'Failed to load pending promotions';
      setError(errorMessage);
      console.error('Error loading data:', err);
      setApiData([]);
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
    fetchPendingPromotions,
    setLoading,
    setError,
    selectedItems,
    setSelectedItems,
  };
};

const useApiParams = () => {
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
      promotionTypeId: 0,
      pageIndex: 1,
      pageSize: PAGE_SIZE_OPTIONS[2],
    };
  }, []);

  const [apiParams, setApiParams] = useState(initialParams);

  const updateApiParams = useCallback((updates: Partial<typeof initialParams>) => {
    setApiParams((prev) => ({ ...prev, ...updates }));
  }, []);

  return {
    apiParams,
    setApiParams,
    updateApiParams,
  };
};

const useFilters = () => {
  const [localSearchText, setLocalSearchText] = useState('');

  const handleLocalSearchChange = useCallback((value: string) => {
    setLocalSearchText(value);
  }, []);

  return {
    localSearchText,
    setLocalSearchText,
    handleLocalSearchChange,
  };
};

const useDisplayedItems = (
  apiData: PromotionTransaction[],
  apiParams: any,
  localSearchText: string
) => {
  const [displayedItems, setDisplayedItems] = useState<PromotionTransaction[]>([]);
  const [tempTotalCount, setTempTotalCount] = useState(0);

  const searchFilteredItems = useMemo(() => {
    if (!localSearchText.trim()) {
      return apiData;
    }

    const lowerCaseSearchText = localSearchText.toLowerCase();
    return apiData.filter(
      (item) =>
        item.username.toLowerCase().includes(lowerCaseSearchText) ||
        item.firstName.toLowerCase().includes(lowerCaseSearchText) ||
        item.lastName.toLowerCase().includes(lowerCaseSearchText) ||
        item.title.toLowerCase().includes(lowerCaseSearchText)
    );
  }, [apiData, localSearchText]);

  useEffect(() => {
    const startIndex = (apiParams.pageIndex - 1) * apiParams.pageSize;
    const endIndex = startIndex + apiParams.pageSize;

    const itemsToDisplay = searchFilteredItems.slice(startIndex, endIndex);
    setDisplayedItems(itemsToDisplay);
    setTempTotalCount(searchFilteredItems.length);
  }, [searchFilteredItems, apiParams.pageIndex, apiParams.pageSize]);

  return {
    displayedItems,
    tempTotalCount,
  };
};

const PendingPromotionList: React.FC<PendingPromotionListProps> = () => {
  const { t } = useTranslation('promotion');
  const { t: tGlobal } = useTranslation('global');
  const { loading, error, apiData, fetchPendingPromotions, selectedItems, setSelectedItems } =
    usePendingPromotionData();
  const { apiParams, updateApiParams } = useApiParams();
  const { localSearchText, handleLocalSearchChange } = useFilters();
  const { displayedItems, tempTotalCount } = useDisplayedItems(apiData, apiParams, localSearchText);
  const [showConfirm, setShowConfirm] = useState<string | null>(null);

  const handleDateRangeChange = useCallback(
    (dates: [string, string]) => {
      updateApiParams({ dateRange: dates, pageIndex: 1 });
    },
    [updateApiParams]
  );

  const handlePromotionTypeChange = useCallback(
    (value: number) => {
      updateApiParams({ promotionTypeId: value, pageIndex: 1 });
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

  const handleSelectionChange = useCallback(
    (selectedItems: PromotionTransaction[]) => {
      setSelectedItems(selectedItems);
    },
    [setSelectedItems]
  );

  const onDownload = useCallback(() => {
    console.log('Download functionality to be implemented');
  }, []);

  const onReject = useCallback(() => {
    selectedItems.forEach((item) => {
      promotionService.rejectPromotionTransaction(item.custId, item.transId, item.amount);
    });
    setShowConfirm(null);
  }, []);

  const onApprove = useCallback(() => {
    selectedItems.forEach((item) => {
      promotionService.approvePromotionTransaction(item.custId, item.transId, item.amount);
    });
    setShowConfirm(null);
  }, []);

  useEffect(() => {
    const paramsForFilterChange: PromotionTransactionRequest = {
      promotionTypeId: apiParams.promotionTypeId,
      fromDate: apiParams.dateRange[0],
      toDate: apiParams.dateRange[1],
    };

    fetchPendingPromotions(paramsForFilterChange);
  }, [apiParams.dateRange, apiParams.promotionTypeId, fetchPendingPromotions]);

  useEffect(() => {
    updateApiParams({ pageIndex: 1 });
  }, [localSearchText, updateApiParams]);

  return (
    <>
      {showConfirm === 'approve' && (
        <ConfirmPopup
          title={t('table.confirmApproveTitle')}
          message={t('table.confirmApproveMessage')}
          onCancel={() => setShowConfirm(null)}
          onConfirm={() => {
            setShowConfirm(null);
            onApprove();
          }}
          cancelText={tGlobal('cancel')}
          confirmText={tGlobal('confirm')}
        />
      )}
      {showConfirm === 'reject' && (
        <ConfirmPopup
          title={t('table.confirmRejectTitle')}
          message={t('table.confirmRejectMessage')}
          onCancel={() => setShowConfirm(null)}
          onConfirm={() => {
            setShowConfirm(null);
            onReject();
          }}
          cancelText={tGlobal('cancel')}
          confirmText={tGlobal('confirm')}
        />
      )}
      <div className="pending-promotion-list-container">
        {error && (
          <div
            className="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded relative mb-4"
            role="alert"
          >
            <strong className="font-bold">Error!</strong>
            <span className="block sm:inline"> {error}</span>
          </div>
        )}
        <Filters
          dateRange={apiParams.dateRange}
          promotionTypeId={apiParams.promotionTypeId}
          onDateRangeChange={handleDateRangeChange}
          onPromotionTypeChange={handlePromotionTypeChange}
        />
        <div className="w-full border border-[var(--table-border-color)] border-b-0 p-[6px]">
          <Search
            value={localSearchText}
            onChange={handleLocalSearchChange}
            placeholder={t('filter.searchPlaceholder')}
            items={displayedItems}
            selectedItems={selectedItems}
            onReject={() => setShowConfirm('reject')}
            onApprove={() => setShowConfirm('approve')}
            onDownload={onDownload}
          />
        </div>

        <DataGrid
          items={displayedItems}
          pageIndex={apiParams.pageIndex}
          pageSize={apiParams.pageSize}
          isLoading={loading}
          onSelectionChange={handleSelectionChange}
          refreshGrid={() => {
            fetchPendingPromotions({
              promotionTypeId: apiParams.promotionTypeId,
              fromDate: apiParams.dateRange[0],
              toDate: apiParams.dateRange[1],
            });
          }}
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
    </>
  );
};

export default PendingPromotionList;
