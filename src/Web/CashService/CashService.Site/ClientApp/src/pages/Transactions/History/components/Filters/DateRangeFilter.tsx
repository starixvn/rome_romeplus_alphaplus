import React from 'react';
import { useTranslation } from 'react-i18next';
import DateRangePickerComponent from '../../../../../components/DateRangePickerComponent';

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
      val[0] ? val[0].toLocaleString('en-US') : '',
      val[1] ? val[1].toLocaleString('en-US') : '',
    ]);
  };
  const currentYear = new Date().getFullYear();
  return (
    <div className="form-group mb-[8px]">
      <label className="block text-[12px] font-semibold text-[#333333] my-[8px]">
        {t('filter.dateRange')}
      </label>
      <DateRangePickerComponent
        value={value}
        onChange={handleChange}
        minYear={currentYear - 3}
        maxRangeDays={92}
      />
    </div>
  );
};

export default DateRangeFilter;
