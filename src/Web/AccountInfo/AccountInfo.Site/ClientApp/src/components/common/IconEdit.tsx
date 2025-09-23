import classNames from 'classnames';

import styles from './Icons.module.scss';

export const IconEdit = ({ className, onClick }: { className?: string, onClick?: () => void }) => (
    <span className={classNames('icon-border-color', styles['icon-edited'], className)} onClick={onClick} />
);
