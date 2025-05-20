import React from 'react';
import { useTranslation } from 'react-i18next';
import { ButtonComponent } from '../../../../components/ButtonComponents';

interface HistoryTabProps {
  activeTab: number;
  onTabChange: (tab: number) => void;
}

const HistoryTab: React.FC<HistoryTabProps> = ({ activeTab, onTabChange }) => {
  const { t } = useTranslation('transactionHistory');

  return (
    <>
      <div className="flex gap-[8px]">
        <ButtonComponent
          className="w-[183px]"
          type={activeTab === 1 ? 'apply' : 'cancel'}
          style="medium"
          text={t('deposit')}
          onClick={() => onTabChange(1)}
        />
        <ButtonComponent
          className="w-[183px]"
          type={activeTab === 2 ? 'apply' : 'cancel'}
          style="medium"
          text={t('withdrawal')}
          onClick={() => onTabChange(2)}
        />
      </div>
    </>
  );
};

export default HistoryTab;
