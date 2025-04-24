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

// Import global translations
import globalEn from './locales/global/en.json';
import globalJa from './locales/global/ja.json';
import globalKo from './locales/global/ko.json';
import globalVi from './locales/global/vi.json';
import globalZhCN from './locales/global/zh-CN.json';
import globalZhTW from './locales/global/zh-TW.json';

// Import memberSite translations
import memberSiteEn from './locales/memberSite/en.json';
import memberSiteVi from './locales/memberSite/vi.json';
import memberSiteZhCN from './locales/memberSite/zh-CN.json';
import memberSiteZhTW from './locales/memberSite/zh-TW.json';
import memberSiteJa from './locales/memberSite/ja.json';
import memberSiteKo from './locales/memberSite/ko.json';

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
        global: globalEn,
        managePayments: managePaymentsEn,
        memberSite: memberSiteEn
      },
      vi: {
        global: globalVi,
        managePayments: managePaymentsVi,
        memberSite: memberSiteVi
      },
      'zh-CN': {
        global: globalZhCN,
        managePayments: managePaymentsZhCN,
        memberSite: memberSiteZhCN
      },
      'zh-TW': {
        global: globalZhTW,
        managePayments: managePaymentsZhTW,
        memberSite: memberSiteZhTW
      },
      ja: {
        global: globalJa,
        managePayments: managePaymentsJa,
        memberSite: memberSiteJa
      },
      ko: {
        global: globalKo,
        managePayments: managePaymentsKo,
        memberSite: memberSiteKo
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