import React from 'react';
import { describe, it, expect, vi } from 'vitest';
import { handleNumericInput, handleIntegerInput, ensureNumericInput, ensureNameInput } from './inputHandlers';

describe('Input Handlers', () => {
  describe('handleNumericInput', () => {
    it('should allow valid numeric input with decimal point', () => {
      const setter = vi.fn();
      handleNumericInput('123.45', setter);
      expect(setter).toHaveBeenCalledWith('123.45');
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

  describe('ensureNumericInput', () => {
    it('should remove non-numeric characters', () => {
      const event = {
        target: { value: '123abc' },
        preventDefault: vi.fn()
      } as unknown as React.ChangeEvent<HTMLInputElement>;

      const result = ensureNumericInput(event);
      expect(result.target.value).toBe('123');
      expect(result.preventDefault).toHaveBeenCalled();
    });

    it('should keep only numeric characters', () => {
      const event = {
        target: { value: 'abc123def456' },
        preventDefault: vi.fn()
      } as unknown as React.ChangeEvent<HTMLInputElement>;

      const result = ensureNumericInput(event);
      expect(result.target.value).toBe('123456');
      expect(result.preventDefault).toHaveBeenCalled();
    });
  });

  describe('ensureNameInput', () => {
    it('should remove numeric characters', () => {
      const event = {
        target: { value: 'John123' },
        preventDefault: vi.fn()
      } as unknown as React.ChangeEvent<HTMLInputElement>;

      const result = ensureNameInput(event);
      expect(result.target.value).toBe('John');
      expect(result.preventDefault).toHaveBeenCalled();
    });

    it('should keep only non-numeric characters', () => {
      const event = {
        target: { value: '123John456Doe789' },
        preventDefault: vi.fn()
      } as unknown as React.ChangeEvent<HTMLInputElement>;

      const result = ensureNameInput(event);
      expect(result.target.value).toBe('JohnDoe');
      expect(result.preventDefault).toHaveBeenCalled();
    });
  });
}); 