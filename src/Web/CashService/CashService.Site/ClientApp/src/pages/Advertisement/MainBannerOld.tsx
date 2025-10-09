import { useMemo, useState, useImperativeHandle, forwardRef, StrictMode, useEffect, useRef } from 'react';
import styles from './cash-service-ads.module.scss';
import { useTranslation, Trans } from 'react-i18next';
import classNames from 'classnames';
import { AnalyzeCashServiceAds } from './AnalzeCash';
import { createRoot } from 'react-dom/client';
import { useCookieLanguage } from '../../hooks/useCookieLanguage';
import i18n from '../../i18n/i18-advertisement';
import { axiosInstance, createHeaderXMLHttp } from '../../services/axios';

declare const appConfig: {
    isCanUpdateCashServiceStatus: boolean;
    isCanEditCashService: boolean;
    isOfferedCashService: boolean;
    isCashServiceEnabled: boolean;
    nickName: string;
};

declare const homeUrl: string;

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

interface MainContentProps {
    readonly onClickActivateNow: (() => void) | undefined;
    readonly findOutMore: () => void;
    readonly onDontShowAgain?: () => void;
}

function MainContent(props: MainContentProps) {
    const { onClickActivateNow, findOutMore, onDontShowAgain } = props;
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
            {onDontShowAgain &&
                <div className={styles['dont-show-again']} onClick={onDontShowAgain}>
                    <input type="checkbox" id="dont-show-again" />
                    <label htmlFor="dont-show-again">{t('DontShowAgain')}</label>
                </div>}
        </div>
    );
}

interface CashServiceAdsProps {
    readonly isOpen: boolean;
    readonly onDontShowAgain?: () => void;
    readonly onConfirm?: () => void;
    readonly onClose?: () => void;
    readonly cashServiceStatus: CashServiceStatus;
    readonly nickName: string;
}

enum ContentFrame {
    Main = 'MainContent',
    TourGuide = 'TourGuide'
}

interface StepConfig {
    id: number;
    title: string;
    image?: string;
    video?: string;
    description?: string;
    className?: string;
    isSpecial?: boolean;
    specialContent?: React.ReactNode;
}

const IconCheck = () => {
    return <span className={styles['icon-check']} />
}

interface StepSliderProps {
    steps: React.ReactNode[];
    currentStep: number;
    onStepChange: (step: number) => void;
    onClose?: () => void;
    onFinish?: () => void;
}

const StepSlider: React.FC<StepSliderProps> = (props) => {
    const { steps, currentStep, onStepChange, onClose, onFinish } = props;
    const { t } = useTranslation('advertisement');

    const stepIndex = currentStep as number;
    const isLast = stepIndex === steps.length - 1;
    const isFirst = stepIndex === 0;
    const cssBackButton = classNames(styles['step-button'], styles['back-button']);
    const cssNextButton = classNames(styles['step-button'], styles['next-button']);
    return (
        <div className={styles['step-slider']}>
            {onClose && (
                <button
                    onClick={() => {
                        onClose();
                    }}
                    className={styles['close-button']}
                >×</button>
            )}

            <div className={styles['step-content']}>{steps[stepIndex]}</div>

            <div className={styles['step-navigation']}>
                <button onClick={() => onStepChange((currentStep as number) - 1)} disabled={isFirst} className={cssBackButton}>
                    {t('Back')}
                </button>
                {isLast ?
                    <button onClick={onFinish} disabled={!onFinish} className={cssNextButton}>
                        {t('Confirm')}
                    </button>
                    : <>
                        <button onClick={onFinish} disabled={!onFinish} className={cssNextButton}>
                            {t('ActivateNow')}
                        </button>
                        <button onClick={() => onStepChange((currentStep as number) + 1)} className={cssBackButton}>
                            {t('Next')}
                        </button>
                    </>
                }
            </div>
            <div className={styles['step-counter']}>
                {(currentStep as number) + 1} of {steps.length}
            </div>
        </div>
    );
};

declare const cashServiceUrl: string;

