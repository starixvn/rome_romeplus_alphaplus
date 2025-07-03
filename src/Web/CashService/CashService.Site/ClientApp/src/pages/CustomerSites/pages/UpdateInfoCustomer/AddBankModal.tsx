import { useState, useEffect } from "react";
import { useTranslation } from "react-i18next";
import CustomDropdown, { DropdownOption } from '../../../../components/CustomDropdownComponent';
import { ButtonClosePopupComponent, ButtonCancelComponent, ButtonApplyComponent } from "../../../../components/Buttons";
import { MediumTitleComponent, LabelTitleComponent } from "../../../../components/Title";
import { FormProvider, useForm, Controller } from 'react-hook-form';
import { AddMemberBankInfo, GetBanks, GetCustomerBankInfo } from "../../services/CustomerInfoServices";
import { useResultMessage } from "../../../../hooks/useResultMessage";
import { ValidateMessageComponent } from "../../../../components/Message";


interface AddBankModalProps {
    closeModal: () => void;
    onAddSuccess: () => void;
}

interface Bank {
    bankId: number;
    bankName: string;
    bankCode: string;
    bankAccountNumber: string;
}

interface AddMemberBankFormData {
    memberId: number;
    memberBankID: number;
    agentBankID: number;
    memberBankAccountNumber: string;
    agentBankAccountNumber: string;
    memberBankDropdownOptions: DropdownOption[];
    agentBankDropdownOptions: DropdownOption[];
    agentBanks: any[];
}
export const AddBankModal = ({ closeModal, onAddSuccess }: AddBankModalProps) => {
    const { t } = useTranslation('customerSites');
    const [isLoading, setIsLoading] = useState(false);
    const [isBlockAction, setIsBlockAction] = useState(false);
    const { ResultMessage, showResult, clearResult } = useResultMessage();

    useEffect(() => {
        loadModalData();
    }, []);

    const loadModalData = async () => {
        const memberBanks = await GetBanks<Bank[]>({});
        const memberBankDropdownOptions: DropdownOption[] = memberBanks.map((bank) => ({
            value: bank.bankId,
            label: bank.bankName,
            icon: <img src={`https://cdn-customer.nexdev.net/v5/cash-member/create/bank/${bank.bankCode?.toUpperCase()}.png`} alt={bank.name} className="w-[16px] h-[16px] inline-block mr-[5px]" />
        }));

        const agentBanks = await GetCustomerBankInfo<Bank[]>({
            roleId: 2
        });
        const agentBankDropdownOptions: DropdownOption[] = agentBanks.map((bank) => ({
            value: bank.bankId,
            label: bank.bankAccountNumber,
            icon: <img src={`https://cdn-customer.nexdev.net/v5/cash-member/create/bank/${bank.bankCode?.toUpperCase()}.png`} alt={bank.name} className="w-[16px] h-[16px] inline-block mr-[5px]" />
        }));
        console.log('agentBanks', agentBanks);
        reset({
            memberId: updateInfoCustomerPageConfig.custId,
            memberBankID: memberBanks[0].bankId,
            memberBankDropdownOptions: memberBankDropdownOptions,
            agentBankID: agentBanks[0].bankId,
            agentBankDropdownOptions: agentBankDropdownOptions,
            agentBanks: agentBanks,
        });

    }

    const methods = useForm<AddMemberBankFormData>({
        mode: 'onChange',
        criteriaMode: 'all',
    });

    const {
        handleSubmit,
        control,
        watch,
        formState: { isDirty, errors },
        reset,
        setValue,
    } = methods;

    const onSubmit = async (formData: AddMemberBankFormData) => {
        try {
            setIsLoading(true);
            setIsBlockAction(true);
            const response = await AddMemberBankInfo<any>({
                MemberId: formData.memberId,
                MemberBankId: formData.memberBankID,
                MemberBankAccountNumber: formData.memberBankAccountNumber,
                AgentBankId: formData.agentBankID,
                AgentBankAccountNumber: formData.agentBankAccountNumber,
            });

            if (response.isSuccessful) {
                showResult('success', t('global:addSuccess'), 3000, onAddSuccess);
            } else {
                showResult('error', response.message);
            }

        } catch (error) {
            console.error('Error adding bank account:', error);
            showResult('error', t('global:addFailed'));
        } finally {
            setIsLoading(false);
            setIsBlockAction(false);
        }
    };

    return (
        <div className="bg-[#00000080] fixed z-[100] top-0 left-0 right-0 bottom-0 flex items-start justify-center">
            <div className="p-[12px] pt-[0] bg-[var(--modal-background)] border-[1px] border-[var(--modal-border-color)] shadow-[var(--modal-box-shadow)] mt-[100px] min-w-[350px] max-[768px]:max-w-[90%] rounded-[3px] dark:bg-[#121212]">
                <div className="relative mb-[10px] text-center border-b-[1px] border-[var(--border-color)] py-[11px]" >
                    <MediumTitleComponent text={t('updateInfoCashMember.addBankAccount')} />
                    <ButtonClosePopupComponent onClick={closeModal} />
                </div>
                <div>
                    <div className="mb-[10px]">
                        {ResultMessage()}
                    </div>
                    <FormProvider {...methods}>
                        <form onSubmit={handleSubmit(onSubmit)} className="space-y-[15px]">
                            <div>
                                <LabelTitleComponent text={t('updateInfoCashMember.memberBankAccount')} required={true} />
                                <CustomDropdown
                                    options={watch('memberBankDropdownOptions') ?? []}
                                    onSelect={(option) => {
                                        setValue('memberBankID', Number(option.value));
                                    }}
                                    initialValue={watch('memberBankDropdownOptions')?.[0]?.value}
                                />
                                {errors.memberBankID && (
                                    <div className="">{errors.memberBankID.message}</div>
                                )}
                            </div>

                            <div>
                                <LabelTitleComponent text={t('updateInfoCashMember.memberBankAccountNumber')} required={true} />
                                <Controller
                                    name="memberBankAccountNumber"
                                    control={control}
                                    rules={{ required: t('updateInfoCashMember.accountNumberRequired') || 'Account number is required' }}
                                    render={({ field }) => (
                                        <input
                                            {...field}
                                            type="text"
                                            className={`w-full border border-gray-300 rounded px-2 py-1 focus:outline-none ${errors.memberBankAccountNumber ? 'border-red-500' : ''}`}
                                        />
                                    )}
                                />
                                <ValidateMessageComponent message={errors.memberBankAccountNumber?.message as string} />
                            </div>
                            <div>
                                <LabelTitleComponent text={t('updateInfoCashMember.agentBankAccount')} />
                                <CustomDropdown
                                    options={watch('agentBankDropdownOptions') ?? []}
                                    onSelect={(option) => {
                                        const selectedBank = watch('agentBanks')?.find((opt) => opt.bankId === option.value);
                                        console.log('selectedBank', selectedBank);
                                        setValue('agentBankID', selectedBank?.bankId ?? 0);
                                        setValue('agentBankAccountNumber', selectedBank?.bankAccountNumber ?? '');
                                    }}
                                    initialValue={watch('agentBankDropdownOptions')?.[0]?.value}
                                />
                            </div>

                            <div className="flex justify-center gap-[8px] pt-4">
                                <ButtonCancelComponent
                                    text={t('global:cancel')}
                                    onClick={() => {
                                        closeModal();
                                        clearResult();
                                    }}
                                    disabled={isLoading || isBlockAction}
                                />
                                <ButtonApplyComponent
                                    text={isLoading ? t('global:loading') : t('global:save')}
                                    type='submit'
                                    disabled={isLoading || isBlockAction}
                                />
                            </div>
                        </form>
                    </FormProvider>
                </div>
            </div>
        </div>
    )
}
