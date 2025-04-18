import { describe, expect, it } from 'vitest';

import { render, screen } from '@testing-library/react';

import DomainCard from './DomainCard';

describe('DomainCard', () => {
  const mockProps = {
    domain: 'example.com',
    url: 'https://example.com/register',
  };

  it('renders domain and URL correctly', () => {
    render(<DomainCard {...mockProps} />);
    expect(screen.getByText(mockProps.domain)).toBeInTheDocument();
    expect(screen.getByText(mockProps.url)).toBeInTheDocument();
  });

  it('renders download QR code button', () => {
    render(<DomainCard {...mockProps} />);
    expect(screen.getByRole('button', { name: /Download QR Code/i })).toBeInTheDocument();
  });

  it('applies correct styling to domain text', () => {
    render(<DomainCard {...mockProps} />);
    const domainText = screen.getByText(mockProps.domain);
    expect(domainText).toHaveClass('font-semibold');
  });

  it('applies correct styling to URL text', () => {
    render(<DomainCard {...mockProps} />);
    const urlText = screen.getByText(mockProps.url);
    expect(urlText).toHaveClass('text-sm');
    expect(urlText).toHaveClass('opacity-90');
    expect(urlText).toHaveClass('break-all');
  });

  it('applies correct styling to the card container', () => {
    render(<DomainCard {...mockProps} />);
    const card = screen.getByRole('button', { name: /Download QR Code/i }).closest('div');
    expect(card).toHaveClass('domain-card');
  });

  it('applies correct styling to the button', () => {
    render(<DomainCard {...mockProps} />);
    const button = screen.getByRole('button', { name: /Download QR Code/i });
    expect(button).toHaveClass('domain-card-button');
  });
});
