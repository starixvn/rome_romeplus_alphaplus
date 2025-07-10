import React from 'react';
import { IconClose, IconDelete, IconPen } from '../Icons';
import { ButtonComponentProps, ButtonIconComponentProps} from './types';
import './ButtonComponents.scss';

export const ButtonClosePopupComponent: React.FC<ButtonComponentProps> = ({ onClick, type }) => {
  return (
    <button
      type={type || 'button'}
      className="absolute top-[11px] right-[-5px] cursor-pointer outline-none"
      onClick={onClick}
      aria-label="Close"
    >
      <IconClose />
    </button>
  );
};

export const ButtonPenComponent: React.FC<ButtonIconComponentProps> = ({ onClick, disabled, type, sizeIcon }) => {
  return (
    <button
      type={type || 'button'}
      className={` mx-[4px] cursor-pointer p-[4px] rounded-[4px] hover:bg-[var(--button-icon-hover)] text-[var(--text-color-primary)] ${disabled ? 'cursor-not-allowed opacity-80' : ''}`}
      onClick={onClick}
    >
      <IconPen className="fill-[var(--icon-color)]" width={sizeIcon?.width} height={sizeIcon?.height} />
    </button>
  );
};

export const ButtonDeleteComponent: React.FC<ButtonIconComponentProps> = ({ onClick, type, sizeIcon }) => {
  return (
    <button
      type={type || 'button'}
      className="mx-[4px] cursor-pointer p-[4px] rounded-[4px] hover:bg-[var(--button-icon-hover)]"
      onClick={onClick}
    >
      <IconDelete className='fill-[#d33]' width={sizeIcon?.width} height={sizeIcon?.height} />
    </button>
  );
}; 