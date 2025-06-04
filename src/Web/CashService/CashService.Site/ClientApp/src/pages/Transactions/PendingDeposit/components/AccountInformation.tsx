import React from 'react';
import { useTranslation } from 'react-i18next';

import { renderBankOptionValue } from '../../../../common/commonFunctions';
import BankIcon from '../../../../components/BankComponents';
import CustomDropdown, { DropdownOption } from '../../../../components/CustomDropdownComponent';
import { LabelTitleComponent, MediumTitleComponent } from '../../../../components/TitleComponents';
import { AmountInput } from '../../../../components/ui/AmountInput';
import { BankInfo } from '../../../../services/interfaces/Transactions/BankInfo';
import { MemberLoginName } from '../../../../services/interfaces/Transactions/MemberLoginName';

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
  agentAvailableCredit: number;
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
  agentAvailableCredit,
  remarks,
  onRemarksChange,
}) => {
  const { t } = useTranslation('pendingDeposit');

  return (
    <div className="flex-1 max-w-[700px]">
      <MediumTitleComponent text={`${t('manualDepositForm.accountInformation')}`} />
      <div className="flex flex-wrap">
        <div className="flex flex-nowrap gap-[16px] mb-[4px] justify-between max-[768px]:gap-[0] max-[768px]:flex-wrap w-full">
          <div className="min-w-[230px] pt-[4px] w-[48%] mb-[8px]">
            <LabelTitleComponent
              text={`${t('manualDepositForm.memberLoginName')}`}
              required={true}
            />
            <div className="relative z-[9999]">
              <CustomDropdown
                options={memberLoginOptions.map((option) => ({
                  value: option.custId.toString(),
                  label: option.username + (option.nickName ? ' (' + option.nickName + ')' : ''),
                }))}
                onSelect={(selected) => onMemberChange(selected)}
                initialValue={memberId?.toString()}
                showOptionAll={false}
                searchable={true}
              />
            </div>
          </div>

          <div className="min-w-[230px] pt-[4px] w-[48%] mb-[8px]">
            <LabelTitleComponent text={`${t('manualDepositForm.from')}`} required={true} />
            <CustomDropdown
              options={cashMemberOptions.map((option) => ({
                value: renderBankOptionValue(option.bankCode, option.bankAccountNumber),
                label: option.bankAccountNumber,
                icon: <BankIcon code={option.bankCode} />,
              }))}
              onSelect={(selected) => onPaymentFromChange(selected)}
              initialValue={paymentFrom}
              showOptionAll={false}
              searchable={true}
            />
          </div>
        </div>

        <div className="flex flex-nowrap gap-[16px] mb-[4px] justify-between max-[768px]:gap-[0] max-[768px]:flex-wrap w-full ">
          <div className="min-w-[230px] pt-[4px] w-[48%] mb-[8px]">
            <LabelTitleComponent text={`${t('manualDepositForm.to')}`} required={true} />
            <CustomDropdown
              options={cashAgentOptions.map((option) => ({
                value: renderBankOptionValue(option.bankCode, option.bankAccountNumber),
                label: option.bankAccountNumber,
                icon: <BankIcon code={option.bankCode} />,
              }))}
              onSelect={(selected) => onPaymentToChange(selected)}
              initialValue={paymentTo}
              showOptionAll={false}
              searchable={true}
            />
          </div>
          <div className="min-w-[230px] pt-[4px] w-[48%] mb-[8px]">
            <LabelTitleComponent text={`${t('manualDepositForm.depositAmount')}`} required={true} />
            <AmountInput
              id="depositAmount"
              value={depositAmount}
              onChange={onDepositAmountChange}
              className="border border-gray-300 rounded-md px-2 w-full h-[32px] max-[768px]:h-[40px] focus:outline-none focus:border-[var(--button-border)] bg-[var(--background-control)]"
            />
            <p className="text-xs my-[8px] text-gray-500 font-bold">
              {t('manualDepositForm.agentAvailableCredit')}:
              <span className="text-black font-bold">{` ${agentAvailableCredit}`}</span>
            </p>
          </div>
        </div>

        <div className="w-full">
          <LabelTitleComponent text={`${t('manualDepositForm.remarks')}`} required={false} />
          <textarea
            id="remarks"
            value={remarks}
            onChange={onRemarksChange}
            className="border border-gray-300 rounded-md p-2 w-full focus:outline-none focus:border-[var(--button-border)] min-h-[200px] bg-[var(--background-control)]"
            placeholder={t('manualDepositForm.remarksPlaceholder')}
          />
        </div>
      </div>
    </div>
  );
};

export default AccountInformation;
