import React from 'react';
import './TitleComponents.scss';
import { BaseTitleProps } from './types';

export const MediumTitleComponent: React.FC<BaseTitleProps> = ({
  text,
  required = false,
  className = '',
}) => {
  return (
    text && (
      <h3
        className={`${className} w-[100%] font-semibold text-[14px] text-[#094980] dark:text-[#459be5] capitalize`}
      >
        {text ? text : '\u00A0'}
        {required && <span className="text-[#FF0000] dark:text-[#FF4F50] ml-[4px]">*</span>}
      </h3>
    )
  );
}; 