import React from 'react';
import { describe, it, expect, vi } from 'vitest';
import { render, screen } from '@testing-library/react';
import '@testing-library/jest-dom';
import { StatusMaintenanceComponent } from './StatusMaintenanceComponent';

// Mock react-i18next
vi.mock('react-i18next', () => ({
  useTranslation: () => ({
    t: (key: string) => {
      const translations = {
        'maintenance': 'Maintenance'
      };
      return translations[key] || key;
    }
  })
}));

describe('StatusMaintenanceComponent', () => {
  it('renders with default text when no text prop is provided', () => {
    render(<StatusMaintenanceComponent />);
    
    const statusElement = screen.getByRole('alert');
    expect(statusElement).toBeInTheDocument();
    
    const spanElement = statusElement.querySelector('span');
    expect(spanElement).toHaveTextContent('Maintenance');
  });

  it('renders with custom text when text prop is provided', () => {
    const customText = 'Custom Maintenance Status';
    render(<StatusMaintenanceComponent text={customText} />);
    
    const statusElement = screen.getByRole('alert');
    expect(statusElement).toBeInTheDocument();
    
    const spanElement = statusElement.querySelector('span');
    expect(spanElement).toHaveTextContent(customText);
  });

  it('renders with empty string when text prop is empty string', () => {
    render(<StatusMaintenanceComponent text="" />);
    
    const statusElement = screen.getByRole('alert');
    expect(statusElement).toBeInTheDocument();
    
    const spanElement = statusElement.querySelector('span');
    expect(spanElement).toHaveTextContent('');
  });

  it('has correct CSS classes for styling', () => {
    render(<StatusMaintenanceComponent />);
    
    const spanElement = screen.getByRole('alert').querySelector('span');
    expect(spanElement).toHaveClass(
      'inline-block',
      'font-semibold',
      'rounded-[4px]',
      'min-w-[90px]',
      'text-center',
      'px-[10px]',
      'py-[3px]',
      'bg-[#fcae7b]',
      'dark:bg-[#955e39]'
    );
  });

  it('has correct accessibility attributes', () => {
    render(<StatusMaintenanceComponent />);
    
    const alertElement = screen.getByRole('alert');
    expect(alertElement).toBeInTheDocument();
  });

  it('renders span element with correct structure', () => {
    render(<StatusMaintenanceComponent />);
    
    const alertElement = screen.getByRole('alert');
    const spanElement = alertElement.querySelector('span');
    
    expect(spanElement).toBeInTheDocument();
    expect(spanElement?.tagName).toBe('SPAN');
  });

  it('handles special characters in text prop', () => {
    const specialText = 'Maintenance with special chars: !@#$%^&*()';
    render(<StatusMaintenanceComponent text={specialText} />);
    
    const spanElement = screen.getByRole('alert').querySelector('span');
    expect(spanElement).toHaveTextContent(specialText);
  });

  it('handles unicode characters in text prop', () => {
    const unicodeText = 'Maintenance with unicode: 中文 Español Français';
    render(<StatusMaintenanceComponent text={unicodeText} />);
    
    const spanElement = screen.getByRole('alert').querySelector('span');
    expect(spanElement).toHaveTextContent(unicodeText);
  });

  it('maintains consistent styling regardless of text content', () => {
    const { rerender } = render(<StatusMaintenanceComponent text="Short" />);
    
    let spanElement = screen.getByRole('alert').querySelector('span');
    const initialClasses = spanElement?.className;
    
    rerender(<StatusMaintenanceComponent text="Very long maintenance status text that might wrap" />);
    
    spanElement = screen.getByRole('alert').querySelector('span');
    expect(spanElement?.className).toBe(initialClasses);
  });

  it('uses maintenance-specific background colors', () => {
    render(<StatusMaintenanceComponent />);
    
    const spanElement = screen.getByRole('alert').querySelector('span');
    expect(spanElement).toHaveClass('bg-[#fcae7b]', 'dark:bg-[#955e39]');
  });

  it('renders with null text prop', () => {
    render(<StatusMaintenanceComponent text={null as any} />);
    
    const statusElement = screen.getByRole('alert');
    expect(statusElement).toBeInTheDocument();
    
    const spanElement = statusElement.querySelector('span');
    expect(spanElement).toHaveTextContent('Maintenance');
  });

  it('renders with undefined text prop', () => {
    render(<StatusMaintenanceComponent text={undefined} />);
    
    const statusElement = screen.getByRole('alert');
    expect(statusElement).toBeInTheDocument();
    
    const spanElement = statusElement.querySelector('span');
    expect(spanElement).toHaveTextContent('Maintenance');
  });
}); 