import React, { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { TransactionItem } from '../../../../services/interfaces/Transactions/TransactionHistory';
import {
  displayAmount,
  renderBankAccount,
  formatDateTime,
  CurrentCurrency,
} from '../../../../common/commonFunctions';
import NoRecordFound from '../../../../components/Table/NoRecordFound';
import Pagination from '../../Components/Pagination';
import { ButtonFullComponent } from '../../../../components/ButtonComponents';
import { transactionAPI } from '../../../../services/transactionsHistoryAPI';
import PendingDepositModal from './PendingDepositModal';

interface DataGridProps {
  deposits: TransactionItem[];
}

const DataGrid: React.FC<DataGridProps> = ({ deposits }) => {
  const { t } = useTranslation('pendingDeposit');
  const [currentPage, setCurrentPage] = useState(1);
  const [pageSize, setPageSize] = useState(100);
  const [error, setError] = useState<string | null>(null);
  const [paginatedDeposits, setPaginatedDeposits] = useState<TransactionItem[]>([]);
  const [processingDeposit, setProcessingDeposit] = useState<TransactionItem | null>(null);
  const totalItems = deposits.length;
  const [isLoading, setIsLoading] = useState(false);

  useEffect(() => {
    const startIndex = (currentPage - 1) * pageSize;
    const endIndex = startIndex + pageSize;
    setPaginatedDeposits(deposits.slice(startIndex, endIndex));
  }, [deposits, currentPage, pageSize]);

  const handlePageChange = (page: number) => {
    setCurrentPage(page);
  };

  const handlePageSizeChange = (newPageSize: number) => {
    setPageSize(newPageSize);
    setCurrentPage(1);
  };

  const handleProcessClick = async (deposit: TransactionItem) => {
    setIsLoading(true);
    setError(null);
    try {
      const detail = await transactionAPI.getTransactionDetail(
        deposit.transId,
        deposit.custId
      );
      setProcessingDeposit(detail);
    } catch (err) {
      setError(t('dataGrid.errorFetchingDetail'));
    } finally {
      setIsLoading(false);
    }
  };

  const handleCloseProcess = () => {
    setProcessingDeposit(null);
    setError(null);
  };

  return (
    <>
      <div className="table-fixed-header table-fixed-body bg-[#ffffff] h-full">
        <table className="max-[600px]:text-[17px] w-full whitespace-nowrap border-collapse border border-[#0000001f] border-t-[0px]  border-l-[0px] text-[#333333] shadow-[0_1px_5px_#0003,0_2px_2px_#00000024,0_3px_1px_-2px_#0000001f]">
          <thead className="z-11!">
            <tr>
              <th scope="col" className="w-[32px] min-w-[32px] sticky left-0 bg-[#ffffff] z-11">
                #
              </th>
              <th
                scope="col"
                className="w-[150px] min-w-[150px] sticky left-[32px] bg-[#ffffff] z-11 shadow-[3px_0_4px_0_rgba(99,84,84,0.3)]"
              >
                <div>{t('dataGrid.username')}</div>
                <div>{t('dataGrid.nickname')}</div>
              </th>
              <th scope="col">
                <div>{t('dataGrid.firstLastName')}</div>
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
            {paginatedDeposits.length > 0 ? (
              paginatedDeposits.map((deposit, index) => (
                <tr key={deposit.transId}>
                  <td className="w-[32px] min-w-[32px] sticky left-0 bg-[#ffffff] z-10">
                    {index + 1}
                  </td>
                  <td className="w-[150px] min-w-[150px] sticky left-[32px] bg-[#ffffff] z-10 shadow-[3px_0_4px_0_rgba(99,84,84,0.3)]">
                    <div>{deposit.username}</div>
                    <div>{deposit.nickname}</div>
                  </td>
                  <td>
                    <div>{deposit.firstName}</div>
                    <div>{deposit.lastName}</div>
                  </td>
                  <td>{displayAmount(deposit.amount)}</td>
                  <td>{renderBankAccount(deposit.paymentFrom)}</td>
                  <td>{renderBankAccount(deposit.paymentTo)}</td>
                  <td>{deposit.transId}</td>
                  <td>{formatDateTime(deposit.createdTime)}</td>
                  <td>
                    <ButtonFullComponent
                      text={t('dataGrid.process')}
                      disabled={isLoading}
                      onClick={() => handleProcessClick(deposit)}
                      className=""
                    />
                  </td>
                </tr>
              ))
            ) : (
              <NoRecordFound colSpan={11} message={t('dataGrid.noDepositsFound')} />
            )}
          </tbody>
        </table>
      </div>

      {error && <div className="mt-4 p-4 bg-red-100 text-red-700 rounded-md">{error}</div>}

      {processingDeposit && (
        <PendingDepositModal deposit={processingDeposit} onClose={handleCloseProcess} />
      )}

      {paginatedDeposits.length > 0 && (
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
