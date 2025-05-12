import { describe, it, expect, vi } from 'vitest';
import { handleNumericInput, handleIntegerInput } from './inputHandlers';

describe('handleNumericInput', () => {
  it('should allow valid numeric input with decimal point', () => {
    const setter = vi.fn();
    handleNumericInput('123.45', setter);
    expect(setter).toHaveBeenCalledWith('123.45');
  });

  it('should allow valid numeric input without decimal point', () => {
    const setter = vi.fn();
    handleNumericInput('123', setter);
    expect(setter).toHaveBeenCalledWith('123');
  });

  it('should allow empty string', () => {
    const setter = vi.fn();
    handleNumericInput('', setter);
    expect(setter).toHaveBeenCalledWith('');
  });

  it('should not allow non-numeric characters', () => {
    const setter = vi.fn();
    handleNumericInput('123abc', setter);
    expect(setter).not.toHaveBeenCalled();
  });

  it('should not allow multiple decimal points', () => {
    const setter = vi.fn();
    handleNumericInput('123.45.67', setter);
    expect(setter).not.toHaveBeenCalled();
  });
});

describe('handleIntegerInput', () => {
  it('should allow valid integer input', () => {
    const setter = vi.fn();
    handleIntegerInput('123', setter);
    expect(setter).toHaveBeenCalledWith('123');
  });

  it('should allow empty string', () => {
    const setter = vi.fn();
    handleIntegerInput('', setter);
    expect(setter).toHaveBeenCalledWith('');
  });

  it('should not allow decimal points', () => {
    const setter = vi.fn();
    handleIntegerInput('123.45', setter);
    expect(setter).not.toHaveBeenCalled();
  });

  it('should not allow non-numeric characters', () => {
    const setter = vi.fn();
    handleIntegerInput('123abc', setter);
    expect(setter).not.toHaveBeenCalled();
  });
}); 