/* eslint-disable import/extensions */
import i18next from 'i18next';
import LanguageDetector from 'i18next-browser-languagedetector';
import { initReactI18next } from 'react-i18next';

import enUS from './en-US/pt-template.json';
import thTh from './th-TH/pt-template.json';
import viVN from './vi-VN/pt-template.json';
import zhCN from './zh-CN/pt-template.json';
import zhTW from './zh-TW/pt-template.json';

export const defaultNS = 'ptTemplate';

export const resources = {
  'en-US': { ptTemplate: enUS },
  'th-TH': { ptTemplate: thTh },
  'vi-VN': { ptTemplate: viVN },
  'zh-CN': { ptTemplate: zhCN },
  'zh-TW': { ptTemplate: zhTW },
};

i18next
  .use(LanguageDetector)
  .use(initReactI18next)
  .init({
    debug: true,
    supportedLngs: ['en-US', 'th-TH', 'vi-VN', 'zh-CN', 'zh-TW'],
    fallbackLng: 'en-US',
    load: 'currentOnly',
    resources,
    defaultNS,
  });
