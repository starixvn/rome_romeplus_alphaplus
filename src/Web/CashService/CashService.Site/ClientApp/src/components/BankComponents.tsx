import React from 'react';

export const BankIcon: React.FC<any> = ({ code, alterText }) => {
  const upperCaseCode = code.toUpperCase();
  console.log('upperCaseCode', upperCaseCode);
  return code && (
    <img
      src={`../assets/bank/${upperCaseCode}.png`}
      alt={`${alterText || upperCaseCode} bank icon`}
      className={`w-[16px] h-[16px] mr-[8px] inline-block`}
    />
  );
};

export default BankIcon; 