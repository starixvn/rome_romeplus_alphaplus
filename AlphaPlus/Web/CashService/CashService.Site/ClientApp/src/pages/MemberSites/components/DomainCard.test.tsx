import { describe, expect, it } from 'vitest';

import { render, screen } from '@testing-library/react';

import DomainCard from './DomainCard';

describe('DomainCard', () => {
  const mockDomain = {
    url: 'https://example.com',
    qrCode: 'https://example.com/register',
  };

  it('renders URL correctly', () => {
    render(<DomainCard domain={mockDomain} />);
    expect(screen.getByText(mockDomain.url)).toBeInTheDocument();
  });

  it('renders QR code image when provided', () => {
    render(<DomainCard domain={mockDomain} />);
    const qrCodeImage = screen.getByAltText('QR Code');
    expect(qrCodeImage).toBeInTheDocument();
    expect(qrCodeImage).toHaveAttribute('src', mockDomain.qrCode);
  });

  it('renders download QR code button', () => {
    render(<DomainCard domain={mockDomain} />);
    expect(screen.getByRole('button', { name: /Download QR Code/i })).toBeInTheDocument();
  });

  it('applies correct styling to URL text', () => {
    render(<DomainCard domain={mockDomain} />);
    const urlText = screen.getByText(mockDomain.url);
    expect(urlText).toHaveClass('font-medium');
  });

  it('applies correct styling to the card container', () => {
    render(<DomainCard domain={mockDomain} />);
    const card = screen.getByRole('button', { name: /Download QR Code/i }).closest('div');
    expect(card).toHaveClass('bg-[#1e4d8c]');
    expect(card).toHaveClass('text-white');
    expect(card).toHaveClass('rounded-lg');
  });

  it('applies correct styling to the button', () => {
    render(<DomainCard domain={mockDomain} />);
    const button = screen.getByRole('button', { name: /Download QR Code/i });
    expect(button).toHaveClass('bg-white');
    expect(button).toHaveClass('text-[#1e4d8c]');
    expect(button).toHaveClass('rounded');
    expect(button).toHaveClass('text-sm');
  });
});
