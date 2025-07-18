import React, { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import CustomDropdown, { DropdownOption } from '../../../../../components/CustomDropdownComponent';
import { LabelTitleComponent } from '../../../../../components/Title';
import { promotionService } from '../../../../../services/promotionAPI';

interface PromotionTypeFilterProps {
  promotionTypeId: number;
  onPromotionTypeChange: (value: number) => void;
}

const PromotionTypeFilter: React.FC<PromotionTypeFilterProps> = ({
  promotionTypeId,
  onPromotionTypeChange,
}) => {
  const { t } = useTranslation('promotion');
  const [promotionTypeOptions, setPromotionTypeOptions] = useState<DropdownOption[]>([]);
  useEffect(() => {
    const fetchPromotionTypeOptions = async () => {
      const options = await promotionService.getPromotionTypeOptions();
      setPromotionTypeOptions(options);
    };
    fetchPromotionTypeOptions();
  }, []);

  return (
    <div className="form-group">
      <LabelTitleComponent text={t('filter.promotionType')} />
      <CustomDropdown
        options={promotionTypeOptions}
        onSelect={(option) => onPromotionTypeChange(Number(option.value))}
        initialValue={promotionTypeId}
        showOptionAll={true}
      />
    </div>
  );
};

export default PromotionTypeFilter;
