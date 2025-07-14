import React from 'react';
import { IconInfo } from '../Icons';
import './TitleComponents.scss';
import { BaseTitleProps } from './types';

export const LabelTitleComponent: React.FC<BaseTitleProps> = ({
  text,
  required = false,
  className = '',
  isShowIcon = false
}) => {
  return (
    text && (
      <label className={`${className} block font-semibold text-[var(--titleComponent-label-color)] mb-[4px] capitalize`}>
        {text}
        {required && <span className="text-[#FF0000] dark:text-[#FF4F50] ml-[4px]">*</span>}
        {isShowIcon && <IconInfo className="w-[20px] cursor-pointer h-[20px] fill-[#163f78]" />}
      </label>
    )
  );
}; 