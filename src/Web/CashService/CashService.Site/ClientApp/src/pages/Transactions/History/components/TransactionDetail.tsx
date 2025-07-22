import 'react-confirm-alert/src/react-confirm-alert.css';

import React, { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';

import {
  ButtonComponent,
} from '../../../../components/Buttons/index';

import { MessageComponent } from '../../../../components/Message';
import { TransactionItem } from '../../../../services/interfaces/Transactions/TransactionHistory';
import { transactionAPI } from '../../../../services/transactionsHistoryAPI';
import {
  displayAmount,
  formatDateTime,
  mappingStatus,
  renderBankAccount,
  TransactionType,
} from '../../../../utils/commonFunctions';
import { TitleComponent } from '../../../../components/Title'; 
import LoadingComponent from '../../../../components/LoadingComponent';
import ConfirmPopup from '../../../../components/ConfirmPopup';
import { ButtonIconComponent } from '../../../../components/Buttons/ButtonIconComponent';

interface TransactionDetailProps {
  transactionId: number | null;
  custId: number | null;
  isOpen: boolean;
  transactionType: number;
  onClose: () => void;
  onRevertSuccess?: () => void;
  isCashServiceEnabled: boolean;
}

const TransactionDetail: React.FC<TransactionDetailProps> = ({
  transactionId,
  custId,
  isOpen,
  transactionType,
  onClose,
  onRevertSuccess,
  isCashServiceEnabled,
}) => {
  const { t } = useTranslation('transactionHistory');
  const { t: tGlobal } = useTranslation('global');
  const [detailData, setDetailData] = useState<TransactionItem | null>(null);
  const [loadingDetail, setLoadingDetail] = useState(false);
  const [detailError, setDetailError] = useState<string | null>(null);
  const [reverting, setReverting] = useState(false);
  const [revertAlert, setRevertAlert] = useState<{
    type: 'success' | 'error' | null;
    message: string;
  }>({ type: null, message: '' });
  const [remarks, setRemarks] = useState('');
  const [showConfirm, setShowConfirm] = useState(false);

  useEffect(() => {
    if (isOpen && transactionId !== null && custId !== null) {
      setLoadingDetail(true);
      setDetailError(null);
      setDetailData(null);
      setRevertAlert({ type: null, message: '' });
      transactionAPI
        .getTransactionDetail(transactionId, custId)
        .then((response) => {
          setDetailData(response.data);
          setRemarks(response.data.remark || '');
        })
        .catch((err) => {
          console.error(err);
          setDetailError(t('detail.error'));
        })
        .finally(() => {
          setLoadingDetail(false);
        });
    } else {
      setDetailData(null);
      setDetailError(null);
      setLoadingDetail(false);
      setReverting(false);
      setRemarks('');
    }
  }, [isOpen, transactionId, t]);

  const handleConfirmRevert = async () => {
    if (transactionId === null) return;
    setReverting(true);
    setRevertAlert({ type: null, message: '' });
    try {
      const response = await transactionAPI.revertTransaction(
        transactionId,
        remarks,
        transactionType === TransactionType.Deposit
      );
      if (response.isSuccessful) {
        setRevertAlert({
          type: 'success',
          message: t('detail.revertSuccess'),
        });
        setTimeout(() => {
          onRevertSuccess?.();
          onClose();
        }, 3000);
      } else {
        setReverting(false);
        setRevertAlert({ type: 'error', message: response.message });
      }
    } catch (err) {
      console.error(err);
      setReverting(false);
      setRevertAlert({ type: 'error', message: t('detail.revertError') });
    }
  };

  const handleRevert = () => {
    setShowConfirm(true);
  };

  if (!isOpen) return null;

  const bodyClassName =
    detailData && detailData.base64Payslip ? 'w-[850px]' : 'max-w-[540px] max-[768px]:max-w-[90%] ';

  return (
    <>
      {showConfirm && (
        <ConfirmPopup
          title={t('detail.confirmRevert')}
          message={t('detail.confirmRevertMessage').replace(
            '{{transactionId}}',
            detailData?.transId?.toString() || ''
          )}
          onCancel={() => setShowConfirm(false)}
          onConfirm={() => {
            setShowConfirm(false);
            handleConfirmRevert();
          }}
        />
      )}
      <div className="bg-[#00000080] fixed inset-0 flex justify-center z-50 items-center">
        <div
          className={`${bodyClassName} p-[12px] pt-[0] bg-[var(--modal-background)] border-[1px] border-[var(--modal-border-color)] shadow-[var(--modal-box-shadow)] max-[768px]:max-w-[90%] max-h-[90%] overflow-auto rounded-[4px]`}
        >
          <div className="relative mb-[10px] border-b-[1px] border-[var(--border-color)] py-[11px] flex items-center">
            <TitleComponent type='medium'
              className="w-auto!"
              text={`${t('detail.title')} (${transactionId?.toString() ?? t('detail.loading')})`}
            />
            <div className="relative ml-[10px]">
              {detailData && mappingStatus(detailData.statusId, detailData.status)}
            </div>
            <ButtonIconComponent style='Close' onClick={onClose} />
          </div>

          {revertAlert.type && (
            <MessageComponent type={revertAlert.type} message={revertAlert.message} className="mb-[10px] max-[768px]:mb-[0]" />
          )}

          {loadingDetail ? (
            <LoadingComponent />
          ) : detailError ? (
            <MessageComponent type="error" message={detailError} className="mb-4" />
          ) : !detailData ? (
            <MessageComponent type="warning" message={t('detail.noDetails')} className="mb-4" />
          ) : (
            <>
              <div className="flex gap-6">
                <div className="w-[460px] h-full">
                  <div className="border border-[var(--border-control)] rounded-[4px] mb-[8px] p-[8px] flex flex-wrap ">
                    <div className="w-[50%] mb-[8px] flex flex-col">
                      <label className="font-semibold text-[var(--text-color-secondary)]">
                        {t('detail.username')}:
                      </label>
                      <span>{detailData.username}</span>
                    </div>
                    <div className="w-[50%] mb-[8px] flex flex-col">
                      <label className="font-semibold text-[var(--text-color-secondary)]">
                        {t('detail.nickname')}:
                      </label>
                      <span>{detailData.nickname}</span>
                    </div>
                    <div className="w-[50%] mb-[8px] flex flex-col">
                      <label className="font-semibold text-[var(--text-color-secondary)]">
                        {t('detail.fullName')}:
                      </label>
                      <span>{`${detailData.firstName} ${detailData.lastName}`}</span>
                    </div>
                    <div className="w-[50%] mb-[8px] flex flex-col  ">
                      <label className="font-semibold text-[var(--text-color-secondary)]">
                        {t('detail.betCredit')}:
                      </label>
                      <span className="ml-[4px] text-[var(--text-link-color)] font-semibold">
                        {displayAmount(detailData.betCredit)}
                      </span>
                    </div>
                  </div>
                  <div className="flex flex-wrap gap-[10px] mb-[10px]">
                    <div className="w-[48%] flex flex-col">
                      <TitleComponent type='label' text={t('detail.from')} />
                      <div className="flex items-center">
                        {renderBankAccount(detailData.paymentFrom, tGlobal)}
                      </div>
                    </div>
                    <div className="w-[48%]">
                      <TitleComponent type='label' text={t('detail.to')} />
                      <div className="flex items-center">
                        {renderBankAccount(detailData.paymentTo, tGlobal)}
                      </div>
                    </div>
                    <div className="w-[48%]">
                      <TitleComponent type='label' text={t('detail.dateAndTime')} />
                      <div>{formatDateTime(detailData.createdTime)}</div>
                    </div>
                    <div className="w-[48%]">
                      <TitleComponent type='label' text={t('detail.operatedBy')} />
                      <div>{detailData.createdBy}</div>
                    </div>
                    <div className="w-[48%]">
                      <TitleComponent type='label' text={t('detail.transactionType')} />
                      <div className="text-[var(--text-link-color)] font-semibold">
                        {detailData.transactionType}
                      </div>
                    </div>
                    <div className="w-[48%]">
                      <TitleComponent type='label' text={t('detail.amount')} />
                      <div className="text-[var(--text-link-color)] font-semibold">
                        {displayAmount(detailData.amount)}
                      </div>
                    </div>
                    <div className="w-[48%]">
                      <TitleComponent type='label' text={t('detail.processedOn')} />
                      <div>{formatDateTime(detailData.operatedTime)}</div>
                    </div>
                  </div>
                  <div className="mb-[10px]">
                    <TitleComponent type='label' text={t('detail.remarks')} />
                    <textarea
                      value={remarks}
                      onChange={(e) => setRemarks(e.target.value)}
                      placeholder={t('detail.remarksPlaceholder')}
                      className="block h-[150px]! py-[10px] control-field"
                      disabled={reverting || !detailData?.isAllowRevert}
                    />
                  </div>
                </div>

                {detailData.base64Payslip && (
                  <div className="w-[400px] h-full">
                    <img
                      src={detailData.base64Payslip}
                      alt={detailData.transId.toString()}
                      className="w-full h-full object-contain"
                    />
                  </div>
                )}
              </div>
            </>
          )}

          <div className="flex justify-center gap-[8px]">
            <ButtonComponent
              variant="cancel"
              onClick={onClose}
              disabled={reverting}
              text={t('detail.cancel')}
            />
            {detailData?.isAllowRevert && (
              <ButtonComponent
                variant="apply"
                onClick={handleRevert}
                disabled={reverting || !detailData || !isCashServiceEnabled}
                text={t('detail.revertToPending')}
                title={isCashServiceEnabled ? '' : tGlobal('cashServiceDisabled')}
              />
            )}
          </div>
        </div>
      </div>
    </>
  );
};

export default TransactionDetail;
