import i18n from 'i18next';
import { initReactI18next } from 'react-i18next';
import LanguageDetector from 'i18next-browser-languagedetector';

// Import global resources
import globalResources from './global-resources';

import enFile from './locales/customerSites/en.json';
import viFile from './locales/customerSites/vi.json';
import zhCNFile from './locales/customerSites/zh-CN.json';
import zhTWFile from './locales/customerSites/zh-TW.json';
import jaFile from './locales/customerSites/ja.json';
import koFile from './locales/customerSites/ko.json';
import thFile from './locales/customerSites/th.json';

// Configure i18next for i18n
i18n
  .use(LanguageDetector)
  .use(initReactI18next)
  .init({
    resources: {
      en: {
        ...globalResources.en,
        customerSites: enFile,
      },
      vi: {
        ...globalResources.vi,
        customerSites: viFile,
      },
      'zh-CN': {
        ...globalResources['zh-CN'],
        customerSites: zhCNFile,
      },
      'zh-TW': {
        ...globalResources['zh-TW'],
        customerSites: zhTWFile,
      },
      ja: {
        ...globalResources.ja,
        customerSites: jaFile,
      },
      ko: {
        ...globalResources.ko,
        customerSites: koFile,
      },
      th: {
        ...globalResources.th,
        customerSites: thFile,
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