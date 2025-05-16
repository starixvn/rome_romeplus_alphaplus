import React from 'react';
import { describe, expect, it, vi } from 'vitest';
import { render, screen, fireEvent } from '@testing-library/react';
import RegistrationLinks from './RegistrationLinks';

vi.mock('react-i18next', () => ({
  useTranslation: () => ({
    t: (key: string) => {
      const translations: Record<string, string> = {
        'registrationLinks.title': 'Registration Links',
        'registrationLinks.note': 'Manage your registration links',
        'registrationLinks.requestNewLink': 'Request New Link',
        'registrationLinks.domain': 'Domain',
        'registrationLinks.downloadQRCode': 'Download QR Code'
      };
      return translations[key] || key;
    }
  })
}));

describe('RegistrationLinks', () => {
  const mockDomains = [
    {
      id: '1',
      name: 'Domain 1',
      url: 'https://example1.com',
      status: 'active' as const
    },
    {
      id: '2',
      name: 'Domain 2',
      url: 'https://example2.com',
      status: 'blocked' as const
    }
  ];

  it('renders all domains with correct data', () => {
    render(<RegistrationLinks domains={mockDomains} loading={false} />);
    
    mockDomains.forEach(domain => {
      expect(screen.getByText(domain.name)).toBeInTheDocument();
      expect(screen.getByText(domain.url)).toBeInTheDocument();
    });
  });

  // it('shows request new link button only when all domains are blocked', () => {
  //   // Test with mixed status domains
  //   const { rerender } = render(<RegistrationLinks domains={mockDomains} loading={false} />);
  //   expect(screen.queryByText('Request New Link')).not.toBeInTheDocument();

  //   // Test with all blocked domains
  //   const allBlockedDomains = mockDomains.map(d => ({ ...d, status: 'blocked' as const }));
  //   rerender(<RegistrationLinks domains={allBlockedDomains} loading={false} />);
  //   expect(screen.getByText('Request New Link')).toBeInTheDocument();
  // });

  // it('calls onRequestNewLink when request button is clicked', () => {
  //   const allBlockedDomains = mockDomains.map(d => ({ ...d, status: 'blocked' as const }));
  //   render(<RegistrationLinks domains={allBlockedDomains} loading={false} />);
    
  //   fireEvent.click(screen.getByText('Request New Link'));
  //   expect(mockOnRequestNewLink).toHaveBeenCalled();
  // });

  it('handles empty domains array correctly', () => {
    render(<RegistrationLinks domains={[]} loading={false} />);
    expect(screen.getByText('No registration links available')).toBeInTheDocument();
  });

});
