import { useTranslation } from 'react-i18next';

import { useGlobalContext } from '../../context/global-context';
import { closeLeftPanelOnMobile } from '../../helpers';
import styles from './QRCode.module.scss';

export default function QRCode() {
    const { t } = useTranslation();
    const { mobileView, customer } = useGlobalContext();

    const handleOpenShowQRCode = () => {
        if (mobileView) {
            closeLeftPanelOnMobile();
        }

        const mainFrame = window.top?.$('#main');
        // if (!window.top?.alpha.core.isIe()) {
        //     mainFrame?.contents().find('head').append(window.$('#AssetCommonCssLink')?.val());
        //     mainFrame?.contents().find('body').empty().block();
        // }

        mainFrame?.attr('src', customer?.ShowQRCodeUrl || '');
    };

    return (
        <div className={styles.container}>
            <div className={styles['feature-item-qr']} onClick={handleOpenShowQRCode}>
                <span className="icon-qr-code" />
                {t('FirstLoginQRCode')}
            </div>
        </div>
    );
}
