import React from 'react';
import { IconInfo } from '../../../../../components/Icons';
import { MediumTitleComponent, LabelTitleComponent } from '../../../../../components/Title';
import CustomDropdown from '../../../../../components/CustomDropdownComponent';
import DateRangePickerComponent from '../../../../../components/DateRangePickerComponent';
import TreeSelect, { TreeNodeOption } from '../../../../../components/TreeSelect/TreeSelect';
import { EnableButtonComponent, DisableButtonComponent } from '../../../../../components/Buttons';
import { Product } from '../../../../../services/interfaces/Promotion';

interface InformationProps {
  isActive: boolean;
  setIsActive: (v: boolean) => void;
  promotionType: number;
  setPromotionType: (v: number) => void;
  period: [Date | null, Date | null];
  setPeriod: (v: [Date | null, Date | null]) => void;
  productOptions: Product[];
  selectedProductIds: number[];
  handleProductChange: (values: (string | number)[], highestParents: TreeNodeOption[]) => void;
  PROMOTION_TYPES: { value: number; label: string }[];
  convertProductToTreeNodeOption: (products: Product[]) => TreeNodeOption[];
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
  convertProductToTreeNodeOption,
}) => (
  <>
    <MediumTitleComponent text="Current Promotion" />
    <p className="my-[8px] text-[12px] font-semibold">
      Do you want to keep your promotion active? You can only run one promotion at a time.
    </p>
    <div
      className="mb-[16px] inline-block border-[1px] border-[var(--border-control)] rounded-[5px]"
      role="group"
    >
      <EnableButtonComponent isActive={isActive} onClick={() => setIsActive(true)} />
      <DisableButtonComponent isActive={!isActive} onClick={() => setIsActive(false)} />
    </div>

    <MediumTitleComponent text="Promotion Settings" />
    <div className="flex items-center mb-[8px]">
      <p className="text-[12px] text-[#333333] font-semibold">Promotion runs on a weekly basis.</p>
      <span className="ml-[4px]" title="Promotion runs on a weekly basis.">
        <IconInfo className="w-[20px] cursor-pointer h-[20px] fill-[#163f78]" />
      </span>
    </div>

    <div className="flex flex-wrap w-full">
      <div className="w-[190px] mr-[12px] mb-[12px]">
        <LabelTitleComponent text="Promotion Type" required={true} />
        <CustomDropdown
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
        />
      </div>

      <div className="w-[190px] mr-[12px] mb-[12px]">
        <LabelTitleComponent text="Provider" required={true} />
        <TreeSelect
          options={convertProductToTreeNodeOption(productOptions)}
          value={selectedProductIds}
          onChange={handleProductChange}
        />
      </div>
    </div>
  </>
);

export default Information;
