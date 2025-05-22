import React, { useState } from 'react';
import TransactionDetail from './TransactionDetail';
import { TransactionItem } from '../../../../services/interfaces/Transactions/TransactionHistory';
import { NoRecordsIcon } from '../../../../components/Icons';
import { useTranslation } from 'react-i18next';
import {
  displayAmount,
  getStatusStyle,
  mappingStatus,
  renderBankAccount,
  CurrentCurrency,
  formatDateTime,
} from '../../../../common/commonFunctions';
import NoRecordFound from '../../../../components/Table/NoRecordFound';

interface DataGridProps {
  items: TransactionItem[];
  pageIndex: number;
  pageSize: number;
  isLoading: boolean;
  onRefreshNeeded?: () => void;
  columnFilter?: string[];
}

const DataGrid: React.FC<DataGridProps> = ({
  items,
  pageIndex,
  pageSize,
  isLoading,
  onRefreshNeeded,
  columnFilter = ['All'],
}) => {
  const { t } = useTranslation('transactionHistory');
  const [selectedTransactionId, setSelectedTransactionId] = useState<number | null>(null);
  const [selectedCustId, setSelectedCustId] = useState<number | null>(null);
  const [isDetailModalOpen, setIsDetailModalOpen] = useState(false);

  const handleViewTransaction = (transactionId: number, custId: number) => {
    setSelectedTransactionId(transactionId);
    setSelectedCustId(custId);
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

  const shouldShowColumn = (columnName: string) => {
    return columnFilter.includes(columnName);
  };

  if (isLoading) {
    return (
      <div className="flex justify-center items-center h-64">
        <div className="loading-spinner"></div>
      </div>
    );
  }

  return (
    <>
      <table className="max-[600px]:text-[17px] w-full whitespace-nowrap border-collapse border border-[#0000001f] border-t-[0px]  border-l-[0px] text-[#333333] shadow-[0_1px_5px_#0003,0_2px_2px_#00000024,0_3px_1px_-2px_#0000001f]">
        <thead className="z-11!">
          <tr>
            <th scope="col" className="w-[32px] min-w-[32px] sticky left-0 bg-[#ffffff] z-11">
              {t('table.no')}
            </th>
            <th
              scope="col"
              className="w-[150px] min-w-[150px] sticky left-[32px] bg-[#ffffff] z-11 shadow-[3px_0_4px_0_rgba(99,84,84,0.3)]"
            >
              <div>{t('table.username')}</div>
              <div>{t('table.nickname')}</div>
            </th>
            {shouldShowColumn(t('table.name')) && <th scope="col">{t('table.name')}</th>}
            {shouldShowColumn(t('table.amount')) && (
              <th scope="col">
                {t('table.amount')} ({CurrentCurrency})
              </th>
            )}
            {shouldShowColumn(t('table.from')) && <th scope="col">{t('table.from')}</th>}
            {shouldShowColumn(t('table.to')) && <th scope="col">{t('table.to')}</th>}
            {shouldShowColumn(t('table.status')) && <th scope="col">{t('table.status')}</th>}
            {shouldShowColumn(t('table.type')) && <th scope="col">{t('table.type')}</th>}
            {shouldShowColumn(t('table.referenceNo')) && (
              <th scope="col">{t('table.referenceNo')}</th>
            )}
            {shouldShowColumn(t('table.dateAndTime')) && (
              <th scope="col">{t('table.dateAndTime')}</th>
            )}
            <th scope="col">{t('table.actions')}</th>
          </tr>
        </thead>
        <tbody className="font-[500]">
          {items && items.length > 0 ? (
            items.map((item, index) => (
              <tr key={item.transId}>
                <td className="w-[32px] min-w-[32px] sticky left-0 bg-[#ffffff] z-10">
                  {index + 1 + (pageIndex - 1) * pageSize}
                </td>
                <td className="w-[150px] min-w-[150px] sticky left-[32px] bg-[#ffffff] z-10 shadow-[3px_0_4px_0_rgba(99,84,84,0.3)]">
                  <div>{item.username}</div>
                  <div>{item.nickname}</div>
                </td>
                {shouldShowColumn(t('table.name')) && (
                  <td>
                    <div>{item.firstName}</div>
                    <div>{item.lastName}</div>
                  </td>
                )}
                {shouldShowColumn(t('table.amount')) && <td>{displayAmount(item.amount)}</td>}
                {shouldShowColumn(t('table.from')) && (
                  <td>
                    <div className="flex items-center">
                      {item.paymentFrom ? <>{renderBankAccount(item.paymentFrom)}</> : '-'}
                    </div>
                  </td>
                )}
                {shouldShowColumn(t('table.to')) && (
                  <td>
                    <div className="flex items-center">{renderBankAccount(item.paymentTo)}</div>
                  </td>
                )}
                {shouldShowColumn(t('table.status')) && (
                  <td className="text-center">
                    <span
                      className={`px-2 py-1 rounded-full text-xs font-medium ${getStatusStyle(item.statusId)}`}
                    >
                      {mappingStatus(item.statusId).toUpperCase()}
                    </span>
                  </td>
                )}
                {shouldShowColumn(t('table.type')) && (
                  <td className="text-center">{item.transactionType}</td>
                )}
                {shouldShowColumn(t('table.referenceNo')) && (
                  <td className="text-center">{item.transId}</td>
                )}
                {shouldShowColumn(t('table.dateAndTime')) && (
                  <td className="text-center">
                    <div>{formatDateTime(item.createdTime)}</div>
                  </td>
                )}
                <td className="text-center">
                  <button
                    onClick={() => handleViewTransaction(item.transId, item.custId)}
                    className="bg-[#163f78] hover:bg-[var(--button-background-hover)] text-white text-xs font-semibold py-1 px-3 rounded-[20px] focus:outline-none focus:ring-2 focus:ring-[#163f78] focus:ring-opacity-50 cursor-pointer"
                  >
                    {t('table.view')}
                  </button>
                </td>
              </tr>
            ))
          ) : (
            <NoRecordFound colSpan={12} message={t('table.noRecords')} />
          )}
        </tbody>
      </table>

      {selectedTransactionId && selectedCustId && (
        <TransactionDetail
          transactionId={selectedTransactionId}
          custId={selectedCustId}
          isOpen={isDetailModalOpen}
          onClose={handleCloseModal}
          onRevertSuccess={handleRevertSuccess}
        />
      )}
    </>
  );
};

export default DataGrid;
