import { describe, expect, it, vi } from 'vitest';

import { fireEvent, render, screen } from '@testing-library/react';

import MessageForm from './MessageForm';

describe('MessageForm', () => {
  const defaultProps = {
    mobile: '',
    lineId: '',
    announcement: '',
    onMessageChange: vi.fn()
  };

  it('renders the title and description', () => {
    render(<MessageForm {...defaultProps} />);
    expect(screen.getByText('Leave a Message')).toBeInTheDocument();
    expect(screen.getByText('Include your contact details and a message for your member.')).toBeInTheDocument();
  });

  it('renders all form fields with correct labels', () => {
    render(<MessageForm {...defaultProps} />);
    expect(screen.getByText('Mobile Number')).toBeInTheDocument();
    expect(screen.getByText('LINE ID')).toBeInTheDocument();
    expect(screen.getByText('Announcement')).toBeInTheDocument();
  });

  it('renders mobile input with country code prefix', () => {
    render(<MessageForm {...defaultProps} />);
    expect(screen.getByText('+66')).toBeInTheDocument();
    expect(screen.getByPlaceholderText('XXXXXXXXX')).toBeInTheDocument();
  });

  it('calls onMessageChange when mobile number changes', () => {
    const onMessageChange = vi.fn();
    render(<MessageForm {...defaultProps} onMessageChange={onMessageChange} />);
    
    fireEvent.change(screen.getByPlaceholderText('XXXXXXXXX'), { 
      target: { value: '123456789' } 
    });
    
    expect(onMessageChange).toHaveBeenCalledWith({ mobile: '123456789' });
  });

  it('calls onMessageChange when LINE ID changes', () => {
    const onMessageChange = vi.fn();
    render(<MessageForm {...defaultProps} onMessageChange={onMessageChange} />);
    
    const lineIdInput = screen.getByLabelText('LINE ID');
    fireEvent.change(lineIdInput, { 
      target: { value: 'testlineid' } 
    });
    
    expect(onMessageChange).toHaveBeenCalledWith({ lineId: 'testlineid' });
  });

  it('calls onMessageChange when announcement changes', () => {
    const onMessageChange = vi.fn();
    render(<MessageForm {...defaultProps} onMessageChange={onMessageChange} />);
    
    const announcementTextarea = screen.getByPlaceholderText('Add your message here...');
    fireEvent.change(announcementTextarea, { 
      target: { value: 'Test announcement' } 
    });
    
    expect(onMessageChange).toHaveBeenCalledWith({ announcement: 'Test announcement' });
  });
});
