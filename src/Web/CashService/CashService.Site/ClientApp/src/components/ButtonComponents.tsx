import React from 'react';
import { IconBack, IconClose } from './Icons';
import { useTranslation } from 'react-i18next';
import './ButtonComponents.scss';
interface ButtonComponentProps {
  text?: string;
  className?: string;
  onClick?: any;
  disabled?: boolean;

  isActive?: boolean;
}

export const ButtonOutlineComponent: React.FC<ButtonComponentProps> = ({
  text,
  onClick,
  className = '',
}) => {
  return (
    <div className={`${className}`}>
      <button
        className="border-[1px] border-[var(--button-outline-border)] bg-[var(--button-outline-background)]  hover:bg-[var(--button-outline-background-hover)] hover:text-[var(--button-outline-text-hover)] text-[var(--button-outline-text)] hover:border-[var(--button-outline-border-hover)] rounded-[5px] cursor-pointer h-[30px] px-[16px] font-semibold outline-none"
        onClick={onClick}
      >
        {text}
      </button>
    </div>
  );
};
export const ButtonFullComponent: React.FC<ButtonComponentProps> = ({
  text,
  onClick,
  className = '',
  disabled = false,
}) => {
  return (
    <div className={`${className}`}>
      <button
        className={`bg-[var(--button-background)] hover:bg-[var(--button-background-hover)] text-[var(--button-text)] rounded-[3px] cursor-pointer h-[30px] px-[16px] font-semibold outline-none
        ${disabled ? 'cursor-not-allowed opacity-80' : 'cursor-pointer'}`}
        onClick={onClick}
        disabled={disabled}
      >
        {text}
      </button>
    </div>
  );
};
export const ButtonCancelComponent: React.FC<ButtonComponentProps> = ({
  text,
  onClick,
  className = '',
  disabled = false,
}) => {
  return (
    <div className={`${className}`}>
      <button
        className="min-w-[90px] bg-[var(--button-background-cancel)] hover:bg-[var(--button-background-cancel-hover)] text-[var(--button-text)] rounded-[3px] cursor-pointer h-[30px] px-[16px] font-semibold outline-none"
        onClick={onClick}
        disabled={disabled}
      >
        {text}
      </button>
    </div>
  );
};

export const ButtonApplyComponent: React.FC<ButtonComponentProps> = ({
  text,
  onClick,
  disabled = false,
  className = '',
}) => {
  return (
    <div className={`${className}`}>
      <button
        className={`min-w-[90px] text-[var(--button-text)] bg-[var(--button-background)] hover:bg-[var(--button-background-hover)] rounded-[3px] h-[30px] px-[16px] font-semibold  max-[768px]:h-[34px] outline-none
        ${disabled ? 'cursor-not-allowed opacity-80' : 'cursor-pointer'}`}
        onClick={onClick}
        disabled={disabled}
      >
        {text}
      </button>
    </div>
  );
};

export const BackButtonComponent: React.FC<ButtonComponentProps> = ({ onClick }) => {
  const { t: tGlobal } = useTranslation('global');

  return (
    <>
      <div className="text-[var(--button-back-text)] hover:text-[var(--button-back-text-hover)] border-[1px] border-[var(--button-back-border)] rounded-[20px] mr-[16px] bg-[var(--button-back-background)] hover:bg-[var(--button-back-background-hover)] hover:border-[var(--button-back-border-hover)] group mb-[8px] ">
        <span
          className="cursor-pointer inline-flex items-center py-[4px] pl-[8px] pr-[16px] "
          onClick={onClick}
        >
          <IconBack className="w-[18px] h-[18px] fill-[var(--button-back-text)] mr-[8px] group-hover:fill-[var(--button-back-text-hover)]" />
          <span className="font-semibold max-w-[100px]">{tGlobal('back')}</span>
        </span>
      </div>
    </>
  );
};

