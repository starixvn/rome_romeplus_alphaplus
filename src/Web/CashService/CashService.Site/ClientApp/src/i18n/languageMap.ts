// List of supported languages
export const supportedLanguages = ['en', 'vi', 'zh-CN', 'zh-TW', 'ja', 'ko', 'th'] as const;

// Language mapping function
export const mapLanguageCode = (lang: string | undefined): string | undefined => {
  if (!lang) return undefined;

  // Normalize to lowercase for comparison
  const normalizedLang = lang.toLowerCase();

  // Direct mappings
  const languageMap: { [key: string]: string } = {
    'zh': 'zh-CN',     // Default Chinese to Simplified Chinese
    'zh-hans': 'zh-CN', // Simplified Chinese
    'zh-hant': 'zh-TW', // Traditional Chinese
    'zh_cn': 'zh-CN',
    'zh_tw': 'zh-TW',
    'ko-kr': 'ko',
    'ja-jp': 'ja',
    'th-th': 'th',
    'vi-vn': 'vi'
  };

  // Check for direct matches with supported languages
  if (supportedLanguages.includes(lang as any)) {
    return lang;
  }

  // Check language map
  return languageMap[normalizedLang] || 'en'; // Default to English if no match found
}; 