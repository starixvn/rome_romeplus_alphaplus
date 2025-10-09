import React from 'react';
import { useTranslation, Trans } from 'react-i18next';
import styles from '../cash-service-ads.module.scss';

interface MainContentProps {
  readonly onClickActivateNow: (() => void) | undefined;
  readonly findOutMore: () => void;
  readonly onDontShowAgain?: () => void;
}

const MainContent: React.FC<MainContentProps> = ({ 
  onClickActivateNow, 
  findOutMore, 
  onDontShowAgain 
}) => {
  const { t } = useTranslation('advertisement');
  
  return (
    <div className={styles['ads-content']}>
      <div className={styles['main-title']}>
        {t('ExclusivelyForYou')}
      </div>
      <div className={styles['sub-title']}>
        <Trans
          t={t}
          i18nKey="EnjoyCashServiceFreeFor14Days"
          components={{
            free: <span className={styles['pulse-animate'] + ' ' + styles['free-highlight']}></span>
          }}
        />
      </div>
      <div className={styles['button-container']}>
        <button
          className={styles['primary-button']}
          onClick={onClickActivateNow}
          disabled={!onClickActivateNow}
        >
          {t('ActivateNow')}
        </button>
        <button
          className={styles['secondary-button']}
          onClick={findOutMore}
        >
          {t('FindOutMore')}
        </button>
      </div>
      {onDontShowAgain && (
        <div className={styles['dont-show-again']} onClick={onDontShowAgain}>
          <input type="checkbox" id="dont-show-again" />
          <label htmlFor="dont-show-again">{t('DontShowAgain')}</label>
        </div>
      )}
    </div>
  );
};

export default MainContent;
