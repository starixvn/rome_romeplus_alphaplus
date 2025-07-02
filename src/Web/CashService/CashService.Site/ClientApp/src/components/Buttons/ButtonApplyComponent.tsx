import React from 'react';
import { ButtonComponentProps } from './types';
import './ButtonComponents.scss';

export const ButtonApplyComponent: React.FC<ButtonComponentProps> = ({
  text,
  onClick,
  disabled = false,
  type,
  className = '',
  title = '',
}) => {
  return (
    <div className={`${className}`}>
      <button
        type={type || 'button'}
        className={`min-w-[90px] text-[var(--button-text)] bg-[var(--button-background)] hover:bg-[var(--button-background-hover)] rounded-[3px] h-[28px] pt-[1px] px-[16px] font-semibold outline-none
        ${disabled ? 'cursor-not-allowed opacity-80' : 'cursor-pointer'}`}
        onClick={onClick}
        disabled={disabled}
        title={title}
      >
        {text}
      </button>
    </div>
  );
}; 