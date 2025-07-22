import React, { useState, useEffect } from 'react';
import { useTranslation } from 'react-i18next';

import { TitleComponent } from '../../../../components/Title';
import { ButtonComponent } from '../../../../components/Buttons';
import { displayAmount } from '../../../../utils/commonFunctions';
import { AmountInput } from '../../../../components/AmountInput';
import { ButtonIconComponent } from '../../../../components/Buttons/ButtonIconComponent';

interface EditPendingPromotionProps {
  isOpen: boolean;
  onClose: () => void;
  transId: number;
  currentAmount: number;
  onApprove: (transId: number, amount: number) => void;
  onReject: (transId: number) => void;
}

const EditPendingPromotion: React.FC<EditPendingPromotionProps> = ({
  isOpen,
  onClose,
  transId,
  currentAmount,
  onApprove,
  onReject,
}) => {
  const { t } = useTranslation('promotion');
  const [bonusAmount, setBonusAmount] = useState(currentAmount);
  const [isSubmitting, setIsSubmitting] = useState(false);

  useEffect(() => {
    if (isOpen) {
      setBonusAmount(currentAmount);
    }
  }, [isOpen, currentAmount]);

  const handleApprove = async () => {
    setIsSubmitting(true);
    try {
      await onApprove(transId, bonusAmount);
      onClose();
    } catch (error) {
      console.error('Error approving promotion:', error);
    } finally {
      setIsSubmitting(false);
    }
  };

  const handleReject = async () => {
    setIsSubmitting(true);
    try {
      await onReject(transId);
      onClose();
    } catch (error) {
      console.error('Error rejecting promotion:', error);
    } finally {
      setIsSubmitting(false);
    }
  };

  if (!isOpen) return null;

  return (
    <div className="bg-[#00000080] fixed inset-0 flex items-center justify-center z-[100]">
      <div className="p-[12px] pt-[0] bg-[var(--modal-background)] border-[1px] border-[var(--modal-border-color)] shadow-[var(--modal-box-shadow)] max-w-[400px] w-full mx-4 rounded-[3px] dark:bg-[#121212]">
        <div className="relative mb-[10px] text-center border-b-[1px] border-[var(--border-color)] py-[11px]">
          <TitleComponent text={`${t('editModal.title')} ${transId}`} />
          <ButtonIconComponent style='Close' onClick={onClose} />
        </div>

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
              {t('editModal.suggested')}: {displayAmount(currentAmount)}
            </p>
          </div>
        </div>

        <div className="flex justify-center gap-[8px]">
          <ButtonComponent
            onClick={handleReject}
            text={t('editModal.reject')}
            variant="cancel"
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
