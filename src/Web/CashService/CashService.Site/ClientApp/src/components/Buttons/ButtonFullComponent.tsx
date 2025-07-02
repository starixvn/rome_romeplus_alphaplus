import React from 'react';
import { ButtonComponentProps } from './types';
import './ButtonComponents.scss';

export const ButtonFullComponent: React.FC<ButtonComponentProps> = ({
  text,
  onClick,  
  type,
  className = '',
  disabled = false,
}) => {
  return (
    <div className={`${className}`}>
      <button
        type={type || 'button'}
        className={`min-w-[90px] bg-[var(--button-background)] hover:bg-[var(--button-background-hover)] text-[var(--button-text)] rounded-[3px] h-[28px] px-[16px] font-semibold outline-none pt-[1px]
        ${disabled ? 'cursor-not-allowed opacity-80' : 'cursor-pointer'}`}
        onClick={onClick}
        disabled={disabled}
      >
        {text}
      </button>
    </div>
  );
}; 