import React, { useState } from 'react';
import { useTranslation } from 'react-i18next';
import { TransactionItem } from '../../../../services/interfaces/Transactions/TransactionHistory';
import { IconClose } from '../../../../components/Icons';
import { ButtonApplyComponent, ButtonCancelComponent } from '../../../../components/ButtonComponents';
import { renderBankAccount } from '../../../../common/commonFunctions';

interface PendingDepositModalProps {
  deposit: TransactionItem;
  onClose: () => void;
  onApprove: (deposit: TransactionItem) => void;
  onReject: (deposit: TransactionItem) => void;
}
const PendingDepositModal: React.FC<PendingDepositModalProps> = ({
  deposit,
  onClose,
  onApprove,
  onReject,
}) => {
  const { t } = useTranslation('pendingDeposit');
  const [remarks, setRemarks] = useState('');
  const [depositAmount, setDepositAmount] = useState(deposit.amount.toString());

  const handleApprove = () => {
    onApprove({
      ...deposit,
      amount: parseFloat(depositAmount),
    });
  };

  const handleReject = () => {
    onReject({
      ...deposit,
      amount: parseFloat(depositAmount),
    });
  };

  return (
    <div className="bg-[#00000066] fixed z-[100] top-0 left-0 right-0 bottom-0 flex items-start justify-center">
      <div className="p-[30px] bg-[#ffffff] mt-[100px] max-w-[540px] max-[768px]:max-w-[90%] rounded-[4px] shadow-[0_2px_4px_-1px_#00000033,0_4px_5px_#00000024,0_1px_10px_#0000001f]">
        <div className="relative mb-[16px]">
          <h5 className="text-[16px] text-[var(--text-color-primary)] font-semibold leading-[21.6px] max-[768px]:text-[20px] max-[768px]:leading-[26px]">
            {t('pendingDepositModal.title')} ({deposit.transId})
          </h5>
          <button
              type="button"
              className="absolute top-[-20px] right-[-20px] cursor-pointer"
              onClick={onClose}
            >
              <IconClose />
            </button>
        </div>

        <div className="modal-body">
          <div className="border border-[#333333] rounded mb-[8px] p-[8px]">
            <table className="w-full">
              <thead>
                <tr className="text-left text-[#333333]">
                  <th className="font-bold px-[8px] py-[4px]">Username</th>
                  <th className="font-bold px-[8px] py-[4px]">Login Name</th>
                  <th className="font-bold px-[8px] py-[4px]">First/Last Name</th>
                  <th className="font-bold px-[8px] py-[4px]">Bet Credit</th>
                </tr>
              </thead>
              <tbody>
                <tr>
                  <td className="px-[8px] py-[4px]">{deposit.username}</td>
                  <td className="px-[8px] py-[4px]">{}</td>
                  <td className="px-[8px] py-[4px]">{`${deposit.firstName} ${deposit.lastName}`}</td>
                  <td className="px-[8px] py-[4px] text-[var(--text-color-primary)] font-semibold">{deposit.amount.toFixed(2)}</td>
                </tr>
              </tbody>
            </table>
          </div>

          <div className="flex flex-nowrap justify-between gap-[16px] max-[768px]:justify-between max-[768px]:gap-[0] max-[768px]:flex-wrap w-full">
            <div className="w-[230px] max-[768px]:w-[48%]">
              <div className="mb-[8px]">
                <div className="block font-bold text-[#333333] my-[8px]">From</div>
                <div className="flex items-center">                   
                  {renderBankAccount(deposit.paymentFrom)}
                </div>
              </div>

              <div>
                <div className="block font-bold text-[#333333] my-[8px]">Date and Time</div>
                <div>{deposit.createdTime}</div>
              </div>
            </div>

            <div className="w-[230px] max-[768px]:w-[48%]">
              <div className="mb-[8px]">
                <div className="block font-bold text-[#333333] my-[8px]">To</div>                 
                {renderBankAccount(deposit.paymentTo)}
              </div>

              <div>
                <div className="block font-bold text-[#333333] my-[8px]">Deposit Amount</div>
                <div className="relative">
                  <input
                    type="text"
                    value={depositAmount}
                    onChange={(e) => setDepositAmount(e.target.value)}
                    className="h-[32px] max-[768px]:h-[40px] w-[100%] border-[1px] rounded-[4px] px-[8px]  focus:outline-none border-[var(--border-control)] focus:border-[var(--button-border)]"
                  />
                </div>
              </div>
            </div>
          </div>

          <div className="mb-[16px]">
            <div className="block font-bold text-[#333333] my-[8px]">Remarks</div>
            <textarea
              value={remarks}
              onChange={(e) => setRemarks(e.target.value)}
              placeholder="Add a remark here..."
              className="block h-[150px] max-[768px]:h-[40px] w-[100%] border-[1px] border-[var(--border-control)] rounded-[4px] px-[8px] py-[10px] focus:outline-none focus:border-[var(--button-border)]"
            />
          </div>
        </div>

        <div className="flex justify-center gap-[8px]">
        <ButtonCancelComponent
            text="Reject"
            onClick={handleReject}
          />
            <ButtonApplyComponent
            text="Approve"
            onClick={handleApprove}
          />
           
        </div>
      </div>
    </div>
  );
};

export default PendingDepositModal;
