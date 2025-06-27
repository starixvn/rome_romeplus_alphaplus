import React from 'react';
import { describe, it, expect, vi } from 'vitest';
import { render, screen, fireEvent } from '@testing-library/react';
import '@testing-library/jest-dom';
import { ButtonApplyComponent } from './ButtonApplyComponent';
import { ButtonComponentProps } from './types';

describe('ButtonApplyComponent', () => {
  const defaultProps: ButtonComponentProps = {
    text: 'Apply',
    onClick: vi.fn(),
  };

  it('renders without crashing', () => {
    render(<ButtonApplyComponent {...defaultProps} />);
    expect(screen.getByText('Apply')).toBeInTheDocument();
  });

  it('displays the correct text', () => {
    render(<ButtonApplyComponent {...defaultProps} />);
    expect(screen.getByText('Apply')).toBeInTheDocument();
  });

  it('calls onClick when clicked', () => {
    const mockOnClick = vi.fn();
    render(<ButtonApplyComponent text="Apply" onClick={mockOnClick} />);
    
    const button = screen.getByText('Apply');
    fireEvent.click(button);
    
    expect(mockOnClick).toHaveBeenCalledTimes(1);
  });

  it('renders with correct CSS classes', () => {
    render(<ButtonApplyComponent {...defaultProps} />);
    
    const button = screen.getByText('Apply');
    expect(button).toHaveClass(
      'min-w-[90px]',
      'text-[var(--button-text)]',
      'bg-[var(--button-background)]',
      'hover:bg-[var(--button-background-hover)]',
      'rounded-[3px]',
      'h-[28px]',
      'pt-[1px]',
      'px-[16px]',
      'font-semibold',
      'outline-none',
      'cursor-pointer'
    );
  });

  it('applies disabled state correctly', () => {
    render(<ButtonApplyComponent {...defaultProps} disabled={true} />);
    
    const button = screen.getByText('Apply');
    expect(button).toBeDisabled();
    expect(button).toHaveClass('cursor-not-allowed', 'opacity-80');
    expect(button).not.toHaveClass('cursor-pointer');
  });

  it('applies enabled state correctly', () => {
    render(<ButtonApplyComponent {...defaultProps} disabled={false} />);
    
    const button = screen.getByText('Apply');
    expect(button).not.toBeDisabled();
    expect(button).toHaveClass('cursor-pointer');
    expect(button).not.toHaveClass('cursor-not-allowed', 'opacity-80');
  });

  it('renders with custom className', () => {
    const customClass = 'custom-button-class';
    render(<ButtonApplyComponent {...defaultProps} className={customClass} />);
    
    const container = screen.getByText('Apply').closest('div');
    expect(container).toHaveClass(customClass);
  });

  it('renders with correct button type', () => {
    render(<ButtonApplyComponent {...defaultProps} type="submit" />);
    
    const button = screen.getByText('Apply');
    expect(button).toHaveAttribute('type', 'submit');
  });

  it('renders with default button type when not provided', () => {
    render(<ButtonApplyComponent {...defaultProps} />);
    
    const button = screen.getByText('Apply');
    expect(button).toHaveAttribute('type', 'button');
  });

  it('renders with title attribute', () => {
    const title = 'Apply changes';
    render(<ButtonApplyComponent {...defaultProps} title={title} />);
    
    const button = screen.getByText('Apply');
    expect(button).toHaveAttribute('title', title);
  });

  it('renders with empty title when not provided', () => {
    render(<ButtonApplyComponent {...defaultProps} />);
    
    const button = screen.getByText('Apply');
    expect(button).toHaveAttribute('title', '');
  });

  it('handles click events correctly when enabled', () => {
    const mockOnClick = vi.fn();
    render(<ButtonApplyComponent text="Apply" onClick={mockOnClick} disabled={false} />);
    
    const button = screen.getByText('Apply');
    fireEvent.click(button);
    
    expect(mockOnClick).toHaveBeenCalledTimes(1);
  });

  it('does not call onClick when disabled', () => {
    const mockOnClick = vi.fn();
    render(<ButtonApplyComponent text="Apply" onClick={mockOnClick} disabled={true} />);
    
    const button = screen.getByText('Apply');
    fireEvent.click(button);
    
    expect(mockOnClick).not.toHaveBeenCalled();
  });

  it('works without onClick prop', () => {
    render(<ButtonApplyComponent text="Apply" />);
    
    const button = screen.getByText('Apply');
    expect(button).toBeInTheDocument();
    
    // Should not throw error when clicked without onClick handler
    expect(() => fireEvent.click(button)).not.toThrow();
  });

  it('maintains consistent DOM structure', () => {
    const { container } = render(<ButtonApplyComponent {...defaultProps} />);
    
    // Check the overall structure
    const outerDiv = container.firstChild as HTMLElement;
    expect(outerDiv.tagName).toBe('DIV');
    
    const button = outerDiv.firstChild as HTMLElement;
    expect(button.tagName).toBe('BUTTON');
    expect(button.textContent).toBe('Apply');
  });

  it('applies hover styles correctly', () => {
    render(<ButtonApplyComponent {...defaultProps} />);
    
    const button = screen.getByText('Apply');
    expect(button).toHaveClass('hover:bg-[var(--button-background-hover)]');
  });

  it('renders with different text content', () => {
    const customText = 'Save Changes';
    render(<ButtonApplyComponent text={customText} onClick={vi.fn()} />);
    
    expect(screen.getByText(customText)).toBeInTheDocument();
  });

  it('handles all button types correctly', () => {
    const types: Array<'button' | 'submit' | 'reset'> = ['button', 'submit', 'reset'];
    
    types.forEach(type => {
      const { unmount } = render(<ButtonApplyComponent {...defaultProps} type={type} />);
      const button = screen.getByText('Apply');
      expect(button).toHaveAttribute('type', type);
      unmount();
    });
  });

  it('combines custom className with default structure', () => {
    const customClass = 'my-custom-class';
    render(<ButtonApplyComponent {...defaultProps} className={customClass} />);
    
    const container = screen.getByText('Apply').closest('div');
    expect(container).toHaveClass(customClass);
  });

  it('renders with all props provided', () => {
    const allProps = {
      text: 'Confirm',
      onClick: vi.fn(),
      disabled: false,
      type: 'submit' as const,
      className: 'test-class',
      title: 'Confirm action'
    };
    
    render(<ButtonApplyComponent {...allProps} />);
    
    const button = screen.getByText('Confirm');
    expect(button).toBeInTheDocument();
    expect(button).toHaveAttribute('type', 'submit');
    expect(button).toHaveAttribute('title', 'Confirm action');
    expect(button).not.toBeDisabled();
    
    const container = button.closest('div');
    expect(container).toHaveClass('test-class');
  });
}); 