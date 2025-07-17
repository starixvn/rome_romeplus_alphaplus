import React from 'react';
import { useTranslation } from 'react-i18next';
import NoRecordFound from '../../../../components/NoRecordFoundComponent';
import { PromotionTransaction } from '../../../../services/interfaces/Promotion';

interface TableProps {
  data: PromotionTransaction[];
  loading?: boolean;
}

const Table: React.FC<TableProps> = ({ data, loading = false }) => {
  const { t } = useTranslation('promotion');

  if (loading) {
    return (
      <div className="flex justify-center items-center h-32">
        <div className="text-gray-500">{t('table.loading')}</div>
      </div>
    );
  }

  return (
    <div className="table-fixed-header table-fixed-body min-h-0">
      <table className="w-full whitespace-nowrap border-collapse border border-[#0000001f] border-t-[0px] border-l-[0px]">
        <thead className="z-11!">
          <tr>
            <th className="w-[32px] min-w-[32px] text-center">{t('table.no')}</th>
            <th className="w-[40px] min-w-[40px] text-center">
              <input type="checkbox" />
            </th>
            <th className="w-[40px] min-w-[40px] text-center">{t('table.edit')}</th>
            <th className="w-[150px] min-w-[150px] text-center">{t('table.username')}</th>
            <th className="w-[150px] min-w-[150px] text-center">
              {t('table.lastName')}
              <br />
              {t('table.firstName')}
            </th>
            <th className="w-[200px] min-w-[200px] text-center">{t('table.title')}</th>
            <th className="w-[120px] min-w-[120px] text-center">{t('table.promotionType')}</th>
            <th className="w-[120px] min-w-[120px] text-right">{t('table.bonusAmount')}</th>
            <th className="w-[200px] min-w-[200px] text-center">{t('table.dateRange')}</th>
          </tr>
        </thead>
        <tbody className="font-[500]">
          {data.length === 0 ? (
            <NoRecordFound colSpan={9} />
          ) : (
            data.map((item, idx) => (
              <tr key={item.promotionId || idx}>
                <td className="w-[32px] min-w-[32px] sticky left-0 bg-white z-10">{idx + 1}</td>
                <td className="text-center">
                  <input type="checkbox" />
                </td>
                <td className="text-center">
                  <button className="text-blue-600 underline">{t('table.edit')}</button>
                </td>
                <td className="text-center">{item.username}</td>
                <td className="text-center">
                  <div>{item.lastName}</div>
                  <div>{item.firstName}</div>
                </td>
                <td className="text-center">{item.title}</td>
                <td className="text-center">{item.promotionTypeName}</td>
                <td className="text-right">
                  {item.amount} {item.currency}
                </td>
                <td className="text-center">
                  {new Date(item.fromDate).toLocaleDateString()} -{' '}
                  {new Date(item.toDate).toLocaleDateString()}
                </td>
              </tr>
            ))
          )}
        </tbody>
      </table>
    </div>
  );
};

export default Table;
