import React, { useEffect, useRef, useCallback } from 'react';
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
  disableClickOutside?: boolean;
}

const ConfirmPopup: React.FC<ConfirmPopupProps> = ({
  title,
  message,
  onCancel,
  onConfirm,
  children,
  cancelText,
  confirmText,
  disableClickOutside = false,
}) => {
  const { t: tGlobal } = useTranslation('global');
  const modalRef = useRef<HTMLDivElement>(null);
  const overlayRef = useRef<HTMLDivElement>(null);

  // Memoize callbacks to prevent unnecessary re-renders
  const handleCancel = useCallback(() => {
    onCancel();
  }, [onCancel]);

  const handleConfirm = useCallback(() => {
    onConfirm();
  }, [onConfirm]);

  useEffect(() => {
    if (modalRef.current) {
      modalRef.current.focus();
    }
  }, []);

  useEffect(() => {
    const handleEsc = (e: KeyboardEvent) => {
      if (e.key === 'Escape') {
        handleCancel();
      }
    };

    const handleTab = (e: KeyboardEvent) => {
      if (!modalRef.current) return;
      
      const focusableElements = modalRef.current.querySelectorAll(
        'button, [href], input, select, textarea, [tabindex]:not([tabindex="-1"])'
      );
      const firstElement = focusableElements[0] as HTMLElement;
      const lastElement = focusableElements[focusableElements.length - 1] as HTMLElement;

      if (e.shiftKey) {
        if (document.activeElement === firstElement) {
          e.preventDefault();
          lastElement.focus();
        }
      } else {
        if (document.activeElement === lastElement) {
          e.preventDefault();
          firstElement.focus();
        }
      }
    };

    document.addEventListener('keydown', handleEsc);
    document.addEventListener('keydown', handleTab);
    
    return () => {
      document.removeEventListener('keydown', handleEsc);
      document.removeEventListener('keydown', handleTab);
    };
  }, [handleCancel]);

  // Click outside handling
  const handleOverlayClick = useCallback((e: React.MouseEvent) => {
    if (!disableClickOutside && e.target === overlayRef.current) {
      handleCancel();
    }
  }, [disableClickOutside, handleCancel]);

  return (
    <div 
      ref={overlayRef}
      className="bg-[#00000080] fixed inset-0 flex items-start justify-center z-[500]"
      onClick={handleOverlayClick}
      role="dialog"
      aria-modal="true"
      aria-labelledby="modal-title"
      aria-describedby="modal-description"
    >
      <div 
        className="p-[12px] pt-[0] bg-[var(--modal-background)] border-[1px] border-[var(--modal-border-color)] shadow-[var(--modal-box-shadow)] mt-[100px] max-[768px]:max-w-[90%] overflow-auto rounded-[4px]" 
        ref={modalRef}
        tabIndex={-1}
      >
        <div className="relative mb-[10px] border-b-[1px] border-[var(--border-color)] py-[11px]">
          <div id="modal-title">
            <TitleComponent type='medium' text={title} />
          </div>
          <ButtonIconComponent 
            style='Close' 
            onClick={handleCancel}
            aria-label={tGlobal('close')}
          />
        </div>
        <div className="mb-[20px]" id="modal-description">{message}</div>
        {children}
        <div className="flex justify-center gap-[8px] mt-2">
          <ButtonComponent onClick={handleCancel} text={cancelText || tGlobal('cancel')} variant='cancel' />
          <ButtonComponent onClick={handleConfirm} text={confirmText || tGlobal('confirm')} variant='apply' />
        </div>
      </div>
    </div>
  );
};

export default ConfirmPopup;
