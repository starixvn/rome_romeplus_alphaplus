import { useState, useEffect, useRef } from 'react';
import LoadingComponent from '../../../../components/LoadingComponent';
import MainReportFilter from './MainReportFilter';
import MainReportTableTitle from './MainReportTableTitle';
import MainReportTable from './MainReportTable';
import useExportExcel from '../../../../hooks/useExportExcel';
import { promotionService } from '../../../../services/promotionAPI';
import { convertToGMT4 } from '../../../../utils/commonFunctions';

function useMainReportData(
  fromDate: string,
  toDate: string,
  promotionType: string,
  status: string
) {
  const [mainReportData, setMainReportData] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    setLoading(true);

    async function fetchData() {
      const data = await promotionService.getMainReport(fromDate, toDate, promotionType, status);
      setMainReportData(data);
    }

    fetchData();
    setLoading(false);
  }, [fromDate, toDate, promotionType, status]);

  return { mainReportData, loading };
}

export default function MainReport({
  onClickPromotionTitle,
}: {
  onClickPromotionTitle: (promotion: any) => void;
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
  const [promotionType, setPromotionType] = useState<string>('');
  const [status, setStatus] = useState<string>('');

  const exportExcel = useExportExcel('ExportMainReport');
  const tableRef = useRef<HTMLTableElement>(null);
  const [search, setSearch] = useState('');

  const { mainReportData, loading }: any = useMainReportData(
    dateRange[0]!.toLocaleString('en-US'),
    dateRange[1]!.toLocaleString('en-US'),
    promotionType,
    status
  );

  async function handleExportExcel() {
    await exportExcel();
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
        dateRange={dateRange}
        setDateRange={setDateRange}
        promotionType={promotionType}
        setPromotionType={setPromotionType}
        status={status}
        setStatus={setStatus}
      />
      {loading ? (
        <LoadingComponent />
      ) : (
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
            onClickPromotionTitle={onClickPromotionTitle}
          />
        </>
      )}
    </>
  );
}
