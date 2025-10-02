import { useGlobalContext } from "../../context/global-context";
import { mobileScale, resizePopupOnMobile } from '../../helpers';
import { useTranslation } from 'react-i18next';
import { IconEdit } from '../common/IconEdit';
import type { ContactInfoRowProps } from './ContactInfoType';


export const RemarkInfo = ({ title, value, key }: ContactInfoRowProps) => {
    const { t } = useTranslation();
    const { mobileView, customer, getCustomerInfo } = useGlobalContext();

    const reloadLeftPanel = (username: string) => {
        const reloadLeftPanelTimeOut = 2000;
        setTimeout(() => {
            const searchAccountNameText = $('#search-account-id').attr('data-search-account-username');
            if (username === searchAccountNameText) {
                getCustomerInfo?.(username);
            }
        }, reloadLeftPanelTimeOut);
    };

    const remarkPopupOnclosing = (username = null, updateSuccessfully = false) => {
        if (updateSuccessfully === true) {
            const autoCloseRemarkPopupTimeOut = 1500;
            reloadLeftPanel(username ?? '');

            setTimeout(() => {
                window.top?.updateRemarkPopup.close();
            }, autoCloseRemarkPopupTimeOut);
        } else {
            window.top?.updateRemarkPopup.close();
        }
    };

    const handleOpenRemarkPopup = () => {
        const remarkInfoUrl =
            `${top?.alpha.urls['accountInfoRootPath']}SearchAccount/RemarkInfo` +
            `?SearchAccountId=${customer?.CustId}&SearchAccountName=${customer?.UserName}` +
            `&SearchAccountRoleId=${customer?.RoleId}` +
            `&isShowCancelButton=true`;
        let width = 470,
            height = 360;
        
        if (mobileView) {
            width = mobileScale(width);
            height = mobileScale(height);
        }

        if ((window.top?.updateRemarkPopup === null || window.top?.updateRemarkPopup === undefined) && window.top?.ageWnd) {
            window.top.updateRemarkPopup = window.top.ageWnd.createInstance();
        }

        if (window.top?.updateRemarkPopup) {
            window.top.updateRemarkPopup.onClosing = remarkPopupOnclosing;
        }
        window.top?.popupManager.openWithHanlder(
            top?.updateRemarkPopup,
            remarkInfoUrl,
            `${t('UpdateRemark')} - ${customer?.UserName}`,
            width,
            height
        );

        if (mobileView) {
            resizePopupOnMobile();
        }
    };
    return (
        <div className="col-xs-12 row-info" key={key}>
            <div className="col-xs-5">{title}</div>
            <div className="col-xs-7 col-right">
                <div style={{position: 'relative', float: 'right', paddingLeft: '24px'}}>
                    <IconEdit onClick={handleOpenRemarkPopup} />
                    <div style={{ whiteSpace: 'nowrap' }}>
                        {value}
                    </div>
                </div>
            </div>
        </div>
    );
}
