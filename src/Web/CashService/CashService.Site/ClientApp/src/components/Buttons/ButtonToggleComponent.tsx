import React from 'react';
import { useTranslation } from 'react-i18next';
import { ButtonComponentProps } from './types';
import './ButtonComponents.scss';

interface ToggleButtonProps extends ButtonComponentProps {
  text: string;
  isLeftSide?: boolean;
}

const ToggleButton: React.FC<ToggleButtonProps> = ({ 
  isActive, 
  onClick, 
  type = 'button', 
  text, 
  isLeftSide = false 
}) => {
  const baseClasses = 'w-[90px] p-[4px] cursor-pointer outline-none font-semibold';
  const activeClasses = 'bg-[var(--button-switch-toggle-background-active)] text-[var(--button-switch-toggle-color-active)]';
  const inactiveClasses = 'bg-[var(--button-switch-toggle-background)] text-[var(--button-switch-toggle-color)]';
  const borderClasses = isLeftSide 
    ? 'rounded-tl-[4px] rounded-bl-[4px]' 
    : 'rounded-tr-[4px] rounded-br-[4px]';
  
  const className = `${baseClasses} ${borderClasses} ${isActive ? activeClasses : inactiveClasses}`;

  return (
    <button
      type={type}
      className={className}
      onClick={onClick}
    >
      {text}
    </button>
  );
};

export const EnableButtonComponent: React.FC<ButtonComponentProps> = (props) => {
  const { t: tGlobal } = useTranslation('global');
  const text = props.text || tGlobal('enable');
  return <ToggleButton {...props} text={text} isLeftSide={true} />;
};

export const DisableButtonComponent: React.FC<ButtonComponentProps> = (props) => {
  const { t: tGlobal } = useTranslation('global');
  const text = props.text || tGlobal('disable');
  return <ToggleButton {...props} text={text} isLeftSide={false} />;
};

export const ButtonOnComponent: React.FC<ButtonComponentProps> = (props) => {
  const { t: tGlobal } = useTranslation('global');
  const text = props.text || tGlobal('on');
  return <ToggleButton {...props} text={text} isLeftSide={true} />;
};

export const ButtonOffComponent: React.FC<ButtonComponentProps> = (props) => {
  const { t: tGlobal } = useTranslation('global');
  const text = props.text || tGlobal('off');
  return <ToggleButton {...props} text={text} isLeftSide={false} />;
}; 