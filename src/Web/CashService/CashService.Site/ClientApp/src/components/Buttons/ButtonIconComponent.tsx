import React, { useMemo } from 'react';
import { IconBack, IconClose, IconCopy, IconDelete, IconExcel, IconPen } from '../Icons';
import { useTranslation } from 'react-i18next';

interface ButtonIconStyles {
    container: string;
    iconComponent?: React.ReactNode;
    text?: string;
}

export const ButtonIconComponent: React.FC<{
    onClick: () => void;
    disabled?: boolean;
    type?: 'button' | 'submit' | 'reset';
    style?: 'Close' | 'Edit' | 'Delete' | 'Excel' | 'Copy' | 'Back';
    icon?: React.ReactNode;
    className?: string;
    
}> = ({ onClick, disabled, type, style, className, icon }) => {
    const { t: tGlobal } = useTranslation('global');
    const styles = useMemo((): ButtonIconStyles => {
        switch (style) {
          case 'Close':
            return {
              container: 'hover:bg-[var(--button-icon-hover)] absolute top-[11px] right-[-5px] outline-none ',
              iconComponent: <IconClose />
            };
          case 'Edit':
            return {
              container: 'hover:bg-[var(--button-icon-hover)] mx-[4px] p-[4px] rounded-[3px]',
              iconComponent: <IconPen />
            };
          case 'Delete':
            return {
              container: 'hover:bg-[var(--button-icon-hover)] mx-[4px] py-[6px] px-[6px] rounded-[3px]',
              iconComponent: <IconDelete />
            };
          case 'Excel':
            return {
              container: 'hover:bg-[var(--button-icon-hover)] mx-[4px] p-[4px] bg-black/10 rounded-[3px]',
              iconComponent: <IconExcel />
            };
          case 'Copy':
            return {
              container: 'hover:bg-[var(--button-icon-hover)] mx-[4px] py-[4px] px-[4px] bg-black/10 rounded-[3px]',
              iconComponent: <IconCopy />
            };
          case 'Back':
            return {
              container: 'min-w-[90px] text-[var(--button-back-text)] hover:text-[var(--button-back-text-hover)] border-[1px] border-[var(--button-back-border)] rounded-[20px] mr-[16px] bg-[var(--button-back-background)] hover:bg-[var(--button-back-background-hover)] hover:border-[transparent] group mb-[8px] inline-flex items-center py-[4px] pl-[8px] pr-[16px]',
              iconComponent: <IconBack className='w-[18px] h-[18px] fill-[var(--button-back-text)] mr-[8px] group-hover:fill-[var(--button-back-text-hover)]' />,
              text: tGlobal('back')
            };
          default:
            return {
              container: 'mx-[4px] py-[4px] px-[4px] rounded-[3px] ',            
            };
        }
      }, [style]);
      
    return (
        <button
            aria-label={style}
            type={type || 'button'}
            className={` ${styles.container} ${className} ${disabled ? 'cursor-not-allowed opacity-80' : 'cursor-pointer'}  focus:outline-none`}
            onClick={onClick}
            disabled={disabled}
        >
            {icon || styles.iconComponent}
            {styles.text && <span className="font-semibold">{styles.text}</span>}
        </button>
    );
};