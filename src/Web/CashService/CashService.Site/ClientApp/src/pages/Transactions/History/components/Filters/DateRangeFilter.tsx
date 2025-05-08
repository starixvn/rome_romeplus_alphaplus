import React from 'react';
import { useTranslation } from 'react-i18next';
import DateRangePicker from '../../../../../components/ui/DateRangePicker';

interface DateRangeFilterProps {
  dateRange: [string, string];
  onDateRangeChange: (dates: [string, string]) => void;
}

const DateRangeFilter: React.FC<DateRangeFilterProps> = ({ dateRange, onDateRangeChange }) => {
  const { t } = useTranslation('transactionHistory');
  const value: [Date | null, Date | null] = [
    dateRange[0] ? new Date(dateRange[0]) : null,
    dateRange[1] ? new Date(dateRange[1]) : null,
  ];

  const handleChange = (val: [Date | null, Date | null]) => {
    onDateRangeChange([
      val[0] ? val[0].toLocaleDateString('en-US') : '',
      val[1] ? val[1].toLocaleDateString('en-US') : '',
    ]);
  };

  return (
    <div className="w-[280px]">
      <label className="block text-sm font-medium text-gray-700 mb-1">
        {t('filter.dateRange')}
      </label>
      <DateRangePicker value={value} onChange={handleChange} />
    </div>
  );
};

export default DateRangeFilter;
