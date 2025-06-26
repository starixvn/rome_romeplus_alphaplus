export interface TitleComponentProps {
  title: string;
  showTippy?: boolean;
  children?: React.ReactNode;
  className?: string;
  required?: boolean;
}

export interface BaseTitleProps {
  text: string;
  className?: string;
  required?: boolean;
} 