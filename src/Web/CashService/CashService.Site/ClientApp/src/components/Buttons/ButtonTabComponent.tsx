import React from 'react';
import { ButtonComponentProps } from './types';
import './ButtonComponents.scss';

export const ButtonTabActiveComponent: React.FC<ButtonComponentProps> = ({
  text,
  onClick,
  type,
}) => {
  return (
    <button
      type={type}
      className="font-semibold px-[20px] pt-[10px] pb-[13px] text-[var(--tab-component-color)] cursor-pointer relative after:content-[''] after:absolute after:bottom-0 after:left-0 after:w-0 after:h-[3px] after:bg-[var(--tab-component-border-color)] after:transition-all after:duration-500 after:ease-in-out after:opacity-0 hover:after:opacity-100 hover:after:w-full hover:text-[var(--tab-component-color-active)]"
      onClick={onClick}
    >
      {text}
    </button>
  );
};

export const ButtonTabInactiveComponent: React.FC<ButtonComponentProps> = ({
  text,
  onClick,
  type,
}) => {
  return (
    <button
      type={type}
      className="font-semibold px-[20px] pt-[10px] pb-[13px] text-[var(--tab-component-color-active)] cursor-pointer relative after:content-[''] after:absolute after:bottom-0 after:left-0 after:w-full after:h-[3px] after:bg-[var(--tab-component-border-color)]"
      onClick={onClick}
    >
      {text}
    </button>
  );
};

export const ButtonTabComponent: React.FC<ButtonComponentProps> = ({
  text,
  onClick,
  isActive,
  type,
}) => {
  return isActive ? (
    <ButtonTabActiveComponent text={text} onClick={onClick} type={type} />
  ) : (
    <ButtonTabInactiveComponent text={text} onClick={onClick} type={type} />
  );
};
