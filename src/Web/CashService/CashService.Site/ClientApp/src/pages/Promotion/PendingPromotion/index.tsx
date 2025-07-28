import React, { useState } from 'react';
import { createRoot } from 'react-dom/client';
import { TitleComponent } from '../../../components/Title';
import PendingPromotionList from './components/PendingPromotionList';
import i18n from '../../../i18n/i18n-promotion';
import { useCookieLanguage } from '../../../hooks/useCookieLanguage';
import '../../../styles/common.css';
import '../../../styles/global.scss';
import { useTranslation } from 'react-i18next';
import AddBonusForm from './components/AddBonusForm';
import { ButtonFullComponent } from '../../../components/Buttons';
import { ToastContainer } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';

const PendingPromotion: React.FC = () => {
  useCookieLanguage(i18n);
  const { t } = useTranslation('promotion');
  const [showAddBonus, setShowAddBonus] = useState(false);
  const [isCashServiceEnabled, setIsCashServiceEnabled] = useState(false);

  React.useEffect(() => {
    const isCashServiceEnabled = document
      .getElementById('IsCashServiceEnabled')
      ?.getAttribute('value');
    setIsCashServiceEnabled(isCashServiceEnabled === '1');
  }, []);

  return (
    <>
      <ToastContainer
        aria-label="toast-container"
        autoClose={3000}
        hideProgressBar={false}
        newestOnTop
        pauseOnFocusLoss
        pauseOnHover
        position="top-center"
      />
      {showAddBonus ? (
        <AddBonusForm onClose={() => setShowAddBonus(false)} />
      ) : (
        <div className="flex h-full flex-col">
          <TitleComponent text={t('pendingPromotion')} type="main" />
          <div className="mb-[8px]">
            <ButtonFullComponent
              text={t('addBonus')}
              onClick={() => setShowAddBonus(true)}
              className="min-w-[183px] [&>*]:min-w-[183px] [&>*]:inline-block"
            />
          </div>
          {showAddBonus && <AddBonusForm onClose={() => setShowAddBonus(false)} />}
          <PendingPromotionList isCashServiceEnabled={isCashServiceEnabled} />
        </div>
      )}
    </>
  );
};

export default PendingPromotion;

if (document.getElementById('root')) {
  createRoot(document.getElementById('root')!).render(
    <React.StrictMode>
      <PendingPromotion />
    </React.StrictMode>
  );
}
