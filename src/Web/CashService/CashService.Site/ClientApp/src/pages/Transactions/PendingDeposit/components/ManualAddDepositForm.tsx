import React, { useState, useEffect } from 'react';
import { useTranslation } from 'react-i18next';
import { transactionAPI } from '../../../../services/transactionsHistoryAPI';
import { MemberLoginName } from '../../../../services/interfaces/Transactions/MemberLoginName';
import { BankInfo } from '../../../../services/interfaces/Transactions/BankInfo';
import pendingDepositAPI from '../../../../services/pendingDepositAPI';
import UploadImage from '../../Components/UploadImage';
import { AddDepositManualModel } from '../../../../services/interfaces/AddDepositManualModel';
import { LoadingFullComponent } from '../../../../components/LoadingComponent';
import { ResultMessageComponent } from '../../../../components/MessageComponents';
import AccountInformation from './AccountInformation';
import TitleComponent from '../../../../components/TitleComponents';
import {
  ButtonCancelComponent,
  ButtonApplyComponent,
  BackButtonComponent,
} from '../../../../components/ButtonComponents';
import { renderBankOptionValue } from '../../../../common/commonFunctions';
import { ResultMessage } from '../../../../services/interfaces/ResultMessage';

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
    renderBankOptionValue(cashAgentOptions[0]?.bankCode, cashAgentOptions[0]?.bankAccountNumber)
  );
  const [cashMemberOptions, setCashMemberOptions] = useState<BankInfo[]>([]);
  const [agentAvailableCredit, setAgentAvailableCredit] = useState<number>(0);
  const [depositAmount, setDepositAmount] = useState<number>(0);
  const [remarks, setRemarks] = useState('');
  const [image, setImage] = useState<File | null>(null);
  const [initialLoading, setInitialLoading] = useState(true);
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [validationMessages, setValidationMessages] = useState<ResultMessage[]>([]);

  useEffect(() => {
    const fetchInitialData = async () => {
      try {
        const memberLoginNames = await transactionAPI.getMemberLoginName();
        setMemberLoginOptions(memberLoginNames);

        if (memberLoginNames?.length > 0) {
          const selectedMemberId = memberLoginNames[0].custId;
          setMemberId(selectedMemberId);

          const [bankInfo, agentAC] = await Promise.all([
            transactionAPI.getCashMemberBankInfo(selectedMemberId),
            pendingDepositAPI.getLimitAmtDeposit(selectedMemberId)
          ]);

          setCashMemberOptions(bankInfo);
          if (bankInfo.length > 0) {
            setPaymentFrom(
              renderBankOptionValue(bankInfo[0].bankCode, bankInfo[0].bankAccountNumber)
            );
          }
          setAgentAvailableCredit(agentAC);
        }
      } catch {
        setValidationMessages([{
          type: 'error',
          message: t('errorMessages.failedToFetchData')
        }]);
      } finally {
        setInitialLoading(false);
      }
    };

    fetchInitialData();
  }, []);

  useEffect(() => {
    const fetchMemberData = async () => {
      if (!memberId) return;
      
      setValidationMessages([]);
      const [bankInfo, agentAC] = await Promise.all([
        transactionAPI.getCashMemberBankInfo(memberId),
        pendingDepositAPI.getLimitAmtDeposit(memberId)
      ]);

      setCashMemberOptions(bankInfo);
      if (bankInfo.length > 0) {
        setPaymentFrom(
          renderBankOptionValue(bankInfo[0].bankCode, bankInfo[0].bankAccountNumber)
        );
      }
      setAgentAvailableCredit(agentAC);
    };

    fetchMemberData();
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

  const validateForm = (params: AddDepositManualModel) => {
    const validationMessages: ResultMessage[] = [];

    if (params.custId <= 0) {
      validationMessages.push({ type: 'error', message: "Please select member login" });
    }
    if (!params.paymentFrom) {
      validationMessages.push({ type: 'error', message: "Please select PaymentFrom" });
    }
    if (!params.paymentTo) {
      validationMessages.push({ type: 'error', message: "Please select PaymentTo" });
    }
    if (params.amount <= 0 || params.amount > agentAvailableCredit) {
      validationMessages.push({ type: 'error', message: "Amount must be greater than 0 and less than MaxLimit" });
    }
    if (!IsValidFile(params.image)) {
      validationMessages.push({ type: 'error', message: "Image must be less than 1MB" });
    }

    return validationMessages;
  };

  const IsValidFile = (file: File) => {
    if (file == null) return true;
    const maxFileSize = 1 * 1024 * 1024;
    return file.size <= maxFileSize;
  }

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    const params = getParams();
    
    const messages = validateForm(params);
    setValidationMessages(messages);
    if (messages.length > 0) {
      return;
    }

    setIsSubmitting(true);
    try {
      const response = await pendingDepositAPI.createDepositManual(params);
      setValidationMessages([]);
      if (response.isSuccessful) {
        setValidationMessages([{
          type: 'success',
          message: t('successMessages.depositSubmitted'),
        }]);
        setTimeout(() => {
          onClose(true);
        }, 3000);
      } else {
        setValidationMessages([{
          type: 'error',
          message: response.message || t('errorMessages.failedToSubmit'),
        }]);
        setIsSubmitting(false);
      }
    } catch (error) {
      setValidationMessages([{
        type: 'error',
        message: t('errorMessages.failedToSubmit'),
      }]);
      setIsSubmitting(false);
    }
  };

  const handleSetDepositAmount = (value: number) => {
    setValidationMessages([]);
    setDepositAmount(value);
  };

  const handlePaymentFromChange = (selected: any) => {
    setValidationMessages([]);
    setPaymentFrom(String(selected.value));
  };

  const handlePaymentToChange = (selected: any) => {
    setValidationMessages([]);
    setPaymentTo(String(selected.value));
  };

  return (
    <>
      {initialLoading ? (        
          <LoadingFullComponent />    
      ) : (
        <div className="py-[12px]">
          <div className="flex items-center justify-flex-start mb-[16px] pt-[8px] border-[#cfcfcf] border-dotted border-b-[2px]">
            <BackButtonComponent onClick={onClose} />
            <TitleComponent title={t('manualDepositForm.title')} />  
          </div>

          <form id="manual-deposit-form" className="w-[1100px] bg-[#ffffff] p-[16px] rounded-[5px] shadow-[0_1px_5px_#0003,0_2px_2px_#00000024,0_3px_1px_-2px_#0000001f] max-[768px]:w-full" onSubmit={handleSubmit}>
            {validationMessages.length > 0 && validationMessages.map((message, index) => (
              <ResultMessageComponent
                key={index}
                message={message.message}
                type={message.type}
                className="mb-[16px] max-[768px]:mb-[0]"
              />
            ))}
            <div className="flex max-[768px]:flex-col">
              <AccountInformation
                memberLoginOptions={memberLoginOptions}
                memberId={memberId}
                onMemberChange={(selected) => setMemberId(Number(selected.value))}
                paymentFrom={paymentFrom}
                onPaymentFromChange={handlePaymentFromChange}
                cashMemberOptions={cashMemberOptions}
                paymentTo={paymentTo}
                onPaymentToChange={handlePaymentToChange}
                cashAgentOptions={cashAgentOptions}
                depositAmount={depositAmount}
                onDepositAmountChange={(e) => handleSetDepositAmount(e)}
                agentAvailableCredit={agentAvailableCredit}
                remarks={remarks}
                onRemarksChange={(e) => setRemarks(e.target.value)}
              />

              <div className="pl-[32px] max-[768px]:pl-0 max-[768px]:mt-[16px]">
                <UploadImage setImage={setImage} />                
              </div>
            </div>
            <div className="flex gap-[16px]">
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
          </form>
        </div>
       
      )}
    </>
  );
};

export default ManualAddDepositForm;
