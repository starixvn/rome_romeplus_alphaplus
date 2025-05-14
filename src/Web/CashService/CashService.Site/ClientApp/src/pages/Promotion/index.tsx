import React, { useState, useEffect } from 'react';
import { createRoot } from 'react-dom/client';
import 'tippy.js/dist/tippy.css'; 

import { promotionService } from '../../services/promotionAPI';
import PromotionList from './components/PromotionList';
import Filter from './components/Filter';
import type { Promotion } from '../../services/interfaces/Promotion';
import AddPromotion from './components/AddPromotion';
import TitleCommon from '../../components/ui/TitleComponents';
import '../../styles/common.css';
import '../../styles/global.scss';

const Promotion: React.FC = () => {
    const [promotionType, setPromotionType] = useState<string>('All');
    const [status, setStatus] = useState<string>('All');
    const [error, setError] = useState<string | null>(null);
    const [promotions, setPromotions] = useState<Promotion[]>([]);
    const [isLoading, setIsLoading] = useState<boolean>(false);
    const [showAddPromotion, setShowAddPromotion] = useState<boolean>(false);
    const [selectedPromotion, setSelectedPromotion] = useState<Promotion | null>(null);

    const handleFilterChange = (type: string, status: string) => {
        setPromotionType(type);
        setStatus(status);
    };

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

    useEffect(() => {
        fetchPromotions();
    }, [promotionType, status]);

    const handleEdit = (promotion: Promotion) => {
        setSelectedPromotion(promotion);
        setShowAddPromotion(true);
    };

    const handleDelete = async (promotion: Promotion) => {
        if (window.confirm('Are you sure you want to delete this promotion?')) {
            try {
                await promotionService.deletePromotion(promotion.promotionId);
                fetchPromotions();
            } catch (err) {
                setError('Failed to delete promotion');
                console.error('Error deleting promotion:', err);
            }
        }
    };

    return (
     
            <div className="p-[12px] h-full flex flex-col promotion-page">
                <div className="mb-[8px]">                      
                    <TitleCommon title='Manage Promotion' Tippy={false} />                 
                </div>
                {error && (
                    <div className="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded relative mb-4" role="alert">
                        <strong className="font-bold">Error!</strong>
                        <span className="block sm:inline"> {error}</span>
                    </div>
                )}
                

                {!showAddPromotion ? (
                    <>
                        <div className="mb-[16px]">
                            <button 
                                className="w-[183px] h-[30px] rounded-[5px] text-[#ffffff] px-[16px] py-[4px] text-[12px] font-bold mr-[8px] bg-[var(--button-background)] hover:bg-[var(--button-background-hover)] cursor-pointer shadow-[0_1px_5px_#0003,0_2px_2px_#00000024,0_3px_1px_-2px_#0000001f]"
                                onClick={() => {
                                    setSelectedPromotion(null);
                                    setShowAddPromotion(true);
                                }}
                            >
                                + Add Promotion
                            </button>
                        </div>
                        <Filter onFilterChange={handleFilterChange} />                        
                        <PromotionList 
                            items={promotions}
                            isLoading={isLoading}
                            onEdit={handleEdit}
                            onDelete={handleDelete}
                        />
                        
                    </>
                ) : (
                    <AddPromotion 
                        onClose={() => {
                            setShowAddPromotion(false);
                            setSelectedPromotion(null);
                        }} 
                        onSave={() => {
                            setShowAddPromotion(false);
                            setSelectedPromotion(null);
                            fetchPromotions();
                        }}
                        promotion={selectedPromotion}
                    />
                )}
            </div>
 
    );
};

createRoot(document.getElementById('root')!).render(
    <React.StrictMode>
        <Promotion />
    </React.StrictMode>
); 