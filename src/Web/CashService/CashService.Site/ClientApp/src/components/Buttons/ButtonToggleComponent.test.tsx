import React from 'react';
import { render, screen } from '@testing-library/react';
import { EnableButtonComponent, DisableButtonComponent, ButtonOnComponent, ButtonOffComponent } from './ButtonToggleComponent';

// Mock react-i18next
jest.mock('react-i18next', () => ({
  useTranslation: () => ({
    t: (key: string) => {
      const translations = {
        'enable': 'Enable',
        'disable': 'Disable',
        'on': 'On',
        'off': 'Off'
      };
      return translations[key] || key;
    }
  })
}));

describe('ButtonToggleComponent', () => {
  describe('EnableButtonComponent', () => {
    it('should display custom text when provided', () => {
      render(<EnableButtonComponent text="Custom Enable" isActive={true} />);
      expect(screen.getByText('Custom Enable')).toBeInTheDocument();
    });

    it('should display default text when no text prop provided', () => {
      render(<EnableButtonComponent isActive={true} />);
      expect(screen.getByText('Enable')).toBeInTheDocument();
    });
  });

  describe('DisableButtonComponent', () => {
    it('should display custom text when provided', () => {
      render(<DisableButtonComponent text="Custom Disable" isActive={true} />);
      expect(screen.getByText('Custom Disable')).toBeInTheDocument();
    });

    it('should display default text when no text prop provided', () => {
      render(<DisableButtonComponent isActive={true} />);
      expect(screen.getByText('Disable')).toBeInTheDocument();
    });
  });

  describe('ButtonOnComponent', () => {
    it('should display custom text when provided', () => {
      render(<ButtonOnComponent text="Custom On" isActive={true} />);
      expect(screen.getByText('Custom On')).toBeInTheDocument();
    });

    it('should display default text when no text prop provided', () => {
      render(<ButtonOnComponent isActive={true} />);
      expect(screen.getByText('On')).toBeInTheDocument();
    });
  });

  describe('ButtonOffComponent', () => {
    it('should display custom text when provided', () => {
      render(<ButtonOffComponent text="Custom Off" isActive={true} />);
      expect(screen.getByText('Custom Off')).toBeInTheDocument();
    });

    it('should display default text when no text prop provided', () => {
      render(<ButtonOffComponent isActive={true} />);
      expect(screen.getByText('Off')).toBeInTheDocument();
    });
  });
}); 