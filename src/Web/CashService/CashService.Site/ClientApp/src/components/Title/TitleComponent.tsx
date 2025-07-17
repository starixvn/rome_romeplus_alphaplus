import './TitleComponents.scss';

import Tippy from '@tippyjs/react';

import { IconInfo } from '../Icons';
import { TitleComponentProps } from './types';

export default function TitleComponent({
  title,
  showTippy,
  children,
}: Readonly<TitleComponentProps>) {
  return (
    <h2 className="mb-[12px] flex text-[14px] font-semibold items-center uppercase text-[var(--titleComponent-color)] border-b border-[var(--titleComponent-border-color)] leading-[39px]">
      {title}
      {showTippy && (
        <Tippy content={children} placement="top" theme="light" arrow={true}>
          <span>
            <IconInfo className="w-[18px] cursor-pointer h-[18px] ml-[10px] fill-[#118AF2]" />
          </span>
        </Tippy>
      )}
    </h2>
  );
}
