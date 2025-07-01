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
        <span className="h-[24px] min-w-[24px] rounded-[3px] leading-[24px] px-[5px] text-center bg-[#f6c699] text-[#7e3d00] font-bold">{formatNumber(totalBonus)}</span>
      </div>
      <div className="flex items-center font-semibold bg-[#D4E7D9] dark:bg-[#24853e] rounded-[3px] pr-[3px] justify-center py-[3px] mr-[8px]">
        <span className="block px-[12px]">Total Request Approved</span>
        <span className="h-[24px] min-w-[24px] rounded-[3px] leading-[24px] px-[5px] text-center bg-[#65cf90] dark:bg-[#115d2f] font-bold">{formatNumber(totalApproved)}</span>
      </div>
      <div className="flex items-center font-semibold bg-[#FBD8D8] dark:bg-[#af4646] rounded-[3px] pr-[3px] justify-center py-[3px]">
        <span className="block px-[12px]">Total Request Rejected</span>
        <span className="h-[24px] min-w-[24px] rounded-[3px] leading-[24px] px-[5px] text-center bg-[#ff9ca1] dark:bg-[#8b2328] font-bold">{formatNumber(totalRejected)}</span>
      </div>
       
    </div>
     

    </>
  );
}
