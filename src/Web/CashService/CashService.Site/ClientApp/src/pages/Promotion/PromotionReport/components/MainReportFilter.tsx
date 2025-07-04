import DateRangePickerComponent from '../../../../components/DateRangePickerComponent';
import CustomDropdownComponent, {
  DropdownOption,
} from '../../../../components/CustomDropdownComponent';
import { LabelTitleComponent } from '../../../../components/Title';

const promotionTypeOptions: DropdownOption[] = [
  { value: 1, label: 'Rebate on Turnover' },
  { value: 2, label: 'Rebate on Deposit' },
  { value: 3, label: 'Rebate on Loss' },
  { value: 4, label: 'Rebate on Profit' },
  { value: 5, label: 'Rebate on Net Revenue' },
  { value: 6, label: 'Rebate on Net Gaming Revenue' },
  { value: 7, label: 'Rebate on Net Profit' },
  { value: 8, label: 'Bonus' },
  { value: 9, label: 'Freebet' },
];

const statusOptions: DropdownOption[] = [
  { value: 1, label: 'Enabled' },
  { value: 2, label: 'Disabled' },
];

export default function MainReportFilter({
  dateRange,
  setDateRange,
  promotionType,
  setPromotionType,
  status,
  setStatus,
}: {
  dateRange: [Date | null, Date | null];
  setDateRange: (range: [Date | null, Date | null]) => void;
  promotionType: string;
  setPromotionType: (type: string) => void;
  status: string;
  setStatus: (status: string) => void;
}) {
  const currentYear = new Date().getFullYear();
  return (
    <div className="flex flex-nowrap max-[768px]:flex-wrap w-full">
      <div className="w-[190px] mb-[12px] mr-[12px]">
        <LabelTitleComponent text="Date Range" />
        <DateRangePickerComponent
          value={dateRange}
          onChange={setDateRange}
          minYear={currentYear - 3}
          maxRangeDays={92}
        />
      </div>
      <div className="w-[190px] mb-[12px] mr-[12px]">
        <LabelTitleComponent text="Promotion Type" />
        <CustomDropdownComponent
          options={promotionTypeOptions}
          onSelect={(opt) => setPromotionType(opt.value as string)}
          initialValue={promotionType}
          showOptionAll
          searchable
        />
      </div>
      <div className="w-[190px] mb-[12px] mr-[12px]">
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
