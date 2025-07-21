import React from 'react';
import { useTranslation } from 'react-i18next';
import BankIcon from '../../../../components/BankComponents';
import CustomDropdown from '../../../../components/CustomDropdownComponent';
import { IconArrowDown, IconSearch } from '../../../../components/Icons';
import { TitleComponent } from '../../../../components/Title';

export enum TimeFilterOptions {
  All = 'All',
  Last30Mins = 'Last30Mins',
  Last60Mins = 'Last60Mins',
}

interface FilterProps {
  recentDeposits: TimeFilterOptions;
  setRecentDeposits: (value: TimeFilterOptions) => void;
  setCashAgentTo: (value: string) => void;
  searchTerm: string;
  setSearchTerm: (value: string) => void;
  uniquePaymentToOptions: string[];
}

const TimeFilter: React.FC<{
  value: TimeFilterOptions;
  onChange: (value: TimeFilterOptions) => void;
  t: (key: string) => string;
}> = ({ value, onChange, t }) => (
  <div className="w-[190px] mb-[12px] mr-[12px]">
    <TitleComponent type='label' text={t('filter.recentDeposits')} />
    <div className="select-wrapper relative">
      <select
        value={value}
        onChange={(e) => onChange(e.target.value as TimeFilterOptions)}
        className="control-field"
      >
        <option value={TimeFilterOptions.All}>{t('filter.all')}</option>
        <option value={TimeFilterOptions.Last30Mins}>{t('filter.last30Mins')}</option>
        <option value={TimeFilterOptions.Last60Mins}>{t('filter.last60Mins')}</option>
      </select>
      <IconArrowDown className="w-[12px] h-[12px] absolute right-[3px] top-[7px] fill-[#333] dark:fill-[#ededed]  " />
    </div>
  </div>
);

const BankFilter: React.FC<{
  options: string[];
  onSelect: (value: string) => void;
  t: (key: string) => string;
}> = ({ options, onSelect, t }) => (
  <div className="w-[190px] mb-[12px] mr-[12px]">
    <TitleComponent type='label' text={t('filter.to')} />
    <CustomDropdown
      options={options.map((option) => {
        const [bankCode, accountNumber] = option.split('-');
        return {
          value: option,
          label: accountNumber || bankCode,
          code: bankCode,
          icon: bankCode && accountNumber ? <BankIcon code={bankCode} /> : null,
        };
      })}
      onSelect={(selected) => onSelect(String(selected.value))}
      showOptionAll={true}
      searchable={true}
    />
  </div>
);

const SearchInput: React.FC<{
  value: string;
  onChange: (value: string) => void;
  placeholder: string;
}> = ({ value, onChange, placeholder }) => (
  <div className="flex">
    <div className="w-[250px] ml-auto">
      <div className="relative">
        <span className="absolute transform-[translatey(-50%)] z-[10] top-1/2 left-[8px]">
          <IconSearch />
        </span>
        <input
          type="text"
          value={value}
          onChange={(e) => onChange(e.target.value)}
          className="control-field pl-[30px]!"
          placeholder={placeholder}
        />
      </div>
    </div>
  </div>
);

const Filter: React.FC<FilterProps> = ({
  recentDeposits,
  setRecentDeposits,
  setCashAgentTo,
  searchTerm,
  setSearchTerm,
  uniquePaymentToOptions,
}) => {
  const { t } = useTranslation('pendingDeposit');

  return (
    <>
      <div className="flex flex-wrap w-full">
        <TimeFilter
          value={recentDeposits as TimeFilterOptions}
          onChange={setRecentDeposits}
          t={t}
        />
        <BankFilter options={uniquePaymentToOptions} onSelect={setCashAgentTo} t={t} />
      </div>
      <div className="search-section w-full border border-[var(--table-border-color)] border-b-0 p-[6px]">
      <SearchInput
        value={searchTerm}
        onChange={setSearchTerm}
        placeholder={t('filter.usernamePlaceholder')}
      />
      </div>
      
    </>
  );
};

export default Filter;
