import React from 'react';
import { useTranslation, Trans } from 'react-i18next';
import styles from '../cash-service-ads.module.scss';

const IconCheck = () => <span className={styles['icon-check']} />;

export const SpecialStepContent: React.FC = () => {
  const { t } = useTranslation('advertisement');
  
  return (
    <>
      <div className={styles.divider} />
      <div className={styles['step-highlight-block']}>
        <span className={styles['free-badge']}>
          <Trans t={t} i18nKey="14-daysFreeTrial" components={{ b: <b></b> }} />
        </span>
        <span className={styles['save-amount']}>
          <Trans t={t} i18nKey="StartTodayAndEnjoyInstantSavingsOf" components={{ b: <b></b> }} />
        </span>
      </div>
      <ul className={styles['step-benefits']}>
        <li>
          <IconCheck /> 
          <span style={{ fontWeight: 'bold' }}>{t('NoPlatformFee')}</span> 
          <span>- {t('SaveTHB200DayDuringTheTrial')}</span>
        </li>
        <li>
          <IconCheck /> 
          <span style={{ fontWeight: 'bold' }}>{t('Just015PerTransactionViaYourLinkedBankAccount')}</span>
        </li>
        <li>
          <IconCheck /> {t('GetResultsStayIfYouLoveItCancelAnytime')}
        </li>
      </ul>
      <div className={styles['step-note']}>
        <span className={styles['icon-info']}>ℹ️</span> 
        {t('DontMissOutActivateNowAndTakeYourCashFlowToTheNextLevel')}
      </div>
    </>
  );
};