import { useTranslation } from 'react-i18next';
import DateRangePickerComponent from '../../../../components/DateRangePickerComponent';
import { TitleComponent } from '../../../../components/Title';

export default function ManualBonusReportFilter({
  minDate,
  maxDate,
  dateRange,
  setDateRange,
}: {
  minDate: Date;
  maxDate: Date;
  dateRange: [Date | null, Date | null];
  setDateRange: (range: [Date | null, Date | null]) => void;
}) {
  const { t } = useTranslation('global');

  return (
    <div className="flex flex-nowrap max-[768px]:flex-wrap w-full">
      <div className="w-[190px] mb-[12px] mr-[12px]">
        <TitleComponent text={t('dateRange')} type="label" />
        <DateRangePickerComponent
          value={dateRange}
          onChange={setDateRange}
          minDate={minDate}
          maxDate={maxDate}
        />
      </div>
    </div>
  );
}
