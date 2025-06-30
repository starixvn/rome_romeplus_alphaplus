import DateRangePickerComponent from '../../../../components/DateRangePickerComponent';
import CustomDropdownComponent, {
  DropdownOption,
} from '../../../../components/CustomDropdownComponent';
import { LabelTitleComponent } from '../../../../components/Title';

const promotionTypeOptions: DropdownOption[] = [
  { value: '1', label: 'Discount' },
  { value: '2', label: 'Cashback' },
  { value: '3', label: 'Buy One Get One' },
  { value: '4', label: 'Free Shipping' },
  { value: '5', label: 'Loyalty Points' },
  { value: '6', label: 'Referral Bonus' },
  { value: '7', label: 'Seasonal Offer' },
  { value: '8', label: 'Flash Sale' },
  { value: '9', label: 'Bundle Deal' },
  { value: '10', label: 'Membership Discount' },
  { value: '11', label: 'Gift Card' },
  { value: '12', label: 'Limited Time Offer' },
  { value: '13', label: 'Volume Discount' },
  { value: '14', label: 'Early Bird Special' },
  { value: '15', label: 'Exclusive Access' },
];

const statusOptions: DropdownOption[] = [
  { value: '1', label: 'Approved' },
  { value: '2', label: 'Pending' },
  { value: '3', label: 'Reject' },
];

interface FilterProps {
  dateRange: [Date | null, Date | null];
  setDateRange: (range: [Date | null, Date | null]) => void;
  promotionType: string;
  setPromotionType: (type: string) => void;
  status: string;
  setStatus: (status: string) => void;
}

export default function Filter({
  dateRange,
  setDateRange,
  promotionType,
  setPromotionType,
  status,
  setStatus,
}: FilterProps) {
  const currentYear = new Date().getFullYear();
  return (
    <div className="flex flex-nowrap gap-[16px] max-[768px]:justify-between max-[768px]:gap-[0] max-[768px]:flex-wrap w-full">
      <div className="w-[190px] pt-[4px] max-[768px]:w-[48%] mb-[8px]">
        <LabelTitleComponent text="Date Range" />
        <DateRangePickerComponent
          value={dateRange}
          onChange={setDateRange}
          minYear={currentYear - 3}
          maxRangeDays={92}
        />
      </div>
      <div className="w-[190px] pt-[4px] max-[768px]:w-[48%] mb-[8px]">
        <LabelTitleComponent text="Promotion Type" />
        <CustomDropdownComponent
          options={promotionTypeOptions}
          onSelect={(opt) => setPromotionType(opt.value as string)}
          initialValue={promotionType}
          showOptionAll
          searchable
        />
      </div>
      <div className="w-[190px] pt-[4px] max-[768px]:w-[48%] mb-[8px]">
        <LabelTitleComponent text="Status" />
        <CustomDropdownComponent
          options={statusOptions}
          onSelect={(opt) => setStatus(opt.value as string)}
          initialValue={status}
          showOptionAll
        />
      </div>
    </div>
  );
}
