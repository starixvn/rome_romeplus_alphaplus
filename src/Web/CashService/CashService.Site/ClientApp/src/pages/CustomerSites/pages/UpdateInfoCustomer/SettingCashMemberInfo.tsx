import { useEffect, useState } from "react";
import {
    ButtonFullComponent,
    ButtonOutlineComponent,
} from "../../../../components/Buttons";
import { LabelTitleComponent } from "../../../../components/Title";
import { useTranslation } from "react-i18next";
import { FormProvider, useForm, Controller } from 'react-hook-form';
import {
    EMAIL_MAX_LENGTH,
    EMAIL_REGEX,
    FAX_MAX_LENGTH,
    NAME_MAX_LENGTH,
    NUMBER_REGEX,
    PHONE_CODE_MAX_LENGTH,
    PHONE_NUMBER_MAX_LENGTH
} from "../../../../constants/memberSiteConstants";
import { CustomerInfoData } from "../../types/UpdateCustomerType";
import { ValidateMessageComponent } from "../../../../components/Message/ValidateMessageComponent";
import { GetCustomerInfo, UpdateCustomerInfo } from "../../services/CustomerInfoServices";
import { splitMobile } from "../../../../utils/sharedUtils";
import { useResultMessage } from "../../../../hooks/useResultMessage";

