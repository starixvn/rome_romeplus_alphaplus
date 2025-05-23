import React, { useState, useEffect } from 'react';
import { useTranslation } from 'react-i18next';
import { transactionAPI } from '../../../../services/transactionsHistoryAPI';
import { MemberLoginName } from '../../../../services/interfaces/Transactions/MemberLoginName';
import { BankInfo } from '../../../../services/interfaces/Transactions/BankInfo';
import { CreditAmount } from '../../../../services/interfaces/Transactions/CreditAmount';
import pendingDepositAPI from '../../../../services/pendingDepositAPI';
import UploadImage from '../../Components/UploadImage';
import { AddDepositManualModel } from '../../../../services/interfaces/AddDepositManualModel';
import Loading from '../../../../components/LoadingComponent';
import { ResultMessageComponent } from '../../../../components/MessageComponents';
import AccountInformation from './AccountInformation';
import TitleComponent from '../../../../components/TitleComponents';
import {
  ButtonCancelComponent,
  ButtonApplyComponent,
} from '../../../../components/ButtonComponents';

interface ManualAddDepositFormProps {
  cashAgentOptions: BankInfo[];
  onClose: (isSuccess: boolean) => void;
}

const ManualAddDepositForm: React.FC<ManualAddDepositFormProps> = ({
  cashAgentOptions,
  onClose,
}) => {
  const { t } = useTranslation('pendingDeposit');
  const [memberLoginOptions, setMemberLoginOptions] = useState<MemberLoginName[]>([]);
  const [memberId, setMemberId] = useState<number>(0);
  const [paymentFrom, setPaymentFrom] = useState('');
  const [paymentTo, setPaymentTo] = useState(
    `${cashAgentOptions[0]?.bankCode}_${cashAgentOptions[0]?.bankAccountNumber}`
  );
  const [cashMemberOptions, setCashMemberOptions] = useState<BankInfo[]>([]);
  const [creditAmount, setCreditAmount] = useState<CreditAmount>({ minAmt: 0, maxAmt: 0 });
  const [depositAmount, setDepositAmount] = useState<number>(0);
  const [remarks, setRemarks] = useState('');
  const [image, setImage] = useState<File | null>(null);
  const [initialLoading, setInitialLoading] = useState(true);
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [validationMessage, setValidationMessage] = useState<{
    type: 'error' | 'success' | 'warning';
    message: string;
  } | null>(null);

  useEffect(() => {
    const fetchMemberLoginName = async () => {
      try {
        const memberLoginNames = await transactionAPI.getMemberLoginName();
        setMemberLoginOptions(memberLoginNames);
        setMemberId(memberLoginNames[0]?.custId);
      } catch (error) {
        console.error('Error fetching member login names:', error);
      }
    };
    setValidationMessage(null);
    fetchMemberLoginName();
  }, []);

  useEffect(() => {
    const fetchCashMemberBankInfo = async () => {
      if (memberId) {
        try {
          const bankInfo = await transactionAPI.getCashMemberBankInfo(memberId);
          setCashMemberOptions(bankInfo);
          if (bankInfo.length > 0) {
            setPaymentFrom(`${bankInfo[0]?.bankCode}_${bankInfo[0]?.bankAccountNumber}`);
          }
        } catch (error) {
          console.error('Error fetching cash member bank info:', error);
        }
      }
    };
    setValidationMessage(null);
    fetchCashMemberBankInfo();
  }, [memberId]);

  useEffect(() => {
    const fetchCreditLimitAmount = async () => {
      if (memberId) {
        try {
          const creditAmount = await pendingDepositAPI.getLimitAmtDeposit(memberId);
          setCreditAmount(creditAmount);
        } catch (error) {
          console.error('Error fetching credit limit amount:', error);
        } finally {
          setInitialLoading(false);
        }
      }
    };
    setValidationMessage(null);
    fetchCreditLimitAmount();
  }, [memberId]);

  const getParams = () => {
    const params: AddDepositManualModel = {
      custId: memberId,
      paymentFrom: paymentFrom,
      paymentTo: paymentTo,
      amount: depositAmount,
      remark: remarks,
      image: image as File,
    };
    return params;
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    const params = getParams();

    setIsSubmitting(true);
    try {
      const response = await pendingDepositAPI.createDepositManual(params);
      setValidationMessage(null);
      if (response.isSuccessful) {
        setValidationMessage({
          type: 'success',
          message: t('successMessages.depositSubmitted'),
        });
        setTimeout(() => {
          onClose(true);
        }, 3000);
      } else {
        setValidationMessage({
          type: 'error',
          message: response.message || t('errorMessages.failedToSubmit'),
        });
      }
    } catch (error) {
      setValidationMessage({
        type: 'error',
        message: t('errorMessages.failedToSubmit'),
      });
    } finally {
      setIsSubmitting(false);
    }
  };

  const handleSetDepositAmount = (value: number) => {
    setDepositAmount(value);
  };

  return (
    <div className="">
      {initialLoading ? (
        <div className="flex items-center justify-center h-full">
          <Loading />
        </div>
      ) : (
        <>
          <TitleComponent title={t('manualDepositForm.title')} />

          <form id="manual-deposit-form" onSubmit={handleSubmit}>
            {validationMessage && (
              <ResultMessageComponent
                type={validationMessage.type}
                message={validationMessage.message}
                className="mb-4"
              />
            )}
            <div className="flex flex-row space-x-10">
              <AccountInformation
                memberLoginOptions={memberLoginOptions}
                memberId={memberId}
                onMemberChange={(selected) => setMemberId(Number(selected.value))}
                paymentFrom={paymentFrom}
                onPaymentFromChange={(selected) => setPaymentFrom(String(selected.value))}
                cashMemberOptions={cashMemberOptions}
                paymentTo={paymentTo}
                onPaymentToChange={(selected) => setPaymentTo(String(selected.value))}
                cashAgentOptions={cashAgentOptions}
                depositAmount={depositAmount}
                onDepositAmountChange={(e) => handleSetDepositAmount(e)}
                creditAmount={creditAmount}
                remarks={remarks}
                onRemarksChange={(e) => setRemarks(e.target.value)}
              />

              <div className="pl-20">
                <UploadImage setImage={setImage} />

                <div className="flex justify-center space-x-4 mt-3 w-full">
                  <ButtonCancelComponent
                    className="w-[140px]"
                    text={t('manualDepositForm.cancel')}
                    onClick={() => onClose(false)}
                  />
                  <ButtonApplyComponent
                    className="w-[140px]"
                    text={
                      isSubmitting
                        ? t('manualDepositForm.approving')
                        : t('manualDepositForm.approve')
                    }
                    disabled={isSubmitting}
                  />
                </div>
              </div>
            </div>
          </form>
        </>
      )}
    </div>
  );
};

export default ManualAddDepositForm;
