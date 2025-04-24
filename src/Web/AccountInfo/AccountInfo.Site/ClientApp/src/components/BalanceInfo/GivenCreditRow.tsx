import classNames from 'classnames';
import { useTranslation } from 'react-i18next';
import { NumericFormat } from 'react-number-format';

import { LoadCustomerInfo } from '../../constants';
import { useGlobalContext } from '../../context/global-context';
import { mobileScale, resizePopupOnMobile } from '../../helpers';
import styles from './BalanceInfo.module.scss';
import BalanceInfoRow from './BalanceInfoRow';

type Props = {
    editable: boolean;
    givenCredit: number;
};
export default function GivenCreditRow({ editable, givenCredit }: Props) {
    const { t } = useTranslation();
    const { mobileView, customer, getCustomerInfo } = useGlobalContext();

    const reloadLeftPanel = (username: string) => {
        const reloadLeftPanelTimeOut = 1500;
        setTimeout(() => {
            const searchAccountNameText = $('#search-account-id').attr('data-search-account-username');
            if (username === searchAccountNameText) {
                getCustomerInfo?.(username, LoadCustomerInfo.ByUpdateGivenCredit);
            }
        }, reloadLeftPanelTimeOut);
    };

    const givenPopupOnclosing = (username = null, updateSuccessfully = false) => {
        if (updateSuccessfully === true) {
            const autoCloseGivenPopupTimeOut = 1000;
            reloadLeftPanel(username ?? '');

            setTimeout(() => {
                window.top?.updateGivenCreditPopup.close();
            }, autoCloseGivenPopupTimeOut);
        } else {
            window.top?.updateGivenCreditPopup.close();
        }
    };

    const handleOpenGivenCreditPopup = () => {
        const givenCreditUrl =
            `${top?.alpha.urls['accountInfoRootPath']}SearchAccount/GivenCredit` +
            `?SearchAccountId=${customer?.CustId}&SearchAccountName=${customer?.UserName}` +
            `&SearchAccountRoleId=${customer?.RoleId}`;
        let width = 470,
            height = 360;

        if (mobileView) {
            width = mobileScale(width);
            height = mobileScale(height);
        }

        if (window.top?.updateGivenCreditPopup === null || window.top?.updateGivenCreditPopup === undefined) {
            window.top!.updateGivenCreditPopup = window.top?.ageWnd.createInstance();
        }

        window.top!.updateGivenCreditPopup.onClosing = givenPopupOnclosing;
        window.top?.popupManager.openWithHanlder(
            top?.updateGivenCreditPopup,
            givenCreditUrl,
            `${t('UpdateGivenCredit')} - ${customer?.UserName}`,
            width,
            height
        );

        if (mobileView) {
            resizePopupOnMobile();
        }
    };

    if (editable) {
        return (
            <div className={classNames('row-info', styles['row-info'])}>
                <div className={classNames('col', styles['col'])}>{t('GivenCredit')}</div>
                <div className={classNames('col', styles['col'], 'col-right')} onClick={handleOpenGivenCreditPopup}>
                    <div className={styles['given-credit']}>
                        <span className={classNames('icon-border-color', styles['icon-edited'])} />
                        <NumericFormat
                            className={styles['number-link']}
                            value={givenCredit}
                            displayType="text"
                            decimalScale={2}
                            fixedDecimalScale
                            thousandSeparator
                        />
                    </div>
                </div>
            </div>
        );
    }

    return <BalanceInfoRow title={t('GivenCredit')} value={givenCredit} />;
}
