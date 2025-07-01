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
import MainReportFilter from './MainReportFilter';

interface PromotionListProps {
  data: any[];
  onClickPromotionTitle: (promotionId: number, promotionTitle: string) => void;
}

interface SearchProps {
  search: string;
  setSearch: (val: string) => void;
  onExportExcel: () => void;
}

function ReportTitle({ search, setSearch, onExportExcel }: SearchProps) {
  const { t: tGlobal } = useTranslation('global');
  const [textCopy, setTextCopy] = useState(tGlobal('copy'));

  async function handleExportExcel() {
    onExportExcel();
  }

  function handleCopy() {
    //navigator.clipboard.writeText(search);
    setTextCopy(tGlobal('copied'));
    setTimeout(() => setTextCopy(tGlobal('copy')), 3000);
  }

  return (
    <div className="flex items-center w-full justify-between">
      <div></div>
      <div className="flex items-center gap-[8px]">
        <Tippy
          content={tGlobal('exportToExcel')}
          placement="top"
          theme="light"
          arrow={true}
          hideOnClick={false}
        >
          <button
            onClick={handleExportExcel}
            className="p-[4px] bg-black/10 rounded-[3px] cursor-pointer"
          >
            <IconExcel />
          </button>
        </Tippy>
        <Tippy content={textCopy} placement="top" theme="light" arrow={true} hideOnClick={false}>
          <button onClick={handleCopy} className="p-[4px] bg-black/10 rounded-[3px] cursor-pointer">
            <IconCopy />
          </button>
        </Tippy>
        <div className="flex">
          <div className="w-[250px] ml-auto relative">
            <span className="absolute transform-[translatey(-50%)] z-[10] top-1/2 left-[8px]">
              <IconSearch />
            </span>
            <input
              type="text"
              value={search}
              onChange={(e) => setSearch(e.target.value)}
              className="control-field pl-[30px]!"
              placeholder="Promotion Title"
              autoComplete="off"
            />
          </div>
        </div>
      </div>
    </div>
  );
}

function useReportData() {
  const [reportData, setReportData] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    setLoading(true);

    async function fetchData() {
      const data = await promotionService.getMainReport();
      setReportData(data);
    }

    fetchData();
    setLoading(false);
  }, []);

  return { reportData, loading };
}

function ReportRow({ idx, item, onClickPromotionTitle }: any) {
  const handleDrillDown = (
    event: React.MouseEvent<HTMLAnchorElement>,
    promotionId: number,
    promotionTitle: string
  ) => {
    event.preventDefault();
    onClickPromotionTitle(promotionId, promotionTitle);
  };

  const mapStatus = (status: number) => {
    if (status === 1) {
      return <StatusEnabledComponent />;
    }

    return <StatusDisabledComponent />;
  };

  return (
    <tr>
      <td className="w-[32px] min-w-[32px] sticky left-0 bg-[var(--table-background)] z-10">
        {formatNumber(idx + 1)}
      </td>
      <td className="text-left sticky left-[32px] bg-[var(--table-background)] z-10 shadow-[3px_0_4px_0_rgba(99,84,84,0.3)]">
        <a
          href="#"
          onClick={(event) => handleDrillDown(event, item.promotionId, item.promotionTitle)}
          className="text-[#045ace] dark:text-[#41a4ff] hover:underline"
        >
          {item.promotionTitle}
        </a>
      </td>
      <td>{item.promotionType}</td>
      <td>{item.dateRange}</td>
      <td className="text-right">{formatNumber(item.totalRequest)}</td>
      <td className="text-right">
        <div>{formatNumber(item.totalApproved)}</div>
        <div className="text-[#CC0000] dark:text-[#F23D3D]">{formatNumber(item.totalRejected)}</div>
      </td>
      <td className="text-right">{formatNumber(item.totalBonus)}</td>
      <td>{mapStatus(item.status)}</td>
    </tr>
  );
}

function ReportTable({ data, onClickPromotionTitle }: PromotionListProps) {
  const [search, setSearch] = useState<string>('');

  return (
    <>
      <div className="search-section w-full border border-[var(--table-border-color)] border-b-0 p-[6px]">
        <ReportTitle search={search} setSearch={setSearch} onExportExcel={() => {}} />
      </div>
      <div className="table-fixed-header table-fixed-body min-h-0">
        <table className="w-full whitespace-nowrap border-collapse border border-[#0000001f] border-t-[0px] border-l-[0px]">
          <thead className="z-11!">
            <tr>
              <th className="w-[32px] min-w-[32px] sticky left-0 bg-[var(--table-header-background)] z-11">
                #
              </th>
              <th className="sticky left-[32px] bg-[var(--table-header-background)] shadow-[3px_0_4px_0_rgba(99,84,84,0.3)]">
                Promotion Title
              </th>
              <th>Promotion Type</th>
              <th>Date Range</th>
              <th>Total Request</th>
              <th>
                <div>Total Request Approved</div>
                <div>Total Request Rejected</div>
              </th>
              <th>Total Bonus Given</th>
              <th className="w-[140px] min-w-[140px]">Status</th>
            </tr>
          </thead>
          <tbody className="font-[500]">
            {data.length === 0 ? (
              <NoRecordFoundComponent colSpan={8} />
            ) : (
              data.map((item, idx) => (
                <ReportRow
                  key={item.promotionId}
                  idx={idx}
                  item={item}
                  onClickPromotionTitle={onClickPromotionTitle}
                />
              ))
            )}
          </tbody>
        </table>
      </div>
    </>
  );
}

export default function MainReportContainer({ onClickPromotionTitle }: any) {
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
  //const [filter, setFilter] = useState('');
  const { reportData, loading } = useReportData();

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
      {/* <MainReportFilter onFilterChange={handleFilterChange} /> */}
      {loading ? (
        <LoadingComponent />
      ) : (
        <ReportTable data={reportData} onClickPromotionTitle={onClickPromotionTitle} />
      )}
    </>
  );
}
