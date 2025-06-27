import React from 'react';
import { describe, it, expect, vi } from 'vitest';
import { render, screen, fireEvent } from '@testing-library/react';
import '@testing-library/jest-dom';
import { ButtonCancelComponent } from './ButtonCancelComponent';
import { ButtonComponentProps } from './types';

describe('ButtonCancelComponent', () => {
  const defaultProps: ButtonComponentProps = {
    text: 'Cancel',
    onClick: vi.fn(),
  };

  it('renders without crashing', () => {
    render(<ButtonCancelComponent {...defaultProps} />);
    expect(screen.getByText('Cancel')).toBeInTheDocument();
  });

  it('displays the correct text', () => {
    render(<ButtonCancelComponent {...defaultProps} />);
    expect(screen.getByText('Cancel')).toBeInTheDocument();
  });

  it('calls onClick when clicked', () => {
    const mockOnClick = vi.fn();
    render(<ButtonCancelComponent text="Cancel" onClick={mockOnClick} />);
    
    const button = screen.getByText('Cancel');
    fireEvent.click(button);
    
    expect(mockOnClick).toHaveBeenCalledTimes(1);
  });

  it('renders with correct CSS classes', () => {
    render(<ButtonCancelComponent {...defaultProps} />);
    
    const button = screen.getByText('Cancel');
    expect(button).toHaveClass(
      'min-w-[90px]',
      'bg-[var(--button-background-cancel)]',
      'hover:bg-[var(--button-background-cancel-hover)]',
      'text-[var(--button-text)]',
      'rounded-[3px]',
      'cursor-pointer',
      'h-[28px]',
      'pt-[1px]',
      'px-[16px]',
      'font-semibold',
      'outline-none'
    );
  });

  it('applies disabled state correctly', () => {
    render(<ButtonCancelComponent {...defaultProps} disabled={true} />);
    
    const button = screen.getByText('Cancel');
    expect(button).toBeDisabled();
  });

  it('applies enabled state correctly', () => {
    render(<ButtonCancelComponent {...defaultProps} disabled={false} />);
    
    const button = screen.getByText('Cancel');
    expect(button).not.toBeDisabled();
    expect(button).toHaveClass('cursor-pointer');
  });

  it('renders with custom className', () => {
    const customClass = 'custom-cancel-button-class';
    render(<ButtonCancelComponent {...defaultProps} className={customClass} />);
    
    const container = screen.getByText('Cancel').closest('div');
    expect(container).toHaveClass(customClass);
  });

  it('renders with correct button type', () => {
    render(<ButtonCancelComponent {...defaultProps} type="submit" />);
    
    const button = screen.getByText('Cancel');
    expect(button).toHaveAttribute('type', 'submit');
  });

  it('renders with default button type when not provided', () => {
    render(<ButtonCancelComponent {...defaultProps} />);
    
    const button = screen.getByText('Cancel');
    expect(button).toHaveAttribute('type', 'button');
  });

  it('handles click events correctly when enabled', () => {
    const mockOnClick = vi.fn();
    render(<ButtonCancelComponent text="Cancel" onClick={mockOnClick} disabled={false} />);
    
    const button = screen.getByText('Cancel');
    fireEvent.click(button);
    
    expect(mockOnClick).toHaveBeenCalledTimes(1);
  });

  it('does not call onClick when disabled', () => {
    const mockOnClick = vi.fn();
    render(<ButtonCancelComponent text="Cancel" onClick={mockOnClick} disabled={true} />);
    
    const button = screen.getByText('Cancel');
    fireEvent.click(button);
    
    expect(mockOnClick).not.toHaveBeenCalled();
  });

  it('works without onClick prop', () => {
    render(<ButtonCancelComponent text="Cancel" />);
    
    const button = screen.getByText('Cancel');
    expect(button).toBeInTheDocument();
    
    // Should not throw error when clicked without onClick handler
    expect(() => fireEvent.click(button)).not.toThrow();
  });

  it('maintains consistent DOM structure', () => {
    const { container } = render(<ButtonCancelComponent {...defaultProps} />);
    
    // Check the overall structure
    const outerDiv = container.firstChild as HTMLElement;
    expect(outerDiv.tagName).toBe('DIV');
    
    const button = outerDiv.firstChild as HTMLElement;
    expect(button.tagName).toBe('BUTTON');
    expect(button.textContent).toBe('Cancel');
  });

  it('applies hover styles correctly', () => {
    render(<ButtonCancelComponent {...defaultProps} />);
    
    const button = screen.getByText('Cancel');
    expect(button).toHaveClass('hover:bg-[var(--button-background-cancel-hover)]');
  });

  it('renders with different text content', () => {
    const customText = 'Discard Changes';
    render(<ButtonCancelComponent text={customText} onClick={vi.fn()} />);
    
    expect(screen.getByText(customText)).toBeInTheDocument();
  });

  it('handles all button types correctly', () => {
    const types: Array<'button' | 'submit' | 'reset'> = ['button', 'submit', 'reset'];
    
    types.forEach(type => {
      const { unmount } = render(<ButtonCancelComponent {...defaultProps} type={type} />);
      const button = screen.getByText('Cancel');
      expect(button).toHaveAttribute('type', type);
      unmount();
    });
  });

  it('combines custom className with default structure', () => {
    const customClass = 'my-custom-cancel-class';
    render(<ButtonCancelComponent {...defaultProps} className={customClass} />);
    
    const container = screen.getByText('Cancel').closest('div');
    expect(container).toHaveClass(customClass);
  });

  it('renders with all props provided', () => {
    const allProps = {
      text: 'Abort',
      onClick: vi.fn(),
      disabled: false,
      type: 'reset' as const,
      className: 'test-cancel-class',
    };
    
    render(<ButtonCancelComponent {...allProps} />);
    
    const button = screen.getByText('Abort');
    expect(button).toBeInTheDocument();
    expect(button).toHaveAttribute('type', 'reset');
    expect(button).not.toBeDisabled();
    
    const container = button.closest('div');
    expect(container).toHaveClass('test-cancel-class');
  });

  it('renders with empty className when not provided', () => {
    render(<ButtonCancelComponent {...defaultProps} />);
    
    const container = screen.getByText('Cancel').closest('div');
    expect(container).toHaveAttribute('class', '');
  });

  it('renders with cancel-specific background color', () => {
    render(<ButtonCancelComponent {...defaultProps} />);
    
    const button = screen.getByText('Cancel');
    expect(button).toHaveClass('bg-[var(--button-background-cancel)]');
  });

  it('renders with cancel-specific hover background color', () => {
    render(<ButtonCancelComponent {...defaultProps} />);
    
    const button = screen.getByText('Cancel');
    expect(button).toHaveClass('hover:bg-[var(--button-background-cancel-hover)]');
  });

  it('maintains button dimensions', () => {
    render(<ButtonCancelComponent {...defaultProps} />);
    
    const button = screen.getByText('Cancel');
    expect(button).toHaveClass('min-w-[90px]', 'h-[28px]');
  });

  it('maintains button padding', () => {
    render(<ButtonCancelComponent {...defaultProps} />);
    
    const button = screen.getByText('Cancel');
    expect(button).toHaveClass('pt-[1px]', 'px-[16px]');
  });

  it('maintains button styling', () => {
    render(<ButtonCancelComponent {...defaultProps} />);
    
    const button = screen.getByText('Cancel');
    expect(button).toHaveClass('rounded-[3px]', 'font-semibold', 'outline-none');
  });
}); 