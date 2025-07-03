import React from "react";
import {
    ButtonFullComponent,
    ButtonPenComponent,
    ButtonDeleteComponent,
} from "../../../../components/Buttons";

import { useTranslation } from "react-i18next";

import NoRecordFound from "../../../../components/NoRecordFoundComponent";
import { MemberBankAccountRecord } from "../../types/UpdateCustomerType";
import { AddBankModal } from "./AddBankModal";
import { GetCustomerBankInfo } from "../../services/CustomerInfoServices";



export const SettingCashMemberBankInfo = () => {
    const { t } = useTranslation('customerSites');
    const [showAddBankAccountPopup, setShowAddBankAccountPopup] = React.useState(false);
    const [bankAccounts, setBankAccounts] = React.useState<MemberBankAccountRecord[]>([]);
    const [isLoading, setIsLoading] = React.useState(false);

    // Load bank accounts on component mount
    React.useEffect(() => {
        loadBankAccounts();
    }, []);

    const loadBankAccounts = async () => {
        try {
            setIsLoading(true);
            // TODO: Implement API call to load bank accounts
            // Simulate API call
            const memberBanks = await GetCustomerBankInfo<MemberBankAccountRecord[]>({
                custId: updateInfoCustomerPageConfig.custId,
                roleId: 1
            });
            console.log('memberBanks', memberBanks);
            await new Promise(resolve => setTimeout(resolve, 500));

            // Mock data - replace with actual API call

            setBankAccounts(memberBanks);
        } catch (error) {
            console.error('Error loading bank accounts:', error);
        } finally {
            setIsLoading(false);
        }
    };

    const handleDeleteBankAccount = async (id: number) => {
        if (window.confirm(t('updateInfoCashMember.confirmDelete') || 'Are you sure you want to delete this bank account?')) {
            try {
                setIsLoading(true);
                // TODO: Implement API call to delete bank account
                await new Promise(resolve => setTimeout(resolve, 500));

                setBankAccounts(prev => prev.filter(account => account.memberBankAccountID !== id));
                alert(t('updateInfoCashMember.deleteSuccess') || 'Bank account deleted successfully');
            } catch (error) {
                console.error('Error deleting bank account:', error);
                alert(t('updateInfoCashMember.deleteError') || 'Failed to delete bank account');
            } finally {
                setIsLoading(false);
            }
        }
    };

    const handleAddBankAccount = (newAccount: MemberBankAccountRecord) => {
        console.log('newAccount', newAccount);
        // const bankAccount: MemberBankAccountRecord = {
        //     agentBankAccountID: newAccount.agentBankAccountID,
        //     agentBankAccountNumber: newAccount.agentBankAccountNumber,
        //     agentBankAccountStatus: newAccount.agentBankAccountStatus,
        //     agentBankCode: newAccount.agentBankCode,
        //     agentBankID: newAccount.agentBankID,
        //     memberBankAccountID: newAccount.memberBankAccountID,
        //     memberBankAccountNumber: newAccount.memberBankAccountNumber,
        // };

        // setBankAccounts(prev => [...prev, bankAccount]);
        // setShowAddBankAccountPopup(false);
    };

    const handleEditBankAccount = (id: number) => {
        console.log('edit bank account', id);
    };

    const renderCell = ({ accountNumber, bankCode }: { accountNumber: string, bankCode: string }) => {
        if (!accountNumber || !bankCode) return "No Matching Bank Account"
        return (
            <>
                <img
                    src={`https://cdn-customer.nexdev.net/v5/cash-member/create/bank/${bankCode}.png`}
                    alt={`${bankCode} bank icon`}
                    className={`w-[16px] h-[16px] inline-block mr-[5px]`}
                />
                {accountNumber}
            </>
        )
    }

    return (
        <>
            <div className="flex gap-[8px] mb-[10px]">
                <ButtonFullComponent
                    className="w-[150px] [&>*]:w-[100%]"
                    text={t('updateInfoCashMember.addBankAccount')}
                    onClick={() => setShowAddBankAccountPopup(true)}
                    disabled={isLoading}
                />

            </div>
            <div className="table">
                <table className="w-[50%] border-collapse no-hover">
                    <thead>
                        <tr>
                            <th scope="col">#</th>
                            <th scope="col">{t('updateInfoCashMember.memberBankAccount')}</th>
                            <th scope="col">{t('updateInfoCashMember.agentBankAccount')}</th>

                            <th scope="col">{t('updateInfoCashMember.action')}</th>
                        </tr>
                    </thead>
                    <tbody>
                        {bankAccounts.length > 0 && (
                            bankAccounts.map((account, index) => (
                                <tr key={account.memberBankAccountID}>
                                    <td>{index + 1}</td>
                                    <td>
                                        {renderCell({ accountNumber: account.memberBankAccountNumber, bankCode: account.memberBankCode })}
                                    </td>
                                    <td>
                                        {renderCell({ accountNumber: account.agentBankAccountNumber, bankCode: account.agentBankCode })}
                                    </td>
                                    <td>
                                        <div className="flex justify-center">
                                            <ButtonPenComponent
                                                onClick={() => handleEditBankAccount(account.memberBankAccountID)}
                                                disabled={isLoading}
                                                sizeIcon={{ width: 20, height: 20 }}
                                            />
                                            <ButtonDeleteComponent
                                                onClick={() => handleDeleteBankAccount(account.memberBankAccountID)}
                                                disabled={isLoading}
                                                sizeIcon={{ width: 16, height: 16 }}
                                            />
                                        </div>
                                    </td>
                                </tr>
                            ))
                        )}
                    </tbody>
                    {!isLoading && bankAccounts.length === 0 && (
                        <tbody>
                            <NoRecordFound colSpan={8} />
                        </tbody>
                    )}
                </table>
            </div>
            {showAddBankAccountPopup && (
                <AddBankModal
                    onClose={() => setShowAddBankAccountPopup(false)}
                    onAdd={handleAddBankAccount}
                />
            )}
        </>
    )
}


