import React from 'react';

/**
 * Base props for all button components
 */
export interface ButtonComponentProps {
  /** Button text content */
  text?: string;
  /** Additional CSS classes */
  className?: string;
  /** Click event handler */
  onClick?: (event: React.MouseEvent<HTMLButtonElement>) => void;
  /** Whether the button is disabled */
  disabled?: boolean;
  /** Whether the button is in active state (for toggle buttons) */
  isActive?: boolean;
  /** HTML button type */
  type?: 'button' | 'submit' | 'reset';
  /** Tooltip text */
  title?: string;
  /** Visual variant of the button */
  variant?: 'apply' | 'cancel' | 'full' | 'outline' | 'reject';
  /** Accessibility label for screen readers */
  'aria-label'?: string;
  /** Accessibility description */
  'aria-describedby'?: string;
  /** Additional HTML attributes */
  [key: string]: any;
}

/**
 * Props for icon button components
 * Extends ButtonComponentProps but removes text and variant since icon buttons don't use them
 */
export interface ButtonIconComponentProps extends Omit<ButtonComponentProps, 'text' | 'variant'> {
  /** Optional tooltip text */
  tooltip?: string;
}

/**
 * Props for toggle button components
 */
export interface ButtonToggleComponentProps extends ButtonComponentProps {
  /** Whether the toggle is in enabled state */
  isEnabled?: boolean;
  /** Text for enabled state */
  enabledText?: string;
  /** Text for disabled state */
  disabledText?: string;
}

/**
 * Props for tab button components
 */
export interface ButtonTabComponentProps extends ButtonComponentProps {
  /** Whether this tab is currently active */
  isActive?: boolean;
  /** Tab index for accessibility */
  tabIndex?: number;
}

/**
 * Button variant type for better type safety
 */
export type ButtonVariant = 'apply' | 'cancel' | 'full' | 'outline' | 'reject';

/**
 * Button type for form submission
 */
export type ButtonType = 'button' | 'submit' | 'reset';

/**
 * Common button sizes
 */
export type ButtonSize = 'small' | 'medium' | 'large';

/**
 * Extended button props with size
 */
export interface ButtonWithSizeProps extends ButtonComponentProps {
  /** Button size */
  size?: ButtonSize;
}