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
import managePaymentsTh from './locales/managePayments/th.json';

// Import global translations
import globalEn from './locales/global/en.json';
import globalJa from './locales/global/ja.json';
import globalKo from './locales/global/ko.json';
import globalVi from './locales/global/vi.json';
import globalZhCN from './locales/global/zh-CN.json';
import globalZhTW from './locales/global/zh-TW.json';
import globalTh from './locales/global/th.json';

// Import memberSite translations
import memberSiteEn from './locales/memberSite/en.json';
import memberSiteVi from './locales/memberSite/vi.json';
import memberSiteZhCN from './locales/memberSite/zh-CN.json';
import memberSiteZhTW from './locales/memberSite/zh-TW.json';
import memberSiteJa from './locales/memberSite/ja.json';
import memberSiteKo from './locales/memberSite/ko.json';
import memberSiteTh from './locales/memberSite/th.json';

// Import pendingDeposit translations
import pendingDepositEn from './locales/pendingDeposit/en.json';
import pendingDepositVi from './locales/pendingDeposit/vi.json';
import pendingDepositZhCN from './locales/pendingDeposit/zh-CN.json';
import pendingDepositZhTW from './locales/pendingDeposit/zh-TW.json';
import pendingDepositJa from './locales/pendingDeposit/ja.json';
import pendingDepositKo from './locales/pendingDeposit/ko.json';
import pendingDepositTh from './locales/pendingDeposit/th.json';

// Import pendingWithdrawal translations
import pendingWithdrawalEn from './locales/pendingWithdrawal/en.json';
import pendingWithdrawalVi from './locales/pendingWithdrawal/vi.json';
import pendingWithdrawalZhCN from './locales/pendingWithdrawal/zh-CN.json';
import pendingWithdrawalZhTW from './locales/pendingWithdrawal/zh-TW.json';
import pendingWithdrawalJa from './locales/pendingWithdrawal/ja.json';
import pendingWithdrawalKo from './locales/pendingWithdrawal/ko.json';
import pendingWithdrawalTh from './locales/pendingWithdrawal/th.json';

// Import transactionHistory translations
import transactionHistoryEn from './locales/transactionHistory/en.json';
import transactionHistoryVi from './locales/transactionHistory/vi.json';
import transactionHistoryZhCN from './locales/transactionHistory/zh-CN.json';
import transactionHistoryZhTW from './locales/transactionHistory/zh-TW.json';
import transactionHistoryJa from './locales/transactionHistory/ja.json';
import transactionHistoryKo from './locales/transactionHistory/ko.json';
import transactionHistoryTh from './locales/transactionHistory/th.json';

// Import promotion translations
import promotionEn from './locales/promotion/en.json';
import promotionVi from './locales/promotion/vi.json';
import promotionZhCN from './locales/promotion/zh-CN.json';
import promotionZhTW from './locales/promotion/zh-TW.json';
import promotionJa from './locales/promotion/ja.json';
import promotionKo from './locales/promotion/ko.json';
import promotionTh from './locales/promotion/th.json';

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
        pendingWithdrawal: pendingWithdrawalEn,
        transactionHistory: transactionHistoryEn,
        promotion: promotionEn,
      },
      vi: {
        global: globalVi,
        managePayments: managePaymentsVi,
        memberSite: memberSiteVi,
        pendingDeposit: pendingDepositVi,
        pendingWithdrawal: pendingWithdrawalVi,
        transactionHistory: transactionHistoryVi,
        promotion: promotionVi,
      },
      'zh-CN': {
        global: globalZhCN,
        managePayments: managePaymentsZhCN,
        memberSite: memberSiteZhCN,
        pendingDeposit: pendingDepositZhCN,
        pendingWithdrawal: pendingWithdrawalZhCN,
        transactionHistory: transactionHistoryZhCN,
        promotion: promotionZhCN,
      },
      'zh-TW': {
        global: globalZhTW,
        managePayments: managePaymentsZhTW,
        memberSite: memberSiteZhTW,
        pendingDeposit: pendingDepositZhTW,
        pendingWithdrawal: pendingWithdrawalZhTW,
        transactionHistory: transactionHistoryZhTW,
        promotion: promotionZhTW,
      },
      ja: {
        global: globalJa,
        managePayments: managePaymentsJa,
        memberSite: memberSiteJa,
        pendingDeposit: pendingDepositJa,
        pendingWithdrawal: pendingWithdrawalJa,
        transactionHistory: transactionHistoryJa,
        promotion: promotionJa,
      },
      ko: {
        global: globalKo,
        managePayments: managePaymentsKo,
        memberSite: memberSiteKo,
        pendingDeposit: pendingDepositKo,
        pendingWithdrawal: pendingWithdrawalKo,
        transactionHistory: transactionHistoryKo,
        promotion: promotionKo,
      },
      th: {
        global: globalTh,
        managePayments: managePaymentsTh,
        memberSite: memberSiteTh,
        pendingDeposit: pendingDepositTh,
        pendingWithdrawal: pendingWithdrawalTh,
        transactionHistory: transactionHistoryTh,
        promotion: promotionTh,
      },
    },
    fallbackLng: 'en',
    debug: process.env.NODE_ENV === 'development',

    interpolation: {
      escapeValue: false, // React already escapes values
    },

    detection: {
      order: ['localStorage', 'navigator'],
      lookupLocalStorage: 'i18nextLng',
      lookupQuerystring: 'lang',
      caches: ['localStorage'],
    },
  });

export default i18n;
