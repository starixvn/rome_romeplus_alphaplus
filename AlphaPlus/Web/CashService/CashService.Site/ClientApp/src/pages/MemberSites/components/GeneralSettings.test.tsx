import { describe, expect, it } from 'vitest';

import { fireEvent, render, screen } from '@testing-library/react';

import GeneralSettings from './GeneralSettings';

describe('GeneralSettings', () => {
  it('renders the title and settings sections', () => {
    render(<GeneralSettings />);
    expect(screen.getByText('General Settings')).toBeInTheDocument();
    expect(screen.getByText('Registration')).toBeInTheDocument();
    expect(screen.getByText('Deposit')).toBeInTheDocument();
  });

  it('renders toggle switches for both settings', () => {
    render(<GeneralSettings />);
    const checkboxes = screen.getAllByRole('checkbox');
    expect(checkboxes).toHaveLength(2);
  });

  it('toggles registration setting when clicked', () => {
    render(<GeneralSettings />);
    const checkboxes = screen.getAllByRole('checkbox');
    const registrationToggle = checkboxes[0];

    expect(registrationToggle).not.toBeChecked();
    fireEvent.click(registrationToggle);
    expect(registrationToggle).toBeChecked();
  });

  it('toggles deposit setting when clicked', () => {
    render(<GeneralSettings />);
    const checkboxes = screen.getAllByRole('checkbox');
    const depositToggle = checkboxes[1];

    expect(depositToggle).not.toBeChecked();
    fireEvent.click(depositToggle);
    expect(depositToggle).toBeChecked();
  });

  it('displays correct descriptions for each setting', () => {
    render(<GeneralSettings />);
    expect(screen.getByText('Allow member to register')).toBeInTheDocument();
    expect(screen.getByText('Allow member to deposit')).toBeInTheDocument();
  });

  it('applies correct styling to toggle switches', () => {
    render(<GeneralSettings />);
    const checkboxes = screen.getAllByRole('checkbox');

    checkboxes.forEach((checkbox) => {
      expect(checkbox).toHaveClass('sr-only');
      expect(checkbox).toHaveClass('peer');
    });
  });
});
