import React, { useState, useEffect } from 'react';
import { useTranslation } from 'react-i18next';
import { paymentProviderService } from '../../services/paymentsAPI';
import { IconClose } from '../../components/Icons';

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
    <div className="bg-[#00000066] fixed top-0 left-0 right-0 bottom-0 flex items-center justify-center" tabIndex={-1}>
      <div className="p-[30px] bg-[#ffffff] max-w-[500px] rounded-[4px] shadow-[0_2px_4px_-1px_#00000033,0_4px_5px_#00000024,0_1px_10px_#0000001f]">
        <div className="relative">
            <h5 className="text-[16px] text-[#163f78] font-bold leading-[21.6px] tracking-[0.2px] pb-[4px]">{tManagePayments('transactionLimitPreferences')}</h5>
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
              {error && (
                <div className="alert alert-danger" role="alert">
                  {error}
                </div>
              )}
              {success && (
                <div className="alert alert-success" role="alert">
                  {successMessage}
                </div>
              )}
              <div className="pt-[4px]">
                 {tManagePayments('customizeTransactionLimitPreferences')}
              </div>
              <div className="text-[#163f78] my-[8px] font-semibold">
                {tManagePayments('deposit')}
              </div>
              <div className='flex justify-between'>
              <div className="w-[48%]">
                <label className="block font-bold text-[#333333] my-[8px]">{tManagePayments('minPerTransaction')}</label>
                <input
                  type="text"
                  className="h-[35px] w-[100%] border-[1px] border-[#0000003D] rounded-[4px] px-[8px] text-[12px] focus:outline-none focus:border-[#163f78]"
                  value={minDeposit}
                  onChange={(e) => setMinDeposit(e.target.value)}
                  min="0"
                  step="0.01"
                  required
                />
              </div>
              <div className="w-[48%]">
                <label className="block font-bold text-[#333333] my-[8px]">{tManagePayments('maxPerTransaction')}</label>
                <input
                  type="text"
                  className="h-[35px] w-[100%] border-[1px] border-[#0000003D] rounded-[4px] px-[8px] text-[12px] focus:outline-none focus:border-[#163f78]"
                  value={maxDeposit}
                  onChange={(e) => setMaxDeposit(e.target.value)}
                  min="0"
                  step="0.01"
                  required
                />
              </div>
              </div>
              <div className="text-[#163f78] my-[8px] font-semibold">{tManagePayments('withdrawal')}</div>
              <div className='flex justify-between'>
              <div className="w-[48%]">
                <label className="block font-bold text-[#333333] my-[8px]">{tManagePayments('minPerTransaction')}</label>
                <input
                  type="text"
                  className="h-[35px] w-[100%] border-[1px] border-[#0000003D] rounded-[4px] px-[8px] text-[12px] focus:outline-none focus:border-[#163f78]"
                  value={minWithdrawal}
                  onChange={(e) => setMinWithdrawal(e.target.value)}
                  min="0"
                  step="0.01"
                  required
                />
              </div>
              <div className="w-[48%]">
                <label className="block font-bold text-[#333333] my-[8px]">{tManagePayments('maxPerTransaction')}</label>
                <input
                  type="text"
                  className="h-[35px] w-[100%] border-[1px] border-[#0000003D] rounded-[4px] px-[8px] text-[12px] focus:outline-none focus:border-[#163f78]"
                  value={maxWithdrawal}
                  onChange={(e) => setMaxWithdrawal(e.target.value)}
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
                  className="h-[35px] w-[100%] border-[1px] border-[#0000003D] rounded-[4px] px-[8px] text-[12px] focus:outline-none focus:border-[#163f78]"
                  value={dailyWithdrawalCount}
                  onChange={(e) => setDailyWithdrawalCount(e.target.value)}
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
                className="w-[140px] h-[30px] rounded-[20px] text-[#163f78] px-[16px] py-[4px] text-[12px] border-[1px] border-[#163f78] mx-[4px] bg-[#ffffff] hover:bg-[#EAEDF2] cursor-pointer "
                onClick={onClose}
                disabled={loading}
              >
                {tGlobal('cancel')}
              </button>
              <button
                type="submit"
                className="w-[140px] h-[30px] rounded-[20px] text-[#ffffff] px-[16px] py-[4px] text-[12px] border-[1px] border-[#163f78] mx-[4px] bg-[#163f78] hover:bg-[#375A8A] cursor-pointer  shadow-[0_1px_5px_#0003,0_2px_2px_#00000024,0_3px_1px_-2px_#0000001f]"
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