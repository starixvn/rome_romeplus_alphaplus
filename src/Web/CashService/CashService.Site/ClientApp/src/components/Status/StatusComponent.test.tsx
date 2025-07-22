import React from 'react';
import { describe, it, expect, vi } from 'vitest';
import { render, screen } from '@testing-library/react';
import '@testing-library/jest-dom';
import { StatusComponent } from './StatusComponent';

// Mock react-i18next
vi.mock('react-i18next', () => ({
  useTranslation: () => ({
    t: (key: string) => {
      const translations = {
        'completed': 'Completed',
        'disabled': 'Disabled',
        'processing': 'Processing',
        'maintenance': 'Maintenance',
        'failed': 'Failed'
      };
      return translations[key] || key;
    }
  })
}));

describe('StatusComponent', () => {
  it('renders with default completed type when no type is provided', () => {
    render(<StatusComponent />);
    
    const statusElement = screen.getByRole('alert');
    expect(statusElement).toBeInTheDocument();
    
    const spanElement = statusElement.querySelector('span');
    expect(spanElement).toHaveTextContent('Completed');
  });

  it('renders with custom text when text prop is provided', () => {
    const customText = 'Custom Status';
    render(<StatusComponent text={customText} />);
    
    const statusElement = screen.getByRole('alert');
    expect(statusElement).toBeInTheDocument();
    
    const spanElement = statusElement.querySelector('span');
    expect(spanElement).toHaveTextContent(customText);
  });

  it('renders completed status correctly', () => {
    render(<StatusComponent type="completed" />);
    
    const spanElement = screen.getByRole('alert').querySelector('span');
    expect(spanElement).toHaveTextContent('Completed');
    expect(spanElement).toHaveClass('bg-[#D4E7D9]', 'dark:bg-[#24853e]');
  });

  it('renders failed status correctly', () => {
    render(<StatusComponent type="failed" />);
    
    const spanElement = screen.getByRole('alert').querySelector('span');
    expect(spanElement).toHaveTextContent('Failed');
    expect(spanElement).toHaveClass('bg-[#E0E0E0]', 'dark:bg-[#6f6f6f]');
  });

  it('renders disabled status correctly', () => {
    render(<StatusComponent type="disabled" />);
    
    const spanElement = screen.getByRole('alert').querySelector('span');
    expect(spanElement).toHaveTextContent('Disabled');
    expect(spanElement).toHaveClass('bg-[#FBD8D8]', 'dark:bg-[#af4646]');
  });

  it('renders processing status correctly', () => {
    render(<StatusComponent type="processing" />);
    
    const spanElement = screen.getByRole('alert').querySelector('span');
    expect(spanElement).toHaveTextContent('Processing');
    expect(spanElement).toHaveClass('bg-[#90d3ff]', 'dark:bg-[#2a668d]');
  });

  it('renders maintenance status correctly', () => {
    render(<StatusComponent type="maintenance" />);
    
    const spanElement = screen.getByRole('alert').querySelector('span');
    expect(spanElement).toHaveTextContent('Maintenance');
    expect(spanElement).toHaveClass('bg-[#fcae7b]', 'dark:bg-[#955e39]');
  });

  it('applies custom className correctly', () => {
    const customClass = 'custom-class';
    render(<StatusComponent className={customClass} />);
    
    const divElement = screen.getByRole('alert');
    expect(divElement).toHaveClass(customClass);
  });

  it('has correct accessibility attributes', () => {
    render(<StatusComponent />);
    
    const alertElement = screen.getByRole('alert');
    expect(alertElement).toBeInTheDocument();
  });

  it('handles invalid status type gracefully with warning', () => {
    const consoleSpy = vi.spyOn(console, 'warn').mockImplementation(() => {});
    
    render(<StatusComponent type={'invalid' as any} />);
    
    expect(consoleSpy).toHaveBeenCalledWith('Invalid status type: invalid. Falling back to \'completed\'');
    
    const spanElement = screen.getByRole('alert').querySelector('span');
    expect(spanElement).toHaveTextContent('Completed'); // Falls back to completed
    
    consoleSpy.mockRestore();
  });

  it('handles empty text gracefully', () => {
    const consoleSpy = vi.spyOn(console, 'warn').mockImplementation(() => {});
    
    const { container } = render(<StatusComponent text="" />);
    
    expect(consoleSpy).toHaveBeenCalledWith('StatusComponent: Empty text provided, using default text');
    expect(container.firstChild).toBeNull(); // Component returns null
    
    consoleSpy.mockRestore();
  });

  it('handles null text gracefully', () => {
    const consoleSpy = vi.spyOn(console, 'warn').mockImplementation(() => {});
    
    const { container } = render(<StatusComponent text={null as any} />);
    
    expect(consoleSpy).toHaveBeenCalledWith('StatusComponent: Empty text provided, using default text');
    expect(container.firstChild).toBeNull(); // Component returns null
    
    consoleSpy.mockRestore();
  });

  it('handles missing translation keys gracefully', () => {
    // Mock translation to return the key itself (simulating missing translation)
    vi.mocked(require('react-i18next').useTranslation)().t = (key: string) => key;
    
    render(<StatusComponent type="completed" />);
    
    const spanElement = screen.getByRole('alert').querySelector('span');
    expect(spanElement).toHaveTextContent('completed'); // Falls back to key
    
    vi.restoreAllMocks();
  });

  it('handles translation errors gracefully', () => {
    const consoleSpy = vi.spyOn(console, 'warn').mockImplementation(() => {});
    
    // Mock translation to throw error
    vi.mocked(require('react-i18next').useTranslation)().t = () => {
      throw new Error('Translation error');
    };
    
    render(<StatusComponent type="completed" />);
    
    expect(consoleSpy).toHaveBeenCalledWith('Translation error for key \'completed\':', expect.any(Error));
    
    const spanElement = screen.getByRole('alert').querySelector('span');
    expect(spanElement).toHaveTextContent('completed'); // Falls back to key
    
    consoleSpy.mockRestore();
    vi.restoreAllMocks();
  });

  it('handles special characters in text prop', () => {
    const specialText = 'Status with special chars: !@#$%^&*()';
    render(<StatusComponent text={specialText} />);
    
    const spanElement = screen.getByRole('alert').querySelector('span');
    expect(spanElement).toHaveTextContent(specialText);
  });

  it('handles unicode characters in text prop', () => {
    const unicodeText = 'Status with unicode: 中文 Español Français';
    render(<StatusComponent text={unicodeText} />);
    
    const spanElement = screen.getByRole('alert').querySelector('span');
    expect(spanElement).toHaveTextContent(unicodeText);
  });

  it('uses nullish coalescing operator consistently', () => {
    // Test that empty string is handled differently from null/undefined
    render(<StatusComponent text="" />);
    
    // Should return null for empty string
    expect(screen.queryByRole('alert')).not.toBeInTheDocument();
    
    // But should work for null/undefined
    render(<StatusComponent />);
    expect(screen.getByRole('alert')).toBeInTheDocument();
  });
}); 