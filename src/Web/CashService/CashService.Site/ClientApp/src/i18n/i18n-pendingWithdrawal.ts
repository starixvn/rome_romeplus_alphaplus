import i18n from 'i18next';
import { initReactI18next } from 'react-i18next';
import LanguageDetector from 'i18next-browser-languagedetector';

// Import global resources
import globalResources from './global-resources';

// Import pendingWithdrawal translations
import pendingWithdrawalEn from './locales/pendingWithdrawal/en.json';
import pendingWithdrawalVi from './locales/pendingWithdrawal/vi.json';
import pendingWithdrawalZhCN from './locales/pendingWithdrawal/zh-CN.json';
import pendingWithdrawalZhTW from './locales/pendingWithdrawal/zh-TW.json';
import pendingWithdrawalJa from './locales/pendingWithdrawal/ja.json';
import pendingWithdrawalKo from './locales/pendingWithdrawal/ko.json';
import pendingWithdrawalTh from './locales/pendingWithdrawal/th.json';

// Configure i18next for pendingWithdrawal
i18n
  .use(LanguageDetector)
  .use(initReactI18next)
  .init({
    resources: {
      en: {
        ...globalResources.en,
        pendingWithdrawal: pendingWithdrawalEn,
      },
      vi: {
        ...globalResources.vi,
        pendingWithdrawal: pendingWithdrawalVi,
      },
      'zh-CN': {
        ...globalResources['zh-CN'],
        pendingWithdrawal: pendingWithdrawalZhCN,
      },
      'zh-TW': {
        ...globalResources['zh-TW'],
        pendingWithdrawal: pendingWithdrawalZhTW,
      },
      ja: {
        ...globalResources.ja,
        pendingWithdrawal: pendingWithdrawalJa,
      },
      ko: {
        ...globalResources.ko,
        pendingWithdrawal: pendingWithdrawalKo,
      },
      th: {
        ...globalResources.th,
        pendingWithdrawal: pendingWithdrawalTh,
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