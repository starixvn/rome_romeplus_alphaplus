import React from 'react';
import { useTranslation } from 'react-i18next';
import { ButtonComponent } from './Buttons/ButtonComponent';
import { ButtonIconComponent } from './Buttons/ButtonIconComponent';
import { TitleComponent } from './Title';

interface ConfirmPopupProps {
  title: string;
  message: React.ReactNode;
  onCancel: () => void;
  onConfirm: () => void;
  cancelText?: string;
  confirmText?: string;
  children?: React.ReactNode;
}

const ConfirmPopup: React.FC<ConfirmPopupProps> = ({
  title,
  message,
  onCancel,
  onConfirm,
  children,
  cancelText,
  confirmText,
}) => {
  const { t: tGlobal } = useTranslation('global');
  return (
    <div className="bg-[#00000080] fixed inset-0 flex items-start justify-center z-[500]">
      <div className="p-[12px] pt-[0] bg-[var(--modal-background)] border-[1px] border-[var(--modal-border-color)] shadow-[var(--modal-box-shadow)] mt-[100px] max-[768px]:max-w-[90%] overflow-auto rounded-[4px]">
        <div className="relative mb-[10px] border-b-[1px] border-[var(--border-color)] py-[11px]">
          <TitleComponent type='medium' text={title} />
          <ButtonIconComponent style='Close' onClick={onCancel} />
        </div>
        <div className="mb-[20px]">{message}</div>
        {children}
        <div className="flex justify-center gap-[8px] mt-2">
          <ButtonComponent onClick={onCancel} text={cancelText || tGlobal('cancel')} variant='cancel' />
          <ButtonComponent onClick={onConfirm} text={confirmText || tGlobal('confirm')} variant='apply' />
        </div>
      </div>
    </div>
  );
};

export default ConfirmPopup;
