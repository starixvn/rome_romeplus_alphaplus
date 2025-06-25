export interface ButtonComponentProps {
  text?: string;
  className?: string;
  onClick?: any;
  disabled?: boolean;
  isActive?: boolean;
  type?: 'button' | 'submit' | 'reset';
} 