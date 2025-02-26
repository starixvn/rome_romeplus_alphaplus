import { ChangeEvent } from 'react';

interface IProps {
  max: number;
  value: number;
  onChange: (event: ChangeEvent<HTMLSelectElement>) => void;
}

const multiplier = 10000;

export default function CommissionSelect({ max, value, onChange }: IProps) {
  const options = [];
  let com = max * multiplier;

  while (com >= 0) {
    options.push(com);
    com -= 5;
  }

  return (
    <select value={value} onChange={onChange}>
      {options.map((option) => (
        <option key={option} value={option / multiplier}>
          {option / multiplier}
        </option>
      ))}
    </select>
  );
}
