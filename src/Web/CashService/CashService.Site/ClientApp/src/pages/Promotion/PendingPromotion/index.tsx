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
import { MessageComponent } from '../../../components/Message';
import { ResultType } from '../../../components/Message/types';
import UpcomingComponent from '../../../components/UpcomingComponent';

const PendingPromotion: React.FC = () => {
  useCookieLanguage(i18n);
  const { t } = useTranslation('promotion');
  const [showAddBonus, setShowAddBonus] = useState(false);
  const [isCashServiceEnabled, setIsCashServiceEnabled] = useState(false);
  const [resultMessage, setResultMessage] = useState<{
    type: ResultType;
    message: string;
  }>({
    type: 'success',
    message: '',
  });
  React.useEffect(() => {
    const isCashServiceEnabled = document
      .getElementById('IsCashServiceEnabled')
      ?.getAttribute('value');
    setIsCashServiceEnabled(isCashServiceEnabled === '1');
  }, []);

  return (
    <>
      {showAddBonus ? (
        <AddBonusForm
          onClose={() => setShowAddBonus(false)}
          isCashServiceEnabled={isCashServiceEnabled}
        />
      ) : (
        <div className="flex h-full flex-col">
          <UpcomingComponent />
          <TitleComponent text={t('pendingPromotion')} type="main" />
          <div className="flex flex-col gap-2 mb-2">
            <MessageComponent message={resultMessage.message} type={resultMessage.type} />
          </div>
          <div className="mb-[8px]">
            <ButtonFullComponent
              text={t('addBonus')}
              onClick={() => setShowAddBonus(true)}
              className="min-w-[183px] [&>*]:min-w-[183px] [&>*]:inline-block"
            />
          </div>

          <PendingPromotionList
            isCashServiceEnabled={isCashServiceEnabled}
            setResultMessage={setResultMessage}
          />
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
