import React from 'react';
import { describe, it, expect, vi } from 'vitest';
import { render, screen, fireEvent } from '@testing-library/react';
import '@testing-library/jest-dom';
import { ButtonComponent } from './ButtonComponent';
import { ButtonVariant } from './types';

describe('ButtonComponent', () => {
  const mockOnClick = vi.fn();
  const defaultProps = {
    text: 'Test Button',
    onClick: mockOnClick,
  };

  beforeEach(() => {
    mockOnClick.mockClear();
  });

  describe('Rendering', () => {
    it('renders with default props', () => {
      render(<ButtonComponent {...defaultProps} />);
      
      const button = screen.getByRole('button', { name: 'Test Button' });
      expect(button).toBeInTheDocument();
      expect(button).toHaveTextContent('Test Button');
    });

    it('renders with custom className', () => {
      render(<ButtonComponent {...defaultProps} className="custom-class" />);
      
      const button = screen.getByRole('button', { name: 'Test Button' });
      expect(button).toHaveClass('custom-class');
    });

    it('renders with title attribute', () => {
      render(<ButtonComponent {...defaultProps} title="Tooltip text" text="Test Button" />);
      
      const button = screen.getByTitle('Tooltip text');
      expect(button).toHaveAttribute('title', 'Tooltip text');
      expect(button).toHaveAttribute('aria-label', 'Tooltip text');
    });

    it('renders without text', () => {
      render(<ButtonComponent onClick={mockOnClick} />);
      
      const button = screen.getByRole('button');
      expect(button).toBeInTheDocument();
      expect(button).toHaveTextContent('');
    });
  });

  describe('Button variants', () => {
    const variants: ButtonVariant[] = ['apply', 'cancel', 'full', 'outline', 'reject'];

    variants.forEach((variant) => {
      it(`renders with ${variant} variant`, () => {
        render(<ButtonComponent {...defaultProps} variant={variant} />);
        
        const button = screen.getByRole('button', { name: 'Test Button' });
        expect(button).toBeInTheDocument();
        
        // Check that the variant classes are applied
        const variantClasses = {
          apply: 'text-[var(--button-text)] bg-[var(--button-background)] hover:bg-[var(--button-background-hover)]',
          cancel: 'bg-[var(--button-background-cancel)] hover:bg-[var(--button-background-cancel-hover)] text-[var(--button-text)]',
          full: 'bg-[var(--button-background)] hover:bg-[var(--button-background-hover)] text-[var(--button-text)]',
          outline: 'border-[1px] border-[var(--button-outline-border)] bg-[var(--button-outline-background)] hover:bg-[var(--button-outline-background-hover)] hover:text-[var(--button-outline-text-hover)] text-[var(--button-outline-text)]',
          reject: 'text-[var(--button-text)] bg-[var(--button-background-reject)] hover:bg-[var(--button-background-reject-hover)]'
        };
        
        expect(button.className).toContain(variantClasses[variant]);
      });
    });

    it('defaults to apply variant when no variant is specified', () => {
      render(<ButtonComponent {...defaultProps} />);
      
      const button = screen.getByRole('button', { name: 'Test Button' });
      expect(button.className).toContain('text-[var(--button-text)] bg-[var(--button-background)] hover:bg-[var(--button-background-hover)]');
    });

    it('defaults to apply variant when invalid variant is provided', () => {
      render(<ButtonComponent {...defaultProps} variant={'invalid' as ButtonVariant} />);
      
      const button = screen.getByRole('button', { name: 'Test Button' });
      expect(button.className).toContain('text-[var(--button-text)] bg-[var(--button-background)] hover:bg-[var(--button-background-hover)]');
    });
  });

  describe('Button types', () => {
    it('renders with button type by default', () => {
      render(<ButtonComponent {...defaultProps} />);
      
      const button = screen.getByRole('button', { name: 'Test Button' });
      expect(button).toHaveAttribute('type', 'button');
    });

    it('renders with submit type', () => {
      render(<ButtonComponent {...defaultProps} type="submit" />);
      
      const button = screen.getByRole('button', { name: 'Test Button' });
      expect(button).toHaveAttribute('type', 'submit');
    });

    it('renders with reset type', () => {
      render(<ButtonComponent {...defaultProps} type="reset" />);
      
      const button = screen.getByRole('button', { name: 'Test Button' });
      expect(button).toHaveAttribute('type', 'reset');
    });
  });

  describe('Disabled state', () => {
    it('renders enabled by default', () => {
      render(<ButtonComponent {...defaultProps} />);
      
      const button = screen.getByRole('button', { name: 'Test Button' });
      expect(button).not.toBeDisabled();
      expect(button).toHaveClass('cursor-pointer');
      expect(button).not.toHaveClass('cursor-not-allowed', 'opacity-80');
    });

    it('renders disabled when disabled prop is true', () => {
      render(<ButtonComponent {...defaultProps} disabled={true} />);
      
      const button = screen.getByRole('button', { name: 'Test Button' });
      expect(button).toBeDisabled();
      expect(button).toHaveClass('cursor-not-allowed', 'opacity-80');
      expect(button).not.toHaveClass('cursor-pointer');
    });

    it('does not call onClick when disabled', () => {
      render(<ButtonComponent {...defaultProps} disabled={true} />);
      
      const button = screen.getByRole('button', { name: 'Test Button' });
      fireEvent.click(button);
      
      expect(mockOnClick).not.toHaveBeenCalled();
    });
  });

  describe('Click handling', () => {
    it('calls onClick when clicked', () => {
      render(<ButtonComponent {...defaultProps} />);
      
      const button = screen.getByRole('button', { name: 'Test Button' });
      fireEvent.click(button);
      
      expect(mockOnClick).toHaveBeenCalledTimes(1);
    });

    it('calls onClick with event object', () => {
      render(<ButtonComponent {...defaultProps} />);
      
      const button = screen.getByRole('button', { name: 'Test Button' });
      fireEvent.click(button);
      
      expect(mockOnClick).toHaveBeenCalledWith(expect.any(Object));
    });

    it('does not call onClick when no onClick handler is provided', () => {
      render(<ButtonComponent text="Test Button" />);
      
      const button = screen.getByRole('button', { name: 'Test Button' });
      fireEvent.click(button);
      
      // Should not throw an error
      expect(button).toBeInTheDocument();
    });
  });

  describe('Base classes', () => {
    it('applies base classes correctly', () => {
      render(<ButtonComponent {...defaultProps} />);
      
      const button = screen.getByRole('button', { name: 'Test Button' });
      expect(button.className).toContain('min-w-[100px]');
      expect(button.className).toContain('rounded-[3px]');
      expect(button.className).toContain('h-[28px]');
      expect(button.className).toContain('pt-[1px]');
      expect(button.className).toContain('px-[10px]');
      expect(button.className).toContain('font-semibold');
      expect(button.className).toContain('outline-none');
    });
  });

  describe('Forwarded ref', () => {
    it('forwards ref correctly', () => {
      const ref = React.createRef<HTMLButtonElement>();
      render(<ButtonComponent {...defaultProps} ref={ref} />);
      
      expect(ref.current).toBeInstanceOf(HTMLButtonElement);
      expect(ref.current).toHaveTextContent('Test Button');
    });
  });

  describe('Additional props', () => {
    it('passes through additional HTML attributes', () => {
      render(
        <ButtonComponent 
          {...defaultProps} 
          data-testid="custom-button"
          aria-describedby="description"
        />
      );
      
      const button = screen.getByTestId('custom-button');
      expect(button).toHaveAttribute('data-testid', 'custom-button');
      expect(button).toHaveAttribute('aria-describedby', 'description');
    });

    it('passes through custom event handlers', () => {
      const mockOnMouseEnter = vi.fn();
      const mockOnMouseLeave = vi.fn();
      
      render(
        <ButtonComponent 
          {...defaultProps} 
          onMouseEnter={mockOnMouseEnter}
          onMouseLeave={mockOnMouseLeave}
        />
      );
      
      const button = screen.getByRole('button', { name: 'Test Button' });
      fireEvent.mouseEnter(button);
      fireEvent.mouseLeave(button);
      
      expect(mockOnMouseEnter).toHaveBeenCalledTimes(1);
      expect(mockOnMouseLeave).toHaveBeenCalledTimes(1);
    });
  });

  describe('Accessibility', () => {
    it('has correct role', () => {
      render(<ButtonComponent {...defaultProps} />);
      
      const button = screen.getByRole('button', { name: 'Test Button' });
      expect(button).toBeInTheDocument();
    });

    it('uses title as aria-label when provided', () => {
      render(<ButtonComponent {...defaultProps} title="Accessibility label" />);
      
      const button = screen.getByTitle('Accessibility label');
      expect(button).toHaveAttribute('aria-label', 'Accessibility label');
    });

    it('does not have aria-label when no title is provided', () => {
      render(<ButtonComponent {...defaultProps} />);
      
      const button = screen.getByRole('button', { name: 'Test Button' });
      expect(button).not.toHaveAttribute('aria-label');
    });
  });
}); 