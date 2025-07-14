import React, { useState, useEffect } from 'react';
import { createRoot } from 'react-dom/client';
import 'tippy.js/dist/tippy.css';

import { promotionService } from '../../../services/promotionAPI';
import PromotionList from './components/PromotionList';
import Filter from './components/Filter';
import type { PromotionDetail } from '../../../services/interfaces/Promotion';
import AddPromotion from './components/AddPromotion';
import { TitleComponent } from '../../../components/Title';
import { ButtonFullComponent } from '../../../components/Buttons/index';
import '../../../styles/common.css';
import '../../../styles/global.scss';
import ConfirmPopup from '../../../components/ConfirmPopup';
import { useTranslation } from 'react-i18next';
import { ResultMessageComponent, ResultType } from '../../../components/Message';

const Promotion: React.FC = () => {
  const [promotionType, setPromotionType] = useState<number>(0);
  const [status, setStatus] = useState<number>(0);
  const [validationMessage, setValidationMessage] = useState<{
    type: ResultType;
    message: string;
  }>({ type: 'success', message: '' });
  const [promotions, setPromotions] = useState<PromotionDetail[]>([]);
  const [isLoading, setIsLoading] = useState<boolean>(false);
  const [showAddPromotion, setShowAddPromotion] = useState<boolean>(false);
  const [showConfirmDelete, setShowConfirmDelete] = useState<boolean>(false);
  const [deletePromotionId, setDeletePromotionId] = useState<number>(0);
  const { t } = useTranslation('promotion');
  const { t: tGlobal } = useTranslation('global');
  const handleFilterChange = (type: number, status: number) => {
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
      setValidationMessage({ type: 'success', message: '' });
    } catch (err) {
      setValidationMessage({ type: 'error', message: 'Failed to load promotions' });
      console.error('Error loading promotions:', err);
    } finally {
      setIsLoading(false);
    }
  };

  useEffect(() => {
    fetchPromotions();
  }, [promotionType, status]);

  const handleEdit = (promotion: PromotionDetail) => {
    console.log(promotion);
    setShowAddPromotion(true);
  };

  const handleDelete = async (promotionId: number) => {
    try {
      var deleteResult = await promotionService.deletePromotion(promotionId);
      if (deleteResult.isSuccessful) {
        setValidationMessage({ type: 'success', message: t('promotionList.deleteSuccess') });
        setTimeout(() => {
          fetchPromotions();
        }, 3000);
      } else {
        setValidationMessage({ type: 'error', message: deleteResult.message });
      }
      fetchPromotions();
    } catch (err) {
      setValidationMessage({ type: 'error', message: 'Failed to delete promotion' });
      console.error('Error deleting promotion:', err);
    }
  };

  return (
    <div className="pb-[12px] h-full flex flex-col promotion-page">
      {validationMessage.message !== '' && (
        <ResultMessageComponent
          message={validationMessage.message}
          type={validationMessage.type}
          className="mb-[16px] max-[768px]:mb-[0]"
        />
      )}
      {showConfirmDelete && (
        <ConfirmPopup
          title={t('promotionList.confirmDeleteTitle')}
          message={t('promotionList.confirmDeleteMessage')}
          onCancel={() => {
            setShowConfirmDelete(false);
            setDeletePromotionId(0);
          }}
          onConfirm={() => {
            setShowConfirmDelete(false);
            handleDelete(deletePromotionId);
            setDeletePromotionId(0);
          }}
          cancelText={tGlobal('cancel')}
          confirmText={tGlobal('confirm')}
        />
      )}
      {!showAddPromotion ? (
        <>
          <TitleComponent title="Manage Promotion" showTippy={false} />
          <div className="mb-[8px]">
            <ButtonFullComponent
              text="Add Promotion"
              onClick={() => {
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
            onDelete={(promotion) => {
              setDeletePromotionId(promotion.promotionId);
              setShowConfirmDelete(true);
            }}
          />
        </>
      ) : (
        <AddPromotion
          onClose={() => {
            setShowAddPromotion(false);
            fetchPromotions();
          }}
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
