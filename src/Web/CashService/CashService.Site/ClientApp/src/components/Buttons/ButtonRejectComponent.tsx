import React from 'react';
import { ButtonComponentProps } from './types';
import './ButtonComponents.scss';

export const ButtonRejectComponent: React.FC<ButtonComponentProps> = ({
  text,
  onClick,
  type = 'button',
  className = '',
  disabled = false,
  title = '',
}) => {
  return (
    <div className={`${className}`}>
      <button
        type={type}
        className={`min-w-[90px] bg-[var(--button-background-reject)] hover:bg-[var(--button-background-reject-hover)] text-[var(--button-text)] rounded-[3px] cursor-pointer h-[28px] px-[16px] font-semibold outline-none
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
