import React, { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';

import { TitleComponent } from '../../../../components/Title';
import { ButtonComponent } from '../../../../components/Buttons';
import { displayAmount } from '../../../../utils/commonFunctions';
import { AmountInput } from '../../../../components/AmountInput';
import { ButtonIconComponent } from '../../../../components/Buttons/ButtonIconComponent';
import { MessageComponent, ResultMessageProps } from '../../../../components/Message';
import { PromotionTransaction } from '../../../../services/interfaces/Promotion';
import { promotionService } from '../../../../services/promotionAPI';

interface EditPendingPromotionProps {
  isOpen: boolean;
  onClose: () => void;
  item: PromotionTransaction | null;
}

const EditPendingPromotion: React.FC<EditPendingPromotionProps> = ({ isOpen, onClose, item }) => {
  const { t } = useTranslation('promotion');
  const [bonusAmount, setBonusAmount] = useState(0);
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [resultMessage, setResultMessage] = useState<ResultMessageProps>({
    type: 'success',
    message: '',
  });

  useEffect(() => {
    setBonusAmount(item?.amount || 0);
  }, [item]);

  const handleApprove = async () => {
    if (bonusAmount <= 0) {
      setResultMessage({
        type: 'error',
        message: t('editModal.bonusAmountMustBeGreaterThan0'),
      });
      return;
    }
    setIsSubmitting(true);
    try {
      var result = await promotionService.approvePromotionTransaction(
        item!.custId,
        item!.transId,
        bonusAmount
      );
      if (result.isSuccessful) {
        setResultMessage({
          type: 'success',
          message: t('editModal.approvedSuccess'),
        });
        setTimeout(() => {
          onClose();
        }, 3000);
      } else {
        setResultMessage({
          type: 'error',
          message: result.message,
        });
        setIsSubmitting(false);
      }
    } catch (error) {
      console.error('Error approving promotion:', error);
    } finally {
      setIsSubmitting(false);
    }
  };

  const handleReject = async () => {
    setIsSubmitting(true);
    try {
      var result = await promotionService.rejectPromotionTransaction(
        item!.custId,
        item!.transId,
        bonusAmount
      );
      if (result.isSuccessful) {
        setResultMessage({
          type: 'success',
          message: t('editModal.rejectedSuccess'),
        });
        setTimeout(() => {
          onClose();
        }, 3000);
      } else {
        setResultMessage({
          type: 'error',
          message: result.message,
        });
        setIsSubmitting(false);
      }
    } catch (error) {
      console.error('Error rejecting promotion:', error);
    } finally {
      setIsSubmitting(false);
    }
  };

  if (!isOpen) return null;

  return (
    <div className="bg-[#00000080] fixed inset-0 flex items-center justify-center z-50">
      <div className="p-[12px] pt-[0] bg-[var(--modal-background)] border-[1px] shadow-[var(--modal-box-shadow)] max-[768px]:max-w-[95%] max-h-[90%] overflow-auto rounded-[4px] w-[25vw]">
        <div className="relative mb-[10px] border-b-[1px] border-[var(--border-color)] py-[11px]">
          <TitleComponent type="medium" text={`${t('editModal.title')} ${item?.transId}`} />
          <ButtonIconComponent style="Close" onClick={onClose} />
        </div>
        <MessageComponent
          type={resultMessage.type}
          message={resultMessage.message}
          className="mb-[16px] max-[768px]:mb-[0]"
        />
        <div className="mb-[20px]">
          <p className="text-[var(--text-color)] text-center mb-[16px]">{t('editModal.message')}</p>

          <div className="mb-[16px]">
            <label className="block text-[var(--text-color)] text-sm font-medium mb-[8px]">
              {t('editModal.bonusAmount')}
            </label>
            <AmountInput
              id="bonusAmount"
              value={bonusAmount}
              onChange={(e) => setBonusAmount(e)}
              className="control-field"
            />
            <p className="text-[var(--text-secondary)] text-sm mt-[4px]">
              {t('editModal.suggested')}: {displayAmount(item?.amount || 0)}
            </p>
          </div>
        </div>

        <div className="flex justify-center gap-[8px]">
          <ButtonComponent
            onClick={handleReject}
            text={t('editModal.reject')}
            variant="reject"
            disabled={isSubmitting}
            className="min-w-[80px]"
          />
          <ButtonComponent
            onClick={handleApprove}
            text={t('editModal.approve')}
            variant="apply"
            disabled={isSubmitting}
            className="min-w-[80px]"
          />
        </div>
      </div>
    </div>
  );
};

export default EditPendingPromotion;
