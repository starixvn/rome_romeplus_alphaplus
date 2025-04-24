/* eslint-disable import/extensions */
import i18next from 'i18next';
import LanguageDetector from 'i18next-browser-languagedetector';
import { initReactI18next } from 'react-i18next';

import enUS from './en-US/translation.json';
import thTh from './th-TH/translation.json';
import viVN from './vi-VN/translation.json';
import zhCN from './zh-CN/translation.json';
import zhTW from './zh-TW/translation.json';
import jaJP from './ja-JP/translation.json';


export const defaultNS = 'translation';

export const resources = {
  'en-US': { translation: enUS },
  'th-TH': { translation: thTh },
  'vi-VN': { translation: viVN },
  'zh-CN': { translation: zhCN },
  'zh-TW': { translation: zhTW },
  'ja-JP': { translation: jaJP },
};

i18next
  .use(LanguageDetector)
  .use(initReactI18next)
  .init({
    supportedLngs: ['en-US', 'zh-CN', 'zh-TW', 'ja-JP', 'ko-KR', 'th-TH', 'vi-VN'],
    fallbackLng: 'en-US',
    resources,
    defaultNS,
  });
