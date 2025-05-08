import React, { useState, useEffect } from 'react';
import { ChevronDownIcon } from '../../../../../components/Icons';
import { useTranslation } from 'react-i18next';
import '../../../../../i18n/i18n';
import { getAccount } from '../../../../../services/transactionsHistoryAPI';
import { AccountOption } from '../../../../../services/interfaces/Transactions/AccountOption';

interface AccountFilterProps {
  account: string;
  transactionType: number;
  onAccountChange: (value: string) => void;
  onAccountsLoaded?: () => void;
}

const AccountFilter: React.FC<AccountFilterProps> = ({
  account,
  transactionType,
  onAccountChange,
  onAccountsLoaded,
}) => {
  const { t } = useTranslation('transactionHistory');
  const [accountOptions, setAccountOptions] = useState<AccountOption[]>([]);
  const [isLoading, setIsLoading] = useState(false);

  useEffect(() => {
    const fetchAccounts = async () => {
      setIsLoading(true);
      try {
        const options = await getAccount(transactionType);
        setAccountOptions(options);
        if (onAccountsLoaded) {
          onAccountsLoaded();
        }
      } catch (error) {
        console.error('Failed to fetch accounts:', error);
        setAccountOptions([]);
      } finally {
        setIsLoading(false);
      }
    };

    fetchAccounts();
  }, [transactionType, onAccountsLoaded]);

  return (
    <div className="w-[180px]">
      <label className="block text-sm font-medium text-gray-700 mb-1">
        {transactionType === 1 ? t('filter.to') : t('filter.from')}
      </label>
      <div className="select-wrapper relative">
        <select
          value={account}
          onChange={(e) => onAccountChange(e.target.value)}
          className="appearance-none block w-full bg-white border border-gray-300 px-3 py-1.5 pr-8 rounded text-gray-900 focus:outline-none focus:ring-1 focus:ring-blue-500 focus:border-blue-500 sm:text-sm h-9"
          disabled={isLoading}
        >
          <option value="" disabled>
            {isLoading ? t('filter.loadingAccounts') : t('filter.selectAccount')}
          </option>
          {!isLoading &&
            accountOptions.map((option) => (
              <option key={option.bankId} value={option.bankId}>
                {option.bankAccountNumber}
              </option>
            ))}
        </select>
        <div className="pointer-events-none absolute inset-y-0 right-0 flex items-center px-2 text-gray-700">
          <ChevronDownIcon className="h-4 w-4" />
        </div>
      </div>
    </div>
  );
};

export default AccountFilter;
