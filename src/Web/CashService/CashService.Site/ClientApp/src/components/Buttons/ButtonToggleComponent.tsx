import React from 'react';
import { useTranslation } from 'react-i18next';
import { ButtonComponentProps } from './types';
import './ButtonComponents.scss';

export const EnableButtonComponent: React.FC<ButtonComponentProps> = ({ isActive, onClick, type = 'button' }) => {
  const { t: tGlobal } = useTranslation('global');

  return (
    <button
      type={type}
      className={` w-[90px] p-[4px] cursor-pointer rounded-tl-[4px] rounded-bl-[4px] outline-none font-semibold ${isActive ? 'bg-[var(--button-switch-toggle-background-active)] text-[var(--button-switch-toggle-color-active)]' : 'bg-[var(--button-switch-toggle-background)] text-[var(--button-switch-toggle-color)]'}`}
      onClick={onClick}
    >
      {tGlobal('enable')}
    </button>
  );
};

export const DisableButtonComponent: React.FC<ButtonComponentProps> = ({ isActive, onClick, type = 'button' }) => {
  const { t: tGlobal } = useTranslation('global');

  return (
    <button
      type={type}
      className={` w-[90px] p-[4px] cursor-pointer rounded-tr-[4px] rounded-br-[4px] outline-none font-semibold ${isActive ? 'bg-[var(--button-switch-toggle-background-active)] text-[var(--button-switch-toggle-color-active)]' : 'bg-[var(--button-switch-toggle-background)] text-[var(--button-switch-toggle-color)]'}`}
      onClick={onClick}
    >
      {tGlobal('disable')}
    </button>
  );
};

export const ButtonOnComponent: React.FC<ButtonComponentProps> = ({ isActive, onClick, type = 'button' }) => {
  const { t: tGlobal } = useTranslation('global');

  return (
    <button
      type={type}
      className={`w-[90px] p-[4px] cursor-pointer rounded-tl-[4px] rounded-bl-[4px] outline-none font-semibold ${isActive ? 'bg-[var(--button-switch-toggle-background-active)] text-[var(--button-switch-toggle-color-active)]' : 'bg-[var(--button-switch-toggle-background)] text-[var(--button-switch-toggle-color)]'}`}
      onClick={onClick}
    >
      {tGlobal('on')}
    </button>
  );
};

export const ButtonOffComponent: React.FC<ButtonComponentProps> = ({ isActive, onClick, type = 'button' }) => {
  const { t: tGlobal } = useTranslation('global');

  return (
    <button
      type={type}
      className={`w-[90px] p-[4px] cursor-pointer rounded-tr-[4px] rounded-br-[4px] outline-none font-semibold ${isActive ? 'bg-[var(--button-switch-toggle-background-active)] text-[var(--button-switch-toggle-color-active)]' : 'bg-[var(--button-switch-toggle-background)] text-[var(--button-switch-toggle-color)]'}`}
      onClick={onClick}
    >
      {tGlobal('off')}
    </button>
  );
}; 