import '../../../styles/common.css';
import '../../../styles/global.scss';
import React, { useState } from 'react';
import i18n from '../../../i18n/i18n-promotion';
import { useCookieLanguage } from '../../../hooks/useCookieLanguage';
import { createRoot } from 'react-dom/client';
import { TitleComponent } from '../../../components/Title';
import MainReportContainer from './components/MainReport';
import DetailsReportContainer from './components/DetailsReport';
import {
  PromotionDetailsSummaryProps,
  PromotionDetailsSummary,
} from './components/PromotionDetailsSummary.tsx';
import PromotionDetailsTable from './components/PromotionDetailsTable.tsx';

function PromotionReport() {
  useCookieLanguage(i18n);

  const [promotion, setPromotion]: any = useState(null);
  const [promotionDetailsSummaryData, setPromotionDetailsSummaryData] =
    useState<PromotionDetailsSummaryProps>();

  return (
    <>
      <TitleComponent title="Promotion Report" />
      {promotionDetailsSummaryData && <PromotionDetailsSummary {...promotionDetailsSummaryData} />}
      {promotion === null ? (
        <MainReportContainer
          onClickPromotionTitle={(id: number, name: string) => {
            setPromotion({ promotionId: id, promotionTitle: name });
          }}
        />
      ) : (
        <DetailsReportContainer
          promotion={promotion}
          onClickBreadcrumb={() => setPromotion(null)}
        />
      )}
    </>
  );
}

createRoot(document.getElementById('root')!).render(
  <React.StrictMode>
    <PromotionReport />
  </React.StrictMode>
);
