import * as React from 'react';
import {  ButtonClosePopupComponent } from './ButtonComponents';
import { MediumTitleComponent } from './TitleComponents';

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
      <div className="px-[20px] pb-[10px] bg-[var(--modal-background)] border-[1px] border-[var(--modal-border-color)] shadow-[var(--modal-box-shadow)] mt-[100px] max-w-[560px] min-w-[580px] max-[768px]:max-w-[90%] rounded-[3px] dark:bg-[#121212]">
        <div className="relative my-[10px] text-center" >
          <MediumTitleComponent className='text-[16px] text-[var(--modal-title-color)]!' text={title} />
          <ButtonClosePopupComponent onClick={onClose} />
        </div>
        <div className="mb-[12px]">
          {children}
        </div>       
      </div>
    </div>
  );  
};

export default PopupModalComponent; 