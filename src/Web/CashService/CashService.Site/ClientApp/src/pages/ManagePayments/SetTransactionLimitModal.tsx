import React, { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';

import {
  ButtonApplyComponent,
  ButtonCancelComponent,
  ButtonClosePopupComponent,
} from '../../components/ButtonComponents';
import {
  ErrorMessageComponent,
  SuccessMessageComponent,
  ValidateMessageComponent,
} from '../../components/MessageComponents';
import { LabelTitleComponent, MediumTitleComponent } from '../../components/TitleComponents';
import { createDefaultPaymentTransactionLimit } from '../../services/interfaces/Payments';
import { paymentProviderService } from '../../services/paymentsAPI';
import { ensureDecimalInput, ensureIntegerInput } from '../../utils/inputHandlers';

interface SetTransactionLimitModalProps {
  onClose: () => void;
  onSave: () => void;
}

interface FormState {
  minDeposit: string;
  maxDeposit: string;
  minWithdrawal: string;
  maxWithdrawal: string;
  dailyWithdrawalCount: string;
}

interface FormErrors {
  minDeposit: string;
  maxDeposit: string;
  minWithdrawal: string;
  maxWithdrawal: string;
  dailyWithdrawalCount: string;
}

const SetTransactionLimitModal: React.FC<SetTransactionLimitModalProps> = ({ onClose, onSave }) => {
  const { t: tManagePayments } = useTranslation('managePayments');
  const { t: tGlobal } = useTranslation('global');

  const [error, setError] = useState<string | null>(null);
  const [successMessage, setSuccessMessage] = useState<string>('');
  const [loading, setLoading] = useState(false);
  const [isBlockAction, setIsBlockAction] = useState<boolean>(false);

  const [formState, setFormState] = useState<FormState>({
    minDeposit: '',
    maxDeposit: '',
    minWithdrawal: '',
    maxWithdrawal: '',
    dailyWithdrawalCount: '',
  });
  const [originalFormState, setOriginalFormState] = useState<FormState>({
    minDeposit: '',
    maxDeposit: '',
    minWithdrawal: '',
    maxWithdrawal: '',
    dailyWithdrawalCount: '',
  });
  const [fieldErrors, setFieldErrors] = useState<FormErrors>({
    minDeposit: '',
    maxDeposit: '',
    minWithdrawal: '',
    maxWithdrawal: '',
    dailyWithdrawalCount: '',
  });

  const hasFormChanged = (): boolean => {
    return (
      formState.minDeposit !== originalFormState.minDeposit ||
      formState.maxDeposit !== originalFormState.maxDeposit ||
      formState.minWithdrawal !== originalFormState.minWithdrawal ||
      formState.maxWithdrawal !== originalFormState.maxWithdrawal ||
      formState.dailyWithdrawalCount !== originalFormState.dailyWithdrawalCount
    );
  };

  const validateFormState = (): boolean => {
    const minDepositNum = parseFloat(formState.minDeposit) || 0;
    const maxDepositNum = parseFloat(formState.maxDeposit) || 0;
    const minWithdrawalNum = parseFloat(formState.minWithdrawal) || 0;
    const maxWithdrawalNum = parseFloat(formState.maxWithdrawal) || 0;
    let hasError = false;

    // Reset all field errors
    setFieldErrors({
      minDeposit: '',
      maxDeposit: '',
      minWithdrawal: '',
      maxWithdrawal: '',
      dailyWithdrawalCount: '',
    });

    // Validate min deposit
    if (minDepositNum <= 0) {
      setFieldErrors((prev) => ({
        ...prev,
        minDeposit: tGlobal('valueMustBeGreaterThanZero'),
      }));
      hasError = true;
    }

    // Validate max deposit
    if (maxDepositNum <= 0) {
      setFieldErrors((prev) => ({
        ...prev,
        maxDeposit: tGlobal('valueMustBeGreaterThanZero'),
      }));
      hasError = true;
    }

    // Validate min withdrawal
    if (minWithdrawalNum <= 0) {
      setFieldErrors((prev) => ({
        ...prev,
        minWithdrawal: tGlobal('valueMustBeGreaterThanZero'),
      }));
      hasError = true;
    }

    // Validate max withdrawal
    if (maxWithdrawalNum <= 0) {
      setFieldErrors((prev) => ({
        ...prev,
        maxWithdrawal: tGlobal('valueMustBeGreaterThanZero'),
      }));
      hasError = true;
    }

    // Validate min deposit <= max deposit
    if (minDepositNum > maxDepositNum) {
      setFieldErrors((prev) => ({
        ...prev,
        minDeposit: tManagePayments('minDepositMustBeLessThanOrEqualMaxDeposit'),
        maxDeposit: tManagePayments('minDepositMustBeLessThanOrEqualMaxDeposit'),
      }));
      hasError = true;
    }

    // Validate min withdrawal <= max withdrawal
    if (minWithdrawalNum > maxWithdrawalNum) {
      setFieldErrors((prev) => ({
        ...prev,
        minWithdrawal: tManagePayments('minWithdrawalMustBeLessThanOrEqualMaxWithdrawal'),
        maxWithdrawal: tManagePayments('minWithdrawalMustBeLessThanOrEqualMaxWithdrawal'),
      }));
      hasError = true;
    }

    // Validate daily withdrawal count
    if (
      parseInt(formState.dailyWithdrawalCount) < 1 ||
      parseInt(formState.dailyWithdrawalCount) > 32767
    ) {
      setFieldErrors((prev) => ({
        ...prev,
        dailyWithdrawalCount: tManagePayments('dailyWithdrawalCountOutOfRange'),
      }));
      hasError = true;
    }

    return !hasError;
  };

  useEffect(() => {
    const fetchTransactionLimit = async () => {
      const response = await paymentProviderService.getTransactionLimit();
      if (response.error) {
        setError(response.error.message);
        return;
      }

      const limit = response.data || createDefaultPaymentTransactionLimit();
      const newFormState = {
        minDeposit: limit.minPerDepositTransaction.toString(),
        maxDeposit: limit.maxPerDepositTransaction.toString(),
        minWithdrawal: limit.minPerWithdrawalTransaction.toString(),
        maxWithdrawal: limit.maxPerWithdrawalTransaction.toString(),
        dailyWithdrawalCount: limit.dailyWithdrawalCount.toString(),
      };
      setFormState(newFormState);
      setOriginalFormState(newFormState);
    };

    fetchTransactionLimit();
  }, [tManagePayments]);

  const onInputChange = (field: keyof FormState, value: string) => {
    setFormState((prev) => ({
      ...prev,
      [field]: value,
    }));
  };

  const onSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError(null);
    setLoading(true);

    if (!hasFormChanged()) {
      setError(tGlobal('thereNoChangeToUpdate'));
      setLoading(false);
      return;
    }

    if (!validateFormState()) {
      setLoading(false);
      return;
    }

    const response = await paymentProviderService.setTransactionLimit({
      dailyWithdrawalCount: parseInt(formState.dailyWithdrawalCount) || 0,
      minPerDepositTransaction: parseFloat(formState.minDeposit) || 0,
      maxPerDepositTransaction: parseFloat(formState.maxDeposit) || 0,
      minPerWithdrawalTransaction: parseFloat(formState.minWithdrawal) || 0,
      maxPerWithdrawalTransaction: parseFloat(formState.maxWithdrawal) || 0,
    });

    if (response.error) {
      setError(response.error.message);
      setLoading(false);
      return;
    }

    const result = response.data || {
      isSuccessful: false,
      code: 1,
      message: tManagePayments('failedToSetLimit'),
    };

    if (result.isSuccessful) {
      setIsBlockAction(true);
      setSuccessMessage(tManagePayments('transactionLimitUpdated'));
      setTimeout(() => {
        onSave();
      }, 3000);
    } else {
      setError(result.message || tManagePayments('failedToSetLimit'));
    }

    setLoading(false);
  };

  return (
    <div
      className="bg-[#00000066] fixed z-[100] top-0 left-0 right-0 bottom-0 flex items-start justify-center"
      tabIndex={-1}
    >
      <div className="p-[30px] bg-[#ffffff] mt-[100px] max-w-[540px] max-[768px]:max-w-[90%] rounded-[4px] shadow-[0_2px_4px_-1px_#00000033,0_4px_5px_#00000024,0_1px_10px_#0000001f]">
        <div className="relative">
          <MediumTitleComponent text={tManagePayments('transactionLimitPreferences')} />
          <ButtonClosePopupComponent onClick={onClose} />
        </div>
        <form onSubmit={onSubmit}>
          <div className="mb-[16px]">
            <div className="my-[8px]">
              <ErrorMessageComponent message={error} />
              <SuccessMessageComponent message={successMessage} />
            </div>
            <div className="pt-[4px]">
              {tManagePayments('customizeTransactionLimitPreferences')}
            </div>
            <div className="text-[var(--text-color-primary)] my-[8px] font-semibold">
              {tManagePayments('deposit')}
            </div>
            <div className="flex flex-nowrap gap-[16px] mb-[4px] max-[768px]:justify-between max-[768px]:gap-[0] max-[768px]:flex-wrap w-full">
              <div className="w-[230px] pt-[4px] max-[768px]:w-[48%]">
                <LabelTitleComponent text={tManagePayments('minPerTransaction')} />
                <input
                  type="text"
                  className={`h-[32px] max-[768px]:h-[40px] w-[100%] border-[1px] rounded-[4px] px-[8px]  focus:outline-none ${
                    fieldErrors.minDeposit
                      ? 'border-[#ff0000] focus:border-[#ff0000]'
                      : 'border-[var(--border-control)] focus:border-[var(--button-border)]'
                  }`}
                  value={formState.minDeposit}
                  onChange={(e) => onInputChange('minDeposit', ensureDecimalInput(e).target.value)}
                  min="0"
                  step="0.01"
                  required
                  maxLength={12}
                />
                <ValidateMessageComponent message={fieldErrors.minDeposit} />
              </div>
              <div className="w-[230px] pt-[4px] max-[768px]:w-[48%]">
                <LabelTitleComponent text={tManagePayments('maxPerTransaction')} />
                <input
                  type="text"
                  className={`h-[32px] max-[768px]:h-[40px] w-[100%] border-[1px] rounded-[4px] px-[8px]  focus:outline-none ${
                    fieldErrors.maxDeposit
                      ? 'border-[#ff0000] focus:border-[#ff0000]'
                      : 'border-[var(--border-control)] focus:border-[var(--button-border)]'
                  }`}
                  value={formState.maxDeposit}
                  onChange={(e) => onInputChange('maxDeposit', ensureDecimalInput(e).target.value)}
                  min="0"
                  step="0.01"
                  required
                  maxLength={12}
                />
                <ValidateMessageComponent message={fieldErrors.maxDeposit} />
              </div>
            </div>
            <div className="text-[var(--text-color-primary)] my-[8px] font-semibold">
              {tManagePayments('withdrawal')}
            </div>
            <div className="flex flex-nowrap gap-[16px] mb-[4px] max-[768px]:justify-between max-[768px]:gap-[0] max-[768px]:flex-wrap w-full">
              <div className="w-[230px] pt-[4px] max-[768px]:w-[48%]">
                <LabelTitleComponent text={tManagePayments('minPerTransaction')} />
                <input
                  type="text"
                  className={`h-[32px] max-[768px]:h-[40px] w-[100%] border-[1px] rounded-[4px] px-[8px]  focus:outline-none ${
                    fieldErrors.minWithdrawal
                      ? 'border-[#ff0000] focus:border-[#ff0000]'
                      : 'border-[var(--border-control)] focus:border-[var(--button-border)]'
                  }`}
                  value={formState.minWithdrawal}
                  onChange={(e) =>
                    onInputChange('minWithdrawal', ensureDecimalInput(e).target.value)
                  }
                  min="0"
                  step="0.01"
                  required
                  maxLength={12}
                />
                <ValidateMessageComponent message={fieldErrors.minWithdrawal} />
              </div>
              <div className="w-[230px] pt-[4px] max-[768px]:w-[48%]">
                <LabelTitleComponent text={tManagePayments('maxPerTransaction')} />
                <input
                  type="text"
                  className={`h-[32px] max-[768px]:h-[40px] w-[100%] border-[1px] rounded-[4px] px-[8px]  focus:outline-none ${
                    fieldErrors.maxWithdrawal
                      ? 'border-[#ff0000] focus:border-[#ff0000]'
                      : 'border-[var(--border-control)] focus:border-[var(--button-border)]'
                  }`}
                  value={formState.maxWithdrawal}
                  onChange={(e) =>
                    onInputChange('maxWithdrawal', ensureDecimalInput(e).target.value)
                  }
                  min="0"
                  step="0.01"
                  required
                  maxLength={12}
                />
                <ValidateMessageComponent message={fieldErrors.maxWithdrawal} />
              </div>
            </div>
            <div className="flex flex-nowrap gap-[16px] mb-[4px] max-[768px]:justify-between max-[768px]:gap-[0] max-[768px]:flex-wrap w-full">
              <div className="w-[230px] pt-[4px] max-[768px]:w-[48%]">
                <LabelTitleComponent text={tManagePayments('dailyWithdrawalCount')} />
                <input
                  type="text"
                  className="h-[32px] max-[768px]:h-[40px] w-[100%] border-[1px] border-[var(--border-control)] rounded-[4px] px-[8px]  focus:outline-none focus:border-[var(--button-border)]"
                  value={formState.dailyWithdrawalCount}
                  onChange={(e) =>
                    onInputChange('dailyWithdrawalCount', ensureIntegerInput(e).target.value)
                  }
                  min="0"
                  maxLength={5}
                  required
                />
                <ValidateMessageComponent message={fieldErrors.dailyWithdrawalCount} />
                <label className=" block mt-[4px] text-[#828282] text-[11px] max-[768px]:text-[15px] font-semibold">
                  {tManagePayments('dailyWithdrawalCountTip')}
                </label>
              </div>
            </div>
          </div>
          <div className="flex justify-center gap-[8px]">
            <ButtonCancelComponent text={tGlobal('cancel')} onClick={onClose} />
            <ButtonApplyComponent
              text={loading ? tGlobal('saving') : tGlobal('save')}
              onClick={() => onSubmit({ preventDefault: () => {} } as React.FormEvent)}
              disabled={loading || isBlockAction}
            />
          </div>
        </form>
      </div>
    </div>
  );
};

export default SetTransactionLimitModal;
