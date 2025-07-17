import React from 'react';
import { useTranslation } from 'react-i18next';
import CustomDropdown from '../../../../../components/CustomDropdownComponent';
import { LabelTitleComponent } from '../../../../../components/Title';

interface PromotionTypeFilterProps {
  promotionTypeId: number;
  onPromotionTypeChange: (value: number) => void;
}

const PromotionTypeFilter: React.FC<PromotionTypeFilterProps> = ({
  promotionTypeId,
  onPromotionTypeChange,
}) => {
  const { t } = useTranslation('promotion');

  const promotionTypeOptions = [
    { value: -1, label: t('filter.all') },
    { value: 1, label: t('promotionType.welcome') },
    { value: 2, label: t('promotionType.reload') },
    { value: 3, label: t('promotionType.cashback') },
    { value: 4, label: t('promotionType.freebet') },
  ];

  return (
    <div className="form-group">
      <LabelTitleComponent text={t('filter.promotionType')} />
      <CustomDropdown
        options={promotionTypeOptions}
        onSelect={(option) => onPromotionTypeChange(Number(option.value))}
        initialValue={promotionTypeId}
        showOptionAll={false}
      />
    </div>
  );
};

export default PromotionTypeFilter;
