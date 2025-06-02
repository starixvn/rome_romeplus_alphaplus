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
    <div className="flex flex-nowrap gap-[16px] max-[768px]:justify-between max-[768px]:gap-[0] max-[768px]:flex-wrap w-full">
      <div className="w-[280px] pt-[4px] max-[768px]:w-[48%] mb-[8px]">
        <DateRangeFilter dateRange={dateRange} onDateRangeChange={onDateRangeChange} />
      </div>
      <div className="w-[230px] pt-[4px] max-[768px]:w-[48%] mb-[8px]">
        <AccountFilter
          account={account}
          transactionType={transactionType}
          paymentInformations={paymentInformations}
          onAccountChange={onAccountChange}
        />
      </div>
      <div className="w-[230px] pt-[4px] max-[768px]:w-[48%] mb-[8px]">
        <StatusFilter status={status} onStatusChange={onStatusChange} />
      </div>
      <div className="w-[220px] pt-[4px] max-[768px]:w-[48%] mb-[8px]">
        <ColumnFilter column={column} onColumnChange={onColumnChange} />
      </div>
    </div>
  );
};

export default Filters;
