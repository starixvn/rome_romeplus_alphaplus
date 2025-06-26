import React from 'react';
import { ValidateMessageProps } from './types';

export const ValidateMessageComponent: React.FC<ValidateMessageProps> = ({ message }) => {
  return message && <div className="mt-[2px] text-[#FE0000] dark:text-[#EB5757]">{message}</div>;
}; 