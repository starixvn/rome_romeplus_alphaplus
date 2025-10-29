import { StrictMode } from 'react';
import { createRoot } from "react-dom/client";
import styleTopBanner from './TopBanner.module.scss';
import { useCookieLanguage } from '../../hooks/useCookieLanguage';
import i18n from '../../i18n/i18-advertisement';
import { Trans, useTranslation } from 'react-i18next';
import classNames from 'classnames';
import { useCashServiceAds } from './hooks/useCashServiceAds';
import { MESSAGE_TYPES } from './constants/advertisement';

function TopBannerComponent() {
    useCookieLanguage(i18n);
    const { t } = useTranslation('advertisement');
    const { isOpen } = useCashServiceAds();

    const handleActivateNow = () => {
        window.postMessage({ type: MESSAGE_TYPES.OPEN_POPUP }, '*');
        setTimeout(() => {
            window.postMessage({ type: MESSAGE_TYPES.GO_TO_STEP, step: 4 }, '*');
        }, 100);
    };

    const handleFindOutMore = () => {
        window.postMessage({ type: MESSAGE_TYPES.OPEN_POPUP }, '*');
        setTimeout(() => {
            window.postMessage({ type: MESSAGE_TYPES.FIND_OUT_MORE }, '*');
        }, 100);
    };

    if (!isOpen) {
        return null;
    }

    return (
        <div className={styleTopBanner['cash-service-banner'] + ' has-banner'}>
            <span className={styleTopBanner['exclusively']}>
                <img src="assets/images/lightning.png" />
                <span>{t('ExclusivelyForYou')}</span>
            </span>
            <div className={styleTopBanner['cash-service-banner-content']}>
                <span className={styleTopBanner['message']}>{
                    <Trans
                        t={t}
                        i18nKey="EnjoyCashServiceFreeFor14Days"
                        components={{
                            free: <span></span>
                        }}
                    />}
                </span>
                <button className={styleTopBanner['banner-btn']} onClick={handleActivateNow}>
                    {t('ActivateNow')}
                </button>
                <button className={classNames(styleTopBanner['banner-btn'], styleTopBanner['secondary'])} onClick={handleFindOutMore} >
                    {t('FindOutMore')}
                </button>
            </div>
        </div>
    );
}
export default TopBannerComponent;
const rootElement = document.getElementById('top-banner-root');
if (rootElement) {
    createRoot(rootElement).render(
        <StrictMode>
            <TopBannerComponent />
        </StrictMode>
    );
}