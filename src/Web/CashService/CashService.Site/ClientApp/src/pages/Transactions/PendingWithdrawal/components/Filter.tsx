import React from 'react';
import { useTranslation } from 'react-i18next';

import { CalendarIcon, IconSearch } from '../../../../components/Icons';
import { LabelTitleComponent } from '../../../../components/TitleComponents';

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
          <LabelTitleComponent text={t('filter.recentWithdrawals')} />
          <div className="select-wrapper relative">
            <select
              value={recentWithdrawals}
              onChange={(e) => setRecentWithdrawals(e.target.value)}
              className="control-field"
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
            <span className="absolute transform-[translatey(-50%)] z-[10] top-1/2 left-[8px]">
              <IconSearch />
            </span>
            <input
              type="text"
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
              className="control-field pl-[30px]!"
              placeholder={t('filter.usernamePlaceholder')}
            />
          </div>
        </div>
      </div>
    </>
  );
};

export default Filter;
