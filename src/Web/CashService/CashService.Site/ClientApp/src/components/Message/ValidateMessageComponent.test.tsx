import React from 'react';
import { describe, it, expect } from 'vitest';
import { render, screen } from '@testing-library/react';
import { ValidateMessageComponent } from './ValidateMessageComponent';

describe('ValidateMessageComponent', () => {
  it('should render message when message is provided', () => {
    const testMessage = 'This is a validation message';
    render(<ValidateMessageComponent message={testMessage} />);
    
    expect(screen.getByText(testMessage)).toBeInTheDocument();
  });

  it('should not render when message is empty string', () => {
    const { container } = render(<ValidateMessageComponent message="" />);
    
    expect(container.firstChild).toBeNull();
  });

  it('should not render when message is falsy', () => {
    const { container } = render(<ValidateMessageComponent message={null as any} />);
    
    expect(container.firstChild).toBeNull();
  });

  it('should render with custom className', () => {
    const testMessage = 'Validation message';
    const customClassName = 'custom-validation-class';
    
    render(<ValidateMessageComponent message={testMessage} className={customClassName} />);
    
    const messageElement = screen.getByText(testMessage);
    expect(messageElement).toHaveClass(customClassName);
    expect(messageElement).toHaveClass('mt-[2px]');
    expect(messageElement).toHaveClass('text-[#FE0000]');
    expect(messageElement).toHaveClass('dark:text-[#EB5757]');
  });

  it('should render with default className when no className is provided', () => {
    const testMessage = 'Validation message';
    
    render(<ValidateMessageComponent message={testMessage} />);
    
    const messageElement = screen.getByText(testMessage);
    expect(messageElement).toHaveClass('mt-[2px]');
    expect(messageElement).toHaveClass('text-[#FE0000]');
    expect(messageElement).toHaveClass('dark:text-[#EB5757]');
  });

  it('should have correct styling classes', () => {
    const testMessage = 'Validation message';
    
    render(<ValidateMessageComponent message={testMessage} />);
    
    const messageElement = screen.getByText(testMessage);
    expect(messageElement).toHaveClass(
      'mt-[2px]',
      'text-[#FE0000]',
      'dark:text-[#EB5757]'
    );
  });

  it('should handle HTML content safely', () => {
    const htmlMessage = 'Validation with <strong>bold text</strong> and <em>italic text</em>';
    
    render(<ValidateMessageComponent message={htmlMessage} />);
    
    const messageElement = screen.getByText(htmlMessage);
    expect(messageElement).toHaveTextContent(
        'Validation with <strong>bold text</strong> and <em>italic text</em>'
      );
  });

  it('should handle special characters in message', () => {
    const specialCharsMessage = 'Validation with special chars: & < > " \'';
    
    render(<ValidateMessageComponent message={specialCharsMessage} />);
    
    expect(screen.getByText(specialCharsMessage)).toBeInTheDocument();
  });

  it('should handle very long messages', () => {
    const longMessage = 'A'.repeat(1000);
    
    render(<ValidateMessageComponent message={longMessage} />);
    
    expect(screen.getByText(longMessage)).toBeInTheDocument();
  }); 

  it('should render as a div element', () => {
    const testMessage = 'Validation message';
    
    render(<ValidateMessageComponent message={testMessage} />);
    
    const messageElement = screen.getByText(testMessage);
    expect(messageElement.tagName).toBe('DIV');
  });

  it('should combine custom className with default classes', () => {
    const testMessage = 'Validation message';
    const customClassName = 'my-custom-class';
    
    render(<ValidateMessageComponent message={testMessage} className={customClassName} />);
    
    const messageElement = screen.getByText(testMessage);
    expect(messageElement).toHaveClass(customClassName);
    expect(messageElement).toHaveClass('mt-[2px]');
    expect(messageElement).toHaveClass('text-[#FE0000]');
    expect(messageElement).toHaveClass('dark:text-[#EB5757]');
  });

  it('should handle multiple custom classes', () => {
    const testMessage = 'Validation message';
    const customClassName = 'class1 class2 class3';
    
    render(<ValidateMessageComponent message={testMessage} className={customClassName} />);
    
    const messageElement = screen.getByText(testMessage);
    expect(messageElement).toHaveClass('class1', 'class2', 'class3');
    expect(messageElement).toHaveClass('mt-[2px]');
    expect(messageElement).toHaveClass('text-[#FE0000]');
    expect(messageElement).toHaveClass('dark:text-[#EB5757]');
  });

  it('should handle undefined className prop', () => {
    const testMessage = 'Validation message';
    
    render(<ValidateMessageComponent message={testMessage} className={undefined} />);
    
    const messageElement = screen.getByText(testMessage);
    expect(messageElement).toHaveClass('mt-[2px]');
    expect(messageElement).toHaveClass('text-[#FE0000]');
    expect(messageElement).toHaveClass('dark:text-[#EB5757]');
  });

  it('should handle empty className prop', () => {
    const testMessage = 'Validation message';
    
    render(<ValidateMessageComponent message={testMessage} className="" />);
    
    const messageElement = screen.getByText(testMessage);
    expect(messageElement).toHaveClass('mt-[2px]');
    expect(messageElement).toHaveClass('text-[#FE0000]');
    expect(messageElement).toHaveClass('dark:text-[#EB5757]');
  });

  it('should handle numeric message content', () => {
    const numericMessage = '12345';
    
    render(<ValidateMessageComponent message={numericMessage} />);
    
    expect(screen.getByText(numericMessage)).toBeInTheDocument();
  });

  it('should handle message with emojis', () => {
    const emojiMessage = 'Validation message with 🚀 emoji';
    
    render(<ValidateMessageComponent message={emojiMessage} />);
    
    expect(screen.getByText(emojiMessage)).toBeInTheDocument();
  });

  it('should handle message with unicode characters', () => {
    const unicodeMessage = 'Validation with unicode: 你好世界 ñáéíóú';
    
    render(<ValidateMessageComponent message={unicodeMessage} />);
    
    expect(screen.getByText(unicodeMessage)).toBeInTheDocument();
  });
}); 