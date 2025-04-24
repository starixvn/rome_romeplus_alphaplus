import React, { useState, useEffect } from 'react';
import { Modal, Spin, Alert, message } from 'antd';
import { TransactionItem } from '../../../../interfaces/TransactionHistory';
import {
  getTransactionDetail,
  revertTransaction,
} from '../../../../services/transactionsHistoryAPI';

interface TransactionDetailProps {
  transactionId: number | null;
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
  isOpen,
  onClose,
  onRevertSuccess,
}) => {
  const [detailData, setDetailData] = useState<TransactionItem | null>(null);
  const [loadingDetail, setLoadingDetail] = useState(false);
  const [detailError, setDetailError] = useState<string | null>(null);
  const [reverting, setReverting] = useState(false);

  useEffect(() => {
    if (isOpen && transactionId !== null) {
      setLoadingDetail(true);
      setDetailError(null);
      setDetailData(null);
      getTransactionDetail(transactionId)
        .then((data) => {
          setDetailData(data);
        })
        .catch((err) => {
          console.error(err);
          setDetailError('Failed to load transaction details.');
        })
        .finally(() => {
          setLoadingDetail(false);
        });
    } else {
      setDetailData(null);
      setDetailError(null);
      setLoadingDetail(false);
      setReverting(false);
    }
  }, [isOpen, transactionId]);

  const handleRevert = async () => {
    if (transactionId === null) return;

    setReverting(true);
    try {
      await revertTransaction(transactionId);
      message.success('Transaction reverted to pending successfully!');
      onRevertSuccess?.();
      onClose();
    } catch (err) {
      console.error(err);
      message.error('Failed to revert transaction.');
    } finally {
      setReverting(false);
    }
  };

  const renderContent = () => {
    if (loadingDetail) {
      return (
        <div className="text-center p-10">
          <Spin size="large" />
        </div>
      );
    }
    if (detailError) {
      return <Alert message={detailError} type="error" showIcon className="mb-4" />;
    }
    if (!detailData) {
      return (
        <Alert
          message="No transaction details available."
          type="warning"
          showIcon
          className="mb-4"
        />
      );
    }

    return (
      <div className="p-4">
        <div className="bg-white rounded-lg border border-gray-200 p-4 mb-4">
          <div className="grid grid-cols-4 gap-4">
            <div>
              <div className="text-sm text-gray-600">Username</div>
              <div className="font-medium">{detailData.username}</div>
            </div>
            <div>
              <div className="text-sm text-gray-600">Login Name</div>
              <div className="font-medium">{detailData.username}</div>
            </div>
            <div>
              <div className="text-sm text-gray-600">First/Last Name</div>
              <div className="font-medium">{`${detailData.firstName} ${detailData.lastName}`}</div>
            </div>
            <div>
              <div className="text-sm text-gray-600">Bet Credit</div>
              <div className="font-medium">0.00</div>
            </div>
          </div>
        </div>

        <div className="grid grid-cols-2 gap-6">
          <div>
            <div className="mb-4">
              <div className="text-sm text-gray-600">From</div>
              <div className="flex items-center gap-2">
                <div className="w-4 h-4 rounded-full bg-green-500"></div>
                <span className="font-medium">{detailData.from}</span>
              </div>
            </div>
            <div className="mb-4">
              <div className="text-sm text-gray-600">Date and Time</div>
              <div className="font-medium">{formatDateTime(detailData.dateTime)}</div>
            </div>
            <div className="mb-4">
              <div className="text-sm text-gray-600">Deposit Type</div>
              <div className="font-medium">{detailData.transactionType}</div>
            </div>
            <div className="mb-4">
              <div className="text-sm text-gray-600">Operated By</div>
              <div className="font-medium">{detailData.username?.substring(0, 9)}</div>
            </div>
          </div>
          <div>
            <div className="mb-4">
              <div className="text-sm text-gray-600">To</div>
              <div className="flex items-center gap-2">
                <div className="w-4 h-4 rounded-full bg-green-500"></div>
                <span className="font-medium">{detailData.to}</span>
              </div>
            </div>
            <div className="mb-4">
              <div className="text-sm text-gray-600">Processed On</div>
              <div className="font-medium">{formatDateTime(detailData.dateTime)}</div>
            </div>
            <div className="mb-4">
              <div className="text-sm text-gray-600">Deposit Amount</div>
              <div className="font-medium">{`${detailData.amount.toFixed(2)} ${detailData.currency}`}</div>
            </div>
          </div>
        </div>

        <div className="mt-4">
          <div className="text-sm text-gray-600 mb-2">Remarks</div>
          <textarea
            className="w-full h-24 p-2 border border-gray-300 rounded-md"
            placeholder="Add a remark here..."
            readOnly
          />
        </div>
      </div>
    );
  };

  return (
    <Modal
      title={
        <div className="flex items-center gap-2">
          <span>Deposit History ({detailData?.referenceNumber ?? 'Loading...'})</span>
          {detailData && (
            <span className={`status-badge ${detailData.status.toLowerCase()}`}>
              {detailData.status}
            </span>
          )}
        </div>
      }
      open={isOpen}
      onCancel={onClose}
      footer={[
        <button
          key="close"
          onClick={onClose}
          className="px-4 py-2 text-gray-700 bg-gray-100 rounded-md hover:bg-gray-200 focus:outline-none focus:ring-2 focus:ring-gray-400 transition duration-150 ease-in-out"
        >
          Close
        </button>,
        <button
          key="revert"
          onClick={handleRevert}
          className={`px-4 py-2 ml-2 text-white bg-blue-600 rounded-md hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:ring-opacity-50 transition duration-150 ease-in-out ${reverting ? 'opacity-50 cursor-not-allowed' : ''}`}
          disabled={reverting || !detailData}
        >
          {reverting ? <Spin size="small" className="mr-2" /> : null}
          Revert To Pending
        </button>,
      ]}
      width={600}
      destroyOnClose
    >
      {renderContent()}
    </Modal>
  );
};

export default TransactionDetail;
