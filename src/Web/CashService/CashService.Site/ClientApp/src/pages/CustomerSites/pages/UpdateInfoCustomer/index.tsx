import { createRoot } from "react-dom/client";
import { useCookieLanguage } from "../../../../hooks/useCookieLanguage";
import i18n from '../../../../i18n/i18n-customerSites';
import '../../../../styles/common.css';
import '../../../../styles/global.scss';
import React from "react";
import { SettingCashMemberWrapper } from "./SettingCashMemberWrapper";
import { SettingCustomerInfoWrapper } from "./SettingCustomerInfoWrapper";
import { CustomerInfoProvider } from "../../context/CustomerInfoContext";

declare global {
    const updateInfoCustomerPageConfig: {
        settingSiteUrl: string;
        isCashMember: boolean;
        custId: number;
    }
}

function UpdateInfoCustomerContent() {
    useCookieLanguage(i18n);

    if (updateInfoCustomerPageConfig.isCashMember) {
        return <SettingCashMemberWrapper />
    }
    return <SettingCustomerInfoWrapper />
}

export default function UpdateInfoCustomer() {
    return (
        <CustomerInfoProvider custId={updateInfoCustomerPageConfig.custId} isCashMember={updateInfoCustomerPageConfig.isCashMember}>
            <UpdateInfoCustomerContent />
        </CustomerInfoProvider>
    );
}

createRoot(document.getElementById('root')!).render(
    <React.StrictMode>
        <UpdateInfoCustomer />
    </React.StrictMode>
)