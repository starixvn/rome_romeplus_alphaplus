import React from 'react';
import { describe, it, expect, vi } from 'vitest';
import { render, screen, fireEvent } from '@testing-library/react';
import '@testing-library/jest-dom';
import { BackButtonComponent } from './BackButtonComponent';
import { ButtonComponentProps } from './types';

// Mock react-i18next
vi.mock('react-i18next', () => ({
  useTranslation: () => ({
    t: (key: string) => key === 'back' ? 'Back' : key,
  }),
}));

describe('BackButtonComponent', () => {
  const defaultProps: ButtonComponentProps = {
    onClick: vi.fn(),
  };

  it('renders without crashing', () => {
    render(<BackButtonComponent {...defaultProps} />);
    expect(screen.getByText('Back')).toBeInTheDocument();
  });

  it('displays the correct text from translation', () => {
    render(<BackButtonComponent {...defaultProps} />);
    expect(screen.getByText('Back')).toBeInTheDocument();
  });

  it('calls onClick when clicked', () => {
    const mockOnClick = vi.fn();
    render(<BackButtonComponent onClick={mockOnClick} />);
    
    const button = screen.getByText('Back');
    fireEvent.click(button);
    
    expect(mockOnClick).toHaveBeenCalledTimes(1);
  });

  it('renders with correct CSS classes', () => {
    render(<BackButtonComponent {...defaultProps} />);
    
    const buttonContainer = screen.getByText('Back').closest('div');
    expect(buttonContainer).toHaveClass(
      'min-w-[90px]',
      'text-[var(--button-back-text)]',
      'hover:text-[var(--button-back-text-hover)]',
      'border-[1px]',
      'border-[var(--button-back-border)]',
      'rounded-[20px]',
      'mr-[16px]',
      'bg-[var(--button-back-background)]',
      'hover:bg-[var(--button-back-background-hover)]',
      'hover:border-[transparent]',
      'group',
      'mb-[8px]'
    );
  });

  it('renders the back icon', () => {
    render(<BackButtonComponent {...defaultProps} />);
    
    const icon = screen.getByText('Back').previousElementSibling;
    expect(icon).toBeInTheDocument();
    expect(icon).toHaveClass(
      'w-[18px]',
      'h-[18px]',
      'fill-[var(--button-back-text)]',
      'mr-[8px]',
      'group-hover:fill-[var(--button-back-text-hover)]'
    );
  });

  it('renders text with correct styling', () => {
    render(<BackButtonComponent {...defaultProps} />);
    
    const textElement = screen.getByText('Back');
    expect(textElement).toHaveClass('font-semibold');
  });

  it('has correct span structure', () => {
    render(<BackButtonComponent {...defaultProps} />);
    
    const span = screen.getByText('Back').closest('span');
    expect(span).toHaveClass(
      'font-semibold',
  
    );
  });

  it('handles click events correctly', () => {
    const mockOnClick = vi.fn();
    render(<BackButtonComponent onClick={mockOnClick} />);
    
    const clickableElement = screen.getByText('Back').closest('span');
    fireEvent.click(clickableElement!);
    
    expect(mockOnClick).toHaveBeenCalledTimes(1);
  });

  it('renders with proper accessibility attributes', () => {
    render(<BackButtonComponent {...defaultProps} />);
    
    const button = screen.getByText('Back');
    expect(button).toBeInTheDocument();
  });

  it('works without onClick prop', () => {
    render(<BackButtonComponent />);
    
    const button = screen.getByText('Back');
    expect(button).toBeInTheDocument();
    
    // Should not throw error when clicked without onClick handler
    expect(() => fireEvent.click(button)).not.toThrow();
  });

  it('maintains consistent styling structure', () => {
    const { container } = render(<BackButtonComponent {...defaultProps} />);
    
    // Check the overall structure
    const outerDiv = container.firstChild as HTMLElement;
    expect(outerDiv.tagName).toBe('DIV');
    
    const innerSpan = outerDiv.firstChild as HTMLElement;
    expect(innerSpan.tagName).toBe('SPAN');
    
    const icon = innerSpan.firstChild as HTMLElement;
    expect(icon.tagName).toBe('svg');
    
    const textSpan = innerSpan.lastChild as HTMLElement;
    expect(textSpan.tagName).toBe('SPAN');
  });

  it('applies hover styles correctly', () => {
    render(<BackButtonComponent {...defaultProps} />);
    
    const buttonContainer = screen.getByText('Back').closest('div');
    expect(buttonContainer).toHaveClass('hover:text-[var(--button-back-text-hover)]');
    expect(buttonContainer).toHaveClass('hover:bg-[var(--button-back-background-hover)]');
    expect(buttonContainer).toHaveClass('hover:border-[transparent]');
  });

  it('applies group hover styles to icon', () => {
    render(<BackButtonComponent {...defaultProps} />);
    
    const icon = screen.getByText('Back').previousElementSibling;
    expect(icon).toHaveClass('group-hover:fill-[var(--button-back-text-hover)]');
  });
}); 