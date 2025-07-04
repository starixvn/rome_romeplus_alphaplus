import { useState, useEffect, useMemo, useCallback, memo } from "react";
import {
    ButtonFullComponent,
    ButtonPenComponent,
    ButtonDeleteComponent,
} from "../../../../components/Buttons";

import { useTranslation } from "react-i18next";
import CustomDropdown, { DropdownOption } from "../../../../components/CustomDropdownComponent";

import NoRecordFound from "../../../../components/NoRecordFoundComponent";
import { MemberBankAccountRecord } from "../../types/UpdateCustomerType";
import { AddBankModal } from "./AddBankModal";
import { GetCustomerBankInfo, RemoveMemberBankInfo, UpdateMemberBankInfo } from "../../services/CustomerInfoServices";
import { IconClose, IconTick } from "../../../../components/Icons";
import { useResultMessage } from "../../../../hooks/useResultMessage";

interface Bank {
    bankId: number;
    bankName: string;
    bankCode: string;
    bankAccountNumber: string;
}

interface EditingRow {
    id: number;
    memberBankID: number;
    memberBankAccountID: number;
    memberBankAccountNumber: string;
    agentBankID: number;
    agentBankAccountNumber: string;
    agentBankCode: string;
}

// Memoized bank cell component
const BankCell = memo(({ accountNumber, bankCode }: { accountNumber: string, bankCode: string }) => {
    if (!accountNumber || !bankCode) return <div className="text-left">No Matching Bank Account</div>;

    return (
        <div className="flex items-center">
            <img
                src={`${updateInfoCustomerPageConfig.cdnUri}cash-member/create/bank/${bankCode}.png`}
                alt={`${bankCode} bank icon`}
                className="w-[16px] h-[16px] inline-block mr-[5px]"
                loading="lazy"
            />
            {accountNumber}
        </div>
    );
});

BankCell.displayName = 'BankCell';

// Memoized table row component
const TableRow = memo(({
    account,
    index,
    isEditing,
    editingRow,
    agentBankOptions,
    onEdit,
    onDelete,
    onSave,
    onCancel,
    onAgentBankChange,
    isLoading
}: {
    account: MemberBankAccountRecord;
    index: number;
    isEditing: boolean;
    editingRow: EditingRow | null;
    agentBankOptions: DropdownOption[];
    onEdit: (account: MemberBankAccountRecord) => void;
    onDelete: (id: number) => void;
    onSave: () => void;
    onCancel: () => void;
    onAgentBankChange: (option: DropdownOption) => void;
    isLoading: boolean;
}) => {

    const renderAgentBankCell = () => {
        if (isEditing) {
            return (
                <CustomDropdown
                    options={agentBankOptions}
                    onSelect={onAgentBankChange}
                    initialValue={editingRow?.agentBankAccountNumber}
                    className="w-full"
                />
            );
        }

        return (
            <BankCell
                accountNumber={account.agentBankAccountNumber}
                bankCode={account.agentBankCode}
            />
        );
    };

    const renderActionCell = () => {
        if (isEditing) {
            return (
                <div className="flex justify-center gap-2">
                    <button
                        onClick={onSave}
                        className="p-1 hover:bg-gray-100 rounded"
                        disabled={isLoading}
                    >
                        <IconTick className="w-[20px] h-[20px] fill-[#52A220]! dark:fill-[#52A220]" />
                    </button>
                    <button
                        onClick={onCancel}
                        className="p-1 hover:bg-gray-100 rounded"
                        disabled={isLoading}
                    >
                        <IconClose className="w-[20px] h-[20px] fill-[#D33333] dark:fill-[#ededed]" />
                    </button>
                </div>
            );
        }

        return (
            <div className="flex justify-center">
                <ButtonPenComponent
                    onClick={() => onEdit(account)}
                    disabled={isLoading}
                    sizeIcon={{ width: 20, height: 20 }}
                />
                <ButtonDeleteComponent
                    onClick={() => onDelete(account.memberBankAccountID)}
                    disabled={isLoading}
                    sizeIcon={{ width: 16, height: 16 }}
                />
            </div>
        );
    };

    return (
        <tr key={account.memberBankAccountID}>
            <td>{index + 1}</td>
            <td>
                <BankCell
                    accountNumber={account.memberBankAccountNumber}
                    bankCode={account.memberBankCode}
                />
            </td>
            <td>{renderAgentBankCell()}</td>
            <td>{renderActionCell()}</td>
        </tr>
    );
});

