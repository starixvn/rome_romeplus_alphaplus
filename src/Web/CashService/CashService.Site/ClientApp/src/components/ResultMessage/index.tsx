import React from 'react';
import { IconCheck, IconError } from '../Icons';

export type ResultType = 'error' | 'success';

interface ResultMessageProps {
  type: ResultType;
  message: string;
  className?: string;
}

const getResultStyles = (type: ResultType) => {
  const styles = {
    error: ' bg-[#FFF2F2] border-[#FE0000] ',
    success: 'bg-[#E2FFF2] border-[#5CB774] '
  };
  return styles[type];
};


const getResultIcon = (type: ResultType) => {
  if (type === 'error') {
    return (
      <IconError className="w-[20px] h-[20px] fill-[#D42020] absolute top-[8px] left-[10px] max-[768px]:top-[6px]" />
    );
  }
  return (
      <IconCheck className="w-[20px] h-[20px] fill-[#5CB774] absolute top-[8px] left-[10px]" />
  );
};

export const ResultMessage: React.FC<ResultMessageProps> = ({
  type,
  message,
  className = ''
}) => {
  return (
    <div className={`${className} `} role="alert">
      <div className={`${getResultStyles(type)} relative font-bold text-[12px] text-[#333] border-[1px] rounded-[10px] pl-[36px] pr-[12px] py-[8px] leading-[18px] block capitalize max-[768px]:text-[16px] max-[768px]:leading-[16px] max-[768px]:pl-[32px] max-[768px]:pr-[12px] max-[768px]:py-[8px]`}>
        {getResultIcon(type)}         
        <div dangerouslySetInnerHTML={{ __html: message }}></div>
      </div>
    </div>
  );
}; 