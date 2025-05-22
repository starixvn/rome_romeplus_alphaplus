import React, { useState, useEffect } from 'react';
import { useTranslation } from 'react-i18next';
import { TransactionItem } from '../../../../services/interfaces/Transactions/TransactionHistory';
import { transactionAPI } from '../../../../services/transactionsHistoryAPI';
import {
  displayAmount,
  formatDateTime,
  getStatusStyle,
  mappingStatus,
  renderBankAccount,
} from '../../../../common/commonFunctions';
import { IconClose } from '../../../../components/Icons';
import { ResultMessageComponent } from '../../../../components/MessageComponents';
import { Spinner } from '../../../../components/Icons';

interface TransactionDetailProps {
  transactionId: number | null;
  custId: number | null;
  isOpen: boolean;
  onClose: () => void;
  onRevertSuccess?: () => void;
}

const TransactionDetail: React.FC<TransactionDetailProps> = ({
  transactionId,
  custId,
  isOpen,
  onClose,
  onRevertSuccess,
}) => {
  const { t } = useTranslation('transactionHistory');
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
        .then((data) => {
          setDetailData(data);
          setRemarks(data.remark || '');
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

  const handleRevert = async () => {
    if (transactionId === null) return;

    setReverting(true);
    setRevertAlert({ type: null, message: '' });
    try {
      var result = await transactionAPI.revertTransaction(transactionId, remarks);
      if (result.isSuccessful) {
        setRevertAlert({
          type: 'success',
          message: t('successMessages.reverted'),
        });
        setTimeout(() => {
          onRevertSuccess?.();
          onClose();
        }, 3000);
      } else {
        setRevertAlert({ type: 'error', message: result.message });
        setReverting(false);
      }
    } catch (err) {
      console.error(err);
      setRevertAlert({ type: 'error', message: t('errorMessages.failedToRevert') });
      setReverting(false);
    }
  };

  if (!isOpen) return null;

  return (
    <div className="bg-[#00000066] fixed z-[100] top-0 left-0 right-0 bottom-0 flex items-start justify-center">
      <div className="p-[30px] bg-[#ffffff] mt-[100px] max-w-[540px] max-[768px]:max-w-[90%] rounded-[4px] shadow-[0_2px_4px_-1px_#00000033,0_4px_5px_#00000024,0_1px_10px_#0000001f]">
        <div className="relative mb-[16px]">
          <h5 className="text-[16px] text-[var(--text-color-primary)] font-semibold leading-[21.6px] max-[768px]:text-[20px] max-[768px]:leading-[26px]">
            {t('detail.title')} ({transactionId?.toString() ?? t('detail.loading')})
            {detailData && (
              <span
                className={`ml-2 px-2 py-1 rounded-full text-xs font-medium ${getStatusStyle(detailData.statusId)}`}
              >
                {mappingStatus(detailData.statusId)}
              </span>
            )}
          </h5>
          <button
            type="button"
            className="absolute top-[-20px] right-[-20px] cursor-pointer"
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
            className="mb-4"
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
            <div className="border border-[#333333] rounded mb-[8px] p-[8px]">
              <table className="w-full">
                <thead>
                  <tr className="text-left text-[#333333]">
                    <th className="font-bold px-[8px] py-[4px]">{t('detail.username')}</th>
                    <th className="font-bold px-[8px] py-[4px]">{t('detail.loginName')}</th>
                    <th className="font-bold px-[8px] py-[4px]">{t('detail.fullName')}</th>
                    <th className="font-bold px-[8px] py-[4px]">{t('detail.betCredit')}</th>
                  </tr>
                </thead>
                <tbody>
                  <tr>
                    <td className="px-[8px] py-[4px]">{detailData.username}</td>
                    <td className="px-[8px] py-[4px]">{detailData.username}</td>
                    <td className="px-[8px] py-[4px]">{`${detailData.firstName} ${detailData.lastName}`}</td>
                    <td className="px-[8px] py-[4px] text-[var(--text-color-primary)] font-semibold">
                      {displayAmount(detailData.betCredit)}
                    </td>
                  </tr>
                </tbody>
              </table>
            </div>

            <div className="flex justify-between">
              <div className="w-[230px] max-[768px]:w-[48%]">
                <div className="mb-[8px]">
                  <div className="block font-bold text-[#333333] my-[8px]">{t('detail.from')}</div>
                  <div className="flex items-center">
                    {renderBankAccount(detailData.paymentFrom)}
                  </div>
                </div>

                <div>
                  <div className="block font-bold text-[#333333] my-[8px]">
                    {t('detail.dateAndTime')}
                  </div>
                  <div>{formatDateTime(detailData.createdTime)}</div>
                </div>

                <div>
                  <div className="block font-bold text-[#333333] my-[8px]">
                    {t('detail.transactionType')}
                  </div>
                  <div>{detailData.transactionType}</div>
                </div>

                <div>
                  <div className="block font-bold text-[#333333] my-[8px]">
                    {t('detail.operatedBy')}
                  </div>
                  <div>{detailData.username}</div>
                </div>
              </div>

              <div className="w-[230px] max-[768px]:w-[48%]">
                <div className="mb-[8px]">
                  <div className="block font-bold text-[#333333] my-[8px]">{t('detail.to')}</div>
                  <div className="flex items-center">{renderBankAccount(detailData.paymentTo)}</div>
                </div>

                <div>
                  <div className="block font-bold text-[#333333] my-[8px]">
                    {t('detail.processedOn')}
                  </div>
                  <div>{formatDateTime(detailData.lastUpdatedDate)}</div>
                </div>

                <div>
                  <div className="block font-bold text-[#333333] my-[8px]">
                    {t('detail.amount')}
                  </div>
                  <div>{displayAmount(detailData.amount)}</div>
                </div>
              </div>
            </div>

            <div>
              <div className="block font-bold text-[#333333] my-[8px]">{t('detail.remarks')}</div>
              <textarea
                value={remarks}
                onChange={(e) => setRemarks(e.target.value)}
                placeholder={t('detail.remarksPlaceholder')}
                className="block h-[150px] max-[768px]:h-[40px] w-[100%] border-[1px] border-[var(--border-control)] rounded-[4px] px-[8px] py-[10px] focus:outline-none focus:border-[var(--button-border)]"
                disabled={reverting}
              />
            </div>
          </>
        )}

        <div className="flex justify-center gap-[8px] mt-6">
          <button
            onClick={onClose}
            className="w-[140px] capitalize font-bold h-[32px] max-[768px]:h-[40px] rounded-[20px] px-[16px] text-[#163f78] py-[4px] text-[12px] border-[1px] border-[#163f78] mx-[4px] bg-[#ffffff] hover:bg-[#dee2e7] cursor-pointer shadow-[0_1px_5px_#0003,0_2px_2px_#00000024,0_3px_1px_-2px_#0000001f] disabled:opacity-50 disabled:cursor-not-allowed"
            disabled={reverting}
          >
            {t('detail.cancel')}
          </button>
          {detailData?.isAllowRevert && (
            <button
              onClick={handleRevert}
              className="w-[140px] capitalize font-bold h-[32px] max-[768px]:h-[40px] rounded-[20px] text-white px-[16px] py-[4px] text-[12px] border-[1px] border-[#163f78] mx-[4px] bg-[#163f78] hover:bg-[var(--button-background-hover)] cursor-pointer shadow-[0_1px_5px_#0003,0_2px_2px_#00000024,0_3px_1px_-2px_#0000001f] disabled:opacity-50 disabled:cursor-not-allowed"
              disabled={reverting || !detailData}
            >
              {reverting ? <Spinner size="small" className="mr-2" /> : null}
              {t('detail.revertToPending')}
            </button>
          )}
        </div>
      </div>
    </div>
  );
};

export default TransactionDetail;
