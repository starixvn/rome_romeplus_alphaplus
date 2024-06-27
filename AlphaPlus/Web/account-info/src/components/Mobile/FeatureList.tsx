import ContentItem from '../../models/content-item';
import CustomLink from './CustomLink';
import styles from './Mobile.module.scss';

type Props = {
    accordionItems: ContentItem[];
};

export default function FeatureList({ accordionItems }: Props) {
    return (
        <div className={`${styles['feature-list']} ${styles['translate-from-left']}`}>
            <nav>
                {Object.values(accordionItems).map(
                    (content, index) => content.visible && <CustomLink {...content} key={index} />
                )}
            </nav>
        </div>
    );
}
