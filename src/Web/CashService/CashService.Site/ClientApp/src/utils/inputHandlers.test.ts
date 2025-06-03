import React from 'react';
import { describe, it, expect } from 'vitest';
import { ensureIntegerInput, ensureDecimalInput, isDecimalInput } from './inputHandlers';

describe('Input Handlers', () => {
  describe('ensureIntegerInput', () => {
    it('should remove non-numeric characters', () => {
      const mockEvent = {
        target: { value: '123abc456' },
        preventDefault: () => {}
      } as React.ChangeEvent<HTMLInputElement>;

      const result = ensureIntegerInput(mockEvent);
      expect(result.target.value).toBe('123456');
    });

    it('should handle empty input', () => {
      const mockEvent = {
        target: { value: '' },
        preventDefault: () => {}
      } as React.ChangeEvent<HTMLInputElement>;

      const result = ensureIntegerInput(mockEvent);
      expect(result.target.value).toBe('');
    });

    it('should handle input with only non-numeric characters', () => {
      const mockEvent = {
        target: { value: 'abc' },
        preventDefault: () => {}
      } as React.ChangeEvent<HTMLInputElement>;

      const result = ensureIntegerInput(mockEvent);
      expect(result.target.value).toBe('');
    });
  });

  describe('ensureDecimalInput', () => {
    it('should allow valid decimal numbers', () => {
      const mockEvent = {
        target: { value: '123.45' },
        preventDefault: () => {}
      } as React.ChangeEvent<HTMLInputElement>;

      const result = ensureDecimalInput(mockEvent);
      expect(result.target.value).toBe('123.45');
    });

    it('should remove multiple decimal points', () => {
      const mockEvent = {
        target: { value: '123.45.67' },
        preventDefault: () => {}
      } as React.ChangeEvent<HTMLInputElement>;

      const result = ensureDecimalInput(mockEvent);
      expect(result.target.value).toBe('123.4567');
    });

    it('should remove non-numeric characters except decimal point', () => {
      const mockEvent = {
        target: { value: '123.45abc' },
        preventDefault: () => {}
      } as React.ChangeEvent<HTMLInputElement>;

      const result = ensureDecimalInput(mockEvent);
      expect(result.target.value).toBe('123.45');
    });

    it('should handle empty input', () => {
      const mockEvent = {
        target: { value: '' },
        preventDefault: () => {}
      } as React.ChangeEvent<HTMLInputElement>;

      const result = ensureDecimalInput(mockEvent);
      expect(result.target.value).toBe('');
    });
  });

  describe('isDecimalInput', () => {
    it('should return true for valid decimal numbers', () => {
      expect(isDecimalInput('123.45')).toBe(true);
      expect(isDecimalInput('0.123')).toBe(true);
      expect(isDecimalInput('123')).toBe(true);
    });

    it('should return false for invalid decimal numbers', () => {
      expect(isDecimalInput('123.45.67')).toBe(false);
      expect(isDecimalInput('abc')).toBe(false);
      expect(isDecimalInput('123abc')).toBe(false);
    });

    it('should handle empty string', () => {
      expect(isDecimalInput('')).toBe(true);
    });

    it('should handle trailing decimal point', () => {
      expect(isDecimalInput('123.')).toBe(true);
    });
  });
}); 