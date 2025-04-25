import React from 'react';
import { DatePicker } from 'antd';
import type { RangePickerProps } from 'antd/es/date-picker';
import dayjs from 'dayjs';
import { useTranslation } from 'react-i18next';
import '../../../../../i18n/i18n';

const { RangePicker } = DatePicker;

interface DateRangeFilterProps {
  dateRange: [string, string];
  onDateRangeChange: (dates: [string, string]) => void;
}

const DateRangeFilter: React.FC<DateRangeFilterProps> = ({ dateRange, onDateRangeChange }) => {
  const { t } = useTranslation('transactionHistory');

  const handleDateRangeChange: RangePickerProps['onChange'] = (dates) => {
    if (dates && dates[0] && dates[1]) {
      onDateRangeChange([dates[0].format('MM/DD/YYYY'), dates[1].format('MM/DD/YYYY')]);
    } else {
      onDateRangeChange(['', '']);
    }
  };

  return (
    <div className="w-[280px]">
      <label className="block text-sm font-medium text-gray-700 mb-1">
        {t('filter.dateRange')}
      </label>
      <RangePicker
        value={[
          dateRange[0] ? dayjs(dateRange[0]) : null,
          dateRange[1] ? dayjs(dateRange[1]) : null,
        ]}
        onChange={handleDateRangeChange}
        format="MM/DD/YYYY"
        allowClear={false}
        className="block w-full bg-white border border-gray-300 rounded text-gray-900 focus:outline-none focus:ring-1 focus:ring-blue-500 focus:border-blue-500 sm:text-sm h-9"
      />
    </div>
  );
};

export default DateRangeFilter;
