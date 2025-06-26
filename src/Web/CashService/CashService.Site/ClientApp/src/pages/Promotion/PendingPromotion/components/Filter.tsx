import React from 'react';
import DateRangePickerComponent from '../../../../components/DateRangePickerComponent';
import CustomDropdownComponent, {
  DropdownOption,
} from '../../../../components/CustomDropdownComponent';
import { LabelTitleComponent } from '../../../../components/Title';

const promotionTypeOptions: DropdownOption[] = [
  { value: 'type1', label: 'Type 1' },
  { value: 'type2', label: 'Type 2' },
];

interface FilterProps {
  dateRange: [Date | null, Date | null];
  setDateRange: (range: [Date | null, Date | null]) => void;
  promotionType: string;
  setPromotionType: (type: string) => void;
}

const Filter: React.FC<FilterProps> = ({
  dateRange,
  setDateRange,
  promotionType,
  setPromotionType,
}) => {
  const currentYear = new Date().getFullYear();
  return (
    <div className="flex flex-nowrap gap-[16px] max-[768px]:justify-between max-[768px]:gap-[0] max-[768px]:flex-wrap w-full">
      <div className="w-[230px] pt-[4px] max-[768px]:w-[48%] mb-[8px]">
        <div className="form-group ">
          <LabelTitleComponent text="Date Range" />
          <DateRangePickerComponent
            value={dateRange}
            onChange={setDateRange}
            minYear={currentYear - 3}
            maxRangeDays={92}
          />
        </div>
      </div>
      <div className="w-[230px] pt-[4px] max-[768px]:w-[48%] mb-[8px]">
        <LabelTitleComponent text="Promotion Type" />
        <CustomDropdownComponent
          options={promotionTypeOptions}
          onSelect={(opt) => setPromotionType(opt.value as string)}
          initialValue={promotionType}
          showOptionAll
        />
      </div>
    </div>
  );
};

export default Filter;
