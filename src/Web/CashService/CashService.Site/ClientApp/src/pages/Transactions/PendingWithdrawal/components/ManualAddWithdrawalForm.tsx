import React, { useState, useEffect } from 'react';
import { useTranslation } from 'react-i18next';
import { WithdrawalAgentBankDropDown } from '../../../../services/interfaces/Transactions/BankInfo';
import { MemberLoginName } from '../../../../services/interfaces/Transactions/MemberLoginName';
import { TransactionTypes } from '../../../../utils/constants';
import { transactionAPI } from '../../../../services/transactionsHistoryAPI';
import {
  getSettingLimitAmtWithdrawal,
  manualAddWithdrawal,
} from '../../../../services/pendingWithdrawalAPI';
import { AddWithdrawalModel } from '../../../../services/interfaces/AddWithdrawalModel';
import { Alert } from '../../../../components/ui/Alert';
import UploadImage from '../../Components/UploadImage';

interface ManualAddWithdrawalFormProps {
  onClose: () => void;
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
  const [maxAmount, setMaxAmount] = useState(0);
  const [providerId, setProviderId] = useState(0);
  const [memberLoginNames, setMemberLoginNames] = useState<MemberLoginName[]>([]);
  const [agentBankInfo, setBankInfo] = useState<WithdrawalAgentBankDropDown[]>([]);
  const [memberBankInfo, setMemberBankInfo] = useState<WithdrawalAgentBankDropDown[]>([]);
  const [alert, setAlert] = useState<{ type: 'success' | 'error' | null; message: string }>({
    type: null,
    message: '',
  });

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!verifyParam()) return;

    const manualWithdrawalData: AddWithdrawalModel = {
      custId: custId as number,
      paymentFrom: from,
      paymentTo: to,
      minLimitAmt: minAmount,
      maxLimitAmt: maxAmount,
      amount: parseFloat(withdrawalAmount),
      remark: remarks,
      image: image as File,
      providerId,
    };
    const addWithdrawalResponse = await manualAddWithdrawal(manualWithdrawalData);
    if (addWithdrawalResponse.isSuccessful) {
      setAlert({ type: 'success', message: t('addWithdrawalSuccess') });
      setTimeout(() => {
        onClose();
      }, 3000);
    } else {
      setAlert({ type: 'error', message: addWithdrawalResponse.message });
    }
  };

  const verifyParam = (): boolean => {
    if (!custId || custId <= 0) {
      setAlert({ type: 'error', message: t('manualWithdrawalForm.memberLoginNameRequired') });
      return false;
    }

    if (!from) {
      setAlert({ type: 'error', message: t('manualWithdrawalForm.agentBankSelectionRequired') });
      return false;
    }

    if (!to) {
      setAlert({ type: 'error', message: t('manualWithdrawalForm.memberBankSelectionRequired') });
      return false;
    }

    const amount = parseFloat(withdrawalAmount);
    if (isNaN(amount) || amount <= minAmount || amount > maxAmount) {
      setAlert({ type: 'error', message: t('manualWithdrawalForm.amountRangeError') });
      return false;
    }

    if (image && image.size > 1024 * 1024) {
      setAlert({ type: 'error', message: t('manualWithdrawalForm.imageSizeError') });
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
        providerId: 0,
        paymentMethodId: 0,
      }))
    );
    await handleUpdateLimitWithdrawalAmt();
  };

  const handleFromChange = async (e: React.ChangeEvent<HTMLSelectElement>) => {
    const selectedFrom = e.target.value;
    setFrom(selectedFrom);
    await handleUpdateLimitWithdrawalAmt();
  };

  const handleUpdateLimitWithdrawalAmt = async () => {
    if (!custId) return;

    const fromSelect = document.getElementById('from') as HTMLSelectElement;
    const selectedOption = fromSelect?.options[fromSelect.selectedIndex];
    const providerId = selectedOption
      ? parseInt(selectedOption.getAttribute('data-provider-id') || '0')
      : 0;
    const paymentMethodId = selectedOption
      ? parseInt(selectedOption.getAttribute('data-payment-method-id') || '0')
      : 0;
    const limits = await getSettingLimitAmtWithdrawal({
      custId,
      providerId,
      paymentMethodId,
    });
    setMinAmount(limits.minAmt);
    setMaxAmount(limits.maxAmt);
    setProviderId(providerId);
  };

  useEffect(() => {
    const fetchData = async () => {
      const memberNames = await transactionAPI.getMemberLoginName();
      setMemberLoginNames(memberNames);

      const bankInfo = await transactionAPI.getCashAgentBankInfo(TransactionTypes.Withdrawal);
      const providerInfo = await transactionAPI.getCashAgentProvider(TransactionTypes.Withdrawal);
      const banks: WithdrawalAgentBankDropDown[] = bankInfo.map((bank) => ({
        value: `${bank.bankCode}_${bank.bankAccountNumber}`,
        label: bank.bankAccountNumber,
        providerId: 0,
        paymentMethodId: 0,
      }));
      const mappedProviderInfo: WithdrawalAgentBankDropDown[] = providerInfo.map((provider) => ({
        value: `${provider.providerName}_${provider.providerName}`,
        label: provider.providerName,
        providerId: provider.providerId,
        paymentMethodId: provider.paymentMethodId,
      }));

      setBankInfo([...banks, ...mappedProviderInfo]);
    };

    fetchData();
  }, []);

  const renderMinMaxAmount = () => {
    if (maxAmount === 0) {
      return '';
    }
    return (
      <span className="text-xs text-gray-500 pl-3">
        ({minAmount} - {maxAmount})
      </span>
    );
  };

  return (
    <div className="fixed inset-0 bg-white flex flex-col z-50">
      <div className="bg-gray-100 p-4">
        <h2 className="text-lg font-medium">{t('manualWithdrawalForm.title')}</h2>
      </div>

      <form id="manual-withdrawal-form" onSubmit={handleSubmit} className="p-6 flex-1">
        {alert.type && (
          <Alert
            message={alert.message}
            type={alert.type}
            showIcon
            style={{ marginBottom: '20px' }}
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
                    <select
                      id="memberLoginName"
                      value={custId ?? ''}
                      onChange={(e) => handleMemberChange(parseInt(e.target.value))}
                      className="border border-gray-300 rounded-md px-2 w-full h-[32px] max-[768px]:h-[40px] focus:outline-none focus:ring-1 focus:ring-[#214184] focus:border-[#214184] appearance-auto"
                    >
                      <option value="" disabled hidden>
                        {t('manualWithdrawalForm.selectMember')}
                      </option>
                      {memberLoginNames.map((name) => (
                        <option key={name.custId} value={name.custId}>
                          {name.username}
                        </option>
                      ))}
                    </select>
                  </div>

                  <div>
                    <label
                      htmlFor="from"
                      className="block text-sm font-medium text-gray-700 mb-1 relative"
                    >
                      {t('manualWithdrawalForm.from')}
                      <span className="text-red-500 absolute">*</span>
                    </label>
                    <select
                      id="from"
                      value={from}
                      onChange={handleFromChange}
                      className="border border-gray-300 rounded-md px-2 w-full h-[32px] max-[768px]:h-[40px] focus:outline-none focus:ring-1 focus:ring-[#214184] focus:border-[#214184] appearance-auto"
                    >
                      <option value="" disabled hidden>
                        {t('manualWithdrawalForm.selectBank')}
                      </option>
                      {agentBankInfo.map((bank) => (
                        <option
                          key={bank.value}
                          value={bank.value}
                          data-provider-id={bank.providerId}
                          data-payment-method-id={bank.paymentMethodId}
                        >
                          {bank.label}
                        </option>
                      ))}
                    </select>
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
                    <select
                      id="to"
                      value={to}
                      onChange={(e) => setTo(e.target.value)}
                      className="border border-gray-300 rounded-md px-2 w-full h-[32px] max-[768px]:h-[40px] focus:outline-none focus:ring-1 focus:ring-[#214184] focus:border-[#214184] appearance-auto"
                      disabled={!custId}
                    >
                      <option value="" disabled hidden>
                        {t('manualWithdrawalForm.selectMemberBank')}
                      </option>
                      {memberBankInfo.map((bank) => (
                        <option key={bank.value} value={bank.value}>
                          {bank.label}
                        </option>
                      ))}
                    </select>
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
                      type="number"
                      value={withdrawalAmount}
                      onChange={(e) => setWithdrawalAmount(e.target.value)}
                      className="border border-gray-300 rounded-md px-2 w-full h-[32px] max-[768px]:h-[40px] focus:outline-none focus:ring-1 focus:ring-[#214184] focus:border-[#214184]"
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
                    className="border border-gray-300 rounded-md p-2 w-full focus:outline-none focus:ring-1 focus:ring-[#214184] focus:border-[#214184] min-h-[120px]"
                    placeholder={t('manualWithdrawalForm.remarksPlaceholder')}
                  />
                </div>
              </div>
            </div>
          </div>

          <div className="flex flex-col items-center pl-20">
            <UploadImage setImage={setImage} />

            <div className="flex justify-center space-x-4 mt-3 w-full">
              <button
                type="button"
                onClick={onClose}
                className="w-[140px] h-[32px] max-[768px]:h-[40px] border border-gray-300 rounded-full text-sm font-medium text-gray-700 bg-white hover:bg-gray-50 focus:outline-none"
              >
                {t('manualWithdrawalForm.cancel')}
              </button>
              <button
                type="submit"
                className="w-[140px] h-[32px] max-[768px]:h-[40px] border border-transparent rounded-full text-sm font-medium text-white bg-[#163f78] hover:bg-[#0f2d5c] focus:outline-none"
              >
                {t('manualWithdrawalForm.approve')}
              </button>
            </div>
          </div>
        </div>
      </form>
    </div>
  );
};

export default ManualAddWithdrawalForm;
