import React from 'react';
import PromotionTypeFilter from './PromotionTypeFilter';
import DateRangeFilter from './DateRangeFilter';

interface FiltersProps {
  dateRange: [string, string];
  promotionTypeId: number;
  onDateRangeChange: (dates: [string, string]) => void;
  onPromotionTypeChange: (value: number) => void;
  onMenuClose?: () => void;
}

const Filters: React.FC<FiltersProps> = ({
  dateRange,
  promotionTypeId,
  onDateRangeChange,
  onPromotionTypeChange,
}) => {
  return (
    <div className="flex flex-wrap w-full">
      <div className="w-[190px] mb-[12px] mr-[12px]">
        <DateRangeFilter dateRange={dateRange} onDateRangeChange={onDateRangeChange} />
      </div>
      <div className="w-[190px] mb-[12px] mr-[12px]">
        <PromotionTypeFilter
          promotionTypeId={promotionTypeId}
          onPromotionTypeChange={onPromotionTypeChange}
        />
      </div>
    </div>
  );
};

export default Filters;
