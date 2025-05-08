import React from 'react';
import ColumnFilter from './ColumnFilter';
import StatusFilter from './StatusFilter';
import AccountFilter from './AccountFilter';
import DateRangeFilter from './DateRangeFilter';

interface FiltersProps {
  dateRange: [string, string];
  status: string;
  column: string[];
  account: string;
  transactionType: number;
  onDateRangeChange: (dates: [string, string]) => void;
  onStatusChange: (value: string) => void;
  onColumnChange: (value: string[]) => void;
  onAccountChange: (value: string) => void;
}

const Filters: React.FC<FiltersProps> = ({
  dateRange,
  status,
  column,
  account,
  transactionType,
  onDateRangeChange,
  onStatusChange,
  onColumnChange,
  onAccountChange,
}) => {
  return (
    <div className="flex items-center space-x-4 bg-white rounded-lg">
      <DateRangeFilter dateRange={dateRange} onDateRangeChange={onDateRangeChange} />
      <AccountFilter
        account={account}
        transactionType={transactionType}
        onAccountChange={onAccountChange}
      />
      <StatusFilter status={status} onStatusChange={onStatusChange} />
      <ColumnFilter column={column} onColumnChange={onColumnChange} />
    </div>
  );
};

export default Filters;
