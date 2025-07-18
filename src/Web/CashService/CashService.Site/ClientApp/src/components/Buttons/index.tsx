import React from 'react';
import { ButtonComponent } from './ButtonComponent';
import { ButtonComponentProps } from './types';
import { 
  ButtonClosePopupComponent, 
  ButtonPenComponent, 
  ButtonDeleteComponent,
  ButtonExcelComponent,
  ButtonCopyComponent,
  ButtonBackComponent
} from './ButtonIconComponent';
import { 
  EnableButtonComponent, 
  DisableButtonComponent,
} from './ButtonToggleComponent';
import { 
  ButtonTabActiveComponent, 
  ButtonTabInactiveComponent, 
  ButtonTabComponent 
} from './ButtonTabComponent';

// Re-export the main ButtonComponent
export { ButtonComponent } from './ButtonComponent';

// Button variant constants for better type safety
export const BUTTON_VARIANTS = {
  APPLY: 'apply',
  CANCEL: 'cancel', 
  FULL: 'full',
  OUTLINE: 'outline',
  REJECT: 'reject'
} as const;

export type ButtonVariant = typeof BUTTON_VARIANTS[keyof typeof BUTTON_VARIANTS];

/**
 * Wrapper components for backward compatibility
 * These components provide pre-configured variants of ButtonComponent
 */

/**
 * Apply button variant - typically used for primary actions
 */
export const ButtonApplyComponent: React.FC<ButtonComponentProps> = (props) => (
  <ButtonComponent {...props} variant={BUTTON_VARIANTS.APPLY} />
);

/**
 * Cancel button variant - typically used for secondary/cancel actions
 */
export const ButtonCancelComponent: React.FC<ButtonComponentProps> = (props) => (
  <ButtonComponent {...props} variant={BUTTON_VARIANTS.CANCEL} />
);

/**
 * Full button variant - typically used for full-width buttons
 */
export const ButtonFullComponent: React.FC<ButtonComponentProps> = (props) => (
  <ButtonComponent {...props} variant={BUTTON_VARIANTS.FULL} />
);

/**
 * Outline button variant - typically used for secondary actions
 */
export const ButtonOutlineComponent: React.FC<ButtonComponentProps> = (props) => (
  <ButtonComponent {...props} variant={BUTTON_VARIANTS.OUTLINE} />
);

/**
 * Reject button variant - typically used for destructive actions
 */
export const ButtonRejectComponent: React.FC<ButtonComponentProps> = (props) => (
  <ButtonComponent {...props} variant={BUTTON_VARIANTS.REJECT} />
);

// Export icon button components
export { 
  ButtonClosePopupComponent, 
  ButtonPenComponent, 
  ButtonDeleteComponent,
  ButtonExcelComponent,
  ButtonCopyComponent,
  ButtonBackComponent
} from './ButtonIconComponent';

// Export toggle button components
export { 
  EnableButtonComponent, 
  DisableButtonComponent,
} from './ButtonToggleComponent';

// Export tab button components
export { 
  ButtonTabActiveComponent, 
  ButtonTabInactiveComponent, 
  ButtonTabComponent 
} from './ButtonTabComponent';

// Export types
export type { ButtonComponentProps } from './types';
export type { ButtonIconComponentProps } from './types';

// Export all button components for convenience
export const Buttons = {
  // Main component
  Button: ButtonComponent,
  
  // Variant wrappers
  Apply: ButtonApplyComponent,
  Cancel: ButtonCancelComponent,
  Full: ButtonFullComponent,
  Outline: ButtonOutlineComponent,
  Reject: ButtonRejectComponent,
  
  // Icon buttons
  Close: ButtonClosePopupComponent,
  Edit: ButtonPenComponent,
  Delete: ButtonDeleteComponent,
  Excel: ButtonExcelComponent,
  Copy: ButtonCopyComponent,
  Back: ButtonBackComponent,
  
  // Toggle buttons
  Enable: EnableButtonComponent,
  Disable: DisableButtonComponent,
  
  // Tab buttons
  Tab: ButtonTabComponent,
  TabActive: ButtonTabActiveComponent,
  TabInactive: ButtonTabInactiveComponent,
} as const;
