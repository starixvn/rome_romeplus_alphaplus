import { formatNumber } from '../../../../utils/numberFormatting';

export interface PromotionDetailsSummaryProps {
  totalBonus: number;
  totalApproved: number;
  totalRejected: number;
}

export function PromotionDetailsSummary({
  totalBonus,
  totalApproved,
  totalRejected,
}: PromotionDetailsSummaryProps) {
  return (
    <>
    <div className="flex mb-[8px]">
      <div className="flex items-center font-semibold border border-[var(--table-border-color)] rounded-[3px] pr-[3px] justify-center py-[3px] mr-[8px]">
        <span className="block px-[12px]">Total Bonus Given</span>
        <span className="h-[24px] min-w-[24px] rounded-[3px] leading-[24px] px-[5px] text-center  text-[120%]">{formatNumber(totalBonus)}</span>
      </div>
      <div className="flex items-center font-semibold bg-[#E6F9F0] dark:bg-[#24853e] rounded-[3px] pr-[3px] justify-center py-[3px] mr-[8px]">
        <span className="block px-[12px]">Total Request Approved</span>
        <span className="h-[24px] min-w-[24px] rounded-[3px] leading-[24px] px-[5px] text-center text-[#3aa573] text-[120%] dark:text-[#fff]">{formatNumber(totalApproved)}</span>
      </div>
      <div className="flex items-center font-semibold bg-[#FBD8D8] dark:bg-[#af4646] rounded-[3px] pr-[3px] justify-center py-[3px]">
        <span className="block px-[12px]">Total Request Rejected</span>
        <span className="h-[24px] min-w-[24px] rounded-[3px] leading-[24px] px-[5px] text-center text-[#9d1c1c] text-[120%] dark:text-[#fff]">{formatNumber(totalRejected)}</span>
      </div>
    </div>
     

    </>
  );
}
