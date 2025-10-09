import { useMemo, useState, useImperativeHandle, forwardRef, StrictMode, useEffect } from 'react';
import styles from './cash-service-ads.module.scss';
import { useTranslation } from 'react-i18next';
import classNames from 'classnames';
import { createRoot } from 'react-dom/client';
import { useCookieLanguage } from '../../hooks/useCookieLanguage';
import i18n from '../../i18n/i18-advertisement';

import CloseButton from './components/CloseButton';
import BannerImage from './components/BannerImage';
import ContentRenderer from './components/ContentRenderer';
import SpecialStepContent from './components/SpecialStepContent';

import { useCashServiceAds } from './hooks/useCashServiceAds';
import { useCashServiceActions } from './hooks/useCashServiceActions';

import { STEP_CONFIGS, CONTENT_FRAME, STORAGE_KEYS, StepConfig } from './constants/advertisement';

declare const appConfig: {
  isCanUpdateCashServiceStatus: boolean;
  isCanEditCashService: boolean;
  isOfferedCashService: boolean;
  isCashServiceEnabled: boolean;
  nickName: string;
};

declare const homeUrl: string;
declare const cashServiceUrl: string;

export interface CashServiceStatus {
  isOfferedCashService: boolean;
  isCanUpdateCashServiceStatus: boolean;
  isCashServiceEnabled: boolean;
  isAllowCloseAds?: boolean;
}

export interface CashServiceAdsRef {
  findOutMore: () => void;
  activateNow: () => void;
  goToStep: (step: number) => void;
  close: () => void;
}

interface CashServiceAdsProps {
  readonly isOpen: boolean;
  readonly onDontShowAgain?: () => void;
  readonly onConfirm?: () => void;
  readonly onClose?: () => void;
  readonly cashServiceStatus: CashServiceStatus;
  readonly nickName: string;
}

export const CashServiceAds = forwardRef<CashServiceAdsRef, CashServiceAdsProps>((props, ref) => {
  const { t } = useTranslation('advertisement');
  const { isOpen, onClose, onConfirm, cashServiceStatus, onDontShowAgain, nickName } = props;
  
  const [contentFrame, setContentFrame] = useState<string>(CONTENT_FRAME.MAIN);
  const [currentStep, setCurrentStep] = useState(0);
  
  const imagePath = `${cashServiceUrl}assets/images/cash-service-ads/`;
  const bannerPath = useMemo(() => `${imagePath}BannerPC.jpg`, [imagePath]);

  const stepConfigs = useMemo(() => 
    STEP_CONFIGS.map((config: StepConfig) => ({
      ...config,
      className: config.className ? styles[config.className] : undefined,
      specialContent: config.isSpecial ? <SpecialStepContent /> : undefined
    })), [t]
  );

  const steps = useMemo(() => 
    stepConfigs.map((config) => (
      <div key={config.id}>
        <div className={styles['step-title']}>{t(config.title)}</div>
        {!config.isSpecial && config.image && (
          <div className={styles['step-media']}>
            <img
              className={classNames(styles['step-image'], config.className)}
              src={`${imagePath}${config.image}`}
              alt={t(config.title)}
            />
          </div>
        )}
        {!config.isSpecial && config.video && (
          <div className={styles['step-media']}>
            <video
              style={{ zIndex: 1, position: 'relative' }}
              className={classNames(styles['step-image'], config.className)}
              src={`${imagePath}${config.video}`}
              muted
              loop
              playsInline
              controls
            />
          </div>
        )}
        {!config.isSpecial && (
          <div className={styles['step-description']}>{t(config.description as string)}</div>
        )}
        {config.isSpecial && config.specialContent}
      </div>
    )), [stepConfigs, t, imagePath]
  );

  const actions = useMemo(() => ({
    findOutMore: () => {
      setContentFrame(CONTENT_FRAME.TOUR_GUIDE as string);
      setCurrentStep(0);
    },
    activateNow: () => {
      setContentFrame(CONTENT_FRAME.TOUR_GUIDE as string);
      setCurrentStep(stepConfigs.length - 1);
    },
    goToStep: (step: number) => {
      setContentFrame(CONTENT_FRAME.TOUR_GUIDE as string);
      setCurrentStep(step);
    },
    close: () => onClose?.()
  }), [onClose, stepConfigs.length]);

  useImperativeHandle(ref, () => actions, [actions]);

  if (!isOpen) {
    return null;
  }

  return (
    <div id="cash-service-popup" className={styles.overlay}>
      <div className={styles.popup}>
        <CloseButton onClose={onClose} nickName={nickName} />
        <BannerImage src={bannerPath} />
        <ContentRenderer 
          contentFrame={contentFrame}
          cashServiceStatus={cashServiceStatus}
          actions={actions}
          onDontShowAgain={onDontShowAgain}
          steps={steps}
          currentStep={currentStep}
          onStepChange={setCurrentStep}
          onConfirm={onConfirm}
          nickName={nickName}
        />
      </div>
    </div>
  );
});

CashServiceAds.displayName = 'CashServiceAds';

const MainBanner = () => {
  useCookieLanguage(i18n);
  const { isOpen, setIsOpen, shouldShowPopup, setShouldShowPopup, cashServiceAdsRef } = useCashServiceAds();
  const { onConfirmEnableCashService, checkDontShowAgain } = useCashServiceActions();

  useEffect(() => {
    const hideAds = localStorage.getItem(STORAGE_KEYS.HIDE_ADS(appConfig.nickName));
    if (hideAds !== 'true') {
      setShouldShowPopup(true);
    }
  }, [setShouldShowPopup]);

  if (!shouldShowPopup) {
    return null;
  }

  const cashServiceStatus = {
    isCanUpdateCashServiceStatus: appConfig.isCanUpdateCashServiceStatus,
    isCashServiceEnabled: appConfig.isCashServiceEnabled,
    isOfferedCashService: appConfig.isOfferedCashService
  };

  return (
    <CashServiceAds
      ref={cashServiceAdsRef}
      onClose={() => setIsOpen(false)}
      isOpen={isOpen}
      onConfirm={onConfirmEnableCashService}
      cashServiceStatus={cashServiceStatus}
      onDontShowAgain={checkDontShowAgain}
      nickName={appConfig.nickName}
    />
  );
};

const rootElement = document.getElementById('adv-popup-root');
if (rootElement) {
  createRoot(rootElement).render(
    <StrictMode>
      <MainBanner />
    </StrictMode>
  );
}
