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

declare global {
    const updateInfoCustomerPageConfig: {
        settingSiteUrl: string;
        isCashMember: boolean;
        username: string;
        custId: number;
    }
}
function PageTitle(){
    console.log(updateInfoCustomerPageConfig);
    const { t } = useTranslation('global');
    return (
        <TitleComponent title={`${t('global:info')} ${updateInfoCustomerPageConfig.username}`} showTippy={false} />
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