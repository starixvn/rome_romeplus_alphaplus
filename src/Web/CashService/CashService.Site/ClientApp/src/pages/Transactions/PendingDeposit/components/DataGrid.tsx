import React, { useState } from 'react';
import { DepositAccountInfo } from '../../../../services/interfaces/DepositAccountInfo';
import PendingDepositModal from './PendingDepositModal';
import { useTranslation } from 'react-i18next';

interface DataGridProps {
  deposits: DepositAccountInfo[];
}

const DataGrid: React.FC<DataGridProps> = ({ deposits }) => {
  const { t } = useTranslation('pendingDeposit');
  const [processingDeposit, setProcessingDeposit] = useState<DepositAccountInfo | null>(null);

  const handleProcessClick = (deposit: DepositAccountInfo) => {
    setProcessingDeposit(deposit);
  };

  const handleCloseProcess = () => {
    setProcessingDeposit(null);
  };

  const handleApproveDeposit = (deposit: DepositAccountInfo) => {
    console.log('Approving deposit:', deposit);
    setProcessingDeposit(null);
  };

  const handleRejectDeposit = (deposit: DepositAccountInfo) => {
    console.log('Rejecting deposit:', deposit);
    setProcessingDeposit(null);
  };

  return (
    <>
      <div className="max-h-[50vh] overflow-auto shadow-[0_1px_5px_#0003,0_2px_2px_#00000024,0_3px_1px_-2px_#0000001f]">
        <table className="text-[14px] w-full border-collapse border border-[#0000001f] text-[#333333] shadow-[0_1px_5px_#0003,0_2px_2px_#00000024,0_3px_1px_-2px_#0000001f]">
          <thead className="sticky top-0 shadow-[0_2px_5px_#0003]">
            <tr>
              <th scope="col" className="bg-white py-1 px-2 text-center font-bold border border-[#dee2e6] text-gray-700">#</th>
              <th scope="col" className="bg-white py-1 px-2 text-center font-bold border border-[#dee2e6] text-gray-700">{t('dataGrid.username')}</th>
              <th scope="col" className="bg-white py-1 px-2 text-center font-bold border border-[#dee2e6] text-gray-700">{t('dataGrid.loginName')}</th>
              <th scope="col" className="bg-white py-1 px-2 text-center font-bold border border-[#dee2e6] text-gray-700">
                <div>{t('dataGrid.firstName')}</div>
                <div>{t('dataGrid.lastName')}</div>
              </th>
              <th scope="col" className="bg-white py-1 px-2 text-center font-bold border border-[#dee2e6] text-gray-700">{t('dataGrid.amount')}</th>
              <th scope="col" className="bg-white py-1 px-2 text-center font-bold border border-[#dee2e6] text-gray-700">{t('dataGrid.currency')}</th>
              <th scope="col" className="bg-white py-1 px-2 text-center font-bold border border-[#dee2e6] text-gray-700">{t('dataGrid.from')}</th>
              <th scope="col" className="bg-white py-1 px-2 text-center font-bold border border-[#dee2e6] text-gray-700">{t('dataGrid.to')}</th>
              <th scope="col" className="bg-white py-1 px-2 text-center font-bold border border-[#dee2e6] text-gray-700">{t('dataGrid.referenceNo')}</th>
              <th scope="col" className="bg-white py-1 px-2 text-center font-bold border border-[#dee2e6] text-gray-700">{t('dataGrid.dateAndTime')}</th>
              <th scope="col" className="bg-white py-1 px-2 text-center font-bold border border-[#dee2e6] text-gray-700">{t('dataGrid.actions')}</th>
            </tr>
          </thead>
          <tbody className="font-[500]">
            {deposits.length > 0 ? (
              deposits.map((deposit, index) => (
                <tr key={deposit.id}>
                  <td className="py-1 px-2 border border-[#dee2e6] text-center">{index + 1}</td>
                  <td className="py-1 px-2 border border-[#dee2e6] text-center">{deposit.username}</td>
                  <td className="py-1 px-2 border border-[#dee2e6] text-center">{deposit.loginName}</td>
                  <td className="py-1 px-2 border border-[#dee2e6] text-center">
                    <div>{deposit.firstName}</div>
                    <div>{deposit.lastName}</div>
                  </td>
                  <td className="py-1 px-2 border border-[#dee2e6] text-center">{deposit.amount.toFixed(2)}</td>
                  <td className="py-1 px-2 border border-[#dee2e6] text-center">{deposit.currency}</td>
                  <td className="py-1 px-2 border border-[#dee2e6] text-center">{deposit.from}</td>
                  <td className="py-1 px-2 border border-[#dee2e6] text-center">{deposit.to}</td>
                  <td className="py-1 px-2 border border-[#dee2e6] text-center">{deposit.referenceNo}</td>
                  <td className="py-1 px-2 border border-[#dee2e6] text-center">
                    {deposit.dateAndTimeStr}
                  </td>
                  <td className="py-1 px-2 border border-[#dee2e6] text-center">
                    <button 
                      className="font-bold text-[13px] tracking-[0.12px] leading-[26px] px-3 rounded-[4px] bg-[#163f78] hover:bg-[#375A8A] text-[#ffffff] cursor-pointer"
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