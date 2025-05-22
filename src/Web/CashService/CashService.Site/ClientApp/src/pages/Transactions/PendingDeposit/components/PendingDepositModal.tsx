import React, { useState } from 'react';
import { useTranslation } from 'react-i18next';
import { TransactionItem } from '../../../../services/interfaces/Transactions/TransactionHistory';
import { IconClose } from '../../../../components/Icons';
import { ButtonApplyComponent, ButtonCancelComponent } from '../../../../components/ButtonComponents';
import { renderBankAccount, displayAmount, formatDateTime } from '../../../../common/commonFunctions';
import pendingDepositAPI from '../../../../services/pendingDepositAPI';
import { ApproveRejectDepositModel } from '../../../../services/interfaces/ApproveRejectDepositModel.ts';
import { ResultMessageComponent } from '../../../../components/MessageComponents.tsx';

interface PendingDepositModalProps {
  deposit: TransactionItem | null;
  onClose: (isSuccess: boolean) => void;
}
const PendingDepositModal: React.FC<PendingDepositModalProps> = ({
  deposit,
  onClose
}) => {
  const { t } = useTranslation('pendingDeposit');
  const [remarks, setRemarks] = useState('');
  const [depositAmount, setDepositAmount] = useState(deposit?.amount.toString());
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [resultMessage, setResultMessage] = useState<{ type: 'success' | 'error'; message: string } | null>(null);

  const getParams = () => {
    const params: ApproveRejectDepositModel = {
      transId: deposit?.transId ? deposit.transId : 0,
      amount: depositAmount === '' ? 0 : Number(depositAmount),
      remark: remarks,
    }
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
          message: t('successMessages.depositApproved')
        });
        setTimeout(() => {
          onClose(true);
        }, 3000);
      } else {
        setIsSubmitting(false);
        setResultMessage({
          type: 'error',
          message: response.message || t('errorMessages.failedToApprove')
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
          message: t('successMessages.depositRejected')
        });
        setTimeout(() => {
          onClose(true);
        }, 3000);
      } else {
        setIsSubmitting(false);
        setResultMessage({
          type: 'error',
          message: t('errorMessages.failedToReject')
        });
      }
    } catch (error) {
      console.error('Error processing deposit:', error);
    }
  };  

  const handleSetDepositAmount = (e: React.ChangeEvent<HTMLInputElement>) => {
    if (e.target.validity.valid) {
      setDepositAmount(e.target.value);
    }
  };

  return (
    <div className="bg-[#00000066] fixed z-[100] top-0 left-0 right-0 bottom-0 flex items-start justify-center">
      <div className="p-[30px] bg-[#ffffff] mt-[100px] max-w-[540px] max-[768px]:max-w-[90%] rounded-[4px] shadow-[0_2px_4px_-1px_#00000033,0_4px_5px_#00000024,0_1px_10px_#0000001f]">
        <div className="relative mb-[16px]">
          <h5 className="text-[16px] text-[var(--text-color-primary)] font-semibold leading-[21.6px] max-[768px]:text-[20px] max-[768px]:leading-[26px]">
            {t('pendingDepositModal.title')} ({deposit?.transId})
          </h5>
          <button
              type="button"
              className="absolute top-[-20px] right-[-20px] cursor-pointer"
              onClick={() => onClose(false)}
            >
              <IconClose />
            </button>
        </div>
        {resultMessage && (
          <ResultMessageComponent
            type={resultMessage.type}
            message={resultMessage.message}
            className="mb-4"
          />
        )}
        <div className="modal-body">
          <div className="border border-[#333333] rounded mb-[8px] p-[8px]">
            <table className="w-full">
              <thead>
                <tr className="text-left text-[#333333]">
                  <th className="font-bold px-[8px] py-[4px]">{t('pendingDepositModal.username')}</th>
                  <th className="font-bold px-[8px] py-[4px]">{t('pendingDepositModal.firstLastName')}</th>
                  <th className="font-bold px-[8px] py-[4px]">{t('pendingDepositModal.betCredit')}</th>
                </tr>
              </thead>
              <tbody>
                <tr>
                  <td className="px-[8px] py-[4px]">{deposit?.username}</td>
                  <td className="px-[8px] py-[4px]">{`${deposit?.firstName} ${deposit?.lastName}`}</td>
                  <td className="px-[8px] py-[4px] text-[var(--text-color-primary)] font-semibold">{deposit?.amount && displayAmount(deposit.amount)}</td>
                </tr>
              </tbody>
            </table>
          </div>

          <div className="flex flex-nowrap justify-between gap-[16px] max-[768px]:justify-between max-[768px]:gap-[0] max-[768px]:flex-wrap w-full">
            <div className="w-[230px] max-[768px]:w-[48%]">
              <div className="mb-[8px]">
                <div className="block font-bold text-[#333333] my-[8px]">{t('pendingDepositModal.from')}</div>
                <div className="flex items-center">                   
                  {deposit?.paymentFrom && renderBankAccount(deposit.paymentFrom)}
                </div>
              </div>

              <div>
                <div className="block font-bold text-[#333333] my-[8px]">{t('pendingDepositModal.dateAndTime')}</div>
                <div>{formatDateTime(deposit?.createdTime || '')}</div>
              </div>
            </div>

            <div className="w-[230px] max-[768px]:w-[48%]">
              <div className="mb-[8px]">
                <div className="block font-bold text-[#333333] my-[8px]">{t('pendingDepositModal.to')}</div>                 
                {deposit?.paymentTo && renderBankAccount(deposit.paymentTo)}
              </div>

              <div>
                <div className="block font-bold text-[#333333] my-[8px]">{t('pendingDepositModal.depositAmount')}</div>
                <div className="relative">
                  <input
                    type="text"
                    value={depositAmount}
                    pattern="[0-9]*"
                    placeholder="0"
                    onChange={(e) => handleSetDepositAmount(e)}
                    className="h-[32px] max-[768px]:h-[40px] w-[100%] border-[1px] rounded-[4px] px-[8px]  focus:outline-none border-[var(--border-control)] focus:border-[var(--button-border)]"
                  />
                </div>
              </div>
            </div>
          </div>

          <div className="mb-[16px]">
            <div className="block font-bold text-[#333333] my-[8px]">{t('pendingDepositModal.remarks')}</div>
            <textarea
              value={remarks}
              onChange={(e) => setRemarks(e.target.value)}
              placeholder={t('pendingDepositModal.remarksPlaceholder')}
              className="block h-[150px] max-[768px]:h-[40px] w-[100%] border-[1px] border-[var(--border-control)] rounded-[4px] px-[8px] py-[10px] focus:outline-none focus:border-[var(--button-border)]"
            />
          </div>
        </div>

        <div className="flex justify-center gap-[8px]">
          <ButtonComponent className="w-[140px]" type="cancel" style="rounded" text={t('pendingDepositModal.reject')} onClick={handleReject} disabled={isSubmitting} />
          <ButtonComponent className="w-[140px]" type="apply" style="rounded" text={t('pendingDepositModal.approve')} onClick={handleApprove} disabled={isSubmitting} />
        </div>
      </div>
    </div>
  );
};

export default PendingDepositModal;
