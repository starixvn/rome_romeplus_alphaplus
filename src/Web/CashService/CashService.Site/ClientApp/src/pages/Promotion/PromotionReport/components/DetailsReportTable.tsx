import NoRecordFoundComponent from '../../../../components/NoRecordFoundComponent';
import {
  StatusCompletedComponent,
  StatusProcessingComponent,
  StatusDisabledComponent,
} from '../../../../components/StatusComponents';
import { formatNumber } from '../../../../utils/numberFormatting';

interface PromotionDetailsProps {
  data: any[];
}

const PromotionDetailsRow = ({ idx, data }: any) => {
  const mapStatus = (status: number) => {
    if (status === 1) {
      return <StatusCompletedComponent text="Approved" />;
    }

    if (status === 2) {
      return <StatusProcessingComponent text="Pending" />;
    }

    return <StatusDisabledComponent text="Rejected" />;
  };

  return (
    <tr>
      <td className="w-[32px] min-w-[32px] sticky left-0 bg-[var(--table-background)] z-10">
        {formatNumber(idx + 1)}
      </td>
      <td className="text-left w-[250px] min-w-[250px] sticky left-[32px] bg-[var(--table-background)] z-10 shadow-[3px_0_4px_0_rgba(99,84,84,0.3)]">
        <div>{data.username}</div>
        {data.nickname && data.nickname.length && <div>{data.nickname}</div>}
      </td>
      <td className="text-left">
        <div>{data.firstName}</div>
        <div>{data.lastName}</div>
      </td>
      <td>{data.promotionType}</td>
      <td>{data.dateRange}</td>
      <td className="text-right">{formatNumber(data.bonusAmount)}</td>
      <td>{data.lastUpdated}</td>
      <td>{mapStatus(data.status)}</td>
    </tr>
  );
};

export default function DetailsReportTable({ data }: PromotionDetailsProps) {
  return (
    <div className="table-fixed-header table-fixed-body min-h-0">
      <table className="w-full whitespace-nowrap border-collapse border border-[#0000001f] border-t-[0px] border-l-[0px]">
        <thead className="z-11!">
          <tr>
            <th className="w-[32px] min-w-[32px] sticky left-0 bg-[var(--table-header-background)] z-11">
              #
            </th>
            <th className="w-[250px] min-w-[250px] sticky left-[32px] bg-[var(--table-header-background)] shadow-[3px_0_4px_0_rgba(99,84,84,0.3)]">
              <div>Username</div>
              <div>Nickname</div>
            </th>
            <th>First & Last Name</th>
            <th>Promotion Type</th>
            <th>Date Range</th>
            <th>Bonus Amount</th>
            <th>Last Updated</th>
            <th className="w-[140px] min-w-[140px]">Status</th>
          </tr>
        </thead>
        <tbody className="font-[500]">
          {data.length === 0 ? (
            <NoRecordFoundComponent colSpan={8} />
          ) : (
            data.map((item, idx) => (
              <PromotionDetailsRow key={item.username} idx={idx} data={item} />
            ))
          )}
        </tbody>
      </table>
    </div>
  );
}
