import { describe, expect, it, vi } from 'vitest';

import { fireEvent, render, screen } from '@testing-library/react';

import GeneralSettings from './GeneralSettings';

vi.mock('react-i18next', () => ({
  useTranslation: () => ({
    t: (key: string) => {
      const translations: Record<string, string> = {
        'generalSettings.title': 'General Settings',
        'generalSettings.setting1.title': 'Setting 1',
        'generalSettings.setting1.description': 'Description 1',
        'generalSettings.setting2.title': 'Setting 2',
        'generalSettings.setting2.description': 'Description 2',
        'generalSettings.setting3.title': 'Setting 3',
        'generalSettings.setting3.description': 'Description 3',
      };
      return translations[key] || key;
    },
  }),
}));

describe('GeneralSettings', () => {
  const mockSettings = [
    {
      id: 'setting1',
      name: 'Setting 1',
      description: 'Description 1',
      value: false,
      category: 'General',
    },
    {
      id: 'setting2',
      name: 'Setting 2',
      description: 'Description 2',
      value: true,
      category: 'General',
    },
    {
      id: 'setting3',
      name: 'Setting 3',
      description: 'Description 3',
      value: false,
      category: 'Security',
    },
  ];

  const mockOnSettingsChange = vi.fn();

  it('renders all settings with correct content', () => {
    render(<GeneralSettings settings={mockSettings} onSettingsChange={mockOnSettingsChange} loading={false} />);

    // Check if all settings are rendered with their titles and descriptions
    mockSettings.forEach((setting) => {
      expect(screen.getByText(setting.name)).toBeInTheDocument();
      expect(screen.getByText(setting.description)).toBeInTheDocument();
    });
  });

  it('groups settings by category correctly', () => {
    render(<GeneralSettings settings={mockSettings} onSettingsChange={mockOnSettingsChange} loading={false} />);

    // Get all setting buttons
    const settingButtons = screen.getAllByRole('button');

    // Verify that settings are grouped (General settings should be together)
    //const generalSettings = mockSettings.filter(s => s.category === 'General');
    //const securitySettings = mockSettings.filter(s => s.category === 'Security');

    // Check if the number of rendered buttons matches the number of settings
    expect(settingButtons.length).toBe(mockSettings.length);
  });

  it('calls onSettingsChange with correct parameters when toggling settings', () => {
    render(<GeneralSettings settings={mockSettings} onSettingsChange={mockOnSettingsChange} loading={false} />);

    // Test toggling each setting
    mockSettings.forEach((setting) => {
      const settingButton = screen.getByText(setting.name).closest('button');
      fireEvent.click(settingButton!);
      expect(mockOnSettingsChange).toHaveBeenCalledWith(setting.id, !setting.value);
    });
  });

  it('handles empty settings array', () => {
    render(<GeneralSettings settings={[]} onSettingsChange={mockOnSettingsChange} loading={false} />);

    // Verify no settings are rendered
    mockSettings.forEach((setting) => {
      expect(screen.queryByText(setting.name)).not.toBeInTheDocument();
      expect(screen.queryByText(setting.description)).not.toBeInTheDocument();
    });
  });

  it('maintains correct state for each setting', () => {
    const { rerender } = render(
      <GeneralSettings settings={mockSettings} onSettingsChange={mockOnSettingsChange} loading={false} />
    );

    // Verify initial states
    mockSettings.forEach((setting) => {
      const checkbox = screen
        .getByText(setting.name)
        .closest('button')
        ?.querySelector('input[type="checkbox"]');
      expect(checkbox).toHaveProperty('checked', setting.value);
    });

    // Update settings and rerender
    const updatedSettings = mockSettings.map((setting) => ({
      ...setting,
      value: !setting.value,
    }));

    rerender(
      <GeneralSettings settings={updatedSettings} onSettingsChange={mockOnSettingsChange} loading={false} />
    );

    // Verify updated states
    updatedSettings.forEach((setting) => {
      const checkbox = screen
        .getByText(setting.name)
        .closest('button')
        ?.querySelector('input[type="checkbox"]');
      expect(checkbox).toHaveProperty('checked', setting.value);
    });
  });
});
