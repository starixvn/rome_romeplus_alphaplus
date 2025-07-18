export interface ButtonComponentProps {
  text?: string;
  className?: string;
  onClick?: any;
  disabled?: boolean;
  isActive?: boolean;
  type?: 'button' | 'submit' | 'reset'; 
  title?: string;
  variant?: 'apply' | 'cancel' | 'full' | 'outline' | 'reject';
} 

export interface ButtonIconComponentProps extends ButtonComponentProps {
   
}