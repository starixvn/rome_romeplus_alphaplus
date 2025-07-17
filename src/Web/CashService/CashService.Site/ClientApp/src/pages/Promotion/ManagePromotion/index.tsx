import React, { useEffect, useState } from 'react';
import { createRoot } from 'react-dom/client';
import 'tippy.js/dist/tippy.css';

import { useTranslation } from 'react-i18next';
import { ButtonFullComponent } from '../../../components/Buttons/index';
import ConfirmPopup from '../../../components/ConfirmPopup';
import { ResultMessageComponent, ResultType } from '../../../components/Message';
import { TitleComponent } from '../../../components/Title';
import type { PromotionDetail } from '../../../services/interfaces/Promotion';
import { promotionService } from '../../../services/promotionAPI';
import '../../../styles/common.css';
import '../../../styles/global.scss';
import AddPromotion from './components/AddPromotion';
import EditPromotion from './components/EditPromotion';
import Filter from './components/Filter';
import PromotionList from './components/PromotionList';

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
  const [showEditPromotion, setShowEditPromotion] = useState<boolean>(false);
  const [editPromotionId, setEditPromotionId] = useState<number>(0);
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
    setEditPromotionId(promotion.promotionId);
    setShowEditPromotion(true);
  };

  const handleDelete = async (promotionId: number) => {
    try {
      var deleteResult = await promotionService.deletePromotion(promotionId);
      if (deleteResult.isSuccessful) {
        setValidationMessage({ type: 'success', message: t('table.deleteSuccess') });
        setTimeout(() => {
          fetchPromotions();
        }, 3000);
      } else {
        setValidationMessage({ type: 'error', message: deleteResult.message });
        setTimeout(() => {
          fetchPromotions();
        }, 3000);
      }
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
          title={t('table.confirmDeleteTitle')}
          message={t('table.confirmDeleteMessage')}
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
      {!showAddPromotion && !showEditPromotion ? (
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
      ) : showAddPromotion ? (
        <AddPromotion
          onClose={() => {
            setShowAddPromotion(false);
            fetchPromotions();
          }}
        />
      ) : (
        <EditPromotion
          promotionId={editPromotionId}
          onClose={() => {
            setShowEditPromotion(false);
            setEditPromotionId(0);
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
