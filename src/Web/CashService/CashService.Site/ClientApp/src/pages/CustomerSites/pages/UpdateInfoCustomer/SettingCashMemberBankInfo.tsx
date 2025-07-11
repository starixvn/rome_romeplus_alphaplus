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
import { IconClose, IconInfo, IconTick } from "../../../../components/Icons";
import { useResultMessage } from "../../../../hooks/useResultMessage";
import ConfirmPopup from "../../../../components/ConfirmPopup";
import Tippy from "@tippyjs/react";

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
    const { t } = useTranslation('customerSites');
    if (!accountNumber || !bankCode) return <div className="text-left px-[8px]">{t('updateInfoCashMember.noMatchingBank')}</div>;

    return (
        <div className="flex items-center px-[8px]">
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
                    initialValue={editingRow?.agentBankID + '|' + editingRow?.agentBankAccountNumber}
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
            <td className="w-[300px]">
                {renderAgentBankCell()}
            </td>
            <td>{renderActionCell()}</td>
        </tr>
    );
});

TableRow.displayName = 'TableRow';

export const SettingCashMemberBankInfo = () => {
    const { t } = useTranslation('customerSites');
    const [showAddBankAccountPopup, setShowAddBankAccountPopup] = useState(false);
    const [gridData, setGridData] = useState<MemberBankAccountRecord[]>([]);
    const [isLoading, setIsLoading] = useState(false);
    const [editingRow, setEditingRow] = useState<EditingRow | null>(null);
    const [agentBankOptions, setAgentBankOptions] = useState<DropdownOption[]>([]);
    const [agentBanks, setAgentBanks] = useState<Bank[]>([]);
    const [showDeleteConfirm, setShowDeleteConfirm] = useState(false);
    const [deleteAccountId, setDeleteAccountId] = useState<number | null>(null);
    const { ResultMessage, showResult } = useResultMessage();

    // Memoized values
    const editingRowId = useMemo(() => editingRow?.id, [editingRow?.id]);

    useEffect(() => {
        setIsLoading(true);
        const initializeData = async () => {
            await Promise.all([
                loadGridData(),
                loadAgentBankOptions()
            ]);
        };
        initializeData().finally(() => {
            setIsLoading(false);
        });
    }, []);

    const loadGridData = useCallback(async () => {
        const gridData = await GetCustomerBankInfo<MemberBankAccountRecord[]>({
            custId: updateInfoCustomerPageConfig.custId,
            roleId: 1,
        });

        setGridData(gridData);
    }, []);

    const loadAgentBankOptions = useCallback(async () => {
        const noMachingBank = {
            bankId: 0,
            bankName: '',
            bankCode: '',
            bankAccountNumber: '',
        }
        const agentBanks = await GetCustomerBankInfo<Bank[]>({
            roleId: 2
        });
        const agentBanksWithNoMachingBank = [noMachingBank, ...agentBanks];
        setAgentBanks(agentBanksWithNoMachingBank);

        const agentBankDropdownOptions: DropdownOption[] = [{
            value: noMachingBank.bankId + '|' + noMachingBank.bankAccountNumber,
            label: t('updateInfoCashMember.noMatchingBank'),
            icon: null
        }, ...agentBanks.map((bank) => ({
            value: bank.bankId + '|' + bank.bankAccountNumber,
            label: bank.bankAccountNumber,
            icon: <img
                src={`${updateInfoCustomerPageConfig.cdnUri}cash-member/create/bank/${bank.bankCode?.toUpperCase()}.png`}
                alt={bank.bankName}
                className="w-[16px] h-[16px] inline-block mr-[5px]"
                loading="lazy"
            />
        }))];

        setAgentBankOptions(agentBankDropdownOptions);
    }, []);

    const handleDeleteBankAccount = useCallback(async (id: number) => {
        setEditingRow(null);
        setDeleteAccountId(id);
        setShowDeleteConfirm(true);
    }, []);

    const handleConfirmDelete = useCallback(async () => {
        if (!deleteAccountId) return;

        try {
            setIsLoading(true);
            const response = await RemoveMemberBankInfo<any>({
                custId: updateInfoCustomerPageConfig.custId,
                memberBankAccountID: deleteAccountId,
            });
            setGridData(prev => prev.filter(account => account.memberBankAccountID !== deleteAccountId));
            if (response.isSuccessful) {
                showResult('success', t('global:removeSuccess') || 'Bank account removed successfully');
            } else {
                showResult('error', response.message);
            }
        } catch (error) {
            console.error('Error deleting bank account:', error);
            showResult('error', t('global:removeError') || 'Failed to remove bank account');
            loadGridData();
        } finally {
            setIsLoading(false);
            setShowDeleteConfirm(false);
            setDeleteAccountId(null);
        }
    }, [deleteAccountId, t]);

    const handleCancelDelete = useCallback(() => {
        setShowDeleteConfirm(false);
        setDeleteAccountId(null);
    }, []);

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
            setGridData(prev => prev.map(account =>
                account.memberBankAccountID === editingRow.id
                    ? {
                        ...account,
                        agentBankID: editingRow.agentBankID,
                        agentBankAccountNumber: editingRow.agentBankAccountNumber,
                        agentBankCode: editingRow.agentBankCode,
                    }
                    : account
            ));
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
        const selectedBank = agentBanks.find(bank => (bank.bankId + '|' + bank.bankAccountNumber) === option.value);
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
        loadGridData();
    }, [loadGridData]);

    const handleCloseModal = useCallback(() => {
        setShowAddBankAccountPopup(false);
    }, []);

    const handleAddBankClick = useCallback(() => {
        setEditingRow(null);
        setShowAddBankAccountPopup(true);
    }, []);

    // Memoized table rows
    const tableRows = useMemo(() => {
        return gridData.map((account, index) => {
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
        gridData,
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
                <div className="w-[70%] mb-[10px]">
                    {ResultMessage()}
                </div>
                <table className="w-[70%] border-collapse no-hover">
                    <thead>
                        <tr>
                            <th scope="col">#</th>
                            <th scope="col">{t('updateInfoCashMember.memberBankAccount')}</th>
                            <th scope="col">{
                                <div className="flex items-center justify-center">
                                    {t('updateInfoCashMember.agentBankAccount')}
                                    <Tippy content={<div>
                                        <div>{t('updateInfoCashMember.agentBankAccountTooltip')}</div>
                                        <div>{t('updateInfoCashMember.agentBankAccountTooltip2')}</div>
                                    </div>}
                                        placement="top"
                                        theme="light" arrow={true}>
                                        <span>
                                            <IconInfo className="w-[18px] cursor-pointer h-[18px] ml-[10px] fill-[#118AF2]" />
                                        </span>
                                    </Tippy>
                                </div>
                            }
                            </th>
                            <th scope="col">{t('updateInfoCashMember.action')}</th>
                        </tr>
                    </thead>
                    <tbody>
                        {tableRows}
                    </tbody>
                    {!isLoading && gridData.length === 0 && (
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
            {showDeleteConfirm && (
                <ConfirmPopup
                    title={t('updateInfoCashMember.deleteBankAccount').toLocaleUpperCase()}
                    message={
                        <>
                            <div className="text-center">
                                {t('updateInfoCashMember.areYouSureYouWantToDeleteThisBankAccount', {
                                    bankNumber: gridData.find(record => record.memberBankAccountID === deleteAccountId)?.memberBankAccountNumber
                                })}
                            </div>
                            <div className="text-center text-[12px]">
                                *{t('updateInfoCashMember.thisActionIsPermanentAndCannotBeUndone')}
                            </div>
                        </>
                    }
                    onCancel={handleCancelDelete}
                    onConfirm={handleConfirmDelete}
                />
            )}
        </>
    );
};


