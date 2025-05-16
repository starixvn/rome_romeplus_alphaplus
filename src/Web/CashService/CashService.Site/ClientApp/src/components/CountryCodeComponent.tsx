import React from 'react';

interface CountryCodeComponentProps {
  label: string;
  countryCode: string;
  mobile: string;
  countryCodes: string[];
  fieldErrors: {
    mobile?: string;
    countryCode?: string;
  };
  onCountryCodeChange: (e: React.ChangeEvent<HTMLSelectElement>) => void;
  onMobileChange: (e: React.ChangeEvent<HTMLInputElement>) => void;
  placeholder: string;
}

const MobileInputComponent: React.FC<CountryCodeComponentProps> = ({
  label,
  countryCode,
  mobile,
  countryCodes,
  fieldErrors,
  onCountryCodeChange,
  onMobileChange,
  placeholder
}) => {
  return (
    <div className="mb-[8px]">
      <label className="block text-[12px] font-semibold text-[#333333] my-[8px]">{label}<span className='text-[#E25757]'>*</span></label>
      <div className="flex items-center h-[32px] max-[768px]:h-[40px] w-[470px] border-[1px] border-[#0000003D] rounded-[4px] px-[8px] text-[12px] focus:outline-none focus:border-[var(--button-border)] cursor-pointer">
        <select
          className={`focus:outline-none focus:border-[var(--button-border)] h-[100%] cursor-pointer mr-[5px] ${fieldErrors.countryCode ? 'border-[#FE0000]' : ''}`}
          value={countryCode}
          onChange={onCountryCodeChange}
          style={{ maxWidth: '120px' }}
        >
          {countryCodes.map((code) => (
            <option key={code} value={code} selected={code === countryCode}>
              +{code}
            </option>
          ))}                
        </select>
        <input
          type="text"
          className={`focus:outline-none w-[100%] h-[100%] pl-[10px] border-l-[1px] ${fieldErrors.mobile ? 'border-[#FE0000]' : 'border-[#0000003D]'}`}
          value={mobile}
          onChange={onMobileChange}
          placeholder={placeholder}
        />
      </div>
      {(fieldErrors.mobile || fieldErrors.countryCode) && (
        <div className="text-[#FE0000] text-[11px] mt-1">{fieldErrors.mobile || fieldErrors.countryCode}</div>
      )}
    </div>
  );
};

export default MobileInputComponent; 