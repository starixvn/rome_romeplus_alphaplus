import React, { useState } from 'react';
import { useTranslation } from 'react-i18next';
import CustomMultiSelect from '../../../../../components/ui/Multiselect/MultiSelect';
import { LabelTitleComponent } from '../../../../../components/TitleComponents';

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
    { name: 'name', label: t('table.name') },
    { name: 'amount', label: t('table.amount') },
    { name: 'from', label: t('table.from') },
    { name: 'to', label: t('table.to') },
    { name: 'status', label: t('table.status') },
    { name: 'type', label: t('table.type') },
    { name: 'referenceNo', label: t('table.referenceNo') },
    { name: 'dateAndTime', label: t('table.dateAndTime') },
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
