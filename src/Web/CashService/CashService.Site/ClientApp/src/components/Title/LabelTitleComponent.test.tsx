import { describe, it, expect } from 'vitest';
import { render, screen } from '@testing-library/react';
import '@testing-library/jest-dom';
import React from 'react';
import { LabelTitleComponent } from './LabelTitleComponent';
import { BaseTitleProps } from './types';

describe('LabelTitleComponent', () => {
  const defaultProps: BaseTitleProps = {
    text: 'Test Label',
  };

  it('renders label with default props', () => {
    render(<LabelTitleComponent {...defaultProps} />);
    
    const labelElement = screen.getByText('Test Label');
    expect(labelElement).toBeInTheDocument();
    expect(labelElement.tagName).toBe('LABEL');
  });

  it('renders label with correct CSS classes', () => {
    render(<LabelTitleComponent {...defaultProps} />);
    
    const labelElement = screen.getByText('Test Label');
    expect(labelElement).toHaveClass(
      'block',
      'font-semibold',
      'text-[var(--titleComponent-label-color)]',
      'mb-[4px]',
      'capitalize'
    );
  });

  it('renders label with custom className', () => {
    const customClass = 'custom-class';
    render(<LabelTitleComponent {...defaultProps} className={customClass} />);
    
    const labelElement = screen.getByText('Test Label');
    expect(labelElement).toHaveClass(
      customClass,
      'block',
      'font-semibold',
      'text-[var(--titleComponent-label-color)]',
      'mb-[4px]',
      'capitalize'
    );
  });

  it('renders required asterisk when required is true', () => {
    render(<LabelTitleComponent {...defaultProps} required={true} />);
    
    const labelElement = screen.getByText('Test Label');
    const asteriskElement = screen.getByText('*');
    
    expect(labelElement).toBeInTheDocument();
    expect(asteriskElement).toBeInTheDocument();
    expect(asteriskElement).toHaveClass('text-[#FF0000]', 'dark:text-[#FF4F50]', 'ml-[4px]');
  });

  it('does not render required asterisk when required is false', () => {
    render(<LabelTitleComponent {...defaultProps} required={false} />);
    
    const labelElement = screen.getByText('Test Label');
    const asteriskElement = screen.queryByText('*');
    
    expect(labelElement).toBeInTheDocument();
    expect(asteriskElement).not.toBeInTheDocument();
  });

  it('does not render required asterisk when required is undefined', () => {
    render(<LabelTitleComponent {...defaultProps} />);
    
    const labelElement = screen.getByText('Test Label');
    const asteriskElement = screen.queryByText('*');
    
    expect(labelElement).toBeInTheDocument();
    expect(asteriskElement).not.toBeInTheDocument();
  });

  it('does not render when text is empty', () => {
    render(<LabelTitleComponent text="" />);
    
    const labelElement = screen.queryByRole('label');
    expect(labelElement).not.toBeInTheDocument();
  });

  it('does not render when text is null', () => {
    render(<LabelTitleComponent text={null as any} />);
    
    const labelElement = screen.queryByRole('label');
    expect(labelElement).not.toBeInTheDocument();
  });

  it('does not render when text is undefined', () => {
    render(<LabelTitleComponent text={undefined as any} />);
    
    const labelElement = screen.queryByRole('label');
    expect(labelElement).not.toBeInTheDocument();
  });

  it('renders label with special characters', () => {
    const specialText = 'Label with @#$%^&*() characters';
    render(<LabelTitleComponent text={specialText} />);
    
    const labelElement = screen.getByText(specialText);
    expect(labelElement).toBeInTheDocument();
  });

  it('renders label with HTML entities', () => {
    const htmlText = 'Label with &amp; &lt; &gt; entities';
    render(<LabelTitleComponent text={htmlText} />);
    
    const labelElement = screen.getByText(htmlText);
    expect(labelElement).toBeInTheDocument();
  });

  it('renders label with numbers', () => {
    const numericText = 'Label 123 with 456 numbers';
    render(<LabelTitleComponent text={numericText} />);
    
    const labelElement = screen.getByText(numericText);
    expect(labelElement).toBeInTheDocument();
  });

  it('renders label with unicode characters', () => {
    const unicodeText = 'Label with 中文, 日本語, 한국어, และภาษาไทย';
    render(<LabelTitleComponent text={unicodeText} />);
    
    const labelElement = screen.getByText(unicodeText);
    expect(labelElement).toBeInTheDocument();
  });

  it('renders label with long text', () => {
    const longText = 'This is a very long label text that might wrap to multiple lines and should still be displayed correctly with proper styling';
    render(<LabelTitleComponent text={longText} />);
    
    const labelElement = screen.getByText(longText);
    expect(labelElement).toBeInTheDocument();
  });

  it('renders label with short text', () => {
    const shortText = 'Hi';
    render(<LabelTitleComponent text={shortText} />);
    
    const labelElement = screen.getByText(shortText);
    expect(labelElement).toBeInTheDocument();
  });

  it('renders label with whitespace', () => {
    const whitespaceText = '  Label with whitespace  ';
    render(<LabelTitleComponent text={whitespaceText} />);
  
    const labelElement = screen.getByText('Label with whitespace');
    expect(labelElement.tagName).toBe('LABEL');
  });
  

  it('applies capitalize class correctly', () => {
    render(<LabelTitleComponent text="test label" />);
    
    const labelElement = screen.getByText('test label');
    expect(labelElement).toHaveClass('capitalize');
  });

  it('renders with multiple custom classes', () => {
    const customClasses = 'custom-class another-class third-class';
    render(<LabelTitleComponent {...defaultProps} className={customClasses} />);
    
    const labelElement = screen.getByText('Test Label');
    expect(labelElement).toHaveClass(
      'custom-class',
      'another-class',
      'third-class',
      'block',
      'font-semibold',
      'text-[var(--titleComponent-label-color)]',
      'mb-[4px]',
      'capitalize'
    );
  });

  it('renders with empty className', () => {
    render(<LabelTitleComponent {...defaultProps} className="" />);
    
    const labelElement = screen.getByText('Test Label');
    expect(labelElement).toHaveClass(
      'block',
      'font-semibold',
      'text-[var(--titleComponent-label-color)]',
      'mb-[4px]',
      'capitalize'
    );
  });

  it('renders with undefined className', () => {
    render(<LabelTitleComponent {...defaultProps} className={undefined} />);
    
    const labelElement = screen.getByText('Test Label');
    expect(labelElement).toHaveClass(
      'block',
      'font-semibold',
      'text-[var(--titleComponent-label-color)]',
      'mb-[4px]',
      'capitalize'
    );
  });

  it('renders label with emoji', () => {
    const emojiText = 'Label with 🎉 emoji 🚀';
    render(<LabelTitleComponent text={emojiText} />);
    
    const labelElement = screen.getByText(emojiText);
    expect(labelElement).toBeInTheDocument();
  });

  it('renders label with mixed content', () => {
    const mixedText = 'Label 123 with @#$%^&*() and 🎉 emoji and 中文 text';
    render(<LabelTitleComponent text={mixedText} />);
    
    const labelElement = screen.getByText(mixedText);
    expect(labelElement).toBeInTheDocument();
  });

  it('renders required asterisk with correct styling', () => {
    render(<LabelTitleComponent {...defaultProps} required={true} />);
    
    const asteriskElement = screen.getByText('*');
    expect(asteriskElement).toHaveClass('text-[#FF0000]', 'dark:text-[#FF4F50]', 'ml-[4px]');
  });

  it('renders label and asterisk in correct order', () => {
    render(<LabelTitleComponent text="Test Label" required={true} />);
    
    const labelElement = screen.getByText((content, element) =>
      element?.tagName.toLowerCase() === 'label' &&
      content.includes('Test Label')
    );
  
    expect(labelElement).toHaveTextContent('Test Label*');
  });
  

  it('handles required prop with custom className', () => {
    const customClass = 'custom-label-class';
    render(<LabelTitleComponent {...defaultProps} required={true} className={customClass} />);
    
    const labelElement = screen.getByText('Test Label');
    const asteriskElement = screen.getByText('*');
    
    expect(labelElement).toHaveClass(customClass);
    expect(asteriskElement).toBeInTheDocument();
  });

  it('renders label with CSS custom property for color', () => {
    render(<LabelTitleComponent {...defaultProps} />);
    
    const labelElement = screen.getByText('Test Label');
    expect(labelElement).toHaveClass('text-[var(--titleComponent-label-color)]');
  });

  it('renders label with proper margin bottom', () => {
    render(<LabelTitleComponent {...defaultProps} />);
    
    const labelElement = screen.getByText('Test Label');
    expect(labelElement).toHaveClass('mb-[4px]');
  });

  it('renders label with proper font weight', () => {
    render(<LabelTitleComponent {...defaultProps} />);
    
    const labelElement = screen.getByText('Test Label');
    expect(labelElement).toHaveClass('font-semibold');
  });

  it('renders label with block display', () => {
    render(<LabelTitleComponent {...defaultProps} />);
    
    const labelElement = screen.getByText('Test Label');
    expect(labelElement).toHaveClass('block');
  });
}); 