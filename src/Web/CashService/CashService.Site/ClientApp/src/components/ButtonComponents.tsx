import React from 'react';
import { IconBack } from "./Icons";
import { useTranslation } from 'react-i18next';

interface ButtonComponentProps {
  text?: string;
  className?: string;
  onClick?: () => void;
  disabled?: boolean;
}

export const ButtonOutlineComponent: React.FC<ButtonComponentProps> = ({
  text,
  onClick,
  className = '',
  }) => {
  return (
    <div className={`${className}`}>
      <button className="shadow-[0_1px_5px_#0003,0_2px_2px_#00000024,0_3px_1px_-2px_#0000001f] border-[1px] border-[var(--button-border)] bg-[var(--button-background-cancel)] hover:bg-[var(--button-background-cancel-hover)] hover:border-[var(--button-border-cancel-hover)] text-[var(--button-text-cancel)] rounded-[5px] cursor-pointer h-[30px] px-[16px] font-semibold py-[4px] text-[12px] max-[768px]:text-[16px] max-[768px]:h-[34px]" 
        onClick={onClick}>
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
      <button className={`border-[1px] border-[var(--button-border)] bg-[var(--button-background)] hover:bg-[var(--button-background-hover)] hover:border-[var(--button-border-hover)]  shadow-[0_1px_5px_#0003,0_2px_2px_#00000024,0_3px_1px_-2px_#0000001f] text-[var(--button-text)] rounded-[5px] cursor-pointer h-[30px] px-[16px] font-semibold py-[4px] text-[12px] max-[768px]:text-[16px] max-[768px]:h-[34px] 
        ${disabled ? 'cursor-not-allowed disabled:bg-[var(--button-background-hover)] disabled:border-[var(--button-border-hover)]' : 'cursor-pointer bg-[var(--button-background)] hover:bg-[var(--button-background-hover)] '}`} 
        onClick={onClick} disabled={disabled}>
          {text}
      </button>
    </div>
  );
};
export const ButtonCancelComponent: React.FC<ButtonComponentProps> = ({
  text,
  onClick,
  className = '',
  }) => {
  return (
    <div className={`${className}`}>
      <button className="min-w-[140px] border-[1px] border-[var(--button-border)] bg-[var(--button-background-cancel)] hover:bg-[var(--button-background-cancel-hover)] hover:border-[var(--button-border-cancel-hover)] text-[var(--button-text-cancel)] rounded-[20px] cursor-pointer h-[30px] px-[16px] font-semibold py-[4px] text-[12px] max-[768px]:text-[16px] max-[768px]:h-[34px]" 
        onClick={onClick}>
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
      <button className={`min-w-[140px] border-[1px] border-[var(--button-border)] hover:border-[var(--button-border-hover)] shadow-[0_1px_5px_#0003,0_2px_2px_#00000024,0_3px_1px_-2px_#0000001f] text-[var(--button-text)] rounded-[20px]  h-[30px] px-[16px] font-semibold py-[4px] text-[12px] max-[768px]:text-[16px] max-[768px]:h-[34px] 
        ${disabled ? 'cursor-not-allowed disabled:bg-[var(--button-background-hover)] disabled:border-[var(--button-border-hover)]' : 'cursor-pointer bg-[var(--button-background)] hover:bg-[var(--button-background-hover)] '}`} 
        onClick={onClick} 
        disabled={disabled}>
          {text}
      </button>
    </div>
  );
};

export const BackButtonComponent: React.FC<ButtonComponentProps> = ({ onClick }) => {
  const { t: tGlobal } = useTranslation('global');

  return (
    <>
      <div className='text-[var(--text-color-primary)] px-[8px] py-[4px] border-b-[2px] border-[#cfcfcf] border-dotted mb-[4px]'>
        <span className='cursor-pointer inline-flex items-center py-[8px]' onClick={onClick}>
          <IconBack className='w-[18px] h-[18px] fill-[var(--primary-color)] mr-[8px]'  />
          <span className='text-[var(--text-color-primary)] font-semibold max-w-[100px]'>{tGlobal('back')}</span>
        </span>
      </div>
    </>
  );
};

