import { useTranslation } from 'react-i18next';
import { ButtonTabComponent } from '../../../../components/Buttons/index';

export default function PromotionReportTab({
  activeTab,
  onTabChange,
}: {
  activeTab: number;
  onTabChange: (tab: number) => void;
}) {
  const { t } = useTranslation('promotion');

  return (
    <ul className="flex mt-[-12px] border-b border-[var(--border-color)] mb-[15px]">
      <li>
        <ButtonTabComponent
          text={t('promotionReport')}
          onClick={() => onTabChange(0)}
          isActive={activeTab === 0}
        />
      </li>
      <li>
        <ButtonTabComponent
          text={t('manualBonus')}
          onClick={() => onTabChange(1)}
          isActive={activeTab === 1}
        />
      </li>
    </ul>
  );
}
