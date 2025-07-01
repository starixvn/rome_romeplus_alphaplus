import DateRangePickerComponent from '../../../../components/DateRangePickerComponent';
import CustomDropdownComponent, {
  DropdownOption,
} from '../../../../components/CustomDropdownComponent';
import { LabelTitleComponent } from '../../../../components/Title';

const statusOptions: DropdownOption[] = [
  { value: '1', label: 'Approved' },
  { value: '2', label: 'Pending' },
  { value: '3', label: 'Rejected' },
];

interface FilterProps {
  dateRange: [Date | null, Date | null];
  setDateRange: (range: [Date | null, Date | null]) => void;
  status: string;
  setStatus: (status: string) => void;
}

export default function DetailsReportFilter({
  dateRange,
  setDateRange,
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
