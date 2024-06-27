import { useTranslation } from 'react-i18next';
import { Link } from 'react-router-dom';

import styles from './CustomLink.module.scss';

type Props = {
    iconClass: string;
    pathname: string;
    featureText: string;
    isDetail?: boolean;
};

export default function CustomLink({ iconClass, pathname, featureText, isDetail = false }: Props) {
    const { t } = useTranslation();
    return (
        <Link
            id={`nav-${iconClass}`}
            to={{
                pathname,
                state: { addAnimation: true }
            }}
            className={styles['custom-link']}
        >
            <span className={iconClass} />
            {featureText}

            {isDetail && <span className={styles['feature-item-nav']}>{t('Back')}</span>}
        </Link>
    );
}
