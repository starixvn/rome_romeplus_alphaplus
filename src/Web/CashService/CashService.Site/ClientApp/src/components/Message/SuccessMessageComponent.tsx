import React from 'react';
import { IconCheck } from '../Icons';
import { ResultMessageProps } from './types';

export const SuccessMessageComponent: React.FC<ResultMessageProps> = ({
  message,
  className = '',
}) => {
  return (
    message && (
      <div className={`${className} max-[768px]:py-[12px]`} role="alert">
        <div
          className={`bg-[#E2FFF2] dark:bg-[#3A5031] border-[#5CB774] dark:border-[#7ECC4D] relative font-[500] border-[1px] rounded-[3px] pl-[35px] pr-[7px] py-[8px] leading-[18px]`}
        >
          <IconCheck className="w-[20px] h-[20px] fill-[#268026] dark:fill-[#7ECC4D] absolute top-[7px] left-[10px]" />
          <div dangerouslySetInnerHTML={{ __html: message }}></div>
        </div>
      </div>
    )
  );
}; 