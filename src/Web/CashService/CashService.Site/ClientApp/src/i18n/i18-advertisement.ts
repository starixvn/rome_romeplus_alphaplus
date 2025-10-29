import i18n from 'i18next';
import { initReactI18next } from 'react-i18next';
import LanguageDetector from 'i18next-browser-languagedetector';

import globalResources from './global-resources';

import advertisementEn from './locales/advertisement/en.json';
import advertisementVi from './locales/advertisement/vi.json';
import advertisementZhCN from './locales/advertisement/zh-CN.json';
import advertisementZhTW from './locales/advertisement/zh-TW.json';
import advertisementJa from './locales/advertisement/ja.json';
import advertisementKo from './locales/advertisement/ko.json';
import advertisementTh from './locales/advertisement/th.json';

i18n
  .use(LanguageDetector)
  .use(initReactI18next)
  .init({
    resources: {
      en: {
        ...globalResources.en,
        advertisement: advertisementEn,
      },
      vi: {
        ...globalResources.vi,
        advertisement: advertisementVi,
      },
      'zh-CN': {
        ...globalResources['zh-CN'],
        advertisement: advertisementZhCN,
      },
      'zh-TW': {
        ...globalResources['zh-TW'],
        advertisement: advertisementZhTW,
      },
      ja: {
        ...globalResources.ja,
        advertisement: advertisementJa,
      },
      ko: {
        ...globalResources.ko,
        advertisement: advertisementKo,
      },
      th: {
        ...globalResources.th,
        advertisement: advertisementTh,
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