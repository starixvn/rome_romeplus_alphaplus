import React, { useState } from 'react';
import { useTranslation } from 'react-i18next';
import CustomMultiSelect from '../../../../../components/ui/Multiselect/MultiSelect';

interface ColumnFilterProps {
  column: string[];
  onColumnChange: (value: string[]) => void;
  onMenuClose?: () => void;
}

type MultiselectOption = { value: string; label: string };

const ColumnFilter: React.FC<ColumnFilterProps> = ({ column, onColumnChange }) => {
  const { t } = useTranslation('transactionHistory');
  const [selectedColumns, setSelectedColumns] = useState<string[]>(column);

  const columns = [
    t('table.name'),
    t('table.amount'),
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

  const handleChange = (values: string[]) => {
    setSelectedColumns(values);
    onColumnChange(values);
  };

  const handleMenuClose = () => {
    onColumnChange(selectedColumns);
  };

  return (
    <div className="form-group mb-[8px]">
      <label className="block text-[12px] font-semibold text-[#333333] my-[8px]">
        {t('filter.column')}
      </label>
      <CustomMultiSelect
        options={options}
        initialValues={selectedColumns}
        onChange={handleChange}
        placeholder={t('filter.allColumns') as string}
        isCheckAll={true}
        onMenuClose={handleMenuClose}
      />
    </div>
  );
};

export default ColumnFilter;
