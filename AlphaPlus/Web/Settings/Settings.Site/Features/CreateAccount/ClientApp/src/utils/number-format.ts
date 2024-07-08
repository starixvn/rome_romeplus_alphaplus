export const getNumberOfCommaString = (commaNumber: string | number) =>
  Number(commaNumber?.toString().replaceAll(',', ''));

export const formatThousandNumber = (number: string) => {
  number = String(number).replaceAll(',', '');

  return Number(number).toLocaleString('en-US', {
    minimumFractionDigits: 0,
    maximumFractionDigits: 2,
  });
};

export const formatNumber = (number: number) => {
  return Number(Math.trunc(number * 100) / 100).toLocaleString('en-US');
};
