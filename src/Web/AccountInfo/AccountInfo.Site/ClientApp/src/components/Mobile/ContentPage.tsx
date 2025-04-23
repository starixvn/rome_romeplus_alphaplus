import ContentItem from '../../models/content-item';
import CustomLink from './CustomLink';
import styles from './Mobile.module.scss';

export default function ContentPage({
    fetchDataFromServer,
    iconClass = '',
    featureText = '',
    children
}: Partial<ContentItem>) {
    fetchDataFromServer?.();

    return (
        <div className={`${styles['content-page']} ${styles['translate-from-left']}`}>
            <CustomLink {...{ iconClass, featureText, pathname: '/featureList' }} isDetail />
            {children}
        </div>
    );
}
