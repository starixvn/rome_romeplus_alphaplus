import React from 'react';
import { describe, it, expect, vi } from 'vitest';
import { render, screen } from '@testing-library/react';
import '@testing-library/jest-dom';
import { TitleComponent } from './TitleComponent';

describe('TitleComponent', () => {
  it('renders with default main type when no type is provided', () => {
    render(<TitleComponent text="Test Title" />);
    
    const titleElement = screen.getByText('Test Title');
    expect(titleElement).toBeInTheDocument();
    expect(titleElement.tagName).toBe('H2');
  });

  it('renders with custom text when text prop is provided', () => {
    const customText = 'Custom Title';
    render(<TitleComponent text={customText} />);
    
    const titleElement = screen.getByText(customText);
    expect(titleElement).toBeInTheDocument();
  });

  it('renders main title correctly', () => {
    render(<TitleComponent type="main" text="Main Title" />);
    
    const titleElement = screen.getByText('Main Title');
    expect(titleElement.tagName).toBe('H2');
    expect(titleElement).toHaveClass('mb-[12px]', 'flex', 'text-[14px]', 'font-semibold');
  });

  it('renders medium title correctly', () => {
    render(<TitleComponent type="medium" text="Medium Title" />);
    
    const titleElement = screen.getByText('Medium Title');
    expect(titleElement.tagName).toBe('H3');
    expect(titleElement).toHaveClass('w-[100%]', 'font-semibold', 'text-[14px]');
  });

  it('renders normal title correctly', () => {
    render(<TitleComponent type="normal" text="Normal Title" />);
    
    const titleElement = screen.getByText('Normal Title');
    expect(titleElement.tagName).toBe('H3');
    expect(titleElement).toHaveClass('py-[8px]', 'font-semibold', 'capitalize');
  });

  it('renders label title correctly', () => {
    render(<TitleComponent type="label" text="Label Title" />);
    
    const titleElement = screen.getByText('Label Title');
    expect(titleElement.tagName).toBe('LABEL');
    expect(titleElement).toHaveClass('block', 'font-semibold', 'mb-[4px]', 'capitalize');
  });

  it('applies custom className correctly', () => {
    const customClass = 'custom-class';
    render(<TitleComponent text="Test Title" className={customClass} />);
    
    const titleElement = screen.getByText('Test Title');
    expect(titleElement).toHaveClass(customClass);
  });

  it('renders required indicator when required is true', () => {
    render(<TitleComponent text="Required Title" required={true} />);
    
    const titleElement = screen.getByText('Required Title');
    const requiredSpan = titleElement.nextElementSibling;
    
    expect(requiredSpan).toHaveTextContent('*');
    expect(requiredSpan).toHaveClass('text-[#FF0000]', 'dark:text-[#FF4F50]', 'ml-[4px]');
  });

  it('does not render required indicator when required is false', () => {
    render(<TitleComponent text="Not Required Title" required={false} />);
    
    const titleElement = screen.getByText('Not Required Title');
    const nextElement = titleElement.nextElementSibling;
    
    expect(nextElement).not.toHaveTextContent('*');
  });

  it('renders tippy when showTippy is true and children is provided', () => {
    render(
      <TitleComponent 
        text="Title with Tippy" 
        showTippy={true} 
        children="Tooltip content"
      />
    );
    
    const titleElement = screen.getByText('Title with Tippy');
    const iconElement = titleElement.querySelector('svg');
    
    expect(iconElement).toBeInTheDocument();
    expect(iconElement).toHaveClass('w-[18px]', 'cursor-pointer', 'h-[18px]', 'ml-[10px]');
  });

  it('does not render tippy when showTippy is false', () => {
    render(
      <TitleComponent 
        text="Title without Tippy" 
        showTippy={false} 
        children="Tooltip content"
      />
    );
    
    const titleElement = screen.getByText('Title without Tippy');
    const iconElement = titleElement.querySelector('svg');
    
    expect(iconElement).not.toBeInTheDocument();
  });

  it('renders icon when isShowIcon is true', () => {
    render(<TitleComponent text="Title with Icon" isShowIcon={true} />);
    
    const titleElement = screen.getByText('Title with Icon');
    const iconElement = titleElement.querySelector('svg');
    
    expect(iconElement).toBeInTheDocument();
    expect(iconElement).toHaveClass('w-[18px]', 'h-[18px]', 'ml-[10px]');
  });

  it('does not render icon when isShowIcon is false', () => {
    render(<TitleComponent text="Title without Icon" isShowIcon={false} />);
    
    const titleElement = screen.getByText('Title without Icon');
    const iconElement = titleElement.querySelector('svg');
    
    expect(iconElement).not.toBeInTheDocument();
  });

  it('handles invalid title type gracefully with warning', () => {
    const consoleSpy = vi.spyOn(console, 'warn').mockImplementation(() => {});
    
    render(<TitleComponent type={'invalid' as any} text="Test Title" />);
    
    expect(consoleSpy).toHaveBeenCalledWith('Invalid title type: invalid. Falling back to \'main\'');
    
    const titleElement = screen.getByText('Test Title');
    expect(titleElement.tagName).toBe('H2'); // Falls back to main
    
    consoleSpy.mockRestore();
  });

  it('handles empty text gracefully', () => {
    const consoleSpy = vi.spyOn(console, 'warn').mockImplementation(() => {});
    
    const { container } = render(<TitleComponent text="" />);
    
    expect(consoleSpy).toHaveBeenCalledWith('TitleComponent: Empty text provided');
    expect(container.firstChild).toBeNull(); // Component returns null
    
    consoleSpy.mockRestore();
  });

  it('handles null text gracefully', () => {
    const consoleSpy = vi.spyOn(console, 'warn').mockImplementation(() => {});
    
    const { container } = render(<TitleComponent text={null as any} />);
    
    expect(consoleSpy).toHaveBeenCalledWith('TitleComponent: Empty text provided');
    expect(container.firstChild).toBeNull(); // Component returns null
    
    consoleSpy.mockRestore();
  });

  it('handles special characters in text prop', () => {
    const specialText = 'Title with special chars: !@#$%^&*()';
    render(<TitleComponent text={specialText} />);
    
    const titleElement = screen.getByText(specialText);
    expect(titleElement).toBeInTheDocument();
  });

  it('handles unicode characters in text prop', () => {
    const unicodeText = 'Title with unicode: 中文 Español Français';
    render(<TitleComponent text={unicodeText} />);
    
    const titleElement = screen.getByText(unicodeText);
    expect(titleElement).toBeInTheDocument();
  });

  it('combines multiple features correctly', () => {
    render(
      <TitleComponent 
        type="medium"
        text="Complex Title" 
        required={true}
        isShowIcon={true}
        className="custom-class"
      />
    );
    
    const titleElement = screen.getByText('Complex Title');
    expect(titleElement.tagName).toBe('H3');
    expect(titleElement).toHaveClass('custom-class');
    
    const requiredSpan = titleElement.nextElementSibling;
    expect(requiredSpan).toHaveTextContent('*');
    
    const iconElement = titleElement.querySelector('svg');
    expect(iconElement).toBeInTheDocument();
  });
}); 