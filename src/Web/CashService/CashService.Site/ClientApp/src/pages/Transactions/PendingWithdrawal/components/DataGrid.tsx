import React, { useState } from 'react';
import { useTranslation } from 'react-i18next';
import { TransactionItem } from '../../../../services/interfaces/Transactions/TransactionHistory.ts';
import {
  CurrentCurrency,
  displayAmount,
  formatDateTime,
  renderBankAccount,
} from '../../../../common/commonFunctions.tsx';
import PendingWithdrawalModal from './PendingWithdrawalModal.tsx';

interface DataGridProps {
  withdrawals: TransactionItem[];
}

const DataGrid: React.FC<DataGridProps> = ({ withdrawals }) => {
  const { t } = useTranslation('pendingWithdrawal');
  const [processingWithdrawal, setProcessingWithdrawal] = useState<TransactionItem | null>(null);

  const handleProcessClick = (withdrawal: TransactionItem) => {
    setProcessingWithdrawal(withdrawal);
  };

  const handleCloseProcess = () => {
    setProcessingWithdrawal(null);
  };

  const handleApproveWithdrawal = (withdrawal: TransactionItem) => {
    console.log('Approving withdrawal:', withdrawal);
    setProcessingWithdrawal(null);
  };

  const handleRejectWithdrawal = (withdrawal: TransactionItem) => {
    console.log('Rejecting withdrawal:', withdrawal);
    setProcessingWithdrawal(null);
  };

  return (
    <>
      <div className="max-h-[50vh] overflow-auto shadow-[0_1px_5px_#0003,0_2px_2px_#00000024,0_3px_1px_-2px_#0000001f]">
        <table className="text-[14px] w-full border-collapse border border-[#0000001f] text-[#333333] shadow-[0_1px_5px_#0003,0_2px_2px_#00000024,0_3px_1px_-2px_#0000001f]">
          <thead className="sticky top-0 shadow-[0_2px_5px_#0003]">
            <tr>
              <th
                scope="col"
                className="bg-white py-1 px-2 text-center font-bold border border-[#dee2e6] text-gray-700"
              >
                #
              </th>
              <th
                scope="col"
                className="bg-white py-1 px-2 text-center font-bold border border-[#dee2e6] text-gray-700"
              >
                <div>{t('dataGrid.username')}</div>
                <div>{t('dataGrid.nickname')}</div>
              </th>
              <th
                scope="col"
                className="bg-white py-1 px-2 text-center font-bold border border-[#dee2e6] text-gray-700"
              >
                <div>{t('dataGrid.firstName')}</div>
              </th>
              <th
                scope="col"
                className="bg-white py-1 px-2 text-center font-bold border border-[#dee2e6] text-gray-700"
              >
                {t('dataGrid.amount')} ({CurrentCurrency})
              </th>
              <th
                scope="col"
                className="bg-white py-1 px-2 text-center font-bold border border-[#dee2e6] text-gray-700"
              >
                {t('dataGrid.from')}
              </th>
              <th
                scope="col"
                className="bg-white py-1 px-2 text-center font-bold border border-[#dee2e6] text-gray-700"
              >
                {t('dataGrid.to')}
              </th>
              <th
                scope="col"
                className="bg-white py-1 px-2 text-center font-bold border border-[#dee2e6] text-gray-700"
              >
                {t('dataGrid.referenceNo')}
              </th>
              <th
                scope="col"
                className="bg-white py-1 px-2 text-center font-bold border border-[#dee2e6] text-gray-700"
              >
                {t('dataGrid.dateAndTime')}
              </th>
              <th
                scope="col"
                className="bg-white py-1 px-2 text-center font-bold border border-[#dee2e6] text-gray-700"
              >
                {t('dataGrid.actions')}
              </th>
            </tr>
          </thead>
          <tbody className="font-[500]">
            {withdrawals.length > 0 ? (
              withdrawals.map((withdrawal, index) => (
                <tr key={withdrawal.transId}>
                  <td className="py-1 px-2 border border-[#dee2e6] text-center">{index + 1}</td>
                  <td className="py-1 px-2 border border-[#dee2e6] text-center">
                    {withdrawal.username}
                  </td>
                  <td className="py-1 px-2 border border-[#dee2e6] text-center">
                    <div>{withdrawal.firstName}</div>
                    <div>{withdrawal.lastName}</div>
                  </td>
                  <td className="py-1 px-2 border border-[#dee2e6] text-center">
                    {displayAmount(withdrawal.amount)}
                  </td>
                  <td className="py-1 px-2 border border-[#dee2e6] text-center">
                    {renderBankAccount(withdrawal.paymentFrom)}
                  </td>
                  <td className="py-1 px-2 border border-[#dee2e6] text-center">
                    {renderBankAccount(withdrawal.paymentTo)}
                  </td>
                  <td className="py-1 px-2 border border-[#dee2e6] text-center">
                    {withdrawal.transId}
                  </td>
                  <td className="py-1 px-2 border border-[#dee2e6] text-center">
                    {formatDateTime(withdrawal.createdTime)}
                  </td>
                  <td className="py-1 px-2 border border-[#dee2e6] text-center">
                    <button
                      className="font-bold text-[13px] tracking-[0.12px] leading-[26px] px-3 rounded-[4px] bg-[#163f78] hover:bg-[var(--button-background-hover)] text-[#ffffff] cursor-pointer"
                      onClick={() => handleProcessClick(withdrawal)}
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
                    <div className="text-sm text-gray-500">{t('dataGrid.noWithdrawalsFound')}</div>
                  </div>
                </td>
              </tr>
            )}
          </tbody>
        </table>
      </div>

      {processingWithdrawal && (
        <PendingWithdrawalModal
          withdrawal={processingWithdrawal}
          onClose={handleCloseProcess}
          onApprove={handleApproveWithdrawal}
          onReject={handleRejectWithdrawal}
        />
      )}
    </>
  );
};

export default DataGrid;
