import Tippy from "@tippyjs/react";
import { IconInfo } from "./Icons";
import { ReactNode } from 'react';

interface TitleComponentProps {
  title: string;
  showTippy?: boolean;
  children?: ReactNode;
}

export default function TitleComponent({ title, showTippy, children }: TitleComponentProps) {
    return (
      <>
        <div className="mb-[8px]">
            <h1 className="flex items-center font-bold text-[15px] text-[#333333] dark:text-[#ffffff] max-[768px]:text-[19px]">
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
            </h1>
        </div>
      </>
    );
  }
  