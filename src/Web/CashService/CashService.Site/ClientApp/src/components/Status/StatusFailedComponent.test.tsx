import React from 'react';
import { describe, it, expect, vi, beforeEach } from 'vitest';
import { render, screen } from '@testing-library/react';
import '@testing-library/jest-dom';
import { StatusFailedComponent } from './StatusFailedComponent';
import { useTranslation } from 'react-i18next';

// Mock react-i18next
vi.mock('react-i18next', () => ({
  useTranslation: vi.fn()
}));

describe('StatusFailedComponent', () => {
  beforeEach(() => {
    vi.mocked(useTranslation).mockReturnValue({
      t: (key: string) => {
        const translations = {
          'failed': 'Failed'
        };
        return translations[key] || key;
      }
    });
  });

  it('renders with default text when no text prop is provided', () => {
    render(<StatusFailedComponent />);
    
    const statusElement = screen.getByRole('alert');
    expect(statusElement).toBeInTheDocument();
    
    const spanElement = statusElement.querySelector('span');
    expect(spanElement).toHaveTextContent('Failed');
  });

  it('renders with custom text when text prop is provided', () => {
    const customText = 'Custom Failed Status';
    render(<StatusFailedComponent text={customText} />);
    
    const statusElement = screen.getByRole('alert');
    expect(statusElement).toBeInTheDocument();
    
    const spanElement = statusElement.querySelector('span');
    expect(spanElement).toHaveTextContent(customText);
  });

  it('renders with empty string when text prop is empty string', () => {
    render(<StatusFailedComponent text="" />);
    
    const spanElement = screen.getByText('Failed');
    expect(spanElement).toBeInTheDocument();
  });
  

  it('has correct CSS classes for styling', () => {
    render(<StatusFailedComponent />);
    
    const spanElement = screen.getByRole('alert').querySelector('span');
    expect(spanElement).toHaveClass(
      'inline-block',
      'font-semibold',
      'rounded-[4px]',
      'min-w-[90px]',
      'text-center',
      'px-[10px]',
      'py-[3px]',
      'bg-[#E0E0E0]',
      'dark:bg-[#6f6f6f]'
    );
  });

  it('has correct accessibility attributes', () => {
    render(<StatusFailedComponent />);
    
    const alertElement = screen.getByRole('alert');
    expect(alertElement).toBeInTheDocument();
  });

  it('renders span element with correct structure', () => {
    render(<StatusFailedComponent />);
    
    const alertElement = screen.getByRole('alert');
    const spanElement = alertElement.querySelector('span');
    
    expect(spanElement).toBeInTheDocument();
    expect(spanElement?.tagName).toBe('SPAN');
  });

  it('handles special characters in text prop', () => {
    const specialText = 'Failed with special chars: !@#$%^&*()';
    render(<StatusFailedComponent text={specialText} />);
    
    const spanElement = screen.getByRole('alert').querySelector('span');
    expect(spanElement).toHaveTextContent(specialText);
  });

  it('handles unicode characters in text prop', () => {
    const unicodeText = 'Failed with unicode: 中文 Español Français';
    render(<StatusFailedComponent text={unicodeText} />);
    
    const spanElement = screen.getByRole('alert').querySelector('span');
    expect(spanElement).toHaveTextContent(unicodeText);
  });

  it('maintains consistent styling regardless of text content', () => {
    const { rerender } = render(<StatusFailedComponent text="Short" />);
    
    let spanElement = screen.getByRole('alert').querySelector('span');
    const initialClasses = spanElement?.className;
    
    rerender(<StatusFailedComponent text="Very long failed status text that might wrap" />);
    
    spanElement = screen.getByRole('alert').querySelector('span');
    expect(spanElement?.className).toBe(initialClasses);
  });

  it('uses correct translation key for default text', () => {
    const mockT = vi.fn((key: string) => key) as any;
    vi.mocked(useTranslation).mockReturnValue({
      t: mockT
    });

    render(<StatusFailedComponent />);
    
    expect(mockT).toHaveBeenCalledWith('failed');
  });

  it('does not call translation when custom text is provided', () => {
    const mockT = vi.fn((key: string) => key) as any;
    vi.mocked(useTranslation).mockReturnValue({
      t: mockT
    });

    render(<StatusFailedComponent text="Custom Text" />);
    
    expect(mockT).not.toHaveBeenCalled();
  });
}); 