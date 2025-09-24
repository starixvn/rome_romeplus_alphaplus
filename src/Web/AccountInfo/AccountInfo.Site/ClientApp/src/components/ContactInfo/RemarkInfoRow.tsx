import { useGlobalContext } from "../../context/global-context";
import { mobileScale, resizePopupOnMobile } from '../../helpers';
import { useTranslation } from 'react-i18next';
import { Tooltip } from 'react-tippy';
import { IconEdit } from '../common/IconEdit';

export const RemarkInfo = ({ title, value }: { title: string; value: string }) => {
    const { t } = useTranslation();
    const { mobileView, customer, getCustomerInfo } = useGlobalContext();
    const maxLength = 15;
    const isTruncated = value?.length > maxLength;
    const content = () => <div>{isTruncated ? `${value.substring(0, maxLength)}...` : value}</div>;
    const contentWithTooltip = () => (
        <Tooltip {...{ title: value, position: 'bottom', trigger: 'mouseenter', arrow: true }}>{content()}</Tooltip>
    );

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
        const givenCreditUrl =
            `${top?.alpha.urls['accountInfoRootPath']}SearchAccount/RemarkInfo` +
            `?SearchAccountId=${customer?.CustId}&SearchAccountName=${customer?.UserName}` +
            `&SearchAccountRoleId=${customer?.RoleId}`;
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
            givenCreditUrl,
            `${t('UpdateRemark')} - ${customer?.UserName}`,
            width,
            height
        );

        if (mobileView) {
            resizePopupOnMobile();
        }
    };
    return (
        <div className="col-xs-12 row-info">
            <div className="col-xs-5">{title}</div>
            <div className="col-xs-7 col-right">
                <div style={{position: 'relative', float: 'right', paddingLeft: '24px'}}>
                    <IconEdit onClick={handleOpenRemarkPopup} />
                    {isTruncated ? contentWithTooltip() : content()}
                </div>
            </div>
        </div>
    );
}
