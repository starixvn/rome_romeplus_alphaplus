import React, { useState, useEffect } from 'react';
import { useTranslation } from 'react-i18next';
import { transactionAPI } from '../../../../../services/transactionsHistoryAPI';
import CustomDropdown from '../../../../../components/DropdownComponent';
import { BankInfo } from '../../../../../services/interfaces/Transactions/BankInfo';

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
  const [accountOptions, setAccountOptions] = useState<BankInfo[]>([]);
  const [isLoading, setIsLoading] = useState(false);

  useEffect(() => {
    const fetchAccounts = async () => {
      setIsLoading(true);
      try {
        const options = await transactionAPI.getCashAgentBankInfo(transactionType);
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
          code: option.bankCode,
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
