import './ButtonComponents.scss';

import React from 'react';
import { useTranslation } from 'react-i18next';

import { IconBack } from '../Icons';
import { ButtonComponentProps } from './types';

export const BackButtonComponent: React.FC<ButtonComponentProps> = ({ onClick }) => {
  const { t: tGlobal } = useTranslation('global');

  return (
    <div className="min-w-[90px] text-[var(--button-back-text)] hover:text-[var(--button-back-text-hover)] border-[1px] border-[var(--button-back-border)] rounded-[20px] mr-[16px] bg-[var(--button-back-background)] hover:bg-[var(--button-back-background-hover)] hover:border-[transparent] group mb-[8px] ">
      <span
        className="cursor-pointer inline-flex items-center py-[4px] pl-[8px] pr-[16px]"
        onClick={onClick}
      >
        <IconBack className="w-[18px] h-[18px] fill-[var(--button-back-text)] mr-[8px] group-hover:fill-[var(--button-back-text-hover)]" />
        <span className="font-semibold">{tGlobal('back')}</span>
      </span>
    </div>
  );
};
