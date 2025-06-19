/* eslint-disable import/extensions */
import i18next from 'i18next';
import LanguageDetector from 'i18next-browser-languagedetector';
import { initReactI18next } from 'react-i18next';

import enUS from './en-US/product-setting-info.json';
import thTh from './th-TH/product-setting-info.json';
import viVN from './vi-VN/product-setting-info.json';
import zhCN from './zh-CN/product-setting-info.json';
import zhTW from './zh-TW/product-setting-info.json';
import jaJP from './ja-JP/product-setting-info.json';
import koKR from './ko-KR/product-setting-info.json';

export const defaultNS = 'productSettingInfoTrans';

export const resources = {
  'en-US': { productSettingInfoTrans: enUS },
  'th-TH': { productSettingInfoTrans: thTh },
  'vi-VN': { productSettingInfoTrans: viVN },
  'zh-CN': { productSettingInfoTrans: zhCN },
  'zh-TW': { productSettingInfoTrans: zhTW },
  'ja-JP': { productSettingInfoTrans: jaJP },
  'ko-KR': { productSettingInfoTrans: koKR }
};

i18next
  .use(LanguageDetector)
  .use(initReactI18next)
  .init({
    supportedLngs: ['en-US', 'th-TH', 'vi-VN', 'zh-CN', 'zh-TW', 'ja-JP', 'ko-KR'],
    fallbackLng: 'en-US',
    load: 'currentOnly',
    resources,
    defaultNS,
  });
