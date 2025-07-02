import React from 'react';
import { useTranslation } from 'react-i18next';
import {
  ButtonTabActiveComponent,
  ButtonTabInactiveComponent,
} from '../../../../components/Buttons/index';
import { TransactionType } from '../../../../utils/commonFunctions';

interface HistoryTabProps {
  activeTab: number;
  onTabChange: (tab: number) => void;
}

const HistoryTab: React.FC<HistoryTabProps> = ({ activeTab, onTabChange }) => {
  const { t } = useTranslation('transactionHistory');
  const renderButtonApply = (text: string, onClick: () => void) => {
    if (activeTab == TransactionType.Deposit) {
      return <> <ButtonTabActiveComponent text={text} onClick={onClick} /></>;
    } else {
      return <><ButtonTabInactiveComponent text={text} onClick={onClick} /></>;
    }
  };

  const renderButtonCancel = (text: string, onClick: () => void) => {
    if (activeTab == TransactionType.Deposit) {
      return <><ButtonTabInactiveComponent text={text} onClick={onClick} /></>;
    } else {
      return <><ButtonTabActiveComponent text={text} onClick={onClick} /></>;
    }
  };
  return (
    <>
      <ul className='flex mt-[-12px] border-b border-[var(--border-color)] mb-[15px]'>
        <li>
        {renderButtonApply(t('deposit'), () => onTabChange(TransactionType.Deposit))}
        </li>
        <li>
        {renderButtonCancel(t('withdrawal'), () => onTabChange(TransactionType.Withdrawal))}
        </li>
      </ul>      
    </>
  );
};

export default HistoryTab;
