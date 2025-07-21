import React from 'react';
import { DisableButtonComponent, EnableButtonComponent } from '../../../../../components/Buttons';
import CustomDropdown from '../../../../../components/CustomDropdownComponent';
import DateRangePickerComponent from '../../../../../components/DateRangePickerComponent';
import { TitleComponent } from '../../../../../components/Title';
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
    <TitleComponent text="Current Promotion" type='medium'/>
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
    <TitleComponent text={"Promotion Settings"} showTippy={true} classNameTippy={"max-w-[640px]!"} type='main' >
      <div>During the promotion period, bonus amounts will be calculated every Tuesday based on qualified net turnover from the previous week (Monday to Sunday).</div>
      <div>The bonus is a rebate on qualified net turnover, calculated from WON and LOST bet tickets on selected products.</div>
      <div><b>Qualified Net Turnover:</b></div>
      <div>•  Sportsbook & Virtual Sports: Excludes Draw bets and Cash Out.</div>
      <div>•  RNG Casino: Slot games only.</div>
      <div>•  Live Casino: Includes Draw bets.</div>
      <div><b>Note:</b></div>
      <div>•  For Live Casino and RNG Casino, if a member bets on both sides of a game, the qualified net turnover is calculated as: Total payout – Total stake</div>
      <div>•  Example: A member bets 100 on both Player and Banker in a Baccarat game. Banker wins with odds = 1.</div>
      <div>•  Payout = 200, Stake = 200 → Net turnover = 0 (200 - 200)</div>
    </TitleComponent>     
    <div className="flex flex-wrap w-full">
      <div className="w-[190px] mr-[12px] mb-[12px]">
        <div className="flex items-center">
          <TitleComponent text="Promotion Type" className="mr-[5px]" required={true} type='label'/>
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
        <TitleComponent text="Period" required={true} type='label'/>
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
          <TitleComponent text="Product" className="mr-[5px]" required={true} type='label'/>
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

export default Information;
