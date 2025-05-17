import { MobilePhone as MobilePhoneType } from "../../../services/interfaces/MemberSites";
import { useTranslation } from "react-i18next";

interface MobiCodeProps {
    code: string;
    onChange: (code: string) => void;
}
const MobiCode = (props: MobiCodeProps) => {
    const { code, onChange } = props;
    const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        e.preventDefault();
        const value = e.target.value.replace(/[^0-9]/g, '').slice(0, 3);
        onChange(value);
    }

    return (
        <>
            <span>+</span>
            <input
                id="numberPhone"
                key="numberPhone"
                type="text"
                value={code}
                onChange={handleInputChange}
                className="focus:outline-none focus:border-[var(--button-border)] h-[100%] w-[30px] min-w-[30px]"
                maxLength={3}
                autoFocus
            />
        </>

    );
};

interface MobilePhoneInputProps {
    mobilePhone: MobilePhoneType;
    handleMobilePhoneChange: (mobilePhone: MobilePhoneType) => void;
}

export const MobilePhone = (props: MobilePhoneInputProps) => {
    const { t } = useTranslation('memberSite');
    const { mobilePhone, handleMobilePhoneChange } = props;

    return (
        <>
            <label className="block text-[12px] pt-[4px] mb-[8px] font-semibold max-[768px]:text-[16px] max-[768px]:leading-[16px] max-[768px]:mb-[8px]" htmlFor="mobile">
                {t('message.mobile')}
            </label>
            <div className="flex items-center h-[32px] max-[768px]:h-[40px]  border-[1px] border-[var(--border-control)] rounded-[4px] px-[8px] text-[12px] focus:outline-none focus:border-[var(--border-control-focus)] cursor-pointer">
                <MobiCode code={mobilePhone.code} onChange={(code) => {
                    handleMobilePhoneChange({ phone: mobilePhone.phone, code: code });
                }} />
                <input
                    className="focus:outline-none w-[100%] h-[100%] pl-[10px] border-l-[1px] border-[var(--border-control)] max-[768px]:text-[16px] max-[768px]:leading-[16px]"
                    id="mobile"
                    type="text"
                    value={mobilePhone.phone}
                    onChange={(e) => {
                        const value = e.target.value.replace(/[^0-9]/g, '');
                        handleMobilePhoneChange({ phone: value, code: mobilePhone.code });
                    }}
                    placeholder=""
                    maxLength={40}
                />
            </div>
        </>
    )
}
