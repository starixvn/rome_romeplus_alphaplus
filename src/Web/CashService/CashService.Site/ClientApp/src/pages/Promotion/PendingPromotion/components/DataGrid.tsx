import React from 'react';
import { useTranslation } from 'react-i18next';

import LoadingComponent from '../../../../components/LoadingComponent';
import NoRecordFound from '../../../../components/NoRecordFoundComponent';
import { PromotionTransaction } from '../../../../services/interfaces/Promotion';
import { displayAmount, formatDateTime } from '../../../../utils/commonFunctions';
import { ButtonFullComponent } from '../../../../components/Buttons';

interface DataGridProps {
  items: PromotionTransaction[];
  pageIndex: number;
  pageSize: number;
  isLoading: boolean;
}

const DataGrid: React.FC<DataGridProps> = ({ items, pageIndex, pageSize, isLoading }) => {
  const { t } = useTranslation('promotion');

  if (isLoading) {
    return (
      <div className="flex justify-center items-center h-64">
        <LoadingComponent />
      </div>
    );
  }

  return (
    <div className="table-fixed-header table-fixed-body min-h-0">
      <table className="w-full whitespace-nowrap border-collapse border border-[#0000001f] border-t-[0px]  border-l-[0px]">
        <thead className="z-11!">
          <tr>
            <th
              scope="col"
              className="w-[32px] min-w-[32px] bg-[var(--table-header-background)] sticky left-0 z-11"
            >
              {t('table.no')}
            </th>
            <th
              scope="col"
              className="w-[150px] min-w-[150px] sticky left-[32px] bg-[var(--table-header-background)] z-11 shadow-[3px_0_4px_0_rgba(99,84,84,0.3)]"
            >
              <div>{t('table.username')}</div>
              <div>{t('table.nickname')}</div>
            </th>
            <th scope="col">{t('table.name')}</th>
            <th scope="col">{t('table.title')}</th>
            <th scope="col">{t('table.promotionType')}</th>
            <th scope="col" className="min-w-[150px]">
              {t('table.amount')}
            </th>
            <th scope="col">{t('table.status')}</th>
            <th scope="col">{t('table.dateRange')}</th>
            <th scope="col">{t('table.actions')}</th>
          </tr>
        </thead>
        <tbody className="font-[500]">
          {items && items.length > 0 ? (
            items.map((item, index) => (
              <tr key={item.promotionId}>
                <td className="w-[32px] min-w-[32px] sticky left-0 bg-[var(--table-background)] z-10">
                  {index + 1 + (pageIndex - 1) * pageSize}
                </td>
                <td className="w-[150px] min-w-[150px] sticky left-[32px] bg-[var(--table-background)] z-10 shadow-[3px_0_4px_0_rgba(99,84,84,0.3)]">
                  <div>{item.username}</div>
                  <div>{item.nickname}</div>
                </td>
                <td>
                  <div>{item.firstName}</div>
                  <div>{item.lastName}</div>
                </td>
                <td className="text-center">{item.title}</td>
                <td className="text-center">{item.promotionTypeName}</td>
                <td className="text-right">
                  <div className="flex items-center justify-end pr-[25px] relative">
                    {displayAmount(item.amount)}
                  </div>
                </td>
                <td className="text-center">{item.status}</td>
                <td className="text-center">
                  <div>{formatDateTime(item.fromDate)}</div>
                  <div>{formatDateTime(item.toDate)}</div>
                </td>
                <td className="text-center">
                  <ButtonFullComponent
                    text={t('table.edit')}
                    onClick={() => {
                      console.log('Edit promotion:', item.promotionId);
                    }}
                  />
                </td>
              </tr>
            ))
          ) : (
            <NoRecordFound colSpan={9} />
          )}
        </tbody>
      </table>
    </div>
  );
};

export default DataGrid;
