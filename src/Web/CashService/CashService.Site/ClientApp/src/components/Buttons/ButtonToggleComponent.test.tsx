import { describe, it, expect, vi, beforeEach } from 'vitest';
import { render, screen, fireEvent } from '@testing-library/react';
import '@testing-library/jest-dom';
import React from 'react';
import {
  EnableButtonComponent,
  DisableButtonComponent,
  ButtonOnComponent,
  ButtonOffComponent
} from './ButtonToggleComponent';

// Mock the react-i18next module
vi.mock('react-i18next', () => ({
  useTranslation: () => ({
    t: (key: string) => {
      const translations = {
        enable: 'Enable',
        disable: 'Disable',
        on: 'On',
        off: 'Off'
      };
      return translations[key] || key;
    }
  })
}));

describe('ButtonToggleComponent', () => {
  describe('EnableButtonComponent', () => {
    it('renders enable button with default props', () => {
      render(<EnableButtonComponent onClick={() => {}} />);
      
      const button = screen.getByRole('button');
      expect(button).toBeInTheDocument();
      expect(button).toHaveTextContent('Enable');
      expect(button).toHaveAttribute('type', 'button');
      expect(button).toHaveClass(
        'w-[90px]',
        'p-[4px]',
        'cursor-pointer',
        'rounded-tl-[4px]',
        'rounded-bl-[4px]',
        'outline-none',
        'font-semibold'
      );
    });

    it('renders enable button with custom type', () => {
      render(<EnableButtonComponent onClick={() => {}} type="submit" />);
      
      const button = screen.getByRole('button');
      expect(button).toHaveAttribute('type', 'submit');
    });

    it('calls onClick when clicked', () => {
      const mockOnClick = vi.fn();
      render(<EnableButtonComponent onClick={mockOnClick} />);
      
      const button = screen.getByRole('button');
      fireEvent.click(button);
      
      expect(mockOnClick).toHaveBeenCalledTimes(1);
    });

    it('applies active styles when isActive is true', () => {
      render(<EnableButtonComponent onClick={() => {}} isActive={true} />);
      
      const button = screen.getByRole('button');
      expect(button).toHaveClass('bg-[var(--button-switch-toggle-background-active)]');
      expect(button).toHaveClass('text-[var(--button-switch-toggle-color-active)]');
    });

    it('applies inactive styles when isActive is false', () => {
      render(<EnableButtonComponent onClick={() => {}} isActive={false} />);
      
      const button = screen.getByRole('button');
      expect(button).toHaveClass('bg-[var(--button-switch-toggle-background)]');
      expect(button).toHaveClass('text-[var(--button-switch-toggle-color)]');
    });

    it('applies inactive styles when isActive is undefined', () => {
      render(<EnableButtonComponent onClick={() => {}} />);
      
      const button = screen.getByRole('button');
      expect(button).toHaveClass('bg-[var(--button-switch-toggle-background)]');
      expect(button).toHaveClass('text-[var(--button-switch-toggle-color)]');
    });

    it('has proper button role', () => {
      render(<EnableButtonComponent onClick={() => {}} />);
      
      const button = screen.getByRole('button');
      expect(button).toBeInTheDocument();
    });

    it('is keyboard accessible', () => {
      const mockOnClick = vi.fn();
      render(<EnableButtonComponent onClick={mockOnClick} />);
      
      const button = screen.getByRole('button');
      expect(button).not.toBeDisabled();
      
      fireEvent.click(button);
      expect(mockOnClick).toHaveBeenCalledTimes(1);
    });
  });

  describe('DisableButtonComponent', () => {
    it('renders disable button with default props', () => {
      render(<DisableButtonComponent onClick={() => {}} />);
      
      const button = screen.getByRole('button');
      expect(button).toBeInTheDocument();
      expect(button).toHaveTextContent('Disable');
      expect(button).toHaveAttribute('type', 'button');
      expect(button).toHaveClass(
        'w-[90px]',
        'p-[4px]',
        'cursor-pointer',
        'rounded-tr-[4px]',
        'rounded-br-[4px]',
        'outline-none',
        'font-semibold'
      );
    });

    it('renders disable button with custom type', () => {
      render(<DisableButtonComponent onClick={() => {}} type="reset" />);
      
      const button = screen.getByRole('button');
      expect(button).toHaveAttribute('type', 'reset');
    });

    it('calls onClick when clicked', () => {
      const mockOnClick = vi.fn();
      render(<DisableButtonComponent onClick={mockOnClick} />);
      
      const button = screen.getByRole('button');
      fireEvent.click(button);
      
      expect(mockOnClick).toHaveBeenCalledTimes(1);
    });

    it('applies active styles when isActive is true', () => {
      render(<DisableButtonComponent onClick={() => {}} isActive={true} />);
      
      const button = screen.getByRole('button');
      expect(button).toHaveClass('bg-[var(--button-switch-toggle-background-active)]');
      expect(button).toHaveClass('text-[var(--button-switch-toggle-color-active)]');
    });

    it('applies inactive styles when isActive is false', () => {
      render(<DisableButtonComponent onClick={() => {}} isActive={false} />);
      
      const button = screen.getByRole('button');
      expect(button).toHaveClass('bg-[var(--button-switch-toggle-background)]');
      expect(button).toHaveClass('text-[var(--button-switch-toggle-color)]');
    });

    it('applies inactive styles when isActive is undefined', () => {
      render(<DisableButtonComponent onClick={() => {}} />);
      
      const button = screen.getByRole('button');
      expect(button).toHaveClass('bg-[var(--button-switch-toggle-background)]');
      expect(button).toHaveClass('text-[var(--button-switch-toggle-color)]');
    });

    it('has proper button role', () => {
      render(<DisableButtonComponent onClick={() => {}} />);
      
      const button = screen.getByRole('button');
      expect(button).toBeInTheDocument();
    });

    it('is keyboard accessible', () => {
      const mockOnClick = vi.fn();
      render(<DisableButtonComponent onClick={mockOnClick} />);
      
      const button = screen.getByRole('button');
      expect(button).not.toBeDisabled();
      
      fireEvent.click(button);
      expect(mockOnClick).toHaveBeenCalledTimes(1);
    });
  });

  describe('ButtonOnComponent', () => {
    it('renders on button with default props', () => {
      render(<ButtonOnComponent onClick={() => {}} />);
      
      const button = screen.getByRole('button');
      expect(button).toBeInTheDocument();
      expect(button).toHaveTextContent('On');
      expect(button).toHaveAttribute('type', 'button');
      expect(button).toHaveClass(
        'w-[90px]',
        'p-[4px]',
        'cursor-pointer',
        'rounded-tl-[4px]',
        'rounded-bl-[4px]',
        'outline-none',
        'font-semibold'
      );
    });

    it('renders on button with custom type', () => {
      render(<ButtonOnComponent onClick={() => {}} type="submit" />);
      
      const button = screen.getByRole('button');
      expect(button).toHaveAttribute('type', 'submit');
    });

    it('calls onClick when clicked', () => {
      const mockOnClick = vi.fn();
      render(<ButtonOnComponent onClick={mockOnClick} />);
      
      const button = screen.getByRole('button');
      fireEvent.click(button);
      
      expect(mockOnClick).toHaveBeenCalledTimes(1);
    });

    it('applies active styles when isActive is true', () => {
      render(<ButtonOnComponent onClick={() => {}} isActive={true} />);
      
      const button = screen.getByRole('button');
      expect(button).toHaveClass('bg-[var(--button-switch-toggle-background-active)]');
      expect(button).toHaveClass('text-[var(--button-switch-toggle-color-active)]');
    });

    it('applies inactive styles when isActive is false', () => {
      render(<ButtonOnComponent onClick={() => {}} isActive={false} />);
      
      const button = screen.getByRole('button');
      expect(button).toHaveClass('bg-[var(--button-switch-toggle-background)]');
      expect(button).toHaveClass('text-[var(--button-switch-toggle-color)]');
    });

    it('applies inactive styles when isActive is undefined', () => {
      render(<ButtonOnComponent onClick={() => {}} />);
      
      const button = screen.getByRole('button');
      expect(button).toHaveClass('bg-[var(--button-switch-toggle-background)]');
      expect(button).toHaveClass('text-[var(--button-switch-toggle-color)]');
    });

    it('has proper button role', () => {
      render(<ButtonOnComponent onClick={() => {}} />);
      
      const button = screen.getByRole('button');
      expect(button).toBeInTheDocument();
    });

    it('is keyboard accessible', () => {
      const mockOnClick = vi.fn();
      render(<ButtonOnComponent onClick={mockOnClick} />);
      
      const button = screen.getByRole('button');
      expect(button).not.toBeDisabled();
      
      fireEvent.click(button);
      expect(mockOnClick).toHaveBeenCalledTimes(1);
    });
  });

  describe('ButtonOffComponent', () => {
    it('renders off button with default props', () => {
      render(<ButtonOffComponent onClick={() => {}} />);
      
      const button = screen.getByRole('button');
      expect(button).toBeInTheDocument();
      expect(button).toHaveTextContent('Off');
      expect(button).toHaveAttribute('type', 'button');
      expect(button).toHaveClass(
        'w-[90px]',
        'p-[4px]',
        'cursor-pointer',
        'rounded-tr-[4px]',
        'rounded-br-[4px]',
        'outline-none',
        'font-semibold'
      );
    });

    it('renders off button with custom type', () => {
      render(<ButtonOffComponent onClick={() => {}} type="reset" />);
      
      const button = screen.getByRole('button');
      expect(button).toHaveAttribute('type', 'reset');
    });

    it('calls onClick when clicked', () => {
      const mockOnClick = vi.fn();
      render(<ButtonOffComponent onClick={mockOnClick} />);
      
      const button = screen.getByRole('button');
      fireEvent.click(button);
      
      expect(mockOnClick).toHaveBeenCalledTimes(1);
    });

    it('applies active styles when isActive is true', () => {
      render(<ButtonOffComponent onClick={() => {}} isActive={true} />);
      
      const button = screen.getByRole('button');
      expect(button).toHaveClass('bg-[var(--button-switch-toggle-background-active)]');
      expect(button).toHaveClass('text-[var(--button-switch-toggle-color-active)]');
    });

    it('applies inactive styles when isActive is false', () => {
      render(<ButtonOffComponent onClick={() => {}} isActive={false} />);
      
      const button = screen.getByRole('button');
      expect(button).toHaveClass('bg-[var(--button-switch-toggle-background)]');
      expect(button).toHaveClass('text-[var(--button-switch-toggle-color)]');
    });

    it('applies inactive styles when isActive is undefined', () => {
      render(<ButtonOffComponent onClick={() => {}} />);
      
      const button = screen.getByRole('button');
      expect(button).toHaveClass('bg-[var(--button-switch-toggle-background)]');
      expect(button).toHaveClass('text-[var(--button-switch-toggle-color)]');
    });

    it('has proper button role', () => {
      render(<ButtonOffComponent onClick={() => {}} />);
      
      const button = screen.getByRole('button');
      expect(button).toBeInTheDocument();
    });

    it('is keyboard accessible', () => {
      const mockOnClick = vi.fn();
      render(<ButtonOffComponent onClick={mockOnClick} />);
      
      const button = screen.getByRole('button');
      expect(button).not.toBeDisabled();
      
      fireEvent.click(button);
      expect(mockOnClick).toHaveBeenCalledTimes(1);
    });
  });

  describe('Edge cases', () => {
    it('handles undefined onClick gracefully for all components', () => {
      const { rerender } = render(<EnableButtonComponent onClick={undefined} />);
      expect(() => fireEvent.click(screen.getByRole('button'))).not.toThrow();

      rerender(<DisableButtonComponent onClick={undefined} />);
      expect(() => fireEvent.click(screen.getByRole('button'))).not.toThrow();

      rerender(<ButtonOnComponent onClick={undefined} />);
      expect(() => fireEvent.click(screen.getByRole('button'))).not.toThrow();

      rerender(<ButtonOffComponent onClick={undefined} />);
      expect(() => fireEvent.click(screen.getByRole('button'))).not.toThrow();
    });

    it('handles null onClick gracefully for all components', () => {
      const { rerender } = render(<EnableButtonComponent onClick={null} />);
      expect(() => fireEvent.click(screen.getByRole('button'))).not.toThrow();

      rerender(<DisableButtonComponent onClick={null} />);
      expect(() => fireEvent.click(screen.getByRole('button'))).not.toThrow();

      rerender(<ButtonOnComponent onClick={null} />);
      expect(() => fireEvent.click(screen.getByRole('button'))).not.toThrow();

      rerender(<ButtonOffComponent onClick={null} />);
      expect(() => fireEvent.click(screen.getByRole('button'))).not.toThrow();
    });
  });

  describe('Translation', () => {
    it('renders translated text for all components', () => {
      const { rerender } = render(<EnableButtonComponent onClick={() => {}} />);
      expect(screen.getByText('Enable')).toBeInTheDocument();

      rerender(<DisableButtonComponent onClick={() => {}} />);
      expect(screen.getByText('Disable')).toBeInTheDocument();

      rerender(<ButtonOnComponent onClick={() => {}} />);
      expect(screen.getByText('On')).toBeInTheDocument();

      rerender(<ButtonOffComponent onClick={() => {}} />);
      expect(screen.getByText('Off')).toBeInTheDocument();
    });
  });
}); 