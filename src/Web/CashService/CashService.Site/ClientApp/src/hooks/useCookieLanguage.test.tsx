import { renderHook } from '@testing-library/react';
import { describe, it, expect, beforeEach, afterAll, vi } from 'vitest';
import { useCookieLanguage } from './useCookieLanguage';
import i18n from '../i18n/i18n';

describe('useCookieLanguage', () => {
  const originalCookie = global.document.cookie;
  const changeLanguageSpy = vi.spyOn(i18n, 'changeLanguage');

  beforeEach(() => {
    changeLanguageSpy.mockClear();
    global.document.cookie = '';
  });

  afterAll(() => {
    global.document.cookie = originalCookie;
    changeLanguageSpy.mockRestore();
  });

  it('should not call i18n.changeLanguage if cookie is not set', () => {
    global.document.cookie = '';
    renderHook(() => useCookieLanguage());
    expect(changeLanguageSpy).not.toHaveBeenCalled();
  });

  it('should call i18n.changeLanguage with the cookie value if cookie is set', () => {
    global.document.cookie = 'LANGUAGE=fr;';
    renderHook(() => useCookieLanguage());
    expect(changeLanguageSpy).toHaveBeenCalledWith('fr');
  });

  it('should use the provided cookieName', () => {
    global.document.cookie = 'MY_LANG=de;';
    renderHook(() => useCookieLanguage('MY_LANG'));
    expect(changeLanguageSpy).toHaveBeenCalledWith('de');
  });
}); 