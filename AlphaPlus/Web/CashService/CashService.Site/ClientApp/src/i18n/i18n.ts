import i18n from 'i18next';
import { initReactI18next } from 'react-i18next';
import LanguageDetector from 'i18next-browser-languagedetector';

// Import ManagePayments specific translations
import managePaymentsEn from './locales/managePayments/en.json';
import managePaymentsVi from './locales/managePayments/vi.json';
import managePaymentsZhCN from './locales/managePayments/zh-CN.json';
import managePaymentsZhTW from './locales/managePayments/zh-TW.json';
import managePaymentsJa from './locales/managePayments/ja.json';
import managePaymentsKo from './locales/managePayments/ko.json';

// Configure i18next
i18n
  // Detect user language
  .use(LanguageDetector)
  // Pass the i18n instance to react-i18next
  .use(initReactI18next)
  // Initialize i18next
  .init({
    resources: {
      en: {
        managePayments: managePaymentsEn,
      },
      vi: {
        managePayments: managePaymentsVi,
      },
      'zh-CN': {
        managePayments: managePaymentsZhCN,
      },
      'zh-TW': {
        managePayments: managePaymentsZhTW,
      },
      ja: {
        managePayments: managePaymentsJa,
      },
      ko: {
        managePayments: managePaymentsKo,
      },
    },
    fallbackLng: 'en',
    debug: process.env.NODE_ENV === 'development',
    
    interpolation: {
      escapeValue: false, // React already escapes values
    },
    
    detection: {
      order: ['localStorage', 'navigator'],
      caches: ['localStorage'],
    },
  });

export default i18n; 