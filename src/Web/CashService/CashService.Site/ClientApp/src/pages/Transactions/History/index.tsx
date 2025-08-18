import React, { useState } from 'react';
import { createRoot } from 'react-dom/client';
import TransactionList from './components/TransactionList';
import HistoryTab from './components/HistoryTab';
import i18n from '../../../i18n/i18n-transactionHistory';
import { useCookieLanguage } from '../../../hooks/useCookieLanguage';
import '../../../styles/common.css';
import '../../../styles/global.scss';
import { TitleComponent } from '../../../components/Title';
import { useTranslation } from 'react-i18next';

const History: React.FC = () => {
  useCookieLanguage(i18n);
  const [activeTab, setActiveTab] = useState<number>(1);
  const { t: tTransactionHistory } = useTranslation('transactionHistory');
  const [isCashServiceEnabled, setIsCashServiceEnabled] = useState(false);

  React.useEffect(() => {
    const isCashServiceEnabled = document
      .getElementById('IsCashServiceEnabled')
      ?.getAttribute('value');
    setIsCashServiceEnabled(isCashServiceEnabled === '1');
  }, []);

  const handleTabChange = (tab: number) => {
    setActiveTab(tab);
  };

  React.useEffect(() => {
    setTimeout(function () {
      top?.indexManager && top.indexManager.initFeedbackFunction(36);
    }, 1000);
  }, []);

  return (
    <div className="flex h-full flex-col">
      <TitleComponent type="main" text={tTransactionHistory('transactionHistory')} />
      <HistoryTab activeTab={activeTab} onTabChange={handleTabChange} />
      <TransactionList transactionType={activeTab} isCashServiceEnabled={isCashServiceEnabled} />
    </div>
  );
};

export default History;

if (document.getElementById('root')) {
  createRoot(document.getElementById('root')!).render(
    <React.StrictMode>
      <History />
    </React.StrictMode>
  );
}
