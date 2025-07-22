import * as React from 'react';
import {  ButtonIconComponent } from './Buttons/ButtonIconComponent';
import { TitleComponent } from './Title';

interface PopupModalProps {
  title: string;
  onClose: () => void;
  children: React.ReactNode;
}

const PopupModalComponent: React.FC<PopupModalProps> = ({
  title,
  onClose,
  children,
}) => {  
  return (
    <div className="bg-[#00000080] fixed z-[100] top-0 left-0 right-0 bottom-0 flex items-start justify-center">
      <div className="p-[12px] pt-[0] bg-[var(--modal-background)] border-[1px] border-[var(--modal-border-color)] shadow-[var(--modal-box-shadow)] mt-[100px] max-w-[560px] min-w-[580px] max-[768px]:max-w-[90%] rounded-[3px] dark:bg-[#121212]">
        <div className="relative mb-[10px] text-center border-b-[1px] border-[var(--border-color)] py-[11px]" >
          <TitleComponent type='medium' text={title} />
          <ButtonIconComponent style='Close' onClick={onClose} />
        </div>
        <div>
          {children}
        </div>       
      </div>
    </div>
  );  
};

export default PopupModalComponent; 