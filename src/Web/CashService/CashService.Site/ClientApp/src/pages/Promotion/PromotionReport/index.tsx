import '../../../styles/common.css';
import '../../../styles/global.scss';
import React, { useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useCookieLanguage } from '../../../hooks/useCookieLanguage';
import i18n from '../../../i18n/i18n-promotion';
import { createRoot } from 'react-dom/client';
import { TitleComponent } from '../../../components/Title';
import PromotionReportTab from './components/PromotionReportTab';
import MainReport from './components/MainReport';
import DetailsReport from './components/DetailsReport';
import ManualBonusReport from './components/ManualBonusReport';
import { convertToGMT4 } from '../../../utils/commonFunctions';

function calculatedDates() {
  // Get today in GMT-4
  const today = new Date();
  const todayInGMT4 = convertToGMT4(today);
  todayInGMT4.setHours(0, 0, 0, 0);

  // Max Date is tomorrow in GMT-4
  let maxDate = new Date(todayInGMT4);
  maxDate = new Date(maxDate.getFullYear() + 1, 11, 31);

  // Min Date
  let minDate;
  const limitedDateToShowInfo = document.getElementById(
    'limitedDateToShowInfo'
  ) as HTMLInputElement;
  if (limitedDateToShowInfo) {
    minDate = new Date(limitedDateToShowInfo.value);
  } else {
    const currentYear = todayInGMT4.getFullYear();
    minDate = new Date(currentYear - 3, 0, 1);
  }

  // From Date is 7 days ago in GMT-4
  const sevenDaysAgo = new Date();
  const sevenDaysAgoInGMT4 = convertToGMT4(sevenDaysAgo);
  sevenDaysAgoInGMT4.setDate(sevenDaysAgoInGMT4.getDate() - 6);
  sevenDaysAgoInGMT4.setHours(0, 0, 0, 0);

  return {
    minDate,
    maxDate,
    fromDate: sevenDaysAgoInGMT4 > minDate ? sevenDaysAgoInGMT4 : minDate,
    toDate: todayInGMT4,
  };
}

function PromotionReport() {
  const { t } = useTranslation('promotion');
  useCookieLanguage(i18n);
  const [initialDates, _] = useState(calculatedDates);
  const [activeTab, setActiveTab] = useState(0);
  const [promotion, setPromotion] = useState(null);
  const [mainReportFilterParams, setMainReportFilterParams] = useState({
    ...initialDates,
    promotionType: '0',
    status: '2',
  });

  function handleTabChange(tab: number) {
    setActiveTab(tab);
    if (tab === 0) {
      setPromotion(null);
      setMainReportFilterParams({
        ...initialDates,
        promotionType: '0',
        status: '2',
      });
    }
  }

  function handleClickPromotionTitle(promotion: any, filterParams: any) {
    setMainReportFilterParams((prevFilter: any) => ({
      ...prevFilter,
      ...filterParams,
    }));
    setPromotion(promotion);
  }

  return (
    <>
      <TitleComponent text={t('promotionReport')} type="main" />
      <PromotionReportTab activeTab={activeTab} onTabChange={handleTabChange} />
      {activeTab === 0 ? (
        promotion === null ? (
          <MainReport
            onClickPromotionTitle={handleClickPromotionTitle}
            initialFilterParams={mainReportFilterParams}
          />
        ) : (
          <DetailsReport
            promotion={promotion}
            onClickBreadcrumb={() => setPromotion(null)}
            initialDateRange={{
              minDate: mainReportFilterParams.minDate,
              maxDate: mainReportFilterParams.maxDate,
            }}
          />
        )
      ) : (
        <ManualBonusReport {...initialDates} />
      )}
    </>
  );
}

createRoot(document.getElementById('root')!).render(
  <React.StrictMode>
    <PromotionReport />
  </React.StrictMode>
);
