import 'react-confirm-alert/src/react-confirm-alert.css';

import React, { useEffect, useState } from 'react';
import { confirmAlert } from 'react-confirm-alert';
import { useTranslation } from 'react-i18next';

import {
    ButtonApplyComponent, ButtonCancelComponent
} from '../../../../components/ButtonComponents';
import { IconClose, Spinner } from '../../../../components/Icons';
import { ResultMessageComponent } from '../../../../components/MessageComponents';
import { TransactionItem } from '../../../../services/interfaces/Transactions/TransactionHistory';
import { transactionAPI } from '../../../../services/transactionsHistoryAPI';
import {
    displayAmount, formatDateTime, mappingStatus, renderBankAccount, TransactionType
} from '../../../../utils/commonFunctions';

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
    <div className="fixed inset-0 flex items-center justify-center bg-[#00000080] z-[1000]">
      <div className="bg-white rounded-[4px] p-[20px] pt-[4px] pb-[10px] w-[400px] shadow-[0_2px_4px_-1px_#00000033,0_4px_5px_#00000024,0_1px_10px_#0000001f]">
        <h1 className="font-[500] text-[var(--text-color-primary)] text-[16px] my-[10px]">
          {t('detail.confirmRevert')}
        </h1>
        <p className="mb-6 text-[#333333]">
          {t('detail.confirmRevertMessage').replace(
            '{{transactionId}}',
            detailData?.transId?.toString() || ''
          )}
        </p>
        <div className="flex justify-center gap-2">
          <button
            className="min-w-[90px] bg-[var(--button-background-cancel)] hover:bg-[var(--button-background-cancel-hover)] text-[var(--button-text)] rounded-[3px] cursor-pointer h-[28px] px-[16px] font-semibold"
            onClick={closeConfirm}
          >
            {tGlobal('cancel')}
          </button>
          <button
            className="min-w-[90px] bg-[var(--button-background)] hover:bg-[var(--button-background-hover)] text-[var(--button-text)] rounded-[3px] cursor-pointer h-[28px] px-[16px] font-semibold"
            onClick={() => {
              closeConfirm();
              handleConfirmRevert();
            }}
          >
            {tGlobal('confirm')}
          </button>
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
    detailData && detailData.base64Payslip
      ? 'bg-white w-[850px]'
      : 'bg-[#ffffff] mt-[100px] max-w-[540px] max-[768px]:max-w-[90%] ';

  return (
    <div className="bg-[#00000066] fixed inset-0 flex items-center justify-center z-50">
      <div
        className={`${bodyClassName} p-[20px] rounded-[4px] shadow-[0_2px_4px_-1px_#00000033,0_4px_5px_#00000024,0_1px_10px_#0000001f]`}
      >
        <div className="relative">
          <h5 className="text-[16px] text-[#163f78] font-bold pb-[15px]">
            {t('detail.title')} ({transactionId?.toString() ?? t('detail.loading')})
            {detailData && (
              <span className={'text-xs font-medium inline-block ml-2'}>
                {mappingStatus(detailData.statusId)}
              </span>
            )}
          </h5>
          <button
            type="button"
            className="absolute top-[-15px] right-[-15px] cursor-pointer"
            onClick={onClose}
            disabled={reverting}
          >
            <IconClose />
          </button>
        </div>

        {revertAlert.type && (
          <ResultMessageComponent
            type={revertAlert.type}
            message={revertAlert.message}
            className="mb-5"
          />
        )}

        {loadingDetail ? (
          <div className="text-center p-10">
            <Spinner size="large" />
          </div>
        ) : detailError ? (
          <ResultMessageComponent type="error" message={detailError} className="mb-4" />
        ) : !detailData ? (
          <ResultMessageComponent type="warning" message={t('detail.noDetails')} className="mb-4" />
        ) : (
          <>
            <div className="flex gap-6">
              <div className="w-[460px] h-full">
                <div className="border border-[#214184] rounded mb-6">
                  <table className="w-full">
                    <thead>
                      <tr className="text-left text-[#333333]">
                        <th className="font-bold px-[8px] py-[4px]">{t('detail.username')}</th>
                        <th className="font-bold px-[8px] py-[4px]">{t('detail.nickname')}</th>
                        <th className="font-bold px-[8px] py-[4px]">{t('detail.fullName')}</th>
                        <th className="font-bold px-[8px] py-[4px]">{t('detail.betCredit')}</th>
                      </tr>
                    </thead>
                    <tbody>
                      <tr>
                        <td className="px-[8px] py-[4px]">
                          <div>{detailData.username}</div>
                        </td>
                        <td className="px-[8px] py-[4px]">{detailData.nickname}</td>
                        <td className="px-[8px] py-[4px]">{`${detailData.firstName} ${detailData.lastName}`}</td>
                        <td className="px-[8px] py-[4px] text-[var(--text-color-primary)] font-semibold">
                          {displayAmount(detailData.betCredit)}
                        </td>
                      </tr>
                    </tbody>
                  </table>
                </div>

                <div className="grid grid-cols-2 gap-6">
                  <div>
                    <div className="mb-4">
                      <div className="text-sm font-medium mb-1">{t('detail.from')}</div>
                      <div className="flex items-center">
                        {renderBankAccount(detailData.paymentFrom)}
                      </div>
                    </div>

                    <div className="mb-4">
                      <div className="text-sm font-medium mb-1">{t('detail.dateAndTime')}</div>
                      <div>{formatDateTime(detailData.createdTime)}</div>
                    </div>

                    <div className="mb-4">
                      <div className="text-sm font-medium mb-1">{t('detail.transactionType')}</div>
                      <div>{detailData.transactionType}</div>
                    </div>
                    <div className="mb-4">
                      <div className="text-sm font-medium mb-1">{t('detail.processedOn')}</div>
                      <div>{formatDateTime(detailData.operatedTime)}</div>
                    </div>
                  </div>

                  <div>
                    <div className="mb-4">
                      <div className="text-sm font-medium mb-1">{t('detail.to')}</div>
                      <div className="flex items-center">
                        {renderBankAccount(detailData.paymentTo)}
                      </div>
                    </div>

                    <div className="mb-4">
                      <div className="text-sm font-medium mb-1">{t('detail.operatedBy')}</div>
                      <div>{detailData.operatedBy}</div>
                    </div>

                    <div className="mb-4">
                      <div className="text-sm font-medium mb-1">{t('detail.amount')}</div>
                      <div>{displayAmount(detailData.amount)}</div>
                    </div>
                  </div>
                </div>

                <div className="mb-4">
                  <div className="text-sm font-medium mb-1">{t('detail.remarks')}</div>
                  <textarea
                    value={remarks}
                    onChange={(e) => setRemarks(e.target.value)}
                    placeholder={t('detail.remarksPlaceholder')}
                    className="block h-[150px] max-[768px]:h-[40px] w-[100%] border-[1px] border-[var(--border-control)] rounded-[4px] px-[8px] py-[10px] focus:outline-none focus:border-[var(--button-border)]"
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

        <div className="flex justify-center gap-[8px] mt-6">
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
