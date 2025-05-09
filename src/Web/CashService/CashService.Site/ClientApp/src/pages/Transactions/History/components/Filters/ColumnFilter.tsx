import React from 'react';
import MultiSelect from '../../../../../components/ui/MultiSelect';
import { useTranslation } from 'react-i18next';


interface ColumnFilterProps {
  column: string[];
  onColumnChange: (value: string[]) => void;
}

type MultiselectOption = { value: string; label: string };

const ColumnFilter: React.FC<ColumnFilterProps> = ({ column, onColumnChange }) => {
  const { t } = useTranslation('transactionHistory');
  const columns = [
    t('table.name'),
    t('table.amount'),
    t('table.currency'),
    t('table.from'),
    t('table.to'),
    t('table.status'),
    t('table.type'),
    t('table.referenceNo'),
    t('table.dateAndTime'),
  ];
  const options: MultiselectOption[] = columns.map((col) => ({
    value: col,
    label: col,
  }));

  return (
    <div className="w-[220px]">
      <label className="block text-sm font-medium text-gray-700 mb-1">{t('filter.column')}</label>
      <MultiSelect
        options={options}
        initialValues={column}
        onChange={onColumnChange}
        placeholder={t('filter.allColumns') as string}
        isCheckAll={true}
      />
    </div>
  );
};

export default ColumnFilter;
