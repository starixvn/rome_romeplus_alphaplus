import classNames from 'classnames';
import { useTranslation } from 'react-i18next';

import { LoadCustomerAllStatus } from '../../constants';
import { useGlobalContext } from '../../context/global-context';
import { mobileScale, resizePopupOnMobile } from '../../helpers';
import StatusItem from '../../models/status-item';

export default function ProductLevelStatus({
    status,
    getCustomerStatus
}: {
    status: StatusItem;
    getCustomerStatus: (requestId: string | undefined) => void;
}) {
    const { mobileView, username } = useGlobalContext();
    const { t } = useTranslation();

    const reloadLeftPanel = (username?: string) => {
        const reloadLeftPanelTimeOut = 1500;
        setTimeout(() => {
            const searchAccountNameText = $('#search-account-id').attr('data-search-account-username');
            if (username === searchAccountNameText) {
                getCustomerStatus(LoadCustomerAllStatus.ByUpdateStatus);
            }
        }, reloadLeftPanelTimeOut);
    };

    const productStatusOnclosing = (username = undefined, updateSuccessfully = false) => {
        if (updateSuccessfully === true) {
            const autoPopupTimeOut = 1000;
            reloadLeftPanel(username);

            setTimeout(() => {
                window.top?.updateProductStatusPopup.close();
            }, autoPopupTimeOut);
        } else {
            window.top?.updateProductStatusPopup.close();
        }
    };

    const handleOpenPopup = () => {
        if (!status.Disabled) {
            if (!window.top?.updateProductStatusPopup) {
                window.top!.updateProductStatusPopup = window.top?.ageWnd.createInstance();
            }

            const popupTitle = `${t('Update')} ${status.Text} ${t('Status')} - ${username}`;
            const width = 475,
                height = 400;
            window.top!.updateProductStatusPopup.onClosing = productStatusOnclosing;
            window.top?.popupManager.openWithHanlder(
                top?.updateProductStatusPopup,
                status.Url,
                popupTitle,
                width,
                mobileView ? mobileScale(height) : height
            );

            if (mobileView) {
                resizePopupOnMobile();
            }
        }
    };

    return (
        <div
            id={`product-${status.Text}`}
            className={classNames('col-xs-6', 'checkbox', { ['disabled']: status.Disabled })}
            onClick={handleOpenPopup}
        >
            <input type="checkbox" checked={status.Checked} />
            <label className="label-text" htmlFor={status.DisplayOrder}>
                {status.Text}
            </label>
        </div>
    );
}
