import { useTranslation } from 'react-i18next';
import DateRangePickerComponent from '../../../../components/DateRangePickerComponent';
import CustomDropdownComponent, {
  DropdownOption,
} from '../../../../components/CustomDropdownComponent';
import { LabelTitleComponent } from '../../../../components/Title';

export default function DetailsReportFilter({
  minDate,
  maxDate,
  dateRange,
  setDateRange,
  status,
  setStatus,
}: {
  minDate: Date;
  maxDate: Date;
  dateRange: [Date | null, Date | null];
  setDateRange: (range: [Date | null, Date | null]) => void;
  status: string;
  setStatus: (status: string) => void;
}) {
  const { t } = useTranslation('global');
  const statusOptions: DropdownOption[] = [
    { value: '0', label: t('global:all') },
    { value: 1, label: t('global:status.approved') },
    { value: 2, label: t('global:status.pending') },
    { value: 3, label: t('global:status.rejected') },
  ];
  return (
    <div className="flex flex-nowrap gap-[16px] max-[768px]:gap-[12px] max-[768px]:flex-wrap w-full">
      <div className="w-[190px] mb-[12px]">
        <LabelTitleComponent text={t('global:dateRange')} />
        <DateRangePickerComponent
          value={dateRange}
          onChange={setDateRange}
          minDate={minDate}
          maxDate={maxDate}
        />
      </div>
      <div className="w-[160px] mb-[12px]">
        <LabelTitleComponent text={t('global:statusText')} />
        <CustomDropdownComponent
          options={statusOptions}
          onSelect={(opt) => setStatus(opt.value as string)}
          initialValue={status}
        />
      </div>
    </div>
  );
}
