import React, { useState, useEffect } from 'react';
import { useTranslation } from 'react-i18next';
import { getAccount } from '../../../../../services/transactionsHistoryAPI';
import { AccountOption } from '../../../../../services/interfaces/Transactions/AccountOption';
import CustomDropdown from '../../../../../components/ui/customDropdown';

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
    <div className="form-group mb-[8px]">
      <label className="block text-[12px] font-semibold text-[#333333] my-[8px]">
        {transactionType === 1 ? t('filter.to') : t('filter.from')}
      </label>
      <CustomDropdown
        options={accountOptions.map((option) => ({
          value: option.bankId,
          label: option.bankAccountNumber,
          code: option.BankCode,
        }))}
        onSelect={(selected) => onAccountChange(String(selected.value))}
        disable={isLoading}
        showOptionAll={true}
        initialValue={account}
      />
    </div>
  );
};

export default AccountFilter;
