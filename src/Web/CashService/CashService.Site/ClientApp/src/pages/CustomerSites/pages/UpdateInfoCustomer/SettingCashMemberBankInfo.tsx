import React from "react";
import {
    ButtonFullComponent,
    ButtonOutlineComponent,
} from "../../../../components/Buttons";

import { useTranslation } from "react-i18next";

import NoRecordFound from "../../../../components/NoRecordFoundComponent";
import { MemberBankAccountRecord } from "../../types/UpdateCustomerType";
import { AddBankModal } from "./AddBankModal";



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
            await new Promise(resolve => setTimeout(resolve, 500));

            // Mock data - replace with actual API call
            let mockData: MemberBankAccountRecord[] = [
                {
                    id: '1',
                    memberBankAccountId: '1',
                    memberBankAccountNumber: '1234567890',
                    agentBankAccountNumber: '0987654321',
                }
            ];
            mockData = []
            setBankAccounts(mockData);
        } catch (error) {
            console.error('Error loading bank accounts:', error);
        } finally {
            setIsLoading(false);
        }
    };

    const handleDeleteBankAccount = async (id: string) => {
        if (window.confirm(t('updateInfoCashMember.confirmDelete') || 'Are you sure you want to delete this bank account?')) {
            try {
                setIsLoading(true);
                // TODO: Implement API call to delete bank account
                await new Promise(resolve => setTimeout(resolve, 500));

                setBankAccounts(prev => prev.filter(account => account.id !== id));
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
        const bankAccount: MemberBankAccountRecord = {
            id: Date.now().toString(),
            memberBankAccountId: newAccount.memberBankAccountId,
            memberBankAccountNumber: newAccount.memberBankAccountNumber,
            agentBankAccountNumber: newAccount.agentBankAccountNumber,
        };

        setBankAccounts(prev => [...prev, bankAccount]);
        setShowAddBankAccountPopup(false);
    };

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
                                <tr key={account.id}>
                                    <td>{index + 1}</td>
                                    <td>{account.memberBankAccountNumber}</td>
                                    <td>{account.agentBankAccountNumber}</td>
                                    <td>
                                        <ButtonOutlineComponent
                                            className="w-[80px] [&>*]:w-[100%] text-red-600 border-red-600 hover:bg-red-600 hover:text-white"
                                            text={t('global:delete') || 'Delete'}
                                            onClick={() => handleDeleteBankAccount(account.id)}
                                            disabled={isLoading}
                                        />
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


