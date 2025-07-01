import NoRecordFoundComponent from '../../../../components/NoRecordFoundComponent';
import {
  StatusEnabledComponent,
  StatusDisabledComponent,
} from '../../../../components/StatusComponents';
import { formatNumber } from '../../../../utils/numberFormatting';

interface PromotionListProps {
  data: any[];
  onClickPromotionTitle: (promotionId: number, promotionTitle: string) => void;
}

const PromotionRow = ({ idx, item, onClickPromotionTitle }: any) => {
  const handleDrillDown = (
    event: React.MouseEvent<HTMLAnchorElement>,
    promotionId: number,
    promotionTitle: string
  ) => {
    event.preventDefault();
    onClickPromotionTitle(promotionId, promotionTitle);
  };

  const mapStatus = (status: number) => {
    if (status === 1) {
      return <StatusEnabledComponent />;
    }

    return <StatusDisabledComponent />;
  };

  return (
    <tr>
      <td className="w-[32px] min-w-[32px] sticky left-0 bg-[var(--table-background)] z-10">
        {formatNumber(idx + 1)}
      </td>
      <td className="text-left sticky left-[32px] bg-[var(--table-background)] z-10 shadow-[3px_0_4px_0_rgba(99,84,84,0.3)]">
        <a
          href="#"
          onClick={(event) => handleDrillDown(event, item.promotionId, item.promotionTitle)}
          className="text-[#118AF2] hover:underline"
        >
          {item.promotionTitle}
        </a>
      </td>
      <td>{item.promotionType}</td>
      <td>{item.period}</td>
      <td className="text-right">{formatNumber(item.totalRequest)}</td>
      <td className="text-right">
        <div>{formatNumber(item.totalApproved)}</div>
        <div className="text-red-600 hover:underline">{formatNumber(item.totalRejected)}</div>
      </td>
      <td className="text-right">{formatNumber(item.totalBonus)}</td>
      <td>{mapStatus(item.status)}</td>
    </tr>
  );
};

export default function PromotionTable({ data, onClickPromotionTitle }: PromotionListProps) {
  return (
    <div className="table-fixed-header table-fixed-body min-h-0">
      <table className="w-full whitespace-nowrap border-collapse border border-[#0000001f] border-t-[0px] border-l-[0px]">
        <thead className="z-11!">
          <tr>
            <th className="w-[32px] min-w-[32px] sticky left-0 bg-[var(--table-header-background)] z-11">
              #
            </th>
            <th className="sticky left-[32px] bg-[var(--table-header-background)] shadow-[3px_0_4px_0_rgba(99,84,84,0.3)]">
              Promotion Title
            </th>
            <th>Promotion Type</th>
            <th>Date Range</th>
            <th>Total Request</th>
            <th>
              <div>Total Request Approved</div>
              <div>Total Request Rejected</div>
            </th>
            <th>Total Bonus Given</th>
            <th className="w-[140px] min-w-[140px]">Status</th>
          </tr>
        </thead>
        <tbody className="font-[500]">
          {data.length === 0 ? (
            <NoRecordFoundComponent colSpan={8} />
          ) : (
            data.map((item, idx) => (
              <PromotionRow
                key={item.promotionId}
                idx={idx}
                item={item}
                onClickPromotionTitle={onClickPromotionTitle}
              />
            ))
          )}
        </tbody>
      </table>
    </div>
  );
}
