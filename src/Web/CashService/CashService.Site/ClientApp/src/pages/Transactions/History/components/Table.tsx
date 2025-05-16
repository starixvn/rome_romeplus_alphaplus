import React, { useState } from 'react';
import TransactionDetail from './TransactionDetail';
import { TransactionItem } from '../../../../services/interfaces/Transactions/TransactionHistory';
import { NoRecordsIcon } from '../../../../components/Icons';
import { useTranslation } from 'react-i18next';

interface TableProps {
  items: TransactionItem[];
  isLoading: boolean;
  onRefreshNeeded?: () => void;
  columnFilter?: string[];
}

const formatDateTime = (dateTimeStr: string): { date: string; time: string } => {
  const dateObj = new Date(dateTimeStr);
  const month = (dateObj.getMonth() + 1).toString().padStart(2, '0');
  const day = dateObj.getDate().toString().padStart(2, '0');
  const year = dateObj.getFullYear();

  let hours = dateObj.getHours();
  const minutes = dateObj.getMinutes().toString().padStart(2, '0');
  const seconds = dateObj.getSeconds().toString().padStart(2, '0');
  const ampm = hours >= 12 ? 'PM' : 'AM';

  hours = hours % 12;
  hours = hours ? hours : 12;
  const hoursStr = hours.toString().padStart(2, '0');

  return {
    date: `${month}/${day}/${year}`,
    time: `${hoursStr}:${minutes}:${seconds} ${ampm}`,
  };
};

