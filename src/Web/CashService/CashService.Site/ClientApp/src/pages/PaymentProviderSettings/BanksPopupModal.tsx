import * as React from 'react';
import { useTranslation } from 'react-i18next';
import { Bank } from '../../services/interfaces/Payments/Bank';
import { paymentProviderService } from '../../services/paymentsAPI';
import Loading from '../../components/LoadingComponent';
import { ButtonApplyComponent, ButtonClosePopupComponent } from '../../components/ButtonComponents';
import { MediumTitleComponent } from '../../components/TitleComponents';
import BankIcon from '../../components/BankComponents';
import { ErrorMessageComponent, WarningMessageComponent } from '../../components/MessageComponents';

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
          <MediumTitleComponent className='font-bold!' text={tPaymentProviderSettings('listOfBanks')} />
          <ButtonClosePopupComponent onClick={onClose} />
        </div>
        <div className="mb-[12px]">
          {loading ? ( <Loading /> ) 
            : error ? (<ErrorMessageComponent message={error} />) 
            : banks.length === 0 ? (<WarningMessageComponent message={tGlobal('noData')} />) 
            : (
            <>
              <p className='block text-[var(--text-color-primary)] my-[16px] font-semibold'>{tPaymentProviderSettings(`transaction-${transactionType}`)}</p>
              <div className="table-fixed-header max-h-[300px] overflow-y-auto">
                <table className="w-full">
                  <thead className='z-10'>
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
                          <div className='flex items-center relative pl-[25px]'>
                            <span className='absolute top-[-2px] left-[0]'><BankIcon code={bank.code} /></span>
                            {tGlobal(bank.code)}
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