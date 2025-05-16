import React from 'react';
import ColumnFilter from './ColumnFilter';
import StatusFilter from './StatusFilter';
import AccountFilter from './AccountFilter';
import DateRangeFilter from './DateRangeFilter';

interface FiltersProps {
  dateRange: [string, string];
  status: number;
  column: string[];
  account: string;
  transactionType: number;
  onDateRangeChange: (dates: [string, string]) => void;
  onStatusChange: (value: number) => void;
  onColumnChange: (value: string[]) => void;
  onAccountChange: (value: string) => void;
  onMenuClose?: () => void;
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
    <div className="flex bg-[#ffffff] px-[14px] py-[8px] rounded-lg">
      <div className="w-[280px] pr-[14px]">
        <DateRangeFilter dateRange={dateRange} onDateRangeChange={onDateRangeChange} />
      </div>
      <div className="w-[230px] pr-[14px]">
        <AccountFilter
          account={account}
          transactionType={transactionType}
          onAccountChange={onAccountChange}
        />
      </div>
      <div className="w-[230px] pr-[14px]">
        <StatusFilter status={status} onStatusChange={onStatusChange} />
      </div>
      <div className="w-[220px]">
        <ColumnFilter column={column} onColumnChange={onColumnChange} />
      </div>
    </div>
  );
};

export default Filters;
