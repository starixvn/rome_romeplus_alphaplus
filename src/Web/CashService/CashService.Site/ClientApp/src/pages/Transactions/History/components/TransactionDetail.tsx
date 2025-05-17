import React, { useState, useEffect } from 'react';
import { Dialog } from '../../../../components/ui/Dialog';
import { Spinner } from '../../../../components/Icons';
import { Alert } from '../../../../components/ui/Alert';
import { TransactionItem } from '../../../../services/interfaces/Transactions/TransactionHistory';
import { transactionAPI } from '../../../../services/transactionsHistoryAPI';
import { useTranslation } from 'react-i18next';
import { displayAmount, mappingStatus } from '../../../../common/commonFunctions';

interface TransactionDetailProps {
  transactionId: number | null;
  custId: number | null;
  isOpen: boolean;
  onClose: () => void;
  onRevertSuccess?: () => void;
}

const formatDateTime = (dateTimeStr: string): string => {
  const date = new Date(dateTimeStr);
  const month = (date.getMonth() + 1).toString().padStart(2, '0');
  const day = date.getDate().toString().padStart(2, '0');
  const year = date.getFullYear();
  let hours = date.getHours();
  const minutes = date.getMinutes().toString().padStart(2, '0');
  const seconds = date.getSeconds().toString().padStart(2, '0');
  const ampm = hours >= 12 ? 'PM' : 'AM';
  hours = hours % 12;
  hours = hours ? hours : 12;
  return `${month}/${day}/${year} ${hours}:${minutes}:${seconds} ${ampm}`;
};

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

  const renderContent = () => {
    if (loadingDetail) {
      return (
        <div className="text-center p-10">
          <Spinner size="large" />
        </div>
      );
    }
    if (detailError) {
      return <Alert message={detailError} type="error" showIcon className="mb-4" />;
    }
    if (!detailData) {
      return <Alert message={t('detail.noDetails')} type="warning" showIcon className="mb-4" />;
    }

    return (
      <div className="p-4">
        {revertAlert.type && (
          <Alert
            message={revertAlert.message}
            type={revertAlert.type}
            showIcon
            style={{ marginBottom: '20px' }}
          />
        )}
        <div className="bg-white rounded-lg border border-gray-200 p-4 mb-4">
          <div className="grid grid-cols-4 gap-4">
            <div>
              <div className="text-sm text-gray-600">{t('detail.username')}</div>
              <div className="font-medium">{detailData.username}</div>
            </div>
            <div>
              <div className="text-sm text-gray-600">{t('detail.loginName')}</div>
              <div className="font-medium">{detailData.username}</div>
            </div>
            <div>
              <div className="text-sm text-gray-600">{t('detail.fullName')}</div>
              <div className="font-medium">{`${detailData.firstName} ${detailData.lastName}`}</div>
            </div>
            <div>
              <div className="text-sm text-gray-600">{t('detail.betCredit')}</div>
              <div className="font-medium">{displayAmount(detailData.betCredit)}</div>
            </div>
          </div>
        </div>

        <div className="grid grid-cols-2 gap-4 mb-4">
          <div>
            <div className="mb-4">
              <div className="text-sm text-gray-600">{t('detail.from')}</div>
              <div className="flex items-center gap-2">
                <div className="w-4 h-4 rounded-full bg-green-500"></div>
                <span className="font-medium">{detailData.paymentFrom}</span>
              </div>
            </div>
            <div className="mb-4">
              <div className="text-sm text-gray-600">{t('detail.dateAndTime')}</div>
              <div className="font-medium">{formatDateTime(detailData.createdTime)}</div>
            </div>
            <div className="mb-4">
              <div className="text-sm text-gray-600">{t('detail.transactionType')}</div>
              <div className="font-medium">{detailData.transactionType}</div>
            </div>
            <div className="mb-4">
              <div className="text-sm text-gray-600">{t('detail.operatedBy')}</div>
              <div className="font-medium">{detailData.username?.substring(0, 9)}</div>
            </div>
          </div>
          <div>
            <div className="mb-4">
              <div className="text-sm text-gray-600">{t('detail.to')}</div>
              <div className="flex items-center gap-2">
                <div className="w-4 h-4 rounded-full bg-green-500"></div>
                <span className="font-medium">{detailData.paymentTo}</span>
              </div>
            </div>
            <div className="mb-4">
              <div className="text-sm text-gray-600">{t('detail.processedOn')}</div>
              <div className="font-medium">{formatDateTime(detailData.lastUpdatedDate)}</div>
            </div>
            <div className="mb-4">
              <div className="text-sm text-gray-600">{t('detail.amount')}</div>
              <div className="font-medium">{`${detailData.amount.toFixed(2)} ${detailData.currency}`}</div>
            </div>
          </div>
        </div>

        <div className="mb-4">
          <div className="text-sm text-gray-600 mb-2">{t('detail.remarks')}</div>
          <textarea
            value={remarks}
            onChange={(e) => setRemarks(e.target.value)}
            className="w-full h-24 p-2 border border-gray-300 rounded-md"
            placeholder={t('detail.remarksPlaceholder')}
          />
        </div>
      </div>
    );
  };

  return (
    <Dialog
      open={isOpen}
      onOpenChange={(open: boolean) => {
        if (!open) onClose();
      }}
      title={
        <div className="flex items-center gap-2">
          <span>
            {t('detail.title')} ({detailData?.transId.toString() ?? t('detail.loading')})
          </span>
          {detailData && (
            <span className={`status-badge ${mappingStatus(detailData.statusId).toLowerCase()}`}>
              {mappingStatus(detailData.statusId)}
            </span>
          )}
        </div>
      }
      description=""
      footer={[
        <button
          key="cancel"
          onClick={onClose}
          className={`px-4 py-2 text-gray-700 bg-gray-100 rounded-md hover:bg-gray-200 focus:outline-none focus:ring-2 focus:ring-gray-400 transition duration-150 ease-in-out ${reverting ? 'opacity-50 cursor-not-allowed' : 'cursor-pointer'}`}
          disabled={reverting}
        >
          {t('detail.cancel')}
        </button>,
        <button
          key="revert"
          onClick={handleRevert}
          className={`px-4 py-2 ml-2 text-white bg-blue-600 rounded-md hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:ring-opacity-50 transition duration-150 ease-in-out ${reverting ? 'opacity-50 cursor-not-allowed' : 'cursor-pointer'}`}
          disabled={reverting || !detailData}
        >
          {reverting ? <Spinner size="small" className="mr-2" /> : null}
          {t('detail.revertToPending')}
        </button>,
      ]}
      size="2xl"
    >
      {renderContent()}
    </Dialog>
  );
};

export default TransactionDetail;
