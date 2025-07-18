import React from 'react';
import { IconClose, IconCopy, IconDelete, IconExcel, IconPen } from '../Icons';
import { ButtonIconComponentProps } from './types';
import './ButtonComponents.scss';

// Base icon button component
const BaseIconButton: React.FC<ButtonIconComponentProps & {
  icon: React.ReactNode;
  baseClassName?: string;
  ariaLabel?: string;
}> = ({
  onClick,
  disabled,
  type = 'button',
  icon,
  baseClassName = '',
  ariaLabel,
  className,
  ...props
}) => {
  const buttonClasses = [
    'cursor-pointer',
    disabled ? 'cursor-not-allowed opacity-80' : 'hover:bg-[var(--button-icon-hover)]',
    baseClassName,
    className
  ].filter(Boolean).join(' ');

  return (
    <button
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
};

// Icon button factory
const createIconButton = (
  Icon: React.ComponentType<any>,
  config: {
    baseClassName?: string;
    ariaLabel: string;
  }
) => {
  return React.forwardRef<HTMLButtonElement, ButtonIconComponentProps>((props) => (
    <BaseIconButton
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