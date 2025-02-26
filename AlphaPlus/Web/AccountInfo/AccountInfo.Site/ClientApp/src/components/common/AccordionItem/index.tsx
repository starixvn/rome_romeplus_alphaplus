import classNames from 'classnames';
import React from 'react';

import styles from './AccordionItem.module.scss';

type Props = {
    iconClass: string;
    featureText: string;
    expanded: boolean;
    children: React.ReactNode;
    onClick: () => void;
};

export default function AccordionItem({ iconClass, featureText, expanded, children, onClick }: Props) {
    return (
        <div className={classNames(styles['accordion-item'], { [styles.expanded]: expanded })}>
            <div className={styles['feature-item']} onClick={onClick}>
                <span className={iconClass} /> {featureText}
            </div>
            <div className={styles['feature-detail']}>{children}</div>
        </div>
    );
}
