export const ensureIntegerInput = (e: React.ChangeEvent<HTMLInputElement>) => {
  var orgValue = e.target.value;
  const value = orgValue.replace(/[^0-9]/g, '');
  e.preventDefault();
  e.target.value = value;
  return e;
};  

export const ensureDecimalInput = (e: React.ChangeEvent<HTMLInputElement>) => {
  var orgValue = e.target.value;
  if(!isDecimalInput(orgValue)) {    
    // Allow only one decimal point and numbers
    const value = orgValue.replace(/[^0-9.]/g, '').replace(/(\..*)\./g, '$1');
    e.preventDefault();
    e.target.value = value;
  }

  return e;
};

export const ensureNameInput = (e: React.ChangeEvent<HTMLInputElement>) => {
  var orgValue = e.target.value;
  const value = orgValue.replace(/[0-9]/g, '');
  e.preventDefault();
  e.target.value = value;
  return e;
};

export const isDecimalInput = (value: string): boolean => {
  var v = value.endsWith('.') ? value.slice(0, -1) : value;
  return !Number.isNaN(Number(v));
};