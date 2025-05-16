import React from 'react';
import { describe, expect, it, vi } from 'vitest';
import { fireEvent, render, screen } from '@testing-library/react';
import DomainCard from './DomainCard';

vi.mock('react-i18next', () => ({
  useTranslation: () => ({
    t: (key: string) => {
      const translations: Record<string, string> = {
        'registrationLinks.domain': 'Domain',
        'registrationLinks.downloadQRCode': 'Download QR Code',
      };
      return translations[key] || key;
    },
  }),
}));

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

  it('handles copy URL functionality successfully', async () => {
    const writeTextMock = vi.fn().mockResolvedValue(undefined);
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

  it('handles copy URL functionality failure', async () => {
    const writeTextMock = vi.fn().mockRejectedValue(new Error('Failed to copy'));
    const consoleErrorMock = vi.spyOn(console, 'error').mockImplementation(() => {});

    Object.assign(navigator, {
      clipboard: {
        writeText: writeTextMock,
      },
    });

    render(<DomainCard domain={mockDomain} index={0} />);
    const copyButton = screen.getByTitle('Copy URL');

    await fireEvent.click(copyButton);

    expect(consoleErrorMock).toHaveBeenCalledWith('Failed to copy text: ', expect.any(Error));
    consoleErrorMock.mockRestore();
  });

  it('disables download QR code button when domain is blocked', () => {
    const blockedDomain = { ...mockDomain, status: 'blocked' as const };
    render(<DomainCard domain={blockedDomain} index={0} />);
    const button = screen.getByText('Download QR Code');
    expect(button).toBeDisabled();
  });

  it('enables download QR code button when domain is active', () => {
    render(<DomainCard domain={mockDomain} index={0} />);
    const button = screen.getByText('Download QR Code');
    expect(button).not.toBeDisabled();
  });

  it('handles QR code download functionality', () => {
    const createElementSpy = vi.spyOn(document, 'createElement');
    const appendChildSpy = vi.spyOn(document.body, 'appendChild');
    //const removeChildSpy = vi.spyOn(document.body, 'removeChild');
    const clickSpy = vi.spyOn(HTMLAnchorElement.prototype, 'click');

    render(<DomainCard domain={mockDomain} index={0} />);
    const downloadButton = screen.getByText('Download QR Code');

    fireEvent.click(downloadButton);

    expect(createElementSpy).toHaveBeenCalledWith('a');
    expect(appendChildSpy).toHaveBeenCalled();
    expect(clickSpy).toHaveBeenCalled();

    createElementSpy.mockRestore();
    appendChildSpy.mockRestore();
    clickSpy.mockRestore();
  });
});
