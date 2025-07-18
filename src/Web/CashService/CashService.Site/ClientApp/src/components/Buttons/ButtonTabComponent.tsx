import React from 'react';
import { ButtonComponentProps } from './types';
import './ButtonComponents.scss';

// Base tab button component to reduce duplication
const BaseTabButton = React.forwardRef<HTMLButtonElement, ButtonComponentProps & { isActive: boolean }>(({
  text,
  onClick,
  type = 'button',
  isActive,
  className = '',
  ...props
}, ref) => {
  const baseClasses = 'font-semibold px-[20px] pt-[10px] pb-[13px] cursor-pointer relative after:content-[\'\'] after:absolute after:bottom-0 after:left-0 after:h-[3px] after:bg-[var(--tab-component-border-color)]';
  
  const activeClasses = 'text-[var(--tab-component-color-active)] after:w-full';
  const inactiveClasses = 'text-[var(--tab-component-color)] after:w-0 after:transition-all after:duration-500 after:ease-in-out after:opacity-0 hover:after:opacity-100 hover:after:w-full hover:text-[var(--tab-component-color-active)]';
  
  const classes = `${baseClasses} ${isActive ? activeClasses : inactiveClasses} ${className}`;
  
  return (
    <button
      ref={ref}
      type={type}
      className={classes}
      onClick={onClick}
      {...props}
    >
      {text}
    </button>
  );
});

BaseTabButton.displayName = 'BaseTabButton';

// Active tab component
export const ButtonTabActiveComponent = React.forwardRef<HTMLButtonElement, ButtonComponentProps>((props, ref) => (
  <BaseTabButton {...props} ref={ref} isActive={true} />
));

ButtonTabActiveComponent.displayName = 'ButtonTabActiveComponent';

// Inactive tab component
export const ButtonTabInactiveComponent = React.forwardRef<HTMLButtonElement, ButtonComponentProps>((props, ref) => (
  <BaseTabButton {...props} ref={ref} isActive={false} />
));

ButtonTabInactiveComponent.displayName = 'ButtonTabInactiveComponent';

// Unified tab component that switches based on isActive prop
export const ButtonTabComponent = React.forwardRef<HTMLButtonElement, ButtonComponentProps>(({
  text,
  onClick,
  isActive,
  type,
  ...props
}, ref) => {
  return isActive ? (    
    <ButtonTabActiveComponent 
      ref={ref}
      text={text} 
      onClick={onClick} 
      type={type} 
      {...props}
    />
  ) : (
    <ButtonTabInactiveComponent 
      ref={ref}
      text={text} 
      onClick={onClick} 
      type={type} 
      {...props}
    />
  );
});

ButtonTabComponent.displayName = 'ButtonTabComponent';
