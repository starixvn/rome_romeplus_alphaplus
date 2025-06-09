import { describe, it, expect, vi, beforeEach, afterEach, Mock } from 'vitest';
import { onBackNavigation, compareObjects, reloadPage, splitMobile, isNumberIncludes } from './sharedUtils';

describe('sharedUtils', () => {
  describe('onBackNavigation', () => {
    beforeEach(() => {
      // Mock localStorage
      const localStorageMock = {
        getItem: vi.fn().mockImplementation((key: string) => null),
        removeItem: vi.fn(),
      };
      Object.defineProperty(window, 'localStorage', { value: localStorageMock });
      
      // Mock window.location
      Object.defineProperty(window, 'location', {
        value: { href: '' },
        writable: true,
      });
      
      // Mock window.history
      Object.defineProperty(window, 'history', {
        value: { back: vi.fn() },
        writable: true,
      });
    });

    afterEach(() => {
      vi.clearAllMocks();
    });

    it('should navigate to previous URL if it exists in localStorage', () => {
      const previousUrl = 'https://example.com';
      (localStorage.getItem as Mock).mockReturnValue(previousUrl);
      
      onBackNavigation();
      
      expect(localStorage.getItem).toHaveBeenCalledWith('cashsite_previousUrl');
      expect(localStorage.removeItem).toHaveBeenCalledWith('cashsite_previousUrl');
      expect(window.location.href).toBe(previousUrl);
    });

    it('should call window.history.back() if no previous URL exists', () => {
      (localStorage.getItem as Mock).mockReturnValue(null);
      
      onBackNavigation();
      
      expect(localStorage.getItem).toHaveBeenCalledWith('cashsite_previousUrl');
      expect(window.history.back).toHaveBeenCalled();
    });
  });

  describe('compareObjects', () => {
    it('should return false for identical objects', () => {
      const obj1 = { a: 1, b: 2 };
      const obj2 = { a: 1, b: 2 };
      expect(compareObjects(obj1, obj2)).toBe(false);
    });

    it('should return true for different objects', () => {
      const obj1 = { a: 1, b: 2 };
      const obj2 = { a: 1, b: 3 };
      expect(compareObjects(obj1, obj2)).toBe(true);
    });

    it('should handle nested objects', () => {
      const obj1 = { a: { b: 1 }, c: 2 };
      const obj2 = { a: { b: 1 }, c: 2 };
      const obj3 = { a: { b: 2 }, c: 2 };
      
      expect(compareObjects(obj1, obj2)).toBe(false);
      expect(compareObjects(obj1, obj3)).toBe(true);
    });

    it('should handle null and undefined values', () => {
      expect(compareObjects(null, {})).toBe(true);
      expect(compareObjects({}, null)).toBe(true);
      expect(compareObjects(undefined, {})).toBe(true);
      expect(compareObjects({}, undefined)).toBe(true);
    });
  });

  describe('splitMobile', () => {
    it('should split mobile number with country code', () => {
      const result = splitMobile('+1-1234567890');
      expect(result).toEqual({
        countryCode: '+1',
        mobile: '1234567890'
      });
    });

    it('should handle mobile number without country code', () => {
      const result = splitMobile('1234567890');
      expect(result).toEqual({
        countryCode: '',
        mobile: '1234567890'
      });
    });
  });

  describe('isNumberIncludes', () => {
    it('should return true if number is included in comma-separated string', () => {
      expect(isNumberIncludes('1,2,3', 2)).toBe(true);
      expect(isNumberIncludes('1,2,3', 4)).toBe(false);
    });

    it('should handle single number string', () => {
      expect(isNumberIncludes('1', 1)).toBe(true);
      expect(isNumberIncludes('1', 2)).toBe(false);
    });

    it('should handle empty string', () => {
      expect(isNumberIncludes('', 1)).toBe(false);
    });
  });
}); 