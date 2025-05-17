import React, { useState } from 'react';
import PendingDepositModal from './PendingDepositModal';
import { useTranslation } from 'react-i18next';
import { TransactionItem } from '../../../../services/interfaces/Transactions/TransactionHistory';
import { displayAmount, renderBankAccount } from '../../../../common/commonFunctions';

interface DataGridProps {
  deposits: TransactionItem[];
}

const DataGrid: React.FC<DataGridProps> = ({ deposits }) => {
  const { t } = useTranslation('pendingDeposit');
  const [processingDeposit, setProcessingDeposit] = useState<TransactionItem | null>(null);

  const handleProcessClick = (deposit: TransactionItem) => {
    setProcessingDeposit(deposit);
  };

  const handleCloseProcess = () => {
    setProcessingDeposit(null);
  };

  const handleApproveDeposit = (deposit: TransactionItem) => {
    console.log('Approving deposit:', deposit);
    setProcessingDeposit(null);
  };

  const handleRejectDeposit = (deposit: TransactionItem) => {
    console.log('Rejecting deposit:', deposit);
    setProcessingDeposit(null);
  };

  return (
    <>
      <div className="table-fixed-header table-fixed-body bg-[#ffffff] h-full">
        <table className="text-[12px] max-[600px]:text-[17px] w-full whitespace-nowrap border-collapse border border-[#0000001f] border-t-[0px]  border-l-[0px] text-[#333333] shadow-[0_1px_5px_#0003,0_2px_2px_#00000024,0_3px_1px_-2px_#0000001f]">
          <thead>
            <tr>
              <th scope="col">#</th>
              <th scope="col">{t('dataGrid.username')}</th>
              <th scope="col"><div>{t('dataGrid.firstName')}</div>                <div>{t('dataGrid.lastName')}</div></th>
              <th scope="col">{t('dataGrid.amount')}</th>
              <th scope="col">{t('dataGrid.currency')}</th>
              <th scope="col">{t('dataGrid.from')}</th>
              <th scope="col">{t('dataGrid.to')}</th>
              <th scope="col">{t('dataGrid.referenceNo')}</th>
              <th scope="col">{t('dataGrid.dateAndTime')}</th>
              <th scope="col">{t('dataGrid.actions')}</th>
            </tr>
          </thead>
          <tbody className="font-[500]">
            {deposits.length > 0 ? (
              deposits.map((deposit, index) => (
                <tr key={deposit.transId}>
                  <td>{index + 1}</td>
                  <td>
                    {deposit.username}
                  </td>
                  <td>
                    <div>{deposit.firstName}</div>
                    <div>{deposit.lastName}</div>
                  </td>
                  <td>
                    {displayAmount(deposit.amount)}
                  </td>
                  <td>
                    {deposit.currency}
                  </td>
                  <td>
                    {renderBankAccount(deposit.paymentFrom)}
                  </td>
                  <td>
                    {renderBankAccount(deposit.paymentTo)}
                  </td>
                  <td>
                    {deposit.transId}
                  </td>
                  <td>
                    {deposit.createdTime}
                  </td>
                  <td>
                    <button
                      className="font-bold text-[13px] tracking-[0.12px] leading-[26px] px-3 rounded-[4px] bg-[#163f78] hover:bg-[var(--button-background-hover)] text-[#ffffff] cursor-pointer"
                      onClick={() => handleProcessClick(deposit)}
                    >
                      {t('dataGrid.process')}
                    </button>
                  </td>
                </tr>
              ))
            ) : (
              <tr>
                <td colSpan={11} className="p-3 text-center border border-[#dee2e6]">
                  <div className="flex flex-col items-center justify-center py-16">
                    <svg
                      width="70"
                      height="70"
                      viewBox="0 0 70 70"
                      fill="none"
                      xmlns="http://www.w3.org/2000/svg"
                      className="mb-4 text-gray-400"
                    >
                      <g opacity="0.5">
                        <path
                          d="M55 15H15C12.2386 15 10 17.2386 10 20V50C10 52.7614 12.2386 55 15 55H55C57.7614 55 60 52.7614 60 50V20C60 17.2386 57.7614 15 55 15Z"
                          stroke="#9CA3AF"
                          strokeWidth="2"
                          strokeLinecap="round"
                          strokeLinejoin="round"
                        />
                        <circle cx="35" cy="35" r="5" stroke="#9CA3AF" strokeWidth="2" />
                        <path
                          d="M35 35L45 45"
                          stroke="#9CA3AF"
                          strokeWidth="2"
                          strokeLinecap="round"
                        />
                        <path
                          d="M20 26V26.01"
                          stroke="#9CA3AF"
                          strokeWidth="2"
                          strokeLinecap="round"
                        />
                        <path
                          d="M26 26V26.01"
                          stroke="#9CA3AF"
                          strokeWidth="2"
                          strokeLinecap="round"
                        />
                        <path
                          d="M32 26V26.01"
                          stroke="#9CA3AF"
                          strokeWidth="2"
                          strokeLinecap="round"
                        />
                      </g>
                    </svg>
                    <div className="text-sm text-gray-500">{t('dataGrid.noDepositsFound')}</div>
                  </div>
                </td>
              </tr>
            )}
          </tbody>
        </table>
      </div>

      {processingDeposit && (
        <PendingDepositModal
          deposit={processingDeposit}
          onClose={handleCloseProcess}
          onApprove={handleApproveDeposit}
          onReject={handleRejectDeposit}
        />
      )}
    </>
  );
};

export default DataGrid;
