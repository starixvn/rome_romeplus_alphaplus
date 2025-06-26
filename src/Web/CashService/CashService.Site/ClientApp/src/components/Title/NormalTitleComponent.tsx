import React from 'react';
import { BaseTitleProps } from './types';
import './TitleComponents.scss';

export const NormalTitleComponent: React.FC<BaseTitleProps> = ({
  text,
  className = '',
}) => {
  return (
    text && (
      <h3
        className={`${className} py-[8px] text-[#094980] dark:text-[#459be5] font-semibold capitalize`}
      >
        {text}
      </h3>
    )
  );
}; 