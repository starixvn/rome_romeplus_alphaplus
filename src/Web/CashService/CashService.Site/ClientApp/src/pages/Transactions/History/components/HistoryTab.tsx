import React from 'react';
import { useTranslation } from 'react-i18next';
import {
  ButtonApplyComponent,
  ButtonOutlineComponent,
} from '../../../../components/ButtonComponents';
import { TransactionType } from '../../../../utils/commonFunctions';

interface HistoryTabProps {
  activeTab: number;
  onTabChange: (tab: number) => void;
}

const HistoryTab: React.FC<HistoryTabProps> = ({ activeTab, onTabChange }) => {
  const { t } = useTranslation('transactionHistory');
  const renderButtonApply = (text: string, onClick: () => void) => {
    if (activeTab == TransactionType.Deposit) {
      return <ButtonApplyComponent className="[&>*]:min-w-[150px]" text={text} onClick={onClick} />;
    } else {
      return <ButtonOutlineComponent className="[&>*]:min-w-[150px]" text={text} onClick={onClick} />;
    }
  };

  const renderButtonCancel = (text: string, onClick: () => void) => {
    if (activeTab == TransactionType.Deposit) {
      return <ButtonOutlineComponent className="[&>*]:min-w-[150px]" text={text} onClick={onClick} />;
    } else {
      return <ButtonApplyComponent className="[&>*]:min-w-[150px]" text={text} onClick={onClick} />;
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
