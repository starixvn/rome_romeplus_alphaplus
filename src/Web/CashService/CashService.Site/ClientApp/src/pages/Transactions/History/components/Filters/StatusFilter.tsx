import React from 'react';
import { ChevronDownIcon } from '../../../../../components/Icons';
import { useTranslation } from 'react-i18next';
import '../../../../../i18n/i18n';

interface StatusFilterProps {
  status: string;
  onStatusChange: (value: string) => void;
}

const StatusFilter: React.FC<StatusFilterProps> = ({ status, onStatusChange }) => {
  const { t } = useTranslation('transactionHistory');

  return (
    <div className="w-[180px]">
      <label className="block text-sm font-medium text-gray-700 mb-1">{t('filter.status')}</label>
      <div className="select-wrapper relative">
        <select
          value={status}
          onChange={(e) => onStatusChange(e.target.value)}
          className="appearance-none block w-full bg-white border border-gray-300 px-3 py-1.5 pr-8 rounded text-gray-900 focus:outline-none focus:ring-1 focus:ring-blue-500 focus:border-blue-500 sm:text-sm h-9"
        >
          <option value="All">{t('filter.all')}</option>
          <option value="COMPLETED">{t('status.completed')}</option>
          <option value="REJECTED">{t('status.rejected')}</option>
          <option value="PROVIDER_REJECTED">{t('status.providerRejected')}</option>
          <option value="EXPIRED">{t('status.expired')}</option>
        </select>
        <div className="pointer-events-none absolute inset-y-0 right-0 flex items-center px-2 text-gray-700">
          <ChevronDownIcon className="h-4 w-4" />
        </div>
      </div>
    </div>
  );
};

export default StatusFilter;
