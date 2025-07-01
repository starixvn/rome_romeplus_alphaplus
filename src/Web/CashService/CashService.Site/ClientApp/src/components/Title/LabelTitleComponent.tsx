import React from 'react';
import { BaseTitleProps } from './types';
import './TitleComponents.scss';

export const LabelTitleComponent: React.FC<BaseTitleProps> = ({
  text,
  required = false,
  className = '',
}) => {
  return (
    text && (
      <label className={`${className} block font-semibold text-[var(--titleComponent-label-color)] mb-[4px] capitalize`}>
        {text}
        {required && <span className="text-[#FF0000] dark:text-[#FF4F50] ml-[4px]">*</span>}
      </label>
    )
  );
}; 