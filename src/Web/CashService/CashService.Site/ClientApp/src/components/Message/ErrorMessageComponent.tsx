import React from 'react';
import { IconError } from '../Icons';
import { ResultMessageProps } from './types';

export const ErrorMessageComponent: React.FC<ResultMessageProps> = ({
  message,
  className = '',
}) => {
  return (
    message && (
      <div className={`${className} max-[768px]:py-[12px]`} role="alert">
        <div
          className={`bg-[#FFE8E8] dark:bg-[#501B1B] border-[#E2B6B6] dark:border-[#FF8C8C] relative font-[500] border-[1px] rounded-[3px] pl-[35px] pr-[7px] py-[8px] leading-[18px]`}
        >
          <IconError className="w-[20px] h-[20px] fill-[#CC0000] dark:fill-[#FF6D6D] absolute top-[7px] left-[10px]" />
          <div dangerouslySetInnerHTML={{ __html: message }}></div>
        </div>
      </div>
    )
  );
}; 