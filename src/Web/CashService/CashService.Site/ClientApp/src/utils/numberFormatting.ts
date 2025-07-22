/**
 * Utility functions for number formatting
 */

/**
 * Format number with thousands separators
 * @param value - The value to format
 * @returns Formatted string with thousands separators
 */
export const formatNumber = (value: string | number | null | undefined): string => {
  if (value === null || value === undefined) return '';
  return Number(value).toLocaleString();
};

/**
 * Format input value to ensure it's a valid number
 * @param value - The input value to format
 * @returns Formatted string with only valid numeric characters
 */
export const formatInputValue = (value: string): string => {
  // Remove any non-numeric characters except decimal point
  const numericValue = value.replace(/[^\d.]/g, '');

  // Ensure only one decimal point
  const parts = numericValue.split('.');
  return parts.length > 1 ? parts[0] + '.' + parts.slice(1).join('') : numericValue;
};

export const formatDecimal = (value: number | null | undefined): string => {
  if (value === null || value === undefined) return '';
  return value.toLocaleString('en-US', { minimumFractionDigits: 2, maximumFractionDigits: 2 });
};
