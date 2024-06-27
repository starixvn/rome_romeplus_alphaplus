import ContentItem from '../../models/content-item';
import CustomLink from '../common/AccordionItem/CustomLink';
import styles from './Mobile.module.scss';

export default function ContentPage({
    fetchDataFromServer,
    iconClass = '',
    featureText = '',
    children
}: Partial<ContentItem>) {
    fetchDataFromServer?.();

    return (
        <div className={`${styles['feature-detail']} ${styles['translate-from-left']}`}>
            <CustomLink {...{ iconClass, featureText, pathname: '/featureList' }} />
            {children}
        </div>
    );
}
