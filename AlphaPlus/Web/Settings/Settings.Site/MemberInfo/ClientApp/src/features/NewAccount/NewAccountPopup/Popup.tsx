import useNewAccountStore from '@/store/newAccountStore';

import styles from './Popup.module.scss';

export default function Popup() {
  const popup = useNewAccountStore((state) => state.popup);

  if (popup) {
    return (
      <div className={styles['popup-overlay']}>
        <div className={styles['popup-box']}>{popup}</div>
      </div>
    );
  }

  return null;
}
