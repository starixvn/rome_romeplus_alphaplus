import { useTranslation } from "react-i18next";
import { useState, useEffect } from "react";
import { CustomerInfoData } from "../../types/UpdateCustomerType";
import { FormProvider, useForm, Controller } from 'react-hook-form';
import { ButtonFullComponent, ButtonOutlineComponent } from "../../../../components/Buttons";
import { LabelTitleComponent } from "../../../../components/Title";
import { GetCustomerInfo, UpdateCustomerInfo } from "../../services/CustomerInfoServices";
import { splitMobile } from "../../../../utils/sharedUtils";
import { useResultMessage } from "../../../../hooks/useResultMessage";
import {
    EMAIL_MAX_LENGTH,
    EMAIL_REGEX,
    FAX_MAX_LENGTH,
    NAME_MAX_LENGTH,
    NUMBER_REGEX,
    PHONE_NUMBER_MAX_LENGTH
} from "../../../../constants/memberSiteConstants";

export const SettingCustomerInfoWrapper = () => {
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
        formState: { isDirty },
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
            if (!isValidEmail(formData)) {
                return;
            }

            setLoading(true);
            setIsBlockAction(true);

            const response = await UpdateCustomerInfo({
                FirstName: formData.firstName,
                LastName: formData.lastName,
                Phone: formData.phone,
                Mobile: formData.mobilePhone,
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

    const isValidEmail = (formData: CustomerInfoData) => {
        if (formData.email && !EMAIL_REGEX.test(formData.email)) {
            showResult('error', t('updateInfoCashMember.emailNotValid'));
            return false;
        }
        return true;
    }

    const handleReset = (e: React.MouseEvent<HTMLButtonElement>) => {
        e.preventDefault();
        reset(originalCustomerInfo);
    }

    return (
        <div>
            <FormProvider {...methods}>
                <div className="w-[50%] mb-[10px]">
                    {ResultMessage()}
                </div>
                <form>
                    <div className="table">
                        <table className="w-[50%] border-collapse no-hover">
                            <thead style={{ boxShadow: 'none' }}>
                                <tr>
                                    <th colSpan={4} scope="col">{t('updateInfoCashMember.customerInformation')}</th>
                                </tr>
                            </thead>
                            <tbody className="font-[500]">
                                <tr>
                                    <td>
                                        <LabelTitleComponent text={t('updateInfoCashMember.firstName')} className="text-right" />
                                    </td>
                                    <td>
                                        <Controller
                                            name="firstName"
                                            control={control}
                                            render={({ field }) => (
                                                <input
                                                    {...field}
                                                    type="text"
                                                    maxLength={NAME_MAX_LENGTH}
                                                    className="w-full border border-gray-300 rounded px-2 py-1 focus:outline-none"
                                                    onBlur={(e) => {
                                                        const trimmedValue = e.target.value.trim();
                                                        field.onChange(trimmedValue);
                                                    }}
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
                                                    maxLength={NAME_MAX_LENGTH}
                                                    className="w-full border border-gray-300 rounded px-2 py-1 focus:outline-none"
                                                    onBlur={(e) => {
                                                        const trimmedValue = e.target.value.trim();
                                                        field.onChange(trimmedValue);
                                                    }}
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
                                                    maxLength={PHONE_NUMBER_MAX_LENGTH}
                                                    className="w-full border border-gray-300 rounded px-2 py-1 focus:outline-none"
                                                    onChange={(e) => {
                                                        e.preventDefault();
                                                        const value = e.target.value.replace(NUMBER_REGEX, '').slice(0, PHONE_NUMBER_MAX_LENGTH);
                                                        field.onChange(value);
                                                    }}
                                                    onBlur={(e) => {
                                                        const trimmedValue = e.target.value.trim();
                                                        field.onChange(trimmedValue);
                                                    }}
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
                                                    maxLength={PHONE_NUMBER_MAX_LENGTH}
                                                    className="w-full border border-gray-300 rounded px-2 py-1 focus:outline-none"
                                                    onChange={(e) => {
                                                        e.preventDefault();
                                                        const value = e.target.value.replace(NUMBER_REGEX, '').slice(0, PHONE_NUMBER_MAX_LENGTH);
                                                        field.onChange(value);
                                                    }}
                                                    onBlur={(e) => {
                                                        const trimmedValue = e.target.value.trim();
                                                        field.onChange(trimmedValue);
                                                    }}
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
                                                    maxLength={EMAIL_MAX_LENGTH}
                                                    className="w-full border border-gray-300 rounded px-2 py-1 focus:outline-none"
                                                    onBlur={(e) => {
                                                        const trimmedValue = e.target.value.trim();
                                                        field.onChange(trimmedValue);
                                                    }}
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
                                                    maxLength={FAX_MAX_LENGTH}
                                                    className="w-full border border-gray-300 rounded px-2 py-1 focus:outline-none"
                                                    onBlur={(e) => {
                                                        const trimmedValue = e.target.value.trim();
                                                        field.onChange(trimmedValue);
                                                    }}
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
                            text={loading ? t('global:loading') : t('updateInfoCashMember.update')}
                            onClick={handleSubmit(onSubmit)}
                            disabled={loading || isBlockAction}
                        />
                        <ButtonOutlineComponent
                            className="w-[120px] [&>*]:w-[100%]"
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
