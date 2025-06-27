import React from 'react';
import { ValidateMessageProps } from './types';

export const ValidateMessageComponent: React.FC<ValidateMessageProps> = ({ message, className = '' }) => {
  return message && <div className={`mt-[2px] text-[#FE0000] dark:text-[#EB5757] ${className}`}>{message}</div>;
}; 

export const ValidateMessageComponentV2: React.FC<ValidateMessageProps> = ({ message, className = '' }) => {
  return <div className={`text-[#FE0000] dark:text-[#EB5757] ${className}`}>{message}</div>;
};