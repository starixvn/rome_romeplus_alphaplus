import React from 'react';
import { useTranslation } from 'react-i18next';
import { DisableButtonComponent, EnableButtonComponent } from '../../../../../components/Buttons';
import CustomDropdown from '../../../../../components/CustomDropdownComponent';
import DateRangePickerComponent from '../../../../../components/DateRangePickerComponent';
import { TitleComponent } from '../../../../../components/Title';
import TreeSelect, { TreeNodeOption } from '../../../../../components/TreeSelect/TreeSelect';
import Tippy from '@tippyjs/react';
import { IconInfo } from '../../../../../components/Icons';

interface InformationProps {
  isActive: boolean;
  setIsActive: (v: boolean) => void;
  promotionType: number;
  setPromotionType: (v: number) => void;
  period: [Date | null, Date | null];
  setPeriod: (v: [Date | null, Date | null]) => void;
  productOptions: TreeNodeOption[];
  selectedProductIds: number[];
  handleProductChange: (values: (string | number)[], highestParents: TreeNodeOption[]) => void;
  PROMOTION_TYPES: { value: number; label: string }[];
  readOnlyMode: boolean;
}

const Information: React.FC<InformationProps> = ({
  isActive,
  setIsActive,
  promotionType,
  setPromotionType,
  period,
  setPeriod,
  productOptions,
  selectedProductIds,
  handleProductChange,
  PROMOTION_TYPES,
  readOnlyMode
}) => {
  const { t } = useTranslation('promotion');
  return (
  <>
    <TitleComponent text={t('addPromotionForm.currentPromotion')} type='medium'/>   
    <p className="my-[8px]">
        {t('addPromotionForm.onlyOnePromotionCanBeActiveAtATime')}
    </p>
    <div
      className="mb-[16px] inline-block border-[1px] border-[var(--border-control)] rounded-[5px]"
      role="group"
    >
      <EnableButtonComponent isActive={isActive} onClick={() => setIsActive(true)} />
      <DisableButtonComponent isActive={!isActive} onClick={() => setIsActive(false)} />
    </div>
    <div className='flex items-center pb-[5px] border-b border-[var(--border-control)] mb-[10px]'>
      <div className='inline-block mr-[10px]'>
        <TitleComponent text={t('addPromotionForm.promotionSettings.title')}   type='medium' ></TitleComponent>  
      </div>
      <Tippy className='max-w-[640px]!' content={<><div>{t('addPromotionForm.promotionSettings.description1')}</div>
        <div>{t('addPromotionForm.promotionSettings.description2')}</div>
        <div><b>{t('addPromotionForm.promotionSettings.description3')}</b></div>
        <div>•  {t('addPromotionForm.promotionSettings.description4')}</div>
        <div>•  {t('addPromotionForm.promotionSettings.description5')}</div>
        <div>•  {t('addPromotionForm.promotionSettings.description6')}</div>
        <div><b>{t('addPromotionForm.promotionSettings.description7')}</b></div>
        <div>•  {t('addPromotionForm.promotionSettings.description8')}</div>
        <div>•  {t('addPromotionForm.promotionSettings.description9')}</div>
        <div>•  {t('addPromotionForm.promotionSettings.description10')}</div></>} theme="light" arrow={true}>
        <span>
          <IconInfo className="cursor-pointer  " />
        </span>
      </Tippy> 
    </div>
    <p className='mb-[10px]'>{t('addPromotionForm.promotionConfigureCalculated')}</p>
    <div className="flex flex-wrap w-full">
      <div className="w-[190px] mr-[12px] mb-[12px]">
        <div className="flex items-center">
          <TitleComponent text={t('addPromotionForm.promotionType')} className="mr-[5px]" required={true} type='label'/>
        </div>
        <CustomDropdown
          disable={readOnlyMode}
          options={PROMOTION_TYPES}
          onSelect={(option) => setPromotionType(option.value as number)}
          initialValue={promotionType}
          showOptionAll={false}
        />
      </div>

      <div className="w-[190px] mr-[12px] mb-[12px]">
        <TitleComponent text={t('table.period')} required={true} type='label'/>
        <DateRangePickerComponent
          value={period}
          onChange={(value) => setPeriod(value as [Date | null, Date | null])}
          hideQuickRanges={true}
          minDate={new Date()}
          maxDate={new Date(new Date().setFullYear(new Date().getFullYear() + 1))}
        />
      </div>

      <div className="w-[190px] mr-[12px] mb-[12px]">
        <div className="flex items-center">
          <TitleComponent text={t('table.Product')} className="mr-[5px]" required={true} type='label'/>
        </div>
        <TreeSelect
          options={productOptions}
          value={selectedProductIds}
          onChange={handleProductChange}
          isShowSelectAll={false}
        />
      </div>
    </div>
  </>
  );
};

export default Information;
