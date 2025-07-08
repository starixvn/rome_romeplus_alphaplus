import React from 'react';
import { describe, it, expect } from 'vitest';
import { render, screen } from '@testing-library/react';
import '@testing-library/jest-dom';
import { WarningMessageComponent } from './WarningMessageComponent';

describe('WarningMessageComponent', () => {
  it('should render warning message when message is provided', () => {
    const testMessage = 'This is a warning message';
    render(<WarningMessageComponent message={testMessage} />);
    
    expect(screen.getByText(testMessage)).toBeInTheDocument();
    expect(screen.getByRole('alert')).toBeInTheDocument();
  });

  it('should not render when message is null', () => {
    const { container } = render(<WarningMessageComponent message={null} />);
    
    expect(container.firstChild).toBeNull();
  });

  it('should not render when message is empty string', () => {
    const { container } = render(<WarningMessageComponent message="" />);
    
    expect(container.firstChild).toBeNull();
  });

  it('should apply custom className when provided', () => {
    const testMessage = 'Warning message';
    const customClass = 'custom-warning-class';
    
    render(<WarningMessageComponent message={testMessage} className={customClass} />);
    
    const alertContainer = screen.getByRole('alert');
    expect(alertContainer).toHaveClass(customClass);
    expect(alertContainer).toHaveClass('max-[768px]:py-[12px]');
  });

  it('should render with default className when no className is provided', () => {
    const testMessage = 'Warning message';
    
    render(<WarningMessageComponent message={testMessage} />);
    
    const alertContainer = screen.getByRole('alert');
    expect(alertContainer).toHaveClass('max-[768px]:py-[12px]');
  });

  it('should render HTML content safely using dangerouslySetInnerHTML', () => {
    const htmlMessage = '<strong>Bold warning</strong> with <em>emphasis</em>';
    
    render(<WarningMessageComponent message={htmlMessage} />);
    
    const alertContainer = screen.getByRole('alert');
    const messageDiv = alertContainer.querySelector('div[class*="bg-[#FFFFFA]"]');
    
    expect(messageDiv?.innerHTML).toContain('<strong>Bold warning</strong>');
    expect(messageDiv?.innerHTML).toContain('<em>emphasis</em>');
  });

  it('should have correct styling classes for warning appearance', () => {
    const testMessage = 'Warning message';
    
    render(<WarningMessageComponent message={testMessage} />);
    
    const messageContainer = screen.getByRole('alert').querySelector('div[class*="bg-[#FFFFFA]"]');
    
    expect(messageContainer).toHaveClass('bg-[#FFFFFA]');
    expect(messageContainer).toHaveClass('dark:bg-[#312F26]');
    expect(messageContainer).toHaveClass('border-[#C6A180]');
    expect(messageContainer).toHaveClass('dark:border-[#78776D]');
    expect(messageContainer).toHaveClass('border-[1px]');
    expect(messageContainer).toHaveClass('rounded-[3px]');
    expect(messageContainer).toHaveClass('pl-[35px]');
    expect(messageContainer).toHaveClass('pr-[7px]');
    expect(messageContainer).toHaveClass('py-[8px]');
    expect(messageContainer).toHaveClass('leading-[18px]');
    expect(messageContainer).toHaveClass('relative');
    expect(messageContainer).toHaveClass('font-[500]');
  });

  it('should render warning icon with correct styling', () => {
    const testMessage = 'Warning message';
    
    render(<WarningMessageComponent message={testMessage} />);
    
    const icon = screen.getByRole('alert').querySelector('svg');
    
    expect(icon).toBeInTheDocument();
    expect(icon).toHaveClass('w-[20px]');
    expect(icon).toHaveClass('h-[20px]');
    expect(icon).toHaveClass('fill-[#A75000]');
    expect(icon).toHaveClass('dark:fill-[#EB8526]');
    expect(icon).toHaveClass('absolute');
    expect(icon).toHaveClass('top-[7px]');
    expect(icon).toHaveClass('left-[10px]');
  });

  it('should render warning icon in correct position', () => {
    const testMessage = 'Warning message';
    
    render(<WarningMessageComponent message={testMessage} />);
    
    const messageContainer = screen.getByRole('alert').querySelector('div[class*="bg-[#FFFFFA]"]');
    const icon = messageContainer?.querySelector('svg');
    
    expect(icon).toHaveClass('absolute');
    expect(icon).toHaveClass('top-[7px]');
    expect(icon).toHaveClass('left-[10px]');
  });

  it('should handle complex HTML content', () => {
    const complexHtmlMessage = '<div><p>Paragraph with <a href="#">link</a></p><ul><li>List item</li></ul></div>';
    
    render(<WarningMessageComponent message={complexHtmlMessage} />);
    
    const alertContainer = screen.getByRole('alert');
    const link = alertContainer.querySelector('a');
    const listItem = alertContainer.querySelector('li');

    expect(link).toHaveAttribute('href', '#');
    expect(link).toHaveTextContent('link');
    expect(listItem).toHaveTextContent('List item');
  });

  it('should maintain accessibility with role="alert"', () => {
    const testMessage = 'Warning message';
    
    render(<WarningMessageComponent message={testMessage} />);
    
    const alertElement = screen.getByRole('alert');
    expect(alertElement).toBeInTheDocument();
  });

  it('should combine multiple custom classes correctly', () => {
    const testMessage = 'Warning message';
    const customClass1 = 'custom-class-1';
    const customClass2 = 'custom-class-2';
    
    render(
      <WarningMessageComponent 
        message={testMessage} 
        className={`${customClass1} ${customClass2}`} 
      />
    );
    
    const alertContainer = screen.getByRole('alert');
    expect(alertContainer).toHaveClass(customClass1);
    expect(alertContainer).toHaveClass(customClass2);
    expect(alertContainer).toHaveClass('max-[768px]:py-[12px]');
  });

  it('should handle special characters in message', () => {
    const specialMessage = 'Warning with special chars: & < > " \'';
    
    render(<WarningMessageComponent message={specialMessage} />);
    
    const alertContainer = screen.getByRole('alert');
    const messageDiv = alertContainer.querySelector('div[class*="bg-[#FFFFFA]"]');
    
    expect(messageDiv).toHaveTextContent('Warning with special chars: & < > " \'');
  });
}); 