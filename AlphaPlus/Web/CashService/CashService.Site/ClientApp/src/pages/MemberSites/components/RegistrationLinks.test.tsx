import { describe, expect, it, vi } from 'vitest';

import { render, screen } from '@testing-library/react';

import RegistrationLinks from './RegistrationLinks';

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

  const mockOnRequestNewLink = vi.fn();

  it('renders the title and description', () => {
    render(<RegistrationLinks domains={mockDomains} />);
    expect(screen.getByText('Registration Links')).toBeInTheDocument();
    expect(screen.getByText('If bolth link are blocked request a new link')).toBeInTheDocument();
  });

  it('renders all domain cards', () => {
    render(<RegistrationLinks domains={mockDomains} />);
    expect(screen.getAllByText(/Domain \d/)).toHaveLength(2);
  });

  it('displays correct URL for each card', () => {
    render(<RegistrationLinks domains={mockDomains} />);
    mockDomains.forEach((domain) => {
      expect(screen.getByText(domain.url)).toBeInTheDocument();
    });
  });

  it('shows request new link button when all domains are blocked', () => {
    const blockedDomains = [
      { ...mockDomains[0], status: 'blocked' as const },
      { ...mockDomains[1], status: 'blocked' as const }
    ];
    render(<RegistrationLinks domains={blockedDomains} onRequestNewLink={mockOnRequestNewLink} />);
    expect(screen.getByText('Request New Link')).toBeInTheDocument();
    expect(screen.getByText('All links are blocked. You may request new ones.')).toBeInTheDocument();
  });

  it('does not show request new link button when not all domains are blocked', () => {
    render(<RegistrationLinks domains={mockDomains} onRequestNewLink={mockOnRequestNewLink} />);
    expect(screen.queryByText('Request New Link')).not.toBeInTheDocument();
  });

  it('renders empty state when no domains are available', () => {
    render(<RegistrationLinks domains={[]} />);
    expect(screen.getByText('No registration links available')).toBeInTheDocument();
  });


  it('applies correct styling to the request new link button', () => {
    const blockedDomains = [
      { ...mockDomains[0], status: 'blocked' as const },
      { ...mockDomains[1], status: 'blocked' as const }
    ];
    render(<RegistrationLinks domains={blockedDomains} onRequestNewLink={mockOnRequestNewLink} />);
    const button = screen.getByText('Request New Link');
    expect(button).toHaveClass('px-4', 'py-2', 'text-sm', 'bg-[#1e4d8c]', 'text-white', 'rounded-md', 'hover:bg-[#173d70]', 'transition-colors');
  });
});
