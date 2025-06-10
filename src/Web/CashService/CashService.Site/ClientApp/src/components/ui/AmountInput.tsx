import React, { useState, useEffect } from 'react';

interface AmountInputProps {
  value: number;
  onChange: (value: number) => void;
  isDisabled?: boolean;
  id?: string;
  className?: string;
}

export const AmountInput: React.FC<AmountInputProps> = ({
  value,
  onChange,
  className,
  isDisabled,
  id,
}) => {
  const formatNumberWithCommas = (number: string) => {
    const parts = number.split('.');
    parts[0] = parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, ',');
    return parts.join('.');
  };

  const removeCommas = (number: string) => {
    return number.replace(/,/g, '');
  };

  const [displayValue, setDisplayValue] = useState<string>(
    formatNumberWithCommas(value.toString())
  );

  useEffect(() => {
    const currentDisplayNumber = parseFloat(removeCommas(displayValue));
    if (currentDisplayNumber !== value && !displayValue.endsWith('.')) {
      const formattedValue = value.toFixed(2).replace(/\.00$/, '');
      setDisplayValue(formatNumberWithCommas(formattedValue));
    }
  }, [value]);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    let inputValue = e.target.value;

    inputValue = removeCommas(inputValue);

    inputValue = inputValue.replace(/[^\d.]/g, '');
    const parts = inputValue.split('.');
    if (parts.length > 2) {
      inputValue = parts[0] + '.' + parts.slice(1).join('');
    }

    if (parts[1] && parts[1].length > 2) {
      inputValue = parts[0] + '.' + parts[1].substring(0, 2);
    }

    if (inputValue === '.') {
      inputValue = '0.';
    }

    setDisplayValue(formatNumberWithCommas(inputValue));

    if (
      (!isNaN(parseFloat(inputValue)) && parseFloat(inputValue) !== value) ||
      inputValue.endsWith('.') ||
      inputValue === ''
    ) {
      const numericValue = parseFloat(inputValue);
      onChange(isNaN(numericValue) ? 0 : numericValue);
    }
  };

  return (
    <input
      type="text"
      value={displayValue}
      inputMode="decimal"
      placeholder="0"
      onChange={handleChange}
      className={`${className || ''} [appearance:textfield] [&::-webkit-outer-spin-button]:appearance-none [&::-webkit-inner-spin-button]:appearance-none`}
      disabled={isDisabled}
      id={id}
    />
  );
};
