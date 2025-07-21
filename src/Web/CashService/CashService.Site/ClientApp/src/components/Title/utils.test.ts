import { describe, it, expect, vi } from 'vitest';
import { 
  validateTitleType, 
  isValidText, 
  getTitleClasses 
} from './utils';
import { TitleType } from './types';

describe('Title Utils', () => {
  describe('validateTitleType', () => {
    it('returns valid title types as-is', () => {
      const validTypes: TitleType[] = ['main', 'medium', 'normal', 'label'];
      
      validTypes.forEach(type => {
        expect(validateTitleType(type)).toBe(type);
      });
    });

    it('returns main for invalid types with warning', () => {
      const consoleSpy = vi.spyOn(console, 'warn').mockImplementation(() => {});
      
      expect(validateTitleType('invalid')).toBe('main');
      expect(consoleSpy).toHaveBeenCalledWith('Invalid title type: invalid. Falling back to \'main\'');
      
      expect(validateTitleType('')).toBe('main');
      expect(validateTitleType('random')).toBe('main');
      
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

  describe('getTitleClasses', () => {
    it('returns correct classes for each title type', () => {
      expect(getTitleClasses('main')).toContain('mb-[12px]');
      expect(getTitleClasses('main')).toContain('flex');
      expect(getTitleClasses('main')).toContain('text-[14px]');
      expect(getTitleClasses('main')).toContain('font-semibold');
      expect(getTitleClasses('main')).toContain('uppercase');
      
      expect(getTitleClasses('medium')).toContain('w-[100%]');
      expect(getTitleClasses('medium')).toContain('font-semibold');
      expect(getTitleClasses('medium')).toContain('text-[14px]');
      expect(getTitleClasses('medium')).toContain('capitalize');
      
      expect(getTitleClasses('normal')).toContain('py-[8px]');
      expect(getTitleClasses('normal')).toContain('font-semibold');
      expect(getTitleClasses('normal')).toContain('capitalize');
      
      expect(getTitleClasses('label')).toContain('block');
      expect(getTitleClasses('label')).toContain('font-semibold');
      expect(getTitleClasses('label')).toContain('mb-[4px]');
      expect(getTitleClasses('label')).toContain('capitalize');
    });

    it('combines custom className with base classes', () => {
      const customClass = 'custom-class';
      const result = getTitleClasses('main', customClass);
      
      expect(result).toContain(customClass);
      expect(result).toContain('mb-[12px]');
      expect(result).toContain('flex');
    });

    it('returns main classes for unknown types', () => {
      const result = getTitleClasses('unknown' as TitleType);
      
      expect(result).toContain('mb-[12px]');
      expect(result).toContain('flex');
      expect(result).toContain('text-[14px]');
      expect(result).toContain('font-semibold');
    });

    it('handles empty className correctly', () => {
      const result = getTitleClasses('main', '');
      
      expect(result).toContain('mb-[12px]');
      expect(result).toContain('flex');
      expect(result).not.toContain('undefined');
    });
  });
}); 