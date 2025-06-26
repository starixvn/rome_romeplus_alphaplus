import React from 'react';
import { useTranslation } from 'react-i18next';
import i18n from '../../../../i18n/i18n-promotion';
import { useCookieLanguage } from '../../../../hooks/useCookieLanguage';
import { Promotion } from '../../../../services/interfaces/Promotion';
import { IconDelete, IconPen } from '../../../../components/Icons';
import NoRecordFoundComponent from '../../../../components/NoRecordFoundComponent';

interface TableProps {
  items: Promotion[];
  isLoading: boolean;
  onEdit?: (item: Promotion) => void;
  onDelete?: (item: Promotion) => void;
}

const Table: React.FC<TableProps> = ({ items, isLoading, onEdit, onDelete }) => {
  const { t } = useTranslation('promotion');
  const { t: tGlobal } = useTranslation('global');
  useCookieLanguage(i18n);

  const getStatusStyle = (status: string) => {
    var s = status ? status.toLocaleUpperCase() : '';
    switch (s) {
      case 'ACTIVE':
        return 'bg-[#D4E7D9]';
      case 'SCHEDULED':
        return 'bg-[#FBD8D8]';
      case 'EXPIRED':
        return 'bg-[#FBD8D8]';
      default:
        return '';
    }
  };

  const buildHeader = () => (
    <thead>
      <tr>
        <th rowSpan={2} scope="col" className="w-[32px] sticky z-10 left-0 bg-[#ffffff] ">
          {t('table.no')}
        </th>
        <th rowSpan={2} scope="col" className="w-[75px] sticky z-10 left-[32px] bg-[#ffffff]">
          {t('table.action')}
        </th>
        <th
          rowSpan={2}
          className="sticky z-10 w-[110px]  left-[107px] bg-[#ffffff] shadow-[3px_0_4px_0_rgba(99,84,84,0.3)]"
          scope="col"
        >
          <span className="relative z-3">{t('table.promotionTitle')}</span>
        </th>
        <th rowSpan={2} scope="col">
          {t('table.promotionType')}
        </th>
        <th colSpan={4} className="text-center" scope="col">
          Percentage (%)
          <br />
          Max Payout
        </th>
        <th rowSpan={2} scope="col">
          {t('table.period')}
        </th>
        <th rowSpan={2} scope="col">
          Provider
        </th>
        <th rowSpan={2} scope="col">
          Status
        </th>
        <th rowSpan={2} scope="col">
          Last updated
        </th>
      </tr>
      <tr>
        <th className="text-center" scope="col">
          Sports
        </th>
        <th className="text-center" scope="col">
          Virtual Sports
        </th>
        <th className="text-center" scope="col">
          Games
        </th>
        <th className="text-center" scope="col">
          Live Casino
        </th>
      </tr>
    </thead>
  );

  const buildRow = (item: Promotion, index: number) => (
    <tr>
      <td className="w-[32px] sticky left-0 bg-[#ffffff]">{index + 1}</td>
      <td className="w-[75px] sticky left-[32px] bg-[#ffffff]">
        <div className="flex gap-[4px]">
          <button
            className="cursor-pointer p-[4px] rounded-[4px] bg-[#ffffff] hover:bg-[#EAEDF2] text-[var(--text-color-primary)]"
            title={tGlobal('edit')}
            onClick={() => onEdit?.(item)}
          >
            <IconPen />
          </button>
          <button
            className="cursor-pointer p-[4px] rounded-[4px] bg-[#ffffff] hover:bg-[#EAEDF2] text-[var(--text-color-primary)]"
            title={tGlobal('delete')}
            onClick={() => onDelete?.(item)}
          >
            <IconDelete />
          </button>
        </div>
      </td>
      <td className="text-left sticky left-[107px] bg-[#ffffff] shadow-[3px_0_4px_0_rgba(99,84,84,0.3)]">
        {item.promotionTitle}
      </td>
      <td className="">{item.promotionType}</td>
      <td className="text-right">
        {item.sportsPercentage}
        <br />
        {item.sportsMaxPayout}
      </td>
      <td className="text-right">
        {item.virtualSportsPercentage}
        <br />
        {item.virtualSportsMaxPayout}
      </td>
      <td className="text-right">
        {item.gamesPercentage}
        <br />
        {item.gamesMaxPayout}
      </td>
      <td className="text-right">
        {item.liveCasinoPercentage}
        <br />
        {item.liveCasinoMaxPayout}
      </td>
      <td className=""> </td>
      <td className="">{item.provider}</td>
      <td className="">
        <span className={`rounded-[20px] px-[10px] py-[3px] ${getStatusStyle(item.status)}`}>
          {item.status.toUpperCase()}
        </span>
      </td>
      <td className="">{item.lastUpdated}</td>
    </tr>
  );

  const buildBody = () => (
    <tbody className="font-[500]">
      {items && items.length > 0 ? (
        items.map(buildRow)
      ) : (
        <NoRecordFoundComponent colSpan={12} message="No pending promotions found" />
      )}
    </tbody>
  );

  if (isLoading) {
    return (
      <div className="flex justify-center items-center h-64">
        <div className="loading-spinner"></div>
      </div>
    );
  }

  return (
    <>
      <div className="table-fixed-header table-fixed-body bg-[#ffffff] max-h-[calc(100vh-140px)] overflow-y-auto">
        <table className="min-w-[1300px] w-full whitespace-nowrap border-collapse border border-[#0000001f] border-t-[0px]  border-l-[0px] text-[#333333] shadow-[0_1px_5px_#0003,0_2px_2px_#00000024,0_3px_1px_-2px_#0000001f]">
          {buildHeader()}
          {buildBody()}
        </table>
      </div>
    </>
  );
};

export default Table;
