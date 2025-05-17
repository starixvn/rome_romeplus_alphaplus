import React from 'react';
import { IconCheck, IconError, IconWarning } from './Icons';

export type ResultType = 'error' | 'success' | 'warning';

interface ResultMessageProps {
  type: ResultType;
  message: string;
  className?: string;
}

interface ValidateMessageProps {
  message: string;
}

const getResultStyles = (type: ResultType) => {
  const styles = {
    error: ' bg-[#FFF2F2] border-[#FE0000] ',
    success: 'bg-[#E2FFF2] border-[#5CB774] ',
    warning: 'bg-[#F6F2D7] border-[#FFDD9C] '
  };
  return styles[type];
};


const getResultIcon = (type: ResultType) => {
  switch (type) {
    case 'error':
      return (
        <IconError className="w-[20px] h-[20px] fill-[#D42020] absolute top-[7px] left-[10px] max-[768px]:top-[10px] max-[768px]:left-[12px]" />
      );
    case 'warning':
      return (
        <IconWarning className="w-[20px] h-[20px] fill-[#7F3D1D] absolute top-[7px] left-[10px] max-[768px]:top-[10px] max-[768px]:left-[12px]" />
      );
    default:
      return (
        <IconCheck className="w-[20px] h-[20px] fill-[#5CB774] absolute top-[7px] left-[10px] max-[768px]:top-[10px] max-[768px]:left-[12px]" />
      );
  }
};

export const ResultMessageComponent: React.FC<ResultMessageProps> = ({
  type,
  message,
  className = ''
}) => {
  return (
    <div className={`${className} max-[768px]:py-[12px]`} role="alert">
      <div className={`${getResultStyles(type)} relative font-bold text-[12px] text-[#333] border-[1px] rounded-[10px] pl-[36px] pr-[12px] py-[8px] leading-[18px] block capitalize max-[768px]:text-[16px] max-[768px]:leading-[16px] max-[768px]:pl-[40px] max-[768px]:pr-[12px] max-[768px]:py-[12px]`}>
        {getResultIcon(type)}         
        <div dangerouslySetInnerHTML={{ __html: message }}></div>
      </div>
    </div>
  );
};

export const ValidateMessageComponent: React.FC<ValidateMessageProps> = ({
  message,
}) => {
  return (
    <div className="text-[#FE0000]">{message}</div>
  );
};
