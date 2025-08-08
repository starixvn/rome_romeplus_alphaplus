import React, { ForwardedRef } from 'react';
import { useTranslation } from 'react-i18next';
import Tippy from '@tippyjs/react';
import NoRecordFound from '../../../../components/NoRecordFoundComponent';
import { StatusComponent } from '../../../../components/Status';
import { formatNumber, formatDecimal } from '../../../../utils/numberFormatting';
import { formatDateTime } from '../../../../utils/commonFunctions';
import { PROMOTION_BONUS_STATUS_OPTIONS } from '../../../../constants/promotionConstants';

interface ManualBonusReportTableProps {
  data: any[];
}

function ManualBonusReportStatusRow({ status }: { status: number }) {
  return status === PROMOTION_BONUS_STATUS_OPTIONS.approved ? (
    <StatusComponent type="completed" />
  ) : null;
}

function ManualBonusReportRow({ idx, data }: { idx: number; data: any }) {
  function remarkTooltip(remark: string) {
    const maxLength = 20;
    const length = remark ? remark.length : 0;

    if (length <= maxLength) {
      return <td className="text-left">{remark}</td>;
    }

    return (
      <td className="text-left" data-tooltip={remark}>
        <Tippy content={remark} theme="light" arrow={true} placement="bottom">
          <span className="cursor-pointer">{remark.substring(0, maxLength)} ...</span>
        </Tippy>
      </td>
    );
  }

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
      <td className="text-right">{formatDecimal(data.amount)}</td>
      <td>{data.refNo}</td>
      <td className="text-left">{data.subAccName ? data.subAccName : data.creator}</td>
      <td>{formatDateTime(data.createdTime)}</td>
      <td>
        <ManualBonusReportStatusRow status={data.statusId} />
      </td>
      {remarkTooltip(data.remark)}
    </tr>
  );
}

const ManualBonusReportTable = React.forwardRef<HTMLTableElement, ManualBonusReportTableProps>(
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
              <th>{t('promotion:table.bonusAmount')}</th>
              <th>{t('promotion:table.refNo')}</th>
              <th>{t('global:createdBy')}</th>
              <th>{t('global:createdTime')}</th>
              <th className="w-[140px] min-w-[140px]">{t('global:statusText')}</th>
              <th>{t('promotion:addBonusForm:remarks')}</th>
            </tr>
          </thead>
          <tbody className="font-[500]">
            {data.length === 0 ? (
              <NoRecordFound colSpan={9} />
            ) : (
              data.map((item, idx) => (
                <ManualBonusReportRow key={`${item.refNo}_${idx}`} idx={idx} data={item} />
              ))
            )}
          </tbody>
        </table>
      </div>
    );
  }
);

export default ManualBonusReportTable;
