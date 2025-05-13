import React, { useState } from 'react';
import { useTranslation } from 'react-i18next';
import { DepositAccountInfo } from '../../../../services/interfaces/DepositAccountInfo';

interface PendingDepositModalProps {
  deposit: DepositAccountInfo;
  onClose: () => void;
  onApprove: (deposit: DepositAccountInfo) => void;
  onReject: (deposit: DepositAccountInfo) => void;
}
const PendingDepositModal: React.FC<PendingDepositModalProps> = ({ 
  deposit, 
  onClose, 
  onApprove, 
  onReject 
}) => {
  const { t } = useTranslation('pendingDeposit');
  const [remarks, setRemarks] = useState('');
  const [depositAmount, setDepositAmount] = useState(deposit.amount.toString());

  const handleApprove = () => {
    onApprove({
      ...deposit,
      amount: parseFloat(depositAmount)
    });
  };

  const handleReject = () => {
    onReject({
      ...deposit,
      amount: parseFloat(depositAmount)
    });
  };

  // Format name for display
  const formattedName = `${deposit.firstName} ${deposit.lastName}`;

  return (
    <div className="bg-[#00000066] fixed inset-0 flex items-center justify-center z-50">
      <div className="p-[30px] bg-white max-w-[600px] rounded-[4px] shadow-[0_2px_4px_-1px_#00000033,0_4px_5px_#00000024,0_1px_10px_#0000001f]">
        <div className="relative">
          <h5 className="text-[16px] text-[#163f78] font-bold leading-[21.6px] tracking-[0.2px] pb-[15px]">
            {t('pendingDepositModal.title')} ({deposit.referenceNo})
          </h5>
          <button type="button" className="absolute top-[-20px] right-[-20px] cursor-pointer" onClick={onClose}>
            <svg xmlns="http://www.w3.org/2000/svg" className="h-5 w-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
              <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M6 18L18 6M6 6l12 12" />
            </svg>
          </button>
        </div>

        <div className="modal-body">
          <div className="border border-[#214184] rounded mb-6">
            <table className="w-full">
              <thead>
                <tr>
                  <th className="text-sm text-left p-3 text-gray-700">Username</th>
                  <th className="text-sm text-left p-3 text-gray-700">Login Name</th>
                  <th className="text-sm text-left p-3 text-gray-700">First/Last Name</th>
                  <th className="text-sm text-left p-3 text-gray-700">Bet Credit</th>
                </tr>
              </thead>
              <tbody>
                <tr>
                  <td className="text-sm p-3">{deposit.username}</td>
                  <td className="text-sm p-3">{deposit.loginName}</td>
                  <td className="text-sm p-3">{formattedName}</td>
                  <td className="text-sm p-3">{deposit.amount.toFixed(2)}</td>
                </tr>
              </tbody>
            </table>
          </div>

          <div className="grid grid-cols-2 gap-6 mb-6">
            <div>
              <div className="mb-4">
                <div className="text-sm font-medium mb-1">From</div>
                <div className="flex items-center">
                  <div className="mr-2 flex items-center justify-center w-8 h-8 rounded-full bg-green-100 text-green-600">
                    <span>🔄</span>
                  </div>
                  <span>{deposit.from}</span>
                </div>
              </div>

              <div>
                <div className="text-sm font-medium mb-1">Date and Time</div>
                <div>
                  {deposit.dateAndTimeStr}
                </div>
              </div>
            </div>

            <div>
              <div className="mb-4">
                <div className="text-sm font-medium mb-1">To</div>
                <div className="flex items-center">
                  <div className="mr-2 flex items-center justify-center w-8 h-8 rounded-full bg-blue-100 text-blue-600">
                    <span>🔄</span>
                  </div>
                  <span>{deposit.to}</span>
                </div>
              </div>

              <div>
                <div className="text-sm font-medium mb-1">Deposit Amount</div>
                <div className="relative">
                  <input 
                    type="text"
                    value={depositAmount} 
                    onChange={(e) => setDepositAmount(e.target.value)}
                    className="border border-gray-300 rounded-md py-1 px-2 w-full h-[32px] focus:outline-none focus:ring-1 focus:ring-[#214184] focus:border-[#214184]"
                  />
                </div>
              </div>
            </div>
          </div>

          <div className="mb-6">
            <div className="text-sm font-medium mb-1">Remarks</div>
            <textarea 
              value={remarks}
              onChange={(e) => setRemarks(e.target.value)}
              placeholder="Add a remark here..."
              className="border border-gray-300 rounded-md p-2 w-full h-[120px] focus:outline-none focus:ring-1 focus:ring-[#214184] focus:border-[#214184] text-gray-700"
            />
          </div>
        </div>
        
        <div className="flex justify-end space-x-3 mt-[-10px]">
          <button
            onClick={handleReject}
            className="w-[140px] capitalize font-bold h-[32px] rounded-[20px] px-[16px] text-[#163f78] py-[4px] text-[12px] border-[1px] border-[#163f78] mx-[4px] bg-[#ffffff] hover:bg-[#dee2e7] cursor-pointer shadow-[0_1px_5px_#0003,0_2px_2px_#00000024,0_3px_1px_-2px_#0000001f]"
          >
            Reject
          </button>
          <button
            onClick={handleApprove}
            className="w-[140px] capitalize font-bold h-[32px] rounded-[20px] text-white px-[16px] py-[4px] text-[12px] border-[1px] border-[#163f78] mx-[4px] bg-[#163f78] hover:bg-[var(--button-background-hover)] cursor-pointer shadow-[0_1px_5px_#0003,0_2px_2px_#00000024,0_3px_1px_-2px_#0000001f]"
          >
            Approve
          </button>
        </div>
      </div>
    </div>
  );
};

export default PendingDepositModal; 
