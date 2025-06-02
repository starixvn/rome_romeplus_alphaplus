import React from 'react';
import { IconBack, IconClose } from './Icons';
import { useTranslation } from 'react-i18next';

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
        className="shadow-[0_1px_5px_#0003,0_2px_2px_#00000024,0_3px_1px_-2px_#0000001f] border-[1px] border-[var(--button-border)] bg-[var(--button-background-cancel)] hover:bg-[var(--button-background-cancel-hover)] hover:border-[var(--button-border-cancel-hover)] text-[var(--button-text-cancel)] rounded-[5px] cursor-pointer h-[30px] px-[16px] font-semibold py-[4px] text-[12px] max-[768px]:text-[16px] max-[768px]:h-[34px] outline-none"
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
        className={`border-[1px] border-[var(--button-border)] bg-[var(--button-background)] hover:bg-[var(--button-background-hover)] hover:border-[var(--button-border-hover)]  shadow-[0_1px_5px_#0003,0_2px_2px_#00000024,0_3px_1px_-2px_#0000001f] text-[var(--button-text)] rounded-[5px] cursor-pointer h-[30px] px-[16px] font-semibold py-[4px] text-[12px] max-[768px]:text-[16px] max-[768px]:h-[34px] outline-none
        ${disabled ? 'cursor-not-allowed disabled:bg-[var(--button-background-hover)] disabled:border-[var(--button-border-hover)]' : 'cursor-pointer bg-[var(--button-background)] hover:bg-[var(--button-background-hover)] '}`}
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
        className="min-w-[140px] border-[1px] border-[var(--button-border)] bg-[var(--button-background-cancel)] hover:bg-[var(--button-background-cancel-hover)] hover:border-[var(--button-border-cancel-hover)] text-[var(--button-text-cancel)] rounded-[20px] cursor-pointer h-[30px] px-[16px] font-semibold py-[4px] text-[12px] max-[768px]:text-[16px] max-[768px]:h-[34px] outline-none"
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
        className={`min-w-[140px] border-[1px] border-[var(--button-border)] hover:border-[var(--button-border-hover)] shadow-[0_1px_5px_#0003,0_2px_2px_#00000024,0_3px_1px_-2px_#0000001f] text-[var(--button-text)] rounded-[20px]  h-[30px] px-[16px] font-semibold py-[4px] text-[12px] max-[768px]:text-[16px] max-[768px]:h-[34px] outline-none
        ${disabled ? 'cursor-not-allowed disabled:bg-[var(--button-background-hover)] disabled:border-[var(--button-border-hover)]' : 'cursor-pointer bg-[var(--button-background)] hover:bg-[var(--button-background-hover)] '}`}
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
      <div className="text-[var(--text-color-primary)] border-[1px] border-[var(--primary-color)] rounded-[20px] mr-[16px] hover:bg-[var(--primary-color)] hover:text-[#ffffff] group mb-[8px]">
        <span
          className="cursor-pointer inline-flex items-center py-[4px] pl-[8px] pr-[16px] "
          onClick={onClick}
        >
          <IconBack className="w-[18px] h-[18px] fill-[var(--primary-color)] mr-[8px] group-hover:fill-[#ffffff]" />
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
      className="absolute top-[-20px] right-[-20px] cursor-pointer outline-none"
      onClick={onClick}
    >
      <IconClose />
    </button>
  );
};

export const ButtonTabComponent: React.FC<ButtonComponentProps> = ({ text, onClick }) => {
  return (
    <button
      className="font-bold text-[12px] leading-[36px] max-[768px]:text-[16px] px-[16px] rounded-t-[4px] bg-[#dbe2ef] text-[var(--text-color-primary)] cursor-pointer"
      onClick={onClick}
    >
      {text}
    </button>
  );
};

export const ButtonTabActiveComponent: React.FC<ButtonComponentProps> = ({ text }) => {
  return (
    <button className="font-bold text-[12px] leading-[36px] max-[768px]:text-[16px] px-[16px] rounded-t-[4px]  bg-[var(--button-background)] text-[#ffffff]">
      {text}
    </button>
  );
};
