import React from 'react';
import { describe, it, expect } from 'vitest';
import { render, screen } from '@testing-library/react';
import { SuccessMessageComponent } from './SuccessMessageComponent';

describe('SuccessMessageComponent', () => {
  it('should render success message when message is provided', () => {
    const message = 'Operation completed successfully!';
    render(<SuccessMessageComponent message={message} />);
    
    expect(screen.getByText('Operation completed successfully!')).toBeInTheDocument();
    expect(screen.getByRole('alert')).toBeInTheDocument();
  });

  it('should not render anything when message is null', () => {
    const { container } = render(<SuccessMessageComponent message={null} />);
    
    expect(container.firstChild).toBeNull();
  });

  it('should not render anything when message is empty string', () => {
    const { container } = render(<SuccessMessageComponent message="" />);
    
    expect(container.firstChild).toBeNull();
  });

  it('should render with custom className', () => {
    const message = 'Success message';
    const customClass = 'custom-success-class';
    
    render(<SuccessMessageComponent message={message} className={customClass} />);
    
    const alertElement = screen.getByRole('alert');
    expect(alertElement).toHaveClass(customClass);
    expect(alertElement).toHaveClass('max-[768px]:py-[12px]');
  });

  it('should render with default className when no className is provided', () => {
    const message = 'Success message';
    
    render(<SuccessMessageComponent message={message} />);
    
    const alertElement = screen.getByRole('alert');
    expect(alertElement).toHaveClass('max-[768px]:py-[12px]');
  });

  it('should render HTML content safely using dangerouslySetInnerHTML', () => {
    const message = 'Success with <strong>bold text</strong> and <em>italic text</em>';
    
    render(<SuccessMessageComponent message={message} />);
    
    const alertElement = screen.getByRole('alert');
    expect(alertElement.innerHTML).toContain('<strong>bold text</strong>');
    expect(alertElement.innerHTML).toContain('<em>italic text</em>');
  });

  it('should render check icon with correct styling', () => {
    const message = 'Success message';
    
    render(<SuccessMessageComponent message={message} />);
    
    const iconElement = screen.getByRole('alert').querySelector('svg');
    expect(iconElement).toBeInTheDocument();
    expect(iconElement).toHaveClass('w-[20px]', 'h-[20px]', 'fill-[#268026]', 'dark:fill-[#7ECC4D]');
  });

  it('should have correct container styling', () => {
    const message = 'Success message';
    
    render(<SuccessMessageComponent message={message} />);
    
    const containerElement = screen.getByRole('alert').querySelector('div > div');
    expect(containerElement).toHaveClass(
      'bg-[#E2FFF2]',
      'dark:bg-[#3A5031]',
      'border-[#5CB774]',
      'dark:border-[#7ECC4D]',
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

  it('should have correct icon positioning', () => {
    const message = 'Success message';
    
    render(<SuccessMessageComponent message={message} />);
    
    const iconElement = screen.getByRole('alert').querySelector('svg');
    expect(iconElement).toHaveClass('absolute', 'top-[7px]', 'left-[10px]');
  });

  it('should handle complex HTML content', () => {
    const message = `
      <div>
        <h3>Success!</h3>
        <p>Your <a href="#">profile</a> has been updated.</p>
        <ul>
          <li>Item 1</li>
          <li>Item 2</li>
        </ul>
      </div>
    `;
    
    render(<SuccessMessageComponent message={message} />);
    
    const alertElement = screen.getByRole('alert');
    expect(alertElement.innerHTML).toContain('<h3>Success!</h3>');
    expect(alertElement.innerHTML).toContain('<a href="#">profile</a>');
    expect(alertElement.innerHTML).toContain('<ul>');
    expect(alertElement.innerHTML).toContain('<li>Item 1</li>');
  });

  it('should handle special characters in message', () => {
    const message = 'Success with special chars: &lt;script&gt;alert("test")&lt;/script&gt;';
    
    render(<SuccessMessageComponent message={message} />);
    
    expect(screen.getByText(/Success with special chars:/)).toBeInTheDocument();
  });

  it('should have proper accessibility attributes', () => {
    const message = 'Success message';
    
    render(<SuccessMessageComponent message={message} />);
    
    const alertElement = screen.getByRole('alert');
    expect(alertElement).toBeInTheDocument();
  });
}); 