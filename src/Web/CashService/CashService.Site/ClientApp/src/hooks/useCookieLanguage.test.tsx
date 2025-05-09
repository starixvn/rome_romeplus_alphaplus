import { describe, it, expect, vi, beforeEach } from 'vitest';
import { renderHook } from '@testing-library/react';
import { useCookieLanguage } from './useCookieLanguage';

describe('useCookieLanguage', () => {
  const mockI18nInstance = {
    changeLanguage: vi.fn(),
  };

  beforeEach(() => {
    // Reset mocks before each test
    vi.clearAllMocks();
    // Reset document.cookie
    document.cookie = '';
  });

  it('should not change language when no cookie is present', () => {
    renderHook(() => useCookieLanguage(mockI18nInstance));
    expect(mockI18nInstance.changeLanguage).not.toHaveBeenCalled();
  });

  it('should change language when cookie is present', () => {
    // Set a test cookie
    document.cookie = 'LANGUAGE=fr';
    
    renderHook(() => useCookieLanguage(mockI18nInstance));
    
    expect(mockI18nInstance.changeLanguage).toHaveBeenCalledWith('fr');
  });

  it('should use custom cookie name when provided', () => {
    // Set a test cookie with custom name
    document.cookie = 'CUSTOM_LANG=es';
    
    renderHook(() => useCookieLanguage(mockI18nInstance, 'CUSTOM_LANG'));
    
    expect(mockI18nInstance.changeLanguage).toHaveBeenCalledWith('es');
  });

  it('should handle multiple cookies correctly', () => {
    // Set multiple cookies
    document.cookie = 'LANGUAGE=de';
    document.cookie = 'OTHER_COOKIE=value';
    
    renderHook(() => useCookieLanguage(mockI18nInstance));
    
    expect(mockI18nInstance.changeLanguage).toHaveBeenCalledWith('de');
  });

  it('should handle malformed cookie values', () => {
    // Set a malformed cookie
    document.cookie = 'LANGUAGE=';
    
    renderHook(() => useCookieLanguage(mockI18nInstance));
    
    expect(mockI18nInstance.changeLanguage).not.toHaveBeenCalled();
  });
}); 