import React, { useState, useEffect } from 'react';

interface AmountInputProps {
  value: number; // This remains number for the external API
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
  const [displayValue, setDisplayValue] = useState<string>(value.toString());

  useEffect(() => {
    const currentDisplayNumber = parseFloat(displayValue);
    if (
      currentDisplayNumber !== value ||
      !/^\d*\.?\d{0,2}$/.test(displayValue) ||
      displayValue.endsWith('.')
    ) {
      setDisplayValue(value.toFixed(2).replace(/\.00$/, ''));
    }
  }, [value]);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    let inputValue = e.target.value;

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

    setDisplayValue(inputValue);

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
