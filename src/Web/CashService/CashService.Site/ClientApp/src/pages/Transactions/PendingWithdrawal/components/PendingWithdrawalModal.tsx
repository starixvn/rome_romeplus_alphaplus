import React, { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';

import {
  displayAmount,
  formatDateTime,
  renderBankAccount,
  renderBankOptionValue,
} from '../../../../common/commonFunctions.tsx';
import BankIcon from '../../../../components/BankComponents.tsx';
import {
  ButtonApplyComponent,
  ButtonCancelComponent,
  ButtonClosePopupComponent,
} from '../../../../components/ButtonComponents.tsx';
import DropdownComponent, { DropdownOption } from '../../../../components/CustomDropdownComponent';
import { ResultMessageComponent } from '../../../../components/MessageComponents.tsx';
import {
  LabelTitleComponent,
  MediumTitleComponent,
} from '../../../../components/TitleComponents.tsx';
import { TransactionItem } from '../../../../services/interfaces/Transactions/TransactionHistory.ts';
import { approveWithdrawal, rejectWithdrawal } from '../../../../services/pendingWithdrawalAPI.ts';
import { transactionAPI } from '../../../../services/transactionsHistoryAPI.ts';
import { TransactionTypes } from '../../../../constants/transaction-type-constants.ts';
import UploadImage from '../../Components/UploadImage';

interface PendingWithdrawalModalProps {
  withdrawal: TransactionItem;
  onClose: () => void;
}

const PendingWithdrawalModal: React.FC<PendingWithdrawalModalProps> = ({ withdrawal, onClose }) => {
  const { t } = useTranslation('pendingWithdrawal');
  const [remark, setRemark] = useState(withdrawal.remark);
  const [withdrawalFrom, setWithdrawalFrom] = useState('');
  const [bankOptions, setBankOptions] = useState<DropdownOption[]>([]);
  const [image, setImage] = useState<File | null>(null);
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [resultMessage, setResultMessage] = useState<{
    type: 'success' | 'error' | 'warning' | null;
    message: string;
  }>({
    type: null,
    message: '',
  });

  useEffect(() => {
    const fetchBankOptions = async () => {
      try {
        const [providers, bankInfo] = await Promise.all([
          transactionAPI.getCashAgentProvider(TransactionTypes.Withdrawal),
          transactionAPI.getCashAgentBankInfo(TransactionTypes.Withdrawal),
        ]);

        const providerOptions = providers.map((provider) => ({
          value: renderBankOptionValue('', provider.providerName),
          label: provider.providerName,
          code: '',
        }));

        const bankOptions = bankInfo.map((bank) => ({
          value: renderBankOptionValue(bank.bankCode, bank.bankAccountNumber),
          label: bank.bankAccountNumber,
          code: bank.bankCode,
          icon: <BankIcon code={bank.bankCode} />,
        }));

        setBankOptions([...bankOptions, ...providerOptions]);
        if (
          withdrawal.paymentFrom &&
          bankOptions.find((option) => option.value === withdrawal.paymentFrom)
        ) {
          setWithdrawalFrom(withdrawal.paymentFrom);
        }
      } catch (error) {
        console.error('Error fetching bank options:', error);
      }
    };

    fetchBankOptions();
  }, []);

  const handleApprove = async () => {
    if (!isValidParams()) return;

    try {
      setIsSubmitting(true);
      const response = await approveWithdrawal({
        transId: withdrawal.transId,
        paymentFrom: withdrawalFrom,
        remark: remark,
        image: image,
      });
      if (response.isSuccessful) {
        setResultMessage({
          type: 'success',
          message: t('pendingWithdrawalModal.approveSuccess'),
        });
        setTimeout(() => {
          onClose();
        }, 3000);
      } else {
        setIsSubmitting(false);
        setResultMessage({
          type: 'error',
          message: t('pendingWithdrawalModal.approveError'),
        });
      }
    } catch (error) {
      console.error('Error processing withdrawal:', error);
    }
  };

  const handleReject = async () => {
    if (!isValidParams()) return;
    setIsSubmitting(true);
    const response = await rejectWithdrawal({
      transId: withdrawal.transId,
      paymentFrom: withdrawalFrom,
      remark: remark,
      image: image,
    });
    if (response.isSuccessful) {
      setResultMessage({
        type: 'success',
        message: t('pendingWithdrawalModal.rejectSuccess'),
      });
    } else {
      setIsSubmitting(false);
      setResultMessage({
        type: 'error',
        message: t('pendingWithdrawalModal.rejectError'),
      });
    }
  };

  const isValidParams = () => {
    if (!withdrawalFrom) {
      setResultMessage({
        type: 'error',
        message: t('pendingWithdrawalModal.selectBank'),
      });
      return false;
    }
    return true;
  };
  const handleChangePaymentFrom = (option: DropdownOption) => {
    setWithdrawalFrom(option.value.toString());
  };

  return (
    <div className="bg-[#00000066] fixed inset-0 flex items-start justify-center z-50 max-[768px]:items-start">
      <div className="p-[30px] w-full bg-[#ffffff] mt-[50px] max-w-[900px] max-[768px]:max-w-[90%] max-[768px]:h-[90%] overflow-auto rounded-[4px] shadow-[0_2px_4px_-1px_#00000033,0_4px_5px_#00000024,0_1px_10px_#0000001f] max-[768px]:mt-[16px]">
        <div className="relative mb-[16px]">
          <MediumTitleComponent
            text={` ${t('pendingWithdrawalModal.title')} (${withdrawal.transId})`}
          />
          <ButtonClosePopupComponent onClick={onClose} />
        </div>
        {resultMessage.type && (
          <ResultMessageComponent
            type={resultMessage.type}
            message={resultMessage.message}
            className="mb-[16px] max-[768px]:mb-[0]"
          />
        )}
        <div className="flex max-[768px]:flex-wrap">
          <div className="flex-1 h-full max-[768px]:mb-[16px]">
            <div className="border border-[#333333] rounded mb-[8px] p-[8px]">
              <table className="w-full h-full">
                <thead>
                  <tr className="text-left text-[#333333] font-semibold">
                    <th className="px-[8px] py-[4px]">{t('pendingWithdrawalModal.username')}</th>
                    <th className="px-[8px] py-[4px]">{t('dataGrid.nickname')}</th>
                    <th className="px-[8px] py-[4px]">{t('dataGrid.firstName')}</th>
                    <th className="px-[8px] py-[4px]">{t('pendingWithdrawalModal.betCredit')}</th>
                  </tr>
                </thead>
                <tbody>
                  <tr>
                    <td className="px-[8px] py-[4px]">{withdrawal.username}</td>
                    <td className="px-[8px] py-[4px]">{withdrawal.nickname}</td>
                    <td className="px-[8px] py-[4px]">{`${withdrawal.firstName} ${withdrawal.lastName}`}</td>
                    <td className="px-[8px] py-[4px] text-[var(--text-color-primary)] font-semibold">
                      {displayAmount(withdrawal.betCredit)}
                    </td>
                  </tr>
                </tbody>
              </table>
            </div>

            <div className="grid grid-cols-2 gap-6">
              <div>
                <div className="mb-[16px]">
                  <LabelTitleComponent text={t('pendingWithdrawalModal.to')} />
                  <div className="flex items-center">
                    <span>{renderBankAccount(withdrawal.paymentTo)}</span>
                  </div>
                </div>

                <div className="mb-[16px]">
                  <LabelTitleComponent text={t('pendingWithdrawalModal.withdrawalAmount')} />
                  <div className="mt-1">
                    <span>
                      {displayAmount(withdrawal.amount)} {withdrawal.currency}
                    </span>
                  </div>
                </div>

                <div>
                  <LabelTitleComponent text={t('pendingWithdrawalModal.from')} required />
                  <div className="w-full">
                    <DropdownComponent
                      options={bankOptions}
                      onSelect={handleChangePaymentFrom}
                      initialValue={withdrawal.paymentFrom}
                      placeholder={t('pendingWithdrawalModal.selectBank')}
                      disable={isSubmitting}
                      searchable={true}
                    />
                  </div>
                </div>
              </div>

              <div>
                <div className="mb-[16px]">
                  <LabelTitleComponent text={t('pendingWithdrawalModal.dateAndTime')} />
                  <div>{formatDateTime(withdrawal.createdTime)}</div>
                </div>
              </div>
            </div>

            <div className="mt-6 h-full">
              <LabelTitleComponent text={t('pendingWithdrawalModal.remarks')} />
              <textarea
                value={withdrawal.remark}
                onChange={(e) => setRemark(e.target.value)}
                placeholder={t('pendingWithdrawalModal.remarksPlaceholder')}
                className="block h-[150px] w-[100%] border-[1px] border-[var(--border-control)] rounded-[4px] px-[8px] py-[10px] focus:outline-none focus:border-[var(--button-border)]"
                disabled={isSubmitting}
              />
            </div>
          </div>

          <div className="pl-[32px] max-[768px]:pl-[0] max-[768px]:w-full">
            <UploadImage setImage={setImage} disabled={isSubmitting} />
          </div>
        </div>

        <div className="flex justify-center gap-[8px]">
          <ButtonCancelComponent
            text={t('pendingWithdrawalModal.reject')}
            onClick={handleReject}
            disabled={isSubmitting}
          />
          <ButtonApplyComponent
            text={t('pendingWithdrawalModal.approve')}
            onClick={handleApprove}
            disabled={isSubmitting}
          />
        </div>
      </div>
    </div>
  );
};

export default PendingWithdrawalModal;
