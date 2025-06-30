import React, { useState } from 'react';
import { useTranslation } from 'react-i18next';

import LoadingComponent from '../../../../components/LoadingComponent';
import NoRecordFound from '../../../../components/NoRecordFoundComponent.tsx';
import { TransactionItem } from '../../../../services/interfaces/Transactions/TransactionHistory';
import {
  displayAmount,
  formatDateTime,
  mappingStatus,
  renderBankAccount,
} from '../../../../utils/commonFunctions';
import TransactionDetail from './TransactionDetail';
import { ButtonFullComponent } from '../../../../components/Buttons/index';
import { IconImage } from '../../../../components/Icons.tsx';

interface DataGridProps {
  items: TransactionItem[];
  pageIndex: number;
  pageSize: number;
  isLoading: boolean;
  transactionType: number;
  onRefreshNeeded?: () => void;
  columnFilter?: string[];
  isCashServiceEnabled: boolean;
}

const DataGrid: React.FC<DataGridProps> = ({
  items,
  pageIndex,
  pageSize,
  isLoading,
  transactionType,
  onRefreshNeeded,
  columnFilter = ['All'],
  isCashServiceEnabled,
}) => {
  const { t } = useTranslation('transactionHistory');
  const { t: tGlobal } = useTranslation('global');
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
    onRefreshNeeded?.();
    handleCloseModal();
  };

  const shouldShowColumn = (columnName: string) => {
    return columnFilter.includes(columnName);
  };

  if (isLoading) {
    return (
      <div className="flex justify-center items-center h-64">
        <LoadingComponent />
      </div>
    );
  }

  return (
    <>
      <div className="table-fixed-header table-fixed-body min-h-0">
        <table className="w-full whitespace-nowrap border-collapse border border-[#0000001f] border-t-[0px]  border-l-[0px]">
          <thead className="z-11!">
            <tr>
              <th
                scope="col"
                className="w-[32px] min-w-[32px] bg-[var(--table-header-background)] sticky left-0 z-11"
              >
                {t('table.no')}
              </th>
              <th
                scope="col"
                className="w-[150px] min-w-[150px] sticky left-[32px] bg-[var(--table-header-background)] z-11 shadow-[3px_0_4px_0_rgba(99,84,84,0.3)]"
              >
                <div>{t('table.username')}</div>
                <div>{t('table.nickname')}</div>
              </th>
              {shouldShowColumn('name') && <th scope="col">{t('table.name')}</th>}
              {shouldShowColumn('amount') && (
                <th scope="col" className="min-w-[150px]">
                  {t('table.amount')}
                </th>
              )}
              {shouldShowColumn('from') && <th scope="col">{t('table.from')}</th>}
              {shouldShowColumn('to') && <th scope="col">{t('table.to')}</th>}
              {shouldShowColumn('status') && <th scope="col">{t('table.status')}</th>}
              {shouldShowColumn('type') && <th scope="col">{t('table.type')}</th>}
              {shouldShowColumn('referenceNo') && <th scope="col">{t('table.referenceNo')}</th>}
              {shouldShowColumn('dateAndTime') && <th scope="col">{t('table.dateAndTime')}</th>}
              <th scope="col">{t('table.actions')}</th>
            </tr>
          </thead>
          <tbody className="font-[500]">
            {items && items.length > 0 ? (
              items.map((item, index) => (
                <tr key={item.transId}>
                  <td className="w-[32px] min-w-[32px] sticky left-0 bg-[var(--table-background)] z-10">
                    {index + 1 + (pageIndex - 1) * pageSize}
                  </td>
                  <td className="w-[150px] min-w-[150px] sticky left-[32px] bg-[var(--table-background)] z-10 shadow-[3px_0_4px_0_rgba(99,84,84,0.3)]">
                    <div>{item.username}</div>
                    <div>{item.nickname}</div>
                  </td>
                  {shouldShowColumn('name') && (
                    <td>
                      <div>{item.firstName}</div>
                      <div>{item.lastName}</div>
                    </td>
                  )}
                  {shouldShowColumn('amount') && (
                    <td className="text-right">
                      <div className="flex items-center justify-end pr-[25px] relative">
                        {displayAmount(item.amount)}
                        {item.isHadPayslip && (
                          <IconImage className="w-[20px] h-[20px] ml-[5px] absolute right-[0px]" />
                        )}
                      </div>
                    </td>
                  )}
                  {shouldShowColumn('from') && (
                    <td>
                      <div className="flex items-center">
                        {renderBankAccount(item.paymentFrom, tGlobal)}
                      </div>
                    </td>
                  )}
                  {shouldShowColumn('to') && (
                    <td>
                      <div className="flex items-center">
                        {renderBankAccount(item.paymentTo, tGlobal)}
                      </div>
                    </td>
                  )}
                  {shouldShowColumn('status') && (
                    <td className="text-center">{mappingStatus(item.statusId, item.status)}</td>
                  )}
                  {shouldShowColumn('type') && (
                    <td className="text-center">{item.transactionType}</td>
                  )}
                  {shouldShowColumn('referenceNo') && (
                    <td className="text-center">{item.transId}</td>
                  )}
                  {shouldShowColumn('dateAndTime') && (
                    <td className="text-center">
                      <div>{formatDateTime(item.createdTime)}</div>
                    </td>
                  )}
                  <td className="text-center">
                    <ButtonFullComponent
                      text={t('table.view')}
                      onClick={() => handleViewTransaction(item.transId, item.custId)}
                    />
                  </td>
                </tr>
              ))
            ) : (
              <NoRecordFound colSpan={12} message={t('table.noRecords')} />
            )}
          </tbody>
        </table>
      </div>

      {selectedTransactionId && selectedCustId && (
        <TransactionDetail
          transactionId={selectedTransactionId}
          custId={selectedCustId}
          isOpen={isDetailModalOpen}
          transactionType={transactionType}
          onClose={handleCloseModal}
          onRevertSuccess={handleRevertSuccess}
          isCashServiceEnabled={isCashServiceEnabled}
        />
      )}
    </>
  );
};

export default DataGrid;
