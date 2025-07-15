import React, { ForwardedRef } from 'react';
import { useTranslation } from 'react-i18next';
import NoRecordFoundComponent from '../../../../components/NoRecordFoundComponent';
import {
  StatusCompletedComponent,
  StatusProcessingComponent,
  StatusDisabledComponent,
} from '../../../../components/StatusComponents';
import { formatNumber } from '../../../../utils/numberFormatting';

interface DetailsReportTableProps {
  data: any[];
}

function DetailsReportStatusRow({ status }: { status: number }) {
  const { t } = useTranslation(['promotion', 'global']);
  if (status === 1) {
    return <StatusCompletedComponent text={t('global:status.approved')} />;
  }

  if (status === 2) {
    return <StatusProcessingComponent text={t('global:status.pending')} />;
  }

  return <StatusDisabledComponent text={t('global:status.rejected')} />;
}

function DetailsReportRow({ idx, data }: { idx: number; data: any }) {
  return (
    <tr>
      <td className="w-[32px] min-w-[32px] sticky left-0 bg-[var(--table-background)] z-10">
        {formatNumber(idx + 1)}
      </td>
      <td className="text-left sticky left-[32px] bg-[var(--table-background)] z-10 shadow-[3px_0_4px_0_rgba(99,84,84,0.3)]">
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
      <td>
        <DetailsReportStatusRow status={data.status} />
      </td>
    </tr>
  );
}

const DetailsReportTable = React.forwardRef<HTMLTableElement, DetailsReportTableProps>(
  (props, ref: ForwardedRef<HTMLTableElement>) => {
    const { t } = useTranslation(['promotion', 'global']);
    const { data } = props;
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
                <div>{t('global:username')}</div>
                <div>{t('global:nickname')}</div>
              </th>
              <th>{t('global:firstAndLastName')}</th>
              <th>{t('promotion:table.promotionTitle')}</th>
              <th>{t('promotion:table.period')}</th>
              <th>{t('promotion:table.bonusAmount')}</th>
              <th>{t('promotion:table.lastUpdated')}</th>
              <th className="w-[140px] min-w-[140px]">{t('global:statusText')}</th>
            </tr>
          </thead>
          <tbody className="font-[500]">
            {data.length === 0 ? (
              <NoRecordFoundComponent colSpan={8} />
            ) : (
              data.map((item, idx) => (
                <DetailsReportRow key={item.username} idx={idx} data={item} />
              ))
            )}
          </tbody>
        </table>
      </div>
    );
  }
);

export default DetailsReportTable;
