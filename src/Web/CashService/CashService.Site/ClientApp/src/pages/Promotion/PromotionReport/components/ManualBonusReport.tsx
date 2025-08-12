import { useState, useEffect, useRef } from 'react';
import LoadingComponent from '../../../../components/LoadingComponent';
import ManualBonusReportFilter from './ManualBonusReportFilter';
import ManualBonusReportTableTitle from './ManualBonusReportTableTitle';
import ManualBonusReportTable from './ManualBonusReportTable';
import useExportExcel from '../../../../hooks/useExportExcel';
import { promotionService } from '../../../../services/promotionAPI';
import { formatSimpleDate } from '../../../../utils/sharedUtils';

function useManualBonusReportData(filterParams: { fromDate: string; toDate: string }) {
  const [manualBonusReportData, setManualBonusReportData] = useState(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    async function fetchData() {
      setLoading(true);

      const data = await promotionService.getManualBonusReport(filterParams);
      setManualBonusReportData(data);

      setLoading(false);
    }

    fetchData();
  }, [filterParams.fromDate, filterParams.toDate]);

  return { manualBonusReportData, loading };
}

export default function ManualBonusReport({
  minDate,
  maxDate,
  fromDate,
  toDate,
}: {
  minDate: Date;
  maxDate: Date;
  fromDate: Date;
  toDate: Date;
}) {
  const [dateRange, setDateRange] = useState<[Date | null, Date | null]>([fromDate, toDate]);

  const filterParams = {
    fromDate: formatSimpleDate(dateRange[0]!),
    toDate: formatSimpleDate(dateRange[1]!),
  };
  const exportExcel = useExportExcel(
    `ExportManualBonusReport?${new URLSearchParams(filterParams).toString()}`
  );
  const tableRef = useRef<HTMLTableElement>(null);
  const [search, setSearch] = useState('');

  const { manualBonusReportData, loading }: any = useManualBonusReportData(filterParams);

  async function handleExportExcel() {
    await exportExcel();
  }

  const lowerCaseSearch = search ? search.trim().toLowerCase() : '';
  const filteredReportData =
    lowerCaseSearch.length === 0
      ? manualBonusReportData
      : manualBonusReportData.filter(
          (data: any) =>
            data.username.toLowerCase().includes(lowerCaseSearch) ||
            (data.nickname && data.nickname.toLowerCase().includes(lowerCaseSearch)) ||
            (data.firstName && data.firstName.toLowerCase().includes(lowerCaseSearch)) ||
            (data.lastName && data.lastName.toLowerCase().includes(lowerCaseSearch))
        );

  return (
    <>
      <ManualBonusReportFilter
        minDate={minDate}
        maxDate={maxDate}
        dateRange={dateRange}
        setDateRange={setDateRange}
      />
      {loading ? (
        <LoadingComponent />
      ) : filteredReportData === null ? null : (
        <>
          <ManualBonusReportTableTitle
            tableRef={tableRef}
            search={search}
            setSearch={setSearch}
            onExportExcel={handleExportExcel}
          />
          <ManualBonusReportTable ref={tableRef} data={filteredReportData} />
        </>
      )}
    </>
  );
}
