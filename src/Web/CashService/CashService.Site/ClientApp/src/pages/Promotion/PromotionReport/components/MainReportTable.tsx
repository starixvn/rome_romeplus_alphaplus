import React, { ForwardedRef } from 'react';
import NoRecordFoundComponent from '../../../../components/NoRecordFoundComponent';
import {
  StatusEnabledComponent,
  StatusDisabledComponent,
} from '../../../../components/StatusComponents';
import { formatNumber } from '../../../../utils/numberFormatting';

interface MainReportTableProps {
  data: any[];
  onClickPromotionTitle: (promotion: any) => void;
}

function MainReportStatusRow({ status }: { status: number }) {
  if (status === 1) {
    return <StatusEnabledComponent />;
  }

  return <StatusDisabledComponent />;
}

function MainReportRow({
  idx,
  data,
  onClickPromotionTitle,
}: {
  idx: number;
  data: any;
  onClickPromotionTitle: (promotion: any) => void;
}) {
  const handleDrillDown = (
    event: React.MouseEvent<HTMLAnchorElement>,
    promotionId: number,
    promotionTitle: string
  ) => {
    event.preventDefault();
    onClickPromotionTitle({ promotionId, promotionTitle });
  };

  return (
    <tr>
      <td className="w-[32px] min-w-[32px] sticky left-0 bg-[var(--table-background)] z-10">
        {formatNumber(idx + 1)}
      </td>
      <td className="text-left sticky left-[32px] bg-[var(--table-background)] z-10 shadow-[3px_0_4px_0_rgba(99,84,84,0.3)]">
        <a
          href="#"
          onClick={(event) => handleDrillDown(event, data.promotionId, data.promotionTitle)}
          className="text-[#045ace] dark:text-[#41a4ff] hover:underline"
        >
          {data.promotionTitle}
        </a>
      </td>
      <td>{data.promotionType}</td>
      <td>{data.dateRange}</td>
      <td className="text-right">{formatNumber(data.totalRequest)}</td>
      <td className="text-right">
        <div>{formatNumber(data.totalApproved)}</div>
        <div className="text-[#CC0000] dark:text-[#F23D3D]">{formatNumber(data.totalRejected)}</div>
      </td>
      <td className="text-right">{formatNumber(data.totalBonus)}</td>
      <td>
        <MainReportStatusRow status={data.status} />
      </td>
    </tr>
  );
}

const MainReportTable = React.forwardRef<HTMLTableElement, MainReportTableProps>(
  (props, ref: ForwardedRef<HTMLTableElement>) => {
    const { data, onClickPromotionTitle } = props;
    return (
      <div className="table-fixed-header table-fixed-body min-h-0">
        <table
          ref={ref}
          className="w-full whitespace-nowrap border-collapse border border-[#0000001f] border-t-[0px] border-l-[0px]"
        >
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
                <MainReportRow
                  key={item.promotionId}
                  idx={idx}
                  data={item}
                  onClickPromotionTitle={onClickPromotionTitle}
                />
              ))
            )}
          </tbody>
        </table>
      </div>
    );
  }
);

export default MainReportTable;
