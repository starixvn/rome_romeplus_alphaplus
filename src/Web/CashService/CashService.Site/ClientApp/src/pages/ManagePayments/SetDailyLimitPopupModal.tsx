import * as React from 'react';
import { useTranslation } from 'react-i18next';
import { paymentProviderService } from '../../services/paymentsAPI';
import { IconClose, } from '../../components/Icons';
import { ErrorMessageComponent, SuccessMessageComponent } from '../../components/MessageComponents';
import { ButtonApply, ButtonCancel } from '../../components/ButtonComponents';
import { ensureNumericInput } from '../../utils/inputHandlers';


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
  const [orgDailyLimit, setOrgDailyLimit] = React.useState<number>(0);
  const [isBlockAction, setIsBlockAction] = React.useState<boolean>(false);

  React.useEffect(() => {
    const loadDailyWithdrawalCount = async () => {
      try {
        const count = await paymentProviderService.getDailyWithdrawalCount();
        setDailyLimit(count);
        setOrgDailyLimit(count);
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

    if(dailyLimit < 1 || dailyLimit > 32767) {
      setError(t('managePayments:dailyWithdrawalCountOutOfRange'));
      setIsLoading(false);
      return;
    }

    if(orgDailyLimit === dailyLimit) {
      setError(t('global:thereNoChangeToUpdate'));
      setIsLoading(false);
      return;
    }

    try {
      const response = await paymentProviderService.setDailyWithdrawalCount(dailyLimit);
      if (response.isSuccessful) {
        setIsBlockAction(true);
        setSuccessMessage(t('managePayments:setLimitSuccess'));
        onSave(dailyLimit);
        setTimeout(() => {
          onClose();
        }, 3000);
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
      <div className="p-[30px] bg-[#ffffff] mt-[100px] max-w-[400px] max-[768px]:max-w-[90%] rounded-[4px] shadow-[0_2px_4px_-1px_#00000033,0_4px_5px_#00000024,0_1px_10px_#0000001f] text-[12px] max-[768px]:text-[16px]">
        <div className="relative">
          <h5 className="text-[16px] text-[var(--text-color-primary)] font-semibold leading-[21.6px] max-[768px]:text-[20px] max-[768px]:leading-[26px]">{t('managePayments:setDailyWithdrawalCount')}</h5>
          <button type="button" className="absolute top-[-20px] right-[-20px] cursor-pointer" onClick={onClose}>
           <IconClose />
          </button>
        </div>
        <div className='my-[8px]'>
          <ErrorMessageComponent message={error} />
          <SuccessMessageComponent message={successMessage} />
        </div>
        <form onSubmit={handleSubmit}>
          <div className="mb-[16px]">
            <div className=" ">
              <div className="pb-[8px] mb-[20px] block">{t('managePayments:setMaxDailyWithdrawalCountForYourMembers')} </div>
              <div>
                <label htmlFor="dailyLimit" className="pb-[8px] block">{t('managePayments:dailyWithdrawalCount')}</label>
              </div>
              <input
                type="text"
                maxLength={5}
                className="h-[32px] max-[768px]:h-[40px] w-[100%] border-[1px] border-[var(--border-control)] rounded-[4px] px-[8px] focus:outline-none focus:border-[var(--button-border)]"
                id="dailyLimit"
                value={dailyLimit}
                onChange={(e) => setDailyLimit(Number(ensureNumericInput(e).target.value))}
                min="0"
                required
              />              
            </div>
          </div>
          <div className="flex justify-center gap-[8px]">         
            <ButtonCancel className="w-[140px] [&>*]:w-[100%]" text={t('global:cancel')} onClick={onClose}  />
            <ButtonApply className='w-[140px] [&>*]:w-[100%]' text={isLoading ? t('global:loading') : t('global:save')}   disabled={isLoading || isBlockAction}/>          
          </div>
        </form>
      </div>
    </div>
  );
};

export default SetDailyLimitPopupModal; 