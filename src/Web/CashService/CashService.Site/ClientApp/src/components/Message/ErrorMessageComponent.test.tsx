import React from 'react';
import { describe, it, expect } from 'vitest';
import { render, screen } from '@testing-library/react';
import { ErrorMessageComponent } from './ErrorMessageComponent';

describe('ErrorMessageComponent', () => {
  it('should render error message when message is provided', () => {
    const testMessage = 'This is an error message';
    render(<ErrorMessageComponent message={testMessage} />);
    
    expect(screen.getByText(testMessage)).toBeInTheDocument();
    expect(screen.getByRole('alert')).toBeInTheDocument();
  });

  it('should not render when message is null', () => {
    const { container } = render(<ErrorMessageComponent message={null} />);
    
    expect(container.firstChild).toBeNull();
  });

  it('should not render when message is empty string', () => {
    const { container } = render(<ErrorMessageComponent message="" />);
    
    expect(container.firstChild).toBeNull();
  });

  it('should render with custom className', () => {
    const testMessage = 'Error message';
    const customClassName = 'custom-error-class';
    
    render(<ErrorMessageComponent message={testMessage} className={customClassName} />);
    
    const alertElement = screen.getByRole('alert');
    expect(alertElement).toHaveClass(customClassName);
    expect(alertElement).toHaveClass('max-[768px]:py-[12px]');
  });

  it('should render with default className when no className is provided', () => {
    const testMessage = 'Error message';
    
    render(<ErrorMessageComponent message={testMessage} />);
    
    const alertElement = screen.getByRole('alert');
    expect(alertElement).toHaveClass('max-[768px]:py-[12px]');
  });

  it('should render HTML content safely', () => {
    const htmlMessage = 'Error with <strong>bold text</strong> and <em>italic text</em>';
    
    render(<ErrorMessageComponent message={htmlMessage} />);
    
    const alertElement = screen.getByRole('alert');
    expect(alertElement).toContainHTML('<strong>bold text</strong>');
    expect(alertElement).toContainHTML('<em>italic text</em>');
  });

  it('should have correct styling classes', () => {
    const testMessage = 'Error message';
    
    render(<ErrorMessageComponent message={testMessage} />);
    
    const messageContainer = screen.getByRole('alert').querySelector('div');
    expect(messageContainer).toHaveClass(
      'bg-[#FFE8E8]',
      'dark:bg-[#501B1B]',
      'border-[#E2B6B6]',
      'dark:border-[#FF8C8C]',
      'relative',
      'font-[500]',
      'border-[1px]',
      'rounded-[3px]',
      'pl-[35px]',
      'pr-[7px]',
      'py-[8px]',
      'leading-[18px]'
    );
  });

  it('should render error icon with correct styling', () => {
    const testMessage = 'Error message';
    
    render(<ErrorMessageComponent message={testMessage} />);
    
    const iconElement = screen.getByRole('alert').querySelector('svg');
    expect(iconElement).toBeInTheDocument();
    expect(iconElement).toHaveClass(
      'w-[20px]',
      'h-[20px]',
      'fill-[#CC0000]',
      'dark:fill-[#FF6D6D]',
      'absolute',
      'top-[7px]',
      'left-[10px]'
    );
  });

  it('should have correct accessibility attributes', () => {
    const testMessage = 'Error message';
    
    render(<ErrorMessageComponent message={testMessage} />);
    
    const alertElement = screen.getByRole('alert');
    expect(alertElement).toBeInTheDocument();
  });

  it('should handle complex HTML content', () => {
    const complexHtmlMessage = `
      <div>
        <h3>Error Title</h3>
        <p>Error description with <a href="#">link</a></p>
        <ul>
          <li>Error detail 1</li>
          <li>Error detail 2</li>
        </ul>
      </div>
    `;
    
    render(<ErrorMessageComponent message={complexHtmlMessage} />);
    
    const alertElement = screen.getByRole('alert');
    expect(alertElement).toContainHTML('<h3>Error Title</h3>');
    expect(alertElement).toContainHTML('<a href="#">link</a>');
    expect(alertElement).toContainHTML('<li>Error detail 1</li>');
  });

  it('should handle special characters in message', () => {
    const specialCharsMessage = 'Error with special chars: & < > " \'';
    
    render(<ErrorMessageComponent message={specialCharsMessage} />);
    
    expect(screen.getByText(specialCharsMessage)).toBeInTheDocument();
  });

  it('should handle very long messages', () => {
    const longMessage = 'A'.repeat(1000);
    
    render(<ErrorMessageComponent message={longMessage} />);
    
    expect(screen.getByText(longMessage)).toBeInTheDocument();
  });

  it('should handle whitespace-only messages', () => {
    const whitespaceMessage = '   \n\t   ';
    
    render(<ErrorMessageComponent message={whitespaceMessage} />);
    
    expect(screen.getByRole('alert')).toBeInTheDocument();
  });
}); 