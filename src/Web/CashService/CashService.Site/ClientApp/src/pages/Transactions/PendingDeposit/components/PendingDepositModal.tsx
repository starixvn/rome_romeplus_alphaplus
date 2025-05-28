import React, { useState } from 'react';
import { useTranslation } from 'react-i18next';
import { TransactionItem } from '../../../../services/interfaces/Transactions/TransactionHistory';
import {
  ButtonApplyComponent,
  ButtonCancelComponent,
  ButtonClosePopupComponent,
} from '../../../../components/ButtonComponents';
import {
  renderBankAccount,
  displayAmount,
  formatDateTime,
} from '../../../../common/commonFunctions';
import pendingDepositAPI from '../../../../services/pendingDepositAPI';
import { ApproveRejectDepositModel } from '../../../../services/interfaces/ApproveRejectDepositModel.ts';
import { ResultMessageComponent } from '../../../../components/MessageComponents.tsx';
import { AmountInput } from '../../../../components/ui/AmountInput.tsx';
import { LableTitleComponent, MediumTitleComponent } from '../../../../components/TitleComponents.tsx';

interface PendingDepositModalProps {
  deposit: TransactionItem | null;
  onClose: (isSuccess: boolean) => void;
}
const PendingDepositModal: React.FC<PendingDepositModalProps> = ({ deposit, onClose }) => {
  const { t } = useTranslation('pendingDeposit');
  const [remarks, setRemarks] = useState('');
  const [depositAmount, setDepositAmount] = useState<number>(deposit?.amount || 0);
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [resultMessage, setResultMessage] = useState<{
    type: 'success' | 'error';
    message: string;
  } | null>(null);

  const getParams = () => {
    const params: ApproveRejectDepositModel = {
      transId: deposit?.transId ? deposit.transId : 0,
      amount: depositAmount,
      remark: remarks,
    };
    return params;
  };

  const handleApprove = async () => {
    const params = getParams();
    setIsSubmitting(true);

    try {
      const response = await pendingDepositAPI.approveDeposit(params);
      if (response.isSuccessful) {
        setResultMessage({
          type: 'success',
          message: t('successMessages.depositApproved'),
        });
        setTimeout(() => {
          onClose(true);
        }, 3000);
      } else {
        setIsSubmitting(false);
        setResultMessage({
          type: 'error',
          message: response.message || t('errorMessages.failedToApprove'),
        });
      }
    } catch (error) {
      console.error('Error processing deposit:', error);
    }
  };

  const handleReject = async () => {
    const params = getParams();
    setIsSubmitting(true);

    try {
      const response = await pendingDepositAPI.rejectDeposit(params);
      if (response.isSuccessful) {
        setResultMessage({
          type: 'success',
          message: t('successMessages.depositRejected'),
        });
        setTimeout(() => {
          onClose(true);
        }, 3111000);
      } else {
        setIsSubmitting(false);
        setResultMessage({
          type: 'error',
          message: t('errorMessages.failedToReject'),
        });
      }
    } catch (error) {
      console.error('Error processing deposit:', error);
    }
  };

  const handleSetDepositAmount = (value: number) => {
    setDepositAmount(value);
  };

  return (
    <div className="bg-[#00000066] fixed z-[100] top-0 left-0 right-0 bottom-0 flex items-start justify-center">
      <div className="p-[30px] w-full bg-[#ffffff] mt-[100px] max-w-[540px] max-[768px]:max-w-[90%] rounded-[4px] shadow-[0_2px_4px_-1px_#00000033,0_4px_5px_#00000024,0_1px_10px_#0000001f]">
        <div className="relative mb-[16px]">
          <MediumTitleComponent text={` ${t('pendingDepositModal.title')} (${deposit?.transId})`} />
          <ButtonClosePopupComponent onClick={() => onClose(false)} />
        </div>
        {resultMessage && (
          <ResultMessageComponent
            type={resultMessage.type}
            message={resultMessage.message}
            className="mb-[16px] max-[768px]:mb-[0]"
          />
        )}
        <div className="modal-body">
          <div className="border border-[#333333] rounded mb-[8px] p-[8px]">
            <table className="w-full">
              <thead>
                <tr className="text-left text-[#333333] font-semibold">
                  <th className="px-[8px] py-[4px]">
                    {t('pendingDepositModal.username')}
                  </th>
                  <th className="px-[8px] py-[4px]">
                    {t('pendingDepositModal.firstLastName')}
                  </th>
                  <th className="px-[8px] py-[4px]">
                    {t('pendingDepositModal.betCredit')}
                  </th>
                </tr>
              </thead>
              <tbody>
                <tr>
                  <td className="px-[8px] py-[4px]">{deposit?.username}</td>
                  <td className="px-[8px] py-[4px]">{`${deposit?.firstName} ${deposit?.lastName}`}</td>
                  <td className="px-[8px] py-[4px] text-[var(--text-color-primary)] font-semibold">
                    {deposit?.amount && displayAmount(deposit.amount)}
                  </td>
                </tr>
              </tbody>
            </table>
          </div>

          <div className="flex flex-nowrap justify-between gap-[16px] max-[768px]:justify-between max-[768px]:gap-[0] max-[768px]:flex-wrap w-full">
            <div className="w-[230px] max-[768px]:w-[48%]">
              <div className="mb-[16px]">
                <LableTitleComponent text={t('pendingDepositModal.from')} />                
                <div className="flex items-center">
                  {deposit?.paymentFrom && renderBankAccount(deposit.paymentFrom)}
                </div>
              </div>
              <div>
                <LableTitleComponent text={t('pendingDepositModal.dateAndTime')} />                 
                <div>{formatDateTime(deposit?.createdTime || '')}</div>
              </div>
            </div>

            <div className="w-[230px] max-[768px]:w-[48%]">
              <div className="mb-[16px]">
                <LableTitleComponent text={t('pendingDepositModal.to')} />                 
                {deposit?.paymentTo && renderBankAccount(deposit.paymentTo)}
              </div>

              <div>
                <LableTitleComponent text={t('pendingDepositModal.depositAmount')} />                 
                <div className="relative">
                  <AmountInput
                    value={depositAmount}
                    onChange={(e) => handleSetDepositAmount(e)}
                    isDisabled={isSubmitting}
                    className="border border-gray-300 rounded-md py-1 px-2 w-full h-[32px] focus:outline-none focus:ring-1 focus:ring-[#214184] focus:border-[#214184]"
                  />
                </div>
              </div>
            </div>
          </div>

          <div className="mb-[16px]">
            <LableTitleComponent text={t('pendingDepositModal.remarks')} />           
            <textarea
              value={remarks}
              onChange={(e) => setRemarks(e.target.value)}
              placeholder={t('pendingDepositModal.remarksPlaceholder')}
              className="block h-[150px] w-[100%] border-[1px] border-[var(--border-control)] rounded-[4px] px-[8px] py-[10px] focus:outline-none focus:border-[var(--button-border)]"
            />
          </div>
        </div>
        <div className="flex justify-center gap-[8px]">
          <ButtonCancelComponent
            className="w-[140px]"
            text={t('pendingDepositModal.reject')}
            onClick={handleReject}
            disabled={isSubmitting}
          />
          <ButtonApplyComponent
            className="w-[140px]"
            text={t('pendingDepositModal.approve')}
            onClick={handleApprove}
            disabled={isSubmitting}
          />
        </div>
      </div>
    </div>
  );
};

export default PendingDepositModal;
