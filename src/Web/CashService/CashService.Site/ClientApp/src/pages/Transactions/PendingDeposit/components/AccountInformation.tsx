import React from 'react';
import { useTranslation } from 'react-i18next';
import { MemberLoginName } from '../../../../services/interfaces/Transactions/MemberLoginName';
import { BankInfo } from '../../../../services/interfaces/Transactions/BankInfo';
import { CreditAmount } from '../../../../services/interfaces/Transactions/CreditAmount';
import CustomDropdown, { DropdownOption } from '../../../../components/CustomDropdownComponent';
import BankIcon from '../../../../components/BankComponents';
import { AmountInput } from '../../../../components/ui/AmountInput';
import { renderBankOptionValue } from '../../../../common/commonFunctions';
interface AccountInformationProps {
  memberLoginOptions: MemberLoginName[];
  memberId: number;
  onMemberChange: (selected: DropdownOption) => void;
  paymentFrom: string;
  onPaymentFromChange: (selected: DropdownOption) => void;
  cashMemberOptions: BankInfo[];
  paymentTo: string;
  onPaymentToChange: (selected: DropdownOption) => void;
  cashAgentOptions: BankInfo[];
  depositAmount: number;
  onDepositAmountChange: (value: number) => void;
  creditAmount: CreditAmount;
  remarks: string;
  onRemarksChange: (e: React.ChangeEvent<HTMLTextAreaElement>) => void;
}

const AccountInformation: React.FC<AccountInformationProps> = ({
  memberLoginOptions,
  memberId,
  onMemberChange,
  paymentFrom,
  onPaymentFromChange,
  cashMemberOptions,
  paymentTo,
  onPaymentToChange,
  cashAgentOptions,
  depositAmount,
  onDepositAmountChange,
  creditAmount,
  remarks,
  onRemarksChange,
}) => {
  const { t } = useTranslation('pendingDeposit');

  return (
    <div className="mb-5 w-[900px]">
      <h3 className="text-md font-medium text-blue-900 mb-3">
        {t('manualDepositForm.accountInformation')}
      </h3>

      <div className="flex flex-col space-y-4">
        <div className="grid grid-cols-2 gap-4">
          <div>
            <label
              htmlFor="memberLoginName"
              className="block font-semibold text-[#333333] my-[8px] relative"
            >
              {t('manualDepositForm.memberLoginName')}
              <span className="text-red-500 absolute">*</span>
            </label>
            <div className="relative z-[9999]">
              <CustomDropdown
                options={memberLoginOptions.map((option) => ({
                  value: option.custId.toString(),
                  label: option.username,
                }))}
                onSelect={(selected) => onMemberChange(selected)}
                initialValue={memberId?.toString()}
                showOptionAll={false}
                searchable={true}
              />
            </div>
          </div>

          <div>
            <label
              htmlFor="paymentFrom"
              className="block font-semibold text-[#333333] my-[8px] relative"
            >
              {t('manualDepositForm.from')}
              <span className="text-red-500 absolute">*</span>
            </label>
            <CustomDropdown
              options={cashMemberOptions.map((option) => ({
                value: renderBankOptionValue(option.bankCode, option.bankAccountNumber),
                label: option.bankAccountNumber,
                icon: <BankIcon code={option.bankCode} />
              }))}
              onSelect={(selected) => onPaymentFromChange(selected)}
              initialValue={paymentFrom}
              showOptionAll={false}
            />
          </div>
        </div>

        <div className="grid grid-cols-2 gap-4">
          <div>
            <label
              htmlFor="paymentTo"
              className="block font-semibold text-[#333333] my-[8px] relative"
            >
              {t('manualDepositForm.to')}
              <span className="text-red-500 absolute">*</span>
            </label>
            <CustomDropdown
              options={cashAgentOptions.map((option) => ({
                value: renderBankOptionValue(option.bankCode, option.bankAccountNumber),
                label: option.bankAccountNumber,
                code: option.bankCode,
              }))}
              onSelect={(selected) => onPaymentToChange(selected)}
              initialValue={paymentTo}
              showOptionAll={false}
            />
          </div>

          <div>
            <label
              htmlFor="depositAmount"
              className="block font-semibold text-[#333333] my-[8px] relative"
            >
              {t('manualDepositForm.depositAmount')}
              <span className="text-red-500 absolute">*</span>
              <span className="ml-2 inline-block">
                <span className="text-blue-900">
                  ({t('manualDepositForm.agentAvailableCredit')}:
                  <span className="text-red-500">{` ${creditAmount.maxAmt}`}</span>)
                </span>
              </span>
            </label>
            <AmountInput
              id="depositAmount"
              value={depositAmount}
              onChange={onDepositAmountChange}
              className="border border-gray-300 rounded-md px-2 w-full h-[32px] max-[768px]:h-[40px] focus:outline-none focus:border-[var(--button-border)] bg-[var(--background-control)]"
            />
          </div>
        </div>

        <div>
          <label htmlFor="remarks" className="block font-semibold text-[#333333] my-[8px]">
            {t('manualDepositForm.remarks')}
          </label>
          <textarea
            id="remarks"
            value={remarks}
            onChange={onRemarksChange}
            className="border border-gray-300 rounded-md p-2 w-full focus:outline-none focus:border-[var(--button-border)] min-h-[120px] bg-[var(--background-control)]"
            placeholder={t('manualDepositForm.remarksPlaceholder')}
          />
        </div>
      </div>
    </div>
  );
};

export default AccountInformation;
