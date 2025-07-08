import { describe, it, expect, vi } from 'vitest';
import { render, screen } from '@testing-library/react';
import '@testing-library/jest-dom';
import React from 'react';
import TitleComponent from './TitleComponent';
import { TitleComponentProps } from './types';

// Mock Tippy component
vi.mock('@tippyjs/react', () => ({
  default: ({ children, content }: { children: React.ReactNode; content: React.ReactNode }) => (
    <div data-testid="tippy-tooltip" data-content={typeof content === 'string' ? content : 'tooltip-content'}>
      {children}
    </div>
  ),
}));

// Mock IconInfo component
vi.mock('../Icons', () => ({
  IconInfo: ({ className }: { className: string }) => (
    <svg data-testid="icon-info" className={className} />
  ),
}));

describe('TitleComponent', () => {
  const defaultProps: TitleComponentProps = {
    title: 'Test Title',
  };

  it('renders title with default props', () => {
    render(<TitleComponent {...defaultProps} />);
    
    const titleElement = screen.getByText('Test Title');
    expect(titleElement).toBeInTheDocument();
    expect(titleElement.tagName).toBe('H2');
  });

  it('renders title with correct CSS classes', () => {
    render(<TitleComponent {...defaultProps} />);
    
    const titleElement = screen.getByText('Test Title');
    expect(titleElement).toHaveClass(
      'mb-[12px]',
      'flex',
      'text-[14px]',
      'font-semibold',
      'items-center',
      'uppercase',
      'text-[var(--titleComponent-color)]',
      'border-b',
      'border-[var(--titleComponent-border-color)]',
      'leading-[39px]'
    );
  });

  it('renders title without tooltip when showTippy is false', () => {
    render(<TitleComponent {...defaultProps} showTippy={false} />);
    
    const titleElement = screen.getByText('Test Title');
    expect(titleElement).toBeInTheDocument();
    
    const tooltip = screen.queryByTestId('tippy-tooltip');
    expect(tooltip).not.toBeInTheDocument();
  });

  it('renders title without tooltip when showTippy is undefined', () => {
    render(<TitleComponent {...defaultProps} />);
    
    const titleElement = screen.getByText('Test Title');
    expect(titleElement).toBeInTheDocument();
    
    const tooltip = screen.queryByTestId('tippy-tooltip');
    expect(tooltip).not.toBeInTheDocument();
  });

  it('renders title with tooltip when showTippy is true', () => {
    const tooltipContent = 'This is a helpful tooltip';
    render(
      <TitleComponent {...defaultProps} showTippy={true}>
        {tooltipContent}
      </TitleComponent>
    );
    
    const titleElement = screen.getByText('Test Title');
    expect(titleElement).toBeInTheDocument();
    
    const tooltip = screen.getByTestId('tippy-tooltip');
    expect(tooltip).toBeInTheDocument();
    expect(tooltip).toHaveAttribute('data-content', tooltipContent);
  });

  it('renders info icon when showTippy is true', () => {
    render(
      <TitleComponent {...defaultProps} showTippy={true}>
        Tooltip content
      </TitleComponent>
    );
    
    const infoIcon = screen.getByTestId('icon-info');
    expect(infoIcon).toBeInTheDocument();
    expect(infoIcon).toHaveClass('w-[18px]', 'cursor-pointer', 'h-[18px]', 'ml-[10px]', 'fill-[#118AF2]');
  });

  it('does not render info icon when showTippy is false', () => {
    render(<TitleComponent {...defaultProps} showTippy={false} />);
    
    const infoIcon = screen.queryByTestId('icon-info');
    expect(infoIcon).not.toBeInTheDocument();
  });

  it('renders empty title', () => {
    render(<TitleComponent title="" />);
    
    const titleElement = screen.getByRole('heading', { level: 2 });
    expect(titleElement).toBeInTheDocument();

    expect(titleElement).toHaveTextContent('');
  });

  it('renders title with special characters', () => {
    const specialTitle = 'Title with @#$%^&*() characters';
    render(<TitleComponent title={specialTitle} />);
    
    const titleElement = screen.getByText(specialTitle);
    expect(titleElement).toBeInTheDocument();
  });

  it('renders title with HTML entities', () => {
    const htmlTitle = 'Title with &amp; &lt; &gt; entities';
    render(<TitleComponent title={htmlTitle} />);
    
    const titleElement = screen.getByText(htmlTitle);
    expect(titleElement).toBeInTheDocument();
  });

  it('renders tooltip with complex content', () => {
    const complexContent = (
      <div>
        <h3>Complex Tooltip</h3>
        <p>This is a complex tooltip with multiple elements</p>
        <ul>
          <li>Item 1</li>
          <li>Item 2</li>
        </ul>
      </div>
    );

    render(
      <TitleComponent {...defaultProps} showTippy={true}>
        {complexContent}
      </TitleComponent>
    );
    
    const tooltip = screen.getByTestId('tippy-tooltip');
    expect(tooltip).toBeInTheDocument();
    expect(tooltip).toHaveAttribute('data-content', 'tooltip-content');
  });

  it('renders tooltip with string content', () => {
    const stringContent = 'Simple string tooltip';
    render(
      <TitleComponent {...defaultProps} showTippy={true}>
        {stringContent}
      </TitleComponent>
    );
    
    const tooltip = screen.getByTestId('tippy-tooltip');
    expect(tooltip).toBeInTheDocument();
    expect(tooltip).toHaveAttribute('data-content', stringContent);
  });

  it('renders tooltip with number content', () => {
    const numberContent = 123;
    render(
      <TitleComponent {...defaultProps} showTippy={true}>
        {numberContent}
      </TitleComponent>
    );
    
    const tooltip = screen.getByTestId('tippy-tooltip');
    expect(tooltip).toBeInTheDocument();
    expect(tooltip).toHaveAttribute('data-content', 'tooltip-content');
  });

  it('has proper heading role and level', () => {
    render(<TitleComponent {...defaultProps} />);
    
    const titleElement = screen.getByRole('heading', { level: 2 });
    expect(titleElement).toBeInTheDocument();
    expect(titleElement).toHaveTextContent('Test Title');
  });

  it('renders with long title text', () => {
    const longTitle = 'This is a very long title that might wrap to multiple lines and should still be displayed correctly';
    render(<TitleComponent title={longTitle} />);
    
    const titleElement = screen.getByText(longTitle);
    expect(titleElement).toBeInTheDocument();
  });

  it('renders with short title text', () => {
    const shortTitle = 'Hi';
    render(<TitleComponent title={shortTitle} />);
    
    const titleElement = screen.getByText(shortTitle);
    expect(titleElement).toBeInTheDocument();
  });

  it('renders tooltip with empty content', () => {
    render(
      <TitleComponent {...defaultProps} showTippy={true}>
        {''}
      </TitleComponent>
    );
    
    const tooltip = screen.getByTestId('tippy-tooltip');
    expect(tooltip).toBeInTheDocument();
    expect(tooltip).toHaveAttribute('data-content', '');
  });

  it('renders tooltip with null content', () => {
    render(
      <TitleComponent {...defaultProps} showTippy={true}>
        {null}
      </TitleComponent>
    );
    
    const tooltip = screen.getByTestId('tippy-tooltip');
    expect(tooltip).toBeInTheDocument();
    expect(tooltip).toHaveAttribute('data-content', 'tooltip-content');
  });

  it('renders tooltip with undefined content', () => {
    render(
      <TitleComponent {...defaultProps} showTippy={true}>
        {undefined}
      </TitleComponent>
    );
    
    const tooltip = screen.getByTestId('tippy-tooltip');
    expect(tooltip).toBeInTheDocument();
    expect(tooltip).toHaveAttribute('data-content', 'tooltip-content');
  });
}); 