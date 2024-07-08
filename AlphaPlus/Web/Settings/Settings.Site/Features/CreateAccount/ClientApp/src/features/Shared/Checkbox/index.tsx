import classNames from 'classnames';

import styles from './Checkbox.module.scss';

interface IProps {
  checked: boolean;
  onChange: () => void;
  label?: string;
  marginLeft?: boolean;
}
const cx = classNames.bind(styles);

export default function Checkbox({ checked, onChange, label, marginLeft }: IProps) {
  return (
    <label className={cx(styles.checkbox, { [styles.left]: marginLeft })} htmlFor="checkbox">
      <input type="checkbox" id="checkbox" checked={checked} onChange={onChange} />
      {label && <span>{label}</span>}
    </label>
  );
}

Checkbox.defaultProps = { label: undefined, marginLeft: false };
