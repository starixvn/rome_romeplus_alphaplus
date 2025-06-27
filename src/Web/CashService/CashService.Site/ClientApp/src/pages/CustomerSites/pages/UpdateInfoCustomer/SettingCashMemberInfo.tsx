import React, { useEffect } from "react";
import {
    ButtonFullComponent,
    ButtonOutlineComponent,
} from "../../../../components/Buttons";
import { LabelTitleComponent } from "../../../../components/Title";
import { useTranslation } from "react-i18next";
import { FormProvider, useForm, Controller } from 'react-hook-form';
import { NUMBER_REGEX, PHONE_CODE_MAX_LENGTH, PHONE_NUMBER_MAX_LENGTH } from "../../../../constants/memberSiteConstants";
import { CustomerInfoData } from "../../types/UpdateCustomerType";
import { useCustomerInfo } from "../../context/CustomerInfoContext";
import LoadingComponent from "../../../../components/LoadingComponent";
import { ValidateMessageComponentV2 } from "../../../../components/Message/ValidateMessageComponent";

export const SettingCashMemberInfo = () => {
    const { t } = useTranslation('customerSites');
    const { t: tGlobal } = useTranslation('global');
    const { customerInfo, loading, originalCustomerInfo } = useCustomerInfo();
    const [isLoading, setIsLoading] = React.useState(false);
    const [isBlockAction, setIsBlockAction] = React.useState(false);

    const methods = useForm<CustomerInfoData>({
        defaultValues: originalCustomerInfo,
        mode: 'onChange',
        criteriaMode: 'all',
    });

    const {
        handleSubmit,
        reset,
        control,
        setValue,
        formState: { isDirty, errors },
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

            // Simulate API call
            await new Promise(resolve => setTimeout(resolve, 1000));

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
        reset(originalCustomerInfo);
    };

    if (loading) {
        return <LoadingComponent />;
    }

    return (
        <div>
            <FormProvider {...methods}>
                <form onSubmit={handleSubmit(onSubmit)}>
                    <div className="w-full sm:w-[80%] md:w-[60%] lg:w-[50%] xl:w-[40%] bg-white border border-gray-200 rounded-lg pt-[10px] pl-[15px] pr-[15px] pb-[5px]">
                        {/* <div className="flex flex-col">
                            <div className="flex flex-col">
                                <LabelTitleComponent
                                    text={t('updateInfoCashMember.memberInfo')}
                                    className=" font-medium text-gray-700"
                                />
                            </div>
                        </div> */}

                        <div className="flex flex-col">
                            {/* First Name and Last Name Row */}
                            <div className="grid grid-cols-1 lg:grid-cols-2 gap-4 sm:gap-6">
                                <div className="flex flex-col gap-2">
                                    <LabelTitleComponent
                                        text={t('updateInfoCashMember.fistName')}
                                        required={true}
                                        className=" font-medium text-gray-700"
                                    />
                                    <Controller
                                        name="fistName"
                                        control={control}
                                        rules={{ required: tGlobal('pleaseFillField') || 'First name is required' }}
                                        render={({ field }) => (
                                            <input
                                                {...field}
                                                type="text"
                                                className={`control-field ${errors.fistName ? 'error' : ''}`}
                                                placeholder={t('updateInfoCashMember.fistName')}
                                            />
                                        )}
                                    />
                                    <ValidateMessageComponentV2 message={errors.fistName?.message as string} className="min-h-[20px]" />
                                </div>

                                <div className="flex flex-col gap-2">
                                    <LabelTitleComponent
                                        text={t('updateInfoCashMember.lastName')}
                                        required={true}
                                        className=" font-medium text-gray-700"
                                    />
                                    <Controller
                                        name="lastName"
                                        control={control}
                                        rules={{ required: tGlobal('pleaseFillField') || 'Last name is required' }}
                                        render={({ field }) => (
                                            <input
                                                {...field}
                                                type="text"
                                                className={`control-field ${errors.lastName ? 'error' : ''}`}
                                                placeholder={t('updateInfoCashMember.lastName')}
                                            />
                                        )}
                                    />
                                
                                    <ValidateMessageComponentV2 message={errors.lastName?.message as string} className="min-h-[20px]" />
                                </div>
                            </div>

                            {/* Phone and Mobile Row */}
                            <div className="grid grid-cols-1 lg:grid-cols-2 gap-4 sm:gap-6">
                                <div className="flex flex-col gap-1">
                                    <LabelTitleComponent
                                        text={t('updateInfoCashMember.mobile')}
                                        className="font-medium text-gray-700"
                                        required={true}
                                    />
                                    <div className={`flex items-center border control-field ${errors.mobileCode || errors.mobileNumber ? 'error' : ''}`}>
                                        <span className="text-gray-500">+</span>
                                        <Controller
                                            name="mobileCode"
                                            control={control}
                                            rules={{ required: tGlobal('pleaseFillField') || 'Mobile code is required' }}
                                            render={({ field }) => (
                                                <input
                                                    {...field}
                                                    id="mobileCode"
                                                    type="text"
                                                    className={`focus:outline-none focus:border-[var(--button-border)] h-[100%] w-[30px] min-w-[30px]`}
                                                    maxLength={PHONE_CODE_MAX_LENGTH}
                                                    onChange={(e) => {
                                                        e.preventDefault();
                                                        const value = e.target.value.replace(NUMBER_REGEX, '').slice(0, PHONE_CODE_MAX_LENGTH);
                                                        field.onChange(value);
                                                    }}
                                                />
                                            )}
                                        />
                                        <div className="w-px h-4 sm:h-6 bg-gray-300"></div>
                                        <Controller
                                            name="mobileNumber"
                                            control={control}
                                            rules={{ required: tGlobal('pleaseFillField') || 'Mobile number is required' }}
                                            render={({ field }) => (
                                                <input
                                                    {...field}
                                                    id="mobileNumber"
                                                    className="flex-1 border-none focus:outline-none px-2 sm:px-3 py-2"
                                                    type="text"
                                                    placeholder="Enter mobile number"
                                                    maxLength={PHONE_NUMBER_MAX_LENGTH}
                                                    onChange={(e) => {
                                                        e.preventDefault();
                                                        const value = e.target.value.replace(NUMBER_REGEX, '').slice(0, PHONE_NUMBER_MAX_LENGTH);
                                                        field.onChange(value);
                                                    }}
                                                />
                                            )}
                                        />
                                    </div>
                                    <ValidateMessageComponentV2 message={(errors.mobileCode?.message || errors.mobileNumber?.message) as string} className="min-h-[20px]" />
                                </div>
                                <div className="flex flex-col gap-1">
                                    <LabelTitleComponent
                                        text={t('updateInfoCashMember.phone')}
                                        className=" font-medium text-gray-700"
                                    />
                                    <Controller
                                        name="phone"
                                        control={control}
                                        render={({ field }) => (
                                            <input
                                                {...field}
                                                type="text"
                                                className="control-field"
                                                placeholder={t('updateInfoCashMember.phone')}
                                            />
                                        )}
                                    />
                                    <ValidateMessageComponentV2 message={errors.phone?.message as string} />
                                </div>


                            </div>

                            {/* Email and Fax Row */}
                            <div className="grid grid-cols-1 lg:grid-cols-2 gap-4 sm:gap-6">
                                <div className="flex flex-col gap-1">
                                    <LabelTitleComponent
                                        text={t('updateInfoCashMember.email')}
                                        className="font-medium text-gray-700"
                                    />
                                    <Controller
                                        name="email"
                                        control={control}
                                        rules={{
                                            pattern: {
                                                value: /^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,}$/i,
                                                message: t('updateInfoCashMember.emailNotValid') || 'Invalid email address'
                                            }
                                        }}
                                        render={({ field }) => (
                                            <input
                                                {...field}
                                                type="email"
                                                className={`control-field ${errors.email ? 'error' : ''}`}
                                                placeholder="example@email.com"
                                            />
                                        )}
                                    />
                                    <ValidateMessageComponentV2 message={errors.email?.message as string} className="min-h-[20px]" />
                                </div>

                                <div className="flex flex-col gap-1">
                                    <LabelTitleComponent
                                        text={t('updateInfoCashMember.fax')}
                                        className=" font-medium text-gray-700"
                                    />
                                    <Controller
                                        name="fax"
                                        control={control}
                                        render={({ field }) => (
                                            <input
                                                {...field}
                                                type="text"
                                                className="control-field"
                                                placeholder={t('updateInfoCashMember.fax')}
                                            />
                                        )}
                                    />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div className="flex flex-col sm:flex-row gap-[8px] mt-[15px]">
                        <ButtonFullComponent
                            className="w-full sm:w-[100px] [&>*]:w-[100%]"
                            text={isLoading ? t('global:loading') : t('updateInfoCashMember.update')}
                            type="submit"
                            disabled={isLoading || isBlockAction}
                        />
                        <ButtonOutlineComponent
                            className="w-full sm:w-[100px] [&>*]:w-[100%]"
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



