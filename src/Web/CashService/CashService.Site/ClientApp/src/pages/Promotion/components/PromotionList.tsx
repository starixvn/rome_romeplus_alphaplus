import React from 'react';

import Table from './Table';
import type { Promotion } from '../../../services/interfaces/Promotion';

interface PromotionListProps {
    items: Promotion[];
    isLoading: boolean;
    onEdit?: (item: Promotion) => void;
    onDelete?: (item: Promotion) => void;
}

const PromotionList: React.FC<PromotionListProps> = ({
    items,
    isLoading,
    onEdit,
    onDelete
}) => {
    return (
        <Table
            items={items}
            isLoading={isLoading}
            onEdit={onEdit}
            onDelete={onDelete}
        />
    );
};

export default PromotionList;