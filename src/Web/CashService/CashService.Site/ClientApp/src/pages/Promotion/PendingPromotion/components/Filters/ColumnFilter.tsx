import React, { useState } from 'react';
import { useTranslation } from 'react-i18next';
import CustomMultiSelect from '../../../../../components/Multiselect/MultiSelect';
import { LabelTitleComponent } from '../../../../../components/Title';

interface ColumnFilterProps {
  column: string[];
  onColumnChange: (value: string[]) => void;
  onMenuClose?: () => void;
}

type MultiselectOption = { value: string; label: string };

const ColumnFilter: React.FC<ColumnFilterProps> = ({ column, onColumnChange }) => {
  const { t } = useTranslation('promotion');
  const [selectedColumns, setSelectedColumns] = useState<string[]>(column);

  const columns = [
    { name: 'username', label: t('table.username') },
    { name: 'name', label: t('table.name') },
    { name: 'title', label: t('table.title') },
    { name: 'promotionType', label: t('table.promotionType') },
    { name: 'amount', label: t('table.amount') },
    { name: 'status', label: t('table.status') },
    { name: 'dateRange', label: t('table.dateRange') },
  ];
  const options: MultiselectOption[] = columns.map((col) => ({
    value: col.name,
    label: col.label,
  }));

  const handleChange = (values: string[]) => {
    setSelectedColumns(values);
    onColumnChange(values);
  };

  const handleMenuClose = () => {
    onColumnChange(selectedColumns);
  };

  return (
    <div className="form-group">
      <LabelTitleComponent text={t('filter.column')} />
      <CustomMultiSelect
        className="react-select-custom control-field p-0!"
        options={options}
        initialValues={selectedColumns}
        onChange={handleChange}
        placeholder={t('filter.allColumns') as string}
        isCheckAll={true}
        onMenuClose={handleMenuClose}
        noneSelectedText={t('filter.noneSelected')}
      />
    </div>
  );
};

export default ColumnFilter;
