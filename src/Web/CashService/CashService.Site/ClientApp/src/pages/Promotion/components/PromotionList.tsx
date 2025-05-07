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
        <div className="space-y-4">
            <Table items={items} isLoading={isLoading} />
        </div>
    );
};

export default PromotionList;