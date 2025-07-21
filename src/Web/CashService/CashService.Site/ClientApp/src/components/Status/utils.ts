import { StatusType } from './types';

/**
 * Validates status type and returns a safe type
 * @param type - The status type to validate
 * @returns A valid status type, defaults to 'completed' if invalid
 */
export const validateStatusType = (type: string): StatusType => {
  const validTypes: StatusType[] = ['completed', 'failed', 'disabled', 'processing', 'maintenance'];
  
  if (!validTypes.includes(type as StatusType)) {
    console.warn(`Invalid status type: ${type}. Falling back to 'completed'`);
    return 'completed';
  }
  
  return type as StatusType;
};

/**
 * Safe translation function with error handling
 * @param t - Translation function from react-i18next
 * @param key - Translation key
 * @returns Translated text or fallback to key
 */
export const safeTranslate = (t: (key: string) => string, key: string): string => {
  try {
    const translated = t(key);
    // Check if translation returned the key itself (indicating missing translation)
    return translated === key ? key : translated;
  } catch (error) {
    console.warn(`Translation error for key '${key}':`, error);
    return key;
  }
};

/**
 * Validates text prop and handles empty/null values
 * @param text - The text to validate
 * @returns true if text is valid, false if empty/null
 */
export const isValidText = (text?: string | null): boolean => {
  return text !== null && text !== undefined && text.trim() !== '';
};

/**
 * Gets the appropriate CSS classes for a status type
 * @param type - The status type
 * @returns CSS classes string
 */
export const getStatusClasses = (type: StatusType): string => {
  const baseClasses = 'inline-block font-semibold rounded-[4px] min-w-[90px] text-center px-[10px] py-[3px]';
  
  switch (type) {
    case 'completed':
      return `${baseClasses} bg-[#D4E7D9] dark:bg-[#24853e]`;
    case 'failed':
      return `${baseClasses} bg-[#E0E0E0] dark:bg-[#6f6f6f]`;
    case 'disabled':
      return `${baseClasses} bg-[#FBD8D8] dark:bg-[#af4646]`;
    case 'processing':
      return `${baseClasses} bg-[#90d3ff] dark:bg-[#2a668d]`;
    case 'maintenance':
      return `${baseClasses} bg-[#fcae7b] dark:bg-[#955e39]`;
    default:
      return `${baseClasses} bg-[#D4E7D9] dark:bg-[#24853e]`;
  }
}; 