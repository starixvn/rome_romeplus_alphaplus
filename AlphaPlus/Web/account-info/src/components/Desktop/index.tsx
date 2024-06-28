import { useState } from 'react';

import { BlockIds } from '../../constants';
import { useGlobalContext } from '../../context/global-context';
import ContentItem from '../../models/content-item';
import AccordionItem from '../common/AccordionItem';
import QRCode from '../QRCode';
import styles from './Desktop.module.scss';

export default function AccountInfoDesktop({ accordionItems }: { accordionItems: ContentItem[] }) {
    const [expandedBlockId, setExpandedBlockId] = useState<BlockIds | undefined>(BlockIds.BalanceInfo);
    const { customer } = useGlobalContext();

    const handleBlockClick = (blockId: BlockIds, fetchDataFromServer?: (...arg: any[]) => void) => {
        if (blockId === expandedBlockId) {
            setExpandedBlockId(undefined);
        } else {
            setExpandedBlockId(blockId);
        }

        fetchDataFromServer?.();
    };

    return (
        <div className={styles['accordions']}>
            {accordionItems.map(
                ({ blockId, children, visible, iconClass, featureText, fetchDataFromServer }) =>
                    visible && (
                        <AccordionItem
                            key={blockId}
                            expanded={expandedBlockId === blockId}
                            iconClass={iconClass}
                            featureText={featureText}
                            onClick={() => handleBlockClick(blockId, fetchDataFromServer)}
                        >
                            {children}
                        </AccordionItem>
                    )
            )}
            {customer?.ShowQRCodeUrl && <QRCode />}
        </div>
    );
}
