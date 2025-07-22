import { useTranslation } from 'react-i18next';
import DateRangePickerComponent from '../../../../components/DateRangePickerComponent';
import CustomDropdownComponent, {
  DropdownOption,
} from '../../../../components/CustomDropdownComponent';
import { TitleComponent } from '../../../../components/Title';
import {
  PROMOTION_TYPES,
  PROMOTION_STATUS_OPTIONS,
} from '../../../../constants/promotionConstants';

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
  const promotionTypeOptions: DropdownOption[] = [
    { value: '0', label: t('global:all') },
    ...PROMOTION_TYPES.map((type) => ({
      value: type.value.toString(),
      label: t(`promotion:promotionType_${type.value}`),
    })),
  ];
  const statusOptions: DropdownOption[] = Object.keys(PROMOTION_STATUS_OPTIONS).map((key) => ({
    value: PROMOTION_STATUS_OPTIONS[key as keyof typeof PROMOTION_STATUS_OPTIONS],
    label: t(`global:${key}`),
  }));
  return (
    <div className="flex flex-nowrap max-[768px]:flex-wrap w-full">
      <div className="w-[190px] mb-[12px] mr-[12px]">
        <TitleComponent text={t('global:dateRange')} type='label'/>
        <DateRangePickerComponent
          value={dateRange}
          onChange={setDateRange}
          minDate={minDate}
          maxDate={maxDate}
        />
      </div>
      <div className="w-[190px] mb-[12px] mr-[12px]">
        <TitleComponent text={t('promotion:table.promotionType')} type='label'/>
        <CustomDropdownComponent
          options={promotionTypeOptions}
          onSelect={(opt) => setPromotionType(opt.value as string)}
          initialValue={promotionType}
          searchable
        />
      </div>
      <div className="w-[140px] mb-[12px] mr-[12px]">
        <TitleComponent text={t('promotion:table.status')} type='label'/>
        <CustomDropdownComponent
          options={statusOptions}
          onSelect={(opt) => setStatus(opt.value as string)}
          initialValue={status}
        />
      </div>
    </div>
  );
}