export const EnableButtonComponent: React.FC<ButtonComponentProps> = ({ isActive, onClick }) => {
  const { t: tGlobal } = useTranslation('global');

  return (
    <button
      type="button"
      className={` w-[100px] px-[16px] py-[8px] cursor-pointer rounded-tl-[4px] rounded-bl-[4px] outline-none ${isActive ? 'bg-[var(--button-switch-toggle-background-active)] text-[var(--button-switch-toggle-background-color-active)]' : 'bg-[var(--button-switch-toggle-background)] text-[var(--button-switch-toggle-background-color)]'}`}
      onClick={onClick}
    >
      {tGlobal('enable')}
    </button>
  );
};

export const DisableButtonComponent: React.FC<ButtonComponentProps> = ({ isActive, onClick }) => {
  const { t: tGlobal } = useTranslation('global');

  return (
    <button
      type="button"
      className={` w-[100px] px-[16px] py-[8px] cursor-pointer rounded-tr-[4px] rounded-br-[4px] outline-none ${isActive ? 'bg-[var(--button-switch-toggle-background-active)] text-[var(--button-switch-toggle-background-color-active)]' : 'bg-[var(--button-switch-toggle-background)] text-[var(--button-switch-toggle-background-color)]'}`}
      onClick={onClick}
    >
      {tGlobal('disable')}
    </button>
  );
};

export const ButtonOnComponent: React.FC<ButtonComponentProps> = ({ isActive, onClick }) => {
  const { t: tGlobal } = useTranslation('global');

  return (
    <button
      type="button"
      className={` w-[70px] px-[16px] py-[8px] cursor-pointer rounded-tl-[4px] rounded-bl-[4px] outline-none ${isActive ? 'bg-[var(--button-switch-toggle-background-active)] text-[var(--button-switch-toggle-background-color-active)]' : 'bg-[var(--button-switch-toggle-background)] text-[var(--button-switch-toggle-background-color)]'}`}
      onClick={onClick}
    >
      {tGlobal('on')}
    </button>
  );
};

export const ButtonOffComponent: React.FC<ButtonComponentProps> = ({ isActive, onClick }) => {
  const { t: tGlobal } = useTranslation('global');

  return (
    <button
      type="button"
      className={` w-[70px] px-[16px] py-[8px] cursor-pointer rounded-tr-[4px] rounded-br-[4px] outline-none ${isActive ? 'bg-[var(--button-switch-toggle-background-active)] text-[var(--button-switch-toggle-background-color-active)]' : 'bg-[var(--button-switch-toggle-background)] text-[var(--button-switch-toggle-background-color)]'}`}
      onClick={onClick}
    >
      {tGlobal('off')}
    </button>
  );
};

export const ButtonClosePopupComponent: React.FC<ButtonComponentProps> = ({ onClick }) => {
  return (
    <button
      type="button"
      className="absolute top-0 right-[-10px] cursor-pointer outline-none"
      onClick={onClick}
    >
      <IconClose />
    </button>
  );
};

export const ButtonTabActiveComponent: React.FC<ButtonComponentProps> = ({ text, onClick }) => {
  return (
    <button
      className="font-semibold px-[20px] pt-[10px] pb-[13px] text-[var(--tab-component-color)] cursor-pointer relative after:content-[''] after:absolute after:bottom-0 after:left-0 after:w-0 after:h-[3px] after:bg-[var(--tab-component-border-color)] after:transition-all after:duration-500 after:ease-in-out after:opacity-0 hover:after:opacity-100 hover:after:w-full hover:text-[var(--tab-component-color-active)]"
      onClick={onClick}
    >
      {text}
    </button>
  );
};

export const ButtonTabInactiveComponent: React.FC<ButtonComponentProps> = ({ text, onClick }) => {
  return (
    <button
      className="font-semibold px-[20px] pt-[10px] pb-[13px] text-[var(--tab-component-color-active)] cursor-pointer relative after:content-[''] after:absolute after:bottom-0 after:left-0 after:w-full after:h-[3px] after:bg-[var(--tab-component-border-color)]"
      onClick={onClick}
    >
      {text}
    </button>
  );
};

export const ButtonTabComponent: React.FC<ButtonComponentProps> = ({ text, onClick, isActive }) => {
  return isActive ? (
    <ButtonTabActiveComponent text={text} onClick={onClick} />
  ) : (
    <ButtonTabInactiveComponent text={text} onClick={onClick} />
  );
};
