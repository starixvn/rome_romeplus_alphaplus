import React, { useState, useEffect } from 'react';
import { useTranslation } from 'react-i18next';
import { paymentProviderService } from '../../services/paymentsAPI';
import { IconClose } from '../../components/Icons';
import { ResultMessage } from '../../components/ResultMessage';
import { handleNumericInput, handleIntegerInput } from '../../utils/inputHandlers';

interface SetTransactionLimitModalProps {
  onClose: () => void;
  onSave: () => void;
}

const SetTransactionLimitModal: React.FC<SetTransactionLimitModalProps> = ({
  onClose,
  onSave
}) => {
  const { t: tManagePayments } = useTranslation('managePayments');
  const { t: tGlobal } = useTranslation('global');
  
  const [minDeposit, setMinDeposit] = useState<string>('');
  const [maxDeposit, setMaxDeposit] = useState<string>('');
  const [minWithdrawal, setMinWithdrawal] = useState<string>('');
  const [maxWithdrawal, setMaxWithdrawal] = useState<string>('');
  const [dailyWithdrawalCount, setDailyWithdrawalCount] = useState<string>('');
  const [error, setError] = useState<string | null>(null);
  const [success, setSuccess] = useState<boolean>(false);
  const [successMessage, setSuccessMessage] = useState<string>('');
  const [loading, setLoading] = useState(false);
  const [fieldErrors, setFieldErrors] = useState<{
    minDeposit: boolean;
    maxDeposit: boolean;
    minWithdrawal: boolean;
    maxWithdrawal: boolean;
  }>({
    minDeposit: false,
    maxDeposit: false,
    minWithdrawal: false,
    maxWithdrawal: false
  });

  useEffect(() => {
    const fetchTransactionLimit = async () => {
      try {
        const limit = await paymentProviderService.getTransactionLimit();
        setMinDeposit(limit.minPerDepositTransaction.toString());
        setMaxDeposit(limit.maxPerDepositTransaction.toString());
        setMinWithdrawal(limit.minPerWithdrawalTransaction.toString());
        setMaxWithdrawal(limit.maxPerWithdrawalTransaction.toString());
        setDailyWithdrawalCount(limit.dailyWithdrawalCount.toString());
      } catch (err) {
        setError(tManagePayments('failedToFetchLimit'));
        console.error('Error fetching transaction limit:', err);
      }
    };

    fetchTransactionLimit();
  }, [tManagePayments]);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError(null);
    setSuccess(false);
    setLoading(true);
    setFieldErrors({
      minDeposit: false,
      maxDeposit: false,
      minWithdrawal: false,
      maxWithdrawal: false
    });

    // Validate min < max
    const minDepositNum = parseFloat(minDeposit) || 0;
    const maxDepositNum = parseFloat(maxDeposit) || 0;
    const minWithdrawalNum = parseFloat(minWithdrawal) || 0;
    const maxWithdrawalNum = parseFloat(maxWithdrawal) || 0;
    var hasError = false;
    var errorMessage = '';
    if (minDepositNum > maxDepositNum) {
      errorMessage = tManagePayments('minDepositMustBeLessThanOrEqualMaxDeposit');
      setFieldErrors(prev => ({
        ...prev,
        minDeposit: true,
        maxDeposit: true
      }));
      setLoading(false);
      hasError = true;
    }

    if (minWithdrawalNum > maxWithdrawalNum) {
      errorMessage += "<br />" + tManagePayments('minWithdrawalMustBeLessThanOrEqualMaxWithdrawal');
      setFieldErrors(prev => ({
        ...prev,
        minWithdrawal: true,
        maxWithdrawal: true
      }));
      setLoading(false);
      hasError = true;
    }

    if (hasError) {
      setError(errorMessage);
      return;
    }

    try {
      const result = await paymentProviderService.setTransactionLimit({
        dailyWithdrawalCount: parseInt(dailyWithdrawalCount) || 0,
        minPerDepositTransaction: parseFloat(minDeposit) || 0,
        maxPerDepositTransaction: parseFloat(maxDeposit) || 0,
        minPerWithdrawalTransaction: parseFloat(minWithdrawal) || 0,
        maxPerWithdrawalTransaction: parseFloat(maxWithdrawal) || 0
      });

      if (result.isSuccessful) {
        setSuccess(true);
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
      <div className="p-[30px] bg-[#ffffff] mt-[100px] max-w-[500px] rounded-[4px] shadow-[0_2px_4px_-1px_#00000033,0_4px_5px_#00000024,0_1px_10px_#0000001f]">
        <div className="relative">
            <h5 className="text-[16px] text-[var(--text-color-primary)] font-bold leading-[21.6px] tracking-[0.2px] pb-[4px]">{tManagePayments('transactionLimitPreferences')}</h5>
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
            <div className="text-[12px] leading-[14px] mb-[16px]">
              <div className='my-[8px]'>      
                { error && <ResultMessage type="error" message={error} />}
                { success && <ResultMessage type="success" message={successMessage} />}   
              </div>
              <div className="pt-[4px]">
                 {tManagePayments('customizeTransactionLimitPreferences')}
              </div>
              <div className="text-[var(--text-color-primary)] my-[8px] font-semibold">
                {tManagePayments('deposit')}
              </div>
              <div className='flex justify-between'>
              <div className="w-[48%]">
                <label className="block font-bold text-[#333333] my-[8px]">{tManagePayments('minPerTransaction')}</label>
                <input
                  type="text"
                  className={`h-[32px] max-[768px]:h-[40px] w-[100%] border-[1px] rounded-[4px] px-[8px] text-[12px] focus:outline-none ${
                    fieldErrors.minDeposit 
                      ? 'border-[#ff0000] focus:border-[#ff0000]' 
                      : 'border-[var(--border-control)] focus:border-[var(--button-border)]'
                  }`}
                  value={minDeposit}
                  onChange={(e) => handleNumericInput(e.target.value, setMinDeposit)}
                  min="0"
                  step="0.01"
                  required
                />
              </div>
              <div className="w-[48%]">
                <label className="block font-bold text-[#333333] my-[8px]">{tManagePayments('maxPerTransaction')}</label>
                <input
                  type="text"
                  className={`h-[32px] max-[768px]:h-[40px] w-[100%] border-[1px] rounded-[4px] px-[8px] text-[12px] focus:outline-none ${
                    fieldErrors.maxDeposit 
                      ? 'border-[#ff0000] focus:border-[#ff0000]' 
                      : 'border-[var(--border-control)] focus:border-[var(--button-border)]'
                  }`}
                  value={maxDeposit}
                  onChange={(e) => handleNumericInput(e.target.value, setMaxDeposit)}
                  min="0"
                  step="0.01"
                  required
                />
              </div>
              </div>
              <div className="text-[var(--text-color-primary)] my-[8px] font-semibold">{tManagePayments('withdrawal')}</div>
              <div className='flex justify-between'>
              <div className="w-[48%]">
                <label className="block font-bold text-[#333333] my-[8px]">{tManagePayments('minPerTransaction')}</label>
                <input
                  type="text"
                  className={`h-[32px] max-[768px]:h-[40px] w-[100%] border-[1px] rounded-[4px] px-[8px] text-[12px] focus:outline-none ${
                    fieldErrors.minWithdrawal 
                      ? 'border-[#ff0000] focus:border-[#ff0000]' 
                      : 'border-[var(--border-control)] focus:border-[var(--button-border)]'
                  }`}
                  value={minWithdrawal}
                  onChange={(e) => handleNumericInput(e.target.value, setMinWithdrawal)}
                  min="0"
                  step="0.01"
                  required
                />
              </div>
              <div className="w-[48%]">
                <label className="block font-bold text-[#333333] my-[8px]">{tManagePayments('maxPerTransaction')}</label>
                <input
                  type="text"
                  className={`h-[32px] max-[768px]:h-[40px] w-[100%] border-[1px] rounded-[4px] px-[8px] text-[12px] focus:outline-none ${
                    fieldErrors.maxWithdrawal 
                      ? 'border-[#ff0000] focus:border-[#ff0000]' 
                      : 'border-[var(--border-control)] focus:border-[var(--button-border)]'
                  }`}
                  value={maxWithdrawal}
                  onChange={(e) => handleNumericInput(e.target.value, setMaxWithdrawal)}
                  min="0"
                  step="0.01"
                  required
                />
              </div>
              </div>
              <div className='flex justify-between'>
              <div className="w-[48%]">
                <label className="block font-bold text-[#333333] my-[8px]">{tManagePayments('dailyWithdrawalCount')}</label>
                <input
                  type="text"
                  className="h-[32px] max-[768px]:h-[40px] w-[100%] border-[1px] border-[var(--border-control)] rounded-[4px] px-[8px] text-[12px] focus:outline-none focus:border-[var(--button-border)]"
                  value={dailyWithdrawalCount}
                  onChange={(e) => handleIntegerInput(e.target.value, setDailyWithdrawalCount)}
                  min="0"
                  required
                />
                <label className=" block mt-[4px] text-[#828282] text-[11px] leading-[12px] font-semibold">{tManagePayments('dailyWithdrawalCountTip')}</label>
              </div>
              
              </div>
            </div>
            <div className="flex justify-center">
              <button
                type="button"
                className="w-[140px] h-[30px] rounded-[20px] text-[var(--text-color-primary)] px-[16px] py-[4px] text-[12px] border-[1px] border-[var(--button-border)] mx-[4px] bg-[var(--button-background-cancel)] hover:bg-[var(--button-background-cancel-hover)] cursor-pointer "
                onClick={onClose}
                disabled={loading}
              >
                {tGlobal('cancel')}
              </button>
              <button
                type="submit"
                className="w-[140px] h-[30px] rounded-[20px] text-[#ffffff] px-[16px] py-[4px] text-[12px] border-[1px] border-[var(--button-border)] mx-[4px] bg-[var(--button-background)] hover:bg-[var(--button-background-hover)] hover:border-[var(--button-border-hover)] cursor-pointer shadow-[0_1px_5px_#0003,0_2px_2px_#00000024,0_3px_1px_-2px_#0000001f]"
                disabled={loading}
              >
                {loading ? tGlobal('saving') : tGlobal('save')}
              </button>
            </div>
          </form>
        </div>
      </div>
 
  );
};

export default SetTransactionLimitModal; 