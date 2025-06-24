import React, { useState, useEffect } from 'react';
import { createRoot } from 'react-dom/client';
import 'tippy.js/dist/tippy.css';

import { promotionService } from '../../services/promotionAPI';
import PromotionList from './components/PromotionList';
import Filter from './components/Filter';
import type { Promotion } from '../../services/interfaces/Promotion';
import AddPromotion from './components/AddPromotion';
import TitleComponent from '../../components/TitleComponents';
import { ButtonFullComponent } from '../../components/ButtonComponents';
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
        status: status,
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
    <div className="py-[12px] h-full flex flex-col promotion-page">
      {error && (
        <div
          className="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded relative mb-4"
          role="alert"
        >
          <strong className="font-bold">Error!</strong>
          <span className="block sm:inline"> {error}</span>
        </div>
      )}

      {!showAddPromotion ? (
        <>
          <div className="mb-[8px]">
            <TitleComponent title="Manage Promotion" showTippy={false} />
          </div>
          <div className="mb-[8px]">
            <ButtonFullComponent
              text="Add Promotion"
              onClick={() => {
                setSelectedPromotion(null);
                setShowAddPromotion(true);
              }}
              className="min-w-[183px] [&>*]:min-w-[183px] [&>*]:inline-block"
            />
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
