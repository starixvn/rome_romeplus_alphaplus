import React, { useState, useEffect } from 'react';
import { useTranslation } from 'react-i18next';
import CustomDropdown from '../../../../../components/CustomDropdownComponent';
import { TransactionType } from '../../../../../utils/commonFunctions';
import BankIcon from '../../../../../components/BankComponents';
import { LabelTitleComponent } from '../../../../../components/TitleComponents';

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
    <div className="form-group">
      <LabelTitleComponent
        text={transactionType === TransactionType.Deposit ? t('filter.to') : t('filter.from')}
      />
      <CustomDropdown
        options={accountOptions.map((option) => {
          if (option === '-') return { value: option, label: option };
          const [bankCode, accountNumber] = option.split('-');
          return {
            value: option,
            label: accountNumber,
            code: bankCode,
            icon: <BankIcon code={bankCode} />,
          };
        })}
        onSelect={(selected) => onAccountChange(String(selected.value))}
        disable={isLoading}
        initialValue={account}
        showOptionAll={true}
        searchable={true}
      />
    </div>
  );
};

export default AccountFilter;
