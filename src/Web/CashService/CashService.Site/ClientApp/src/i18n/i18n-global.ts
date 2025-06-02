import i18n from 'i18next';
import { initReactI18next } from 'react-i18next';
import LanguageDetector from 'i18next-browser-languagedetector';

import globalEn from './locales/global/en.json';
import globalJa from './locales/global/ja.json';
import globalKo from './locales/global/ko.json';
import globalVi from './locales/global/vi.json';
import globalZhCN from './locales/global/zh-CN.json';
import globalZhTW from './locales/global/zh-TW.json';
import globalTh from './locales/global/th.json';

// Configure i18next for managePayments
i18n
  .use(LanguageDetector)
  .use(initReactI18next)
  .init({
    resources: {
      en: {
        global: globalEn,
      },
      vi: {
        global: globalVi,
      },
      'zh-CN': {
        global: globalZhCN,
      },
      'zh-TW': {
        global: globalZhTW,
      },
      ja: {
        global: globalJa,
      },
      ko: {
        global: globalKo,
      },
      th: {
        global: globalTh,
      },
    },
    fallbackLng: 'en',
    debug: process.env.NODE_ENV === 'development',
    interpolation: {
      escapeValue: false,
    },
    detection: {
      order: ['localStorage', 'navigator'],
      lookupLocalStorage: 'i18nextLng',
      lookupQuerystring: 'lang',
      caches: ['localStorage'],
    },
  });

export default i18n; 