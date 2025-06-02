import i18n from 'i18next';
import { initReactI18next } from 'react-i18next';
import LanguageDetector from 'i18next-browser-languagedetector';

// Import global resources
import globalResources from './global-resources';

// Import memberSite translations
import memberSiteEn from './locales/memberSite/en.json';
import memberSiteVi from './locales/memberSite/vi.json';
import memberSiteZhCN from './locales/memberSite/zh-CN.json';
import memberSiteZhTW from './locales/memberSite/zh-TW.json';
import memberSiteJa from './locales/memberSite/ja.json';
import memberSiteKo from './locales/memberSite/ko.json';
import memberSiteTh from './locales/memberSite/th.json';

// Configure i18next for memberSite
i18n
  .use(LanguageDetector)
  .use(initReactI18next)
  .init({
    resources: {
      en: {
        ...globalResources.en,
        memberSite: memberSiteEn,
      },
      vi: {
        ...globalResources.vi,
        memberSite: memberSiteVi,
      },
      'zh-CN': {
        ...globalResources['zh-CN'],
        memberSite: memberSiteZhCN,
      },
      'zh-TW': {
        ...globalResources['zh-TW'],
        memberSite: memberSiteZhTW,
      },
      ja: {
        ...globalResources.ja,
        memberSite: memberSiteJa,
      },
      ko: {
        ...globalResources.ko,
        memberSite: memberSiteKo,
      },
      th: {
        ...globalResources.th,
        memberSite: memberSiteTh,
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