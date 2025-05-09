import React from 'react';
import { useTranslation } from 'react-i18next';


interface HistoryTabProps {
  activeTab: number;
  onTabChange: (tab: number) => void;
}

const HistoryTab: React.FC<HistoryTabProps> = ({ activeTab, onTabChange }) => {
  const { t } = useTranslation('transactionHistory');

  const getButtonClass = (tab: number) => {
    const baseClass =
      'px-6 py-2 text-sm font-medium transition-colors duration-200 focus:outline-none rounded-md cursor-pointer';
    if (tab === activeTab) {
      return `${baseClass} bg-blue-600 text-white hover:bg-blue-700`;
    }
    return `${baseClass} text-gray-600 border border-gray-300 hover:border-gray-400 hover:bg-gray-50`;
  };

  return (
    <div className="mb-6">
      <h1 className="text-xl font-semibold text-gray-900 mb-4">{t('transactionHistory')}</h1>
      <div className="flex space-x-4">
        <button onClick={() => onTabChange(1)} className={getButtonClass(1)}>
          {t('deposit')}
        </button>
        <button onClick={() => onTabChange(2)} className={getButtonClass(2)}>
          {t('withdrawal')}
        </button>
      </div>
    </div>
  );
};

export default HistoryTab;
