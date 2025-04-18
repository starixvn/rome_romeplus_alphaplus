import { describe, expect, it } from 'vitest';

import { render, screen } from '@testing-library/react';

import RegistrationLinks from './RegistrationLinks';

describe('RegistrationLinks', () => {
  const mockDomains = [
    { domain: 'example1.com', url: 'https://example1.com/register' },
    { domain: 'example2.com', url: 'https://example2.com/register' },
  ];

  it('renders the title and description', () => {
    render(<RegistrationLinks domains={mockDomains} />);
    expect(screen.getByText('Registration Links')).toBeInTheDocument();
    expect(screen.getByText('If both links are blocked, request a new link.')).toBeInTheDocument();
  });

  it('renders all domain cards', () => {
    render(<RegistrationLinks domains={mockDomains} />);
    expect(screen.getAllByRole('button', { name: /Download QR Code/i })).toHaveLength(2);
  });

  it('displays correct domain and URL for each card', () => {
    render(<RegistrationLinks domains={mockDomains} />);

    mockDomains.forEach((domain) => {
      expect(screen.getByText(domain.domain)).toBeInTheDocument();
      expect(screen.getByText(domain.url)).toBeInTheDocument();
    });
  });

  it('renders with empty domains array', () => {
    render(<RegistrationLinks domains={[]} />);
    expect(screen.queryByRole('button', { name: /Download QR Code/i })).not.toBeInTheDocument();
  });

  it('applies correct styling to domain cards', () => {
    render(<RegistrationLinks domains={mockDomains} />);
    const cards = screen
      .getAllByRole('button', { name: /Download QR Code/i })
      .map((button) => button.closest('div'));

    cards.forEach((card) => {
      expect(card).toHaveClass('bg-[#1e4d8c]');
      expect(card).toHaveClass('text-white');
      expect(card).toHaveClass('rounded-lg');
    });
  });
});
