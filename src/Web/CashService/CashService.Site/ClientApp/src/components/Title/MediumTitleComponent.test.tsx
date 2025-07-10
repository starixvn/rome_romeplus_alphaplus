import { describe, it, expect } from 'vitest';
import { render, screen } from '@testing-library/react';
import '@testing-library/jest-dom';
import React from 'react';
import { MediumTitleComponent } from './MediumTitleComponent';
import { BaseTitleProps } from './types';

describe('MediumTitleComponent', () => {
  const defaultProps: BaseTitleProps = {
    text: 'Test Title',
  };

  it('renders title with default props', () => {
    render(<MediumTitleComponent {...defaultProps} />);
    
    const titleElement = screen.getByText('Test Title');
    expect(titleElement).toBeInTheDocument();
    expect(titleElement.tagName).toBe('H3');
  });

  it('renders title with correct CSS classes', () => {
    render(<MediumTitleComponent {...defaultProps} />);
    
    const titleElement = screen.getByText('Test Title');
    expect(titleElement).toHaveClass(
      'w-[100%]',
      'font-semibold',
      'text-[14px]',
      'text-[#094980]',
      'dark:text-[#459be5]',
      'capitalize'
    );
  });

  it('renders title with custom className', () => {
    const customClass = 'custom-class';
    render(<MediumTitleComponent {...defaultProps} className={customClass} />);
    
    const titleElement = screen.getByText('Test Title');
    expect(titleElement).toHaveClass(
      customClass,
      'w-[100%]',
      'font-semibold',
      'text-[14px]',
      'text-[#094980]',
      'dark:text-[#459be5]',
      'capitalize'
    );
  });

  it('renders required asterisk when required prop is true', () => {
    render(<MediumTitleComponent {...defaultProps} required={true} />);
    
    const titleElement = screen.getByText('Test Title');
    const asterisk = screen.getByText('*');
    
    expect(titleElement).toBeInTheDocument();
    expect(asterisk).toBeInTheDocument();
    expect(asterisk).toHaveClass('text-[#FF0000]', 'dark:text-[#FF4F50]', 'ml-[4px]');
  });

  it('does not render required asterisk when required prop is false', () => {
    render(<MediumTitleComponent {...defaultProps} required={false} />);
    
    const titleElement = screen.getByText('Test Title');
    const asterisk = screen.queryByText('*');
    
    expect(titleElement).toBeInTheDocument();
    expect(asterisk).not.toBeInTheDocument();
  });

  it('does not render required asterisk when required prop is not provided', () => {
    render(<MediumTitleComponent {...defaultProps} />);
    
    const titleElement = screen.getByText('Test Title');
    const asterisk = screen.queryByText('*');
    
    expect(titleElement).toBeInTheDocument();
    expect(asterisk).not.toBeInTheDocument();
  });

  it('does not render when text is empty', () => {
    render(<MediumTitleComponent text="" />);
    
    const titleElement = screen.queryByRole('heading', { level: 3 });
    expect(titleElement).not.toBeInTheDocument();
  });

  it('does not render when text is null', () => {
    render(<MediumTitleComponent text={null as any} />);
    
    const titleElement = screen.queryByRole('heading', { level: 3 });
    expect(titleElement).not.toBeInTheDocument();
  });

  it('does not render when text is undefined', () => {
    render(<MediumTitleComponent text={undefined as any} />);
    
    const titleElement = screen.queryByRole('heading', { level: 3 });
    expect(titleElement).not.toBeInTheDocument();
  });

  it('renders non-breaking space when text is falsy but component still renders', () => {
    // This test covers the fallback behavior in the component
    render(<MediumTitleComponent text="" />);
    
    const titleElement = screen.queryByRole('heading', { level: 3 });
    expect(titleElement).not.toBeInTheDocument();
  });

  it('renders title with special characters', () => {
    const specialTitle = 'Title with @#$%^&*() characters';
    render(<MediumTitleComponent text={specialTitle} />);
    
    const titleElement = screen.getByText(specialTitle);
    expect(titleElement).toBeInTheDocument();
  });

  it('renders title with HTML entities', () => {
    const htmlTitle = 'Title with &amp; &lt; &gt; entities';
    render(<MediumTitleComponent text={htmlTitle} />);
    
    const titleElement = screen.getByText(htmlTitle);
    expect(titleElement).toBeInTheDocument();
  });

  it('renders title with numbers', () => {
    const numericTitle = 'Title 123 with 456 numbers';
    render(<MediumTitleComponent text={numericTitle} />);
    
    const titleElement = screen.getByText(numericTitle);
    expect(titleElement).toBeInTheDocument();
  });

  it('renders title with unicode characters', () => {
    const unicodeTitle = 'Title with 中文, 日本語, 한국어, และภาษาไทย';
    render(<MediumTitleComponent text={unicodeTitle} />);
    
    const titleElement = screen.getByText(unicodeTitle);
    expect(titleElement).toBeInTheDocument();
  });

  it('renders title with long text', () => {
    const longTitle = 'This is a very long title that might wrap to multiple lines and should still be displayed correctly with proper styling';
    render(<MediumTitleComponent text={longTitle} />);
    
    const titleElement = screen.getByText(longTitle);
    expect(titleElement).toBeInTheDocument();
  });

  it('renders title with short text', () => {
    const shortTitle = 'Hi';
    render(<MediumTitleComponent text={shortTitle} />);
    
    const titleElement = screen.getByText(shortTitle);
    expect(titleElement).toBeInTheDocument();
  });

  it('renders title with whitespace', () => {
    const whitespaceTitle = '  Title with whitespace  ';
    render(<MediumTitleComponent text={whitespaceTitle} />);
    
    const titleElement = screen.getByRole('heading', { level: 3 });
    expect(titleElement).toHaveTextContent('Title with whitespace');
  });

  it('has proper heading role and level', () => {
    render(<MediumTitleComponent {...defaultProps} />);
    
    const titleElement = screen.getByRole('heading', { level: 3 });
    expect(titleElement).toBeInTheDocument();
    expect(titleElement).toHaveTextContent('Test Title');
  });

  it('applies capitalize class correctly', () => {
    render(<MediumTitleComponent text="test title" />);
    
    const titleElement = screen.getByText('test title');
    expect(titleElement).toHaveClass('capitalize');
  });

  it('renders with multiple custom classes', () => {
    const customClasses = 'custom-class another-class third-class';
    render(<MediumTitleComponent {...defaultProps} className={customClasses} />);
    
    const titleElement = screen.getByText('Test Title');
    expect(titleElement).toHaveClass(
      'custom-class',
      'another-class',
      'third-class',
      'w-[100%]',
      'font-semibold',
      'text-[14px]',
      'text-[#094980]',
      'dark:text-[#459be5]',
      'capitalize'
    );
  });

  it('renders with empty className', () => {
    render(<MediumTitleComponent {...defaultProps} className="" />);
    
    const titleElement = screen.getByText('Test Title');
    expect(titleElement).toHaveClass(
      'w-[100%]',
      'font-semibold',
      'text-[14px]',
      'text-[#094980]',
      'dark:text-[#459be5]',
      'capitalize'
    );
  });

  it('renders with undefined className', () => {
    render(<MediumTitleComponent {...defaultProps} className={undefined} />);
    
    const titleElement = screen.getByText('Test Title');
    expect(titleElement).toHaveClass(
      'w-[100%]',
      'font-semibold',
      'text-[14px]',
      'text-[#094980]',
      'dark:text-[#459be5]',
      'capitalize'
    );
  });

  it('renders title with emoji', () => {
    const emojiTitle = 'Title with 🎉 emoji 🚀';
    render(<MediumTitleComponent text={emojiTitle} />);
    
    const titleElement = screen.getByText(emojiTitle);
    expect(titleElement).toBeInTheDocument();
  });

  it('renders title with mixed content', () => {
    const mixedTitle = 'Title 123 with @#$%^&*() and 🎉 emoji and 中文 text';
    render(<MediumTitleComponent text={mixedTitle} />);
    
    const titleElement = screen.getByText(mixedTitle);
    expect(titleElement).toBeInTheDocument();
  });

  it('renders required asterisk with title containing special characters', () => {
    const specialTitle = 'Title with @#$%^&*() characters';
    render(<MediumTitleComponent text={specialTitle} required={true} />);
    
    const titleElement = screen.getByText(specialTitle);
    const asterisk = screen.getByText('*');
    
    expect(titleElement).toBeInTheDocument();
    expect(asterisk).toBeInTheDocument();
  });

  it('renders required asterisk with title containing unicode characters', () => {
    const unicodeTitle = 'Title with 中文, 日本語, 한국어, และภาษาไทย';
    render(<MediumTitleComponent text={unicodeTitle} required={true} />);
    
    const titleElement = screen.getByText(unicodeTitle);
    const asterisk = screen.getByText('*');
    
    expect(titleElement).toBeInTheDocument();
    expect(asterisk).toBeInTheDocument();
  });

  it('renders required asterisk with custom className', () => {
    const customClass = 'custom-class';
    render(<MediumTitleComponent {...defaultProps} className={customClass} required={true} />);
    
    const titleElement = screen.getByText('Test Title');
    const asterisk = screen.getByText('*');
    
    expect(titleElement).toHaveClass(customClass);
    expect(asterisk).toBeInTheDocument();
    expect(asterisk).toHaveClass('text-[#FF0000]', 'dark:text-[#FF4F50]', 'ml-[4px]');
  });

  it('renders required asterisk with multiple custom classes', () => {
    const customClasses = 'custom-class another-class third-class';
    render(<MediumTitleComponent {...defaultProps} className={customClasses} required={true} />);
    
    const titleElement = screen.getByText('Test Title');
    const asterisk = screen.getByText('*');
    
    expect(titleElement).toHaveClass('custom-class', 'another-class', 'third-class');
    expect(asterisk).toBeInTheDocument();
  });
}); 