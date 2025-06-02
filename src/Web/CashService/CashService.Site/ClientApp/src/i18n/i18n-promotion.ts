import i18n from 'i18next';
import { initReactI18next } from 'react-i18next';
import LanguageDetector from 'i18next-browser-languagedetector';

// Import global resources
import globalResources from './global-resources';

// Import promotion translations
import promotionEn from './locales/promotion/en.json';
import promotionVi from './locales/promotion/vi.json';
import promotionZhCN from './locales/promotion/zh-CN.json';
import promotionZhTW from './locales/promotion/zh-TW.json';
import promotionJa from './locales/promotion/ja.json';
import promotionKo from './locales/promotion/ko.json';
import promotionTh from './locales/promotion/th.json';

// Configure i18next for promotion
i18n
  .use(LanguageDetector)
  .use(initReactI18next)
  .init({
    resources: {
      en: {
        ...globalResources.en,
        promotion: promotionEn,
      },
      vi: {
        ...globalResources.vi,
        promotion: promotionVi,
      },
      'zh-CN': {
        ...globalResources['zh-CN'],
        promotion: promotionZhCN,
      },
      'zh-TW': {
        ...globalResources['zh-TW'],
        promotion: promotionZhTW,
      },
      ja: {
        ...globalResources.ja,
        promotion: promotionJa,
      },
      ko: {
        ...globalResources.ko,
        promotion: promotionKo,
      },
      th: {
        ...globalResources.th,
        promotion: promotionTh,
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