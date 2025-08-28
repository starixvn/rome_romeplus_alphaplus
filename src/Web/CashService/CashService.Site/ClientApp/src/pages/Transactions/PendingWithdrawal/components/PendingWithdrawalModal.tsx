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
  ButtonCancelComponent,
  ButtonRejectComponent,
  ButtonApplyComponent,
} from '../../../../components/Buttons/index';
import DropdownComponent, { DropdownOption } from '../../../../components/CustomDropdownComponent';
import { MessageComponent } from '../../../../components/Message';
import { TitleComponent } from '../../../../components/Title';
import { TransactionItem } from '../../../../services/interfaces/Transactions/TransactionHistory.ts';
import {
  approveWithdrawal,
  getSettingLimitAmtWithdrawal,
  rejectWithdrawal,
} from '../../../../services/pendingWithdrawalAPI.ts';
import { transactionAPI } from '../../../../services/transactionsHistoryAPI.ts';
import { TransactionTypes } from '../../../../constants/sharedConstants.ts';
import UploadImage from '../../../../components/UploadImage/UploadImage.tsx';
import { LoadingFullComponent } from '../../../../components/LoadingComponent';
import ConfirmPopup from '../../../../components/ConfirmPopup';
import { ResultModel } from '../../../../services/interfaces/ResultModel.ts';
import { ButtonIconComponent } from '../../../../components/Buttons/ButtonIconComponent.tsx';
import { baseAPI } from '../../../../services/commonAPI.ts';
import { Bank } from '../../../../services/interfaces/Bank.ts';

interface PendingWithdrawalModalProps {
  transId: string;
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
  const [isRemovePayslip, setIsRemovePayslip] = useState(false);
  const [isProviderProcessing, setIsProviderProcessing] = useState(false);
  const [minAmount, setMinAmount] = useState(0);
  const [maxAmount, setMaxAmount] = useState<number | null>(null);
  const [resultMessage, setResultMessage] = useState<{
    type: 'success' | 'error' | 'warning' | null;
    message: string;
  }>({
    type: null,
    message: '',
  });
  const [showConfirm, setShowConfirm] = useState<null | 'approve' | 'reject'>(null);
  const [providerSupportedBank, setProviderSupportedBank] = useState<string[]>([]);

