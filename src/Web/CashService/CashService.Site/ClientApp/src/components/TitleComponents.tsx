import { ReactNode } from 'react';
import Tippy from '@tippyjs/react';
import { IconInfo } from './Icons';
import './TitleComponents.scss';

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

export const MediumTitleComponent: React.FC<MediumTitleComponentProps> = ({
  text,
  required = false,
  className = '',
}) => {
  return (
    text && (
      <h3
        className={`${className} w-[100%] font-semibold text-[14px] text-[var(--text-color-primary)]`}
      >
        {text}
        {required && <span className="text-[#FF0000]">*</span>}
      </h3>
    )
  );
};

export const NormalTitleComponent: React.FC<MediumTitleComponentProps> = ({
  text,
  className = '',
}) => {
  return (
    text && (
      <h3
        className={`${className} text-[14px] max-[768px]:text-[18px] py-[8px] text-[var(--text-color-primary)] font-semibold `}
      >
        {text}
      </h3>
    )
  );
};

export const LabelTitleComponent: React.FC<MediumTitleComponentProps> = ({
  text,
  required = false,
  className = '',
}) => {
  return (
    text && (
      <label className={`${className} block font-semibold text-[var(--titleComponent-label-color)] my-[4px] `}>
        {text}
        {required && <span className="text-[#FF0000]">*</span>}
      </label>
    )
  );
};
