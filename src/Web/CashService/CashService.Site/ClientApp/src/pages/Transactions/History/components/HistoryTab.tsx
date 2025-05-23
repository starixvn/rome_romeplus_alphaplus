import React from 'react';
import { useTranslation } from 'react-i18next';
import { OnButtonComponent } from '../../../../components/ButtonComponents';
import { TransactionType } from '../../../../common/commonFunctions';

interface HistoryTabProps {
  activeTab: number;
  onTabChange: (tab: number) => void;
}

const HistoryTab: React.FC<HistoryTabProps> = ({ activeTab, onTabChange }) => {
  const { t } = useTranslation('transactionHistory');
  console.log(activeTab);
  return (
    <>
      <div className="flex gap-[8px]">
        <OnButtonComponent
          className="w-[183px]"
          text={t('deposit')}
          onClick={() => onTabChange(TransactionType.Deposit)}
        />
        <OnButtonComponent
          className="w-[183px]"
          text={t('withdrawal')}
          onClick={() => onTabChange(TransactionType.Withdrawal)}
        />
      </div>
    </>
  );
};

export default HistoryTab;
