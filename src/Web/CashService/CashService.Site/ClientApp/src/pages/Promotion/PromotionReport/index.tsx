import '../../../styles/common.css';
import '../../../styles/global.scss';
import React, { useState } from 'react';
import i18n from '../../../i18n/i18n-promotion';
import { useCookieLanguage } from '../../../hooks/useCookieLanguage';
import { createRoot } from 'react-dom/client';
import { TitleComponent } from '../../../components/Title';
import Filter from './components/Filter.tsx';
import Search from './components/Search.tsx';
import PromotionTable from './components/PromotionTable.tsx';
import {
  PromotionDetailsSummaryProps,
  PromotionDetailsSummary,
} from './components/PromotionDetailsSummary.tsx';
import PromotionDetailsTable from './components/PromotionDetailsTable.tsx';
import { convertToGMT4 } from '../../../utils/commonFunctions';

function PromotionReport() {
  useCookieLanguage(i18n);
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
  const [search, setSearch] = useState<string>('');
  const [promotionId, setPromotionId] = useState(0);
  const [promotionDetailsSummaryData, setPromotionDetailsSummaryData] =
    useState<PromotionDetailsSummaryProps>();

  const mockData: any[] = [
    {
      promotionId: 1,
      promotionTitle: 'New Year Sale',
      promotionType: 'Discount',
      period: '01 Jan 2023 - 31 Jan 2023',
      totalRequest: 100,
      totalApproved: 80,
      totalRejected: 20,
      totalBonus: 5000,
      status: 1,
    },
    {
      promotionId: 2,
      promotionTitle: 'Summer Sale',
      promotionType: 'Cashback',
      period: '01 Jun 2023 - 30 Jun 2023',
      totalRequest: 150,
      totalApproved: 120,
      totalRejected: 30,
      totalBonus: 7500,
      status: 1,
    },
    {
      promotionId: 3,
      promotionTitle: 'Black Friday',
      promotionType: 'Discount',
      period: '01 Nov 2023 - 30 Nov 2023',
      totalRequest: 200,
      totalApproved: 180,
      totalRejected: 20,
      totalBonus: 10000,
      status: 0,
    },
  ];
  const promotionListData = mockData;

  const [promotionDetailsData, setPromotionDetailsData] = useState([]);

  //const fetchPromotionDetails = useCallback(async () => {}, [promotionId]);

  return (
    <div className="">
      <TitleComponent title="Promotion Report" />
      <Filter
        dateRange={dateRange}
        setDateRange={setDateRange}
        promotionType={promotionType}
        setPromotionType={setPromotionType}
        status={status}
        setStatus={setStatus}
      />
      {promotionDetailsSummaryData && <PromotionDetailsSummary {...promotionDetailsSummaryData} />}
      <div className="search-section w-full border border-[var(--table-border-color)] border-b-0 p-[6px]">
        <Search search={search} setSearch={setSearch} onDownload={() => {}} />
      </div>
      {promotionId === 0 ? (
        <PromotionTable
          data={promotionListData}
          onClickPromotionTitle={(id) => {
            setPromotionId(id);
            setPromotionDetailsSummaryData({
              totalBonus: 0.55,
              totalApproved: 1,
              totalRejected: 1,
            });
            const mockData: any = [
              {
                username: 'superuser010101abc',
                firstName: 'Jane',
                lastName: 'Doe',
                promotionType: 'Summer Sale',
                dateRange: '01 Jun 2023 - 30 Jun 2023',
                bonusAmount: 120,
                lastUpdated: '30 Jun 2023 15:45:59',
                status: 2,
              },
              {
                username: 'stellar_wanderer_9284',
                firstName: 'Alice',
                lastName: 'Smith',
                promotionType: 'Black Friday',
                dateRange: '01 Nov 2023 - 30 Nov 2023',
                bonusAmount: 180,
                lastUpdated: '05 Nov 2023 10:30:00',
                status: 3,
              },
              {
                username: 'quantum_flux_master7',
                firstName: 'Bob',
                lastName: 'Johnson',
                promotionType: 'New Year Sale',
                dateRange: '01 Jan 2023 - 31 Jan 2023',
                bonusAmount: 80,
                lastUpdated: '02 Jan 2023 12:00:00',
                status: 1,
              },
            ];
            setPromotionDetailsData(mockData);
          }}
        />
      ) : (
        <PromotionDetailsTable data={promotionDetailsData} />
      )}
    </div>
  );
}

createRoot(document.getElementById('root')!).render(
  <React.StrictMode>
    <PromotionReport />
  </React.StrictMode>
);
