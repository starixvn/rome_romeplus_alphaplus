import ReactLoading from 'react-loading';

import styles from './Loader.module.scss';

export default function Loader() {
  return (
    <div className={styles.container}>
      <ReactLoading type="spinningBubbles" color="#000" height={45} width={45} />
    </div>
  );
}
