export type TitleType = 'main' | 'medium' | 'normal' | 'label';

export interface TitleComponentProps {
  type?: TitleType;
  text: string;
  className?: string;
  required?: boolean;
  showTippy?: boolean;
  children?: React.ReactNode;
  classNameTippy?: string;
  isShowIcon?: boolean;
} 