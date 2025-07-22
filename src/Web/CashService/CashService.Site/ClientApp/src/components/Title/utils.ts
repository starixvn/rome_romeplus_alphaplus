import { TitleType } from './types';

/**
 * Validates title type and returns a safe type
 * @param type - The title type to validate
 * @returns A valid title type, defaults to 'main' if invalid
 */
export const validateTitleType = (type: string): TitleType => {
  const validTypes: TitleType[] = ['main', 'medium', 'normal', 'label'];
  
  if (!validTypes.includes(type as TitleType)) {
    console.warn(`Invalid title type: ${type}. Falling back to 'main'`);
    return 'main';
  }
  
  return type as TitleType;
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
 * Gets the appropriate CSS classes for a title type
 * @param type - The title type
 * @param className - Additional custom classes
 * @returns CSS classes string
 */
export const getTitleClasses = (type: TitleType, className: string = ''): string => {
  const baseClasses = className ? `${className} ` : '';
  
  switch (type) {
    case 'main':
      return `${baseClasses}mb-[12px] flex text-[14px] font-semibold items-center uppercase text-[var(--titleComponent-color)] border-b border-[var(--titleComponent-border-color)] leading-[39px]`;
    case 'medium':
      return `${baseClasses}w-[100%] font-semibold text-[14px] text-[#094980] dark:text-[#459be5] capitalize`;
    case 'normal':
      return `${baseClasses}py-[8px] text-[#094980] dark:text-[#459be5] font-semibold capitalize`;
    case 'label':
      return `${baseClasses}block font-semibold text-[var(--titleComponent-label-color)] mb-[4px] capitalize`;
    default:
      return `${baseClasses}mb-[12px] flex text-[14px] font-semibold items-center uppercase text-[var(--titleComponent-color)] border-b border-[var(--titleComponent-border-color)] leading-[39px]`;
  }
}; 