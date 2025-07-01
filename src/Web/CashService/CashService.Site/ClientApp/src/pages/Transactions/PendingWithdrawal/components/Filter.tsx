import React from 'react';
import { useTranslation } from 'react-i18next';

import { IconArrowDown, IconSearch } from '../../../../components/Icons';
import { LabelTitleComponent } from '../../../../components/Title';

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
      <div className="flex flex-wrap w-full">
        <div className="w-[190px] mb-[12px] mr-[12px]">
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
            <IconArrowDown className="w-[12px] h-[12px] absolute right-[3px] top-[7px] fill-[#333] dark:fill-[#ededed]  " />
          </div>
        </div>
      </div>
      <div className="search-section w-full border border-[var(--table-border-color)] border-b-0 p-[6px]">
        <div className="flex">
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
      </div>      
    </>
  );
};

export default Filter;
