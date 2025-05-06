import * as React from 'react';
import { useTranslation } from 'react-i18next';
import { paymentProviderService } from '../../services/paymentsAPI';
import { IconCheck, IconClose, IconError } from '../../components/Icons';


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
        },1113000);
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
    <div className="bg-[#00000066] fixed z-[100] top-0 left-0 right-0 bottom-0 flex items-center justify-center">
      <div className="p-[30px] bg-[#ffffff] max-w-[400px] rounded-[4px] shadow-[0_2px_4px_-1px_#00000033,0_4px_5px_#00000024,0_1px_10px_#0000001f]">
        <div className="relative">
          <h5 className="text-[16px] text-[#163f78] font-bold leading-[21.6px] tracking-[0.2px] pb-[4px]">{t('managePayments:setDailyWithdrawalCount')}</h5>
          <button type="button" className="absolute top-[-20px] right-[-20px] cursor-pointer" onClick={onClose}>
           <IconClose />
          </button>
        </div>
        <form onSubmit={handleSubmit}>
          <div className="mb-[16px]">
            <div className=" ">
              <div className="pb-[8px] mb-[20px] text-[12px] leading-[14px] block">{t('managePayments:setMaxDailyWithdrawalCountForYourMembers')} </div>
              <div>
                <label htmlFor="dailyLimit" className="pb-[8px] text-[12px] leading-[14px] block">{t('managePayments:dailyWithdrawalCount')}</label>
              </div>
              <input
                type="text"
                className="h-[35px] w-[100%] border-[1px] border-[#0000003D] rounded-[4px] px-[8px] text-[12px] focus:outline-none focus:border-[#163f78]"
                id="dailyLimit"
                value={dailyLimit}
                onChange={(e) => setDailyLimit(Number(e.target.value))}
                min="0"
                required
              />
              {error && <div className="mt-[16px] font-bold text-[12px] text-[#333] bg-[#FFF2F2] border-[1px] border-[#FE0000] rounded-[10px] pl-[36px] pr-[12px] py-[8px] relative block capitalize"><IconError className='w-[20px] h-[20px] fill-[#fe0000] absolute top-[8px] left-[10px]' />{error}</div>}
              {successMessage && <div className="mt-[16px] font-bold text-[12px] text-[#333] bg-[#E2FFF2] border-[1px] border-[#5cb774] rounded-[10px] pl-[36px] pr-[12px] py-[8px] relative block capitalize"><IconCheck className='w-[20px] h-[20px] fill-[#5CB774] absolute top-[8px] left-[10px]' />{successMessage}</div>}
            </div>
          </div>
          <div className="flex justify-center">
            <button type="button" className="w-[140px] h-[30px] rounded-[20px] text-[#163f78] px-[16px] py-[4px] text-[12px] border-[1px] border-[#163f78] mx-[4px] bg-[#ffffff] hover:bg-[#EAEDF2] cursor-pointer " onClick={onClose}>
              {t('global:cancel')}
            </button>
            <button type="submit" className="w-[140px] h-[30px] rounded-[20px] text-[#ffffff] px-[16px] py-[4px] text-[12px] border-[1px] border-[#163f78] mx-[4px] bg-[#163f78] hover:bg-[#375A8A] cursor-pointer  shadow-[0_1px_5px_#0003,0_2px_2px_#00000024,0_3px_1px_-2px_#0000001f]" disabled={isLoading}>
              {isLoading ? t('global:loading') : t('global:save')}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};

export default SetDailyLimitPopupModal; 