import React from 'react';
import { describe, it, expect, vi, beforeEach } from 'vitest';
import { render, screen, fireEvent } from '@testing-library/react';
import '@testing-library/jest-dom';
import { EnableButtonComponent, DisableButtonComponent } from './ButtonToggleComponent';
import { ButtonComponentProps } from './types';

// Mock react-i18next
vi.mock('react-i18next', () => ({
  useTranslation: () => ({
    t: (key: string) => {
      const translations = {
        enable: 'Enable',
        disable: 'Disable'
      };
      return translations[key] || key;
    }
  })
}));

describe('ButtonToggleComponent', () => {
  const mockOnClick = vi.fn();
  const defaultProps: ButtonComponentProps = {
    onClick: mockOnClick,
  };

  beforeEach(() => {
    mockOnClick.mockClear();
  });

  describe('ToggleButton Base Component', () => {
    describe('EnableButtonComponent', () => {
      describe('Rendering', () => {
        it('renders with default enable text', () => {
          render(<EnableButtonComponent {...defaultProps} />);
          
          const button = screen.getByRole('button', { name: 'Enable' });
          expect(button).toBeInTheDocument();
          expect(button).toHaveTextContent('Enable');
        });

        it('renders with custom text', () => {
          render(<EnableButtonComponent {...defaultProps} text="Custom Enable" />);
          
          const button = screen.getByRole('button', { name: 'Custom Enable' });
          expect(button).toBeInTheDocument();
          expect(button).toHaveTextContent('Custom Enable');
        });

        it('renders with custom className', () => {
          render(<EnableButtonComponent {...defaultProps} className="custom-class" />);
          
          const button = screen.getByRole('button', { name: 'Enable' });
          expect(button).toHaveClass('custom-class');
        });

        it('renders with left position styling', () => {
          render(<EnableButtonComponent {...defaultProps} />);
          
          const button = screen.getByRole('button', { name: 'Enable' });
          expect(button.className).toContain('rounded-l-[4px]');
          expect(button.className).not.toContain('rounded-r-[4px]');
        });
      });

      describe('Active state', () => {
        it('applies active styling when isActive is true', () => {
          render(<EnableButtonComponent {...defaultProps} isActive={true} />);
          
          const button = screen.getByRole('button', { name: 'Enable' });
          expect(button).toHaveClass('bg-[var(--button-switch-toggle-background-active)]');
          expect(button).toHaveClass('text-[var(--button-switch-toggle-color-active)]');
          expect(button).toHaveAttribute('aria-pressed', 'true');
        });

        it('applies inactive styling when isActive is false', () => {
          render(<EnableButtonComponent {...defaultProps} isActive={false} />);
          
          const button = screen.getByRole('button', { name: 'Enable' });
          expect(button).toHaveClass('bg-[var(--button-switch-toggle-background)]');
          expect(button).toHaveClass('text-[var(--button-switch-toggle-color)]');
          expect(button).toHaveAttribute('aria-pressed', 'false');
        });

        it('defaults to inactive state when isActive is not provided', () => {
          render(<EnableButtonComponent {...defaultProps} />);
          
          const button = screen.getByRole('button', { name: 'Enable' });
          expect(button).toHaveClass('bg-[var(--button-switch-toggle-background)]');
          expect(button).toHaveClass('text-[var(--button-switch-toggle-color)]');
          expect(button).toHaveAttribute('aria-pressed', 'false');
        });
      });

      describe('Disabled state', () => {
        it('applies disabled styling when disabled is true', () => {
          render(<EnableButtonComponent {...defaultProps} disabled={true} />);
          
          const button = screen.getByRole('button', { name: 'Enable' });
          expect(button).toHaveClass('cursor-not-allowed');
          expect(button).toHaveClass('opacity-80');
          expect(button).toBeDisabled();
        });

        it('applies enabled styling when disabled is false', () => {
          render(<EnableButtonComponent {...defaultProps} disabled={false} />);
          
          const button = screen.getByRole('button', { name: 'Enable' });
          expect(button).toHaveClass('cursor-pointer');
          expect(button).not.toBeDisabled();
        });

        it('defaults to enabled state when disabled is not provided', () => {
          render(<EnableButtonComponent {...defaultProps} />);
          
          const button = screen.getByRole('button', { name: 'Enable' });
          expect(button).toHaveClass('cursor-pointer');
          expect(button).not.toBeDisabled();
        });
      });

      describe('Button types', () => {
        it('renders with button type by default', () => {
          render(<EnableButtonComponent {...defaultProps} />);
          
          const button = screen.getByRole('button', { name: 'Enable' });
          expect(button).toHaveAttribute('type', 'button');
        });

        it('renders with submit type', () => {
          render(<EnableButtonComponent {...defaultProps} type="submit" />);
          
          const button = screen.getByRole('button', { name: 'Enable' });
          expect(button).toHaveAttribute('type', 'submit');
        });

        it('renders with reset type', () => {
          render(<EnableButtonComponent {...defaultProps} type="reset" />);
          
          const button = screen.getByRole('button', { name: 'Enable' });
          expect(button).toHaveAttribute('type', 'reset');
        });
      });

      describe('Click handling', () => {
        it('calls onClick when clicked', () => {
          render(<EnableButtonComponent {...defaultProps} />);
          
          const button = screen.getByRole('button', { name: 'Enable' });
          fireEvent.click(button);
          
          expect(mockOnClick).toHaveBeenCalledWith(expect.any(Object));
        });

        it('does not call onClick when disabled', () => {
          render(<EnableButtonComponent {...defaultProps} disabled={true} />);
          
          const button = screen.getByRole('button', { name: 'Enable' });
          fireEvent.click(button);
          
          expect(mockOnClick).not.toHaveBeenCalled();
        });

        it('does not call onClick when no onClick handler is provided', () => {
          render(<EnableButtonComponent text="Enable" />);
          
          const button = screen.getByRole('button', { name: 'Enable' });
          fireEvent.click(button);
          
          // Should not throw an error
          expect(button).toBeInTheDocument();
        });
      });

      describe('Base classes', () => {
        it('applies base classes correctly', () => {
          render(<EnableButtonComponent {...defaultProps} />);
          
          const button = screen.getByRole('button', { name: 'Enable' });
          expect(button.className).toContain('w-[90px]');
          expect(button.className).toContain('p-[4px]');
          expect(button.className).toContain('outline-none');
          expect(button.className).toContain('font-semibold');
        });
      });

      describe('Forwarded ref', () => {
        it('forwards ref correctly', () => {
          const ref = React.createRef<HTMLButtonElement>();
          render(<EnableButtonComponent {...defaultProps} ref={ref} />);
          
          expect(ref.current).toBeInstanceOf(HTMLButtonElement);
          expect(ref.current).toHaveTextContent('Enable');
        });
      });

      describe('Additional props', () => {
        it('passes through additional HTML attributes', () => {
          render(
            <EnableButtonComponent 
              {...defaultProps} 
              data-testid="enable-button"
              aria-describedby="description"
            />
          );
          
          const button = screen.getByTestId('enable-button');
          expect(button).toHaveAttribute('data-testid', 'enable-button');
          expect(button).toHaveAttribute('aria-describedby', 'description');
        });

        it('passes through custom event handlers', () => {
          const mockOnMouseEnter = vi.fn();
          const mockOnMouseLeave = vi.fn();
          
          render(
            <EnableButtonComponent 
              {...defaultProps} 
              onMouseEnter={mockOnMouseEnter}
              onMouseLeave={mockOnMouseLeave}
            />
          );
          
          const button = screen.getByRole('button', { name: 'Enable' });
          fireEvent.mouseEnter(button);
          fireEvent.mouseLeave(button);
          
          expect(mockOnMouseEnter).toHaveBeenCalledTimes(1);
          expect(mockOnMouseLeave).toHaveBeenCalledTimes(1);
        });
      });

      describe('Accessibility', () => {
        it('has correct role', () => {
          render(<EnableButtonComponent {...defaultProps} />);
          
          const button = screen.getByRole('button', { name: 'Enable' });
          expect(button).toBeInTheDocument();
        });

        it('has aria-pressed attribute', () => {
          render(<EnableButtonComponent {...defaultProps} isActive={true} />);
          
          const button = screen.getByRole('button', { name: 'Enable' });
          expect(button).toHaveAttribute('aria-pressed', 'true');
        });
      });
    });

    describe('DisableButtonComponent', () => {
      describe('Rendering', () => {
        it('renders with default disable text', () => {
          render(<DisableButtonComponent {...defaultProps} />);
          
          const button = screen.getByRole('button', { name: 'Disable' });
          expect(button).toBeInTheDocument();
          expect(button).toHaveTextContent('Disable');
        });

        it('renders with custom text', () => {
          render(<DisableButtonComponent {...defaultProps} text="Custom Disable" />);
          
          const button = screen.getByRole('button', { name: 'Custom Disable' });
          expect(button).toBeInTheDocument();
          expect(button).toHaveTextContent('Custom Disable');
        });

        it('renders with custom className', () => {
          render(<DisableButtonComponent {...defaultProps} className="custom-class" />);
          
          const button = screen.getByRole('button', { name: 'Disable' });
          expect(button).toHaveClass('custom-class');
        });

        it('renders with right position styling', () => {
          render(<DisableButtonComponent {...defaultProps} />);
          
          const button = screen.getByRole('button', { name: 'Disable' });
          expect(button.className).toContain('rounded-r-[4px]');
          expect(button.className).not.toContain('rounded-l-[4px]');
        });
      });

      describe('Active state', () => {
        it('applies active styling when isActive is true', () => {
          render(<DisableButtonComponent {...defaultProps} isActive={true} />);
          
          const button = screen.getByRole('button', { name: 'Disable' });
          expect(button).toHaveClass('bg-[var(--button-switch-toggle-background-active)]');
          expect(button).toHaveClass('text-[var(--button-switch-toggle-color-active)]');
          expect(button).toHaveAttribute('aria-pressed', 'true');
        });

        it('applies inactive styling when isActive is false', () => {
          render(<DisableButtonComponent {...defaultProps} isActive={false} />);
          
          const button = screen.getByRole('button', { name: 'Disable' });
          expect(button).toHaveClass('bg-[var(--button-switch-toggle-background)]');
          expect(button).toHaveClass('text-[var(--button-switch-toggle-color)]');
          expect(button).toHaveAttribute('aria-pressed', 'false');
        });

        it('defaults to inactive state when isActive is not provided', () => {
          render(<DisableButtonComponent {...defaultProps} />);
          
          const button = screen.getByRole('button', { name: 'Disable' });
          expect(button).toHaveClass('bg-[var(--button-switch-toggle-background)]');
          expect(button).toHaveClass('text-[var(--button-switch-toggle-color)]');
          expect(button).toHaveAttribute('aria-pressed', 'false');
        });
      });

      describe('Disabled state', () => {
        it('applies disabled styling when disabled is true', () => {
          render(<DisableButtonComponent {...defaultProps} disabled={true} />);
          
          const button = screen.getByRole('button', { name: 'Disable' });
          expect(button).toHaveClass('cursor-not-allowed');
          expect(button).toHaveClass('opacity-80');
          expect(button).toBeDisabled();
        });

        it('applies enabled styling when disabled is false', () => {
          render(<DisableButtonComponent {...defaultProps} disabled={false} />);
          
          const button = screen.getByRole('button', { name: 'Disable' });
          expect(button).toHaveClass('cursor-pointer');
          expect(button).not.toBeDisabled();
        });

        it('defaults to enabled state when disabled is not provided', () => {
          render(<DisableButtonComponent {...defaultProps} />);
          
          const button = screen.getByRole('button', { name: 'Disable' });
          expect(button).toHaveClass('cursor-pointer');
          expect(button).not.toBeDisabled();
        });
      });

      describe('Button types', () => {
        it('renders with button type by default', () => {
          render(<DisableButtonComponent {...defaultProps} />);
          
          const button = screen.getByRole('button', { name: 'Disable' });
          expect(button).toHaveAttribute('type', 'button');
        });

        it('renders with submit type', () => {
          render(<DisableButtonComponent {...defaultProps} type="submit" />);
          
          const button = screen.getByRole('button', { name: 'Disable' });
          expect(button).toHaveAttribute('type', 'submit');
        });

        it('renders with reset type', () => {
          render(<DisableButtonComponent {...defaultProps} type="reset" />);
          
          const button = screen.getByRole('button', { name: 'Disable' });
          expect(button).toHaveAttribute('type', 'reset');
        });
      });

      describe('Click handling', () => {
        it('calls onClick when clicked', () => {
          render(<DisableButtonComponent {...defaultProps} />);
          
          const button = screen.getByRole('button', { name: 'Disable' });
          fireEvent.click(button);
          
          expect(mockOnClick).toHaveBeenCalledWith(expect.any(Object));
        });

        it('does not call onClick when disabled', () => {
          render(<DisableButtonComponent {...defaultProps} disabled={true} />);
          
          const button = screen.getByRole('button', { name: 'Disable' });
          fireEvent.click(button);
          
          expect(mockOnClick).not.toHaveBeenCalled();
        });

        it('does not call onClick when no onClick handler is provided', () => {
          render(<DisableButtonComponent text="Disable" />);
          
          const button = screen.getByRole('button', { name: 'Disable' });
          fireEvent.click(button);
          
          // Should not throw an error
          expect(button).toBeInTheDocument();
        });
      });

      describe('Base classes', () => {
        it('applies base classes correctly', () => {
          render(<DisableButtonComponent {...defaultProps} />);
          
          const button = screen.getByRole('button', { name: 'Disable' });
          expect(button.className).toContain('w-[90px]');
          expect(button.className).toContain('p-[4px]');
          expect(button.className).toContain('outline-none');
          expect(button.className).toContain('font-semibold');
        });
      });

      describe('Forwarded ref', () => {
        it('forwards ref correctly', () => {
          const ref = React.createRef<HTMLButtonElement>();
          render(<DisableButtonComponent {...defaultProps} ref={ref} />);
          
          expect(ref.current).toBeInstanceOf(HTMLButtonElement);
          expect(ref.current).toHaveTextContent('Disable');
        });
      });

      describe('Additional props', () => {
        it('passes through additional HTML attributes', () => {
          render(
            <DisableButtonComponent 
              {...defaultProps} 
              data-testid="disable-button"
              aria-describedby="description"
            />
          );
          
          const button = screen.getByTestId('disable-button');
          expect(button).toHaveAttribute('data-testid', 'disable-button');
          expect(button).toHaveAttribute('aria-describedby', 'description');
        });

        it('passes through custom event handlers', () => {
          const mockOnMouseEnter = vi.fn();
          const mockOnMouseLeave = vi.fn();
          
          render(
            <DisableButtonComponent 
              {...defaultProps} 
              onMouseEnter={mockOnMouseEnter}
              onMouseLeave={mockOnMouseLeave}
            />
          );
          
          const button = screen.getByRole('button', { name: 'Disable' });
          fireEvent.mouseEnter(button);
          fireEvent.mouseLeave(button);
          
          expect(mockOnMouseEnter).toHaveBeenCalledTimes(1);
          expect(mockOnMouseLeave).toHaveBeenCalledTimes(1);
        });
      });

      describe('Accessibility', () => {
        it('has correct role', () => {
          render(<DisableButtonComponent {...defaultProps} />);
          
          const button = screen.getByRole('button', { name: 'Disable' });
          expect(button).toBeInTheDocument();
        });

        it('has aria-pressed attribute', () => {
          render(<DisableButtonComponent {...defaultProps} isActive={true} />);
          
          const button = screen.getByRole('button', { name: 'Disable' });
          expect(button).toHaveAttribute('aria-pressed', 'true');
        });
      });
    });
  });

  describe('Component Integration', () => {
    it('both components can be used together in a toggle group', () => {
      render(
        <div>
          <EnableButtonComponent {...defaultProps} isActive={true} />
          <DisableButtonComponent {...defaultProps} isActive={false} />
        </div>
      );
      
      const enableButton = screen.getByRole('button', { name: 'Enable' });
      const disableButton = screen.getByRole('button', { name: 'Disable' });
      
      expect(enableButton).toBeInTheDocument();
      expect(disableButton).toBeInTheDocument();
      expect(enableButton).toHaveAttribute('aria-pressed', 'true');
      expect(disableButton).toHaveAttribute('aria-pressed', 'false');
    });

    it('components have different border radius classes for proper styling', () => {
      render(
        <div>
          <EnableButtonComponent {...defaultProps} />
          <DisableButtonComponent {...defaultProps} />
        </div>
      );
      
      const enableButton = screen.getByRole('button', { name: 'Enable' });
      const disableButton = screen.getByRole('button', { name: 'Disable' });
      
      expect(enableButton.className).toContain('rounded-l-[4px]');
      expect(disableButton.className).toContain('rounded-r-[4px]');
    });
  });
}); 