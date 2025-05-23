import React from 'react';

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
  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const inputValue = e.target.value;
    if (inputValue === '' || /^\d*\.?\d*$/.test(inputValue)) {
      const numberValue = inputValue === '' ? 0 : parseFloat(inputValue);
      onChange(numberValue);
    }
  };

  return (
    <input
      type="text"
      value={value}
      pattern="[0-9]*\.?[0-9]*"
      inputMode="decimal"
      placeholder="0"
      onChange={handleChange}
      className={`${className || ''}`}
      disabled={isDisabled}
      id={id}
    />
  );
};
