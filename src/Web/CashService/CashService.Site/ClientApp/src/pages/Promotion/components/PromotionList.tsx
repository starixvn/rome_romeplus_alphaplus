import React from 'react';

import Table from './Table';
import type { Promotion } from '../../../services/interfaces/Promotion';

interface PromotionListProps {
    items: Promotion[];
    isLoading: boolean;
}

const PromotionList: React.FC<PromotionListProps> = ({
    items,
    isLoading
}) => {
     

    return (
        <>
            <Table items={items} isLoading={isLoading} />
        </>
    );
};

export default PromotionList;