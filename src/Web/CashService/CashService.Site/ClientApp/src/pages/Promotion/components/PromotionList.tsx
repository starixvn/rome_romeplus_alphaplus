import React, { useState } from 'react';
import { useTranslation } from 'react-i18next';
import Table from './Table';
import type { Promotion } from '../../../services/interfaces/Promotion';

interface PromotionListProps {
    items: Promotion[];
    isLoading: boolean;
    onFilterChange: (type: string, status: string) => void;
}

const PromotionList: React.FC<PromotionListProps> = ({
    items,
    isLoading,
    onFilterChange
}) => {
    const { t } = useTranslation('promotion');
    const [searchTerm, setSearchTerm] = useState('');
    const [selectedType, setSelectedType] = useState('All');
    const [selectedStatus, setSelectedStatus] = useState('All');

    const promotionTypes = ['All', 'Welcome Bonus', 'Reload Bonus', 'Cashback', 'Tournament'];
    const statusOptions = ['All', 'Active', 'Scheduled', 'Expired'];

    const handleTypeChange = (type: string) => {
        setSelectedType(type);
        onFilterChange(type, selectedStatus);
    };

    const handleStatusChange = (status: string) => {
        setSelectedStatus(status);
        onFilterChange(selectedType, status);
    };

    const filteredItems = items.filter(item => {
        const matchesSearch = item.promotionTitle.toLowerCase().includes(searchTerm.toLowerCase()) ||
            item.promotionType.toLowerCase().includes(searchTerm.toLowerCase());
        const matchesType = selectedType === 'All' || item.promotionType === selectedType;
        const matchesStatus = selectedStatus === 'All' || item.status === selectedStatus;
        return matchesSearch && matchesType && matchesStatus;
    });

    return (
        <div className="space-y-4">
            <div className="flex flex-wrap gap-4 items-center">
                <div className="flex-1 min-w-[200px]">
                    <input
                        type="text"
                        placeholder={t('search.placeholder')}
                        className="w-full px-4 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
                        value={searchTerm}
                        onChange={(e) => setSearchTerm(e.target.value)}
                    />
                </div>
                <div className="flex gap-4">
                    <select
                        className="px-4 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
                        value={selectedType}
                        onChange={(e) => handleTypeChange(e.target.value)}
                    >
                        {promotionTypes.map(type => (
                            <option key={type} value={type}>{type}</option>
                        ))}
                    </select>
                    <select
                        className="px-4 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
                        value={selectedStatus}
                        onChange={(e) => handleStatusChange(e.target.value)}
                    >
                        {statusOptions.map(status => (
                            <option key={status} value={status}>{status}</option>
                        ))}
                    </select>
                </div>
            </div>
            <Table items={filteredItems} isLoading={isLoading} />
        </div>
    );
};

export default PromotionList;