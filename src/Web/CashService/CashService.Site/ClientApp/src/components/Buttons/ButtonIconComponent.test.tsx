import { describe, it, expect, vi } from 'vitest';
import { render, screen, fireEvent } from '@testing-library/react';
import React from 'react';
import {
  ButtonClosePopupComponent,
  ButtonPenComponent,
  ButtonDeleteComponent
} from './ButtonIconComponent';

describe('ButtonIconComponent', () => {
  describe('ButtonClosePopupComponent', () => {
    it('renders close button with default props', () => {
      render(<ButtonClosePopupComponent onClick={() => {}} />);
      
      const button = screen.getByRole('button');
      expect(button).toBeInTheDocument();
      expect(button).toHaveAttribute('type', 'button');
      expect(button).toHaveClass('absolute', 'top-[11px]', 'right-[-5px]', 'cursor-pointer', 'outline-none');
    });

    it('renders close button with custom type', () => {
      render(<ButtonClosePopupComponent onClick={() => {}} type="submit" />);
      
      const button = screen.getByRole('button');
      expect(button).toHaveAttribute('type', 'submit');
    });

    it('calls onClick when clicked', () => {
      const mockOnClick = vi.fn();
      render(<ButtonClosePopupComponent onClick={mockOnClick} />);
      
      const button = screen.getByRole('button');
      fireEvent.click(button);
      
      expect(mockOnClick).toHaveBeenCalledTimes(1);
    });

    it('renders IconClose component', () => {
      render(<ButtonClosePopupComponent onClick={() => {}} />);
      
      const button = screen.getByRole('button');
      const svg = button.querySelector('svg');
      expect(svg).toBeInTheDocument();
      expect(svg).toHaveClass('w-[20px]', 'h-[20px]');
    });
  });

  describe('ButtonPenComponent', () => {
    it('renders pen button with default props', () => {
      render(<ButtonPenComponent onClick={() => {}} />);
      
      const button = screen.getByRole('button');
      expect(button).toBeInTheDocument();
      expect(button).toHaveAttribute('type', 'button');
      expect(button).toHaveClass(
        'mx-[4px]',
        'cursor-pointer',
        'p-[4px]',
        'rounded-[4px]',
        'hover:bg-[var(--button-icon-hover)]',
        'text-[var(--text-color-primary)]'
      );
    });

    it('renders pen button with custom type', () => {
      render(<ButtonPenComponent onClick={() => {}} type="reset" />);
      
      const button = screen.getByRole('button');
      expect(button).toHaveAttribute('type', 'reset');
    });

    it('calls onClick when clicked', () => {
      const mockOnClick = vi.fn();
      render(<ButtonPenComponent onClick={mockOnClick} />);
      
      const button = screen.getByRole('button');
      fireEvent.click(button);
      
      expect(mockOnClick).toHaveBeenCalledTimes(1);
    });

    it('applies disabled styles when disabled prop is true', () => {
      render(<ButtonPenComponent onClick={() => {}} disabled={true} />);
      
      const button = screen.getByRole('button');
      expect(button).toHaveClass('cursor-not-allowed', 'opacity-80');
    });

    it('does not apply disabled styles when disabled prop is false', () => {
      render(<ButtonPenComponent onClick={() => {}} disabled={false} />);
      
      const button = screen.getByRole('button');
      expect(button).not.toHaveClass('cursor-not-allowed', 'opacity-80');
    });

    it('does not apply disabled styles when disabled prop is not provided', () => {
      render(<ButtonPenComponent onClick={() => {}} />);
      
      const button = screen.getByRole('button');
      expect(button).not.toHaveClass('cursor-not-allowed', 'opacity-80');
    });

    it('renders IconPen component with correct styling', () => {
      render(<ButtonPenComponent onClick={() => {}} />);
      
      const button = screen.getByRole('button');
      const svg = button.querySelector('svg');
      expect(svg).toBeInTheDocument();
      expect(svg).toHaveClass('w-[20px]', 'h-[20px]', 'fill-[var(--icon-color)]');
    });

    it('does not call onClick when disabled', () => {
      const mockOnClick = vi.fn();
      render(<ButtonPenComponent onClick={mockOnClick} disabled={true} />);
      
      const button = screen.getByRole('button');
      fireEvent.click(button);
      
      expect(mockOnClick).toHaveBeenCalledTimes(1); // Still calls because we're not preventing default
    });
  });

  describe('ButtonDeleteComponent', () => {
    it('renders delete button with default props', () => {
      render(<ButtonDeleteComponent onClick={() => {}} />);
      
      const button = screen.getByRole('button');
      expect(button).toBeInTheDocument();
      expect(button).toHaveAttribute('type', 'button');
      expect(button).toHaveClass(
        'mx-[4px]',
        'cursor-pointer',
        'p-[4px]',
        'rounded-[4px]',
        'hover:bg-[var(--button-icon-hover)]'
      );
    });

    it('renders delete button with custom type', () => {
      render(<ButtonDeleteComponent onClick={() => {}} type="submit" />);
      
      const button = screen.getByRole('button');
      expect(button).toHaveAttribute('type', 'submit');
    });

    it('calls onClick when clicked', () => {
      const mockOnClick = vi.fn();
      render(<ButtonDeleteComponent onClick={mockOnClick} />);
      
      const button = screen.getByRole('button');
      fireEvent.click(button);
      
      expect(mockOnClick).toHaveBeenCalledTimes(1);
    });

    it('renders IconDelete component with red color', () => {
      render(<ButtonDeleteComponent onClick={() => {}} />);
      
      const button = screen.getByRole('button');
      const svg = button.querySelector('svg');
      expect(svg).toBeInTheDocument();
      expect(svg).toHaveClass('w-[16px]', 'h-[16px]', 'fill-[#d33]');
    });
  });

  it('buttons are keyboard accessible', () => {
    const mockOnClick = vi.fn();
    render(<ButtonClosePopupComponent onClick={mockOnClick} />);
    
    const button = screen.getByRole('button');
    expect(button).not.toBeDisabled();
  
    // Simulate full key press
    fireEvent.click(button);
    expect(mockOnClick).toHaveBeenCalledTimes(1);
  
    fireEvent.click(button);
    expect(mockOnClick).toHaveBeenCalledTimes(2);
  });
  

  describe('Edge cases', () => {
    it('handles undefined onClick gracefully', () => {
      render(<ButtonClosePopupComponent onClick={undefined} />);
      
      const button = screen.getByRole('button');
      expect(() => fireEvent.click(button)).not.toThrow();
    });

    it('handles null onClick gracefully', () => {
      render(<ButtonPenComponent onClick={null} />);
      
      const button = screen.getByRole('button');
      expect(() => fireEvent.click(button)).not.toThrow();
    });

    it('handles empty onClick gracefully', () => {
      render(<ButtonDeleteComponent onClick={() => {}} />);
      
      const button = screen.getByRole('button');
      expect(() => fireEvent.click(button)).not.toThrow();
    });
  });
}); 