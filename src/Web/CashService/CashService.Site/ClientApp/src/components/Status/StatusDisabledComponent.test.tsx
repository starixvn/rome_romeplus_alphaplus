import React from 'react';
import { describe, it, expect, vi } from 'vitest';
import { render, screen } from '@testing-library/react';
import '@testing-library/jest-dom';
import { StatusDisabledComponent } from './StatusDisabledComponent';

// Mock react-i18next
vi.mock('react-i18next', () => ({
  useTranslation: () => ({
    t: (key: string) => {
      const translations = {
        'disabled': 'Disabled'
      };
      return translations[key] || key;
    }
  })
}));

describe('StatusDisabledComponent', () => {
  it('renders with default text when no text prop is provided', () => {
    render(<StatusDisabledComponent />);
    
    const statusElement = screen.getByRole('alert');
    expect(statusElement).toBeInTheDocument();
    
    const spanElement = statusElement.querySelector('span');
    expect(spanElement).toHaveTextContent('Disabled');
  });

  it('renders with custom text when text prop is provided', () => {
    const customText = 'Custom Disabled Status';
    render(<StatusDisabledComponent text={customText} />);
    
    const statusElement = screen.getByRole('alert');
    expect(statusElement).toBeInTheDocument();
    
    const spanElement = statusElement.querySelector('span');
    expect(spanElement).toHaveTextContent(customText);
  });

  it('renders with empty string when text prop is empty string', () => {
    render(<StatusDisabledComponent text="" />);
    
    const statusElement = screen.getByRole('alert');
    expect(statusElement).toBeInTheDocument();
    
    const spanElement = statusElement.querySelector('span');
    expect(spanElement).toHaveTextContent('');
  });

  it('has correct CSS classes for styling', () => {
    render(<StatusDisabledComponent />);
    
    const spanElement = screen.getByRole('alert').querySelector('span');
    expect(spanElement).toHaveClass(
      'inline-block',
      'font-semibold',
      'rounded-[4px]',
      'min-w-[90px]',
      'text-center',
      'px-[10px]',
      'py-[3px]',
      'bg-[#FBD8D8]',
      'dark:bg-[#af4646]'
    );
  });

  it('has correct accessibility attributes', () => {
    render(<StatusDisabledComponent />);
    
    const alertElement = screen.getByRole('alert');
    expect(alertElement).toBeInTheDocument();
  });

  it('renders span element with correct structure', () => {
    render(<StatusDisabledComponent />);
    
    const alertElement = screen.getByRole('alert');
    const spanElement = alertElement.querySelector('span');
    
    expect(spanElement).toBeInTheDocument();
    expect(spanElement?.tagName).toBe('SPAN');
  });

  it('handles special characters in text prop', () => {
    const specialText = 'Disabled with special chars: !@#$%^&*()';
    render(<StatusDisabledComponent text={specialText} />);
    
    const spanElement = screen.getByRole('alert').querySelector('span');
    expect(spanElement).toHaveTextContent(specialText);
  });

  it('handles unicode characters in text prop', () => {
    const unicodeText = 'Disabled with unicode: 中文 Español Français';
    render(<StatusDisabledComponent text={unicodeText} />);
    
    const spanElement = screen.getByRole('alert').querySelector('span');
    expect(spanElement).toHaveTextContent(unicodeText);
  });

  it('maintains consistent styling regardless of text content', () => {
    const { rerender } = render(<StatusDisabledComponent text="Short" />);
    
    let spanElement = screen.getByRole('alert').querySelector('span');
    const initialClasses = spanElement?.className;
    
    rerender(<StatusDisabledComponent text="Very long disabled status text that might wrap" />);
    
    spanElement = screen.getByRole('alert').querySelector('span');
    expect(spanElement?.className).toBe(initialClasses);
  });


}); 