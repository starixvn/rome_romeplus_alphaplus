import { forwardRef } from 'react';
import { ButtonComponentProps, ButtonVariant } from './types';
import './ButtonComponents.scss';

const BUTTON_VARIANTS: Record<ButtonVariant, string> = {
  apply: 'text-[var(--button-text)] bg-[var(--button-background)] hover:bg-[var(--button-background-hover)]',
  cancel: 'bg-[var(--button-background-cancel)] hover:bg-[var(--button-background-cancel-hover)] text-[var(--button-text)]',
  full: 'bg-[var(--button-background)] hover:bg-[var(--button-background-hover)] text-[var(--button-text)]',
  outline: 'border-[1px] border-[var(--button-outline-border)] bg-[var(--button-outline-background)] hover:bg-[var(--button-outline-background-hover)] hover:text-[var(--button-outline-text-hover)] text-[var(--button-outline-text)]',
  reject: 'text-[var(--button-text)] bg-[var(--button-background-reject)] hover:bg-[var(--button-background-reject-hover)]'
};

export const ButtonComponent = forwardRef<HTMLButtonElement, ButtonComponentProps>(({
  text,
  onClick,
  disabled = false,
  type = 'button',
  className = '',
  title = '',
  variant = 'apply' as ButtonVariant, 
  ...props
}, ref) => {
  const baseClasses = 'min-w-[100px] rounded-[3px] h-[28px] pt-[1px] px-[10px] font-semibold outline-none';
  const variantClasses = BUTTON_VARIANTS[variant as ButtonVariant] || BUTTON_VARIANTS.apply;
  const disabledClasses = disabled ? 'cursor-not-allowed opacity-80' : 'cursor-pointer';

  return (
    <button
      ref={ref}
      type={type}
      className={`${baseClasses} ${variantClasses} ${disabledClasses} ${className}`}
      onClick={onClick}
      disabled={disabled}
      title={title}
      aria-label={title}
      {...props}
    >
      {text}
    </button>
  );
});

ButtonComponent.displayName = 'ButtonComponent'; 