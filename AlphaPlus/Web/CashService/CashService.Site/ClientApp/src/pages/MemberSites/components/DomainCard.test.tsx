import { describe, expect, it, vi } from 'vitest';
import { render, screen, fireEvent } from '@testing-library/react';
import DomainCard from './DomainCard';

describe('DomainCard', () => {
  const mockDomain = {
    id: '1',
    name: 'Example Domain',
    url: 'https://example.com',
    status: 'active' as const,
  };

  it('renders domain URL and index correctly', () => {
    render(<DomainCard domain={mockDomain} index={0} />);
    expect(screen.getByText('Domain 1')).toBeInTheDocument();
    expect(screen.getByText(mockDomain.url)).toBeInTheDocument();
  });

  it('renders copy button with correct icon', () => {
    render(<DomainCard domain={mockDomain} index={0} />);
    const copyButton = screen.getByTitle('Copy URL');
    expect(copyButton).toBeInTheDocument();
    expect(copyButton.querySelector('svg')).toBeInTheDocument();
  });

  it('renders download QR code button with correct styling', () => {
    render(<DomainCard domain={mockDomain} index={0} />);
    const button = screen.getByText('Download QR Code');
    expect(button).toHaveClass('bg-white');
    expect(button).toHaveClass('text-[#1e4d8c]');
    expect(button).toHaveClass('rounded');
  });

  it('disables download QR code button when domain is blocked', () => {
    const blockedDomain = { ...mockDomain, status: 'blocked' as const };
    render(<DomainCard domain={blockedDomain} index={0} />);
    const button = screen.getByText('Download QR Code');
    expect(button).toBeDisabled();
    expect(button).toHaveClass('opacity-50');
    expect(button).toHaveClass('cursor-not-allowed');
  });


  it('handles copy URL functionality', async () => {
    const writeTextMock = vi.fn();
    Object.assign(navigator, {
      clipboard: {
        writeText: writeTextMock,
      },
    });

    render(<DomainCard domain={mockDomain} index={0} />);
    const copyButton = screen.getByTitle('Copy URL');
    
    await fireEvent.click(copyButton);
    
    expect(writeTextMock).toHaveBeenCalledWith(mockDomain.url);
  });
});
