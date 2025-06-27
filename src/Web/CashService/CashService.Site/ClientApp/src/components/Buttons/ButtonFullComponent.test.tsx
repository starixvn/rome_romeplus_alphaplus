import React from 'react';
import { describe, it, expect, vi } from 'vitest';
import { render, screen, fireEvent } from '@testing-library/react';
import '@testing-library/jest-dom';
import { ButtonFullComponent } from './ButtonFullComponent';
import { ButtonComponentProps } from './types';

describe('ButtonFullComponent', () => {
  const defaultProps: ButtonComponentProps = {
    text: 'Submit',
    onClick: vi.fn(),
  };

  it('renders without crashing', () => {
    render(<ButtonFullComponent {...defaultProps} />);
    expect(screen.getByText('Submit')).toBeInTheDocument();
  });

  it('displays the correct text', () => {
    render(<ButtonFullComponent {...defaultProps} />);
    expect(screen.getByText('Submit')).toBeInTheDocument();
  });

  it('calls onClick when clicked', () => {
    const mockOnClick = vi.fn();
    render(<ButtonFullComponent text="Submit" onClick={mockOnClick} />);
    
    const button = screen.getByText('Submit');
    fireEvent.click(button);
    
    expect(mockOnClick).toHaveBeenCalledTimes(1);
  });

  it('renders with correct CSS classes', () => {
    render(<ButtonFullComponent {...defaultProps} />);
    
    const button = screen.getByText('Submit');
    expect(button).toHaveClass(
      'min-w-[90px]',
      'bg-[var(--button-background)]',
      'hover:bg-[var(--button-background-hover)]',
      'text-[var(--button-text)]',
      'rounded-[3px]',
      'cursor-pointer',
      'h-[28px]',
      'px-[16px]',
      'font-semibold',
      'outline-none',
      'pt-[1px]'
    );
  });

  it('applies disabled state correctly', () => {
    render(<ButtonFullComponent {...defaultProps} disabled={true} />);
    
    const button = screen.getByText('Submit');
    expect(button).toBeDisabled();
    expect(button).toHaveClass('cursor-not-allowed', 'opacity-80');
    expect(button).not.toHaveClass('cursor-pointer');
  });

  it('applies enabled state correctly', () => {
    render(<ButtonFullComponent {...defaultProps} disabled={false} />);
    
    const button = screen.getByText('Submit');
    expect(button).not.toBeDisabled();
    expect(button).toHaveClass('cursor-pointer');
    expect(button).not.toHaveClass('cursor-not-allowed', 'opacity-80');
  });

  it('renders with custom className', () => {
    const customClass = 'custom-button-class';
    render(<ButtonFullComponent {...defaultProps} className={customClass} />);
    
    const container = screen.getByText('Submit').closest('div');
    expect(container).toHaveClass(customClass);
  });

  it('renders with correct button type', () => {
    render(<ButtonFullComponent {...defaultProps} type="submit" />);
    
    const button = screen.getByText('Submit');
    expect(button).toHaveAttribute('type', 'submit');
  });

  it('renders with default button type when not provided', () => {
    render(<ButtonFullComponent {...defaultProps} />);
    
    const button = screen.getByText('Submit');
    expect(button).toHaveAttribute('type', 'button');
  });

  it('handles click events correctly when enabled', () => {
    const mockOnClick = vi.fn();
    render(<ButtonFullComponent text="Submit" onClick={mockOnClick} disabled={false} />);
    
    const button = screen.getByText('Submit');
    fireEvent.click(button);
    
    expect(mockOnClick).toHaveBeenCalledTimes(1);
  });

  it('does not call onClick when disabled', () => {
    const mockOnClick = vi.fn();
    render(<ButtonFullComponent text="Submit" onClick={mockOnClick} disabled={true} />);
    
    const button = screen.getByText('Submit');
    fireEvent.click(button);
    
    expect(mockOnClick).not.toHaveBeenCalled();
  });

  it('works without onClick prop', () => {
    render(<ButtonFullComponent text="Submit" />);
    
    const button = screen.getByText('Submit');
    expect(button).toBeInTheDocument();
    
    // Should not throw error when clicked without onClick handler
    expect(() => fireEvent.click(button)).not.toThrow();
  });

  it('maintains consistent DOM structure', () => {
    const { container } = render(<ButtonFullComponent {...defaultProps} />);
    
    // Check the overall structure
    const outerDiv = container.firstChild as HTMLElement;
    expect(outerDiv.tagName).toBe('DIV');
    
    const button = outerDiv.firstChild as HTMLElement;
    expect(button.tagName).toBe('BUTTON');
    expect(button.textContent).toBe('Submit');
  });

  it('applies hover styles correctly', () => {
    render(<ButtonFullComponent {...defaultProps} />);
    
    const button = screen.getByText('Submit');
    expect(button).toHaveClass('hover:bg-[var(--button-background-hover)]');
  });

  it('renders with different text content', () => {
    const customText = 'Save Changes';
    render(<ButtonFullComponent text={customText} onClick={vi.fn()} />);
    
    expect(screen.getByText(customText)).toBeInTheDocument();
  });

  it('handles all button types correctly', () => {
    const types: Array<'button' | 'submit' | 'reset'> = ['button', 'submit', 'reset'];
    
    types.forEach(type => {
      const { unmount } = render(<ButtonFullComponent {...defaultProps} type={type} />);
      const button = screen.getByText('Submit');
      expect(button).toHaveAttribute('type', type);
      unmount();
    });
  });

  it('combines custom className with default structure', () => {
    const customClass = 'my-custom-class';
    render(<ButtonFullComponent {...defaultProps} className={customClass} />);
    
    const container = screen.getByText('Submit').closest('div');
    expect(container).toHaveClass(customClass);
  });

  it('renders with all props provided', () => {
    const allProps = {
      text: 'Confirm',
      onClick: vi.fn(),
      disabled: false,
      type: 'submit' as const,
      className: 'test-class'
    };
    
    render(<ButtonFullComponent {...allProps} />);
    
    const button = screen.getByText('Confirm');
    expect(button).toBeInTheDocument();
    expect(button).toHaveAttribute('type', 'submit');
    expect(button).not.toBeDisabled();
    
    const container = button.closest('div');
    expect(container).toHaveClass('test-class');
  });

  it('handles empty text gracefully', () => {
    render(<ButtonFullComponent text="" onClick={vi.fn()} />);
    
    const button = screen.getByRole('button');
    expect(button).toBeInTheDocument();
    expect(button.textContent).toBe('');
  });

  it('handles undefined text gracefully', () => {
    render(<ButtonFullComponent onClick={vi.fn()} />);
    
    const button = screen.getByRole('button');
    expect(button).toBeInTheDocument();
    expect(button.textContent).toBe('');
  });

  it('applies disabled styles correctly when disabled', () => {
    render(<ButtonFullComponent {...defaultProps} disabled={true} />);
    
    const button = screen.getByText('Submit');
    expect(button).toHaveClass('cursor-not-allowed', 'opacity-80');
  });

  it('does not apply disabled styles when enabled', () => {
    render(<ButtonFullComponent {...defaultProps} disabled={false} />);
    
    const button = screen.getByText('Submit');
    expect(button).not.toHaveClass('cursor-not-allowed', 'opacity-80');
    expect(button).toHaveClass('cursor-pointer');
  });

  it('handles multiple clicks correctly', () => {
    const mockOnClick = vi.fn();
    render(<ButtonFullComponent text="Submit" onClick={mockOnClick} />);
    
    const button = screen.getByText('Submit');
    
    fireEvent.click(button);
    fireEvent.click(button);
    fireEvent.click(button);
    
    expect(mockOnClick).toHaveBeenCalledTimes(3);
  });

  it('maintains button dimensions and spacing', () => {
    render(<ButtonFullComponent {...defaultProps} />);
    
    const button = screen.getByText('Submit');
    expect(button).toHaveClass('min-w-[90px]', 'h-[28px]', 'px-[16px]', 'pt-[1px]');
  });

  it('applies proper border radius', () => {
    render(<ButtonFullComponent {...defaultProps} />);
    
    const button = screen.getByText('Submit');
    expect(button).toHaveClass('rounded-[3px]');
  });

  it('applies proper font styling', () => {
    render(<ButtonFullComponent {...defaultProps} />);
    
    const button = screen.getByText('Submit');
    expect(button).toHaveClass('font-semibold');
  });

  it('removes outline styling', () => {
    render(<ButtonFullComponent {...defaultProps} />);
    
    const button = screen.getByText('Submit');
    expect(button).toHaveClass('outline-none');
  });
}); 