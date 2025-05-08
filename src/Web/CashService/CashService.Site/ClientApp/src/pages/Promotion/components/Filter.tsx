import React, { useState } from 'react';
import { useTranslation } from 'react-i18next';

interface FilterProps {
    onFilterChange: (type: string, status: string) => void;
}

const Filter: React.FC<FilterProps> = ({ onFilterChange }) => {
    const { t } = useTranslation('promotion');
    const [selectedType, setSelectedType] = useState('All');
    const [selectedStatus, setSelectedStatus] = useState('All');
    const [searchTerm, setSearchTerm] = useState('');

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

    return (
        <div className="flex ">
            {/* <div className="flex-1 min-w-[200px]">
                <input
                    type="text"
                    placeholder={t('search.placeholder')}
                    className="w-full px-4 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
                    value={searchTerm}
                    onChange={(e) => setSearchTerm(e.target.value)}
                />
            </div> */}
            <div className="text-[12px] flex gap-[16px]">
                <div className="flex flex-col w-[180px]">
                    <label className="block  font-semibold text-[#333333] my-[4px]">Promotion Type</label>
                    <select
                        className="border-0 border-b-[1px] h-[32px] border-[#0000003D] rounded-[0px] focus:outline-none focus:focus:outline-none focus:ring-blue-500"
                        value={selectedType}
                        onChange={(e) => handleTypeChange(e.target.value)}
                    >
                        {promotionTypes.map(type => (
                            <option key={type} value={type}>{type}</option>
                        ))}
                    </select>
                </div>
                
                <div className="flex flex-col w-[160px]">
                    <label className='block  font-semibold text-[#333333] my-[4px]'>Status</label>
                    <select
                    className="border-0 border-b-[1px] h-[32px] border-[#0000003D] rounded-[0px] focus:outline-none focus:focus:outline-none focus:ring-blue-500"
                    value={selectedStatus}
                    onChange={(e) => handleStatusChange(e.target.value)}
                >
                    {statusOptions.map(status => (
                        <option key={status} value={status}>{status}</option>
                    ))}
                </select>
                </div>
                
            </div>
        </div>
    );
};

export default Filter;
