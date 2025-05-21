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

export const ensureNumericInput = (e: React.ChangeEvent<HTMLInputElement>) => {
  var orgValue = e.target.value;
  const value = orgValue.replace(/[^0-9]/g, '');
  e.preventDefault();
  e.target.value = value;
  return e;
};

export const ensureNameInput = (e: React.ChangeEvent<HTMLInputElement>) => {
  var orgValue = e.target.value;
  const value = orgValue.replace(/[0-9]/g, '');
  e.preventDefault();
  e.target.value = value;
  return e;
};