TableRow.displayName = 'TableRow';

export const SettingCashMemberBankInfo = () => {
    const { t } = useTranslation('customerSites');
    const [showAddBankAccountPopup, setShowAddBankAccountPopup] = useState(false);
    const [bankAccounts, setBankAccounts] = useState<MemberBankAccountRecord[]>([]);
    const [isLoading, setIsLoading] = useState(false);
    const [editingRow, setEditingRow] = useState<EditingRow | null>(null);
    const [agentBankOptions, setAgentBankOptions] = useState<DropdownOption[]>([]);
    const [agentBanks, setAgentBanks] = useState<Bank[]>([]);
    const { ResultMessage, showResult } = useResultMessage();

    // Memoized values
    const editingRowId = useMemo(() => editingRow?.id, [editingRow?.id]);

    useEffect(() => {
        setIsLoading(true);
        const initializeData = async () => {
            await Promise.all([
                loadBankAccounts(),
                loadAgentBankOptions()
            ]);
        };
        initializeData().finally(() => {
            setIsLoading(false);
        });
    }, []);

    const loadBankAccounts = useCallback(async () => {
      
        const memberBanks = await GetCustomerBankInfo<MemberBankAccountRecord[]>({
            custId: updateInfoCustomerPageConfig.custId,
            roleId: 1
        });
        setBankAccounts(memberBanks);
    }, []);

    const loadAgentBankOptions = useCallback(async () => {
        const agentBanks = await GetCustomerBankInfo<Bank[]>({
            roleId: 2
        });
        setAgentBanks(agentBanks);

        const agentBankDropdownOptions: DropdownOption[] = agentBanks.map((bank) => ({
            uuid: Date.now().toString(),
            value: bank.bankId,
            label: bank.bankAccountNumber,
            icon: <img
                src={`${updateInfoCustomerPageConfig.cdnUri}cash-member/create/bank/${bank.bankCode?.toUpperCase()}.png`}
                alt={bank.bankName}
                className="w-[16px] h-[16px] inline-block mr-[5px]"
                loading="lazy"
            />
        }));
        setAgentBankOptions(agentBankDropdownOptions);
    }, []);

    const handleDeleteBankAccount = useCallback(async (id: number) => {
        if (window.confirm(t('updateInfoCashMember.confirmDelete') || 'Are you sure you want to delete this bank account?')) {
            try {
                setIsLoading(true);
                await RemoveMemberBankInfo({
                    custId: updateInfoCustomerPageConfig.custId,
                    memberBankAccountID: id,
                });
                setBankAccounts(prev => prev.filter(account => account.memberBankAccountID !== id));
                alert(t('updateInfoCashMember.deleteSuccess') || 'Bank account deleted successfully');
            } catch (error) {
                console.error('Error deleting bank account:', error);
                alert(t('updateInfoCashMember.deleteError') || 'Failed to delete bank account');
            } finally {
                setIsLoading(false);
            }
        }
    }, [t]);

    const handleEditBankAccount = useCallback((account: MemberBankAccountRecord) => {
        setEditingRow({
            id: account.memberBankAccountID,
            memberBankID: account.memberBankID,
            memberBankAccountID: account.memberBankAccountID,
            memberBankAccountNumber: account.memberBankAccountNumber,
            agentBankID: account.agentBankID,
            agentBankAccountNumber: account.agentBankAccountNumber,
            agentBankCode: account.agentBankCode,
        });
    }, []);

    const handleCancelEdit = useCallback(() => {
        setEditingRow(null);
    }, []);

    const handleSaveEdit = useCallback(async () => {
        if (!editingRow) return;

        try {
            setIsLoading(true);
            const response = await UpdateMemberBankInfo<any>({
                custId: updateInfoCustomerPageConfig.custId,
                memberBankID: editingRow.memberBankID,
                memberBankAccountID: editingRow.memberBankAccountID,
                memberBankAccountNumber: editingRow.memberBankAccountNumber,
                agentBankID: editingRow.agentBankID,
                agentBankAccountNumber: editingRow.agentBankAccountNumber,
                agentBankCode: editingRow.agentBankCode,
            });
            setBankAccounts(prev => prev.map(account =>
                account.memberBankAccountID === editingRow.id
                    ? {
                        ...account,
                        agentBankID: editingRow.agentBankID,
                        agentBankAccountNumber: editingRow.agentBankAccountNumber,
                        agentBankCode: editingRow.agentBankCode,
                    }
                    : account
            ));
            console.log('response', response);
            if (response.isSuccessful) {
                showResult('success', t('global:updateSuccess'));
            } else {
                showResult('error', response.message);
            }
           
        } catch (error) {
            console.error('Error updating bank account:', error);
            showResult('error', t('updateInfoCashMember.updateError') || 'Failed to update bank account');
        } finally {
            setIsLoading(false);
            setEditingRow(null);
        }
    }, [editingRow, t]);

    const handleAgentBankChange = useCallback((option: DropdownOption) => {
        if (!editingRow) return;
        const selectedBank = agentBanks.find(bank => bank.bankAccountNumber === option.label && bank.bankId === option.value);
        if (selectedBank) {
            setEditingRow({
                ...editingRow,
                agentBankID: selectedBank.bankId,
                agentBankAccountNumber: selectedBank.bankAccountNumber,
                agentBankCode: selectedBank.bankCode,
            });
        }
    }, [editingRow, agentBanks]);

    const handleAddBankSuccess = useCallback(() => {
        setShowAddBankAccountPopup(false);
        loadBankAccounts();
    }, [loadBankAccounts]);

    const handleCloseModal = useCallback(() => {
        setShowAddBankAccountPopup(false);
    }, []);

    const handleAddBankClick = useCallback(() => {
        setShowAddBankAccountPopup(true);
    }, []);

    // Memoized table rows
    const tableRows = useMemo(() => {
        return bankAccounts.map((account, index) => {
            const isEditing = editingRowId === account.memberBankAccountID;

            return (
                <TableRow
                    key={account.memberBankAccountID}
                    account={account}
                    index={index}
                    isEditing={isEditing}
                    editingRow={editingRow}
                    agentBankOptions={agentBankOptions}
                    onEdit={handleEditBankAccount}
                    onDelete={handleDeleteBankAccount}
                    onSave={handleSaveEdit}
                    onCancel={handleCancelEdit}
                    onAgentBankChange={handleAgentBankChange}
                    isLoading={isLoading}
                />
            );
        });
    }, [
        bankAccounts,
        editingRowId,
        editingRow,
        agentBankOptions,
        handleEditBankAccount,
        handleDeleteBankAccount,
        handleSaveEdit,
        handleCancelEdit,
        handleAgentBankChange,
        isLoading
    ]);

    return (
        <>
            <div className="flex gap-[8px] mb-[10px]">
                <ButtonFullComponent
                    className="w-[150px] [&>*]:w-[100%]"
                    text={t('updateInfoCashMember.addBankAccount')}
                    onClick={handleAddBankClick}
                    disabled={isLoading}
                />
            </div>
            <div className="table">
                <div className="w-[50%] mb-[10px]">
                    {ResultMessage()}
                </div>
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
                        {tableRows}
                    </tbody>
                    {!isLoading && bankAccounts.length === 0 && (
                        <tbody>
                            <NoRecordFound colSpan={4} />
                        </tbody>
                    )}
                </table>
            </div>
            {showAddBankAccountPopup && (
                <AddBankModal
                    closeModal={handleCloseModal}
                    onAddSuccess={handleAddBankSuccess}
                />
            )}
        </>
    );
};


