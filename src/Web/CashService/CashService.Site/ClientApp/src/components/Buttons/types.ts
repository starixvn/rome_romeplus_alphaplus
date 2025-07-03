export interface ButtonComponentProps {
  text?: string;
  className?: string;
  onClick?: any;
  disabled?: boolean;
  isActive?: boolean;
  type?: 'button' | 'submit' | 'reset'; 
  title?: string;
} 

export interface ButtonIconComponentProps extends ButtonComponentProps {
  sizeIcon?: { width: number; height: number };
}