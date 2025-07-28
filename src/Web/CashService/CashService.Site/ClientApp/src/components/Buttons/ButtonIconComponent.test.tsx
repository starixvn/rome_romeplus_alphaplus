import React from 'react';
import { describe, it, expect, vi, beforeEach } from 'vitest';
import { render, screen, fireEvent } from '@testing-library/react';
import '@testing-library/jest-dom';
import { ButtonIconComponent } from './ButtonIconComponent';

// Mock the translation hook
vi.mock('react-i18next', () => ({
  useTranslation: () => ({
    t: (key: string) => key === 'back' ? 'Back' : key,
  }),
}));

describe('ButtonIconComponent', () => {
  const mockOnClick = vi.fn();
  const defaultProps = {
    onClick: mockOnClick,
  };

  beforeEach(() => {
    mockOnClick.mockClear();
  });

  describe('Rendering', () => {
    it('renders with default props', () => {
      render(<ButtonIconComponent {...defaultProps} />);
      
      const button = screen.getByRole('button');
      expect(button).toBeInTheDocument();
      expect(button).toHaveAttribute('type', 'button');
    });

    it('renders with custom className', () => {
      render(<ButtonIconComponent {...defaultProps} className="custom-class" />);
      
      const button = screen.getByRole('button');
      expect(button).toHaveClass('custom-class');
    });

    it('renders with custom icon', () => {
      const CustomIcon = () => <span data-testid="custom-icon">Custom Icon</span>;
      render(<ButtonIconComponent {...defaultProps} icon={<CustomIcon />} />);
      
      expect(screen.getByTestId('custom-icon')).toBeInTheDocument();
    });
  });

  describe('Button styles', () => {
    const styles = ['Close', 'Edit', 'Delete', 'Excel', 'Copy', 'Back'] as const;

    styles.forEach((style) => {
      it(`renders with ${style} style`, () => {
        render(<ButtonIconComponent {...defaultProps} style={style} />);
        
        const button = screen.getByRole('button');
        expect(button).toBeInTheDocument();
        expect(button).toHaveAttribute('aria-label', style);
      });
    });

    it('renders Close style with correct classes', () => {
      render(<ButtonIconComponent {...defaultProps} style="Close" />);
      
      const button = screen.getByRole('button');
      expect(button.className).toContain('hover:bg-[var(--button-icon-hover)]');
      expect(button.className).toContain('absolute');
      expect(button.className).toContain('top-[11px]');
      expect(button.className).toContain('right-[-5px]');
    });

    it('renders Edit style with correct classes', () => {
      render(<ButtonIconComponent {...defaultProps} style="Edit" />);
      
      const button = screen.getByRole('button');
      expect(button.className).toContain('hover:bg-[var(--button-icon-hover)]');
      expect(button.className).toContain('mx-[4px]');
      expect(button.className).toContain('p-[4px]');
      expect(button.className).toContain('rounded-[3px]');
    });

    it('renders Delete style with correct classes', () => {
      render(<ButtonIconComponent {...defaultProps} style="Delete" />);
      
      const button = screen.getByRole('button');
      expect(button.className).toContain('hover:bg-[var(--button-icon-hover)]');
      expect(button.className).toContain('mx-[4px]');
      expect(button.className).toContain('py-[6px]');
      expect(button.className).toContain('px-[6px]');
      expect(button.className).toContain('rounded-[3px]');
    });

    it('renders Excel style with correct classes', () => {
      render(<ButtonIconComponent {...defaultProps} style="Excel" />);
      
      const button = screen.getByRole('button');
      expect(button.className).toContain('hover:bg-[var(--button-icon-hover)]');
      expect(button.className).toContain('mx-[4px]');
      expect(button.className).toContain('p-[4px]');
      expect(button.className).toContain('bg-black/10');
      expect(button.className).toContain('rounded-[3px]');
    });

    it('renders Copy style with correct classes', () => {
      render(<ButtonIconComponent {...defaultProps} style="Copy" />);
      
      const button = screen.getByRole('button');
      expect(button.className).toContain('hover:bg-[var(--button-icon-hover)]');
      expect(button.className).toContain('mx-[4px]');
      expect(button.className).toContain('py-[4px]');
      expect(button.className).toContain('px-[4px]');
      expect(button.className).toContain('bg-black/10');
      expect(button.className).toContain('rounded-[3px]');
    });

    it('renders Back style with correct classes and text', () => {
      render(<ButtonIconComponent {...defaultProps} style="Back" />);
      
      const button = screen.getByRole('button');
      expect(button.className).toContain('min-w-[90px]');
      expect(button.className).toContain('text-[var(--button-back-text)]');
      expect(button.className).toContain('hover:text-[var(--button-back-text-hover)]');
      expect(button.className).toContain('border-[1px]');
      expect(button.className).toContain('border-[var(--button-back-border)]');
      expect(button.className).toContain('rounded-[20px]');
      expect(button.className).toContain('mr-[16px]');
      expect(button.className).toContain('bg-[var(--button-back-background)]');
      expect(button.className).toContain('hover:bg-[var(--button-back-background-hover)]');
      expect(button.className).toContain('hover:border-[transparent]');
      expect(button.className).toContain('group');
      expect(button.className).toContain('mb-[8px]');
      expect(button.className).toContain('inline-flex');
      expect(button.className).toContain('items-center');
      expect(button.className).toContain('py-[4px]');
      expect(button.className).toContain('pl-[8px]');
      expect(button.className).toContain('pr-[16px]');
      
      // Check for text content
      expect(button).toHaveTextContent('Back');
    });

    it('renders with default style when no style is provided', () => {
      render(<ButtonIconComponent {...defaultProps} />);
      
      const button = screen.getByRole('button');
      expect(button.className).toContain('mx-[4px]');
      expect(button.className).toContain('py-[4px]');
      expect(button.className).toContain('px-[4px]');
      expect(button.className).toContain('rounded-[3px]');
    });
  });

  describe('Button types', () => {
    it('renders with button type by default', () => {
      render(<ButtonIconComponent {...defaultProps} />);
      
      const button = screen.getByRole('button');
      expect(button).toHaveAttribute('type', 'button');
    });

    it('renders with submit type', () => {
      render(<ButtonIconComponent {...defaultProps} type="submit" />);
      
      const button = screen.getByRole('button');
      expect(button).toHaveAttribute('type', 'submit');
    });

    it('renders with reset type', () => {
      render(<ButtonIconComponent {...defaultProps} type="reset" />);
      
      const button = screen.getByRole('button');
      expect(button).toHaveAttribute('type', 'reset');
    });
  });

  describe('Disabled state', () => {
    it('renders enabled by default', () => {
      render(<ButtonIconComponent {...defaultProps} />);
      
      const button = screen.getByRole('button');
      expect(button).not.toBeDisabled();
      expect(button).toHaveClass('cursor-pointer');
      expect(button).not.toHaveClass('cursor-not-allowed', 'opacity-80');
    });

    it('renders disabled when disabled prop is true', () => {
      render(<ButtonIconComponent {...defaultProps} disabled={true} />);
      
      const button = screen.getByRole('button');
      expect(button).toBeDisabled();
      expect(button).toHaveClass('cursor-not-allowed', 'opacity-80');
      expect(button).not.toHaveClass('cursor-pointer');
    });

    it('does not call onClick when disabled', () => {
      render(<ButtonIconComponent {...defaultProps} disabled={true} />);
      
      const button = screen.getByRole('button');
      fireEvent.click(button);
      
      expect(mockOnClick).not.toHaveBeenCalled();
    });
  });

  describe('Click handling', () => {
    it('calls onClick when clicked', () => {
      render(<ButtonIconComponent {...defaultProps} />);
      
      const button = screen.getByRole('button');
      fireEvent.click(button);
      
      expect(mockOnClick).toHaveBeenCalledTimes(1);
    });

    it('calls onClick with event object', () => {
      render(<ButtonIconComponent {...defaultProps} />);
      
      const button = screen.getByRole('button');
      fireEvent.click(button);
      
      expect(mockOnClick).toHaveBeenCalledWith(expect.any(Object));
    });
  });

  describe('Icon rendering', () => {
    it('renders default icon for Close style', () => {
      render(<ButtonIconComponent {...defaultProps} style="Close" />);
      
      const button = screen.getByRole('button');
      // The IconClose component should be rendered
      expect(button.querySelector('svg')).toBeInTheDocument();
    });

    it('renders default icon for Edit style', () => {
      render(<ButtonIconComponent {...defaultProps} style="Edit" />);
      
      const button = screen.getByRole('button');
      // The IconPen component should be rendered
      expect(button.querySelector('svg')).toBeInTheDocument();
    });

    it('renders default icon for Delete style', () => {
      render(<ButtonIconComponent {...defaultProps} style="Delete" />);
      
      const button = screen.getByRole('button');
      // The IconDelete component should be rendered
      expect(button.querySelector('svg')).toBeInTheDocument();
    });

    it('renders default icon for Excel style', () => {
      render(<ButtonIconComponent {...defaultProps} style="Excel" />);
      
      const button = screen.getByRole('button');
      // The IconExcel component should be rendered
      expect(button.querySelector('svg')).toBeInTheDocument();
    });

    it('renders default icon for Copy style', () => {
      render(<ButtonIconComponent {...defaultProps} style="Copy" />);
      
      const button = screen.getByRole('button');
      // The IconCopy component should be rendered
      expect(button.querySelector('svg')).toBeInTheDocument();
    });

    it('renders default icon for Back style', () => {
      render(<ButtonIconComponent {...defaultProps} style="Back" />);
      
      const button = screen.getByRole('button');
      // The IconBack component should be rendered
      expect(button.querySelector('svg')).toBeInTheDocument();
    });

    it('renders custom icon when provided', () => {
      const CustomIcon = () => <span data-testid="custom-icon">Custom Icon</span>;
      render(<ButtonIconComponent {...defaultProps} icon={<CustomIcon />} />);
      
      expect(screen.getByTestId('custom-icon')).toBeInTheDocument();
    });

    it('prioritizes custom icon over default icon', () => {
      const CustomIcon = () => <span data-testid="custom-icon">Custom Icon</span>;
      render(<ButtonIconComponent {...defaultProps} style="Close" icon={<CustomIcon />} />);
      
      expect(screen.getByTestId('custom-icon')).toBeInTheDocument();
      // Should not render the default Close icon
      const button = screen.getByRole('button');
      expect(button.querySelector('svg')).not.toBeInTheDocument();
    });
  });

  describe('Text rendering', () => {
    it('renders text for Back style', () => {
      render(<ButtonIconComponent {...defaultProps} style="Back" />);
      
      const button = screen.getByRole('button');
      expect(button).toHaveTextContent('Back');
      
      const textSpan = button.querySelector('span');
      expect(textSpan).toHaveClass('font-semibold');
    });

    it('does not render text for other styles', () => {
      render(<ButtonIconComponent {...defaultProps} style="Close" />);
      
      const button = screen.getByRole('button');
      expect(button).not.toHaveTextContent('Back');
    });

    it('does not render text span for non-Back styles', () => {
      render(<ButtonIconComponent {...defaultProps} style="Edit" />);
      
      const button = screen.getByRole('button');
      const textSpan = button.querySelector('span');
      expect(button.querySelector('span')).toBeNull();
    });
  });

  describe('Base classes', () => {
    it('applies focus outline class correctly', () => {
      render(<ButtonIconComponent {...defaultProps} />);
      
      const button = screen.getByRole('button');
      expect(button.className).toContain('focus:outline-none');
    });
  });

  describe('Accessibility', () => {
    it('has correct aria-label based on style', () => {
      render(<ButtonIconComponent {...defaultProps} style="Close" />);
      
      const button = screen.getByRole('button');
      expect(button).toHaveAttribute('aria-label', 'Close');
    });

    it('has correct aria-label for Back style', () => {
      render(<ButtonIconComponent {...defaultProps} style="Back" />);
      
      const button = screen.getByRole('button');
      expect(button).toHaveAttribute('aria-label', 'Back');
    });

    it('has correct role', () => {
      render(<ButtonIconComponent {...defaultProps} />);
      
      const button = screen.getByRole('button');
      expect(button).toBeInTheDocument();
    });
  });

  describe('Combined props', () => {
    it('renders with all props correctly', () => {
      const CustomIcon = () => <span data-testid="custom-icon">Custom Icon</span>;
      render(
        <ButtonIconComponent 
          {...defaultProps} 
          style="Edit"
          type="submit"
          disabled={true}
          className="custom-class"
          icon={<CustomIcon />}
        />
      );
      
      const button = screen.getByRole('button');
      expect(button).toBeInTheDocument();
      expect(button).toHaveAttribute('type', 'submit');
      expect(button).toHaveAttribute('aria-label', 'Edit');
      expect(button).toBeDisabled();
      expect(button).toHaveClass('custom-class');
      expect(button).toHaveClass('cursor-not-allowed', 'opacity-80');
      expect(screen.getByTestId('custom-icon')).toBeInTheDocument();
    });

    it('handles style and custom icon together', () => {
      const CustomIcon = () => <span data-testid="custom-icon">Custom Icon</span>;
      render(
        <ButtonIconComponent 
          {...defaultProps} 
          style="Back"
          icon={<CustomIcon />}
        />
      );
      
      const button = screen.getByRole('button');
      expect(button).toHaveTextContent('Back');
      expect(screen.getByTestId('custom-icon')).toBeInTheDocument();
    });
  });
}); 