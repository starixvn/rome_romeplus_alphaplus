import React, { useState, useEffect } from 'react';
import { useTranslation } from 'react-i18next';
import { WithdrawalAgentBankDropDown } from '../../../../services/interfaces/Transactions/BankInfo';
import { TransactionTypes } from '../../../../utils/constants';
import { transactionAPI } from '../../../../services/transactionsHistoryAPI';
import {
  getSettingLimitAmtWithdrawal,
  manualAddWithdrawal,
} from '../../../../services/pendingWithdrawalAPI';
import { AddWithdrawalModel } from '../../../../services/interfaces/AddWithdrawalModel';
import { ResultMessageComponent } from '../../../../components/MessageComponents';
import UploadImage from '../../Components/UploadImage';
import DropdownComponent, { DropdownOption } from '../../../../components/CustomDropdownComponent';
import BankIcon from '../../../../components/BankComponents';
interface ManualAddWithdrawalFormProps {
  onClose?: () => void;
}

const ManualAddWithdrawalForm: React.FC<ManualAddWithdrawalFormProps> = ({ onClose }) => {
  const { t } = useTranslation('pendingWithdrawal');
  const [custId, setCustId] = useState<number | null>(null);
  const [from, setFrom] = useState<string>('');
  const [to, setTo] = useState<string>('');
  const [withdrawalAmount, setWithdrawalAmount] = useState('');
  const [remarks, setRemarks] = useState('');
  const [image, setImage] = useState<File | null>(null);
  const [minAmount, setMinAmount] = useState(0);
  const [maxAmount, setMaxAmount] = useState<number | null>(null);
  const [providerId, setProviderId] = useState(0);
  const [paymentMethodId, setPaymentMethodId] = useState(0);
  const [agentBankInfo, setBankInfo] = useState<WithdrawalAgentBankDropDown[]>([]);
  const [memberBankInfo, setMemberBankInfo] = useState<WithdrawalAgentBankDropDown[]>([]);
  const [resultMessage, setResultMessage] = useState<{
    type: 'success' | 'error' | 'warning' | null;
    message: string;
  }>({
    type: null,
    message: '',
  });
  const [memberOptions, setMemberOptions] = useState<DropdownOption[]>([]);
  const [isSubmitting, setIsSubmitting] = useState(false);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!verifyParam()) return;

    setIsSubmitting(true);
    try {
      const manualWithdrawalData: AddWithdrawalModel = {
        custId: custId as number,
        paymentFrom: from,
        paymentTo: to,
        amount: parseFloat(withdrawalAmount),
        remark: remarks,
        image: image as File,
        providerId,
      };

      const addWithdrawalResponse = await manualAddWithdrawal(manualWithdrawalData);
      if (addWithdrawalResponse.isSuccessful) {
        setResultMessage({
          type: 'success',
          message: t('manualWithdrawalForm.addWithdrawalSuccess'),
        });
        if (onClose) {
          setTimeout(() => {
            onClose();
          }, 3000);
        }
      } else {
        setResultMessage({ type: 'error', message: addWithdrawalResponse.message });
      }
    } finally {
      setIsSubmitting(false);
    }
  };

  const verifyParam = (): boolean => {
    if (!custId || custId <= 0) {
      setResultMessage({
        type: 'error',
        message: t('manualWithdrawalForm.memberLoginNameRequired'),
      });
      return false;
    }

    if (!from) {
      setResultMessage({
        type: 'error',
        message: t('manualWithdrawalForm.agentBankSelectionRequired'),
      });
      return false;
    }

    if (!to) {
      setResultMessage({
        type: 'error',
        message: t('manualWithdrawalForm.memberBankSelectionRequired'),
      });
      return false;
    }

    const amount = parseFloat(withdrawalAmount);
    if (isNaN(amount) || amount <= minAmount || (maxAmount && amount > maxAmount)) {
      setResultMessage({ type: 'error', message: t('manualWithdrawalForm.amountRangeError') });
      return false;
    }

    if (image && image.size > 1024 * 1024) {
      setResultMessage({ type: 'error', message: t('manualWithdrawalForm.imageSizeError') });
      return false;
    }

    return true;
  };

  const handleMemberChange = async (selectedMemberId: number) => {
    setCustId(selectedMemberId);
    setTo('');
    const memberBanks = await transactionAPI.getCashMemberBankInfo(selectedMemberId);
    setMemberBankInfo(
      memberBanks.map((bank) => ({
        value: `${bank.bankCode}_${bank.bankAccountNumber}`,
        label: bank.bankAccountNumber,
        code: bank.bankCode,
        providerId: 0,
        paymentMethodId: 0,
      }))
    );
    await handleUpdateLimitWithdrawalAmt();
  };

  const handleFromSelect = (option: DropdownOption) => {
    setFrom(option.value.toString());
    setProviderId(option.attributes?.providerId || 0);
    setPaymentMethodId(option.attributes?.paymentMethodId || 0);
    handleUpdateLimitWithdrawalAmt();
  };

  const handleToSelect = (option: DropdownOption) => {
    setTo(option.value.toString());
  };

  const handleUpdateLimitWithdrawalAmt = async () => {
    if (!custId) return;

    const limits = await getSettingLimitAmtWithdrawal({
      custId,
      providerId,
      paymentMethodId,
    });
    setMinAmount(limits.minAmt);
    setMaxAmount(limits.maxAmt);
  };

  useEffect(() => {
    const fetchData = async () => {
      const [memberNames, bankInfo, providerInfo] = await Promise.all([
        transactionAPI.getMemberLoginName(),
        transactionAPI.getCashAgentBankInfo(TransactionTypes.Withdrawal),
        transactionAPI.getCashAgentProvider(TransactionTypes.Withdrawal),
      ]);

      setMemberOptions(
        memberNames.map((member) => ({
          value: member.custId.toString(),
          label: member.username,
        }))
      );
      const firstMember = memberNames[0];
      setCustId(firstMember?.custId);

      const banks: WithdrawalAgentBankDropDown[] = bankInfo.map((bank) => ({
        value: `${bank.bankCode}_${bank.bankAccountNumber}`,
        label: bank.bankAccountNumber,
        code: bank.bankCode,
        providerId: 0,
        paymentMethodId: 0,
      }));
      const mappedProviderInfo: WithdrawalAgentBankDropDown[] = providerInfo.map((provider) => ({
        value: `${provider.providerName}_${provider.providerName}`,
        label: provider.providerName,
        code: provider.providerName,
        providerId: provider.providerId,
        paymentMethodId: provider.paymentMethodId,
      }));
      const agentBankOptions = [...banks, ...mappedProviderInfo];
      setBankInfo([...banks, ...mappedProviderInfo]);
      setFrom(agentBankOptions[0]?.value.toString());
      setProviderId(agentBankOptions[0]?.providerId || 0);
      setPaymentMethodId(agentBankOptions[0]?.paymentMethodId || 0);

      handleUpdateLimitWithdrawalAmt();

      const memberBanks = await transactionAPI.getCashMemberBankInfo(firstMember?.custId);
      const memberBankOptions = memberBanks.map((bank) => ({
        value: `${bank.bankCode}_${bank.bankAccountNumber}`,
        label: bank.bankAccountNumber,
        code: bank.bankCode,
        providerId: 0,
        paymentMethodId: 0,
      }));
      setMemberBankInfo(memberBankOptions);
      setTo(memberBankOptions[0]?.value.toString());
      await handleUpdateLimitWithdrawalAmt();
    };

    fetchData();
  }, []);

  const handleMemberSelect = (option: DropdownOption) => {
    handleMemberChange(parseInt(option.value.toString()));
  };

  const renderMinMaxAmount = () => {
    let displayMaxAmount = maxAmount ? maxAmount : 'unlimited';
    return (
      <span className="text-xs text-gray-500 pl-3">(Member cash balance: {displayMaxAmount})</span>
    );
  };

  const handleSetWithdrawalAmount = (e: React.ChangeEvent<HTMLInputElement>) => {
    if (e.target.validity.valid) {
      setWithdrawalAmount(e.target.value);
    }
  };

  return (
    <div className="fixed inset-0 bg-white flex flex-col z-50">
      <div className="bg-gray-100 p-4">
        <h2 className="text-lg font-medium">{t('manualWithdrawalForm.title')}</h2>
      </div>

      <form id="manual-withdrawal-form" onSubmit={handleSubmit} className="p-6 flex-1">
        {resultMessage.type && (
          <ResultMessageComponent
            type={resultMessage.type}
            message={resultMessage.message}
            className="mb-5"
          />
        )}
        <div className="flex flex-row space-x-10">
          <div className="">
            <div className="mb-5 w-[900px]">
              <h3 className="text-md font-medium text-blue-900 mb-4">
                {t('manualWithdrawalForm.accountInformation')}
              </h3>

              <div className="flex flex-col space-y-4">
                <div className="grid grid-cols-2 gap-4">
                  <div>
                    <label
                      htmlFor="memberLoginName"
                      className="block text-sm font-medium text-gray-700 mb-1 relative"
                    >
                      {t('manualWithdrawalForm.memberLoginName')}
                      <span className="text-red-500 absolute">*</span>
                    </label>
                    <div className="relative z-[9999]">
                      <DropdownComponent
                        options={memberOptions}
                        onSelect={handleMemberSelect}
                        initialValue={custId?.toString()}
                        searchable={true}
                        placeholder={t('manualWithdrawalForm.selectMember')}
                      />
                    </div>
                  </div>

                  <div>
                    <label
                      htmlFor="from"
                      className="block text-sm font-medium text-gray-700 mb-1 relative"
                    >
                      {t('manualWithdrawalForm.from')}
                      <span className="text-red-500 absolute">*</span>
                    </label>
                    <DropdownComponent
                      options={agentBankInfo.map((bank) => ({
                        value: bank.value,
                        label: bank.label,
                        icon: <BankIcon code={bank.code} />,
                        attributes: {
                          providerId: bank.providerId,
                          paymentMethodId: bank.paymentMethodId,
                        },
                      }))}
                      onSelect={handleFromSelect}
                      initialValue={from}
                    />
                  </div>
                </div>

                <div className="grid grid-cols-2 gap-4">
                  <div>
                    <label
                      htmlFor="to"
                      className="block text-sm font-medium text-gray-700 mb-1 relative"
                    >
                      {t('manualWithdrawalForm.to')}
                      <span className="text-red-500 absolute">*</span>
                    </label>
                    <DropdownComponent
                      options={memberBankInfo.map((bank) => ({
                        value: bank.value,
                        label: bank.label,
                        icon: <BankIcon code={bank.code} />,
                        attributes: {
                          providerId: bank.providerId,
                          paymentMethodId: bank.paymentMethodId,
                        },
                      }))}
                      onSelect={handleToSelect}
                      initialValue={to}
                      showOptionAll={false}
                      disable={!custId}
                      placeholder={t('manualWithdrawalForm.selectMemberBank')}
                    />
                  </div>

                  <div>
                    <label
                      htmlFor="withdrawalAmount"
                      className="block text-sm font-medium text-gray-700 mb-1 relative"
                    >
                      {t('manualWithdrawalForm.withdrawalAmount')}
                      <span className="text-red-500 absolute">*</span>
                      {renderMinMaxAmount()}
                    </label>

                    <input
                      id="withdrawalAmount"
                      type="text"
                      pattern="[0-9]*"
                      value={withdrawalAmount}
                      onChange={(e) => handleSetWithdrawalAmount(e)}
                      className="border border-gray-300 rounded-md px-2 w-full h-[32px] max-[768px]:h-[40px] focus:outline-none focus:border-[var(--button-border)]"
                      placeholder="0"
                    />
                  </div>
                </div>

                <div>
                  <label htmlFor="remarks" className="block text-sm font-medium text-gray-700 mb-1">
                    {t('manualWithdrawalForm.remarks')}
                  </label>
                  <textarea
                    id="remarks"
                    value={remarks}
                    onChange={(e) => setRemarks(e.target.value)}
                    className="border border-gray-300 rounded-md p-2 w-full focus:outline-none focus:border-[var(--button-border)] min-h-[120px]"
                    placeholder={t('manualWithdrawalForm.remarksPlaceholder')}
                  />
                </div>
              </div>
            </div>
          </div>

          <div className="pl-20">
            <UploadImage setImage={setImage} />

            <div className="flex justify-center space-x-4 mt-3 w-full">
              <button
                type="button"
                onClick={onClose}
                className="w-[140px] h-[32px] max-[768px]:h-[40px] border border-gray-300 rounded-full text-sm font-medium text-gray-700 bg-white hover:bg-gray-100 focus:outline-none transition-colors duration-200 cursor-pointer"
              >
                {t('manualWithdrawalForm.cancel')}
              </button>
              <button
                type="submit"
                disabled={isSubmitting}
                className={`w-[140px] h-[32px] max-[768px]:h-[40px] border border-transparent rounded-full text-sm font-medium text-white bg-[#163f78] hover:bg-[#0f2d5c] focus:outline-none transition-colors duration-200 ${
                  isSubmitting ? 'opacity-50 cursor-not-allowed' : 'cursor-pointer'
                }`}
              >
                {isSubmitting
                  ? t('manualWithdrawalForm.processing')
                  : t('manualWithdrawalForm.approve')}
              </button>
            </div>
          </div>
        </div>
      </form>
    </div>
  );
};

export default ManualAddWithdrawalForm;
