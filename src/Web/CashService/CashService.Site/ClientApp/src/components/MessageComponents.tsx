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
  className = ''
}) => {
  if (isError || type === 'error') {
    return <ErrorMessageComponent message={message} className={className} />;
  }

  if (isSuccess || type === 'success') {
    return <SuccessMessageComponent message={message} className={className} />;
  }

  if (isWarning || type === 'warning') {
    return <WarningMessageComponent message={message} className={className} />;
  }

  return null;
};

export const ValidateMessageComponent: React.FC<ValidateMessageProps> = ({
  message,
}) => {
  return message && (<div className="text-[#FE0000]">{message}</div>);
};

export const ErrorMessageComponent: React.FC<ResultMessageProps> = ({
  message,
  className = ''
}) => {
  return message && (
    <div className={`${className} max-[768px]:py-[12px]`} role="alert">
      <div className={`bg-[#FFF2F2] border-[#FE0000] relative font-bold text-[12px] text-[#333] border-[1px] rounded-[10px] pl-[36px] pr-[12px] py-[8px] leading-[18px] block max-[768px]:text-[16px] max-[768px]:leading-[16px] max-[768px]:pl-[40px] max-[768px]:pr-[12px] max-[768px]:py-[12px]`}>
        <IconError className="w-[20px] h-[20px] fill-[#D42020] absolute top-[7px] left-[10px] max-[768px]:top-[10px] max-[768px]:left-[12px]" />
        <div dangerouslySetInnerHTML={{ __html: message }}></div>
      </div>
    </div>
  );
};

export const SuccessMessageComponent: React.FC<ResultMessageProps> = ({
  message,
  className = ''
}) => {
  return message &&  (
    <div className={`${className} max-[768px]:py-[12px]`} role="alert">
      <div className={`bg-[#E2FFF2] border-[#5CB774] relative font-bold text-[12px] text-[#333] border-[1px] rounded-[10px] pl-[36px] pr-[12px] py-[8px] leading-[18px] block max-[768px]:text-[16px] max-[768px]:leading-[16px] max-[768px]:pl-[40px] max-[768px]:pr-[12px] max-[768px]:py-[12px]`}>
        <IconCheck className="w-[20px] h-[20px] fill-[#5CB774] absolute top-[7px] left-[10px] max-[768px]:top-[10px] max-[768px]:left-[12px]" />
        <div dangerouslySetInnerHTML={{ __html: message }}></div>
      </div>
    </div>
  );
};

export const WarningMessageComponent: React.FC<ResultMessageProps> = ({
  message,
  className = ''  
}) => {
  return message &&  (    
    <div className={`${className} max-[768px]:py-[12px]`} role="alert">
      <div className={`bg-[#F6F2D7] border-[#FFDD9C] relative font-bold text-[12px] text-[#333] border-[1px] rounded-[10px] pl-[36px] pr-[12px] py-[8px] leading-[18px] block max-[768px]:text-[16px] max-[768px]:leading-[16px] max-[768px]:pl-[40px] max-[768px]:pr-[12px] max-[768px]:py-[12px]`}>
        <IconWarning className="w-[20px] h-[20px] fill-[#7F3D1D] absolute top-[7px] left-[10px] max-[768px]:top-[10px] max-[768px]:left-[12px]" />
        <div dangerouslySetInnerHTML={{ __html: message }}></div>
      </div>
    </div>
  );
};
