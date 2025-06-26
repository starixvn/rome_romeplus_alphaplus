import React, { useState, useEffect } from 'react';
import { useTranslation } from 'react-i18next';
import { transactionAPI } from '../../../../services/transactionsHistoryAPI';
import { MemberLoginName } from '../../../../services/interfaces/Transactions/MemberLoginName';
import { BankInfo } from '../../../../services/interfaces/Transactions/BankInfo';
import pendingDepositAPI from '../../../../services/pendingDepositAPI';
import UploadImage from '../../Components/UploadImage';
import { AddDepositManualModel } from '../../../../services/interfaces/AddDepositManualModel';
import { LoadingFullComponent } from '../../../../components/LoadingComponent';
import { ResultMessageComponent } from '../../../../components/Message';
import AccountInformation from './AccountInformation';
import { TitleComponent } from '../../../../components/Title';
import {
  ButtonCancelComponent,
  ButtonApplyComponent,
  BackButtonComponent,
} from '../../../../components/Buttons/index';
import { renderBankOptionValue } from '../../../../utils/commonFunctions';

interface ManualAddDepositFormProps {
  cashAgentOptions: BankInfo[];
  onClose: (isSuccess: boolean) => void;
}

interface FormState {
  memberLoginOptions: MemberLoginName[];
  memberId: number;
  paymentFrom: string;
  paymentTo: string;
  cashMemberOptions: BankInfo[];
  agentAvailableCredit: number;
  depositAmount: number;
  remarks: string;
  image: File | null;
}

interface StatusState {
  initialLoading: boolean;
  isSubmitting: boolean;
  validationMessage: {
    type: 'error' | 'success' | 'warning';
    message: string;
  } | null;
}

const Header: React.FC<{ onClose: () => void; title: string }> = ({ onClose, title }) => (
  <div className="flex items-center justify-flex-start mb-[8px] py-[8px] border-[var(--border-control)] border-b-[1px] [&>:nth-child(2)]:border-0 [&>:nth-child(2)]:mb-[0] [&>:nth-child(1)]:mb-[0]">
    <BackButtonComponent onClick={onClose} />
    <TitleComponent title={title} />
  </div>
);

const FormActions: React.FC<{
  onCancel: () => void;
  onSubmit: (e: React.FormEvent) => void;
  isSubmitting: boolean;
  cancelText: string;
  submitText: string;
  submittingText: string;
}> = ({ onCancel, onSubmit, isSubmitting, cancelText, submitText, submittingText }) => (
  <div className="flex gap-[16px]">
    <ButtonCancelComponent text={cancelText} onClick={onCancel} />
    <ButtonApplyComponent
      text={isSubmitting ? submittingText : submitText}
      disabled={isSubmitting}
      onClick={onSubmit}
    />
  </div>
);

