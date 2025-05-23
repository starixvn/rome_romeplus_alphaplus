import React, { useState, useEffect } from 'react';
import { useTranslation } from 'react-i18next';
import { TransactionItem } from '../../../../services/interfaces/Transactions/TransactionHistory.ts';
import {
  displayAmount,
  formatDateTime,
  renderBankAccount,
} from '../../../../common/commonFunctions.tsx';
import CustomDropdownComponent, {
  DropdownOption,
} from '../../../../components/CustomDropdownComponent';
import UploadImage from '../../Components/UploadImage';
import { transactionAPI } from '../../../../services/transactionsHistoryAPI.ts';
import { TransactionTypes } from '../../../../utils/constants.ts';
import { IconClose } from '../../../../components/Icons.tsx';
import { ResultMessageComponent } from '../../../../components/MessageComponents.tsx';
import { approveWithdrawal, rejectWithdrawal } from '../../../../services/pendingWithdrawalAPI.ts';

interface PendingWithdrawalModalProps {
  withdrawal: TransactionItem;
  onClose: () => void;
}

const PendingWithdrawalModal: React.FC<PendingWithdrawalModalProps> = ({ withdrawal, onClose }) => {
  const { t } = useTranslation('pendingWithdrawal');
  const [remarks, setRemarks] = useState(withdrawal.remark);
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
          value: `${provider.providerName}_${provider.providerName}`,
          label: provider.providerName,
          code: provider.providerName,
        }));

        const bankOptions = bankInfo.map((bank) => ({
          value: `${bank.bankCode}_${bank.bankAccountNumber}`,
          label: bank.bankAccountNumber,
          code: bank.bankCode,
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
        remark: remarks,
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
      remark: remarks,
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
    <div className="bg-[#00000066] fixed inset-0 flex items-center justify-center z-50">
      <div className="p-[20px] bg-white w-[850px] rounded-[4px] shadow-[0_2px_4px_-1px_#00000033,0_4px_5px_#00000024,0_1px_10px_#0000001f]">
        <div className="relative">
          <h5 className="text-[16px] text-[#163f78] font-bold pb-[15px]">
            {t('pendingWithdrawalModal.title')} ({withdrawal.transId})
          </h5>
          <button
            type="button"
            className="absolute top-0 right-0 cursor-pointer"
            onClick={onClose}
            disabled={isSubmitting}
          >
            <IconClose />
          </button>
        </div>
        {resultMessage.type && (
          <ResultMessageComponent
            type={resultMessage.type}
            message={resultMessage.message}
            className="mb-5"
          />
        )}
        <div className="flex gap-6">
          <div className="w-[460px] h-full">
            <div className="border border-[#214184] rounded mb-6">
              <table className="w-full h-full">
                <thead>
                  <tr>
                    <th className="text-sm text-left p-2 text-gray-700">
                      {t('pendingWithdrawalModal.username')}
                    </th>
                    <th className="text-sm text-left p-2 text-gray-700">
                      {t('pendingWithdrawalModal.name')}
                    </th>
                    <th className="text-sm text-left p-2 text-gray-700">
                      {t('pendingWithdrawalModal.betCredit')}
                    </th>
                  </tr>
                </thead>
                <tbody>
                  <tr>
                    <td className="text-sm p-2">{withdrawal.username}</td>
                    <td className="text-sm p-2">{`${withdrawal.firstName} ${withdrawal.lastName}`}</td>
                    <td className="text-sm p-2">{displayAmount(withdrawal.amount)}</td>
                  </tr>
                </tbody>
              </table>
            </div>

            <div className="grid grid-cols-2 gap-6">
              <div>
                <div className="mb-4">
                  <div className="text-sm font-medium mb-1">{t('pendingWithdrawalModal.to')}</div>
                  <div className="flex items-center">
                    <span>{renderBankAccount(withdrawal.paymentTo)}</span>
                  </div>
                </div>

                <div className="mb-4">
                  <div className="text-sm font-medium mb-1">
                    {t('pendingWithdrawalModal.withdrawalAmount')}
                  </div>
                  <div className="mt-1">
                    <span>
                      {displayAmount(withdrawal.amount)} {withdrawal.currency}
                    </span>
                  </div>
                </div>

                <div>
                  <div className="text-sm font-medium mb-1 relative">
                    {t('pendingWithdrawalModal.from')}
                    <span className="text-red-500 ml-1">*</span>
                  </div>
                  <div className="w-full md:w-48">
                    <CustomDropdownComponent
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
                <div className="mb-4">
                  <div className="text-sm font-medium mb-1">
                    {t('pendingWithdrawalModal.dateAndTime')}
                  </div>
                  <div>{formatDateTime(withdrawal.createdTime)}</div>
                </div>
              </div>
            </div>

            <div className="mt-6 h-full">
              <div className="text-sm font-medium mb-1">{t('pendingWithdrawalModal.remarks')}</div>
              <textarea
                value={remarks}
                onChange={(e) => setRemarks(e.target.value)}
                placeholder={t('pendingWithdrawalModal.remarksPlaceholder')}
                className="border border-gray-300 rounded-md p-2 w-full h-[120px] focus:outline-none focus:ring-1 focus:ring-[#214184] focus:border-[#214184] text-gray-700"
                disabled={isSubmitting}
              />
            </div>
          </div>

          <div className="w-[240px] pl-20">
            <UploadImage setImage={setImage} disabled={isSubmitting} />
          </div>
        </div>

        <div className="flex justify-end space-x-3 mt-6">
          <button
            onClick={handleReject}
            className="w-[140px] capitalize font-bold h-[32px] max-[768px]:h-[40px] rounded-[20px] px-[16px] text-[#163f78] py-[4px] text-[12px] border-[1px] border-[#163f78] mx-[4px] bg-[#ffffff] hover:bg-[#dee2e7] cursor-pointer shadow-[0_1px_5px_#0003,0_2px_2px_#00000024,0_3px_1px_-2px_#0000001f] disabled:opacity-50 disabled:cursor-not-allowed"
            disabled={isSubmitting}
          >
            {t('pendingWithdrawalModal.reject')}
          </button>
          <button
            onClick={handleApprove}
            className="w-[140px] capitalize font-bold h-[32px] max-[768px]:h-[40px] rounded-[20px] text-white px-[16px] py-[4px] text-[12px] border-[1px] border-[#163f78] mx-[4px] bg-[#163f78] hover:bg-[var(--button-background-hover)] cursor-pointer shadow-[0_1px_5px_#0003,0_2px_2px_#00000024,0_3px_1px_-2px_#0000001f] disabled:opacity-50 disabled:cursor-not-allowed"
            disabled={isSubmitting}
          >
            {t('pendingWithdrawalModal.approve')}
          </button>
        </div>
      </div>
    </div>
  );
};

export default PendingWithdrawalModal;
