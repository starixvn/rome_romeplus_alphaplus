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
  ButtonClosePopupComponent,
  ButtonRejectComponent,
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
  const { t: tGlobal } = useTranslation('global');
  const [withdrawal, setWithdrawal] = useState<TransactionItem | null>(null);
  const [remark, setRemark] = useState('');
  const [withdrawalFrom, setWithdrawalFrom] = useState('');
  const [bankOptions, setBankOptions] = useState<DropdownOption[]>([]);
  const [image, setImage] = useState<File | null>(null);
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [isLoading, setIsLoading] = useState(true);
  const [providerId, setProviderId] = useState(0);
  const [paymentMethodId, setPaymentMethodId] = useState(0);
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

        const bankOptions: DropdownOption[] = bankInfo.map((bank) => ({
          value: renderBankOptionValue(bank.bankCode, bank.bankAccountNumber),
          label: bank.bankAccountNumber,
          icon: <BankIcon code={bank.bankCode} />,
          attributes: {
            providerId: 0,
            paymentMethodId: 0,
          },
        }));
        const mappedProviderInfo: DropdownOption[] = providers.map((provider) => ({
          value: renderBankOptionValue('', provider.providerName),
          label: provider.providerName,
          attributes: {
            providerId: provider.providerId,
            paymentMethodId: provider.paymentMethodId,
          },
        }));

        setBankOptions([...bankOptions, ...mappedProviderInfo]);

        if (
          withdrawal?.paymentFrom &&
          bankOptions.find((option) => option.value === withdrawal.paymentFrom)
        ) {
          let providerId =
            bankOptions.find((option) => option.value === withdrawal.paymentFrom)?.attributes
              ?.providerId || 0;
          let paymentMethodId =
            bankOptions.find((option) => option.value === withdrawal.paymentFrom)?.attributes
              ?.paymentMethodId || 0;
          setProviderId(providerId);
          setPaymentMethodId(paymentMethodId);
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
        custId: withdrawal?.custId || 0,
        paymentFrom: withdrawalFrom,
        providerId: providerId,
        paymentMethodId: paymentMethodId,
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
          message: response.message,
        });
      }
    } catch (error) {
      console.error('Error processing withdrawal:', error);
    }
  };

  const handleReject = async () => {
    if (!isValidParams(true)) return;

    setIsSubmitting(true);
    const response = await rejectWithdrawal({
      transId: withdrawal?.transId || 0,
      custId: withdrawal?.custId || 0,
      paymentFrom: withdrawalFrom,
      providerId: providerId,
      paymentMethodId: paymentMethodId,
      remark: remark,
      image: image,
    });
    if (response.isSuccessful) {
      setResultMessage({
        type: 'success',
        message: t('pendingWithdrawalModal.rejectSuccess'),
      });
      setTimeout(() => {
        onClose();
      }, 3000);
    } else {
      setIsSubmitting(false);
      setResultMessage({
        type: 'error',
        message: response.message,
      });
    }
  };

  const isValidParams = (isReject: boolean = false) => {
    if (!withdrawalFrom && !isReject) {
      setResultMessage({
        type: 'error',
        message: t('pendingWithdrawalModal.bankRequired'),
      });
      return false;
    }

    if (image && image.size > 1024 * 1024) {
      setResultMessage({ type: 'error', message: t('manualWithdrawalForm.imageSizeError') });
      return false;
    }

    const allowedTypes = ['image/jpeg', 'image/jpg', 'image/png'];
    if (image && !allowedTypes.includes(image.type)) {
      setResultMessage({
        type: 'error',
        message: t('manualWithdrawalForm.invalidFileType'),
      });
      return false;
    }

    return true;
  };

  const handleChangePaymentFrom = (option: DropdownOption) => {
    let providerId = option.attributes?.providerId || 0;
    let paymentMethodId = option.attributes?.paymentMethodId || 0;
    setProviderId(providerId);
    setPaymentMethodId(paymentMethodId);
    setWithdrawalFrom(option.value.toString());
  };

  return (
    <div className="bg-[#00000080] fixed inset-0 flex items-start justify-center z-50 w-full">
      <div className="p-[12px] pt-[0] bg-[var(--modal-background)] border-[1px] border-[var(--modal-border-color)] shadow-[var(--modal-box-shadow)] mt-[50px] w-[900px] max-[768px]:max-w-[90%] max-[768px]:w-full max-[768px]:h-[90%] overflow-auto rounded-[4px]">
        <div className="relative mb-[10px] border-b-[1px] border-[var(--border-color)] py-[11px]">
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
        <div className="flex max-[768px]:flex-wrap mb-[16px] max-[768px]:flex-col">
          {isLoading ? (
            <div className="w-full flex items-center justify-center min-h-[200px]">
              <LoadingFullComponent />
            </div>
          ) : (
            <>
              <div className="flex-1 h-full max-[768px]:mb-[16px] w-[400px] max-[768px]:w-full mr-[32px]">
                <div className="border border-[var(--border-control)] rounded-[4px] mb-[8px] p-[8px] flex flex-wrap">
                  <div className="w-[50%] mb-[8px] flex flex-col">
                    <label className="font-semibold text-[var(--text-color-secondary)]">
                      {t('pendingWithdrawalModal.username')}:
                    </label>
                    <span>{withdrawal?.username}</span>
                  </div>
                  <div className="w-[50%] mb-[8px] flex flex-col">
                    <label className="font-semibold  text-[var(--text-color-secondary)]">
                      {t('dataGrid.nickname')}:
                    </label>
                    <span>{withdrawal?.nickname}</span>
                  </div>
                  <div className="w-[50%] flex flex-col">
                    <label className="font-semibold text-[var(--text-color-secondary)]">
                      {t('dataGrid.firstName')}:
                    </label>
                    <span>{`${withdrawal?.firstName} ${withdrawal?.lastName}`}</span>
                  </div>
                  <div className="w-[50%] flex flex-col">
                    <label className="font-semibold text-[var(--text-color-secondary)]">
                      {t('pendingWithdrawalModal.betCredit')}:
                    </label>
                    <span className="text-[var(--text-link-color)] font-semibold">
                      {displayAmount(withdrawal?.betCredit || 0)}
                    </span>
                  </div>
                </div>

                <div className="grid grid-cols-2 gap-6">
                  <div>
                    <div className="mb-[10px]">
                      <LabelTitleComponent text={t('pendingWithdrawalModal.to')} />
                      <div className="flex items-center">
                        <span>{renderBankAccount(withdrawal?.paymentTo || '', tGlobal)}</span>
                      </div>
                    </div>

                    <div className="mb-[10px]">
                      <LabelTitleComponent text={t('pendingWithdrawalModal.withdrawalAmount')} />
                      <div className="mt-1">
                        <span className="py-[4px] text-[var(--text-link-color)] font-semibold">
                          {displayAmount(withdrawal?.amount || 0)}
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
                    className="block h-[140px]! py-[10px] control-field resize-none"
                    disabled={isSubmitting}
                  />
                </div>
              </div>

              <UploadImage
                setImage={setImage}
                prevImage={withdrawal?.base64Payslip}
                disabled={isSubmitting}
              />
            </>
          )}
        </div>

        <div className="flex justify-center gap-[8px]">
          <ButtonRejectComponent
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
