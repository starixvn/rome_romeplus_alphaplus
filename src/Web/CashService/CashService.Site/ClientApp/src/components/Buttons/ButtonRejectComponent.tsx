import React from 'react';
import { ButtonComponentProps } from './types';
import './ButtonComponents.scss';

export const ButtonRejectComponent: React.FC<ButtonComponentProps> = ({
  text,
  onClick,
  type,
  className = '',
  disabled = false,
  title = '',
}) => {
  return (
    <div className={`${className}`}>
      <button
        type={type || 'button'}
        className={`min-w-[90px] text-[var(--button-text)] rounded-[3px] h-[28px] px-[16px] font-semibold outline-none
        ${disabled ? 'cursor-not-allowed opacity-80 bg-[var(--button-background-reject-hover)]' : 'cursor-pointer bg-[var(--button-background-reject)] hover:bg-[var(--button-background-reject-hover)]'}`}
        onClick={onClick}
        disabled={disabled}
        title={title}
      >
        {text}
      </button>
    </div>
  );
};
