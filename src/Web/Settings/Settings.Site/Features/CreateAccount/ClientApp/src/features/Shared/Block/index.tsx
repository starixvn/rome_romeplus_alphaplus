import classNames from 'classnames';
import React, { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';

import { IBlockItemProps, IBlockProps, IBlockRowProps, ILabelProps } from './Block';
import styles from './Block.module.scss';

const cx = classNames.bind(styles);

export function BlockSubHeader({ subHeader }: { subHeader: string }) {
  return <div className={styles['sub-header']}>{subHeader}</div>;
}

export function BlockColumn({ children }: { children: React.ReactNode }) {
  return <div className={styles['block-column']}>{children}</div>;
}

export function Block({ title, children, isCollapse, headerChildren = undefined }: IBlockProps) {
  const { t } = useTranslation();
  const [collapse, setCollapse] = useState(false);

  const onHeaderClick = (e: React.MouseEvent<HTMLDivElement>) => {
    e.preventDefault();
    setCollapse(!collapse);
  };

  useEffect(() => {
    if (isCollapse?.collapse === false) {
      setCollapse(isCollapse.collapse);
    }
  }, [isCollapse]);

  const tooltipContent = collapse ? t('ClickToExpand') : t('ClickToCollapse');
  return (
    <div className={styles['block-tab']}>
      <div className={styles['head-tab']} onClick={onHeaderClick} aria-hidden>
        <span
          className={cx(styles.icon, 'icon', { 'icon-chevron-down': !collapse, 'icon-chevron-right': collapse })}
          data-tooltip-id="global-tooltip"
          data-tooltip-content={tooltipContent}
        />
        {title}
        {headerChildren && (
          <div className={styles['head-control']} onClick={(e) => e.stopPropagation()} aria-hidden>
            {headerChildren}
          </div>
        )}
      </div>
      {!collapse && <div className={styles['content-tab']}>{children}</div>}
    </div>
  );
}

export function Label({ label }: ILabelProps) {
  return <div className={styles['label-text']}>{label}</div>;
}

export function BlockItem({ label, children }: IBlockItemProps) {
  return (
    <div className={styles.item}>
      <Label label={label} />
      <div className={styles.control}>{children}</div>
    </div>
  );
}

export function BlockRow({ children, isHeaderRow = false, label }: IBlockRowProps) {
  const className = cx(styles['block-row'], {
    [styles['block-row-head']]: isHeaderRow,
  });

  return (
    <div className={className}>
      <div className={styles['block-label']}>
        <Label label={label} />
      </div>
      <div className={styles['block-value']}>{children}</div>
    </div>
  );
}
