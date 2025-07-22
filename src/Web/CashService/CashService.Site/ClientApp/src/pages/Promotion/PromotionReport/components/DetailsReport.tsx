import { useState, useEffect, useRef } from 'react';
import LoadingComponent from '../../../../components/LoadingComponent';
import DetailsReportFilter from './DetailsReportFilter';
import DetailsSummary from './DetailsSummary';
import DetailsReportTableTitle from './DetailsReportTableTitle';
import DetailsReportTable from './DetailsReportTable';
import useExportExcel from '../../../../hooks/useExportExcel';
import { promotionService } from '../../../../services/promotionAPI';
import { formatSimpleDate } from '../../../../utils/sharedUtils';

function useDetailsReportData(filterParams: {
  promotionId: string;
  fromDate: string;
  toDate: string;
  status: string;
}) {
  const [detailsReportData, setDetailsReportData] = useState({
    summary: null,
    promotionDetails: null,
  });
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    async function fetchData() {
      setLoading(true);

      const data = await promotionService.getDetailsReport(filterParams);
      setDetailsReportData(data);

      setLoading(false);
    }

    fetchData();
  }, [filterParams.promotionId, filterParams.fromDate, filterParams.toDate, filterParams.status]);

  return { detailsReportData, loading };
}

export default function DetailsReport({
  promotion,
  onClickBreadcrumb,
  initialDateRange,
}: {
  promotion: any;
  onClickBreadcrumb: () => void;
  initialDateRange: {
    minDate: Date;
    maxDate: Date;
    fromDate: Date;
    toDate: Date;
  };
}) {
  const { promotionId, promotionTitle } = promotion;
  const [dateRange, setDateRange] = useState<[Date | null, Date | null]>([
    initialDateRange.fromDate,
    initialDateRange.toDate,
  ]);
  const [status, setStatus] = useState<string>('0');

  const filterParams = {
    promotionId,
    fromDate: formatSimpleDate(dateRange[0]!),
    toDate: formatSimpleDate(dateRange[1]!),
    status,
  };
  const exportExcel = useExportExcel(
    `ExportDetailsReport?${new URLSearchParams({ ...filterParams, promotionTitle: promotionTitle }).toString()}`
  );
  const tableRef = useRef<HTMLTableElement>(null);
  const [search, setSearch] = useState<string>('');

  const { detailsReportData, loading }: any = useDetailsReportData(filterParams);
  const { summary, promotionDetails } = detailsReportData;

  async function handleExportExcel() {
    await exportExcel();
  }

  const lowerCaseSearch = search ? search.trim().toLowerCase() : '';
  const filteredReportData =
    lowerCaseSearch.length === 0
      ? promotionDetails
      : promotionDetails.filter(
          (data: any) =>
            data.username.toLowerCase().includes(lowerCaseSearch) ||
            (data.nickname && data.nickname.toLowerCase().includes(lowerCaseSearch)) ||
            (data.firstName && data.firstName.toLowerCase().includes(lowerCaseSearch)) ||
            (data.lastName && data.lastName.toLowerCase().includes(lowerCaseSearch))
        );

  return (
    <>
      <DetailsReportFilter
        minDate={initialDateRange.minDate}
        maxDate={initialDateRange.maxDate}
        dateRange={dateRange}
        setDateRange={setDateRange}
        status={status}
        setStatus={setStatus}
      />
      {loading ? (
        <LoadingComponent />
      ) : filteredReportData === null ? null : (
        <>
          <DetailsSummary {...summary} />
          <DetailsReportTableTitle
            promotionTitle={promotionTitle}
            onClickBreadcrumb={onClickBreadcrumb}
            tableRef={tableRef}
            search={search}
            setSearch={setSearch}
            onExportExcel={handleExportExcel}
          />
          <DetailsReportTable ref={tableRef} data={filteredReportData} />
        </>
      )}
    </>
  );
}
