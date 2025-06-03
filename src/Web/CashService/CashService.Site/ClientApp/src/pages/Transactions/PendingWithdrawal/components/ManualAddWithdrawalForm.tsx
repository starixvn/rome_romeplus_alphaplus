import React, { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { WithdrawalAgentBankDropDown } from '../../../../services/interfaces/Transactions/BankInfo';
import { TransactionTypes } from '../../../../constants/sharedConstants';
import { transactionAPI } from '../../../../services/transactionsHistoryAPI';
import {
  getSettingLimitAmtWithdrawal,
  manualAddWithdrawal,
  getWithdrawalMemberCashBalance,
} from '../../../../services/pendingWithdrawalAPI';
import { AddWithdrawalModel } from '../../../../services/interfaces/AddWithdrawalModel';
import { ResultMessageComponent } from '../../../../components/MessageComponents';
import UploadImage from '../../Components/UploadImage';
import BankIcon from '../../../../components/BankComponents';
import { renderBankOptionValue } from '../../../../common/commonFunctions';
import {
  BackButtonComponent,
  ButtonApplyComponent,
  ButtonCancelComponent,
} from '../../../../components/ButtonComponents';
import DropdownComponent, { DropdownOption } from '../../../../components/CustomDropdownComponent';
import TitleComponent, {
  LabelTitleComponent,
  MediumTitleComponent,
} from '../../../../components/TitleComponents';
import { AmountInput } from '../../../../components/ui/AmountInput';

interface ManualAddWithdrawalFormProps {
  onClose?: () => void;
}

const ManualAddWithdrawalForm: React.FC<ManualAddWithdrawalFormProps> = ({ onClose }) => {
  const { t } = useTranslation('pendingWithdrawal');
  const [custId, setCustId] = useState<number | null>(null);
  const [from, setFrom] = useState<string>('');
  const [to, setTo] = useState<string>('');
  const [withdrawalAmount, setWithdrawalAmount] = useState<number>(0);
  const [remarks, setRemarks] = useState('');
  const [image, setImage] = useState<File | null>(null);
  const [minAmount, setMinAmount] = useState(0);
  const [maxAmount, setMaxAmount] = useState<number | null>(null);
  const [memberCashBalance, setMemberCashBalance] = useState<number>(0);
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
        amount: withdrawalAmount,
        remark: remarks,
        image: image as File,
        providerId,
        paymentMethodId,
      };
      console.log(manualWithdrawalData);
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
        setIsSubmitting(false);
      }
    } catch (error) {
      setResultMessage({ type: 'error', message: t('manualWithdrawalForm.error') });
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

    if (isNaN(withdrawalAmount) || withdrawalAmount <= 0 || withdrawalAmount > memberCashBalance) {
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
    const memberBankOptions = memberBanks.map((bank) => ({
      value: renderBankOptionValue(bank.bankCode, bank.bankAccountNumber),
      label: bank.bankAccountNumber,
      code: bank.bankCode,
      providerId: 0,
      paymentMethodId: 0,
    }));
    setMemberBankInfo(memberBankOptions);
    if (memberBankOptions.length > 0) {
      setTo(memberBankOptions[0].value);
    }
    await handleUpdateMemberCashBalance(selectedMemberId);
    await handleUpdateLimitWithdrawalAmt(selectedMemberId);
  };

  const handleFromSelect = (option: DropdownOption) => {
    let providerId = option.attributes?.providerId || 0;
    let paymentMethodId = option.attributes?.paymentMethodId || 0;
    setFrom(option.value.toString());
    setProviderId(providerId);
    setPaymentMethodId(paymentMethodId);
  };

  useEffect(() => {
    if (providerId > 0 && custId) {
      handleUpdateLimitWithdrawalAmt(custId);
    }
  }, [providerId, custId]);

  const handleToSelect = (option: DropdownOption) => {
    setTo(option.value.toString());
  };

  const handleUpdateLimitWithdrawalAmt = async (memberCustId: number | null) => {
    if (!memberCustId || providerId <= 0) {
      return;
    }
    const limits = await getSettingLimitAmtWithdrawal({
      custId: memberCustId,
      providerId,
      paymentMethodId,
    });
    setMinAmount(limits.minAmt);
    setMaxAmount(limits.maxAmt);
  };

  const handleUpdateMemberCashBalance = async (memberCustId: number | null) => {
    if (!memberCustId) return;
    const memberCashBalance = await getWithdrawalMemberCashBalance(memberCustId);
    setMemberCashBalance(memberCashBalance);
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
        value: renderBankOptionValue(bank.bankCode, bank.bankAccountNumber),
        label: bank.bankAccountNumber,
        code: bank.bankCode,
        providerId: 0,
        paymentMethodId: 0,
      }));
      const mappedProviderInfo: WithdrawalAgentBankDropDown[] = providerInfo.map((provider) => ({
        value: renderBankOptionValue('', provider.providerName),
        label: provider.providerName,
        code: '',
        providerId: provider.providerId,
        paymentMethodId: provider.paymentMethodId,
      }));
      const agentBankOptions = [...banks, ...mappedProviderInfo];
      setBankInfo([...banks, ...mappedProviderInfo]);
      setFrom(agentBankOptions[0]?.value.toString());
      setProviderId(agentBankOptions[0]?.providerId || 0);
      setPaymentMethodId(agentBankOptions[0]?.paymentMethodId || 0);

      const memberBanks = await transactionAPI.getCashMemberBankInfo(firstMember?.custId);
      const memberBankOptions = memberBanks.map((bank) => ({
        value: renderBankOptionValue(bank.bankCode, bank.bankAccountNumber),
        label: bank.bankAccountNumber,
        code: bank.bankCode,
        providerId: 0,
        paymentMethodId: 0,
      }));
      setMemberBankInfo(memberBankOptions);
      setTo(memberBankOptions[0]?.value.toString());
      await handleUpdateLimitWithdrawalAmt(firstMember?.custId);
      await handleUpdateMemberCashBalance(firstMember?.custId);
    };

    fetchData();
  }, []);

  const handleMemberSelect = (option: DropdownOption) => {
    handleMemberChange(parseInt(option.value.toString()));
  };

  const renderMinMaxAmount = () => {
    if (providerId <= 0) return null;
    let displayMaxAmount = maxAmount ? maxAmount : 'unlimited';
    return (
      <span className="text-xs text-gray-500 pl-3">
        ({t('manualWithdrawalForm.minAmount')}: {minAmount} - {t('manualWithdrawalForm.maxAmount')}:{' '}
        {displayMaxAmount})
      </span>
    );
  };

  const handleSetWithdrawalAmount = (value: number) => {
    setWithdrawalAmount(value);
  };

  return (
    <div className="py-[12px]">
      <div className="flex items-center justify-flex-start mb-[8px] py-[8px] border-[#cfcfcf] border-b-[1px] [&>:nth-child(2)]:border-0 [&>:nth-child(2)]:mb-[0] [&>:nth-child(1)]:mb-[0]">
        <BackButtonComponent onClick={onClose} />
        <TitleComponent title={t('manualWithdrawalForm.title')} />
      </div>

      <div
        id="manual-withdrawal-form"
        className="w-[1100px] bg-[#ffffff] p-[16px] rounded-[5px] shadow-[0_1px_5px_#0003,0_2px_2px_#00000024,0_3px_1px_-2px_#0000001f] max-[768px]:w-full"
      >
        {resultMessage.type && (
          <ResultMessageComponent
            type={resultMessage.type}
            message={resultMessage.message}
            className="mb-[16px] max-[768px]:mb-[0]"
          />
        )}
        <div className="flex max-[768px]:flex-col">
          <div className="flex-1 max-w-[700px]">
            <MediumTitleComponent text={t('manualWithdrawalForm.accountInformation')} />
            <div className="flex flex-wrap">
              <div className="flex flex-nowrap gap-[16px] mb-[4px] justify-between max-[768px]:gap-[0] max-[768px]:flex-wrap w-full">
                <div className="min-w-[230px] pt-[4px] w-[48%] mb-[8px]">
                  <LabelTitleComponent text={t('manualWithdrawalForm.memberLoginName')} required />

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
                <div className="min-w-[230px] pt-[4px] w-[48%] mb-[8px]">
                  <LabelTitleComponent text={t('manualWithdrawalForm.from')} required />
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
                    searchable={true}
                  />
                  {renderMinMaxAmount()}
                </div>
              </div>
              <div className="flex flex-nowrap gap-[16px] mb-[4px] justify-between max-[768px]:gap-[0] max-[768px]:flex-wrap w-full ">
                <div className="min-w-[230px] pt-[4px] w-[48%] mb-[8px]">
                  <LabelTitleComponent text={t('manualWithdrawalForm.to')} required />
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
                    searchable={true}
                  />
                </div>

                <div className="min-w-[230px] pt-[4px] w-[48%] mb-[8px]">
                  <LabelTitleComponent text={t('manualWithdrawalForm.withdrawalAmount')} required />
                  <AmountInput
                    id="withdrawalAmount"
                    value={withdrawalAmount}
                    onChange={(e) => handleSetWithdrawalAmount(e)}
                    className="border border-gray-300 rounded-md px-2 w-full h-[32px] max-[768px]:h-[40px] focus:outline-none focus:border-[var(--button-border)]"
                  />
                  <label className="block mt-[4px] text-[#828282] text-[11px] max-[768px]:text-[15px] font-semibold">
                    {t('manualWithdrawalForm.memberCashBalance')}: {memberCashBalance}
                  </label>
                </div>
              </div>
              <div className="w-full">
                <LabelTitleComponent text={t('manualWithdrawalForm.remarks')} />
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
          <div className="pl-[32px] max-[768px]:pl-0 max-[768px]:mt-[16px]">
            <UploadImage setImage={setImage} />
          </div>
        </div>
        <div className="flex gap-[16px]">
          <ButtonCancelComponent
            onClick={onClose}
            disabled={isSubmitting}
            text={t('manualWithdrawalForm.cancel')}
          />
          <ButtonApplyComponent
            disabled={isSubmitting}
            text={
              isSubmitting
                ? t('manualWithdrawalForm.processing')
                : t('manualWithdrawalForm.approve')
            }
            onClick={handleSubmit}
          />
        </div>
      </div>
    </div>
  );
};

export default ManualAddWithdrawalForm;
