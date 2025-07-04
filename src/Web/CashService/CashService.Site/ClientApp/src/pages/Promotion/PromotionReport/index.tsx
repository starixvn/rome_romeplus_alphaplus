import '../../../styles/common.css';
import '../../../styles/global.scss';
import React, { useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useCookieLanguage } from '../../../hooks/useCookieLanguage';
import i18n from '../../../i18n/i18n-promotion';
import { createRoot } from 'react-dom/client';
import { TitleComponent } from '../../../components/Title';
import MainReport from './components/MainReport';
import DetailsReport from './components/DetailsReport';

function PromotionReport() {
  const { t } = useTranslation('promotion');
  useCookieLanguage(i18n);
  const [promotion, setPromotion]: any = useState(null);

  return (
    <>
      <TitleComponent title={t('promotionReport')} />
      {promotion === null ? (
        <MainReport onClickPromotionTitle={(promotion) => setPromotion(promotion)} />
      ) : (
        <DetailsReport promotion={promotion} onClickBreadcrumb={() => setPromotion(null)} />
      )}
    </>
  );
}

createRoot(document.getElementById('root')!).render(
  <React.StrictMode>
    <PromotionReport />
  </React.StrictMode>
);
