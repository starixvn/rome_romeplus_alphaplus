import React from 'react';
import Table from './Table';
import type { PromotionDetail } from '../../../../services/interfaces/Promotion';

interface PromotionListProps {
  items: PromotionDetail[];
  isLoading: boolean;
  onEdit?: (item: PromotionDetail) => void;
  onDelete?: (item: PromotionDetail) => void;
}

const PromotionList: React.FC<PromotionListProps> = ({ items, isLoading, onEdit, onDelete }) => {
  return (
    <div className="flex flex-col gap-[16px]">
      <Table items={items} isLoading={isLoading} onEdit={onEdit} onDelete={onDelete} />
    </div>
  );
};

export default PromotionList;
