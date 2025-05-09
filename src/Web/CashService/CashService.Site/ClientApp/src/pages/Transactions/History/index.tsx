import React, { useState } from 'react';
import { createRoot } from 'react-dom/client';
import TransactionList from './components/TransactionList';
import HistoryTab from './components/HistoryTab';
import './HistoryStyle.css';
import i18n from '../../../i18n/i18n-transactionHistory';
import { useCookieLanguage } from '../../../hooks/useCookieLanguage';

const History: React.FC = () => {
  useCookieLanguage(i18n);
  const [activeTab, setActiveTab] = useState<number>(1);

  const handleTabChange = (tab: number) => {
    setActiveTab(tab);
  };

  return (
    <div className="min-h-screen bg-gray-50">
      <div className="bg-white rounded-lg shadow-sm p-4">
        <HistoryTab activeTab={activeTab} onTabChange={handleTabChange} />
        <div className="transaction-list">
          <TransactionList transactionType={activeTab} />
        </div>
      </div>
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
