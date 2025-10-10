import '../../styles/common.css';
import '../../styles/global.scss';

import { StrictMode } from 'react';
import { createRoot } from "react-dom/client";
import styleTopBanner from './TopBanner.module.scss';
import { useCashServiceActions } from './hooks/useCashServiceActions';
import { useCookieLanguage } from '../../hooks/useCookieLanguage';
import i18n from '../../i18n/i18-advertisement';
import { Trans, useTranslation } from 'react-i18next';
import classNames from 'classnames';

function TopBannerComponent() {
    useCookieLanguage(i18n);
    const { t } = useTranslation('advertisement');
    const { onConfirmEnableCashService } = useCashServiceActions();

    const handleFindOutMore = () => {
        window.postMessage({ type: 'openCashServicePopup' }, '*');
        window.postMessage({ type: 'triggerFindOutMore' }, '*');
    };

    return (
        <div className={styleTopBanner['cash-service-banner']}>
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
                <button className={styleTopBanner['banner-btn']} onClick={onConfirmEnableCashService}>
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