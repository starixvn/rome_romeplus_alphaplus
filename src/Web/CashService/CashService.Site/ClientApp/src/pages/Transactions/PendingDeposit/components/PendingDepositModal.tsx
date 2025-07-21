import React, { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import {
  displayAmount,
  formatDateTime,
  renderBankAccount,
} from '../../../../utils/commonFunctions.tsx';
import {
  ButtonClosePopupComponent,
  ButtonComponent,
} from '../../../../components/Buttons/index';
import { MessageComponent } from '../../../../components/Message';
import { TitleComponent } from '../../../../components/Title';
import { ApproveRejectDepositModel } from '../../../../services/interfaces/ApproveRejectDepositModel.ts';
import { TransactionItem } from '../../../../services/interfaces/Transactions/TransactionHistory';
import { transactionAPI } from '../../../../services/transactionsHistoryAPI';
import { LoadingFullComponent } from '../../../../components/LoadingComponent';
import pendingDepositAPI from '../../../../services/pendingDepositAPI';
import ConfirmPopup from '../../../../components/ConfirmPopup';

interface PendingDepositModalProps {
  transId: number;
  custId: number;
  onClose: (isSuccess: boolean) => void;
  isCashServiceEnabled: boolean;
}
const PendingDepositModal: React.FC<PendingDepositModalProps> = ({
  transId,
  custId,
  onClose,
  isCashServiceEnabled,
}) => {
  const { t } = useTranslation('pendingDeposit');
  const { t: tGlobal } = useTranslation('global');
  const [remark, setRemark] = useState('');
  const [deposit, setDeposit] = useState<TransactionItem | null>(null);
  const [depositAmount, setDepositAmount] = useState<number>(deposit?.amount || 0);
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [isLoading, setIsLoading] = useState(true);
  const [resultMessage, setResultMessage] = useState<{
    type: 'success' | 'error';
    message: string;
  } | null>(null);
  const [showConfirm, setShowConfirm] = useState<null | 'approve' | 'reject'>(null);

  useEffect(() => {
    const fetchDeposit = async () => {
      setIsLoading(true);
      const response = await transactionAPI.getTransactionDetail(transId, custId);
      if (response.error) {
        setIsLoading(false);
        return;
      }
      setDeposit(response.data);
      setDepositAmount(response.data.amount);
      setRemark(response.data.remark || '');
      setIsLoading(false);
    };

    fetchDeposit();
  }, [transId, custId]);

  const getParams = () => {
    const params: ApproveRejectDepositModel = {
      transId: deposit?.transId ? deposit.transId : 0,
      custId: deposit?.custId ? deposit.custId : 0,
      newAmount: depositAmount,
      remark: remark,
    };
    return params;
  };

  const handleApprove = async () => {
    setShowConfirm('approve');
  };

  const handleReject = async () => {
    setShowConfirm('reject');
  };

  const doApprove = async () => {
    const params = getParams();
    setIsSubmitting(true);
    try {
      const response = await pendingDepositAPI.approveDeposit(params);
      if (response.isSuccessful) {
        setResultMessage({ type: 'success', message: t('successMessages.depositApproved') });
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
      setIsSubmitting(false);
      setResultMessage({
        type: 'error',
        message: t('errorMessages.failedToApprove'),
      });
      console.error('Error processing deposit:', error);
    }
  };

  const doReject = async () => {
    const params = getParams();
    setIsSubmitting(true);
    try {
      const response = await pendingDepositAPI.rejectDeposit(params);
      if (response.isSuccessful) {
        setResultMessage({ type: 'success', message: t('successMessages.depositRejected') });
        setTimeout(() => {
          onClose(true);
        }, 3000);
      } else {
        setIsSubmitting(false);
        setResultMessage({
          type: 'error',
          message: response.message || t('errorMessages.failedToReject'),
        });
      }
    } catch (error) {
      setIsSubmitting(false);
      setResultMessage({
        type: 'error',
        message: t('errorMessages.failedToReject'),
      });
      console.error('Error processing deposit:', error);
    }
  };

  const classBase64 = deposit?.base64Payslip ? 'max-w-[900px]' : 'max-w-[540px]';

  return (
    <>
      {showConfirm === 'approve' && (
        <ConfirmPopup
          title={t('pendingDepositModal.confirmApproveTitle')}
          message={t('pendingDepositModal.confirmApproveMessage')}
          onCancel={() => setShowConfirm(null)}
          onConfirm={() => {
            setShowConfirm(null);
            doApprove();
          }}
          cancelText={tGlobal('cancel')}
          confirmText={tGlobal('confirm')}
        />
      )}
      {showConfirm === 'reject' && (
        <ConfirmPopup
          title={t('pendingDepositModal.confirmRejectTitle')}
          message={t('pendingDepositModal.confirmRejectMessage')}
          onCancel={() => setShowConfirm(null)}
          onConfirm={() => {
            setShowConfirm(null);
            doReject();
          }}
          cancelText={tGlobal('cancel')}
          confirmText={tGlobal('confirm')}
        />
      )}
      <div className="bg-[#00000080] fixed inset-0 flex items-center justify-center z-50">
        <div
          className={` ${classBase64} p-[12px] pt-[0] bg-[var(--modal-background)] border-[1px] border-[var(--modal-border-color)] shadow-[var(--modal-box-shadow)] max-[768px]:max-w-[90%] max-h-[90%] overflow-auto rounded-[4px]`}>
          <div className="relative mb-[10px] border-b-[1px] border-[var(--border-color)] py-[11px]">
            <TitleComponent type='medium'
              text={` ${t('pendingDepositModal.title')} (${deposit?.transId})`}
            />
            <ButtonClosePopupComponent onClick={() => onClose(false)} />
          </div>
          {resultMessage && (
            <MessageComponent
              type={resultMessage.type}
              message={resultMessage.message}
              className="mb-[16px] max-[768px]:mb-[0]"
            />
          )}
          <div className="flex max-[768px]:flex-wrap">
            {isLoading ? (
              <div className="w-full flex items-center justify-center min-h-[200px]">
                <LoadingFullComponent />
              </div>
            ) : (
              <>
                <div className="flex-1 h-full">
                  <div className="border border-[var(--border-control)] rounded-[4px] mb-[8px] p-[8px] flex flex-wrap ">
                    <div className="w-[50%] mb-[8px] flex flex-col">
                      <label className="font-semibold">{t('pendingDepositModal.username')}:</label>
                      <span>{deposit?.username}</span>
                    </div>
                    <div className="w-[50%] mb-[8px] flex flex-col">
                      <label className="font-semibold">{t('dataGrid.nickname')}:</label>
                      <span>{deposit?.nickname}</span>
                    </div>
                    <div className="w-[50%] flex flex-col">
                      <label className="font-semibold">{t('dataGrid.firstLastName')}:</label>
                      <span>{`${deposit?.firstName} ${deposit?.lastName}`}</span>
                    </div>
                    <div className="w-[50%] flex flex-col">
                      <label className="font-semibold">{t('pendingDepositModal.betCredit')}:</label>
                      <span className="text-[var(--text-link-color)] font-semibold">
                        {deposit?.betCredit && displayAmount(deposit.betCredit)}
                      </span>
                    </div>
                  </div>

                  <div className="grid grid-cols-2 gap-6">
                    <div className="w-[230px] max-[768px]:w-[48%]">
                      <div className="mb-[10px]">
                        <TitleComponent type='label' text={t('pendingDepositModal.from')} />
                        <div className="flex items-center">
                          {renderBankAccount(deposit?.paymentFrom || '', tGlobal)}
                        </div>
                      </div>
                      <div>
                        <TitleComponent type='label' text={t('pendingDepositModal.dateAndTime')} />
                        <div>{formatDateTime(deposit?.createdTime || '')}</div>
                      </div>
                    </div>

                    <div>
                      <div className="mb-[10px]">
                        <TitleComponent type='label' text={t('pendingDepositModal.to')} />
                        {deposit?.paymentTo && renderBankAccount(deposit.paymentTo, tGlobal)}
                      </div>

                      <div>
                        <TitleComponent type='label' text={t('pendingDepositModal.depositAmount')} />
                        <div className="relative">
                          <span className="py-[4px] text-[var(--text-link-color)] font-semibold">
                            {deposit?.amount && displayAmount(deposit.amount)}
                          </span>
                        </div>
                      </div>
                    </div>
                  </div>
                  {deposit?.paymentType === 2 && (
                    <div className="mb-[16px] mt-[10px]">
                      <TitleComponent type='label' text={t('pendingDepositModal.remarks')} />
                      <textarea
                        value={remark}
                        onChange={(e) => setRemark(e.target.value)}
                        maxLength={1000}
                        placeholder={t('pendingDepositModal.remarksPlaceholder')}
                        className="block h-[230px]! py-[10px] control-field max-[768px]:h-[100px]!"
                      />
                    </div>
                  )}
                </div>
                {deposit?.base64Payslip && (
                  <div className="pl-[32px] max-[768px]:pl-[0] max-[768px]:w-full">
                    <div className="w-[350px] h-[450px] max-[768px]:w-[250px] max-[768px]:h-[250px] rounded-[5px] mb-6">
                      <img
                        src={deposit?.base64Payslip}
                        alt={deposit?.transId.toString()}
                        className="w-full h-full object-contain"
                      />
                    </div>
                  </div>
                )}
              </>
            )}
          </div>

          {deposit?.paymentType === 2 && (
            <div className="flex gap-4 justify-end">
              <ButtonComponent
                variant="reject"
                onClick={handleReject}
                disabled={isSubmitting || isLoading || !isCashServiceEnabled}
                text={t('pendingDepositModal.reject')}
                title={isCashServiceEnabled ? '' : tGlobal('cashServiceDisabled')}
              />
              <ButtonComponent
                variant="apply"
                onClick={handleApprove}
                disabled={isSubmitting || isLoading || !isCashServiceEnabled}
                text={t('pendingDepositModal.approve')}
                title={isCashServiceEnabled ? '' : tGlobal('cashServiceDisabled')}
              />
            </div>
          )}
          {deposit?.paymentType === 1 && (
            <div className="flex gap-4 justify-end">
              <ButtonComponent variant="cancel" onClick={() => onClose(false)} text={tGlobal('close')} />
            </div>
          )}
        </div>
      </div>
    </>
  );
};

export default PendingDepositModal;
