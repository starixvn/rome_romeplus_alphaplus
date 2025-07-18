import { useTranslation } from 'react-i18next';
import DateRangePickerComponent from '../../../../components/DateRangePickerComponent';
import CustomDropdownComponent, {
  DropdownOption,
} from '../../../../components/CustomDropdownComponent';
import { LabelTitleComponent } from '../../../../components/Title';
import { PROMOTION_BONUS_STATUS_OPTIONS } from '../../../../constants/promotionConstants';

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
  const statusOptions: DropdownOption[] = Object.keys(PROMOTION_BONUS_STATUS_OPTIONS).map((key) => {
    const value =
      PROMOTION_BONUS_STATUS_OPTIONS[key as keyof typeof PROMOTION_BONUS_STATUS_OPTIONS];
    return {
      value: value,
      label: value === 0 ? t(`global:${key}`) : t(`global:status.${key}`),
    };
  });
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
