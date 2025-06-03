import i18n from 'i18next';
import { initReactI18next } from 'react-i18next';
import LanguageDetector from 'i18next-browser-languagedetector';

// Import global resources
import globalResources from './global-resources';

// Import paymentProviderSettings translations
import paymentProviderSettingsEn from './locales/paymentProviderSettings/en.json';
import paymentProviderSettingsVi from './locales/paymentProviderSettings/vi.json';
import paymentProviderSettingsZhCN from './locales/paymentProviderSettings/zh-CN.json';
import paymentProviderSettingsZhTW from './locales/paymentProviderSettings/zh-TW.json';
import paymentProviderSettingsJa from './locales/paymentProviderSettings/ja.json';
import paymentProviderSettingsKo from './locales/paymentProviderSettings/ko.json';
import paymentProviderSettingsTh from './locales/paymentProviderSettings/th.json';

// Configure i18next for paymentProviderSettings
i18n
  .use(LanguageDetector)
  .use(initReactI18next)
  .init({
    resources: {
      en: {
        ...globalResources.en,
        paymentProviderSettings: paymentProviderSettingsEn,
      },
      vi: {
        ...globalResources.vi,
        paymentProviderSettings: paymentProviderSettingsVi,
      },
      'zh-CN': {
        ...globalResources['zh-CN'],
        paymentProviderSettings: paymentProviderSettingsZhCN,
      },
      'zh-TW': {
        ...globalResources['zh-TW'],
        paymentProviderSettings: paymentProviderSettingsZhTW,
      },
      ja: {
        ...globalResources.ja,
        paymentProviderSettings: paymentProviderSettingsJa,
      },
      ko: {
        ...globalResources.ko,
        paymentProviderSettings: paymentProviderSettingsKo,
      },
      th: {
        ...globalResources.th,
        paymentProviderSettings: paymentProviderSettingsTh,
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