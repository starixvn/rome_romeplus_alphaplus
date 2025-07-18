import React from 'react';
import { useTranslation } from 'react-i18next';

import LoadingComponent from '../../../../components/LoadingComponent';
import NoRecordFound from '../../../../components/NoRecordFoundComponent';
import { PromotionTransaction } from '../../../../services/interfaces/Promotion';
import { displayAmount, formatDate } from '../../../../utils/commonFunctions';
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
            <th scope="col" className="w-[150px] min-w-[150px] left-[32px]">
              {t('table.username')}
              <br />
              {t('table.nickname')}
            </th>
            <th scope="col">
              {t('table.firstName')}
              <br />
              {t('table.lastName')}
            </th>
            <th scope="col" className="w-[200px]">
              {t('table.title')}
            </th>
            <th scope="col">{t('table.promotionType')}</th>
            <th scope="col" className="min-w-[80px]">
              {t('table.amount')}
            </th>
            <th scope="col">{t('table.dateRange')}</th>
            <th scope="col" className="w-[100px]">
              {t('table.actions')}
            </th>
          </tr>
        </thead>
        <tbody className="font-[500]">
          {items && items.length > 0 ? (
            items.map((item, index) => (
              <tr key={item.promotionId}>
                <td className="w-[32px] min-w-[32px]">{index + 1 + (pageIndex - 1) * pageSize}</td>
                <td className="w-[150px] min-w-[150px]">
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
                  <div className="flex w-full justify-end relative">
                    {displayAmount(item.amount)}
                  </div>
                </td>
                <td className="text-center">
                  <div>
                    {formatDate(item.fromDate)} - {formatDate(item.toDate)}
                  </div>
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
