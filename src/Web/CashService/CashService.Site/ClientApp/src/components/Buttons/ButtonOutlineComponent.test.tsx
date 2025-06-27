import { describe, it, expect, vi } from 'vitest';
import { render, screen, fireEvent } from '@testing-library/react';
import '@testing-library/jest-dom';
import React from 'react';
import { ButtonOutlineComponent } from './ButtonOutlineComponent';

describe('ButtonOutlineComponent', () => {
  it('renders button with default props', () => {
    render(<ButtonOutlineComponent text="Test Button" onClick={() => {}} />);
    
    const button = screen.getByRole('button');
    expect(button).toBeInTheDocument();
    expect(button).toHaveTextContent('Test Button');
    expect(button).toHaveAttribute('type', 'button');
    expect(button).toHaveClass(
      'border-[1px]',
      'border-[var(--button-outline-border)]',
      'bg-[var(--button-outline-background)]',
      'hover:bg-[var(--button-outline-background-hover)]',
      'hover:text-[var(--button-outline-text-hover)]',
      'text-[var(--button-outline-text)]',
      'rounded-[5px]',
      'cursor-pointer',
      'h-[28px]',
      'px-[16px]',
      'font-semibold',
      'outline-none',
      'pt-[1px]'
    );
  });

  it('renders button with custom type', () => {
    render(<ButtonOutlineComponent text="Submit" onClick={() => {}} type="submit" />);
    
    const button = screen.getByRole('button');
    expect(button).toHaveAttribute('type', 'submit');
  });

  it('renders button with reset type', () => {
    render(<ButtonOutlineComponent text="Reset" onClick={() => {}} type="reset" />);
    
    const button = screen.getByRole('button');
    expect(button).toHaveAttribute('type', 'reset');
  });

  it('calls onClick when clicked', () => {
    const mockOnClick = vi.fn();
    render(<ButtonOutlineComponent text="Click Me" onClick={mockOnClick} />);
    
    const button = screen.getByRole('button');
    fireEvent.click(button);
    
    expect(mockOnClick).toHaveBeenCalledTimes(1);
  });

  it('renders with custom className', () => {
    render(<ButtonOutlineComponent text="Custom" onClick={() => {}} className="custom-class" />);
    
    const container = screen.getByRole('button').parentElement;
    expect(container).toHaveClass('custom-class');
  });

  it('renders with empty className', () => {
    render(<ButtonOutlineComponent text="No Class" onClick={() => {}} className="" />);
    
    const container = screen.getByRole('button').parentElement;
    expect(container).not.toHaveClass('');
  });

  it('renders with undefined className', () => {
    render(<ButtonOutlineComponent text="Undefined Class" onClick={() => {}} />);
    
    const container = screen.getByRole('button').parentElement;
    expect(container).not.toHaveClass('');
  });

  it('renders button text correctly', () => {
    const testText = 'This is a test button text';
    render(<ButtonOutlineComponent text={testText} onClick={() => {}} />);
    
    const button = screen.getByRole('button');
    expect(button).toHaveTextContent(testText);
  });

  it('renders empty text', () => {
    render(<ButtonOutlineComponent text="" onClick={() => {}} />);
    
    const button = screen.getByRole('button');
    expect(button).toHaveTextContent('');
  });

  it('handles undefined onClick gracefully', () => {
    render(<ButtonOutlineComponent text="No Click" onClick={undefined} />);
    
    const button = screen.getByRole('button');
    expect(() => fireEvent.click(button)).not.toThrow();
  });

  it('handles null onClick gracefully', () => {
    render(<ButtonOutlineComponent text="Null Click" onClick={null} />);
    
    const button = screen.getByRole('button');
    expect(() => fireEvent.click(button)).not.toThrow();
  });

  it('has proper button role', () => {
    render(<ButtonOutlineComponent text="Accessible" onClick={() => {}} />);
    
    const button = screen.getByRole('button');
    expect(button).toBeInTheDocument();
  });

  it('is keyboard accessible', () => {
    const mockOnClick = vi.fn();
    render(<ButtonOutlineComponent text="Keyboard" onClick={mockOnClick} />);
    
    const button = screen.getByRole('button');
    expect(button).not.toBeDisabled();
    
    fireEvent.click(button);
    expect(mockOnClick).toHaveBeenCalledTimes(1);
  });

  it('renders with all CSS custom properties', () => {
    render(<ButtonOutlineComponent text="Styled" onClick={() => {}} />);
    
    const button = screen.getByRole('button');
    expect(button).toHaveClass('border-[var(--button-outline-border)]');
    expect(button).toHaveClass('bg-[var(--button-outline-background)]');
    expect(button).toHaveClass('hover:bg-[var(--button-outline-background-hover)]');
    expect(button).toHaveClass('hover:text-[var(--button-outline-text-hover)]');
    expect(button).toHaveClass('text-[var(--button-outline-text)]');
  });

  it('has correct dimensions and spacing', () => {
    render(<ButtonOutlineComponent text="Dimensions" onClick={() => {}} />);
    
    const button = screen.getByRole('button');
    expect(button).toHaveClass('h-[28px]', 'px-[16px]', 'pt-[1px]');
  });

  it('has correct border and border radius', () => {
    render(<ButtonOutlineComponent text="Border" onClick={() => {}} />);
    
    const button = screen.getByRole('button');
    expect(button).toHaveClass('border-[1px]', 'rounded-[5px]');
  });
});