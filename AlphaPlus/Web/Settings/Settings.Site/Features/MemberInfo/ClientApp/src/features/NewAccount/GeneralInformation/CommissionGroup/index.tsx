import { ChangeEvent } from 'react';

import useNewAccountStore from '@/store/newAccountStore';

interface IProps {
  value: string;
  onChange: (event: ChangeEvent<HTMLSelectElement>) => void;
}
const currencyWon = 45;
export default function CommissionGroup({ value, onChange }: IProps) {
  const site = useNewAccountStore((state) => state.generalInfo.site);
  const currency = useNewAccountStore((state) => state.generalInfo.currency);
  let groups: string[];
  if (site === '76666B') {
    groups = ['a'];
  } else if (currency === currencyWon) {
    groups = ['dea', 'deb', 'dec', 'ded'];
  } else {
    groups = ['a', 'b', 'c', 'd', 'hka', 'hkb', 'hkc', 'hkd'];
  }

  return (
    <select value={value} onChange={onChange}>
      {groups.map((group) => (
        <option key={group} value={group}>
          {group}
        </option>
      ))}
    </select>
  );
}
