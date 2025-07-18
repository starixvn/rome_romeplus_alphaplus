export interface TitleComponentProps {
  title: string;
  showTippy?: boolean;
  children?: React.ReactNode;
  className?: string;
  required?: boolean;
  classNameTippy?: string;
}

export interface BaseTitleProps {
  text: string;
  className?: string;
  required?: boolean;
  isShowIcon?: boolean;
} 