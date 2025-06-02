import { MobilePhone as MobilePhoneType } from "../../../services/interfaces/MemberSites";
import { useTranslation } from "react-i18next";
import { FieldError, useFormContext } from "react-hook-form";
import { NUMBER_REGEX, PHONE_CODE_MAX_LENGTH, PHONE_NUMBER_MAX_LENGTH } from "../../../constants";

interface MobiCodeProps {
    code: string;
    onChange: (code: string) => void;
}

interface MobilePhoneErrors {
    code?: FieldError;
    phone?: FieldError;
}

const getErrorMessage = (error: FieldError | undefined) => {
    if (!error) return '';
    if (typeof error.message === 'string') return error.message;
    if (Array.isArray(error.message)) return error.message[0];
    return '';
};

const MobiCode = (props: MobiCodeProps) => {
    const { onChange } = props;
    const { register, formState: { errors } } = useFormContext();
    const { t } = useTranslation('memberSite');
    const mobilePhoneErrors = errors.mobilePhone as MobilePhoneErrors | undefined;

    const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        e.preventDefault();
        const value = e.target.value.replace(NUMBER_REGEX, '').slice(0, PHONE_CODE_MAX_LENGTH);
        onChange(value);
    }

    return (
        <div className="flex items-center">
            <span>+</span>
            <input
                id="numberPhone"
                key="numberPhone"
                type="text"
                {...register('mobilePhone.code', {
                    required: t('validation.required'),
                    onChange: handleInputChange
                })}
                className={`focus:outline-none focus:border-[var(--button-border)] h-[100%] w-[30px] min-w-[30px] ${
                    mobilePhoneErrors?.code ? 'border-[#E25757]' : ''
                }`}
                maxLength={PHONE_CODE_MAX_LENGTH}
            />
        </div>
    );
};

interface MobilePhoneInputProps {
    isRequired?: boolean;
    mobilePhone: MobilePhoneType;
    handleMobilePhoneChange: (mobilePhone: MobilePhoneType) => void;
}

export const MobilePhone = (props: MobilePhoneInputProps) => {
    const { t } = useTranslation('memberSite');
    const { mobilePhone, handleMobilePhoneChange, isRequired } = props;
    const { register, formState: { errors } } = useFormContext();
    const mobilePhoneErrors = errors.mobilePhone as MobilePhoneErrors | undefined;

    const handlePhoneChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const value = e.target.value.replace(NUMBER_REGEX, '').slice(0, PHONE_NUMBER_MAX_LENGTH);
        handleMobilePhoneChange({ phone: value, code: mobilePhone.code });
    };

    return (
        <div className="w-[230px] pt-[4px] max-[768px]:w-[48%]">
            <label className="block text-[12px] pt-[4px] mb-[8px] font-semibold max-[768px]:text-[16px] max-[768px]:leading-[16px] max-[768px]:mb-[8px]" htmlFor="mobile">
                {t('message.mobile')}
                {isRequired && <span className='text-[#E25757]'>*</span>}
            </label>
            <div className={`flex items-center h-[32px] max-[768px]:h-[40px] border-[1px] rounded-[4px] px-[8px] text-[12px] focus:outline-none focus:border-[var(--border-control-focus)] cursor-pointer ${
                mobilePhoneErrors ? 'border-[#E25757]' : 'border-[var(--border-control)]'
            }`}>
                <MobiCode code={mobilePhone.code} onChange={(code) => {
                    handleMobilePhoneChange({ phone: mobilePhone.phone, code: code });
                }} />
                <input
                    className={`focus:outline-none w-[100%] h-[100%] pl-[10px] border-l-[1px] border-[var(--border-control)] max-[768px]:text-[16px] max-[768px]:leading-[16px] ${
                        mobilePhoneErrors?.phone ? 'border-[#E25757]' : ''
                    }`}
                    id="mobile"
                    type="text"
                    {...register('mobilePhone.phone', {
                        required: isRequired ? t('validation.required') : false,
                        onChange: handlePhoneChange
                    })}
                    placeholder=""
                    maxLength={PHONE_NUMBER_MAX_LENGTH}
                />
            </div>
            {(mobilePhoneErrors?.code || mobilePhoneErrors?.phone) && (
                <p className="text-[#E25757] text-[12px] mt-[4px]">
                    {getErrorMessage(mobilePhoneErrors?.code || mobilePhoneErrors?.phone)}
                </p>
            )}
        </div>
    );
};
