import React, { useState } from 'react';
import TransactionDetail from './TransactionDetail';
import { TransactionItem } from '../../../../services/interfaces/TransactionHistory';
import { NoRecordsIcon } from '../../../../components/Icons';

interface TableProps {
  items: TransactionItem[];
  isLoading: boolean;
  onRefreshNeeded?: () => void;
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
  hours = hours ? hours : 12; // the hour '0' should be '12'

  return `${month}/${day}/${year} ${hours}:${minutes}:${seconds} ${ampm}`;
};

const buildHeader = () => (
  <thead>
    <tr>
      <th
        scope="col"
        className="px-1 py-0.5 text-center text-sm font-semibold text-gray-800 border border-gray-300"
      >
        #
      </th>
      <th
        scope="col"
        className="px-1 py-0.5 text-center text-sm font-semibold text-gray-800 border border-gray-300"
      >
        Username
      </th>
      <th
        scope="col"
        className="px-1 py-0.5 text-center text-sm font-semibold text-gray-800 border border-gray-300"
      >
        <div>First Name</div>
        <div>Last Name</div>
      </th>
      <th
        scope="col"
        className="px-1 py-0.5 text-center text-sm font-semibold text-gray-800 border border-gray-300"
      >
        Amount
      </th>
      <th
        scope="col"
        className="px-1 py-0.5 text-center text-sm font-semibold text-gray-800 border border-gray-300"
      >
        Currency
      </th>
      <th
        scope="col"
        className="px-1 py-0.5 text-center text-sm font-semibold text-gray-800 border border-gray-300"
      >
        From
      </th>
      <th
        scope="col"
        className="px-1 py-0.5 text-center text-sm font-semibold text-gray-800 border border-gray-300"
      >
        To
      </th>
      <th
        scope="col"
        className="px-1 py-0.5 text-center text-sm font-semibold text-gray-800 border border-gray-300"
      >
        Status
      </th>
      <th
        scope="col"
        className="px-1 py-0.5 text-center text-sm font-semibold text-gray-800 border border-gray-300"
      >
        Transaction type
      </th>
      <th
        scope="col"
        className="px-1 py-0.5 text-center text-sm font-semibold text-gray-800 border border-gray-300"
      >
        Reference No.
      </th>
      <th
        scope="col"
        className="px-1 py-0.5 text-center text-sm font-semibold text-gray-800 border border-gray-300"
      >
        Date and Time
      </th>
      <th
        scope="col"
        className="px-1 py-0.5 text-center text-sm font-semibold text-gray-800 border border-gray-300"
      >
        Action
      </th>
    </tr>
  </thead>
);

const Table: React.FC<TableProps> = ({ items, isLoading, onRefreshNeeded }) => {
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

  const buildRow = (item: TransactionItem, index: number) => (
    <tr key={item.transactionId}>
      <td className="px-1 py-0.5 text-sm text-gray-600 border border-gray-300 text-center">
        {index + 1}
      </td>
      <td className="px-1 py-0.5 text-sm text-gray-600 border border-gray-300 text-center">
        {item.username}
      </td>
      <td className="px-1 py-0.5 text-sm text-gray-600 border border-gray-300 text-left">
        <div>{item.firstName}</div>
        <div>{item.lastName}</div>
      </td>
      <td className="px-1 py-0.5 text-sm text-gray-600 border border-gray-300 text-center">
        {item.amount.toFixed(2)}
      </td>
      <td className="px-1 py-0.5 text-sm text-gray-600 border border-gray-300 text-center">
        {item.currency}
      </td>
      <td className="px-1 py-0.5 text-sm text-gray-600 border border-gray-300 text-center">
        {item.from}
      </td>
      <td className="px-1 py-0.5 text-sm text-gray-600 border border-gray-300 text-center">
        {item.to}
      </td>
      <td className="px-1 py-0.5 text-sm border border-gray-300 text-center">
        <span className={`status-badge ${item.status.toLowerCase()}`}>{item.status}</span>
      </td>
      <td className="px-1 py-0.5 text-sm text-gray-600 border border-gray-300 text-center">
        {item.transactionType}
      </td>
      <td className="px-1 py-0.5 text-sm text-gray-600 border border-gray-300 text-center">
        {item.referenceNumber}
      </td>
      <td className="px-1 py-0.5 text-sm text-gray-600 border border-gray-300 text-center">
        {formatDateTime(item.dateTime)}
      </td>
      <td className="px-1 py-0.5 text-sm text-gray-600 border border-gray-300 text-center">
        <button className="action-button" onClick={() => handleViewTransaction(item.transactionId)}>
          View
        </button>
      </td>
    </tr>
  );

  const buildEmptyState = () => (
    <tr>
      <td colSpan={12} className="border border-gray-300">
        <div className="flex flex-col items-center justify-center py-16">
          <NoRecordsIcon className="mb-4 text-gray-400" />
          <p className="text-gray-500 text-sm">No records found</p>
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
      <div className="transaction-history overflow-x-auto">
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
