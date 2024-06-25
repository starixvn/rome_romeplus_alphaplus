import { useState } from 'react';

import { BlockIds } from '../../constants';
import ContentItem from '../../models/content-item';
import AccordionItem from '../common/AccordionItem';
import styles from './Desktop.module.scss';

export default function AccountInfoDesktop({ accordionItems }: { accordionItems: ContentItem[] }) {
    const [expandedBlockId, setExpandedBlockId] = useState<BlockIds | undefined>(BlockIds.BalanceInfo);

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
        </div>
    );
}
