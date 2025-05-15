import * as React from 'react';
import { useTranslation } from 'react-i18next';
import { paymentProviderService } from '../../services/paymentsAPI';
import { IconClose, } from '../../components/Icons';
import { ResultMessageComponent } from '../../components/ResultMessageComponent';


interface SetDailyLimitPopupModalProps {
  onClose: () => void;
  onSave: (dailyLimit: number) => void;
}

const SetDailyLimitPopupModal: React.FC<SetDailyLimitPopupModalProps> = ({ onClose, onSave }) => {
  const { t } = useTranslation(['managePayments', 'global']);
  const [dailyLimit, setDailyLimit] = React.useState<number>(0);
  const [isLoading, setIsLoading] = React.useState<boolean>(false);
  const [error, setError] = React.useState<string>('');
  const [successMessage, setSuccessMessage] = React.useState<string>('');

  React.useEffect(() => {
    const loadDailyWithdrawalCount = async () => {
      try {
        const count = await paymentProviderService.getDailyWithdrawalCount();
        setDailyLimit(count);
      } catch (err) {
        setError(t('global:errorOccurred'));
      }
    };

    loadDailyWithdrawalCount();
  }, [t]);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setIsLoading(true);
    setError('');
    setSuccessMessage('');

    try {
      const response = await paymentProviderService.setDailyWithdrawalCount(dailyLimit);
      if (response.isSuccessful) {
        setSuccessMessage(t('managePayments:setLimitSuccess'));
        onSave(dailyLimit);
        setTimeout(() => {
          onClose();
        },3000);
      } else {
        setError(response.message);
      }
    } catch (err) {
      setError(t('global:errorOccurred'));
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className="bg-[#00000066] fixed z-[100] top-0 left-0 right-0 bottom-0 flex items-start justify-center">
      <div className="p-[30px] bg-[#ffffff] mt-[100px] max-w-[400px] rounded-[4px] shadow-[0_2px_4px_-1px_#00000033,0_4px_5px_#00000024,0_1px_10px_#0000001f]">
        <div className="relative">
          <h5 className="text-[16px] text-[var(--text-color-primary)] font-bold leading-[21.6px] tracking-[0.2px] pb-[4px]">{t('managePayments:setDailyWithdrawalCount')}</h5>
          <button type="button" className="absolute top-[-20px] right-[-20px] cursor-pointer" onClick={onClose}>
           <IconClose />
          </button>
        </div>
        <div className='my-[8px]'>
          { error && <ResultMessageComponent type="error" message={error} />}
          { successMessage && <ResultMessageComponent type="success" message={successMessage} />}
        </div>
        <form onSubmit={handleSubmit}>
          <div className="mb-[16px]">
            <div className=" ">
              <div className="pb-[8px] mb-[20px] text-[12px] leading-[14px] block">{t('managePayments:setMaxDailyWithdrawalCountForYourMembers')} </div>
              <div>
                <label htmlFor="dailyLimit" className="pb-[8px] text-[12px] leading-[14px] block">{t('managePayments:dailyWithdrawalCount')}</label>
              </div>
              <input
                type="number"
                className="h-[32px] max-[768px]:h-[40px] w-[100%] border-[1px] border-[var(--border-control)] rounded-[4px] px-[8px] text-[12px] focus:outline-none focus:border-[var(--button-border)]"
                id="dailyLimit"
                value={dailyLimit}
                onChange={(e) => {
                  const value = e.target.value;
                  if (value === '' || /^\d+$/.test(value)) {
                    setDailyLimit(value === '' ? 0 : Number(value));
                  }
                }}
                min="0"
                required
              />              
            </div>
          </div>
          <div className="flex justify-center">
            <button type="button" className="w-[140px] h-[30px] rounded-[20px] text-[var(--button-text-cancel)] px-[16px] py-[4px] text-[12px] border-[1px] border-[var(--button-border)] mx-[4px] bg-[var(--button-background-cancel)] hover:bg-[var(--button-background-cancel-hover)] cursor-pointer " onClick={onClose}>
              {t('global:cancel')}
            </button>
            <button type="submit" className="w-[140px] h-[30px] rounded-[20px] text-[var(--button-text)] px-[16px] py-[4px] text-[12px] border-[1px] border-[var(--button-border)] mx-[4px] bg-[var(--button-background)] hover:bg-[var(--button-background-hover)] hover:border-[var(--button-border-hover)] cursor-pointer  shadow-[0_1px_5px_#0003,0_2px_2px_#00000024,0_3px_1px_-2px_#0000001f]" disabled={isLoading}>
              {isLoading ? t('global:loading') : t('global:save')}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};

export default SetDailyLimitPopupModal; 