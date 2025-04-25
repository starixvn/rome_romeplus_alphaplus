import React from 'react';
import { ChevronDownIcon } from '../../../../../components/Icons';
import { useTranslation } from 'react-i18next';
import '../../../../../i18n/i18n';

interface ColumnFilterProps {
  column: string;
  onColumnChange: (value: string) => void;
}

const ColumnFilter: React.FC<ColumnFilterProps> = ({ column, onColumnChange }) => {
  const { t } = useTranslation('transactionHistory');

  return (
    <div className="w-[180px]">
      <label className="block text-sm font-medium text-gray-700 mb-1">{t('filter.column')}</label>
      <div className="select-wrapper relative">
        <select
          name="column"
          value={column}
          onChange={(e) => onColumnChange(e.target.value)}
          className="appearance-none block w-full bg-white border border-gray-300 px-3 py-1.5 pr-8 rounded text-gray-900 focus:outline-none focus:ring-1 focus:ring-blue-500 focus:border-blue-500 sm:text-sm h-9"
        >
          <option value="All">{t('filter.allColumns')}</option>
          <option value="name">{t('table.name')}</option>
          <option value="amount">{t('table.amount')}</option>
          <option value="currency">{t('table.currency')}</option>
          <option value="from">{t('table.from')}</option>
          <option value="to">{t('table.to')}</option>
          <option value="status">{t('table.status')}</option>
          <option value="type">{t('table.type')}</option>
          <option value="referenceNo">{t('table.referenceNo')}</option>
          <option value="dateAndTime">{t('table.dateAndTime')}</option>
        </select>
        <div className="pointer-events-none absolute inset-y-0 right-0 flex items-center px-2 text-gray-700">
          <ChevronDownIcon className="h-4 w-4" />
        </div>
      </div>
    </div>
  );
};

export default ColumnFilter;
