import { describe, it, expect } from 'vitest';
import { formatNumber, formatInputValue } from './numberFormatting';

describe('formatNumber', () => {
  it('returns empty string for null or undefined', () => {
    expect(formatNumber(null)).toBe('');
    expect(formatNumber(undefined)).toBe('');
  });

  it('formats numbers with thousands separators', () => {
    expect(formatNumber(1000)).toBe('1,000');
    expect(formatNumber(1234567)).toBe('1,234,567');
    expect(formatNumber(1234.56)).toBe('1,234.56');
  });

  it('formats numeric strings with thousands separators', () => {
    expect(formatNumber('1000')).toBe('1,000');
    expect(formatNumber('1234567')).toBe('1,234,567');
    expect(formatNumber('1234.56')).toBe('1,234.56');
  });

  it('returns "NaN" for non-numeric strings', () => {
    expect(formatNumber('abc')).toBe('NaN');
  });
});

describe('formatInputValue', () => {
  it('removes non-numeric characters except decimal point', () => {
    expect(formatInputValue('1,234.56')).toBe('1234.56');
    expect(formatInputValue('$1,000.99')).toBe('1000.99');
    expect(formatInputValue('abc123')).toBe('123');
    expect(formatInputValue('12.34.56')).toBe('12.3456');
  });

  it('handles empty string', () => {
    expect(formatInputValue('')).toBe('');
  });

  it('handles strings with only non-numeric characters', () => {
    expect(formatInputValue('abc')).toBe('');
    expect(formatInputValue('...')).toBe('.');
  });

  it('handles numbers as strings', () => {
    expect(formatInputValue('1234')).toBe('1234');
    expect(formatInputValue('1234.56')).toBe('1234.56');
  });
}); 