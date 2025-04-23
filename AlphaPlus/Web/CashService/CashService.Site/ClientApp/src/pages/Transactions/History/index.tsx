import React, { useState } from 'react';
import { createRoot } from 'react-dom/client';
import { useTranslation } from 'react-i18next';
import TransactionList from './components/TransactionList';
import HistoryTab from './components/HistoryTab';
import './HistoryStyle.css';

const History: React.FC = () => {
  const { t } = useTranslation('transactions');
  const [activeTab, setActiveTab] = useState<'deposit' | 'withdrawal'>('deposit');

  const handleTabChange = (tab: 'deposit' | 'withdrawal') => {
    setActiveTab(tab);
  };

  return (
    <div className="transaction-history p-6">
      <HistoryTab activeTab={activeTab} onTabChange={handleTabChange} />
      <div className="transaction-list">
        <TransactionList transactionType={activeTab} />
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
