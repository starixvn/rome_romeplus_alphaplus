import { beforeEach, describe, expect, it, vi } from 'vitest';

import { fireEvent, render, screen } from '@testing-library/react';

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
    const toggleCircle = toggle.querySelector('span');
    expect(toggleCircle).toHaveClass('translate-x-1');
  });

  it('renders in on state when value is true', () => {
    render(<ToggleSetting label={mockLabel} value={true} onChange={mockOnChange} />);
    const toggle = screen.getByRole('button');
    expect(toggle).toHaveClass('bg-blue-600');
    const toggleCircle = toggle.querySelector('span');
    expect(toggleCircle).toHaveClass('translate-x-6');
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

  it('applies correct styling to container', () => {
    render(<ToggleSetting label={mockLabel} value={false} onChange={mockOnChange} />);
    const container = screen.getByText(mockLabel).closest('div');
    expect(container).toHaveClass('flex', 'items-center', 'justify-between', 'py-3', 'border-b', 'border-gray-200');
  });

  it('applies correct styling to toggle button', () => {
    render(<ToggleSetting label={mockLabel} value={false} onChange={mockOnChange} />);
    const toggle = screen.getByRole('button');
    expect(toggle).toHaveClass('relative', 'inline-flex', 'h-6', 'w-11', 'items-center', 'rounded-full', 'transition-colors', 'duration-200');
  });

  it('applies correct styling to toggle circle', () => {
    render(<ToggleSetting label={mockLabel} value={false} onChange={mockOnChange} />);
    const toggleCircle = screen.getByRole('button').querySelector('span');
    expect(toggleCircle).toHaveClass('inline-block', 'h-4', 'w-4', 'transform', 'rounded-full', 'bg-white', 'transition-transform', 'duration-200');
  });
});
