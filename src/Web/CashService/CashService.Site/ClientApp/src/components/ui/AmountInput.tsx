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
  return (
    <input
      type="number"
      value={value}
      inputMode="decimal"
      placeholder="0"
      onChange={(e) => onChange(parseFloat(e.target.value) || 0)}
      className={`${className || ''} [appearance:textfield] [&::-webkit-outer-spin-button]:appearance-none [&::-webkit-inner-spin-button]:appearance-none`}
      disabled={isDisabled}
      id={id}
    />
  );
};
