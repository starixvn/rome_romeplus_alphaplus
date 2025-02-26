import classNames from 'classnames';
import React from 'react';

import { LoadCustomerAllStatus, LoadCustomerInfo } from '../../constants';
import { useGlobalContext } from '../../context/global-context';
import { formatMessage, resizeBootboxOnMobile, resizePopupOnMobile, usernameRegExp } from '../../helpers';
import StatusItem from '../../models/status-item';
import { handleError } from '../../services/axios-helper';
import SettingsService from '../../services/settings-service';

type Props = {
    status: StatusItem;
    getCustomerStatus: (requestId: string | undefined) => void;
};

export default function ConfirmStatus({ status, getCustomerStatus }: Props) {
    const { mobileView, username, getCustomerInfo } = useGlobalContext();

    const updateStatus = () => {
        $.blockUI?.();
        SettingsService.UpdateCustomerStatus(status)
            .then((data) => {
                if (data.IsSuccessful) {
                    setTimeout(() => {
                        if (status.ReloadCustomerInfo) {
                            getCustomerInfo?.(username ?? '', LoadCustomerInfo.ByUpdateStatus);
                        }

                        getCustomerStatus?.(LoadCustomerAllStatus.ByUpdateStatus);
                    }, 1500);
                } else {
                    reloadLeftPanel(username ?? '');
                    window.top?.bootboxAlert(data.Message, mobileView ? null : $(`#confirm-${status.DisplayOrder}`)[0]);
                }
            })
            .catch(handleError);
    };

    const reloadLeftPanel = (username: string) => {
        const reloadLeftPanelTimeOut = 1500;
        setTimeout(() => {
            const searchAccountNameText = $('#search-account-id').attr('data-search-account-username');
            if (username === searchAccountNameText) {
                getCustomerInfo?.(username, LoadCustomerInfo.ByUpdateStatus);
                getCustomerStatus?.(LoadCustomerAllStatus.ByUpdateStatus);
            }
        }, reloadLeftPanelTimeOut);
    };

    const confirmPopupOnclosing = (username = '', updateSuccessfully = false) => {
        if (updateSuccessfully === true) {
            const autoClosePopupTimeOut = 1000;
            reloadLeftPanel(username);

            setTimeout(() => {
                window.top?.confirmPopupPopup.close();
            }, autoClosePopupTimeOut);
        } else {
            window.top?.confirmPopupPopup.close();
        }
    };

    const openPopup = () => {
        if (window.top?.confirmPopupPopup === null || window.top?.confirmPopupPopup === undefined) {
            if (window.top) {
                window.top.confirmPopupPopup = window.top?.ageWnd.createInstance();
            }
        }

        const headerPopupText = usernameRegExp(status.PopupHeaderText, username ?? '');

        window.top!.confirmPopupPopup.onClosing = confirmPopupOnclosing;
        window.top?.popupManager.openWithHanlder(top?.confirmPopupPopup, status.Url, headerPopupText, 700, 500);

        if (mobileView) {
            resizePopupOnMobile();
        }
    };

    const openBootboxConfirm = (e: React.MouseEvent<HTMLElement>) => {
        window.top?.bootboxConfirm(formatMessage(status.ConfirmMessage), e.target, updateStatus);

        if (mobileView) {
            resizeBootboxOnMobile();
        }
    };

    const handleOpenPopup = (e: React.MouseEvent<HTMLElement>) => {
        if (!status.Disabled) {
            status.IsPopup ? openPopup() : openBootboxConfirm(e);
        }
    };

    return (
        <div
            id={`confirm-${status.DisplayOrder}`}
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
