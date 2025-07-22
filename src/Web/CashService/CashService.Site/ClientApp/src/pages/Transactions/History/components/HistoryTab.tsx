import React from 'react';
import { useTranslation } from 'react-i18next';
import { ButtonTabComponent } from '../../../../components/Buttons/index';
import { TransactionType } from '../../../../utils/commonFunctions';

interface HistoryTabProps {
  activeTab: number;
  onTabChange: (tab: number) => void;
}

const HistoryTab: React.FC<HistoryTabProps> = ({ activeTab, onTabChange }) => {
  const { t } = useTranslation('transactionHistory');

  return (
    <>
      <ul className="flex mt-[-12px] border-b border-[var(--border-color)] mb-[15px]">
        <li>
          <ButtonTabComponent
            text={t('deposit')}
            onClick={() => onTabChange(TransactionType.Deposit)}
            isActive={activeTab == TransactionType.Deposit}
          />
        </li>
        <li>
          <ButtonTabComponent
            text={t('withdrawal')}
            onClick={() => onTabChange(TransactionType.Withdrawal)}
            isActive={activeTab == TransactionType.Withdrawal}
          />
        </li>
      </ul>
    </>
  );
};

export default HistoryTab;
