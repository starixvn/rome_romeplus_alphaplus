import * as React from 'react';
import { useTranslation } from 'react-i18next';
import { Bank } from '../../services/interfaces/Payments/Bank';
import { paymentProviderService } from '../../services/paymentsAPI';
import { IconClose } from '../../components/Icons';
import Loading from '../../components/LoadingComponent';
import { ButtonApplyComponent } from '../../components/ButtonComponents';
interface BanksPopupModalProps {
  onClose: () => void;
  transactionType: number;
}

const BanksPopupModal: React.FC<BanksPopupModalProps> = ({ onClose, transactionType }) => {
  const { t: tGlobal } = useTranslation('global');
  const { t: tPaymentProviderSettings } = useTranslation('paymentProviderSettings');
  const [banks, setBanks] = React.useState<Bank[]>([]);
  const [loading, setLoading] = React.useState(true);
  const [error, setError] = React.useState<string | null>(null);

  React.useEffect(() => {
    const fetchBanks = async () => {
      setLoading(true);
      const response = await paymentProviderService.getBanksByTransactionType(transactionType);

      if(response.error) {
        setError(response.error.message);
        setLoading(false);
        return;
      }

      const fetchedBanks = response.data || [];
      setBanks(fetchedBanks);
      setError(null);
      setLoading(false);
    };

    fetchBanks();
  }, [tPaymentProviderSettings]);

  return (
    <div className="bg-[#00000066] fixed z-[100] top-0 left-0 right-0 bottom-0 flex items-start justify-center">
      <div className="p-[30px] bg-[#ffffff] mt-[100px] w-full max-w-[560px] max-[768px]:max-w-[90%] rounded-[4px] shadow-[0_2px_4px_-1px_#00000033,0_4px_5px_#00000024,0_1px_10px_#0000001f] text-[12px] max-[768px]:text-[16px]">
        <div className="relative">
          <h5 className="text-[16px] text-[var(--text-color-primary)] font-bold leading-[21.6px] max-[768px]:text-[20px]">{tPaymentProviderSettings('listOfBanks')}</h5>
          <button type="button" className="absolute top-[-20px] right-[-20px] cursor-pointer" onClick={onClose}>
            <IconClose />
          </button>
        </div>
        <div className="mb-[12px]">
          {loading ? ( <Loading /> ) 
            : error ? (<div className="alert alert-danger" role="alert">{error}</div>) 
            : banks.length === 0 ? (<div className="alert alert-info" role="alert">{tGlobal('noData')}</div>) 
            : (
            <>
              <label className='block text-[var(--text-color-primary)] my-[16px] font-semibold'>{tPaymentProviderSettings(`transaction-${transactionType}`)}</label>
              <div className="table-fixed-header max-h-[300px] overflow-y-auto">
                <table className="w-full">
                  <thead>
                    <tr>
                      <th>#</th>
                      <th>{tPaymentProviderSettings('bankName')}</th>
                    </tr>
                  </thead>
                  <tbody>
                    {banks.map((bank, index) => (
                      <tr key={bank.id}>
                        <td>{index + 1}</td>
                        <td className='text-left!'>
                          <div className='flex items-center'>
                            <img src={`../assets/bank/${bank.code.toUpperCase()}.png`} alt={bank.name} className='w-[20px] h-[20px] mr-[8px]' />
                            {bank.name}
                          </div>
                        </td>
                      </tr>
                    ))}
                  </tbody>
                </table>
              </div>
            </>
          )}
        </div>
        <div className="flex justify-center">  
          <ButtonApplyComponent text={tGlobal('close')} onClick={onClose} />
        </div>
      </div>
    </div>
  );
};

export default BanksPopupModal; 