const ManualAddDepositForm: React.FC<ManualAddDepositFormProps> = ({
  cashAgentOptions,
  onClose,
}) => {
  const { t } = useTranslation('pendingDeposit');

  const [formState, setFormState] = useState<FormState>({
    memberLoginOptions: [],
    memberId: 0,
    paymentFrom: '',
    paymentTo: renderBankOptionValue(
      cashAgentOptions[0]?.bankCode,
      cashAgentOptions[0]?.bankAccountNumber
    ),
    cashMemberOptions: [],
    agentAvailableCredit: 0,
    depositAmount: 0,
    remarks: '',
    image: null,
  });

  const [status, setStatus] = useState<StatusState>({
    initialLoading: true,
    isSubmitting: false,
    validationMessage: null,
  });

  const updateFormState = (updates: Partial<FormState>) => {
    setFormState(prev => ({ ...prev, ...updates }));
  };

  const updateStatus = (updates: Partial<StatusState>) => {
    setStatus(prev => ({ ...prev, ...updates }));
  };

  const fetchMemberData = async (selectedMemberId: number) => {
    try {
      const [bankInfo, agentAC] = await Promise.all([
        transactionAPI.getCashMemberBankInfo(selectedMemberId),
        pendingDepositAPI.getLimitAmtDeposit(selectedMemberId)
      ]);

      updateFormState({
        cashMemberOptions: bankInfo,
        paymentFrom: renderBankOptionValue(bankInfo[0]?.bankCode, bankInfo[0]?.bankAccountNumber),
        agentAvailableCredit: agentAC
      });
    } catch (error) {
      updateStatus({
        validationMessage: {
          type: 'error',
          message: t('errorMessages.failedToFetchData')
        }
      });
    }
  };

  const validateForm = (): boolean => {
    const validationChecks = [
      {
        condition: !formState.memberId || formState.memberId <= 0,
        message: t('manualDepositForm.memberLoginNameRequired')
      },
      {
        condition: !formState.paymentFrom,
        message: t('manualDepositForm.agentBankSelectionRequired')
      },
      {
        condition: !formState.paymentTo,
        message: t('manualDepositForm.memberBankSelectionRequired')
      },
      {
        condition: formState.depositAmount <= 0 || formState.depositAmount > formState.agentAvailableCredit,
        message: t('manualDepositForm.amountMustBeGreaterThan0AndLessThanMaxLimit')
      },
      {
        condition: formState.image && formState.image.size > 1024 * 1024,
        message: t('manualDepositForm.imageSizeError')
      },
      {
        condition: formState.image && !['image/jpeg', 'image/jpg', 'image/png'].includes(formState.image.type),
        message: t('manualDepositForm.invalidFileType')
      }
    ];

    const failedValidation = validationChecks.find(check => check.condition);
    if (failedValidation) {
      updateStatus({
        validationMessage: {
          type: 'error',
          message: failedValidation.message
        }
      });
      return false;
    }

    return true;
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!validateForm()) return;

    const params: AddDepositManualModel = {
      custId: formState.memberId,
      paymentFrom: formState.paymentFrom,
      paymentTo: formState.paymentTo,
      amount: formState.depositAmount,
      remark: formState.remarks,
      image: formState.image as File,
    };

    updateStatus({ isSubmitting: true });
    try {
      const response = await pendingDepositAPI.createDepositManual(params);
      if (response.isSuccessful) {
        updateStatus({
          validationMessage: {
            type: 'success',
            message: t('successMessages.depositSubmitted')
          }
        });
        setTimeout(() => onClose(true), 3000);
      } else {
        throw new Error(response.message);
      }
    } catch (error) {
      updateStatus({
        isSubmitting: false,
        validationMessage: {
          type: 'error',
          message: error instanceof Error ? error.message : t('errorMessages.failedToSubmit')
        }
      });
    }
  };

  useEffect(() => {
    const fetchInitialData = async () => {
      try {
        const memberLoginNames = await transactionAPI.getMemberLoginName();
        updateFormState({ memberLoginOptions: memberLoginNames });

        if (memberLoginNames?.length > 0) {
          const selectedMemberId = memberLoginNames[0].custId;
          updateFormState({ memberId: selectedMemberId });
          await fetchMemberData(selectedMemberId);
        }
      } catch (error) {
        updateStatus({
          validationMessage: {
            type: 'error',
            message: t('errorMessages.failedToFetchData')
          }
        });
      } finally {
        updateStatus({ initialLoading: false });
      }
    };

    fetchInitialData();
  }, []);

  useEffect(() => {
    if (formState.memberId) {
      fetchMemberData(formState.memberId);
    }
  }, [formState.memberId]);

  if (status.initialLoading) {
    return <LoadingFullComponent />;
  }

  return (
    <>
      <Header onClose={() => onClose(false)} title={t('manualDepositForm.title')} />
      <div id="manual-deposit-form" className="max-w-[1100px] w-full max-[768px]:w-full pb-[16px] pt-[10px]">
        {status.validationMessage && (
          <ResultMessageComponent
            message={status.validationMessage.message}
            type={status.validationMessage.type}
            className="mb-[16px] max-[768px]:mb-[0]"
          />
        )}
        <div className="flex justify-between max-[768px]:flex-col mb-[16px]">
          <AccountInformation
            memberLoginOptions={formState.memberLoginOptions}
            memberId={formState.memberId}
            onMemberChange={(selected) => updateFormState({ memberId: Number(selected.value) })}
            paymentFrom={formState.paymentFrom}
            onPaymentFromChange={(selected) => updateFormState({ paymentFrom: String(selected.value) })}
            cashMemberOptions={formState.cashMemberOptions}
            paymentTo={formState.paymentTo}
            onPaymentToChange={(selected) => updateFormState({ paymentTo: String(selected.value) })}
            cashAgentOptions={cashAgentOptions}
            depositAmount={formState.depositAmount}
            onDepositAmountChange={(value) => updateFormState({ depositAmount: value })}
            agentAvailableCredit={formState.agentAvailableCredit}
            remarks={formState.remarks}
            onRemarksChange={(e) => updateFormState({ remarks: e.target.value })}
          />
         
            <UploadImage setImage={(image) => updateFormState({ image })} />
     
        </div>
        <FormActions
          onCancel={() => onClose(false)}
          onSubmit={handleSubmit}
          isSubmitting={status.isSubmitting}
          cancelText={t('manualDepositForm.cancel')}
          submitText={t('manualDepositForm.approve')}
          submittingText={t('manualDepositForm.approving')}
        />
      </div>
    </>
  );
};

export default ManualAddDepositForm;
