import React from 'react';
import { IconWarning } from '../Icons';
import { ResultMessageProps } from './types';

export const WarningMessageComponent: React.FC<ResultMessageProps> = ({
  message,
  className = '',
}) => {
  return (
    message && (
      <div className={`${className} max-[768px]:py-[12px]`} role="alert">
        <div
          className={`bg-[#FFFFFA] dark:bg-[#312F26] border-[#C6A180] dark:border-[#78776D] relative font-[500] border-[1px] rounded-[3px] pl-[35px] pr-[7px] py-[8px] leading-[18px]`}
        >
          <IconWarning className="w-[20px] h-[20px] fill-[#A75000] dark:fill-[#EB8526] absolute top-[7px] left-[10px]" />
          <div dangerouslySetInnerHTML={{ __html: message }}></div>
        </div>
      </div>
    )
  );
}; 