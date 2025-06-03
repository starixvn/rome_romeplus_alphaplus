import i18n from 'i18next';
import { initReactI18next } from 'react-i18next';
import LanguageDetector from 'i18next-browser-languagedetector';

// Import global resources
import globalResources from './global-resources';

// Import transactionHistory translations
import transactionHistoryEn from './locales/transactionHistory/en.json';
import transactionHistoryVi from './locales/transactionHistory/vi.json';
import transactionHistoryZhCN from './locales/transactionHistory/zh-CN.json';
import transactionHistoryZhTW from './locales/transactionHistory/zh-TW.json';
import transactionHistoryJa from './locales/transactionHistory/ja.json';
import transactionHistoryKo from './locales/transactionHistory/ko.json';
import transactionHistoryTh from './locales/transactionHistory/th.json';

// Configure i18next for transactionHistory
i18n
  .use(LanguageDetector)
  .use(initReactI18next)
  .init({
    resources: {
      en: {
        ...globalResources.en,
        transactionHistory: transactionHistoryEn,
      },
      vi: {
        ...globalResources.vi,
        transactionHistory: transactionHistoryVi,
      },
      'zh-CN': {
        ...globalResources['zh-CN'],
        transactionHistory: transactionHistoryZhCN,
      },
      'zh-TW': {
        ...globalResources['zh-TW'],
        transactionHistory: transactionHistoryZhTW,
      },
      ja: {
        ...globalResources.ja,
        transactionHistory: transactionHistoryJa,
      },
      ko: {
        ...globalResources.ko,
        transactionHistory: transactionHistoryKo,
      },
      th: {
        ...globalResources.th,
        transactionHistory: transactionHistoryTh,
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