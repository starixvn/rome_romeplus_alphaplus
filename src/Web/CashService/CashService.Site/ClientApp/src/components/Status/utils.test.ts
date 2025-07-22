import { describe, it, expect, vi } from 'vitest';
import { 
  validateStatusType, 
  safeTranslate, 
  isValidText, 
  getStatusClasses 
} from './utils';
import { StatusType } from './types';

describe('Status Utils', () => {
  describe('validateStatusType', () => {
    it('returns valid status types as-is', () => {
      const validTypes: StatusType[] = ['completed', 'failed', 'disabled', 'processing', 'maintenance'];
      
      validTypes.forEach(type => {
        expect(validateStatusType(type)).toBe(type);
      });
    });

    it('returns completed for invalid types with warning', () => {
      const consoleSpy = vi.spyOn(console, 'warn').mockImplementation(() => {});
      
      expect(validateStatusType('invalid')).toBe('completed');
      expect(consoleSpy).toHaveBeenCalledWith('Invalid status type: invalid. Falling back to \'completed\'');
      
      expect(validateStatusType('')).toBe('completed');
      expect(validateStatusType('random')).toBe('completed');
      
      consoleSpy.mockRestore();
    });
  });

  describe('safeTranslate', () => {
    it('returns translated text when translation succeeds', () => {
      const mockT = vi.fn((key: string) => {
        const translations: Record<string, string> = {
          'completed': 'Hoàn thành',
          'failed': 'Thất bại'
        };
        return translations[key] || key;
      });

      expect(safeTranslate(mockT, 'completed')).toBe('Hoàn thành');
      expect(safeTranslate(mockT, 'failed')).toBe('Thất bại');
    });

    it('returns key when translation returns the same key', () => {
      const mockT = vi.fn((key: string) => key); // Simulates missing translation
      
      expect(safeTranslate(mockT, 'missing_key')).toBe('missing_key');
    });

    it('returns key when translation throws error', () => {
      const consoleSpy = vi.spyOn(console, 'warn').mockImplementation(() => {});
      const mockT = vi.fn(() => {
        throw new Error('Translation service error');
      });

      expect(safeTranslate(mockT, 'test_key')).toBe('test_key');
      expect(consoleSpy).toHaveBeenCalledWith('Translation error for key \'test_key\':', expect.any(Error));
      
      consoleSpy.mockRestore();
    });
  });

  describe('isValidText', () => {
    it('returns true for valid text', () => {
      expect(isValidText('Hello')).toBe(true);
      expect(isValidText('   Hello   ')).toBe(true);
      expect(isValidText('123')).toBe(true);
      expect(isValidText('!@#$%')).toBe(true);
    });

    it('returns false for invalid text', () => {
      expect(isValidText('')).toBe(false);
      expect(isValidText('   ')).toBe(false);
      expect(isValidText(null)).toBe(false);
      expect(isValidText(undefined)).toBe(false);
    });
  });

  describe('getStatusClasses', () => {
    it('returns correct classes for each status type', () => {
      const baseClasses = 'inline-block font-semibold rounded-[4px] min-w-[90px] text-center px-[10px] py-[3px]';
      
      expect(getStatusClasses('completed')).toBe(`${baseClasses} bg-[#D4E7D9] dark:bg-[#24853e]`);
      expect(getStatusClasses('failed')).toBe(`${baseClasses} bg-[#E0E0E0] dark:bg-[#6f6f6f]`);
      expect(getStatusClasses('disabled')).toBe(`${baseClasses} bg-[#FBD8D8] dark:bg-[#af4646]`);
      expect(getStatusClasses('processing')).toBe(`${baseClasses} bg-[#90d3ff] dark:bg-[#2a668d]`);
      expect(getStatusClasses('maintenance')).toBe(`${baseClasses} bg-[#fcae7b] dark:bg-[#955e39]`);
    });

    it('returns completed classes for unknown types', () => {
      const baseClasses = 'inline-block font-semibold rounded-[4px] min-w-[90px] text-center px-[10px] py-[3px]';
      
      expect(getStatusClasses('unknown' as StatusType)).toBe(`${baseClasses} bg-[#D4E7D9] dark:bg-[#24853e]`);
    });
  });
}); 