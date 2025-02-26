import { useGlobalContext } from '../../context/global-context';
import ContentItem from '../../models/content-item';
import QRCode from '../QRCode';
import CustomLink from './CustomLink';
import styles from './Mobile.module.scss';

type Props = {
    accordionItems: ContentItem[];
};

export default function FeatureList({ accordionItems }: Props) {
    const { customer } = useGlobalContext();
    return (
        <div className={`${styles['feature-list']} ${styles['translate-from-left']}`}>
            <nav>
                {Object.values(accordionItems).map(
                    (content, index) => content.visible && <CustomLink {...content} key={index} />
                )}
            </nav>
            {customer?.ShowQRCodeUrl && (
                <nav>
                    <QRCode />
                </nav>
            )}
        </div>
    );
}
