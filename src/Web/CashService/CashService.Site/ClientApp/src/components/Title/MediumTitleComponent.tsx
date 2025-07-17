import './TitleComponents.scss';

import React from 'react';

import { IconInfo } from '../Icons';
import { BaseTitleProps } from './types';

export const MediumTitleComponent: React.FC<BaseTitleProps> = ({
  text,
  required = false,
  className = '',
  isShowIcon
}) => {
  return (
    text && (
      <h3
        className={`${className} w-[100%] font-semibold text-[14px] text-[#094980] dark:text-[#459be5] capitalize`}
      >
        {text || '\u00A0'}
        {required && <span className="text-[#FF0000] dark:text-[#FF4F50] ml-[4px]">*</span>}
        {isShowIcon && <IconInfo className="w-[20px] cursor-pointer h-[20px] fill-[#163f78]" />}
      </h3>
    )
  );
};
