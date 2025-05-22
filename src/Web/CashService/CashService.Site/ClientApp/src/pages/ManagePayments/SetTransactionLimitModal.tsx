import React, { useState, useEffect } from 'react';
import { useTranslation } from 'react-i18next';
import { paymentProviderService } from '../../services/paymentsAPI';
import { IconClose } from '../../components/Icons';
import { ErrorMessageComponent, SuccessMessageComponent, ValidateMessageComponent } from '../../components/MessageComponents';
import { ensureIntegerInput, ensureDecimalInput } from '../../utils/inputHandlers';
import { ButtonApplyComponent, ButtonCancelComponent } from '../../components/ButtonComponents';

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

const SetTransactionLimitModal: React.FC<SetTransactionLimitModalProps> = ({
  onClose,
  onSave
}) => {
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
    dailyWithdrawalCount: ''
  });
  const [originalFormState, setOriginalFormState] = useState<FormState>({
    minDeposit: '',
    maxDeposit: '',
    minWithdrawal: '',
    maxWithdrawal: '',
    dailyWithdrawalCount: ''
  });
  const [fieldErrors, setFieldErrors] = useState<FormErrors>({
    minDeposit: '',
    maxDeposit: '',
    minWithdrawal: '',
    maxWithdrawal: '',
    dailyWithdrawalCount: ''
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

  useEffect(() => {
    const fetchTransactionLimit = async () => {
      try {
        const limit = await paymentProviderService.getTransactionLimit();
        const newFormState = {
          minDeposit: limit.minPerDepositTransaction.toString(),
          maxDeposit: limit.maxPerDepositTransaction.toString(),
          minWithdrawal: limit.minPerWithdrawalTransaction.toString(),
          maxWithdrawal: limit.maxPerWithdrawalTransaction.toString(),
          dailyWithdrawalCount: limit.dailyWithdrawalCount.toString()
        };
        setFormState(newFormState);
        setOriginalFormState(newFormState);
      } catch (err) {
        setError(tManagePayments('failedToFetchLimit'));
        console.error('Error fetching transaction limit:', err);
      }
    };

    fetchTransactionLimit();
  }, [tManagePayments]);

  const handleInputChange = (field: keyof FormState, value: string) => {
    setFormState(prev => ({
      ...prev,
      [field]: value
    }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError(null);
    setLoading(true);
    setFieldErrors({
      minDeposit: '',
      maxDeposit: '',
      minWithdrawal: '',
      maxWithdrawal: '',
      dailyWithdrawalCount: ''
    });

    if (!hasFormChanged()) {
      setError(tGlobal('thereNoChangeToUpdate'));
      setLoading(false);
      return;
    }

    // Validate min < max
    const minDepositNum = parseFloat(formState.minDeposit) || 0;
    const maxDepositNum = parseFloat(formState.maxDeposit) || 0;
    const minWithdrawalNum = parseFloat(formState.minWithdrawal) || 0;
    const maxWithdrawalNum = parseFloat(formState.maxWithdrawal) || 0;
    var hasError = false;
    if(minDepositNum <= 0) {
      setFieldErrors(prev => ({
        ...prev,
        minDeposit: tGlobal('valueMustBeGreaterThanZero')
      }));

      hasError = true;
    }

    if(maxDepositNum <= 0) {
      setFieldErrors(prev => ({
        ...prev,
        maxDeposit: tGlobal('valueMustBeGreaterThanZero')
      }));

      hasError = true;
    }
    
    if(minWithdrawalNum <= 0) {
      setFieldErrors(prev => ({
        ...prev,
        minWithdrawal: tGlobal('valueMustBeGreaterThanZero')
      }));

      hasError = true;
    } 

    if(maxWithdrawalNum <= 0) {
      setFieldErrors(prev => ({
        ...prev,
        maxWithdrawal: tGlobal('valueMustBeGreaterThanZero')
      }));

      hasError = true;
    }

    if (minDepositNum > maxDepositNum) {
      setFieldErrors(prev => ({
        ...prev,
        minDeposit: tManagePayments('minDepositMustBeLessThanOrEqualMaxDeposit'),
        maxDeposit: tManagePayments('minDepositMustBeLessThanOrEqualMaxDeposit')
      }));

      hasError = true;
    }

    if (minWithdrawalNum > maxWithdrawalNum) {
      setFieldErrors(prev => ({
        ...prev,
        minWithdrawal: tManagePayments('minWithdrawalMustBeLessThanOrEqualMaxWithdrawal'),
        maxWithdrawal: tManagePayments('minWithdrawalMustBeLessThanOrEqualMaxWithdrawal')
      }));

      hasError = true;
    }

    if(parseInt(formState.dailyWithdrawalCount) < 1 || parseInt(formState.dailyWithdrawalCount) > 32767) {
      setFieldErrors(prev => ({
        ...prev,
        dailyWithdrawalCount: tManagePayments('dailyWithdrawalCountOutOfRange')
      }));
      hasError = true;
    }

    if (hasError) {
      setLoading(false);
      return;
    }

    try {
      const result = await paymentProviderService.setTransactionLimit({
        dailyWithdrawalCount: parseInt(formState.dailyWithdrawalCount) || 0,
        minPerDepositTransaction: parseFloat(formState.minDeposit) || 0,
        maxPerDepositTransaction: parseFloat(formState.maxDeposit) || 0,
        minPerWithdrawalTransaction: parseFloat(formState.minWithdrawal) || 0,
        maxPerWithdrawalTransaction: parseFloat(formState.maxWithdrawal) || 0
      });

      if (result.isSuccessful) {  
        setIsBlockAction(true);
        setSuccessMessage(tManagePayments('transactionLimitUpdated'));
        setTimeout(() => {
          onSave();
        }, 3000);
      } else {
        setError(result.message || tManagePayments('failedToSetLimit'));
      }
    } catch (err) {
      setError(tManagePayments('failedToSetLimit'));
      console.error('Error setting transaction limit:', err);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="bg-[#00000066] fixed z-[100] top-0 left-0 right-0 bottom-0 flex items-start justify-center" tabIndex={-1}>
      <div className="p-[30px] bg-[#ffffff] mt-[100px] max-w-[540px] max-[768px]:max-w-[90%] rounded-[4px] shadow-[0_2px_4px_-1px_#00000033,0_4px_5px_#00000024,0_1px_10px_#0000001f]">
        <div className="relative">
            <h5 className="text-[16px] text-[var(--text-color-primary)] font-semibold leading-[21.6px] max-[768px]:text-[20px] max-[768px]:leading-[26px]">{tManagePayments('transactionLimitPreferences')}</h5>
            <button
              type="button"
              className="absolute top-[-20px] right-[-20px] cursor-pointer"
              onClick={onClose}
              disabled={loading}
            >
              <IconClose />
            </button>
          </div>
          <form onSubmit={handleSubmit}>
            <div className="mb-[16px]">
              <div className='my-[8px]'>      
                <ErrorMessageComponent message={error} />
                <SuccessMessageComponent message={successMessage} />
              </div>
              <div className="pt-[4px]">
                 {tManagePayments('customizeTransactionLimitPreferences')}
              </div>
              <div className="text-[var(--text-color-primary)] my-[8px] font-semibold">
                {tManagePayments('deposit')}
              </div>
              <div className='flex flex-nowrap gap-[16px] mb-[4px] max-[768px]:justify-between max-[768px]:gap-[0] max-[768px]:flex-wrap w-full'>
              <div className="w-[230px] pt-[4px] max-[768px]:w-[48%]">
                <label className="block font-bold text-[#333333] my-[8px]">{tManagePayments('minPerTransaction')}</label>
                <input
                  type="text"
                  className={`h-[32px] max-[768px]:h-[40px] w-[100%] border-[1px] rounded-[4px] px-[8px]  focus:outline-none ${
                    fieldErrors.minDeposit 
                      ? 'border-[#ff0000] focus:border-[#ff0000]' 
                      : 'border-[var(--border-control)] focus:border-[var(--button-border)]'
                  }`}
                  value={formState.minDeposit}
                  onChange={(e) => handleInputChange('minDeposit', ensureDecimalInput(e).target.value)}
                  min="0" 
                  step="0.01"
                  required
                  maxLength={12}
                />
                <ValidateMessageComponent message={fieldErrors.minDeposit} />
              </div>
              <div className="w-[230px] pt-[4px] max-[768px]:w-[48%]">
                <label className="block font-bold text-[#333333] my-[8px]">{tManagePayments('maxPerTransaction')}</label>
                <input
                  type="text"
                  className={`h-[32px] max-[768px]:h-[40px] w-[100%] border-[1px] rounded-[4px] px-[8px]  focus:outline-none ${
                    fieldErrors.maxDeposit 
                      ? 'border-[#ff0000] focus:border-[#ff0000]' 
                      : 'border-[var(--border-control)] focus:border-[var(--button-border)]'
                  }`}
                  value={formState.maxDeposit}
                  onChange={(e) => handleInputChange('maxDeposit', ensureDecimalInput(e).target.value)}
                  min="0"
                  step="0.01"
                  required
                  maxLength={12}
                />
                <ValidateMessageComponent message={fieldErrors.maxDeposit} />
              </div>
              </div>
              <div className="text-[var(--text-color-primary)] my-[8px] font-semibold">{tManagePayments('withdrawal')}</div>
              <div className='flex flex-nowrap gap-[16px] mb-[4px] max-[768px]:justify-between max-[768px]:gap-[0] max-[768px]:flex-wrap w-full'>
              <div className="w-[230px] pt-[4px] max-[768px]:w-[48%]">
                <label className="block font-bold text-[#333333] my-[8px]">{tManagePayments('minPerTransaction')}</label>
                <input
                  type="text"
                  className={`h-[32px] max-[768px]:h-[40px] w-[100%] border-[1px] rounded-[4px] px-[8px]  focus:outline-none ${
                    fieldErrors.minWithdrawal 
                      ? 'border-[#ff0000] focus:border-[#ff0000]' 
                      : 'border-[var(--border-control)] focus:border-[var(--button-border)]'
                  }`}
                  value={formState.minWithdrawal}
                  onChange={(e) => handleInputChange('minWithdrawal', ensureDecimalInput(e).target.value)}
                  min="0"
                  step="0.01"
                  required
                  maxLength={12}
                />
                <ValidateMessageComponent message={fieldErrors.minWithdrawal} />
              </div>
              <div className="w-[230px] pt-[4px] max-[768px]:w-[48%]">
                <label className="block font-bold text-[#333333] my-[8px]">{tManagePayments('maxPerTransaction')}</label>
                <input
                  type="text"
                  className={`h-[32px] max-[768px]:h-[40px] w-[100%] border-[1px] rounded-[4px] px-[8px]  focus:outline-none ${
                    fieldErrors.maxWithdrawal 
                      ? 'border-[#ff0000] focus:border-[#ff0000]' 
                      : 'border-[var(--border-control)] focus:border-[var(--button-border)]'
                  }`}
                  value={formState.maxWithdrawal}
                  onChange={(e) => handleInputChange('maxWithdrawal', ensureDecimalInput(e).target.value)}
                  min="0"
                  step="0.01"
                  required
                  maxLength={12}
                />
                <ValidateMessageComponent message={fieldErrors.maxWithdrawal} />
              </div>
              </div>
              <div className='flex flex-nowrap gap-[16px] mb-[4px] max-[768px]:justify-between max-[768px]:gap-[0] max-[768px]:flex-wrap w-full'>
              <div className="w-[230px] pt-[4px] max-[768px]:w-[48%]">
                <label className="block font-bold text-[#333333] my-[8px]">{tManagePayments('dailyWithdrawalCount')}</label>
                <input
                  type="text"
                  className="h-[32px] max-[768px]:h-[40px] w-[100%] border-[1px] border-[var(--border-control)] rounded-[4px] px-[8px]  focus:outline-none focus:border-[var(--button-border)]"
                  value={formState.dailyWithdrawalCount}
                  onChange={(e) => handleInputChange('dailyWithdrawalCount', ensureIntegerInput(e).target.value)}
                  min="0"
                  maxLength={5}
                  required
                />
                <ValidateMessageComponent message={fieldErrors.dailyWithdrawalCount} />
                <label className=" block mt-[4px] text-[#828282] text-[11px] max-[768px]:text-[15px] font-semibold">{tManagePayments('dailyWithdrawalCountTip')}</label>
              </div>
              
              </div>
            </div>
            <div className="flex justify-center gap-[8px]">    
              <ButtonCancelComponent text={tGlobal('cancel')} onClick={onClose}  />   
              <ButtonApplyComponent text={loading ? tGlobal('saving') : tGlobal('save')} onClick={() => handleSubmit({ preventDefault: () => {} } as React.FormEvent)} disabled={loading || isBlockAction} />
            </div>
          </form>
        </div>
      </div>
 
  );
};

export default SetTransactionLimitModal; 