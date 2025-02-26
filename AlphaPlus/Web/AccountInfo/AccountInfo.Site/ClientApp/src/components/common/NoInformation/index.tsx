import { useTranslation } from 'react-i18next';

import styles from './NoInformation.module.scss';

export default function NoInformation({ text = undefined }: { text?: string }) {
    const { t } = useTranslation();
    return <div className={styles['no-information']}>{text ?? t('NoInformation')}</div>;
}
