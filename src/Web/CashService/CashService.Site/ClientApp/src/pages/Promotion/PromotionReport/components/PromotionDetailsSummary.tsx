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
    <div className="">
      <div className="">
        <span className="">Total Bonus Given</span>
        <span className="">{formatNumber(totalBonus)}</span>
      </div>
      <div className="">
        <span className="">Total Request Approved</span>
        <span className="">{formatNumber(totalApproved)}</span>
      </div>
      <div className="">
        <span className="">Total Request Rejected</span>
        <span className="">{formatNumber(totalRejected)}</span>
      </div>
    </div>
  );
}
