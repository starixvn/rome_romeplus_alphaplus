import React from 'react';
import { ButtonComponentProps } from './types';
import './ButtonComponents.scss';

export const ButtonCancelComponent: React.FC<ButtonComponentProps> = ({
  text,
  onClick,
  type = 'button',
  className = '',
  disabled = false,
}) => {
  return (
    <div className={`${className}`}>
      <button
        type={type || 'button'}
        className="min-w-[90px] bg-[var(--button-background-cancel)] hover:bg-[var(--button-background-cancel-hover)] text-[var(--button-text)] rounded-[3px] cursor-pointer h-[28px] pt-[1px] px-[16px] font-semibold outline-none "
        onClick={onClick}
        disabled={disabled}
      >
        {text}
      </button>
    </div>
  );
}; 