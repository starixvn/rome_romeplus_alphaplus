import React from 'react';
import { useTranslation } from 'react-i18next';

interface StatusFilterProps {
  status: string;
  onStatusChange: (value: string) => void;
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
          onChange={(e) => onStatusChange(e.target.value)}
          className="h-[35px] w-[100%] border-[1px] border-[var(--border-control)] rounded-[4px] px-[8px] text-[12px] focus:outline-none focus:border-[var(--button-border)] cursor-pointer"
        >
          <option value="All">{t('filter.all')}</option>
          <option value="COMPLETED">{t('status.completed')}</option>
          <option value="REJECTED">{t('status.rejected')}</option>
          <option value="PROVIDER_REJECTED">{t('status.providerRejected')}</option>
          <option value="EXPIRED">{t('status.expired')}</option>
        </select>
      </div>
    </div>
  );
};

export default StatusFilter;
