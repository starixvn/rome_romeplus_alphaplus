import { describe, expect, it, vi } from 'vitest';

import { fireEvent, render, screen } from '@testing-library/react';

import GeneralSettings from './GeneralSettings';

describe('GeneralSettings', () => {
  const mockSettings = [
    {
      id: '1',
      name: 'Setting 1',
      description: 'Description 1',
      value: false,
      category: 'Category 1'
    },
    {
      id: '2',
      name: 'Setting 2',
      description: 'Description 2',
      value: true,
      category: 'Category 1'
    },
    {
      id: '3',
      name: 'Setting 3',
      description: 'Description 3',
      value: false,
      category: 'Category 2'
    }
  ];

  const mockOnSettingsChange = vi.fn();

  it('renders the title', () => {
    render(<GeneralSettings settings={mockSettings} onSettingsChange={mockOnSettingsChange} />);
    expect(screen.getByText('General Settings')).toBeInTheDocument();
  });

  it('groups settings by category', () => {
    render(<GeneralSettings settings={mockSettings} onSettingsChange={mockOnSettingsChange} />);
    expect(screen.getByText('Setting 1')).toBeInTheDocument();
    expect(screen.getByText('Setting 2')).toBeInTheDocument();
    expect(screen.getByText('Setting 3')).toBeInTheDocument();
  });

  it('renders setting descriptions when provided', () => {
    render(<GeneralSettings settings={mockSettings} onSettingsChange={mockOnSettingsChange} />);
    expect(screen.getByText('Description 1')).toBeInTheDocument();
    expect(screen.getByText('Description 2')).toBeInTheDocument();
  });

  it('calls onSettingsChange when a setting is toggled', () => {
    render(<GeneralSettings settings={mockSettings} onSettingsChange={mockOnSettingsChange} />);
    const setting1 = screen.getByText('Setting 1').closest('button');
    fireEvent.click(setting1!);
    expect(mockOnSettingsChange).toHaveBeenCalledWith('1', true);
  });

  it('applies correct styling to setting buttons', () => {
    render(<GeneralSettings settings={mockSettings} onSettingsChange={mockOnSettingsChange} />);
    const settingButton = screen.getByText('Setting 1').closest('button');
    expect(settingButton).toHaveClass('w-full', 'text-left', 'flex', 'items-center', 'justify-between', 'p-4', 'bg-gray-50', 'rounded-lg', 'cursor-pointer', 'hover:bg-gray-100', 'transition-colors');
  });

  it('applies correct styling to setting names and descriptions', () => {
    render(<GeneralSettings settings={mockSettings} onSettingsChange={mockOnSettingsChange} />);
    const settingName = screen.getByText('Setting 1');
    const settingDescription = screen.getByText('Description 1');
    
    expect(settingName).toHaveClass('text-sm', 'font-medium', 'text-gray-900');
    expect(settingDescription).toHaveClass('mt-1', 'text-xs', 'text-gray-500', 'line-clamp-2');
  });
});
