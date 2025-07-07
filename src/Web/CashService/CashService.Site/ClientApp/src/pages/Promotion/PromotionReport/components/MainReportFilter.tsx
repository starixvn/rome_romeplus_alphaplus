import { useTranslation } from 'react-i18next';
import DateRangePickerComponent from '../../../../components/DateRangePickerComponent';
import CustomDropdownComponent, {
  DropdownOption,
} from '../../../../components/CustomDropdownComponent';
import { LabelTitleComponent } from '../../../../components/Title';

const promotionTypeOptions: DropdownOption[] = [
  { value: '0', label: 'All' },
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

export default function MainReportFilter({
  minDate,
  maxDate,
  dateRange,
  setDateRange,
  promotionType,
  setPromotionType,
  status,
  setStatus,
}: {
  minDate: Date;
  maxDate: Date;
  dateRange: [Date | null, Date | null];
  setDateRange: (range: [Date | null, Date | null]) => void;
  promotionType: string;
  setPromotionType: (type: string) => void;
  status: string;
  setStatus: (status: string) => void;
}) {
  const { t } = useTranslation(['promotion', 'global']);
  const statusOptions: DropdownOption[] = [
    { value: '2', label: t('global:all') },
    { value: 1, label: t('global:enabled') },
    { value: 0, label: t('global:disabled') },
  ];
  return (
    <div className="flex flex-nowrap max-[768px]:flex-wrap w-full">
      <div className="w-[190px] mb-[12px] mr-[12px]">
        <LabelTitleComponent text="Date Range" />
        <DateRangePickerComponent
          value={dateRange}
          onChange={setDateRange}
          minDate={minDate}
          maxDate={maxDate}
        />
      </div>
      <div className="w-[190px] mb-[12px] mr-[12px]">
        <LabelTitleComponent text="Promotion Type" />
        <CustomDropdownComponent
          options={promotionTypeOptions}
          onSelect={(opt) => setPromotionType(opt.value as string)}
          initialValue={promotionType}
          searchable
        />
      </div>
      <div className="w-[190px] mb-[12px] mr-[12px]">
        <LabelTitleComponent text={t('status')} />
        <CustomDropdownComponent
          options={statusOptions}
          onSelect={(opt) => setStatus(opt.value as string)}
          initialValue={status}
        />
      </div>
    </div>
  );
}
