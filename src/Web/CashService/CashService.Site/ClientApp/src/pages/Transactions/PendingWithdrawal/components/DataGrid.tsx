import React, { useState, useEffect } from 'react';
import { useTranslation } from 'react-i18next';
import { TransactionItem } from '../../../../services/interfaces/Transactions/TransactionHistory.ts';
import {
  displayAmount,
  formatDateTime,
  renderBankAccount,
} from '../../../../utils/commonFunctions.tsx';
import PendingWithdrawalModal from './PendingWithdrawalModal.tsx';
import NoRecordFound from '../../../../components/NoRecordFoundComponent.tsx';
import { ButtonFullComponent } from '../../../../components/Buttons/index';
import Pagination from '../../Components/Pagination';
import { PAGE_SIZE_OPTIONS } from '../../../../constants/paymentConstants';
import { ValidateMessageComponent } from '../../../../components/Message';
import { IconImage } from '../../../../components/Icons.tsx';

interface DataGridProps {
  withdrawals: TransactionItem[];
  reloadGrid: () => void;
  isCashServiceEnabled: boolean;
}

const DataGrid: React.FC<DataGridProps> = ({ withdrawals, reloadGrid }) => {
  const { t } = useTranslation('pendingWithdrawal');
  const { t: tGlobal } = useTranslation('global');
  const [showWithdrawalPopup, setShowWithdrawalPopup] = useState<boolean>(false);
  const [selectedWithdrawal, setSelectedWithdrawal] = useState<TransactionItem | null>(null);
  const [error, setError] = useState<string | null>(null);
  const [currentPage, setCurrentPage] = useState(1);
  const [pageSize, setPageSize] = useState(PAGE_SIZE_OPTIONS[2]);
  const [paginatedWithdrawals, setPaginatedWithdrawals] = useState<TransactionItem[]>([]);
  const totalItems = withdrawals.length;

  useEffect(() => {
    if (withdrawals.length <= pageSize) {
      setCurrentPage(1);
    }
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
    setShowWithdrawalPopup(true);
    setSelectedWithdrawal(withdrawal);
  };

  const handleCloseProcess = () => {
    setShowWithdrawalPopup(false);
    setError(null);
    reloadGrid();
  };

  return (
    <>
      <div className="table table-fixed-body min-h-0">
        <table className="w-full whitespace-nowrap border-collapse border border-[#0000001f] border-t-[0px] border-l-[0px] text-[#333333]">
          <thead className="z-11!">
            <tr>
              <th
                scope="col"
                className="w-[32px] min-w-[32px] sticky left-0 bg-[var(--table-header-background)] z-11"
              >
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
              <th scope="col" className="min-w-[150px]">
                {t('dataGrid.amount')}
              </th>
              <th scope="col" className="w-[250px]">
                {t('dataGrid.from')}
              </th>
              <th scope="col" className="w-[250px]">
                {t('dataGrid.to')}
              </th>
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
                  <td>
                    <div className="flex items-center justify-end pr-[25px] relative">                      
                      {displayAmount(withdrawal.amount)}
                      {withdrawal.isHadPayslip && (
                        <IconImage className="w-[20px] h-[20px] ml-[5px] absolute right-[0px]" />
                      )}
                    </div>
                  </td>
                  <td className="text-left">                   
                      {renderBankAccount(withdrawal.paymentFrom, tGlobal)}                     
                  </td>
                  <td className="text-left">                  
                      {renderBankAccount(withdrawal.paymentTo, tGlobal)}                   
                  </td>
                  <td>{withdrawal.transId}</td>
                  <td>{formatDateTime(withdrawal.createdTime)}</td>
                  <td>
                    <ButtonFullComponent
                      text={t('dataGrid.process')}
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
      {error && <ValidateMessageComponent message={error} />}
      {showWithdrawalPopup && selectedWithdrawal && (
        <PendingWithdrawalModal
          transId={selectedWithdrawal.transId}
          custId={selectedWithdrawal.custId}
          onClose={handleCloseProcess}
        />
      )}
      {paginatedWithdrawals.length > 0 && (
        <div className="mt-[-1px] relative z-12">
          <Pagination
            currentPage={currentPage}
            pageSize={pageSize}
            totalItems={totalItems}
            onPageChange={handlePageChange}
            onPageSizeChange={handlePageSizeChange}
          />
        </div>
      )}
    </>
  );
};

export default DataGrid;
