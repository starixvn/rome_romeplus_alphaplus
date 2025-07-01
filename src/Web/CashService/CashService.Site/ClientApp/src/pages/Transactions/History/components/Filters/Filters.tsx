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
  paymentInformations: string[];
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
  paymentInformations,
  transactionType,
  onDateRangeChange,
  onStatusChange,
  onColumnChange,
  onAccountChange,
}) => {
  return (
    <div className="flex flex-wrap w-full">
      <div className="w-[190px] mb-[12px] mr-[12px]">
        <DateRangeFilter dateRange={dateRange} onDateRangeChange={onDateRangeChange} />
      </div>
      <div className="w-[190px] mb-[12px] mr-[12px]">
        <AccountFilter
          account={account}
          transactionType={transactionType}
          paymentInformations={paymentInformations}
          onAccountChange={onAccountChange}
        />
      </div>
      <div className="w-[190px] mb-[12px] mr-[12px]">
        <StatusFilter status={status} onStatusChange={onStatusChange} />
      </div>
      <div className="w-[190px] mb-[12px] mr-[12px]">
        <ColumnFilter column={column} onColumnChange={onColumnChange} />
      </div>
    </div>
  );
};

export default Filters;
