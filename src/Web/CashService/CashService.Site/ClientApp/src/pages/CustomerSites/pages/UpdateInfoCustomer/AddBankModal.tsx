import React from "react";
import { useTranslation } from "react-i18next";
import CustomDropdown, { DropdownOption } from '../../../../components/CustomDropdownComponent';
import { ButtonClosePopupComponent, ButtonCancelComponent, ButtonApplyComponent } from "../../../../components/Buttons";
import { MediumTitleComponent, LabelTitleComponent } from "../../../../components/Title";
import { FormProvider, useForm, Controller } from 'react-hook-form';
import { MemberBankAccountRecord } from "../../types/UpdateCustomerType";


interface AddBankModalProps {
    onClose: () => void;
    onAdd: (data: MemberBankAccountRecord) => void;
}
const mockBanks: any[] = []

export const AddBankModal = ({ onClose, onAdd }: AddBankModalProps) => {
    const { t } = useTranslation('customerSites');
    const [isLoading, setIsLoading] = React.useState(false);
    const [isBlockAction, setIsBlockAction] = React.useState(false);

    const [memberBank] = React.useState<DropdownOption[]>(mockBanks?.map(e => ({
        value: e.id,
        label: e.name,
        icon: <img
            src={`https://cdn-customer.nexdev.net/v5/cash-member/create/bank/${e.code}.png`}
            alt={`${e.code} bank icon`}
            className={`w-[16px] h-[16px] mr-[8px] inline-block`}
        />,
    })));
    const [agentBank] = React.useState<DropdownOption[]>(mockBanks?.map(e => ({
        value: e.id,
        label: e.name,
        icon: <img
            src={`https://cdn-customer.nexdev.net/v5/cash-member/create/bank/${e.code}.png`}
            alt={`${e.code} bank icon`}
            className={`w-[16px] h-[16px] mr-[8px] inline-block`}
        />,
    })));

    const methods = useForm<MemberBankAccountRecord>({
        defaultValues: {
            memberBankAccountId: '',
            memberBankAccountNumber: '',
            agentBankAccountNumber: ''
        },
        mode: 'onChange',
        criteriaMode: 'all',
    });

    const {
        handleSubmit,
        control,
        formState: { isDirty, errors },
    } = methods;

    const onSubmit = async (formData: MemberBankAccountRecord) => {
        try {
            setIsLoading(true);
            setIsBlockAction(true);

            // TODO: Implement API call to add bank account
            console.log('Adding bank account:', formData);

            // Simulate API call
            await new Promise(resolve => setTimeout(resolve, 1000));

            onAdd(formData);
            alert(t('updateInfoCashMember.addSuccess') || 'Bank account added successfully');

        } catch (error) {
            console.error('Error adding bank account:', error);
            alert(t('updateInfoCashMember.addError') || 'Failed to add bank account');
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
                    <ButtonClosePopupComponent onClick={onClose} />
                </div>
                <div>
                    <FormProvider {...methods}>
                        <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
                            <div>
                                <LabelTitleComponent text={t('updateInfoCashMember.memberBankAccount')} required={true} />
                                <CustomDropdown
                                    options={memberBank}
                                    onSelect={() => { }}
                                    initialValue={''}
                                />
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
                                {errors.memberBankAccountNumber && (
                                    <div className="text-red-500 text-sm mt-1">{errors.memberBankAccountNumber.message}</div>
                                )}
                            </div>
                            <div>
                                <LabelTitleComponent text={t('updateInfoCashMember.agentBankAccount')} />
                                <CustomDropdown
                                    options={agentBank}
                                    onSelect={() => { }}
                                    initialValue={agentBank[0].value}
                                />
                            </div>



                            <div className="flex justify-center gap-[8px] pt-4">
                                <ButtonCancelComponent
                                    text={t('global:cancel')}
                                    onClick={onClose}
                                    disabled={isLoading || isBlockAction}
                                />
                                <ButtonApplyComponent
                                    text={isLoading ? t('global:loading') : t('global:save')}
                                    type='submit'
                                    disabled={isLoading || isBlockAction || !isDirty}
                                />
                            </div>
                        </form>
                    </FormProvider>
                </div>
            </div>
        </div>
    )
}
