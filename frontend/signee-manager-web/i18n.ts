import i18n from 'i18next';
import { initReactI18next } from 'react-i18next';

import enTranslation from './public/locales/en.json';
import skTranslation from './public/locales/sk.json';
import csTranslation from './public/locales/cs.json';

i18n
  .use(initReactI18next)
  .init({
    resources: {
      en: { translation: enTranslation },
      sk: { translation: skTranslation },
      cs: { translation: csTranslation }
    },
    lng: 'sk', // default language
    fallbackLng: 'en', // fallback language
    interpolation: {
      escapeValue: false // react is already safe from xss
    }
  });

export default i18n;