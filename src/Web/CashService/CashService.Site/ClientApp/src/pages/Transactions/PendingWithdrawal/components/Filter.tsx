import React, { useState, useEffect } from 'react';
import { useTranslation } from 'react-i18next';
import { transactionAPI } from '../../../../services/transactionsHistoryAPI';
import { TransactionTypes } from '../../../../utils/constants';
import DropdownComponent, { DropdownOption } from '../../../../components/CustomDropdownComponent';

interface FilterProps {
  recentWithdrawals: string;
  setRecentWithdrawals: (value: string) => void;
  toFilter: string;
  setToFilter: (value: string) => void;
  searchTerm: string;
  setSearchTerm: (value: string) => void;
}

const Filter: React.FC<FilterProps> = ({
  recentWithdrawals,
  setRecentWithdrawals,
  toFilter,
  setToFilter,
  searchTerm,
  setSearchTerm,
}) => {
  const { t } = useTranslation('pendingWithdrawal');
  const [bankOptions, setBankOptions] = useState<DropdownOption[]>([]);

  useEffect(() => {
    const fetchBankOptions = async () => {
      try {
        const [providers, bankInfo] = await Promise.all([
          transactionAPI.getCashAgentProvider(TransactionTypes.Withdrawal),
          transactionAPI.getCashAgentBankInfo(TransactionTypes.Withdrawal),
        ]);

        const providerOptions = providers.map((provider) => ({
          value: `${provider.providerName}_${provider.providerName}`,
          label: provider.providerName,
          code: provider.providerName,
        }));

        const bankOptions = bankInfo.map((bank) => ({
          value: `${bank.bankCode}_${bank.bankAccountNumber}`,
          label: bank.bankAccountNumber,
          code: bank.bankCode,
        }));

        // Combine both options
        setBankOptions([...bankOptions, ...providerOptions]);
      } catch (error) {
        console.error('Error fetching bank options:', error);
      }
    };

    fetchBankOptions();
  }, [t]);

  const handleProviderSelect = (option: DropdownOption) => {
    setToFilter(option.value.toString());
  };

  const handleRecentWithdrawalsChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
    setRecentWithdrawals(e.target.value);
  };

  return (
    <div className="flex flex-wrap gap-6 mb-6">
      <div className="w-full md:w-48">
        <label className="block text-sm font-medium text-gray-700 mb-1">
          {t('filter.recentWithdrawals')}
        </label>
        <div className="select-wrapper relative">
          <select
            value={recentWithdrawals}
            onChange={handleRecentWithdrawalsChange}
            className="appearance-none block w-full bg-white border border-gray-300 px-3 py-1 pr-8 rounded text-gray-900 focus:outline-none focus:ring-blue-500 focus:border-blue-500 sm:text-sm h-8"
            style={{ height: '32px' }}
          >
            <option value="All">{t('filter.all')}</option>
            <option value="Today">{t('filter.today')}</option>
            <option value="Yesterday">{t('filter.yesterday')}</option>
            <option value="Last7Days">{t('filter.last7Days')}</option>
          </select>
          <div className="pointer-events-none absolute inset-y-0 right-0 flex items-center px-2 text-gray-700">
            <svg
              className="h-4 w-4 fill-current"
              xmlns="http://www.w3.org/2000/svg"
              viewBox="0 0 20 20"
            >
              <path d="M9.293 12.95l.707.707L15.657 8l-1.414-1.414L10 10.828 5.757 6.586 4.343 8z" />
            </svg>
          </div>
        </div>
      </div>

      <div className="w-full md:w-48">
        <label className="block text-sm font-medium text-gray-700 mb-1">{t('filter.to')}</label>
        <DropdownComponent
          options={bankOptions}
          onSelect={handleProviderSelect}
          initialValue={toFilter}
          showOptionAll={true}
        />
      </div>

      <div className="w-full md:w-auto ml-auto">
        <label className="block text-sm font-medium text-gray-700 mb-1 opacity-0">
          {t('filter.search')}
        </label>
        <div className="relative">
          <div className="pointer-events-none absolute inset-y-0 left-0 flex items-center pl-3">
            <svg className="h-5 w-5 text-gray-400" viewBox="0 0 20 20" fill="currentColor">
              <path
                fillRule="evenodd"
                d="M9 3.5a5.5 5.5 0 100 11 5.5 5.5 0 000-11zM2 9a7 7 0 1112.452 4.391l3.328 3.329a.75.75 0 11-1.06 1.06l-3.329-3.328A7 7 0 012 9z"
                clipRule="evenodd"
              />
            </svg>
          </div>
          <input
            type="text"
            value={searchTerm}
            onChange={(e) => setSearchTerm(e.target.value)}
            className="block w-full rounded-md border border-gray-300 py-1 pl-10 pr-3 text-gray-900 placeholder-gray-500 focus:border-blue-500 focus:outline-none focus:ring-blue-500 sm:text-sm h-8"
            style={{ height: '32px' }}
            placeholder={t('filter.usernamePlaceholder')}
          />
        </div>
      </div>
    </div>
  );
};

export default Filter;
