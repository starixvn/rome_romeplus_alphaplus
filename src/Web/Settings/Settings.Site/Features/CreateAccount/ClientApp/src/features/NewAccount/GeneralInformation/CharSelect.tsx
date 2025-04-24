import classNames from 'classnames';

import styles from './CharSelector.module.scss';

const charOptions = [
  '0',
  '1',
  '2',
  '3',
  '4',
  '5',
  '6',
  '7',
  '8',
  '9',
  'A',
  'B',
  'C',
  'D',
  'E',
  'F',
  'G',
  'H',
  'I',
  'J',
  'K',
  'L',
  'M',
  'N',
  'O',
  'P',
  'Q',
  'R',
  'S',
  'T',
  'U',
  'V',
  'W',
  'X',
  'Y',
  'Z',
];

interface IProps {
  value: string;
  className: string;
  onSelect: (selected: string) => void;
}

export default function CharSelector({ value, onSelect, className }: IProps) {
  const handleSelect = (e: React.ChangeEvent<HTMLSelectElement>) => {
    const selected = e.target.value;
    if (onSelect) {
      onSelect(selected);
    }
  };

  return (
    <select className={classNames(styles.controlSelect, className)} value={value} onChange={handleSelect}>
      {charOptions.map((char) => (
        <option key={char} value={char}>
          {char}
        </option>
      ))}
    </select>
  );
}
