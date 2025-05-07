import React, { useState, useEffect } from 'react';
import { createRoot } from 'react-dom/client';
import 'tippy.js/dist/tippy.css'; 
import '../../i18n/i18n';
import { promotionService } from '../../services/promotionAPI';
import PromotionList from './components/PromotionList';
import Filter from './components/Filter';
import type { Promotion } from '../../services/interfaces/Promotion';

const Promotion: React.FC = () => {
    const [promotionType, setPromotionType] = useState<string>('All');
    const [status, setStatus] = useState<string>('All');
    const [error, setError] = useState<string | null>(null);
    const [promotions, setPromotions] = useState<Promotion[]>([]);
    const [isLoading, setIsLoading] = useState<boolean>(false);

    const handleFilterChange = (type: string, status: string) => {
        setPromotionType(type);
        setStatus(status);
    };

    useEffect(() => {
        const fetchPromotions = async () => {
            setIsLoading(true);
            try {
                const response = await promotionService.getPromotion({
                    type: promotionType,
                    status: status
                });
                setPromotions(response);
                setError(null);
            } catch (err) {
                setError('Failed to load promotions');
                console.error('Error loading promotions:', err);
            } finally {
                setIsLoading(false);
            }
        };

        fetchPromotions();
    }, [promotionType, status]);

    return (
        <div className="min-h-screen bg-gray-50">
            <div className="bg-white rounded-lg shadow-sm p-4">
                {error && (
                    <div className="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded relative mb-4" role="alert">
                        <strong className="font-bold">Error!</strong>
                        <span className="block sm:inline"> {error}</span>
                    </div>
                )}
                
                <Filter onFilterChange={handleFilterChange} />
                
                <div className="promotion-list">
                    <PromotionList 
                        items={promotions}
                        isLoading={isLoading}
                    />
                </div>
            </div>
        </div>
    );
};

createRoot(document.getElementById('root')!).render(
    <React.StrictMode>
        <Promotion />
    </React.StrictMode>
); 