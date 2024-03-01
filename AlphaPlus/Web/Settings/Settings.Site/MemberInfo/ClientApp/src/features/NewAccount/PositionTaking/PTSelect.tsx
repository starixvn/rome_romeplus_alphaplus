import { ChangeEvent } from 'react';

import styles from './PTSelect.module.scss';

interface PTSelectProps {
  min: number;
  max: number;
  value: number;
  onSelectChange: (event: ChangeEvent<HTMLSelectElement>) => void;
  is3DecimalSite?: boolean;
}

export default function PTSelect({ min, max, value, onSelectChange, is3DecimalSite = false }: PTSelectProps) {
  let options = [];
  let pt = max;

  while (min <= pt) {
    options.push(pt);
    pt -= is3DecimalSite ? 0.5 : 1;
  }

  options = options.includes(value) ? options : [...options, value].sort((a, b) => b - a);

  const invalid = value > max || value < min;

  return (
    <select
      value={value}
      onChange={onSelectChange}
      className={`${styles['pt-select']} ${invalid ? styles.invalid : ''}`}
    >
      {options.map((option) => (
        <option key={option} value={option}>
          {option / 100}
        </option>
      ))}
    </select>
  );
}

PTSelect.defaultProps = {
  is789y: false,
};
