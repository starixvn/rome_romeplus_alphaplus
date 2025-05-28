import React from 'react';
import { useTranslation } from 'react-i18next';
import { CalendarIcon, SearchIcon } from '../../../../components/Icons';
import { LableTitleComponent } from '../../../../components/TitleComponents';
interface FilterProps {
  recentWithdrawals: string;
  setRecentWithdrawals: (value: string) => void;
  searchTerm: string;
  setSearchTerm: (value: string) => void;
}

const Filter: React.FC<FilterProps> = ({
  recentWithdrawals,
  setRecentWithdrawals,
  searchTerm,
  setSearchTerm,
}) => {
  const { t } = useTranslation('pendingWithdrawal');

  return (
    <>
      <div className="flex flex-nowrap gap-[16px] max-[768px]:justify-between max-[768px]:gap-[0] max-[768px]:flex-wrap w-full">
        <div className="w-[230px] pt-[4px] max-[768px]:w-[48%] mb-[8px]">
          <LableTitleComponent text={t('filter.recentWithdrawals')} />
          <div className="select-wrapper relative">
            <select
              value={recentWithdrawals}
              onChange={(e) => setRecentWithdrawals(e.target.value)}
              className="h-[32px] max-[768px]:h-[40px] w-[100%] border-[1px] border-[var(--border-control)] bg-[var(--background-control)] rounded-[4px] px-[8px] pr-[32px] focus:outline-none focus:border-[var(--button-border)] cursor-pointer appearance-none"
            >
              <option value="All">{t('filter.all')}</option>
              <option value="Last30Mins">{t('filter.last30Mins')}</option>
              <option value="Last60Mins">{t('filter.last60Mins')}</option>
            </select>
            <span className="absolute right-[8px] top-[50%] transform -translate-y-1/2 pointer-events-none">
              <CalendarIcon />
            </span>
          </div>
        </div>
      </div>
      <div className="flex mb-[16px]">
        <div className="w-[250px] ml-auto">
          <div className="relative">
            <span className="absolute top-[8px] left-[8px] z-[10] max-[768px]:top-[12px]">
              <SearchIcon />
            </span>
            <input
              type="text"
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
              className="h-[32px] max-[768px]:h-[40px] w-[100%] pl-[30px] bg-[#ffffff] border-[1px] border-[#0000003D] rounded-[4px] px-[8px] focus:outline-none focus:border-[var(--button-border)] shadow-[0_0_6px_rgba(0,0,0,0.16)]"
              placeholder={t('filter.usernamePlaceholder')}
            />
          </div>
        </div>
      </div>
    </>
  );
};

export default Filter;
