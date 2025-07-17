import React from 'react';
import { DisableButtonComponent, EnableButtonComponent } from '../../../../../components/Buttons';
import CustomDropdown from '../../../../../components/CustomDropdownComponent';
import DateRangePickerComponent from '../../../../../components/DateRangePickerComponent';
import { LabelTitleComponent, MediumTitleComponent } from '../../../../../components/Title';
import TreeSelect, { TreeNodeOption } from '../../../../../components/TreeSelect/TreeSelect';

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
}) => (
  <>
    <MediumTitleComponent text="Current Promotion"/>
    <p className="my-[8px]">
        Only one promotion can be active at a time.
    </p>
    <div
      className="mb-[16px] inline-block border-[1px] border-[var(--border-control)] rounded-[5px]"
      role="group"
    >
      <EnableButtonComponent isActive={isActive} onClick={() => setIsActive(true)} />
      <DisableButtonComponent isActive={!isActive} onClick={() => setIsActive(false)} />
    </div>

    <MediumTitleComponent text="Promotion Settings" isShowIcon={true} />

    <div className="flex flex-wrap w-full">
      <div className="w-[190px] mr-[12px] mb-[12px]">
        <LabelTitleComponent text="Promotion Type" required={true} isShowIcon={true} />        
        <CustomDropdown
          disable={readOnlyMode}
          options={PROMOTION_TYPES}
          onSelect={(option) => setPromotionType(option.value as number)}
          initialValue={promotionType}
          showOptionAll={false}
        />
      </div>

      <div className="w-[190px] mr-[12px] mb-[12px]">
        <LabelTitleComponent text="Period" required={true} />
        <DateRangePickerComponent
          value={period}
          onChange={(value) => setPeriod(value as [Date | null, Date | null])}
          hideQuickRanges={true}
          minDate={new Date()}
          maxDate={new Date(new Date().setFullYear(new Date().getFullYear() + 1))}
        />
      </div>

      <div className="w-[190px] mr-[12px] mb-[12px]">
        <LabelTitleComponent text="Product" required={true} isShowIcon={true} />
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

export default Information;
