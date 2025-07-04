import { useState, useEffect, useRef } from 'react';
import LoadingComponent from '../../../../components/LoadingComponent';
import DetailsReportFilter from './DetailsReportFilter';
import DetailsSummary from './DetailsSummary';
import DetailsReportTableTitle from './DetailsReportTableTitle';
import DetailsReportTable from './DetailsReportTable';
import useExportExcel from '../../../../hooks/useExportExcel';
import { promotionService } from '../../../../services/promotionAPI';
import { convertToGMT4 } from '../../../../utils/commonFunctions';
import { set } from 'date-fns';

function useDetailsReportData(
  promotionId: string,
  fromDate: string,
  toDate: string,
  status: string
) {
  const [detailsReportData, setDetailsReportData] = useState({
    summary: {},
    promotionDetails: [],
  });
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    setLoading(true);

    async function fetchData() {
      const data = await promotionService.getDetailsReport(promotionId, fromDate, toDate, status);
      setDetailsReportData(data);
    }

    fetchData();
    setLoading(false);
  }, [promotionId, fromDate, toDate, status]);

  return { detailsReportData, loading };
}

export default function DetailsReport({
  promotion,
  onClickBreadcrumb,
}: {
  promotion: any;
  onClickBreadcrumb: () => void;
}) {
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

  const exportExcel = useExportExcel('ExportDetailsReport');
  const tableRef = useRef<HTMLTableElement>(null);
  const [search, setSearch] = useState<string>('');

  const { promotionId, promotionTitle } = promotion;
  const { detailsReportData, loading }: any = useDetailsReportData(
    promotionId,
    dateRange[0]!.toLocaleString('en-US'),
    dateRange[1]!.toLocaleString('en-US'),
    status
  );
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
        dateRange={dateRange}
        setDateRange={setDateRange}
        status={status}
        setStatus={setStatus}
      />
      {loading ? (
        <LoadingComponent />
      ) : (
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
