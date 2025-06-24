import 'react-confirm-alert/src/react-confirm-alert.css';

import React, { useEffect, useState } from 'react';
import { confirmAlert } from 'react-confirm-alert';
import { useTranslation } from 'react-i18next';

import {
  ButtonApplyComponent,
  ButtonCancelComponent,
  ButtonClosePopupComponent,
} from '../../../../components/ButtonComponents';

import { ResultMessageComponent } from '../../../../components/MessageComponents';
import { TransactionItem } from '../../../../services/interfaces/Transactions/TransactionHistory';
import { transactionAPI } from '../../../../services/transactionsHistoryAPI';
import {
  displayAmount,
  formatDateTime,
  mappingStatus,
  renderBankAccount,
  TransactionType,
} from '../../../../utils/commonFunctions';
import { LabelTitleComponent, MediumTitleComponent } from '../../../../components/TitleComponents';
import LoadingComponent from '../../../../components/LoadingComponent';

interface TransactionDetailProps {
  transactionId: number | null;
  custId: number | null;
  isOpen: boolean;
  transactionType: number;
  onClose: () => void;
  onRevertSuccess?: () => void;
}

const TransactionDetail: React.FC<TransactionDetailProps> = ({
  transactionId,
  custId,
  isOpen,
  transactionType,
  onClose,
  onRevertSuccess,
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
        setRevertAlert({ type: 'error', message: response.message });
      }
    } catch (err) {
      console.error(err);
      setRevertAlert({ type: 'error', message: t('detail.revertError') });
    } finally {
      setReverting(false);
    }
  };

  const renderConfirmUI = ({ onClose: closeConfirm }: { onClose: () => void }) => (
    <div className="bg-[#00000080] fixed inset-0 flex items-start justify-center z-50">
      <div className="p-[12px] pt-[0] bg-[var(--modal-background)] border-[1px] border-[var(--modal-border-color)] shadow-[var(--modal-box-shadow)] mt-[100px] max-[768px]:max-w-[90%] overflow-auto rounded-[4px]">
        <div className="relative mb-[10px] border-b-[1px] border-[var(--border-color)] py-[11px]">
          <MediumTitleComponent text={t('detail.confirmRevert')} />
          <ButtonClosePopupComponent onClick={closeConfirm} />
        </div>
        <p className="mb-[20px]">
          {t('detail.confirmRevertMessage').replace(
            '{{transactionId}}',
            detailData?.transId?.toString() || ''
          )}
        </p>
        <div className="flex justify-center gap-[8px]">
          <ButtonCancelComponent onClick={closeConfirm} text={tGlobal('cancel')} />
          <ButtonApplyComponent
            onClick={() => {
              closeConfirm();
              handleConfirmRevert();
            }}
            text={tGlobal('confirm')}
          />
        </div>
      </div>
    </div>
  );

  const handleRevert = () => {
    confirmAlert({
      customUI: renderConfirmUI,
    });
  };

  if (!isOpen) return null;

  const bodyClassName =
    detailData && detailData.base64Payslip ? 'w-[850px]' : 'max-w-[540px] max-[768px]:max-w-[90%] ';

  return (
    <div className="bg-[#00000080] fixed inset-0 flex items-start justify-center z-50">
      <div
        className={`${bodyClassName} p-[12px] pt-[0] bg-[var(--modal-background)] border-[1px] border-[var(--modal-border-color)] shadow-[var(--modal-box-shadow)] mt-[100px] max-[768px]:max-w-[90%] overflow-auto rounded-[4px]`}
      >
        <div className="relative mb-[10px] border-b-[1px] border-[var(--border-color)] py-[11px] flex items-center">
          <MediumTitleComponent
            className="w-auto!"
            text={`${t('detail.title')} (${transactionId?.toString() ?? t('detail.loading')})`}
          />
          <div className="relative ml-[10px]">
            {detailData && mappingStatus(detailData.statusId)}
          </div>
          <ButtonClosePopupComponent onClick={onClose} />
        </div>

        {revertAlert.type && (
          <ResultMessageComponent
            type={revertAlert.type}
            message={revertAlert.message}
            className="mb-[10px] max-[768px]:mb-[0]"
          />
        )}

        {loadingDetail ? (
          <LoadingComponent />
        ) : detailError ? (
          <ResultMessageComponent type="error" message={detailError} className="mb-4" />
        ) : !detailData ? (
          <ResultMessageComponent type="warning" message={t('detail.noDetails')} className="mb-4" />
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
                    <LabelTitleComponent text={t('detail.from')} />
                    <div className="flex items-center">
                      {renderBankAccount(detailData.paymentFrom)}
                    </div>
                  </div>
                  <div className="w-[48%]">
                    <LabelTitleComponent text={t('detail.to')} />
                    <div className="flex items-center">
                      {renderBankAccount(detailData.paymentTo)}
                    </div>
                  </div>
                  <div className="w-[48%]">
                    <LabelTitleComponent text={t('detail.dateAndTime')} />
                    <div>{formatDateTime(detailData.createdTime)}</div>
                  </div>
                  <div className="w-[48%]">
                    <LabelTitleComponent text={t('detail.operatedBy')} />
                    <div>{detailData.createdBy}</div>
                  </div>
                  <div className="w-[48%]">
                    <LabelTitleComponent text={t('detail.transactionType')} />
                    <div className="text-[var(--text-link-color)] font-semibold">
                      {detailData.transactionType}
                    </div>
                  </div>
                  <div className="w-[48%]">
                    <LabelTitleComponent text={t('detail.amount')} />
                    <div className="text-[var(--text-link-color)] font-semibold">
                      {displayAmount(detailData.amount)}
                    </div>
                  </div>
                  <div className="w-[48%]">
                    <LabelTitleComponent text={t('detail.processedOn')} />
                    <div>{formatDateTime(detailData.operatedTime)}</div>
                  </div>
                </div>
                <div className="mb-[10px]">
                  <LabelTitleComponent text={t('detail.remarks')} />
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
                    src={`data:image/png;base64,${detailData.base64Payslip}`}
                    alt={detailData.transId.toString()}
                    className="w-full h-full object-contain"
                  />
                </div>
              )}
            </div>
          </>
        )}

        <div className="flex justify-center gap-[8px]">
          <ButtonCancelComponent onClick={onClose} disabled={reverting} text={t('detail.cancel')} />
          {detailData?.isAllowRevert && (
            <ButtonApplyComponent
              onClick={handleRevert}
              disabled={reverting || !detailData}
              text={t('detail.revertToPending')}
            />
          )}
        </div>
      </div>
    </div>
  );
};

export default TransactionDetail;
