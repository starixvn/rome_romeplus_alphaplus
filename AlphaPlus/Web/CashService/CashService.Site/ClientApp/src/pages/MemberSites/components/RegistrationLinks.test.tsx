import { describe, expect, it } from 'vitest';

import { render, screen } from '@testing-library/react';

import RegistrationLinks from './RegistrationLinks';

describe('RegistrationLinks', () => {
  const mockDomains = [
    { url: 'https://example1.com', qrCode: 'https://example1.com/register' },
    { url: 'https://example2.com', qrCode: 'https://example2.com/register' },
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

  it('displays correct URL for each card', () => {
    render(<RegistrationLinks domains={mockDomains} />);

    mockDomains.forEach((domain) => {
      expect(screen.getByText(domain.url)).toBeInTheDocument();
    });
  });

  it('renders with empty domains array', () => {
    render(<RegistrationLinks domains={[]} />);
    expect(screen.queryByRole('button', { name: /Download QR Code/i })).not.toBeInTheDocument();
  });

  it('applies correct grid layout', () => {
    render(<RegistrationLinks domains={mockDomains} />);
    const grid = screen.getByRole('button', { name: /Download QR Code/i }).closest('div')?.parentElement;
    expect(grid).toHaveClass('grid');
    expect(grid).toHaveClass('grid-cols-1');
    expect(grid).toHaveClass('md:grid-cols-2');
    expect(grid).toHaveClass('gap-4');
  });
});
