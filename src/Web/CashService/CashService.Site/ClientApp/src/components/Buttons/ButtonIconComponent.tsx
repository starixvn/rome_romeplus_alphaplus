import React from 'react';
import { useTranslation } from 'react-i18next';
import { IconBack, IconClose, IconCopy, IconDelete, IconExcel, IconPen } from '../Icons';
import { ButtonIconComponentProps } from './types';
import './ButtonComponents.scss';

// Base icon button component
const BaseIconButton = React.forwardRef<HTMLButtonElement, ButtonIconComponentProps & {
  icon: React.ReactNode;
  baseClassName?: string;
  ariaLabel?: string;
}>(({
  onClick,
  disabled,
  type = 'button',
  icon,
  baseClassName = '',
  ariaLabel,
  className,
  ...props
}, ref) => {
  const buttonClasses = [
    'cursor-pointer',
    disabled ? 'cursor-not-allowed opacity-80' : 'hover:bg-[var(--button-icon-hover)]',
    baseClassName,
    className
  ].filter(Boolean).join(' ');

  return (
    <button
      ref={ref}
      type={type}
      className={buttonClasses}
      onClick={onClick}
      disabled={disabled}
      aria-label={ariaLabel}
      {...props}
    >
      {icon}
    </button>
  );
});

BaseIconButton.displayName = 'BaseIconButton';

// Icon button factory
const createIconButton = (
  Icon: React.ComponentType<any>,
  config: {
    baseClassName?: string;
    ariaLabel: string;
  }
) => {
  return React.forwardRef<HTMLButtonElement, ButtonIconComponentProps>((props , ref) => (
    <BaseIconButton
      ref={ref}
      {...props}
     
      icon={<Icon />}
      baseClassName={config.baseClassName}
      ariaLabel={config.ariaLabel}
    />
  ));
};

// Icon button components
export const ButtonClosePopupComponent = createIconButton(IconClose, {  
  baseClassName: "absolute top-[11px] right-[-5px] cursor-pointer outline-none ",
  ariaLabel: "Close"
});

// Icon button components
export const ButtonPenComponent = createIconButton(IconPen, {  
  baseClassName: "mx-[4px] p-[4px] rounded-[4px]",
  ariaLabel: "Edit"
});

export const ButtonDeleteComponent = createIconButton(IconDelete, { 
  baseClassName: "mx-[4px] py-[4px] px-[6px] rounded-[4px]",
  ariaLabel: "Delete"
});

export const ButtonExcelComponent = createIconButton(IconExcel, {
  baseClassName: "p-[4px] bg-black/10 rounded-[3px]",
  ariaLabel: "Export to Excel"
});

export const ButtonCopyComponent = createIconButton(IconCopy, {
  baseClassName: "p-[4px] bg-black/10 rounded-[3px]",
  ariaLabel: "Copy"
});

// Special back button component with text
export const ButtonBackComponent = React.forwardRef<HTMLButtonElement, ButtonIconComponentProps>(({ 
  onClick, 
  disabled, 
  className = '',
  type = 'button',
  ...props 
}, ref) => {
  const { t: tGlobal } = useTranslation('global');
  
  const buttonClasses = [
    'min-w-[90px] text-[var(--button-back-text)] hover:text-[var(--button-back-text-hover)] border-[1px] border-[var(--button-back-border)] rounded-[20px] mr-[16px] bg-[var(--button-back-background)] hover:bg-[var(--button-back-background-hover)] hover:border-[transparent] group mb-[8px] inline-flex items-center py-[4px] pl-[8px] pr-[16px]',
    disabled ? 'cursor-not-allowed opacity-80' : 'cursor-pointer',
    className
  ].filter(Boolean).join(' ');

  return (
    <button
      ref={ref}
      type={type}
      className={buttonClasses}
      onClick={onClick}
      disabled={disabled}
      aria-label={tGlobal('back')}
      {...props}
    >
      <IconBack className="w-[18px] h-[18px] fill-[var(--button-back-text)] mr-[8px] group-hover:fill-[var(--button-back-text-hover)]" />
      <span className="font-semibold">{tGlobal('back')}</span>
    </button>
  );
});

ButtonBackComponent.displayName = 'ButtonBackComponent';