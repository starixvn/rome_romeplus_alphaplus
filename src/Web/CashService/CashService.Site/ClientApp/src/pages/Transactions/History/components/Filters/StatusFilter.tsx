import React from 'react';
import { useTranslation } from 'react-i18next';

interface StatusFilterProps {
  status: number;
  onStatusChange: (value: number) => void;
}

const StatusFilter: React.FC<StatusFilterProps> = ({ status, onStatusChange }) => {
  const { t } = useTranslation('transactionHistory');

  return (
    <div className="form-group mb-[8px]">
      <label className="block text-[12px] font-semibold text-[#333333] my-[8px]">
        {t('filter.status')}
      </label>
      <div className="select-wrapper relative">
        <select
          value={status}
          onChange={(e) => onStatusChange(parseInt(e.target.value))}
          className="h-[32px] max-[768px]:h-[40px] w-[100%] border-[1px] border-[var(--border-control)] rounded-[4px] px-[8px] text-[12px] focus:outline-none focus:border-[var(--button-border)] cursor-pointer"
        >
          <option value="-1">{t('filter.all')}</option>
          <option value="4">{t('status.completed')}</option>
          <option value="5">{t('status.rejected')}</option>
          <option value="6">{t('status.providerRejected')}</option>
          <option value="3">{t('status.expired')}</option>
        </select>
      </div>
    </div>
  );
};

export default StatusFilter;
