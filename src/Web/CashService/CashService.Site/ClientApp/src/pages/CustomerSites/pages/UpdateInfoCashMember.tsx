import { createRoot } from "react-dom/client";
import React from "react";
import {
    ButtonTabInactiveComponent,
    ButtonTabActiveComponent,
    ButtonFullComponent,
    ButtonOutlineComponent,
    ButtonCancelComponent,
    ButtonApplyComponent
} from "../../../components/Buttons";
import { TitleComponent, LabelTitleComponent } from "../../../components/Title";
import { useTranslation } from "react-i18next";
import i18n from '../../../i18n/i18n-customerSites';
import '../../../styles/common.css';
import '../../../styles/global.scss';
import { useCookieLanguage } from "../../../hooks/useCookieLanguage";
import { FormProvider, useForm, Controller } from 'react-hook-form';
import { PHONE_CODE_MAX_LENGTH, PHONE_NUMBER_MAX_LENGTH } from "../../../constants/memberSiteConstants";
import PopupModalComponent from "../../../components/PopupModalComponent";
import NoRecordFound from "../../../components/NoRecordFoundComponent";


export default function UpdateInfoCashMember() {
    useCookieLanguage(i18n);
    const { t } = useTranslation('customerSites');
    const [activeTab, setActiveTab] = React.useState('bankInfo');
    const tabButton = (tab: string) => {
        const tabList: React.ReactNode[] = [];
        if (tab === 'memberInfo') {
            tabList.push(<ButtonTabInactiveComponent text={t('updateInfoCashMember.bankInfo')} />)
            tabList.push(<ButtonTabActiveComponent text={t('updateInfoCashMember.memberInfo')} onClick={() => setActiveTab('bankInfo')} />)
        }
        if (tab === 'bankInfo') {
            tabList.push(<ButtonTabActiveComponent text={t('updateInfoCashMember.bankInfo')} onClick={() => setActiveTab('memberInfo')} />)
            tabList.push(<ButtonTabInactiveComponent text={t('updateInfoCashMember.memberInfo')} />)
        }
        return tabList.map((item, index) => (
            <li key={`${tab}-${index}`} className="mr-[8px]">
                {item}
            </li>
        ));
    }
    return (
        <>
            <TitleComponent title={t('updateInfoCashMember.memberInfo')} showTippy={false} />
            <ul className="flex mt-[-12px] border-b border-[var(--border-color)] mb-[15px]">
                {tabButton(activeTab)}
            </ul>

            <div>
                {activeTab === 'memberInfo' && (
                    <SettingCustomerInfo />
                )}
                {activeTab === 'bankInfo' && (
                    <SettingMemberBankInfo />
                )}
            </div>
        </>
    );
}

interface CustomerInfoData {
    fistName: string;
    lastName: string;
    mobile: string;
    phone: string;
    email: string;
    fax: string;
}

