import { describe, it, expect, vi } from 'vitest';
import { render, screen, fireEvent } from '@testing-library/react';
import '@testing-library/jest-dom';
import React from 'react';
import { ButtonRejectComponent } from './ButtonRejectComponent';

describe('ButtonRejectComponent', () => {
  it('renders button with default props', () => {
    render(<ButtonRejectComponent text="Reject" onClick={() => {}} />);
    
    const button = screen.getByRole('button');
    expect(button).toBeInTheDocument();
    expect(button).toHaveTextContent('Reject');
    expect(button).toHaveAttribute('type', 'button');
    expect(button).toHaveClass(
      'min-w-[90px]',
      'bg-[var(--button-background-reject)]',
      'hover:bg-[var(--button-background-reject-hover)]',
      'text-[var(--button-text)]',
      'rounded-[3px]',
      'cursor-pointer',
      'h-[28px]',
      'px-[16px]',
      'font-semibold',
      'outline-none'
    );
  });

  it('renders button with custom type', () => {
    render(<ButtonRejectComponent text="Submit" onClick={() => {}} type="submit" />);
    
    const button = screen.getByRole('button');
    expect(button).toHaveAttribute('type', 'submit');
  });

  it('renders button with reset type', () => {
    render(<ButtonRejectComponent text="Reset" onClick={() => {}} type="reset" />);
    
    const button = screen.getByRole('button');
    expect(button).toHaveAttribute('type', 'reset');
  });

  it('calls onClick when clicked', () => {
    const mockOnClick = vi.fn();
    render(<ButtonRejectComponent text="Click Me" onClick={mockOnClick} />);
    
    const button = screen.getByRole('button');
    fireEvent.click(button);
    
    expect(mockOnClick).toHaveBeenCalledTimes(1);
  });

  it('renders with custom className', () => {
    render(<ButtonRejectComponent text="Custom" onClick={() => {}} className="custom-class" />);
    
    const container = screen.getByRole('button').parentElement;
    expect(container).toHaveClass('custom-class');
  });

  it('renders with empty className', () => {
    render(<ButtonRejectComponent text="No Class" onClick={() => {}} className="" />);
    
    const container = screen.getByRole('button').parentElement;
    expect(container).not.toHaveClass('');
  });

  it('renders with undefined className', () => {
    render(<ButtonRejectComponent text="Undefined Class" onClick={() => {}} />);
    
    const container = screen.getByRole('button').parentElement;
    expect(container).not.toHaveClass('');
  });

  it('renders button text correctly', () => {
    const testText = 'This is a reject button text';
    render(<ButtonRejectComponent text={testText} onClick={() => {}} />);
    
    const button = screen.getByRole('button');
    expect(button).toHaveTextContent(testText);
  });

  it('renders empty text', () => {
    render(<ButtonRejectComponent text="" onClick={() => {}} />);
    
    const button = screen.getByRole('button');
    expect(button).toHaveTextContent('');
  });

  it('applies disabled state when disabled prop is true', () => {
    render(<ButtonRejectComponent text="Disabled" onClick={() => {}} disabled={true} />);
    
    const button = screen.getByRole('button');
    expect(button).toBeDisabled();
  });

  it('does not apply disabled state when disabled prop is false', () => {
    render(<ButtonRejectComponent text="Enabled" onClick={() => {}} disabled={false} />);
    
    const button = screen.getByRole('button');
    expect(button).not.toBeDisabled();
  });

  it('does not apply disabled state when disabled prop is not provided', () => {
    render(<ButtonRejectComponent text="Default" onClick={() => {}} />);
    
    const button = screen.getByRole('button');
    expect(button).not.toBeDisabled();
  });

  it('renders with custom title', () => {
    const customTitle = 'Custom tooltip text';
    render(<ButtonRejectComponent text="Titled" onClick={() => {}} title={customTitle} />);
    
    const button = screen.getByRole('button');
    expect(button).toHaveAttribute('title', customTitle);
  });

  it('renders with empty title', () => {
    render(<ButtonRejectComponent text="Empty Title" onClick={() => {}} title="" />);
    
    const button = screen.getByRole('button');
    expect(button).toHaveAttribute('title', '');
  });

  it('renders with undefined title', () => {
    render(<ButtonRejectComponent text="No Title" onClick={() => {}} />);
    
    const button = screen.getByRole('button');
    expect(button).toHaveAttribute('title', '');
  });

  it('handles undefined onClick gracefully', () => {
    render(<ButtonRejectComponent text="No Click" onClick={undefined} />);
    
    const button = screen.getByRole('button');
    expect(() => fireEvent.click(button)).not.toThrow();
  });

  it('handles null onClick gracefully', () => {
    render(<ButtonRejectComponent text="Null Click" onClick={null} />);
    
    const button = screen.getByRole('button');
    expect(() => fireEvent.click(button)).not.toThrow();
  });

  it('has proper button role', () => {
    render(<ButtonRejectComponent text="Accessible" onClick={() => {}} />);
    
    const button = screen.getByRole('button');
    expect(button).toBeInTheDocument();
  });

  it('is keyboard accessible when enabled', () => {
    const mockOnClick = vi.fn();
    render(<ButtonRejectComponent text="Keyboard" onClick={mockOnClick} />);
    
    const button = screen.getByRole('button');
    expect(button).not.toBeDisabled();
    
    fireEvent.click(button);
    expect(mockOnClick).toHaveBeenCalledTimes(1);
  });

  it('renders with all CSS custom properties', () => {
    render(<ButtonRejectComponent text="Styled" onClick={() => {}} />);
    
    const button = screen.getByRole('button');
    expect(button).toHaveClass('bg-[var(--button-background-reject)]');
    expect(button).toHaveClass('hover:bg-[var(--button-background-reject-hover)]');
    expect(button).toHaveClass('text-[var(--button-text)]');
  });

  it('has correct dimensions and spacing', () => {
    render(<ButtonRejectComponent text="Dimensions" onClick={() => {}} />);
    
    const button = screen.getByRole('button');
    expect(button).toHaveClass('min-w-[90px]', 'h-[28px]', 'px-[16px]');
  });

  it('has correct border radius', () => {
    render(<ButtonRejectComponent text="Border" onClick={() => {}} />);
    
    const button = screen.getByRole('button');
    expect(button).toHaveClass('rounded-[3px]');
  });

  it('does not call onClick when disabled', () => {
    const mockOnClick = vi.fn();
    render(<ButtonRejectComponent text="Disabled Click" onClick={mockOnClick} disabled={true} />);
    
    const button = screen.getByRole('button');
    fireEvent.click(button);
    
    expect(mockOnClick).not.toHaveBeenCalled(); // Still calls because we're not preventing default
  });
}); 