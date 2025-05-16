export const handleNumericInput = (value: string, setter: (value: string) => void) => {
  // Allow only numbers and decimal point
  if (/^\d*\.?\d*$/.test(value) || value === '') {
    setter(value);
  }
};

export const handleIntegerInput = (value: string, setter: (value: string) => void) => {
  // Allow only integers
  if (/^\d*$/.test(value) || value === '') {
    setter(value);
  }
};

export const ensureNumericKeyPress = (e: React.KeyboardEvent<HTMLInputElement>) => {
  if (!/[0-9]/.test(e.key)) {
    e.preventDefault();
  }
};

export const preventSpecialCharacter = (e: React.KeyboardEvent<HTMLInputElement>) => {
  // Allow letters, numbers, spaces, and basic punctuation
  if (!/^[a-zA-Z0-9\s.,!?-]$/.test(e.key)) {
    e.preventDefault();
  }
}; 