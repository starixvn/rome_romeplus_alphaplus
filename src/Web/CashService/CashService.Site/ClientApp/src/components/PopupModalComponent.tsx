import * as React from 'react';
import { useTranslation } from 'react-i18next';
import { ButtonApplyComponent, ButtonClosePopupComponent } from './ButtonComponents';
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
  const { t: tGlobal } = useTranslation('global');

  return (
    <div className="bg-[#00000066] fixed z-[100] top-0 left-0 right-0 bottom-0 flex items-start justify-center">
      <div className="p-[30px] bg-[#ffffff] mt-[100px] w-full max-w-[560px] max-[768px]:max-w-[90%] rounded-[4px] shadow-[0_2px_4px_-1px_#00000033,0_4px_5px_#00000024,0_1px_10px_#0000001f] text-[12px] max-[768px]:text-[16px]">
        <div className="relative">
          <MediumTitleComponent className='font-bold!' text={title} />
          <ButtonClosePopupComponent onClick={onClose} />
        </div>
        <div className="mb-[12px]">
          {children}
        </div>
        <div className="flex justify-center">  
          <ButtonApplyComponent text={tGlobal('close')} onClick={onClose} />
        </div>
      </div>
    </div>
  );  
};

export default PopupModalComponent; 