import { describe, expect, it, vi } from 'vitest';

import { fireEvent, render, screen } from '@testing-library/react';

import MessageForm from './MessageForm';

describe('MessageForm', () => {
  it('renders the title and description', () => {
    render(<MessageForm />);
    expect(screen.getByText('Leave a Message')).toBeInTheDocument();
    expect(
      screen.getByText('Include your contact details and a message for your member.')
    ).toBeInTheDocument();
  });

  it('renders all form fields', () => {
    render(<MessageForm />);
    expect(screen.getByLabelText(/Mobile/i)).toBeInTheDocument();
    expect(screen.getByLabelText(/LINE ID/i)).toBeInTheDocument();
    expect(screen.getByLabelText(/Announcement/i)).toBeInTheDocument();
  });

  it('updates mobile number when input changes', () => {
    render(<MessageForm />);
    const mobileInput = screen.getByLabelText(/Mobile/i);
    fireEvent.change(mobileInput, { target: { value: '1234567890' } });
    expect(mobileInput).toHaveValue('1234567890');
  });

  it('updates LINE ID when input changes', () => {
    render(<MessageForm />);
    const lineIdInput = screen.getByLabelText(/LINE ID/i);
    fireEvent.change(lineIdInput, { target: { value: 'testlineid' } });
    expect(lineIdInput).toHaveValue('testlineid');
  });

  it('updates announcement when textarea changes', () => {
    render(<MessageForm />);
    const announcementTextarea = screen.getByLabelText(/Announcement/i);
    fireEvent.change(announcementTextarea, { target: { value: 'Test announcement' } });
    expect(announcementTextarea).toHaveValue('Test announcement');
  });

  it('shows country code prefix for mobile input', () => {
    render(<MessageForm />);
    expect(screen.getByText('+66')).toBeInTheDocument();
  });

  it('handles form submission', () => {
    const handleSubmit = vi.fn();
    render(<MessageForm />);

    const form = screen.getByTestId('message-form');
    form.onsubmit = handleSubmit;

    fireEvent.submit(form);
    expect(handleSubmit).toHaveBeenCalled();
  });

  it('applies correct styling to form elements', () => {
    render(<MessageForm />);
    const inputs = screen.getAllByRole('textbox');
    const button = screen.getByRole('button', { name: /Save/i });

    inputs.forEach((input) => {
      expect(input).toHaveClass('border-gray-300');
      if (input.id === 'mobile') {
        expect(input).toHaveClass('rounded-r-lg');
      } else {
        expect(input).toHaveClass('rounded-lg');
      }
    });

    expect(button).toHaveClass('bg-[#1e4d8c]');
    expect(button).toHaveClass('text-white');
  });
});