  useEffect(() => {
    const fetchWithdrawal = async () => {
      setIsLoading(true);
      const response = await transactionAPI.getTransactionDetail(transId, custId);
      if (response.error) {
        setIsLoading(false);
        return;
      }
      if (response.data.paymentType === 1) {
        setIsProviderProcessing(true);
        setResultMessage({
          type: 'warning',
          message: t('pendingWithdrawalModal.providerProcessing'),
        });
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

        const bankInfoOptions: DropdownOption[] = bankInfo.map((bank) => ({
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

        setBankOptions([...bankInfoOptions, ...mappedProviderInfo]);

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

  const handleImageChange = (file: File | null) => {
    const isRemovePayslip = withdrawal?.base64Payslip !== null && file === null;
    setImage(file);
    setIsRemovePayslip(isRemovePayslip);
  };

  const handleApprove = async () => {
    if (!isValidParams()) return;

    setShowConfirm('approve');
  };

  const handleReject = async () => {
    if (!isValidParams(true)) return;

    setShowConfirm('reject');
  };

  const doApprove = async () => {
    try {
      setIsSubmitting(true);
      const response = await approveWithdrawal({
        transId: withdrawal?.transId || '',
        custId: withdrawal?.custId || 0,
        paymentFrom: withdrawalFrom,
        providerId: providerId,
        paymentMethodId: paymentMethodId,
        remark: remark || '',
        image: image,
        isRemovePayslip: isRemovePayslip,
      });
      if (response.isSuccessful) {
        setResultMessage({ type: 'success', message: t('pendingWithdrawalModal.approveSuccess') });
        setTimeout(() => {
          onClose();
        }, 3000);
      } else {
        handleApproveFailed(response);
      }
    } catch (error) {
      setIsSubmitting(false);
      setResultMessage({ type: 'error', message: t('pendingWithdrawalModal.approveFailed') });
      console.error('Error processing withdrawal:', error);
    }
  };

  const handleApproveFailed = (response: ResultModel) => {
    if (response.code === 410 || response.code === 411) {
      setResultMessage({ type: 'warning', message: response.message });
      setTimeout(() => {
        onClose();
      }, 3000);
    } else {
      setIsSubmitting(false);
      setResultMessage({ type: 'error', message: response.message });
    }
  };

  const doReject = async () => {
    setIsSubmitting(true);
    const response = await rejectWithdrawal({
      transId: withdrawal?.transId || '',
      custId: withdrawal?.custId || 0,
      paymentFrom: withdrawalFrom,
      providerId: providerId,
      paymentMethodId: paymentMethodId,
      remark: remark || '',
      image: image,
      isRemovePayslip: isRemovePayslip,
    });
    if (response.isSuccessful) {
      setResultMessage({ type: 'success', message: t('pendingWithdrawalModal.rejectSuccess') });
      setTimeout(() => {
        onClose();
      }, 3000);
    } else {
      setIsSubmitting(false);
      setResultMessage({ type: 'error', message: response.message });
    }
  };

  // Validation functions
  const validateBankSelection = (isReject: boolean): boolean => {
    if (!withdrawalFrom && !isReject) {
      setResultMessage({
        type: 'error',
        message: t('pendingWithdrawalModal.bankRequired'),
      });
      return false;
    }
    return true;
  };

  const validateProviderSupportedBank = (isReject: boolean): boolean => {
    if (
      providerSupportedBank.length > 0 &&
      !providerSupportedBank.includes(withdrawal?.paymentTo.split('-')[0] || '') &&
      !isReject
    ) {
      setResultMessage({
        type: 'error',
        message: t('manualWithdrawalForm.providerSupportedBankError').replace(
          '{providerName}',
          withdrawalFrom.split('-')[1]
        ),
      });
      return false;
    }
    return true;
  };

  const validateImageSize = (): boolean => {
    if (image && image.size > 1024 * 1024) {
      setResultMessage({ type: 'error', message: t('manualWithdrawalForm.imageSizeError') });
      return false;
    }
    return true;
  };

  const validateImageType = (): boolean => {
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

  const isValidParams = (isReject: boolean = false) => {
    if (!validateBankSelection(isReject)) {
      return false;
    }

    if (!validateProviderSupportedBank(isReject)) {
      return false;
    }

    if (!validateImageSize()) {
      return false;
    }

    if (!validateImageType()) {
      return false;
    }

    return true;
  };

  const handleChangePaymentFrom = async (option: DropdownOption) => {
    let providerId = option.attributes?.providerId || 0;
    let paymentMethodId = option.attributes?.paymentMethodId || 0;
    setProviderId(providerId);
    setPaymentMethodId(paymentMethodId);
    setWithdrawalFrom(option.value.toString());
    if (providerId > 0) {
      const limits = await getSettingLimitAmtWithdrawal({
        custId: withdrawal?.custId || 0,
        providerId,
        paymentMethodId,
      });
      setMinAmount(limits.minAmt);
      setMaxAmount(limits.maxAmt);
      const response = await baseAPI.getProviderBanks(providerId);
      if (response.error) {
        setResultMessage({
          type: 'error',
          message: response.error.message,
        });
      } else {
        const supportedBankCodes = response.data.map((bank: Bank) => bank.code);
        setProviderSupportedBank(supportedBankCodes);
      }
    }
  };

  const handleProviderSupportedBank = () => {
    if (
      providerSupportedBank.length > 0 &&
      !providerSupportedBank.includes(withdrawal?.paymentTo.split('-')[0] || '')
    ) {
      setResultMessage({
        type: 'error',
        message: t('manualWithdrawalForm.providerSupportedBankError').replace(
          '{providerName}',
          withdrawalFrom.split('-')[1]
        ),
      });
    }
  };

  useEffect(() => {
    if (providerSupportedBank.length > 0) {
      handleProviderSupportedBank();
    }
  }, [providerSupportedBank, withdrawalFrom]);

  const renderMinMaxAmount = () => {
    if (providerId <= 0 || isProviderProcessing) {
      return null;
    }
    const displayMaxAmount = maxAmount ? displayAmount(maxAmount) : 'unlimited';
    return (
      <span className="text-xs text-gray-500">
        ({t('manualWithdrawalForm.minAmount')}: {displayAmount(minAmount)} -{' '}
        {t('manualWithdrawalForm.maxAmount')}: {displayMaxAmount})
      </span>
    );
  };

  return (
    <>
      {showConfirm === 'approve' && (
        <ConfirmPopup
          title={t('pendingWithdrawalModal.confirmApproveTitle')}
          message={t('pendingWithdrawalModal.confirmApproveMessage')}
          onCancel={() => setShowConfirm(null)}
          onConfirm={() => {
            setShowConfirm(null);
            doApprove();
          }}
        />
      )}
      {showConfirm === 'reject' && (
        <ConfirmPopup
          title={t('pendingWithdrawalModal.confirmRejectTitle')}
          message={t('pendingWithdrawalModal.confirmRejectMessage')}
          onCancel={() => setShowConfirm(null)}
          onConfirm={() => {
            setShowConfirm(null);
            doReject();
          }}
        />
      )}
      <div className="bg-[#00000080] fixed inset-0 flex justify-center z-50 items-center">
        <div className="max-w-[900px] p-[12px] pt-[0] bg-[var(--modal-background)] border-[1px] border-[var(--modal-border-color)] shadow-[var(--modal-box-shadow)] max-[768px]:max-w-[90%] max-h-[90%] overflow-auto rounded-[4px]">
          <div className="relative mb-[10px] border-b-[1px] border-[var(--border-color)] py-[11px]">
            <TitleComponent
              type="medium"
              text={` ${t('pendingWithdrawalModal.title')} (${withdrawal?.transId})`}
            />
            <ButtonIconComponent style="Close" onClick={onClose} />
          </div>
          {resultMessage.type && (
            <MessageComponent
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
                        <TitleComponent type="label" text={t('pendingWithdrawalModal.to')} />
                        <div className="flex items-center">
                          <span>{renderBankAccount(withdrawal?.paymentTo || '', tGlobal)}</span>
                        </div>
                      </div>

                      <div className="mb-[10px]">
                        <TitleComponent
                          type="label"
                          text={t('pendingWithdrawalModal.withdrawalAmount')}
                        />
                        <div className="mt-1">
                          <span className="py-[4px] text-[var(--text-link-color)] font-semibold">
                            {displayAmount(withdrawal?.amount || 0)}
                          </span>
                        </div>
                      </div>

                      <div>
                        <TitleComponent
                          type="label"
                          text={t('pendingWithdrawalModal.from')}
                          required
                        />
                        <div className="w-full">
                          <DropdownComponent
                            options={bankOptions}
                            onSelect={handleChangePaymentFrom}
                            initialValue={withdrawal?.paymentFrom || ''}
                            placeholder={t('pendingWithdrawalModal.selectBank')}
                            disable={isSubmitting || isProviderProcessing}
                            searchable={true}
                          />
                          {renderMinMaxAmount()}
                        </div>
                      </div>
                    </div>

                    <div>
                      <div className="mb-[16px]">
                        <TitleComponent
                          type="label"
                          text={t('pendingWithdrawalModal.dateAndTime')}
                        />
                        <div>{formatDateTime(withdrawal?.createdTime || '')}</div>
                      </div>
                    </div>
                  </div>

                  <div className="mt-6 h-full">
                    <TitleComponent type="label" text={t('pendingWithdrawalModal.remarks')} />
                    <textarea
                      value={remark}
                      onChange={(e) => setRemark(e.target.value)}
                      maxLength={1000}
                      placeholder={t('pendingWithdrawalModal.remarksPlaceholder')}
                      className="block h-[140px]! py-[10px] control-field resize-none"
                      disabled={isSubmitting || isProviderProcessing}
                    />
                  </div>
                </div>
                <UploadImage
                  setImage={handleImageChange}
                  prevImage={withdrawal?.base64Payslip}
                  disabled={isSubmitting || isProviderProcessing}
                />
              </>
            )}
          </div>
          {}
          <div className="flex gap-4 justify-end">
            {isProviderProcessing && (
              <ButtonCancelComponent
                onClick={onClose}
                disabled={isSubmitting}
                text={t('pendingWithdrawalModal.close')}
              />
            )}
            {!isProviderProcessing && (
              <>
                <ButtonRejectComponent
                  onClick={handleReject}
                  disabled={isSubmitting || isLoading}
                  text={t('pendingWithdrawalModal.reject')}
                />
                <ButtonApplyComponent
                  variant="apply"
                  onClick={handleApprove}
                  disabled={isSubmitting || isLoading}
                  text={t('pendingWithdrawalModal.approve')}
                />
              </>
            )}
          </div>
        </div>
      </div>
    </>
  );
};

export default PendingWithdrawalModal;
