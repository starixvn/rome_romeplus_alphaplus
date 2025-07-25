import { useState, useEffect, useRef } from 'react';
import LoadingComponent from '../../../../components/LoadingComponent';
import MainReportFilter from './MainReportFilter';
import MainReportTableTitle from './MainReportTableTitle';
import MainReportTable from './MainReportTable';
import useExportExcel from '../../../../hooks/useExportExcel';
import { promotionService } from '../../../../services/promotionAPI';
import { formatSimpleDate } from '../../../../utils/sharedUtils';

function useMainReportData(filterParams: {
  fromDate: string;
  toDate: string;
  promotionType: string;
  status: string;
}) {
  const [mainReportData, setMainReportData] = useState(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    async function fetchData() {
      setLoading(true);

      const data = await promotionService.getMainReport(filterParams);
      setMainReportData(data);

      setLoading(false);
    }

    fetchData();
  }, [filterParams.fromDate, filterParams.toDate, filterParams.promotionType, filterParams.status]);

  return { mainReportData, loading };
}

export default function MainReport({
  onClickPromotionTitle,
  initialFilterParams,
}: {
  onClickPromotionTitle: (promotion: any, filterParams: any) => void;
  initialFilterParams: {
    minDate: Date;
    maxDate: Date;
    fromDate: Date;
    toDate: Date;
    promotionType: string;
    status: string;
  };
}) {
  const [dateRange, setDateRange] = useState<[Date | null, Date | null]>([
    initialFilterParams.fromDate,
    initialFilterParams.toDate,
  ]);
  const [promotionType, setPromotionType] = useState<string>(initialFilterParams.promotionType);
  const [status, setStatus] = useState<string>(initialFilterParams.status);

  const filterParams = {
    fromDate: formatSimpleDate(dateRange[0]!),
    toDate: formatSimpleDate(dateRange[1]!),
    promotionType,
    status,
  };
  const exportExcel = useExportExcel(
    `ExportMainReport?${new URLSearchParams(filterParams).toString()}`
  );
  const tableRef = useRef<HTMLTableElement>(null);
  const [search, setSearch] = useState('');

  const { mainReportData, loading }: any = useMainReportData(filterParams);

  async function handleExportExcel() {
    await exportExcel();
  }

  function handleClickPromotionTitle(promotion: any) {
    onClickPromotionTitle(promotion, {
      promotionType: promotionType,
      status: status,
    });
  }

  const lowerCaseSearch = search ? search.trim().toLowerCase() : '';
  const filteredReportData =
    lowerCaseSearch.length === 0
      ? mainReportData
      : mainReportData.filter((data: any) =>
          data.promotionTitle.toLowerCase().includes(lowerCaseSearch)
        );

  return (
    <>
      <MainReportFilter
        minDate={initialFilterParams.minDate}
        maxDate={initialFilterParams.maxDate}
        dateRange={dateRange}
        setDateRange={setDateRange}
        promotionType={promotionType}
        setPromotionType={setPromotionType}
        status={status}
        setStatus={setStatus}
      />
      {loading ? (
        <LoadingComponent />
      ) : filteredReportData === null ? null : (
        <>
          <MainReportTableTitle
            tableRef={tableRef}
            search={search}
            setSearch={setSearch}
            onExportExcel={handleExportExcel}
          />
          <MainReportTable
            ref={tableRef}
            data={filteredReportData}
            onClickPromotionTitle={handleClickPromotionTitle}
          />
        </>
      )}
    </>
  );
}
