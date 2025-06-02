import React from 'react';
import { useTranslation } from 'react-i18next';
import CustomDropdown from '../../../../../components/CustomDropdownComponent';

interface StatusFilterProps {
  status: number;
  onStatusChange: (value: number) => void;
}

const StatusFilter: React.FC<StatusFilterProps> = ({ status, onStatusChange }) => {
  const { t } = useTranslation('transactionHistory');

  const statusOptions = [
    { value: -1, label: t('filter.all') },
    { value: 4, label: t('status.completed') },
    { value: 5, label: t('status.rejected') },
    { value: 6, label: t('status.providerRejected') },
    { value: 3, label: t('status.expired') },
  ];

  return (
    <div className="form-group mb-[8px]">
      <label className="block text-[12px] font-semibold text-[#333333] my-[8px]">
        {t('filter.status')}
      </label>
      <CustomDropdown
        options={statusOptions}
        onSelect={(option) => onStatusChange(Number(option.value))}
        initialValue={status}
        showOptionAll={false}
      />
    </div>
  );
};

export default StatusFilter;
