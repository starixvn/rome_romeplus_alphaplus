import React, { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { TransactionItem } from '../../../../services/interfaces/Transactions/TransactionHistory';
import {
  displayAmount,
  renderBankAccount,
  formatDateTime,
} from '../../../../utils/commonFunctions';
import NoRecordFound from '../../../../components/Table/NoRecordFound';
import Pagination from '../../Components/Pagination';
import { ButtonFullComponent } from '../../../../components/ButtonComponents';
import PendingDepositModal from './PendingDepositModal';
import { PAGE_SIZE_OPTIONS } from '../../../../constants/paymentConstants';

interface DataGridProps {
  deposits: TransactionItem[];
}

const TableHeader: React.FC<{ t: (key: string) => string }> = ({ t }) => (
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
        <div>{t('dataGrid.firstLastName')}</div>
      </th>
      <th scope="col">{t('dataGrid.amount')}</th>
      <th scope="col">{t('dataGrid.from')}</th>
      <th scope="col">{t('dataGrid.to')}</th>
      <th scope="col">{t('dataGrid.referenceNo')}</th>
      <th scope="col">{t('dataGrid.dateAndTime')}</th>
      <th scope="col">{t('dataGrid.actions')}</th>
    </tr>
  </thead>
);

interface TableRowProps {
  deposit: TransactionItem;
  index: number;
  currentPage: number;
  pageSize: number;
  onProcess: (deposit: TransactionItem) => void;
  t: (key: string) => string;
}

const TableRow: React.FC<TableRowProps> = ({
  deposit,
  index,
  currentPage,
  pageSize,
  onProcess,
  t,
}) => (
  <tr key={deposit.transId}>
    <td className="w-[32px] min-w-[32px] sticky left-0 bg-[var(--table-background)] z-10">
      {index + 1 + (currentPage - 1) * pageSize}
    </td>
    <td className="w-[150px] min-w-[150px] sticky left-[32px] bg-[var(--table-background)] z-10 shadow-[3px_0_4px_0_rgba(99,84,84,0.3)]">
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
        onClick={() => onProcess(deposit)}
        className=""
      />
    </td>
  </tr>
);

const DataGrid: React.FC<DataGridProps> = ({ deposits }) => {
  const { t } = useTranslation('pendingDeposit');
  const [currentPage, setCurrentPage] = useState(1);
  const [pageSize, setPageSize] = useState(PAGE_SIZE_OPTIONS[2]);
  const [paginatedDeposits, setPaginatedDeposits] = useState<TransactionItem[]>([]);
  const [error, setError] = useState<string | null>(null);
  const [showDepositPopup, setShowDepositPopup] = useState<boolean>(false);
  const [selectedDeposit, setSelectedDeposit] = useState<TransactionItem | null>(null);
  const totalItems = deposits.length;

  useEffect(() => {
    if (deposits.length <= pageSize) {
      setCurrentPage(1);
    }
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
    setShowDepositPopup(true);
    setSelectedDeposit(deposit);
    setError(null);
  };

  const handleCloseProcess = () => {
    setShowDepositPopup(false);
    setError(null);
  };

  return (
    <>
      <div className="table-fixed-header table-fixed-body min-h-0">
        <table className="w-full whitespace-nowrap border-collapse border border-[#0000001f] border-t-[0px] border-l-[0px]">
          <TableHeader t={t} />
          <tbody className="font-[500]">
            {paginatedDeposits.length > 0 ? (
              paginatedDeposits.map((deposit, index) => (
                <TableRow
                  key={deposit.transId}
                  deposit={deposit}
                  index={index}
                  currentPage={currentPage}
                  pageSize={pageSize}
                  onProcess={handleProcessClick}
                  t={t}
                />
              ))
            ) : (
              <NoRecordFound colSpan={11} message={t('dataGrid.noDepositsFound')} />
            )}
          </tbody>
        </table>
      </div>

      {error && <div className="mt-4 p-4 bg-red-100 text-red-700 rounded-md">{error}</div>}

      {showDepositPopup && selectedDeposit && (
        <PendingDepositModal
          transId={selectedDeposit.transId}
          custId={selectedDeposit.custId}
          onClose={handleCloseProcess}
        />
      )}

      {paginatedDeposits.length > 0 && (
        <div className="mt-[-1px]">
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
