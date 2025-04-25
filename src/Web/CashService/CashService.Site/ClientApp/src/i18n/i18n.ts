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

// Import pendingDeposit translations
import pendingDepositEn from './locales/pendingDeposit/en.json';
import pendingDepositVi from './locales/pendingDeposit/vi.json';
import pendingDepositZhCN from './locales/pendingDeposit/zh-CN.json';
import pendingDepositZhTW from './locales/pendingDeposit/zh-TW.json';
import pendingDepositJa from './locales/pendingDeposit/ja.json';
import pendingDepositKo from './locales/pendingDeposit/ko.json';

// Import transactionHistory translations
import enTransactionHistory from './locales/transactionHistory/en.json';
import zhTWTransactionHistory from './locales/transactionHistory/zh-TW.json';

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
        memberSite: memberSiteEn,
        pendingDeposit: pendingDepositEn,
        transactionHistory: enTransactionHistory,
      },
      vi: {
        global: globalVi,
        managePayments: managePaymentsVi,
        memberSite: memberSiteVi,
        pendingDeposit: pendingDepositVi,
      },
      'zh-CN': {
        global: globalZhCN,
        managePayments: managePaymentsZhCN,
        memberSite: memberSiteZhCN,
        pendingDeposit: pendingDepositZhCN,
      },
      'zh-TW': {
        global: globalZhTW,
        managePayments: managePaymentsZhTW,
        memberSite: memberSiteZhTW,
        pendingDeposit: pendingDepositZhTW,
        transactionHistory: zhTWTransactionHistory,
      },
      ja: {
        global: globalJa,
        managePayments: managePaymentsJa,
        memberSite: memberSiteJa,
        pendingDeposit: pendingDepositJa,
      },
      ko: {
        global: globalKo,
        managePayments: managePaymentsKo,
        memberSite: memberSiteKo,
        pendingDeposit: pendingDepositKo,
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
