import Tippy from "@tippyjs/react";
import { IconInfo } from "./Icons";
import { ReactNode } from 'react';

interface TitleComponentProps {
  title: string;
  showTippy?: boolean;
  children?: ReactNode;
  className?: string;
  required?: boolean;
}

interface MediumTitleComponentProps {
  text: string;
  className?: string;
  required?: boolean;
}

export default function TitleComponent({ title, showTippy, children }: TitleComponentProps) {
  return (
    <>
      <div className="mb-[8px]">
          <h2 className="flex items-center font-bold text-[15px] text-[#333333] dark:text-[#ffffff] max-[768px]:text-[19px]">
          {title}
          {showTippy && (
            <Tippy
              content={children}
              placement="top"
              theme="light"
              arrow={true}
            >
              <span><IconInfo className="w-[20px] cursor-pointer h-[20px] ml-[4px] fill-[#163f78]" /></span>
            </Tippy>
          )}
          </h2>
      </div>
    </>
  );
}

export const MediumTitleComponent: React.FC<MediumTitleComponentProps> = ({
  text,
  required = false,
  className = '',
}) => {
  return text &&  (    
    <h3 className={`${className} w-[100%] text-[16px] text-[var(--text-color-primary)] font-semibold max-[768px]:text-[20px] `}>
      {text}
      {required && <span className="text-[#FF0000]">*</span>}
    </h3>
  );
};

export const NormalTitleComponent: React.FC<MediumTitleComponentProps> = ({
  text,
  className = '',
}) => {
  return text &&  (    
    <h3 className={`${className} text-[14px] max-[768px]:text-[18px] py-[8px] text-[var(--text-color-primary)] font-semibold `}>
      {text}
    </h3>
  );
};

export const LableTitleComponent: React.FC<MediumTitleComponentProps> = ({
  text,
  required = false,
  className = '',
}) => {
  return text &&  (    
    <label className={`${className} block font-semibold text-[#333333] my-[8px] `}>
      {text}
      {required && <span className="text-[#FF0000]">*</span>}
    </label>
  );
};