import React, { useState } from 'react';
import classNames from 'classnames';
import styles from './ProductSettingInfo.module.scss';

interface IShowHideSession {
  title: string;
  children?: React.ReactNode;
}

export default function Session({ children, title }: IShowHideSession) {
  const [collapse, setCollapse] = useState(false);

  const onHeaderClick = (e: React.MouseEvent<HTMLDivElement>) => {
    e.preventDefault();
    setCollapse(!collapse);
  };
  return (
    <div className={styles['sport-container']}>
      <div onClick={onHeaderClick} className={styles['sport-header']} aria-hidden>
        <span
          className={classNames(styles.icon, {
            'icon-chevron-down': !collapse,
            'icon-chevron-right': collapse,
          })}
        />
        {title}
      </div>
      {!collapse && <div>{children}</div>}
    </div>
  );
}
Session.defaultProps = {
  children: undefined,
};
