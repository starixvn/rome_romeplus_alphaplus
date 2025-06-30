import React from "react";
import { ButtonTabInactiveComponent, ButtonTabActiveComponent } from "../../../../components/Buttons";
import { TitleComponent } from "../../../../components/Title";
import { useTranslation } from "react-i18next";
import { SettingCashMemberBankInfo } from "./SettingCashMemberBankInfo";
import { SettingCashMemberInfo } from "./SettingCashMemberInfo";

export const SettingCashMemberWrapper = () => {
    const { t } = useTranslation('customerSites');
    const [activeTab, setActiveTab] = React.useState('memberInfo');
    const tabButton = (tab: string) => {
        const tabList: React.ReactNode[] = [];
        if (tab === 'memberInfo') {
            tabList.push(<ButtonTabInactiveComponent text={t('updateInfoCashMember.memberInfo')} />)
            tabList.push(<ButtonTabActiveComponent text={t('updateInfoCashMember.bankInfo')} onClick={() => setActiveTab('bankInfo')} />)
        }
        if (tab === 'bankInfo') {
            tabList.push(<ButtonTabActiveComponent text={t('updateInfoCashMember.memberInfo')} onClick={() => setActiveTab('memberInfo')} />)
            tabList.push(<ButtonTabInactiveComponent text={t('updateInfoCashMember.bankInfo')} />)
        }
        return tabList.map((item, index) => (
            <li key={`${tab}-${index}`} className="mr-[8px]">
                {item}
            </li>
        ));
    }

    return (
        <>
            <ul className="flex mt-[-12px] border-b border-[var(--border-color)] mb-[15px]">
                {tabButton(activeTab)}
            </ul>

            <div>
                {activeTab === 'memberInfo' && (
                    <SettingCashMemberInfo />
                )}
                {activeTab === 'bankInfo' && (
                    <SettingCashMemberBankInfo />
                )}
            </div>
        </>
    );
}