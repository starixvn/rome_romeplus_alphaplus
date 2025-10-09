import '../../styles/common.css';
import '../../styles/global.scss';

import { StrictMode } from 'react';
import { createRoot } from "react-dom/client";
import './TopBanner.scss';
import { useCashServiceActions } from './hooks/useCashServiceActions';
import { useCookieLanguage } from '../../hooks/useCookieLanguage';
import i18n from '../../i18n/i18-advertisement';
import { useTranslation } from 'react-i18next';


function TopBannerComponent() {
    useCookieLanguage(i18n);
    const { t } = useTranslation('advertisement');
    const { onConfirmEnableCashService } = useCashServiceActions();

    const handleFindOutMore = () => {
        window.postMessage({ type: 'openCashServicePopup' }, '*');
        window.postMessage({ type: 'triggerFindOutMore' }, '*');
    };

    return (
        <div className="cash-service-banner">
            <span className="exclusively">
                <img src="assets/images/lightning.png" />
                <span>{t('ExclusivelyForYou')}</span>
            </span>
            <div className="cash-service-banner-content">
                <span className="message">{t('EnjoyCashServiceFreeFor14Days')}</span>
                <button className="banner-btn" onClick={onConfirmEnableCashService}>
                    {t('ActivateNow')}
                </button>
                <button className="banner-btn secondary" onClick={handleFindOutMore} >
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