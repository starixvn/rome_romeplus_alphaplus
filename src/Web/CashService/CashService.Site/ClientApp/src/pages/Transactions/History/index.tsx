import React, { useState } from 'react';
import { createRoot } from 'react-dom/client';
import TransactionList from './components/TransactionList';
import HistoryTab from './components/HistoryTab';
import './HistoryStyle.css';

const History: React.FC = () => {
  const [activeTab, setActiveTab] = useState<'deposit' | 'withdrawal'>('deposit');

  const handleTabChange = (tab: 'deposit' | 'withdrawal') => {
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
