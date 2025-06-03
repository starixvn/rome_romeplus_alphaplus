import i18n from 'i18next';
import { initReactI18next } from 'react-i18next';
import LanguageDetector from 'i18next-browser-languagedetector';

// Import global resources
import globalResources from './global-resources';

// Import ManagePayments specific translations
import managePaymentsEn from './locales/managePayments/en.json';
import managePaymentsVi from './locales/managePayments/vi.json';
import managePaymentsZhCN from './locales/managePayments/zh-CN.json';
import managePaymentsZhTW from './locales/managePayments/zh-TW.json';
import managePaymentsJa from './locales/managePayments/ja.json';
import managePaymentsKo from './locales/managePayments/ko.json';
import managePaymentsTh from './locales/managePayments/th.json';

// Configure i18next for managePayments
i18n
  .use(LanguageDetector)
  .use(initReactI18next)
  .init({
    resources: {
      en: {
        ...globalResources.en,
        managePayments: managePaymentsEn,
      },
      vi: {
        ...globalResources.vi,
        managePayments: managePaymentsVi,
      },
      'zh-CN': {
        ...globalResources['zh-CN'],
        managePayments: managePaymentsZhCN,
      },
      'zh-TW': {
        ...globalResources['zh-TW'],
        managePayments: managePaymentsZhTW,
      },
      ja: {
        ...globalResources.ja,
        managePayments: managePaymentsJa,
      },
      ko: {
        ...globalResources.ko,
        managePayments: managePaymentsKo,
      },
      th: {
        ...globalResources.th,
        managePayments: managePaymentsTh,
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