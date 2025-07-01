import { useState, useEffect } from 'react';
import Tippy from '@tippyjs/react';
import { useTranslation } from 'react-i18next';
import NoRecordFoundComponent from '../../../../components/NoRecordFoundComponent';
import {
  StatusEnabledComponent,
  StatusDisabledComponent,
} from '../../../../components/StatusComponents';
import { formatNumber } from '../../../../utils/numberFormatting';
import { convertToGMT4 } from '../../../../utils/commonFunctions';
import { promotionService } from '../../../../services/promotionAPI';
import LoadingComponent from '../../../../components/LoadingComponent';
import { IconCopy, IconExcel, IconSearch } from '../../../../components/Icons';
import DetailsReportFilter from './DetailsReportFilter';
import { PromotionDetailsSummaryProps, PromotionDetailsSummary } from './PromotionDetailsSummary';
import Search from './Search';
import DetailsReportTable from './DetailsReportTable';

function useDetailsReportData(promotionId: number) {
  const [detailsReportData, setDetailsReportData] = useState({
    summary: {} as PromotionDetailsSummaryProps,
    promotionDetails: [],
  });
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    setLoading(true);

    async function fetchData() {
      const data = await promotionService.getDetailsReport(promotionId);
      setDetailsReportData(data);
    }

    fetchData();
    setLoading(false);
  }, [promotionId]);

  return { detailsReportData, loading };
}

export default function DetailsReportContainer({ promotion, onClickBreadcrumb }: any) {
  const today = new Date();
  const todayInGMT4 = convertToGMT4(today);
  todayInGMT4.setHours(23, 59, 59, 0);

  const sevenDaysAgo = new Date();
  const sevenDaysAgoInGMT4 = convertToGMT4(sevenDaysAgo);
  sevenDaysAgoInGMT4.setDate(sevenDaysAgoInGMT4.getDate() - 6);
  sevenDaysAgoInGMT4.setHours(0, 0, 0, 0);

  const [dateRange, setDateRange] = useState<[Date | null, Date | null]>([
    sevenDaysAgoInGMT4,
    todayInGMT4,
  ]);
  const [status, setStatus] = useState<string>('');
  const { detailsReportData, loading }: any = useDetailsReportData(promotion.promotionId);
  const { search, setSearch }: any = useState('');

  function handleFilterChange(queryString: string) {
    // This function can be used to update the report data based on the filter
    // For now, it does nothing as we are not implementing server-side filtering
    console.log('Filter changed: ' + queryString);
  }

  return (
    <>
      <DetailsReportFilter
        dateRange={dateRange}
        setDateRange={setDateRange}
        status={status}
        setStatus={setStatus}
      />
      {loading ? (
        <LoadingComponent />
      ) : (
        <>
          <PromotionDetailsSummary {...detailsReportData.summary} />
          <div className="search-section w-full border border-[var(--table-border-color)] border-b-0 p-[6px]">
            <Search
              promotionTitle={promotion.promotionTitle}
              onClickBreadcrumb={onClickBreadcrumb}
              search={search}
              setSearch={setSearch}
              onDownload={() => {}}
            />
          </div>
          <DetailsReportTable data={detailsReportData.promotionDetails} />
        </>
      )}
    </>
  );
}
