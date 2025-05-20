import React from 'react';
import { useTranslation } from 'react-i18next';
import { MemberLoginName } from '../../../../services/interfaces/Transactions/MemberLoginName';
import { BankInfo } from '../../../../services/interfaces/Transactions/BankInfo';
import { CreditAmount } from '../../../../services/interfaces/Transactions/CreditAmount';
import CustomDropdown, { DropdownOption } from '../../../../components/CustomDropdownComponent';
import BankIcon from '../../../../components/BankComponents';
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
  depositAmount: string;
  onDepositAmountChange: (e: React.ChangeEvent<HTMLInputElement>) => void;
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
      <h3 className="text-md font-medium text-blue-900 mb-4">
        {t('manualDepositForm.accountInformation')}
      </h3>

      <div className="flex flex-col space-y-4">
        <div className="grid grid-cols-2 gap-4">
          <div>
            <label
              htmlFor="memberLoginName"
              className="block text-sm font-medium text-gray-700 mb-1 relative"
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
              className="block text-sm font-medium text-gray-700 mb-1 relative"
            >
              {t('manualDepositForm.from')}
              <span className="text-red-500 absolute">*</span>
            </label>
            <CustomDropdown
              options={cashMemberOptions.map((option) => ({
                value: `${option.bankCode}_${option.bankAccountNumber}`,
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
              className="block text-sm font-medium text-gray-700 mb-1 relative"
            >
              {t('manualDepositForm.to')}
              <span className="text-red-500 absolute">*</span>
            </label>
            <CustomDropdown
              options={cashAgentOptions.map((option) => ({
                value: `${option.bankCode}_${option.bankAccountNumber}`,
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
              className="block text-sm font-medium text-gray-700 mb-1 relative"
            >
              {t('manualDepositForm.depositAmount')}
              <span className="text-red-500 absolute">*</span>
              <span className="text-sm pl-3">
                <span className="text-blue-900">
                  (Agent Available Credit:
                    <span className="text-red-500">
                      {` ${creditAmount.maxAmt}`}
                    </span>
                  )
                </span>
              </span>
            </label>
            <input
              id="depositAmount"
              type="number"
              name="depositAmount"
              value={depositAmount}
              onChange={onDepositAmountChange}
              className="border border-gray-300 rounded-md px-2 w-full h-[32px] max-[768px]:h-[40px] focus:outline-none focus:ring-1 focus:ring-[#214184] focus:border-[#214184]"
            />
          </div>
        </div>

        <div>
          <label htmlFor="remarks" className="block text-sm font-medium text-gray-700 mb-1">
            {t('manualDepositForm.remarks')}
          </label>
          <textarea
            id="remarks"
            value={remarks}
            onChange={onRemarksChange}
            className="border border-gray-300 rounded-md p-2 w-full focus:outline-none focus:ring-1 focus:ring-[#214184] focus:border-[#214184] min-h-[120px]"
            placeholder={t('manualDepositForm.remarksPlaceholder')}
          />
        </div>
      </div>
    </div>
  );
};

export default AccountInformation;