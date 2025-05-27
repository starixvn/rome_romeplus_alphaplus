import React from 'react';

export const BankIcon: React.FC<any> = ({ code, alterText }) => {
  const upperCaseCode = code.toUpperCase();
  return (
    code && (
      <img
        src={`../assets/bank/${upperCaseCode}.png`}
        alt={`${alterText || upperCaseCode} bank icon`}
        className={`w-[16px] h-[16px] mr-[8px] inline-block`}
      />
    )
  );
};

export default BankIcon;