const Table: React.FC<TableProps> = ({
  items,
  isLoading,
  onRefreshNeeded,
  columnFilter = ['All'],
}) => {
  const { t } = useTranslation('transactionHistory');
  const [selectedTransactionId, setSelectedTransactionId] = useState<number | null>(null);
  const [isDetailModalOpen, setIsDetailModalOpen] = useState(false);

  const handleViewTransaction = (transactionId: number) => {
    setSelectedTransactionId(transactionId);
    setIsDetailModalOpen(true);
  };

  const handleCloseModal = () => {
    setIsDetailModalOpen(false);
    setSelectedTransactionId(null);
  };

  const handleRevertSuccess = () => {
    handleCloseModal();
    onRefreshNeeded?.();
  };

  const getStatusById = (id: number) => {
    switch (id) {
      case 0:
        return 'Waiting';
      case 1:
        return 'Cancelled';
      case 2:
        return 'Pending';
      case 3:
        return 'EXPIRED';
      case 4:
        return 'Completed';
      case 5:
        return 'Rejected';
      case 6:
        return 'Provider Rejected';
      default:
        return 'All';
    }
  };

  const getStatusStyle = (statusId: number) => {
    const status = getStatusById(statusId);

    switch (status.toUpperCase()) {
      case 'COMPLETED':
        return 'bg-[#DCFCE7] text-[#166534]';
      case 'EXPIRED':
        return 'bg-[#F3F4F6] text-[#374151]';
      case 'REJECTED':
      case 'PROVIDER_REJECTED':
        return 'bg-[#FEE2E2] text-[#991B1B]';
      default:
        return 'bg-gray-100 text-gray-800';
    }
  };

  const shouldShowColumn = (columnName: string) => {
    return columnFilter.includes(columnName);
  };

  const buildHeader = () => (
    <thead>
      <tr>
        <th
          scope="col"
          className="px-4 py-2 text-left text-sm font-medium text-gray-700 border border-gray-300 bg-gray-50 text-center"
        >
          {t('table.no')}
        </th>
        <th
          scope="col"
          className="px-4 py-2 text-left text-sm font-medium text-gray-700 border border-gray-300 bg-gray-50"
        >
          {t('table.username')}
        </th>
        {shouldShowColumn(t('table.name')) && (
          <th
            scope="col"
            className="px-4 py-2 text-left text-sm font-medium text-gray-700 border border-gray-300 bg-gray-50"
          >
            {t('table.name')}
          </th>
        )}
        {shouldShowColumn(t('table.amount')) && (
          <th
            scope="col"
            className="px-4 py-2 text-left text-sm font-medium text-gray-700 border border-gray-300 bg-gray-50"
          >
            {t('table.amount')}
          </th>
        )}
        {shouldShowColumn(t('table.currency')) && (
          <th
            scope="col"
            className="px-4 py-2 text-left text-sm font-medium text-gray-700 border border-gray-300 bg-gray-50"
          >
            {t('table.currency')}
          </th>
        )}
        {shouldShowColumn(t('table.from')) && (
          <th
            scope="col"
            className="px-4 py-2 text-left text-sm font-medium text-gray-700 border border-gray-300 bg-gray-50"
          >
            {t('table.from')}
          </th>
        )}
        {shouldShowColumn(t('table.to')) && (
          <th
            scope="col"
            className="px-4 py-2 text-left text-sm font-medium text-gray-700 border border-gray-300 bg-gray-50"
          >
            {t('table.to')}
          </th>
        )}
        {shouldShowColumn(t('table.status')) && (
          <th
            scope="col"
            className="px-4 py-2 text-left text-sm font-medium text-gray-700 border border-gray-300 bg-gray-50"
          >
            {t('table.status')}
          </th>
        )}
        {shouldShowColumn(t('table.type')) && (
          <th
            scope="col"
            className="px-4 py-2 text-left text-sm font-medium text-gray-700 border border-gray-300 bg-gray-50"
          >
            {t('table.type')}
          </th>
        )}
        {shouldShowColumn(t('table.referenceNo')) && (
          <th
            scope="col"
            className="px-4 py-2 text-left text-sm font-medium text-gray-700 border border-gray-300 bg-gray-50"
          >
            {t('table.referenceNo')}
          </th>
        )}
        {shouldShowColumn(t('table.dateAndTime')) && (
          <th
            scope="col"
            className="px-4 py-2 text-left text-sm font-medium text-gray-700 border border-gray-300 bg-gray-50"
          >
            {t('table.dateAndTime')}
          </th>
        )}
        <th
          scope="col"
          className="px-4 py-2 text-left text-sm font-medium text-gray-700 border border-gray-300 bg-gray-50"
        >
          {t('table.actions')}
        </th>
      </tr>
    </thead>
  );

  const buildRow = (item: TransactionItem, index: number) => (
    <tr key={item.transactionId} className={index % 2 === 0 ? 'bg-white' : 'bg-gray-50'}>
      <td className="px-4 py-0.5 text-sm text-gray-700 border border-gray-300 text-center">
        {index + 1}
      </td>
      <td className="px-4 py-0.5 text-sm text-gray-700 border border-gray-300">{item.username}</td>
      {shouldShowColumn(t('table.name')) && (
        <td className="px-4 py-0.5 text-sm text-gray-700 border border-gray-300">
          <div>{item.firstName}</div>
          <div>{item.lastName}</div>
        </td>
      )}
      {shouldShowColumn(t('table.amount')) && (
        <td className="px-4 py-0.5 text-sm text-gray-700 border border-gray-300 text-right">
          {item.amount.toFixed(2)}
        </td>
      )}
      {shouldShowColumn(t('table.currency')) && (
        <td className="px-4 py-0.5 text-sm text-gray-700 border border-gray-300 text-center">
          {item.currency}
        </td>
      )}
      {shouldShowColumn(t('table.from')) && (
        <td className="px-4 py-0.5 text-sm text-gray-700 border border-gray-300">
          <div className="flex items-center">
            {item.paymentFrom ? (
              <>
                <img src="/icons/wallet.svg" alt="wallet" className="w-4 h-4 mr-2" />
                {item.paymentFrom}
              </>
            ) : (
              '-'
            )}
          </div>
        </td>
      )}
      {shouldShowColumn(t('table.to')) && (
        <td className="px-4 py-0.5 text-sm text-gray-700 border border-gray-300">
          <div className="flex items-center">
            {item.paymentTo === 'U2Pay' ? (
              <>
                <img src="/icons/u2pay.svg" alt="u2pay" className="w-4 h-4 mr-2" />
                {item.paymentTo}
              </>
            ) : (
              <>
                <img src="/icons/wallet.svg" alt="wallet" className="w-4 h-4 mr-2" />
                {item.paymentTo}
              </>
            )}
          </div>
        </td>
      )}
      {shouldShowColumn(t('table.status')) && (
        <td className="px-4 py-0.5 text-sm border border-gray-300 text-center">
          <span
            className={`px-2 py-1 rounded-full text-xs font-medium ${getStatusStyle(item.statusId)}`}
          >
            {getStatusById(item.statusId).toUpperCase()}
          </span>
        </td>
      )}
      {shouldShowColumn(t('table.type')) && (
        <td className="px-4 py-0.5 text-sm text-gray-700 border border-gray-300 text-center">
          {item.transactionType}
        </td>
      )}
      {shouldShowColumn(t('table.referenceNo')) && (
        <td className="px-4 py-0.5 text-sm text-gray-700 border border-gray-300 text-center">
          {item.transferRefNo}
        </td>
      )}
      {shouldShowColumn(t('table.dateAndTime')) && (
        <td className="px-2 py-0.5 text-sm text-gray-700 border border-gray-300 text-center">
          <div>{formatDateTime(item.createdTime).date}</div>
          <div className="text-xs text-gray-500">{formatDateTime(item.createdTime).time}</div>
        </td>
      )}
      <td className="px-4 py-0.5 text-sm text-gray-700 border border-gray-300 text-center">
        <button
          onClick={() => handleViewTransaction(item.transactionId)}
          className="bg-blue-800 hover:bg-blue-900 text-white text-xs font-semibold py-1 px-3 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-700 focus:ring-opacity-50 cursor-pointer"
        >
          {t('table.view')}
        </button>
      </td>
    </tr>
  );

  const buildEmptyState = () => (
    <tr>
      <td colSpan={12} className="border border-gray-300">
        <div className="flex flex-col items-center justify-center py-16">
          <NoRecordsIcon className="mb-4 text-gray-400" />
          <p className="text-gray-500 text-sm">{t('table.noRecords')}</p>
        </div>
      </td>
    </tr>
  );

  const buildBody = () => (
    <tbody>{items && items.length > 0 ? items.map(buildRow) : buildEmptyState()}</tbody>
  );

  if (isLoading) {
    return (
      <div className="flex justify-center items-center h-64">
        <div className="loading-spinner"></div>
      </div>
    );
  }

  return (
    <>
      <div className="overflow-x-auto">
        <table className="min-w-full border-collapse border border-gray-300">
          {buildHeader()}
          {buildBody()}
        </table>
      </div>

      <TransactionDetail
        transactionId={selectedTransactionId}
        isOpen={isDetailModalOpen}
        onClose={handleCloseModal}
        onRevertSuccess={handleRevertSuccess}
      />
    </>
  );
};

export default Table;