const SettingCustomerInfo = () => {
    const { t } = useTranslation('customerSites');
    const [isLoading, setIsLoading] = React.useState(false);
    const [isBlockAction, setIsBlockAction] = React.useState(false);

    const CustomerInfoDataDefault: CustomerInfoData = {
        fistName: '',
        lastName: '',
        mobile: '',
        phone: '',
        email: '',
        fax: ''
    }

    const methods = useForm<CustomerInfoData>({
        defaultValues: CustomerInfoDataDefault,
        mode: 'onChange',
        criteriaMode: 'all',
    });

    const {
        handleSubmit,
        reset,
        control,
        formState: { isDirty, errors },
    } = methods;

    const onSubmit = async (formData: CustomerInfoData) => {
        try {
            setIsLoading(true);
            setIsBlockAction(true);

            // TODO: Implement API call to update customer info
            console.log('Updating customer info:', formData);

            // Simulate API call
            await new Promise(resolve => setTimeout(resolve, 1000));

            // Show success message or handle response
            alert(t('updateInfoCashMember.updateSuccess') || 'Update successful');

        } catch (error) {
            console.error('Error updating customer info:', error);
            alert(t('updateInfoCashMember.updateError') || 'Update failed');
        } finally {
            setIsLoading(false);
            setIsBlockAction(false);
        }
    };

    const handleClearAll = () => {
        reset(CustomerInfoDataDefault);
    };

    return (
        <div>
            <FormProvider {...methods}>
                <form onSubmit={handleSubmit(onSubmit)}>
                    <div className="flex gap-[8px] mb-[10px]">
                        <ButtonFullComponent
                            className="w-[120px] [&>*]:w-[100%]"
                            text={isLoading ? t('global:loading') : t('updateInfoCashMember.update')}
                            type="submit"
                            disabled={isLoading || isBlockAction || !isDirty}
                        />
                        <ButtonOutlineComponent
                            className="w-[120px] [&>*]:w-[100%]"
                            text={t('updateInfoCashMember.clearAll')}
                            onClick={handleClearAll}
                            disabled={isLoading || isBlockAction}
                        />
                    </div>
                    <div className="table">
                        <table className="w-[50%] border-collapse no-hover">
                            <thead>
                                <tr>
                                    <th colSpan={4} scope="col">{t('updateInfoCashMember.customerInformation')}</th>
                                </tr>
                            </thead>
                            <tbody className="font-[500]">
                                <tr>
                                    <td>
                                        <LabelTitleComponent text={t('updateInfoCashMember.fistName')} required={true} className="text-right" />
                                    </td>
                                    <td>
                                        <Controller
                                            name="fistName"
                                            control={control}
                                            rules={{ required: t('updateInfoCashMember.fistNameRequired') || 'First name is required' }}
                                            render={({ field }) => (
                                                <input
                                                    {...field}
                                                    type="text"
                                                    className={`w-full border border-gray-300 rounded px-2 py-1 focus:outline-none ${errors.fistName ? 'border-red-500' : ''}`}
                                                />
                                            )}
                                        />
                                        {errors.fistName && (
                                            <div className="text-red-500 text-sm mt-1">{errors.fistName.message}</div>
                                        )}
                                    </td>
                                    <td>
                                        <LabelTitleComponent text={t('updateInfoCashMember.lastName')} required={true} className="text-right" />
                                    </td>
                                    <td>
                                        <Controller
                                            name="lastName"
                                            control={control}
                                            rules={{ required: t('updateInfoCashMember.lastNameRequired') || 'Last name is required' }}
                                            render={({ field }) => (
                                                <input
                                                    {...field}
                                                    type="text"
                                                    className={`w-full border border-gray-300 rounded px-2 py-1 focus:outline-none ${errors.lastName ? 'border-red-500' : ''}`}
                                                />
                                            )}
                                        />
                                        {errors.lastName && (
                                            <div className="text-red-500 text-sm mt-1">{errors.lastName.message}</div>
                                        )}
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <LabelTitleComponent text={t('updateInfoCashMember.phone')} className="text-right" />
                                    </td>
                                    <td>
                                        <Controller
                                            name="phone"
                                            control={control}
                                            render={({ field }) => (
                                                <input
                                                    {...field}
                                                    type="text"
                                                    className="w-full border border-gray-300 rounded px-2 py-1 focus:outline-none"
                                                />
                                            )}
                                        />
                                    </td>
                                    <td>
                                        <LabelTitleComponent text={t('updateInfoCashMember.mobilePhone')} className="text-right" />
                                    </td>
                                    <td>
                                        <div className={`flex items-center h-[28px] w-full border-[1px] border-[var(--border-control)] rounded-[3px] px-[8px] focus:outline-none focus:border-[var(--button-border)] cursor-pointer]`}>
                                            <span className="relative top-[1px]">+</span>
                                            <Controller
                                                name="mobile"
                                                control={control}
                                                render={({ field }) => (
                                                    <input
                                                        {...field}
                                                        id="numberPhone"
                                                        key="numberPhone"
                                                        type="text"
                                                        className={`focus:outline-none focus:border-[var(--button-border)] h-[100%] w-[30px] min-w-[30px]`}
                                                        maxLength={PHONE_CODE_MAX_LENGTH}
                                                    />
                                                )}
                                            />
                                            <Controller
                                                name="mobile"
                                                control={control}
                                                render={({ field }) => (
                                                    <input
                                                        {...field}
                                                        className={`focus:outline-none w-[100%] h-[100%] pl-[10px] border-l-[1px] border-[var(--border-control)] max-[768px]:text-[16px] max-[768px]:leading-[16px]`}
                                                        id="mobile"
                                                        type="text"
                                                        placeholder=""
                                                        maxLength={PHONE_NUMBER_MAX_LENGTH}
                                                    />
                                                )}
                                            />
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <LabelTitleComponent text={t('updateInfoCashMember.email')} className="text-right" />
                                    </td>
                                    <td>
                                        <Controller
                                            name="email"
                                            control={control}
                                            rules={{
                                                pattern: {
                                                    value: /^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,}$/i,
                                                    message: t('updateInfoCashMember.emailInvalid') || 'Invalid email address'
                                                }
                                            }}
                                            render={({ field }) => (
                                                <input
                                                    {...field}
                                                    type="email"
                                                    className={`w-full border border-gray-300 rounded px-2 py-1 focus:outline-none ${errors.email ? 'border-red-500' : ''}`}
                                                />
                                            )}
                                        />
                                        {errors.email && (
                                            <div className="text-red-500 text-sm mt-1">{errors.email.message}</div>
                                        )}
                                    </td>
                                    <td>
                                        <LabelTitleComponent text={t('updateInfoCashMember.fax')} className="text-right" />
                                    </td>
                                    <td>
                                        <Controller
                                            name="fax"
                                            control={control}
                                            render={({ field }) => (
                                                <input
                                                    {...field}
                                                    type="text"
                                                    className="w-full border border-gray-300 rounded px-2 py-1 focus:outline-none"
                                                />
                                            )}
                                        />
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </form>
            </FormProvider>
        </div>
    )
}

