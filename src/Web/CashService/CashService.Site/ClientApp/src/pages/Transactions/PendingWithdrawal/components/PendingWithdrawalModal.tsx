import React, { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';

import {
  displayAmount,
  formatDateTime,
  renderBankAccount,
  renderBankOptionValue,
} from '../../../../utils/commonFunctions.tsx';
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
import { TransactionTypes } from '../../../../constants/sharedConstants.ts';
import UploadImage from '../../Components/UploadImage';
import { LoadingFullComponent } from '../../../../components/LoadingComponent';

interface PendingWithdrawalModalProps {
  transId: number;
  custId: number;
  onClose: () => void;
}

const PendingWithdrawalModal: React.FC<PendingWithdrawalModalProps> = ({
  transId,
  custId,
  onClose,
}) => {
  const { t } = useTranslation('pendingWithdrawal');
  const [withdrawal, setWithdrawal] = useState<TransactionItem | null>(null);
  const [remark, setRemark] = useState('');
  const [withdrawalFrom, setWithdrawalFrom] = useState('');
  const [bankOptions, setBankOptions] = useState<DropdownOption[]>([]);
  const [image, setImage] = useState<File | null>(null);
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [isLoading, setIsLoading] = useState(true);
  const [resultMessage, setResultMessage] = useState<{
    type: 'success' | 'error' | 'warning' | null;
    message: string;
  }>({
    type: null,
    message: '',
  });

  useEffect(() => {
    const fetchWithdrawal = async () => {
      setIsLoading(true);
      const response = await transactionAPI.getTransactionDetail(transId, custId);
      if (response.error) {
        setIsLoading(false);
        return;
      }
      setWithdrawal(response.data);
      setRemark(response.data.remark);
      setIsLoading(false);
    };
    fetchWithdrawal();
  }, [transId, custId]);

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
          withdrawal?.paymentFrom &&
          bankOptions.find((option) => option.value === withdrawal.paymentFrom)
        ) {
          setWithdrawalFrom(withdrawal.paymentFrom);
        }
      } catch (error) {
        console.error('Error fetching bank options:', error);
      }
    };

    fetchBankOptions();
  }, [withdrawal]);

  const handleApprove = async () => {
    if (!isValidParams()) return;

    try {
      setIsSubmitting(true);
      const response = await approveWithdrawal({
        transId: withdrawal?.transId || 0,
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
      transId: withdrawal?.transId || 0,
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
  console.log(isSubmitting);
  return (
    <div className="bg-[#00000080] fixed inset-0 flex items-start justify-center z-50">
      <div className="px-[20px] pb-[10px] bg-[var(--modal-background)] border-[1px] border-[var(--modal-border-color)] shadow-[var(--modal-box-shadow)] mt-[100px] max-[768px]:max-w-[90%] max-[768px]:h-[90%] overflow-auto rounded-[4px]">
        <div className="relative my-[10px]">
          <MediumTitleComponent
            text={` ${t('pendingWithdrawalModal.title')} (${withdrawal?.transId})`}
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
          {isLoading ? (
            <div className="w-full flex items-center justify-center min-h-[200px]">
              <LoadingFullComponent />
            </div>
          ) : (
            <>
              <div className="flex-1 h-full max-[768px]:mb-[16px]">
                <div className="border border-[var(--border-control)] rounded mb-[8px] p-[8px]">
                  <table className="w-full h-full no-hover">
                    <thead>
                      <tr className="text-left font-semibold text-[var(--text-color-secondary)]">
                        <th className="px-[8px] py-[4px]">
                          {t('pendingWithdrawalModal.username')}
                        </th>
                        <th className="px-[8px] py-[4px]">{t('dataGrid.nickname')}</th>
                        <th className="px-[8px] py-[4px]">{t('dataGrid.firstName')}</th>
                        <th className="px-[8px] py-[4px]">
                          {t('pendingWithdrawalModal.betCredit')}
                        </th>
                      </tr>
                    </thead>
                    <tbody>
                      <tr>
                        <td className="px-[8px] py-[4px]">{withdrawal?.username}</td>
                        <td className="px-[8px] py-[4px]">{withdrawal?.nickname}</td>
                        <td className="px-[8px] py-[4px]">{`${withdrawal?.firstName} ${withdrawal?.lastName}`}</td>
                        <td className="px-[8px] py-[4px] text-[var(--text-link-color)] font-semibold">
                          {displayAmount(withdrawal?.betCredit || 0)}
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
                        <span>{renderBankAccount(withdrawal?.paymentTo || '')}</span>
                      </div>
                    </div>

                    <div className="mb-[16px]">
                      <LabelTitleComponent text={t('pendingWithdrawalModal.withdrawalAmount')} />
                      <div className="mt-1">
                        <span>
                          {displayAmount(withdrawal?.amount || 0)} {withdrawal?.currency}
                        </span>
                      </div>
                    </div>

                    <div>
                      <LabelTitleComponent text={t('pendingWithdrawalModal.from')} required />
                      <div className="w-full">
                        <DropdownComponent
                          options={bankOptions}
                          onSelect={handleChangePaymentFrom}
                          initialValue={withdrawal?.paymentFrom || ''}
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
                      <div>{formatDateTime(withdrawal?.createdTime || '')}</div>
                    </div>
                  </div>
                </div>

                <div className="mt-6 h-full">
                  <LabelTitleComponent text={t('pendingWithdrawalModal.remarks')} />
                  <textarea
                    value={remark}
                    onChange={(e) => setRemark(e.target.value)}
                    maxLength={1000}
                    placeholder={t('pendingWithdrawalModal.remarksPlaceholder')}
                    className="block h-[150px]! py-[10px] control-field"
                    disabled={isSubmitting}
                  />
                </div>
              </div>

              <div className="pl-[32px] max-[768px]:pl-[0] max-[768px]:w-full">
                <UploadImage setImage={setImage} disabled={isSubmitting} />
              </div>
            </>
          )}
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
