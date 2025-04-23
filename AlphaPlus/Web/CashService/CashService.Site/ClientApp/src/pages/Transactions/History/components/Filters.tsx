import React from 'react';
import { DatePicker } from 'antd';
import type { RangePickerProps } from 'antd/es/date-picker';
import dayjs from 'dayjs';

const { RangePicker } = DatePicker;

interface FiltersProps {
  dateRange: [string, string];
  status: string;
  column: string;
  onDateRangeChange: (dates: [string, string]) => void;
  onStatusChange: (value: string) => void;
  onColumnChange: (value: string) => void;
}

const Filters: React.FC<FiltersProps> = ({
  dateRange,
  status,
  column,
  onDateRangeChange,
  onStatusChange,
  onColumnChange,
}) => {
  const handleDateRangeChange: RangePickerProps['onChange'] = (dates) => {
    if (dates && dates[0] && dates[1]) {
      onDateRangeChange([dates[0].format('MM/DD/YYYY'), dates[1].format('MM/DD/YYYY')]);
    } else {
      onDateRangeChange(['', '']);
    }
  };

  return (
    <div className="flex items-center space-x-4 mb-0 bg-white p-4 rounded-lg">
      <div className="w-[280px]">
        <label className="block text-sm font-medium text-gray-700 mb-1">Date Range</label>
        <RangePicker
          value={[
            dateRange[0] ? dayjs(dateRange[0]) : null,
            dateRange[1] ? dayjs(dateRange[1]) : null,
          ]}
          onChange={handleDateRangeChange}
          format="MM/DD/YYYY"
          allowClear={false}
          className="block w-full bg-white border border-gray-300 rounded text-gray-900 focus:outline-none focus:ring-blue-500 focus:border-blue-500 sm:text-sm h-9"
        />
      </div>

      <div className="w-[180px]">
        <label className="block text-sm font-medium text-gray-700 mb-1">Status</label>
        <div className="select-wrapper relative">
          <select
            value={status}
            onChange={(e) => onStatusChange(e.target.value)}
            className="appearance-none block w-full bg-white border border-gray-300 px-3 py-1.5 pr-8 rounded text-gray-900 focus:outline-none focus:ring-blue-500 focus:border-blue-500 sm:text-sm h-9"
          >
            <option value="All">All</option>
            <option value="COMPLETED">Completed</option>
            <option value="REJECTED">Rejected</option>
            <option value="PROVIDER_REJECTED">Provider Rejected</option>
            <option value="EXPIRED">Expired</option>
          </select>
          <div className="pointer-events-none absolute inset-y-0 right-0 flex items-center px-2 text-gray-700">
            <svg
              className="h-4 w-4 fill-current"
              xmlns="http://www.w3.org/2000/svg"
              viewBox="0 0 20 20"
            >
              <path d="M9.293 12.95l.707.707L15.657 8l-1.414-1.414L10 10.828 5.757 6.586 4.343 8z" />
            </svg>
          </div>
        </div>
      </div>

      <div className="w-[180px]">
        <label className="block text-sm font-medium text-gray-700 mb-1">Column</label>
        <div className="select-wrapper relative">
          <select
            value={column}
            onChange={(e) => onColumnChange(e.target.value)}
            className="appearance-none block w-full bg-white border border-gray-300 px-3 py-1.5 pr-8 rounded text-gray-900 focus:outline-none focus:ring-blue-500 focus:border-blue-500 sm:text-sm h-9"
          >
            <option value="All Columns (9)">All Columns (9)</option>
            <option value="Username">Username</option>
            <option value="Amount">Amount</option>
            <option value="Status">Status</option>
            <option value="Reference No.">Reference No.</option>
          </select>
          <div className="pointer-events-none absolute inset-y-0 right-0 flex items-center px-2 text-gray-700"></div>
        </div>
      </div>
    </div>
  );
};

export default Filters;
