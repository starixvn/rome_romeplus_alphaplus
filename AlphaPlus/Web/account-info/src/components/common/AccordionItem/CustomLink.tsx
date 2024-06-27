import { useTranslation } from 'react-i18next';
import { Link } from 'react-router-dom';

import styles from './AccordionItem.module.scss';

type Props = {
    iconClass: string;
    pathname: string;
    featureText: string;
};

export default function CustomLink({ iconClass, pathname, featureText }: Props) {
    const { t } = useTranslation();
    return (
        <Link
            id={`nav-${iconClass}`}
            to={{
                pathname,
                state: { addAnimation: true }
            }}
            className={styles['feature-item']}
        >
            <span className={iconClass} />
            {featureText}
            <span className={styles['feature-item-nav']}>{t('Back')}</span>
        </Link>
    );
}
