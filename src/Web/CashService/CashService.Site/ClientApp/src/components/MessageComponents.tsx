import React from 'react';

import { IconCheck, IconError, IconWarning } from './Icons';

export type ResultType = 'error' | 'success' | 'warning';

interface ResultMessageProps {
  type?: ResultType;
  isError?: boolean;
  isSuccess?: boolean;
  isWarning?: boolean;
  message: string | null;
  className?: string;
}

interface ValidateMessageProps {
  message: string;
}

export const ResultMessageComponent: React.FC<ResultMessageProps> = ({
  type,
  isError,
  isSuccess,
  isWarning,
  message,
  className = '',
}) => {
  if (isError || type === 'error') {
    return <ErrorMessageComponent message={message} className={className} />;
  }

  if (isWarning || type === 'warning') {
    return <WarningMessageComponent message={message} className={className} />;
  }

  if (isSuccess || type === 'success') {
    return <SuccessMessageComponent message={message} className={className} />;
  }

  return null;
};

export const ValidateMessageComponent: React.FC<ValidateMessageProps> = ({ message }) => {
  return message && <div className="mt-[2px] text-[#FE0000] dark:text-[#EB5757]">{message}</div>;
};

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
          <IconWarning className="w-[20px] h-[20px] fill-[#A75000] dark:fill-[#EB8526] absolute top-[7px] left-[10px] max-[768px]:top-[10px] max-[768px]:left-[12px]" />
          <div dangerouslySetInnerHTML={{ __html: message }}></div>
        </div>
      </div>
    )
  );
};
