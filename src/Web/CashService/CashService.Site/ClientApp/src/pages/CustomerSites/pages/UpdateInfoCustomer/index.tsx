import React from "react";
import { createRoot } from "react-dom/client";
import { useCookieLanguage } from "../../../../hooks/useCookieLanguage";
import i18n from '../../../../i18n/i18n-customerSites';
import '../../../../styles/common.css';
import '../../../../styles/global.scss';
import { SettingCashMemberWrapper } from "./SettingCashMemberWrapper";
import { SettingCustomerInfoWrapper } from "./SettingCustomerInfoWrapper";
import { TitleComponent } from "../../../../components/Title";
import { useTranslation } from "react-i18next";


function PageTitle(){
    const { t } = useTranslation('global');
    let popup = window.parent.document.querySelector('#product-popup');
    if (!popup) {
        popup = window?.top?.document.querySelector('div[id^="page-popup-"]') as HTMLElement;
    }

    const isOpenInPopup = popup !== null;
    if (isOpenInPopup) {
        return null;
    }
    return (
        <TitleComponent title={`${t('global:info')} ${updateInfoCustomerPageConfig.username}`} />
    )
}

function UpdateInfoCustomerContent() {
    useCookieLanguage(i18n);

    if (updateInfoCustomerPageConfig.isCashMember) {
        return <SettingCashMemberWrapper />
    }
    return <SettingCustomerInfoWrapper />
}


createRoot(document.getElementById('root')!).render(
    <React.StrictMode>
        <PageTitle />
        <UpdateInfoCustomerContent />
    </React.StrictMode>
)