export const SettingCashMemberInfo = () => {
    const { t } = useTranslation('customerSites');
    const [originalCustomerInfo, setOriginalCustomerInfo] = useState<CustomerInfoData>({
        firstName: '',
        lastName: '',
        mobilePhone: '',
        mobileCode: '',
        mobileNumber: '',
        phone: '',
        email: '',
        fax: ''
    });
    const [loading, setLoading] = useState(false);
    const [isBlockAction, setIsBlockAction] = useState(false);
    const { showResult, ResultMessage } = useResultMessage();

    const methods = useForm<CustomerInfoData>({
        defaultValues: originalCustomerInfo,
        mode: 'onChange',
        criteriaMode: 'all',
    });

    const {
        handleSubmit,
        reset,
        control,
        formState: { errors, isDirty },
    } = methods;

    useEffect(() => {
        const fetchCustomerInfo = async () => {
            const data = await GetCustomerInfo<CustomerInfoData>({
                custId: updateInfoCustomerPageConfig.custId
            });
            const mobile = splitMobile(data.mobilePhone);
            data.mobileCode = mobile.countryCode;
            data.mobileNumber = mobile.mobile;
            reset(data);
            setOriginalCustomerInfo(data);
        }
        fetchCustomerInfo();
    }, [reset]);

    const onSubmit = async (formData: CustomerInfoData) => {
        try {
            if (!isDirty) {
                showResult('error', t('global:thereNoChangeToUpdate'));
                return;
            }

            setLoading(true);
            setIsBlockAction(true);

            console.log('Updating customer info:', formData);
            const response = await UpdateCustomerInfo({
                CustId: updateInfoCustomerPageConfig.custId,
                FirstName: formData.firstName,
                LastName: formData.lastName,
                Phone: formData.phone,
                MobilePhone: formData.mobileCode + '-' + formData.mobileNumber,
                Email: formData.email,
                Fax: formData.fax
            });
            if ((response as any).isSuccessful) {
                setOriginalCustomerInfo(formData);
                reset(formData);
                showResult('success', t('global:updateSuccess'));
            } else {
                showResult('error', (response as any).message);
            }
        } catch (error) {
            console.error('Error updating customer info:', error);
            showResult('error', t('global:updateFailed'));
        } finally {
            setLoading(false);
            setIsBlockAction(false);
        }
    };

    const handleReset = (e: React.MouseEvent<HTMLButtonElement>) => {
        e.preventDefault();
        reset(originalCustomerInfo);
    }

    return (
        <div className="w-full sm:w-[80%] md:w-[60%] lg:w-[50%] xl:w-[40%]">
            <div className="mb-[10px]">
                {ResultMessage()}
            </div>
            <FormProvider {...methods}>
                <form>
                    <div className="bg-white border border-gray-200 rounded-[3px] p-[15px]">
                        <div className="flex flex-col gap-[12px]">
                            {/* First Name and Last Name Row */}
                            <div className="grid grid-cols-2 gap-[15px]">
                                <div className="flex flex-col">
                                    <LabelTitleComponent
                                        text={t('updateInfoCashMember.firstName')}
                                        required={true}
                                        className=" font-medium text-gray-700"
                                    />
                                    <Controller
                                        name="firstName"
                                        control={control}
                                        rules={{ required: t('updateInfoCashMember.firstNameRequired') }}
                                        render={({ field }) => (
                                            <input
                                                {...field}
                                                maxLength={NAME_MAX_LENGTH}
                                                type="text"
                                                className={`control-field ${errors.firstName ? 'error' : ''}`}
                                                onBlur={(e) => {
                                                    const trimmedValue = e.target.value.trim();
                                                    field.onChange(trimmedValue);
                                                }}
                                            />
                                        )}
                                    />
                                    <ValidateMessageComponent message={errors.firstName?.message as string} />
                                </div>

                                <div className="flex flex-col">
                                    <LabelTitleComponent
                                        text={t('updateInfoCashMember.lastName')}
                                        required={true}
                                        className=" font-medium text-gray-700"
                                    />
                                    <Controller
                                        name="lastName"
                                        control={control}
                                        rules={{ required: t('updateInfoCashMember.lastNameRequired') }}
                                        render={({ field }) => (
                                            <input
                                                {...field}
                                                maxLength={NAME_MAX_LENGTH}
                                                type="text"
                                                className={`control-field ${errors.lastName ? 'error' : ''}`}
                                                onBlur={(e) => {
                                                    const trimmedValue = e.target.value.trim();
                                                    field.onChange(trimmedValue);
                                                }}
                                            />
                                        )}
                                    />

                                    <ValidateMessageComponent message={errors.lastName?.message as string} />
                                </div>
                            </div>

                            {/* Phone and Mobile Row */}
                            <div className="grid grid-cols-2 gap-[15px]">
                                <div className="flex flex-col">
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
                                            rules={{ required: t('updateInfoCashMember.mobileRequired') }}
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
                                            rules={{ required: t('updateInfoCashMember.mobileRequired') }}
                                            render={({ field }) => (
                                                <input
                                                    {...field}
                                                    id="mobileNumber"
                                                    className="flex-1 border-none focus:outline-none px-2 sm:px-3 py-2"
                                                    type="text"
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
                                    <ValidateMessageComponent message={(errors.mobileCode?.message || errors.mobileNumber?.message) as string} />
                                </div>
                                <div className="flex flex-col">
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
                                                maxLength={PHONE_NUMBER_MAX_LENGTH}
                                                onChange={(e) => {
                                                    e.preventDefault();
                                                    const value = e.target.value.replace(NUMBER_REGEX, '').slice(0, PHONE_NUMBER_MAX_LENGTH);
                                                    field.onChange(value);
                                                }}
                                            />
                                        )}
                                    />
                                    <ValidateMessageComponent message={errors.phone?.message as string} />
                                </div>


                            </div>

                            {/* Email and Fax Row */}
                            <div className="grid grid-cols-2 gap-[15px]">
                                <div className="flex flex-col">
                                    <LabelTitleComponent
                                        text={t('updateInfoCashMember.email')}
                                        className="font-medium text-gray-700"
                                    />
                                    <Controller
                                        name="email"
                                        control={control}
                                        rules={{
                                            pattern: {
                                                value: EMAIL_REGEX,
                                                message: t('updateInfoCashMember.emailNotValid')
                                            }
                                        }}
                                        render={({ field }) => (
                                            <input
                                                {...field}
                                                maxLength={EMAIL_MAX_LENGTH}
                                                className={`control-field ${errors.email ? 'error' : ''}`}
                                                onBlur={(e) => {
                                                    const trimmedValue = e.target.value.trim();
                                                    field.onChange(trimmedValue);
                                                }}
                                            />
                                        )}
                                    />
                                    <ValidateMessageComponent message={errors.email?.message as string} />
                                </div>

                                <div className="flex flex-col">
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
                                                maxLength={FAX_MAX_LENGTH}
                                                type="text"
                                                className="control-field"
                                                onBlur={(e) => {
                                                    const trimmedValue = e.target.value.trim();
                                                    field.onChange(trimmedValue);
                                                }}
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
                            onClick={handleSubmit(onSubmit)}
                            text={loading ? t('global:loading') : t('updateInfoCashMember.update')}
                            disabled={loading || isBlockAction}
                        />
                        <ButtonOutlineComponent
                            className="w-full sm:w-[100px] [&>*]:w-[100%]"
                            text={t('updateInfoCashMember.reset')}
                            onClick={handleReset}
                            disabled={loading || isBlockAction}
                        />
                    </div>
                </form>
            </FormProvider>
        </div>
    )
}



