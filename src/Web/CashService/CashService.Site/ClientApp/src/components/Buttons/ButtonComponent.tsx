import React from 'react';
import { ButtonComponentProps } from './types';
import './ButtonComponents.scss';

export const ButtonComponent: React.FC<ButtonComponentProps> = ({
  text,
  onClick,
  disabled = false,
  type = 'button',
  className = '',
  title = '',
  variant = 'apply',
}) => {
  const getButtonClasses = () => {
    const baseClasses = 'min-w-[100px] rounded-[3px] h-[28px] pt-[1px] px-[10px] font-semibold outline-none';
    const disabledClasses = disabled ? 'cursor-not-allowed opacity-80' : 'cursor-pointer';
    
    switch (variant) {
      case 'apply':
        return `${baseClasses} text-[var(--button-text)] bg-[var(--button-background)] hover:bg-[var(--button-background-hover)] ${disabledClasses}`;
      case 'cancel':
        return `${baseClasses} bg-[var(--button-background-cancel)] hover:bg-[var(--button-background-cancel-hover)] text-[var(--button-text)] ${disabledClasses}`;
      case 'full':
        return `${baseClasses} bg-[var(--button-background)] hover:bg-[var(--button-background-hover)] text-[var(--button-text)] ${disabledClasses}`;
      case 'outline':
        return `${baseClasses} border-[1px] border-[var(--button-outline-border)] bg-[var(--button-outline-background)] hover:bg-[var(--button-outline-background-hover)] hover:text-[var(--button-outline-text-hover)] text-[var(--button-outline-text)] ${disabledClasses}`;
      case 'reject':
        const rejectClasses = disabled 
          ? 'cursor-not-allowed opacity-80 bg-[var(--button-background-reject-hover)]' 
          : 'cursor-pointer bg-[var(--button-background-reject)] hover:bg-[var(--button-background-reject-hover)]';
        return `${baseClasses} text-[var(--button-text)] ${rejectClasses}`;
      default:
        return `${baseClasses} text-[var(--button-text)] bg-[var(--button-background)] hover:bg-[var(--button-background-hover)] ${disabledClasses}`;
    }
  };

  return (
    <div className={`${className}`}>
      <button
        type={type}
        className={getButtonClasses()}
        onClick={onClick}
        disabled={disabled}
        title={title}
      >
        {text}
      </button>
    </div>
  );
}; 