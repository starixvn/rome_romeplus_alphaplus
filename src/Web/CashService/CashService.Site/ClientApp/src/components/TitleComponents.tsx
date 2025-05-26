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

  export const TitleMediumComponent: React.FC<TitleComponentProps> = ({
    title,
    required = false,
    className = '',
  }) => {
    return title &&  (    
      <h3 className={`${className} w-[100%] text-[16px] text-[var(--text-color-primary)] font-semibold max-[768px]:text-[20px] `}>
        {title}
        {required && <span className="text-[#FF0000]">*</span>}
      </h3>
    );
  };

  export const TitleNormalComponent: React.FC<TitleComponentProps> = ({
    title,
    className = '',
  }) => {
    return title &&  (    
      <h3 className={`${className} text-[14px] max-[768px]:text-[18px] py-[8px] text-[var(--text-color-primary)] font-semibold `}>
        {title}
      </h3>
    );
  };
  