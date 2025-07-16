import React from 'react';
import { describe, it, expect, vi } from 'vitest';
import { render, screen } from '@testing-library/react';
import '@testing-library/jest-dom';
import { StatusCompletedComponent } from './StatusCompletedComponent';

// Mock react-i18next
vi.mock('react-i18next', () => ({
  useTranslation: () => ({
    t: (key: string) => {
      const translations = {
        'status.completed': 'Completed'
      };
      return translations[key] || key;
    }
  })
}));

describe('StatusCompletedComponent', () => {
  it('renders with default text when no text prop is provided', () => {
    render(<StatusCompletedComponent />);
    
    const statusElement = screen.getByRole('alert');
    expect(statusElement).toBeInTheDocument();
    
    const spanElement = statusElement.querySelector('span');
    expect(spanElement).toHaveTextContent('Completed');
  });

  it('renders with custom text when text prop is provided', () => {
    const customText = 'Custom Status';
    render(<StatusCompletedComponent text={customText} />);
    
    const statusElement = screen.getByRole('alert');
    expect(statusElement).toBeInTheDocument();
    
    const spanElement = statusElement.querySelector('span');
    expect(spanElement).toHaveTextContent(customText);
  });

  it('renders with empty string when text prop is empty string', () => {
    render(<StatusCompletedComponent text="" />);
    
    const statusElement = screen.getByRole('alert');
    expect(statusElement).toBeInTheDocument();
    
    const spanElement = statusElement.querySelector('span');
    expect(spanElement).toHaveTextContent('');
  });

  it('has correct CSS classes for styling', () => {
    render(<StatusCompletedComponent />);
    
    const spanElement = screen.getByRole('alert').querySelector('span');
    expect(spanElement).toHaveClass(
      'inline-block',
      'font-semibold',
      'rounded-[4px]',
      'min-w-[90px]',
      'text-center',
      'px-[10px]',
      'py-[3px]',
      'bg-[#D4E7D9]',
      'dark:bg-[#24853e]'
    );
  });

  it('has correct accessibility attributes', () => {
    render(<StatusCompletedComponent />);
    
    const alertElement = screen.getByRole('alert');
    expect(alertElement).toBeInTheDocument();
  });

  it('renders span element with correct structure', () => {
    render(<StatusCompletedComponent />);
    
    const alertElement = screen.getByRole('alert');
    const spanElement = alertElement.querySelector('span');
    
    expect(spanElement).toBeInTheDocument();
    expect(spanElement?.tagName).toBe('SPAN');
  });

  it('handles special characters in text prop', () => {
    const specialText = 'Status with special chars: !@#$%^&*()';
    render(<StatusCompletedComponent text={specialText} />);
    
    const spanElement = screen.getByRole('alert').querySelector('span');
    expect(spanElement).toHaveTextContent(specialText);
  });

  it('handles unicode characters in text prop', () => {
    const unicodeText = 'Status with unicode: 中文 Español Français';
    render(<StatusCompletedComponent text={unicodeText} />);
    
    const spanElement = screen.getByRole('alert').querySelector('span');
    expect(spanElement).toHaveTextContent(unicodeText);
  });

  it('maintains consistent styling regardless of text content', () => {
    const { rerender } = render(<StatusCompletedComponent text="Short" />);
    
    let spanElement = screen.getByRole('alert').querySelector('span');
    const initialClasses = spanElement?.className;
    
    rerender(<StatusCompletedComponent text="Very long status text that might wrap" />);
    
    spanElement = screen.getByRole('alert').querySelector('span');
    expect(spanElement?.className).toBe(initialClasses);
  });
}); 