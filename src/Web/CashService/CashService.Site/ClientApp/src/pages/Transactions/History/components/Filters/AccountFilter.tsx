import React, { useState, useEffect } from 'react';
import { useTranslation } from 'react-i18next';
import CustomDropdown from '../../../../../components/CustomDropdownComponent';
import BankIcon from '../../../../../components/BankComponents';

interface AccountFilterProps {
  account: string;
  transactionType: number;
  paymentInformations: string[];
  onAccountChange: (value: string) => void;
  onAccountsLoaded?: () => void;
}

const AccountFilter: React.FC<AccountFilterProps> = ({
  account,
  transactionType,
  onAccountChange,
  paymentInformations,
  onAccountsLoaded,
}) => {
  const { t } = useTranslation('transactionHistory');
  const [accountOptions, setAccountOptions] = useState<string[]>([]);
  const [isLoading, setIsLoading] = useState(false);

  useEffect(() => {
    const fetchAccounts = async () => {
      setIsLoading(true);
      try {
        const options = paymentInformations;
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
  }, [transactionType, paymentInformations, onAccountsLoaded]);

  return (
    <div className="form-group mb-[8px]">
      <label className="block text-[12px] font-semibold text-[#333333] my-[8px]">
        {transactionType === 1 ? t('filter.to') : t('filter.from')}
      </label>
      <CustomDropdown
        options={accountOptions.map((option) => ({
          value: option,
          label: option,
          code: option,
          icon: <BankIcon code={option.bankCode} />
        }))}
        onSelect={(selected) => onAccountChange(String(selected.value))}
        disable={isLoading}
        initialValue={account}
        showOptionAll={true}
      />
    </div>
  );
};

export default AccountFilter;
