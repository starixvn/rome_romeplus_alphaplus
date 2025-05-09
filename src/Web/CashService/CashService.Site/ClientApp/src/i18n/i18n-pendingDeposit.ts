import i18n from 'i18next';
import { initReactI18next } from 'react-i18next';
import LanguageDetector from 'i18next-browser-languagedetector';

// Import global resources
import globalResources from './global-resources';

// Import pendingDeposit translations
import pendingDepositEn from './locales/pendingDeposit/en.json';
import pendingDepositVi from './locales/pendingDeposit/vi.json';
import pendingDepositZhCN from './locales/pendingDeposit/zh-CN.json';
import pendingDepositZhTW from './locales/pendingDeposit/zh-TW.json';
import pendingDepositJa from './locales/pendingDeposit/ja.json';
import pendingDepositKo from './locales/pendingDeposit/ko.json';
import pendingDepositTh from './locales/pendingDeposit/th.json';

// Configure i18next for pendingDeposit
i18n
  .use(LanguageDetector)
  .use(initReactI18next)
  .init({
    resources: {
      en: {
        ...globalResources.en,
        pendingDeposit: pendingDepositEn,
      },
      vi: {
        ...globalResources.vi,
        pendingDeposit: pendingDepositVi,
      },
      'zh-CN': {
        ...globalResources['zh-CN'],
        pendingDeposit: pendingDepositZhCN,
      },
      'zh-TW': {
        ...globalResources['zh-TW'],
        pendingDeposit: pendingDepositZhTW,
      },
      ja: {
        ...globalResources.ja,
        pendingDeposit: pendingDepositJa,
      },
      ko: {
        ...globalResources.ko,
        pendingDeposit: pendingDepositKo,
      },
      th: {
        ...globalResources.th,
        pendingDeposit: pendingDepositTh,
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