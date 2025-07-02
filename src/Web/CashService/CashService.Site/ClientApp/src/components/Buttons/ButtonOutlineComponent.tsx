import React from 'react';
import { ButtonComponentProps } from './types';
import './ButtonComponents.scss';

export const ButtonOutlineComponent: React.FC<ButtonComponentProps> = ({
  text,
  onClick,
  type,
  className = '',
}) => {
  return (
    <div className={`${className}`}>
      <button
        type={type || 'button'}
        className="border-[1px] border-[var(--button-outline-border)] bg-[var(--button-outline-background)]  hover:bg-[var(--button-outline-background-hover)] hover:text-[var(--button-outline-text-hover)] text-[var(--button-outline-text)] rounded-[5px] cursor-pointer h-[28px] px-[16px] font-semibold outline-none pt-[1px]"
        onClick={onClick}
      >
        {text}
      </button>
    </div>
  );
}; 