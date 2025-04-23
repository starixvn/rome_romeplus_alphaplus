import React from 'react';
import { useTranslation } from 'react-i18next';

interface HistoryTabProps {
  activeTab: 'deposit' | 'withdrawal';
  onTabChange: (tab: 'deposit' | 'withdrawal') => void;
}

const HistoryTab: React.FC<HistoryTabProps> = ({ activeTab, onTabChange }) => {
  const { t } = useTranslation('transactions');

  const getButtonClass = (tab: 'deposit' | 'withdrawal') => {
    const baseClass =
      'px-6 py-2 text-sm font-medium transition-colors duration-200 focus:outline-none rounded-md';
    if (tab === activeTab) {
      return `${baseClass} bg-blue-600 text-white hover:bg-blue-700`;
    }
    return `${baseClass} text-gray-600 hover:bg-gray-100 hover:text-gray-900`;
  };

  return (
    <div className="mb-6">
      <h1 className="text-xl font-semibold text-gray-900 mb-4">{t('transactionHistory')}</h1>
      <div className="flex space-x-4">
        <button onClick={() => onTabChange('deposit')} className={getButtonClass('deposit')}>
          {t('deposit')}
        </button>
        <button onClick={() => onTabChange('withdrawal')} className={getButtonClass('withdrawal')}>
          {t('withdrawal')}
        </button>
      </div>
    </div>
  );
};

export default HistoryTab;
