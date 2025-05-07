import React, { useState, useEffect } from 'react';
import { createRoot } from 'react-dom/client';
import 'tippy.js/dist/tippy.css'; 
import '../../i18n/i18n';
import { promotionService } from '../../services/promotionAPI';
import { Promotion as PromotionType } from '../../services/interfaces/Promotion';
import PromotionList from './components/PromotionList';

const Promotion: React.FC = () => {
    const [promotionType, setPromotionType] = useState<string>('All');
    const [status, setStatus] = useState<string>('All');
    const [promotions, setPromotions] = useState<PromotionType[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        const fetchPromotions = async () => {
            try {
                setLoading(true);
                const response = await promotionService.getPromotion({
                    Type: promotionType,
                    Status: status
                });
                setPromotions(response);
                setError(null);
            } catch (err) {
                setError('Failed to load promotions');
                console.error('Error loading promotions:', err);
                setPromotions([]);
            } finally {
                setLoading(false);
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
                <div className="promotion-list">
                   
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