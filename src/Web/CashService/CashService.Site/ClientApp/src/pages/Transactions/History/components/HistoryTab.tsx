import React from 'react';
import { useTranslation } from 'react-i18next';
import {
  ButtonCancelComponent,
  ButtonApplyComponent,
} from '../../../../components/ButtonComponents';
import { TransactionType } from '../../../../common/commonFunctions';

interface HistoryTabProps {
  activeTab: number;
  onTabChange: (tab: number) => void;
}

const HistoryTab: React.FC<HistoryTabProps> = ({ activeTab, onTabChange }) => {
  const { t } = useTranslation('transactionHistory');
  const renderButtonApply = (text: string, onClick: () => void) => {
    if (activeTab == TransactionType.Deposit) {
      return <ButtonApplyComponent text={text} onClick={onClick} />;
    } else {
      return <ButtonCancelComponent text={text} onClick={onClick} />;
    }
  };

  const renderButtonCancel = (text: string, onClick: () => void) => {
    if (activeTab == TransactionType.Deposit) {
      return <ButtonCancelComponent text={text} onClick={onClick} />;
    } else {
      return <ButtonApplyComponent text={text} onClick={onClick} />;
    }
  };
  return (
    <>
      <div className="flex gap-[8px]">
        {renderButtonApply(t('deposit'), () => onTabChange(TransactionType.Deposit))}
        {renderButtonCancel(t('withdrawal'), () => onTabChange(TransactionType.Withdrawal))}
      </div>
    </>
  );
};

export default HistoryTab;
