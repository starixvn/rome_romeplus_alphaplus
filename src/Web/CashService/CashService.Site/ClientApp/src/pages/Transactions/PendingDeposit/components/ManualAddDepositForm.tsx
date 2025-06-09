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
import { renderBankOptionValue } from '../../../../utils/commonFunctions';

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
  const [validationMessage, setValidationMessage] = useState<{
    type: 'error' | 'success' | 'warning';
    message: string;
  } | null>(null);


  useEffect(() => {
    const fetchInitialData = async () => {
      try {
        setValidationMessage(null);
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
        setValidationMessage({
          type: 'error',
          message: t('errorMessages.failedToFetchData')
        });
      } finally {
        setInitialLoading(false);
      }
    };

    fetchInitialData();
  }, []);

  useEffect(() => {
    const fetchMemberData = async () => {
      if (!memberId) return;
      
      const [bankInfo, agentAC] = await Promise.all([
        transactionAPI.getCashMemberBankInfo(memberId),
        pendingDepositAPI.getLimitAmtDeposit(memberId)
      ]);

      setCashMemberOptions(bankInfo);
      setPaymentFrom(
        renderBankOptionValue(bankInfo[0]?.bankCode, bankInfo[0]?.bankAccountNumber)
      );
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

  const validateForm = ():boolean => {
    if (!memberId || memberId <= 0) {
      setValidationMessage({
        type: 'error',
        message: t('manualDepositForm.memberLoginNameRequired'),
      });
      return false;
    }

    if (!paymentFrom) {
      setValidationMessage({
        type: 'error',
        message: t('manualDepositForm.agentBankSelectionRequired'),
      });
      return false;
    }

    if (!paymentTo) {
      setValidationMessage({
        type: 'error',
        message: t('manualDepositForm.memberBankSelectionRequired'),
      });
      return false;
    }

    if (depositAmount <= 0 || depositAmount > agentAvailableCredit) {
      setValidationMessage({
        type: 'error',
        message: t('manualDepositForm.amountMustBeGreaterThan0AndLessThanMaxLimit')
      });
      return false;
    }

    if (image && image.size > 1024 * 1024) {
      setValidationMessage({
        type: 'error',
        message: t('manualDepositForm.imageSizeError')
      });
      return false;
    }

    const allowedTypes = ['image/jpeg', 'image/jpg', 'image/png'];
    if (image && !allowedTypes.includes(image.type)) {
      setValidationMessage({
        type: 'error',
        message: t('manualDepositForm.invalidFileType')
      });
      return false;
    }

    return true;
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    const isValid = validateForm();
    if (!isValid) {
      return;
    }
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
        setIsSubmitting(false);
      }
    } catch (error) {
      setValidationMessage({
        type: 'error',
        message: t('errorMessages.failedToSubmit'),
      });
      setIsSubmitting(false);
    }
  };
  
  const handleSetDepositAmount = (value: number) => {
    setDepositAmount(value);
  };

  const handlePaymentFromChange = (selected: any) => {
    setPaymentFrom(String(selected.value));
  };

  const handlePaymentToChange = (selected: any) => {
    setPaymentTo(String(selected.value));
  };

  return (
    <>
      {initialLoading ? (        
          <LoadingFullComponent />    
      ) : (
        <div className="">
          <div className="flex items-center justify-flex-start mb-[8px] py-[8px] border-[#cfcfcf] border-b-[1px] [&>:nth-child(2)]:border-0 [&>:nth-child(2)]:mb-[0] [&>:nth-child(1)]:mb-[0]">
            <BackButtonComponent onClick={onClose} />
            <TitleComponent title={t('manualDepositForm.title')} />  
          </div>

          <div id="manual-deposit-form" className="w-[1100px] bg-[#ffffff] p-[16px] rounded-[5px] shadow-[0_1px_5px_#0003,0_2px_2px_#00000024,0_3px_1px_-2px_#0000001f] max-[768px]:w-full">
            {validationMessage && (
              <ResultMessageComponent
                message={validationMessage.message}
                type={validationMessage.type}
                className="mb-[16px] max-[768px]:mb-[0]"
              />
            )}
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
                    text={t('manualDepositForm.cancel')}
                    onClick={() => onClose(false)}
                  />
                  <ButtonApplyComponent
                    text={
                      isSubmitting
                        ? t('manualDepositForm.approving')
                        : t('manualDepositForm.approve')
                    }
                    disabled={isSubmitting}
                    onClick={handleSubmit}
                  />
                </div>
          </div>
        </div>
       
      )}
    </>
  );
};

export default ManualAddDepositForm;
