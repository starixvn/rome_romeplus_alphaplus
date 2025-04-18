import { describe, it, expect, vi, beforeEach } from 'vitest';
import { render, screen, fireEvent } from '@testing-library/react';
import React from 'react';
import ToggleSetting from './ToggleSetting';

describe('ToggleSetting', () => {
  const mockLabel = 'Test Setting';
  const mockOnChange = vi.fn();

  beforeEach(() => {
    mockOnChange.mockClear();
  });

  it('renders with correct label', () => {
    render(<ToggleSetting label={mockLabel} value={false} onChange={mockOnChange} />);
    expect(screen.getByText(mockLabel)).toBeInTheDocument();
  });

  it('renders in off state when value is false', () => {
    render(<ToggleSetting label={mockLabel} value={false} onChange={mockOnChange} />);
    const toggle = screen.getByRole('button');
    expect(toggle).toHaveClass('bg-gray-200');
  });

  it('renders in on state when value is true', () => {
    render(<ToggleSetting label={mockLabel} value={true} onChange={mockOnChange} />);
    const toggle = screen.getByRole('button');
    expect(toggle).toHaveClass('bg-blue-600');
  });

  it('calls onChange with true when toggled from off to on', () => {
    render(<ToggleSetting label={mockLabel} value={false} onChange={mockOnChange} />);
    const toggle = screen.getByRole('button');
    fireEvent.click(toggle);
    expect(mockOnChange).toHaveBeenCalledWith(true);
  });

  it('calls onChange with false when toggled from on to off', () => {
    render(<ToggleSetting label={mockLabel} value={true} onChange={mockOnChange} />);
    const toggle = screen.getByRole('button');
    fireEvent.click(toggle);
    expect(mockOnChange).toHaveBeenCalledWith(false);
  });
}); 