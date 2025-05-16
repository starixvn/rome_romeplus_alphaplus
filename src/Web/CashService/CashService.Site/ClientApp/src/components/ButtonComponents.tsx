import React from 'react';

export type ButtonType = 'cancel' | 'apply';
export type ButtonStyle = 'rounded' | 'medium';


interface ButtonProps {
  type: ButtonType;
  style: ButtonStyle;
  text: string;
  className?: string;
  onClick?: () => void;
}

const getType = (type: ButtonType) => {
    switch (type) {
      case 'apply':
        return "border-[1px] border-[var(--button-border)] bg-[var(--button-background)] hover:bg-[var(--button-background-hover)] hover:border-[var(--button-border-hover)]  shadow-[0_1px_5px_#0003,0_2px_2px_#00000024,0_3px_1px_-2px_#0000001f] text-[#ffffff]";  
      default:
        return "border-[1px] border-[var(--button-border)] bg-[var(--button-background-cancel)] hover:bg-[var(--button-background-cancel-hover)] hover:border-[var(--button-border-cancel-hover)] ";
    }
  };

  const getStyle = (style: ButtonStyle) => {
    switch (style) {
      case 'rounded':
        return 'rounded-[20px]';
      case 'medium':
        return 'rounded-[5px]';
      default:
        return 'rounded-0';
    }
  };

export const ButtonComponent: React.FC<ButtonProps> = ({
    text,
    type,
    style,
    onClick,
    className = ''
}) => {
  return (
    <button className={`${className} ${getType(type)} ${getStyle(style)} h-[30px] px-[16px] font-semibold py-[4px] text-[12px] cursor-pointer `} onClick={onClick}>{text}</button>
  );
};
