import React from 'react';
import { useTranslation } from 'react-i18next';
import { ButtonComponentProps } from './types';
import './ButtonComponents.scss';

interface ToggleButtonProps extends Omit<ButtonComponentProps, 'text'> {
  text: string;
  position?: 'left' | 'right';
  isActive?: boolean;
}

// Base toggle button component
const ToggleButton = React.forwardRef<HTMLButtonElement, ToggleButtonProps>(({ 
  isActive = false, 
  onClick, 
  type = 'button', 
  text, 
  position = 'left',
  className = '',
  disabled = false,
  ...props
}, ref) => {
  const baseClasses = 'w-[90px] p-[4px] outline-none font-semibold';
  const activeClasses = 'bg-[var(--button-switch-toggle-background-active)] text-[var(--button-switch-toggle-color-active)]';
  const inactiveClasses = 'bg-[var(--button-switch-toggle-background)] text-[var(--button-switch-toggle-color)]';
  const disabledClasses = disabled ? 'cursor-not-allowed opacity-80' : 'cursor-pointer';
  
  const borderClasses = position === 'left'
    ? 'rounded-l-[4px]' 
    : 'rounded-r-[4px]';
  
  const buttonClasses = `${baseClasses} ${borderClasses} ${isActive ? activeClasses : inactiveClasses} ${disabledClasses} ${className}`;

  return (
    <button
      ref={ref}
      type={type}
      className={buttonClasses}
      onClick={onClick}
      disabled={disabled}
      aria-pressed={isActive}
      {...props}
    >
      {text}
    </button>
  );
});

ToggleButton.displayName = 'ToggleButton';

// Enable button component
export const EnableButtonComponent = React.forwardRef<HTMLButtonElement, ButtonComponentProps>((props, ref) => {
  const { t: tGlobal } = useTranslation('global');
  const text = props.text || tGlobal('enable');
  return <ToggleButton {...props} ref={ref} text={text} position="left" />;
});

EnableButtonComponent.displayName = 'EnableButtonComponent';

// Disable button component
export const DisableButtonComponent = React.forwardRef<HTMLButtonElement, ButtonComponentProps>((props, ref) => {
  const { t: tGlobal } = useTranslation('global');
  const text = props.text || tGlobal('disable');
  return <ToggleButton {...props} ref={ref} text={text} position="right" />;
});

DisableButtonComponent.displayName = 'DisableButtonComponent';