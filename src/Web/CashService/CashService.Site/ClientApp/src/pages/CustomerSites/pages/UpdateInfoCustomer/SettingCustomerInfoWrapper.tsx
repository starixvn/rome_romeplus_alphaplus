import { useTranslation } from "react-i18next";
import { useState, useEffect } from "react";
import { CustomerInfoData } from "../../types/UpdateCustomerType";
import { FormProvider, useForm, Controller } from 'react-hook-form';
import { ButtonFullComponent, ButtonOutlineComponent } from "../../../../components/Buttons";
import { LabelTitleComponent, TitleComponent } from "../../../../components/Title";
import { useCustomerInfo } from "../../context/CustomerInfoContext";
import LoadingComponent from "../../../../components/LoadingComponent";
import { UpdateCustomerInfo } from "../../services/CustomerInfoServices";

export const SettingCustomerInfoWrapper = () => {
    const { t } = useTranslation('customerSites');
    const { customerInfo, loading, error, refreshCustomerInfo } = useCustomerInfo();
    const [isLoading, setIsLoading] = useState(false);
    const [isBlockAction, setIsBlockAction] = useState(false);

    const CustomerInfoDataDefault: CustomerInfoData = {
        fistName: '',
        lastName: '',
        mobilePhone: '',
        mobileCode: '',
        mobileNumber: '',
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
        formState: { isDirty },
    } = methods;

    // Populate form when customerInfo is loaded
    useEffect(() => {
        if (customerInfo) {
            reset(customerInfo);
        }
    }, [customerInfo, reset]);

    const onSubmit = async (formData: CustomerInfoData) => {
        try {
            setIsLoading(true);
            setIsBlockAction(true);

            // TODO: Implement API call to update customer info
            console.log('Updating customer info:', formData);

            const response = await UpdateCustomerInfo({
                FirstName: formData.fistName,
                LastName: formData.lastName,
                Phone: formData.phone,
                Mobile: formData.mobilePhone,
                Email: formData.email,
                Fax: formData.fax
            });
            console.log('response', response);

            // Refresh customer info after successful update
            // refreshCustomerInfo();

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

    if (loading) {
        return <LoadingComponent />;
    }

    if (error) {
        return (
            <div className="text-red-500">
                <p>Error: {error}</p>
                <button onClick={refreshCustomerInfo} className="mt-2 px-4 py-2 bg-blue-500 text-white rounded">
                    Retry
                </button>
            </div>
        );
    }

    return (
        <div>
            <TitleComponent title={t('updateInfoCashMember.memberInfo')} showTippy={false} />
            <FormProvider {...methods}>
                <form onSubmit={handleSubmit(onSubmit)}>
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
                                        <LabelTitleComponent text={t('updateInfoCashMember.fistName')} className="text-right" />
                                    </td>
                                    <td>
                                        <Controller
                                            name="fistName"
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
                                        <LabelTitleComponent text={t('updateInfoCashMember.lastName')} className="text-right" />
                                    </td>
                                    <td>
                                        <Controller
                                            name="lastName"
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
                                        <Controller
                                            name="mobilePhone"
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
                                <tr>
                                    <td>
                                        <LabelTitleComponent text={t('updateInfoCashMember.email')} className="text-right" />
                                    </td>
                                    <td>
                                        <Controller
                                            name="email"
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
                    <div className="flex gap-[8px] mt-[15px]">
                        <ButtonFullComponent
                            className="w-[120px] [&>*]:w-[100%]"
                            text={isLoading ? t('global:loading') : t('updateInfoCashMember.update')}
                            type="submit"
                            disabled={isLoading || isBlockAction || !isDirty}
                        />
                        <ButtonOutlineComponent
                            className="w-[120px] [&>*]:w-[100%]"
                            text={t('updateInfoCashMember.reset')}
                            onClick={handleClearAll}
                            disabled={isLoading || isBlockAction}
                        />
                    </div>
                </form>
            </FormProvider>
        </div>
    )
}
