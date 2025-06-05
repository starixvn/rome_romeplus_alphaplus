import React from 'react';

import { ensureIntegerInput } from '../utils/inputHandlers';
import { ValidateMessageComponent } from './MessageComponents';
import { LabelTitleComponent } from './TitleComponents';

interface MobileComponentProps {
  isShow: boolean;
  label: string;
  countryCode: string;
  mobile: string;
  fieldErrors: {
    mobile?: string;
    countryCode?: string;
  };
  onCountryCodeChange: (e: React.ChangeEvent<HTMLInputElement>) => void;
  onMobileChange: (e: React.ChangeEvent<HTMLInputElement>) => void;
  placeholder: string;
}

const MobileComponent: React.FC<MobileComponentProps> = ({
  isShow,
  label,
  mobile,
  countryCode,
  fieldErrors,
  onCountryCodeChange,
  onMobileChange,
  placeholder,
}) => {
  return (
    isShow && (
      <div className="mb-[8px]">
        <LabelTitleComponent text={label} required={true} />
        <div className="flex items-center h-[32px] max-[768px]:h-[40px] w-[470px] border-[1px] border-[var(--border-control)] rounded-[3px] px-[8px] focus:outline-none focus:border-[var(--button-border)] cursor-pointer before:content-['+']">
          <input
            type="text"
            className={`focus:outline-none h-[100%] cursor-pointer mr-[5px] w-[30px]  ${fieldErrors.countryCode ? 'border-[var(--border-control-error)]' : ''}`}
            value={countryCode}
            onChange={(e) => onCountryCodeChange(ensureIntegerInput(e))}
            maxLength={3}
            style={{ maxWidth: '120px' }}
            inputMode="numeric"
          />
          <input
            type="text"
            className={`focus:outline-none w-[100%] h-[100%] pl-[10px] border-l-[1px] ${fieldErrors.mobile ? 'border-[var(--border-control-error)]' : 'border-[var(--border-control)]'}`}
            value={mobile}
            onChange={(e) => onMobileChange(ensureIntegerInput(e))}
            placeholder={placeholder}
            maxLength={40}
          />
        </div>
        <ValidateMessageComponent message={(fieldErrors.mobile || fieldErrors.countryCode) ?? ''} />
      </div>
    )
  );
};

export default MobileComponent;