export const CashServiceAds = forwardRef<CashServiceAdsRef, CashServiceAdsProps>((props, ref) => {
    const { t } = useTranslation('advertisement');
    const { isOpen, onClose, onConfirm, cashServiceStatus, onDontShowAgain, nickName } = props;
    const [contentFrame, setContentFrame] = useState(ContentFrame.Main);
    const imagePath = `${cashServiceUrl}assets/images/cash-service-ads/`;
    const bannerPath = useMemo(() => `${imagePath}BannerPC.jpg`, []);
    const [currentStep, setCurrentStep] = useState(0);

    const stepConfigs: StepConfig[] = useMemo(() => [
        {
            id: 0,
            title: 'InstantDepositsAndWithdrawals',
            image: 'step1.jpg',
            description: 'EasilyAddAPaymentGatewayAndBankAccountsSoYourMembersCanStartPlayingInstantly'
        },
        {
            id: 1,
            title: 'ManageCreditAndCashMember',
            image: 'step2.jpg',
            description: 'MaximiseYourEarningsByManagingBothCreditAndCashInSingleAccount'
        },
        {
            id: 2,
            title: 'DedicatedDomain',
            video: 'step3.mp4',
            description: 'ElevateYourBrandWithASignUpLinkedForMemberRegistrations',
            className: styles['support-scale']
        },
        {
            id: 3,
            title: 'CustomisePromotions',
            image: 'step4.jpg',
            description: 'CreateAndManagePromotionsFeaturingPopularProductsAndProvidersToKeepYourMembersEntertained'
        },
        {
            id: 4,
            title: 'SuperchargeYourCashFlow',
            isSpecial: true,
            specialContent: (
                <>
                    <div className={styles.divider} />
                    <div className={styles['step-highlight-block']}>
                        <span className={styles['free-badge']}>
                            <Trans i18nKey="14-daysFreeTrial" components={{ b: <b></b> }} />
                        </span>
                        <span className={styles['save-amount']}>
                            <Trans i18nKey="StartTodayAndEnjoyInstantSavingsOf" components={{ b: <b></b> }} />
                        </span>
                    </div>
                    <ul className={styles['step-benefits']}>
                        <li><IconCheck /> <span style={{ fontWeight: 'bold' }}>{t('NoPlatformFee')}</span> <span>- {t('SaveTHB200DayDuringTheTrial')}</span></li>
                        <li><IconCheck /> <span style={{ fontWeight: 'bold' }}> {t('Just015PerTransactionViaYourLinkedBankAccount')}</span></li>
                        <li><IconCheck /> {t('GetResultsStayIfYouLoveItCancelAnytime')}</li>
                    </ul>
                    <div className={styles['step-note']}>
                        <span className={styles['icon-info']}>ℹ️</span> {t('DontMissOutActivateNowAndTakeYourCashFlowToTheNextLevel')}
                    </div>
                </>
            )
        }
    ], [t]);

    const steps = useMemo(() => stepConfigs.map((config) => (
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
    )), [stepConfigs, t, imagePath]);

    const onActivateNow = () => {
        setContentFrame(ContentFrame.TourGuide);
        setCurrentStep(stepConfigs.length - 1);
    }

    const onFindOutMore = () => {
        setContentFrame(ContentFrame.TourGuide);
        setCurrentStep(0);
    }

    const goToStep = (step: number) => {
        setContentFrame(ContentFrame.TourGuide);
        setCurrentStep(step);
    }

    const close = () => {
        onClose?.();
    }

    useImperativeHandle(ref, () => ({
        findOutMore: onFindOutMore,
        activateNow: onActivateNow,
        goToStep: goToStep,
        close: close
    }), [onClose, stepConfigs.length]);

    if (!isOpen) {
        return null;
    }

    const callAnalyze = (step: number) => {
        AnalyzeCashServiceAds({
            name: nickName || '',
            step: step,
            act: 'close'
        });
    }

    return (
        <div id="cash-service-popup" className={styles.overlay}>
            <div className={styles.popup}>
                {onClose && <button
                    onClick={() => {
                        callAnalyze(0);
                        onClose();
                    }}
                    className={styles['close-button']}
                >
                    ×
                </button>}
                <img
                    src={bannerPath}
                    alt="AdvCash"
                    className={styles['banner-image']}
                />
                {contentFrame === ContentFrame.Main &&
                    <MainContent
                        onClickActivateNow={cashServiceStatus.isCanUpdateCashServiceStatus ? onActivateNow : undefined}
                        findOutMore={onFindOutMore}
                        onDontShowAgain={onDontShowAgain}
                    />}
                {contentFrame === ContentFrame.TourGuide &&
                    <StepSlider
                        steps={steps}
                        currentStep={currentStep as number}
                        onStepChange={setCurrentStep}
                        onClose={() => {
                            setContentFrame(ContentFrame.Main);
                            callAnalyze(currentStep + 1);
                            onClose?.();
                        }}
                        onFinish={cashServiceStatus.isCanUpdateCashServiceStatus ? onConfirm : undefined}
                    />}
            </div>
        </div>
    );
});

