import { describe, expect, it, vi } from 'vitest';
import { fireEvent, render, screen } from '@testing-library/react';
import MessageForm from './MessageForm';

vi.mock('react-i18next', () => ({
  useTranslation: () => ({
    t: (key: string) => {
      const translations: Record<string, string> = {
        'message.title': 'Message',
        'message.description': 'Description',
        'message.mobile': 'Mobile',
        'message.lineId': 'LINE ID',
        'message.announcement': 'Announcement',
        'message.placeholder.message': 'Enter your message'
      };
      return translations[key] || key;
    }
  })
}));

describe('MessageForm', () => {
  const defaultProps = {
    countryCodes: ['66', '84'],
    countryCode: '',
    mobileCode: '',
    mobilePhone: '',
    lineId: '',
    announcement: '',
    onMessageChange: vi.fn()
  };

  it('renders all form fields with correct labels and placeholders', () => {
    render(<MessageForm {...defaultProps} loading={false} />);
    
    // Check all form fields are present
    expect(screen.getByLabelText('Mobile')).toBeInTheDocument();
    expect(screen.getByLabelText('LINE ID', { exact: false })).toBeInTheDocument();
    expect(screen.getByLabelText('Announcement')).toBeInTheDocument();
    
    // Check placeholders
    expect(screen.getByPlaceholderText('XXXXXXXXX')).toBeInTheDocument();
    expect(screen.getByPlaceholderText('Enter your message')).toBeInTheDocument();
  });

  it('handles mobile number input changes correctly', () => {
    const onMessageChange = vi.fn();
    render(<MessageForm {...defaultProps} onMessageChange={onMessageChange} loading={false} />);
    
    const mobileInput = screen.getByLabelText('Mobile');
    fireEvent.change(mobileInput, { target: { value: '123456789' } });
    
    expect(onMessageChange).toHaveBeenCalledWith({ mobilePhone: '123456789' });
  });

  it('handles LINE ID input changes correctly', () => {
    const onMessageChange = vi.fn();
    render(<MessageForm {...defaultProps} onMessageChange={onMessageChange} loading={false} />);
    
    const lineIdInput = screen.getByLabelText('LINE ID', { exact: false });
    fireEvent.change(lineIdInput, { target: { value: 'testlineid' } });
    
    expect(onMessageChange).toHaveBeenCalledWith({ lineId: 'testlineid' });
  });

  it('handles announcement text changes correctly', () => {
    const onMessageChange = vi.fn();
    render(<MessageForm {...defaultProps} onMessageChange={onMessageChange} loading={false} />);
    
    const announcementInput = screen.getByLabelText('Announcement');
    fireEvent.change(announcementInput, { target: { value: 'Test announcement' } });
    
    expect(onMessageChange).toHaveBeenCalledWith({ announcement: 'Test announcement' });
  });

  it('maintains correct values when props are updated', () => {
    const { rerender } = render(<MessageForm {...defaultProps} loading={false} />);
    
    // Initial render with empty values
    expect(screen.getByLabelText('Mobile')).toHaveValue('');
    expect(screen.getByLabelText('LINE ID', { exact: false })).toHaveValue('');
    expect(screen.getByLabelText('Announcement')).toHaveValue('');
    
    // Update with new values
    const updatedProps = {
      mobile: '123456789',
      lineId: 'testlineid',
      announcement: 'Test announcement',
      onMessageChange: vi.fn(),
      countryCodes: ['66', '84'],
      mobileCode: '66',
      mobilePhone: '123456789'
    };
    
    rerender(<MessageForm {...updatedProps} loading={false} />);
    
    // Verify values are updated
    expect(screen.getByLabelText('Mobile')).toHaveValue('123456789');
    expect(screen.getByLabelText('LINE ID', { exact: false })).toHaveValue('testlineid');
    expect(screen.getByLabelText('Announcement')).toHaveValue('Test announcement');
  });

  it('handles multiple field updates correctly', () => {
    const onMessageChange = vi.fn();
    render(<MessageForm {...defaultProps} onMessageChange={onMessageChange} loading={false} />);
    
    // Update all fields
    fireEvent.change(screen.getByLabelText('Mobile'), { target: { value: '123456789' } });
    fireEvent.change(screen.getByLabelText('LINE ID', { exact: false }), { target: { value: 'testlineid' } });
    fireEvent.change(screen.getByLabelText('Announcement'), { target: { value: 'Test announcement' } });
    
    // Verify all updates were called with correct values
    expect(onMessageChange).toHaveBeenCalledTimes(3);
    expect(onMessageChange).toHaveBeenCalledWith({ mobilePhone: '123456789' });
    expect(onMessageChange).toHaveBeenCalledWith({ lineId: 'testlineid' });
    expect(onMessageChange).toHaveBeenCalledWith({ announcement: 'Test announcement' });
  });
});
