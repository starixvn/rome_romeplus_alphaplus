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
      <IconError className="w-[20px] h-[20px] fill-[#D42020] absolute top-[8px] left-[10px]" />
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
    <div className={`${className} fixed top-0 left-0 w-full h-full inline-flex justify-center items-center pointer-events-none`} role="alert">
      <div className={`${getResultStyles(type)} font-bold text-[12px] text-[#333] border-[1px] rounded-[10px] pl-[36px] pr-[12px] py-[8px] left-[50%] transform-[translateX(-50%)] block capitalize absolute top-0 opacity-0 animate-[var(--animate-fade-in-scale)] `}>
        {getResultIcon(type)}         
        {message}
      </div>
    </div>
  );
}; 