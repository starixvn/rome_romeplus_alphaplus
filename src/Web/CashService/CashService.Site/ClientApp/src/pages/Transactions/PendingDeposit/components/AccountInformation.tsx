import React from 'react';
import { useTranslation } from 'react-i18next';

import { displayAmount, renderBankOptionValue } from '../../../../utils/commonFunctions';
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
    <div className="flex-1 max-w-[700px] max-[768px]:mb-[16px] mr-[30px] max-[768px]:mr-[0]">
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
              className="control-field"
            />
            <p className="text-[12px] my-[4px]">
              {t('manualDepositForm.agentAvailableCredit')}:
              <span className="font-[500] text-[var(--text-color-secondary)]">{` ${displayAmount(agentAvailableCredit)}`}</span>
            </p>
          </div>
        </div>

        <div className="w-full">
          <LabelTitleComponent text={`${t('manualDepositForm.remarks')}`} required={false} />
          <textarea
            id="remarks"
            value={remarks}
            maxLength={1000}
            onChange={onRemarksChange}
            className="block control-field h-[200px]! py-[8px]! max-[768px]:h-[150px]!"
            placeholder={t('manualDepositForm.remarksPlaceholder')}
          />
        </div>
      </div>
    </div>
  );
};

export default AccountInformation;
