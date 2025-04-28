import { useEffect } from 'react';
import i18n from '../../../i18n/i18n';

function getCookie(name: string) {
  const value = `; ${document.cookie}`;
  const parts = value.split(`; ${name}=`);
  if (parts.length === 2) return parts.pop()!.split(';').shift();
  return undefined;
}

export function useCookieLanguage(cookieName = 'LANGUAGE') {
  useEffect(() => {
    const cookieLang = getCookie(cookieName);
    if (cookieLang) {
      i18n.changeLanguage(cookieLang);
    }
  }, [cookieName]);
}