CashServiceAds.displayName = 'CashServiceAds';

const MainBanner = () => {
    useCookieLanguage(i18n);
    const [isOpen, setIsOpen] = useState(true);
    const [shouldShowPopup, setShouldShowPopup] = useState(false);
    const cashServiceAdsRef = useRef<CashServiceAdsRef | null>(null);

    useEffect(() => {
        const handleMessage = (event: MessageEvent) => {
            if (event.data.type === 'openCashServicePopup') {
                setShouldShowPopup(true);
                setIsOpen(true);
            }
            if (event.data.type === 'triggerFindOutMore') {
                cashServiceAdsRef.current?.findOutMore();
            }
            if (event.data.type === 'triggerActivateNow') {
                cashServiceAdsRef.current?.activateNow();
            }
            if (event.data.type === 'triggerGoToStep') {
                cashServiceAdsRef.current?.goToStep(event.data.step);
            }
            if (event.data.type === 'triggerClose') {
                cashServiceAdsRef.current?.close();
            }
        };

        window.addEventListener('message', handleMessage);

        // Cleanup function
        return () => {
            window.removeEventListener('message', handleMessage);
        };
    }, []);

    const onConfirmEnableCashService = async () => {
        const response = await axiosInstance.post(
            `/main/Dashboard/UpdateCashServiceStatus?IsEnableCash=true`, {}, {
            headers: createHeaderXMLHttp(),
        }
        );
        if (response.data.isSuccessful) {
            window.top && window.top.indexManager.showHideCashServiceChecklist(true);
            window.location.reload();
        }
        else {
            alert(response.data.message);
        }
    }
    const checkDontShowAgain = () => {
        if (appConfig.nickName) {
            localStorage.setItem(`${appConfig.nickName}-hideCashServiceAds`, 'true');
        }
        setIsOpen(false);
    }

    useEffect(() => {
        const hideAds = localStorage.getItem(`${appConfig.nickName}-hideCashServiceAds`);
        if (hideAds !== 'true') {
            setShouldShowPopup(true);
        }
    }, []);
    if (!shouldShowPopup) {
        return null;
    }

    return (
        <CashServiceAds
            ref={cashServiceAdsRef}
            onClose={() => {
                setIsOpen(false);
            }}
            isOpen={isOpen}
            onConfirm={onConfirmEnableCashService}
            cashServiceStatus={{
                isCanUpdateCashServiceStatus: appConfig.isCanUpdateCashServiceStatus,
                isCashServiceEnabled: appConfig.isCashServiceEnabled,
                isOfferedCashService: appConfig.isOfferedCashService
            }}
            onDontShowAgain={checkDontShowAgain}
            nickName={appConfig.nickName}
        />
    );
}

const rootElement = document.getElementById('adv-popup-root');
if (rootElement) {
    createRoot(rootElement).render(
        <StrictMode>
            <MainBanner />
        </StrictMode>
    );
}