interface BankAccountData {
    id: string;
    memberBankAccount: string;
    agentBankAccount: string;
    bankName: string;
    accountNumber: string;
    accountHolderName: string;
}

interface AddBankAccountFormData {
    bankName: string;
    accountNumber: string;
    accountHolderName: string;
}

const SettingMemberBankInfo = () => {
    const { t } = useTranslation('customerSites');
    const [showAddBankAccountPopup, setShowAddBankAccountPopup] = React.useState(false);
    const [bankAccounts, setBankAccounts] = React.useState<BankAccountData[]>([]);
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
            let mockData: BankAccountData[] = [
                {
                    id: '1',
                    memberBankAccount: '1234567890',
                    agentBankAccount: '0987654321',
                    bankName: 'Sample Bank',
                    accountNumber: '1234567890',
                    accountHolderName: 'John Doe'
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

    const handleAddBankAccount = (newAccount: AddBankAccountFormData) => {
        const bankAccount: BankAccountData = {
            id: Date.now().toString(),
            memberBankAccount: newAccount.accountNumber,
            agentBankAccount: newAccount.accountNumber, // This might need to be different based on business logic
            bankName: newAccount.bankName,
            accountNumber: newAccount.accountNumber,
            accountHolderName: newAccount.accountHolderName
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
                                    <td>{account.memberBankAccount}</td>
                                    <td>{account.agentBankAccount}</td>
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
                <AddBankAccountPopup
                    onClose={() => setShowAddBankAccountPopup(false)}
                    onAdd={handleAddBankAccount}
                />
            )}
        </>
    )
}

interface AddBankAccountPopupProps {
    onClose: () => void;
    onAdd: (data: AddBankAccountFormData) => void;
}

const AddBankAccountPopup: React.FC<AddBankAccountPopupProps> = ({ onClose, onAdd }) => {
    const { t } = useTranslation('customerSites');
    const [isLoading, setIsLoading] = React.useState(false);
    const [isBlockAction, setIsBlockAction] = React.useState(false);

    const methods = useForm<AddBankAccountFormData>({
        defaultValues: {
            bankName: '',
            accountNumber: '',
            accountHolderName: ''
        },
        mode: 'onChange',
        criteriaMode: 'all',
    });

    const {
        handleSubmit,
        control,
        formState: { isDirty, errors },
    } = methods;

    const onSubmit = async (formData: AddBankAccountFormData) => {
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
        <PopupModalComponent title={t('updateInfoCashMember.addBankAccount')} onClose={onClose}>
            <FormProvider {...methods}>
                <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
                    <div>
                        <LabelTitleComponent text={t('updateInfoCashMember.bankName')} required={true} />
                        <Controller
                            name="bankName"
                            control={control}
                            rules={{ required: t('updateInfoCashMember.bankNameRequired') || 'Bank name is required' }}
                            render={({ field }) => (
                                <input
                                    {...field}
                                    type="text"
                                    className={`w-full border border-gray-300 rounded px-2 py-1 focus:outline-none ${errors.bankName ? 'border-red-500' : ''}`}
                                />
                            )}
                        />
                        {errors.bankName && (
                            <div className="text-red-500 text-sm mt-1">{errors.bankName.message}</div>
                        )}
                    </div>

                    <div>
                        <LabelTitleComponent text={t('updateInfoCashMember.accountNumber')} required={true} />
                        <Controller
                            name="accountNumber"
                            control={control}
                            rules={{ required: t('updateInfoCashMember.accountNumberRequired') || 'Account number is required' }}
                            render={({ field }) => (
                                <input
                                    {...field}
                                    type="text"
                                    className={`w-full border border-gray-300 rounded px-2 py-1 focus:outline-none ${errors.accountNumber ? 'border-red-500' : ''}`}
                                />
                            )}
                        />
                        {errors.accountNumber && (
                            <div className="text-red-500 text-sm mt-1">{errors.accountNumber.message}</div>
                        )}
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
        </PopupModalComponent>
    )
}

createRoot(document.getElementById('root')!).render(
    <React.StrictMode>
        <UpdateInfoCashMember />
    </React.StrictMode>
);
