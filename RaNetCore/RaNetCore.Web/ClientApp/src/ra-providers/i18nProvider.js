import polyglotI18nProvider from 'ra-i18n-polyglot'
import englishMessages from 'ra-language-english'

import { englishDomain } from './language-packs'

const messages = {
    en: { ...englishMessages, ...englishDomain },
}

export default polyglotI18nProvider(
    (locale) => messages[locale],
    'en', // Default locale
    {
        allowMissing: true, // Silince missing translations warnings
    }
)
