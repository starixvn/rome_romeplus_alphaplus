import React from 'react';
import { describe, it, expect, vi } from 'vitest';
import { render, screen, fireEvent } from '@testing-library/react';
import '@testing-library/jest-dom';
import PopupModalComponent from './PopupModalComponent';
import { useTranslation } from 'react-i18next';

// Mock the useTranslation hook
vi.mock('react-i18next', () => ({
  useTranslation: () => ({
    t: (key: string) => key === 'close' ? 'Close' : key,
  }),
}));

describe('PopupModalComponent', () => {
  const mockOnClose = vi.fn();
  const defaultProps = {
    title: 'Test Modal',
    onClose: mockOnClose,
    children: <div>Test Content</div>,
  };

  it('renders with correct title and content', () => {
    render(<PopupModalComponent {...defaultProps} />);
    
    expect(screen.getByText('Test Modal')).toBeInTheDocument();
    expect(screen.getByText('Test Content')).toBeInTheDocument();
  });

  it('calls onClose when close button is clicked', () => {
    render(<PopupModalComponent {...defaultProps} />);
    
    const closeButton = screen.getByText('Close');
    fireEvent.click(closeButton);
    
    expect(mockOnClose).toHaveBeenCalledTimes(1);
  });

  it('calls onClose when close icon is clicked', () => {
    render(<PopupModalComponent {...defaultProps} />);
    
    const closeIcon = screen.getByRole('button', { name: /close/i });
    fireEvent.click(closeIcon);
    
    expect(mockOnClose).toHaveBeenCalledTimes(2);
  });

  it('renders with custom children content', () => {
    const customContent = <div data-testid="custom-content">Custom Content</div>;
    render(
      <PopupModalComponent
        title="Test Modal"
        onClose={mockOnClose}
        children={customContent}
      />
    );
    
    expect(screen.getByTestId('custom-content')).toBeInTheDocument();
    expect(screen.getByText('Custom Content')).toBeInTheDocument();
  });
}); 