import React, { useState, useEffect } from 'react';
import { useTranslation } from 'react-i18next';
import { TransactionItem } from '../../../../services/interfaces/Transactions/TransactionHistory.ts';
import {
  CurrentCurrency,
  displayAmount,
  formatDateTime,
  renderBankAccount,
} from '../../../../utils/commonFunctions.tsx';
import PendingWithdrawalModal from './PendingWithdrawalModal.tsx';
import { transactionAPI } from '../../../../services/transactionsHistoryAPI';
import NoRecordFound from '../../../../components/Table/NoRecordFound';
import { ButtonFullComponent } from '../../../../components/ButtonComponents';
import Pagination from '../../Components/Pagination';
import { PAGE_SIZE_OPTIONS } from '../../../../constants/paymentConstants';
import { ValidateMessageComponent } from '../../../../components/MessageComponents.tsx';

interface DataGridProps {
  withdrawals: TransactionItem[];
}

const DataGrid: React.FC<DataGridProps> = ({ withdrawals }) => {
  const { t } = useTranslation('pendingWithdrawal');
  const [processingWithdrawal, setProcessingWithdrawal] = useState<TransactionItem | null>(null);
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [currentPage, setCurrentPage] = useState(1);
  const [pageSize, setPageSize] = useState(PAGE_SIZE_OPTIONS[2]);
  const [paginatedWithdrawals, setPaginatedWithdrawals] = useState<TransactionItem[]>([]);
  const totalItems = withdrawals.length;

  useEffect(() => {
    const startIndex = (currentPage - 1) * pageSize;
    const endIndex = startIndex + pageSize;
    setPaginatedWithdrawals(withdrawals.slice(startIndex, endIndex));
  }, [withdrawals, currentPage, pageSize]);

  const handlePageChange = (page: number) => {
    setCurrentPage(page);
  };

  const handlePageSizeChange = (newPageSize: number) => {
    setPageSize(newPageSize);
    setCurrentPage(1);
  };

  const handleProcessClick = async (withdrawal: TransactionItem) => {
    setIsLoading(true);
    setError(null);
    try {
      const detail = await transactionAPI.getTransactionDetail(
        withdrawal.transId,
        withdrawal.custId
      );
      setProcessingWithdrawal(detail);
    } catch (err) {
      console.error('Error fetching transaction detail:', err);
      setError(t('dataGrid.errorFetchingDetail'));
    } finally {
      setIsLoading(false);
    }
  };

  const handleCloseProcess = () => {
    setProcessingWithdrawal(null);
    setError(null);
  };

  return (
    <>
      <div className="table-fixed-header table-fixed-body min-h-0">
        <table className="max-[600px]:text-[17px] w-full whitespace-nowrap border-collapse border border-[#0000001f] border-t-[0px] border-l-[0px] text-[#333333]">
          <thead className="z-11!">
            <tr>
              <th scope="col" className="w-[32px] min-w-[32px] sticky left-0 bg-[var(--table-header-background)] z-11">
                #
              </th>
              <th
                scope="col"
                className="w-[150px] min-w-[150px] sticky left-[32px] bg-[var(--table-header-background)] shadow-[3px_0_4px_0_rgba(99,84,84,0.3)]"
              >
                <div>{t('dataGrid.username')}</div>
                <div>{t('dataGrid.nickname')}</div>
              </th>
              <th scope="col">
                <div>{t('dataGrid.firstName')}</div>
              </th>
              <th scope="col">
                {t('dataGrid.amount')} ({CurrentCurrency})
              </th>
              <th scope="col">{t('dataGrid.from')}</th>
              <th scope="col">{t('dataGrid.to')}</th>
              <th scope="col">{t('dataGrid.referenceNo')}</th>
              <th scope="col">{t('dataGrid.dateAndTime')}</th>
              <th scope="col">{t('dataGrid.actions')}</th>
            </tr>
          </thead>
          <tbody className="font-[500]">
            {paginatedWithdrawals.length > 0 ? (
              paginatedWithdrawals.map((withdrawal, index) => (
                <tr key={withdrawal.transId}>
                  <td className="w-[32px] min-w-[32px] sticky left-0 bg-[var(--table-background)] z-10">
                    {index + 1 + (currentPage - 1) * pageSize}
                  </td>
                  <td className="w-[150px] min-w-[150px] sticky left-[32px] bg-[var(--table-background)] z-10 shadow-[3px_0_4px_0_rgba(99,84,84,0.3)]">
                    <div>{withdrawal.username}</div>
                    <div>{withdrawal.nickname}</div>
                  </td>
                  <td>
                    <div>{withdrawal.firstName}</div>
                    <div>{withdrawal.lastName}</div>
                  </td>
                  <td>{displayAmount(withdrawal.amount)}</td>
                  <td>{renderBankAccount(withdrawal.paymentFrom)}</td>
                  <td>{renderBankAccount(withdrawal.paymentTo)}</td>
                  <td>{withdrawal.transId}</td>
                  <td>{formatDateTime(withdrawal.createdTime)}</td>
                  <td>
                    <ButtonFullComponent
                      text={t('dataGrid.process')}
                      disabled={isLoading}
                      onClick={() => handleProcessClick(withdrawal)}
                    />
                  </td>
                </tr>
              ))
            ) : (
              <NoRecordFound colSpan={9} message={t('dataGrid.noWithdrawalsFound')} />
            )}
          </tbody>
        </table>        
      </div>

      {error && <div className="mt-4 p-4 bg-red-100 text-red-700 rounded-md">{error}</div>}
      {
        error && (
          <ValidateMessageComponent message={error} />
        )
      }

      {processingWithdrawal && (
        <PendingWithdrawalModal withdrawal={processingWithdrawal} onClose={handleCloseProcess} />
      )}
      {paginatedWithdrawals.length > 0 && (    
        <Pagination
          currentPage={currentPage}
          pageSize={pageSize}
          totalItems={totalItems}
          onPageChange={handlePageChange}
          onPageSizeChange={handlePageSizeChange}
        />      
      )}      
    </>
  );
};

export default DataGrid;
