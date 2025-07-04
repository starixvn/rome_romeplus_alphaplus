import DateRangePickerComponent from '../../../../components/DateRangePickerComponent';
import CustomDropdownComponent, {
  DropdownOption,
} from '../../../../components/CustomDropdownComponent';
import { LabelTitleComponent } from '../../../../components/Title';

const statusOptions: DropdownOption[] = [
  { value: 1, label: 'Approved' },
  { value: 2, label: 'Pending' },
  { value: 3, label: 'Rejected' },
];

export default function DetailsReportFilter({
  dateRange,
  setDateRange,
  status,
  setStatus,
}: {
  dateRange: [Date | null, Date | null];
  setDateRange: (range: [Date | null, Date | null]) => void;
  status: string;
  setStatus: (status: string) => void;
}) {
  const currentYear = new Date().getFullYear();
  return (
    <div className="flex flex-nowrap gap-[16px] max-[768px]:gap-[12px] max-[768px]:flex-wrap w-full">
      <div className="w-[190px] mb-[12px]">
        <LabelTitleComponent text="Date Range" />
        <DateRangePickerComponent
          value={dateRange}
          onChange={setDateRange}
          minYear={currentYear - 3}
          maxRangeDays={92}
        />
      </div>
      <div className="w-[160px] mb-[12px]">
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
