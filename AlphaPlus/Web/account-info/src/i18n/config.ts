import i18next from 'i18next';
import LanguageDetector from 'i18next-browser-languagedetector';
import { initReactI18next } from 'react-i18next';

import translation from './en-US/translation.json';

i18next
    .use(LanguageDetector)
    .use(initReactI18next)
    .init({
        fallbackLng: 'en-US',
        supportedLngs: ['en-US'],
        debug: true,
        interpolation: {
            escapeValue: false // react already safes from xss
        },
        resources: {
            'en-US': {
                translation
            }
        }
    });
