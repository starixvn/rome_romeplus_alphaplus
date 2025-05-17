import React from 'react';
import { useTranslation } from 'react-i18next';
import { BankInfo } from '../../../../services/interfaces/Transactions/BankInfo';
import { IconSearch } from '../../../../components/Icons';

interface FilterProps {
  recentDeposits: string;
  setRecentDeposits: (value: string) => void;
  cashAgentOptions: BankInfo[];
  cashAgentTo: string;
  setCashAgentTo: (value: string) => void;
  searchTerm: string;
  setSearchTerm: (value: string) => void;
}

const Filter: React.FC<FilterProps> = ({
  recentDeposits,
  setRecentDeposits,
  cashAgentOptions,
  cashAgentTo,
  setCashAgentTo,
  searchTerm,
  setSearchTerm,
}) => {
  const { t } = useTranslation('pendingDeposit');
  
  return (
    <>
    <div className="flex flex-nowrap gap-[16px] max-[768px]:justify-between max-[768px]:gap-[0] max-[768px]:flex-wrap w-full">
      <div className="w-[230px] pt-[4px] max-[768px]:w-[48%] mb-[8px]">
        <label className="block  font-semibold text-[#333333] my-[8px]">
          {t('filter.recentDeposits')}
        </label>
        <div className="select-wrapper relative">
          <select
            value={recentDeposits}
            onChange={(e) => setRecentDeposits(e.target.value)}
            className="h-[32px] max-[768px]:h-[40px] w-[100%] border-[1px] border-[var(--border-control)] rounded-[4px] px-[8px]  focus:outline-none focus:border-[var(--button-border)] cursor-pointer border-[var(--border-control)]"
            style={{ height: '32px' }}
          >
            <option value="All">{t('filter.all')}</option>
            <option value="Last30Mins">{t('filter.last30Mins')}</option>
            <option value="Last60Mins">{t('filter.last60Mins')}</option>
          </select>
        </div>
      </div>

      <div className="w-[230px] pt-[4px] max-[768px]:w-[48%] mb-[8px]">
        <label className="block  font-semibold text-[#333333] my-[8px]">{t('filter.to')}</label>
        <div className="select-wrapper relative">
          <select
            value={cashAgentTo}
            onChange={(e) => setCashAgentTo(e.target.value)}
            className="h-[32px] max-[768px]:h-[40px] w-[100%] border-[1px] border-[var(--border-control)] rounded-[4px] px-[8px]  focus:outline-none focus:border-[var(--button-border)] cursor-pointer border-[var(--border-control)]"
            style={{ height: '32px' }}
          >
            <option value="All">{t('filter.all')}</option>
            {cashAgentOptions.map((option) => (
              <option key={option.bankId} value={option.bankId}>
                {option.bankAccountNumber}
              </option>
            ))}
          </select>
        </div>
      </div>

      
    </div>
    <div className="flex mb-[16px]">
      <div className="w-[250px] ml-auto">
        <div className="relative">
          <span className='absolute top-[8px] left-[8px] z-[10] max-[768px]:top-[12px]'>
            <IconSearch />
          </span>
          <input
            type="text"
            value={searchTerm}
            onChange={(e) => setSearchTerm(e.target.value)}
            className="h-[32px] max-[768px]:h-[40px] w-[100%] pl-[30px] border-[1px] border-[#0000003D] rounded-[4px] px-[8px] focus:outline-none focus:border-[var(--button-border)] shadow-[0_0_6px_rgba(0,0,0,0.16)]"
            placeholder={t('filter.usernamePlaceholder')}
          />
        </div>
      </div> 
    </div>
    </>
  );
};

export default Filter; 