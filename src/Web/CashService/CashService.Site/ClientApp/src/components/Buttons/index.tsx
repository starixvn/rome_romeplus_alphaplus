export { ButtonComponent } from './ButtonComponent';

// Wrapper components for backward compatibility
import { ButtonComponent } from './ButtonComponent';
import { ButtonComponentProps } from './types';

export const ButtonApplyComponent: React.FC<ButtonComponentProps> = (props) => (
  <ButtonComponent {...props} variant="apply" />
);

export const ButtonCancelComponent: React.FC<ButtonComponentProps> = (props) => (
  <ButtonComponent {...props} variant="cancel" />
);

export const ButtonFullComponent: React.FC<ButtonComponentProps> = (props) => (
  <ButtonComponent {...props} variant="full" />
);

export const ButtonOutlineComponent: React.FC<ButtonComponentProps> = (props) => (
  <ButtonComponent {...props} variant="outline" />
);

export const ButtonRejectComponent: React.FC<ButtonComponentProps> = (props) => (
  <ButtonComponent {...props} variant="reject" />
);

// Export other components that don't need to be unified
export { 
  ButtonClosePopupComponent, 
  ButtonPenComponent, 
  ButtonDeleteComponent 
} from './ButtonIconComponent';

export { 
  EnableButtonComponent, 
  DisableButtonComponent,
} from './ButtonToggleComponent';

export { 
  ButtonTabActiveComponent, 
  ButtonTabInactiveComponent, 
  ButtonTabComponent 
} from './ButtonTabComponent';

export { BackButtonComponent } from './BackButtonComponent';

// Types
export type { ButtonComponentProps } from './types';
