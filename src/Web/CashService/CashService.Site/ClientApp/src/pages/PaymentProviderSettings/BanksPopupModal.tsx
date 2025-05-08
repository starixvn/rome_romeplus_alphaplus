import * as React from 'react';
import { useTranslation } from 'react-i18next';
import { Bank } from '../../services/interfaces/Bank';
import { paymentProviderService } from '../../services/paymentsAPI';
import { IconClose } from '../../components/Icons';
interface BanksPopupModalProps {
  onClose: () => void;
  transactionType: number;
}

const BanksPopupModal: React.FC<BanksPopupModalProps> = ({ onClose, transactionType }) => {
  const { t: tGlobal } = useTranslation('global');
  const { t: tManagePayments } = useTranslation('managePayments');
  const [banks, setBanks] = React.useState<Bank[]>([]);
  const [loading, setLoading] = React.useState(true);
  const [error, setError] = React.useState<string | null>(null);

  React.useEffect(() => {
    const fetchBanks = async () => {
      try {
        setLoading(true);
        const fetchedBanks = await paymentProviderService.getBanksByTransactionType(transactionType);
        setBanks(fetchedBanks);
        setError(null);
      } catch (err) {
        setError(tManagePayments('failedToLoadBanks'));
        console.error('Error fetching banks:', err);
      } finally {
        setLoading(false);
      }
    };

    fetchBanks();
  }, [tManagePayments]);

  return (
    <div className="bg-[#00000066] fixed z-[100] top-0 left-0 right-0 bottom-0 flex items-center justify-center">
      <div className="p-[30px] bg-[#ffffff] w-full max-w-[560px] rounded-[4px] shadow-[0_2px_4px_-1px_#00000033,0_4px_5px_#00000024,0_1px_10px_#0000001f]">
        <div className="relative">
          <h5 className="text-[16px] text-[var(--primary-color)] font-bold leading-[21.6px] tracking-[0.2px]">{tManagePayments('listOfBanks')}</h5>
          <button type="button" className="absolute top-[-20px] right-[-20px] cursor-pointer" onClick={onClose}>
            <IconClose />
          </button>
        </div>
        <div className="mb-[12px]">
          {loading ? (
            <div className="text-center">
              <div className="spinner-border" role="status">
                <span className="visually-hidden">{tGlobal('loading')}</span>
              </div>
            </div>
          ) : error ? (
            <div className="alert alert-danger" role="alert">
              {error}
            </div>
          ) : banks.length === 0 ? (
            <div className="alert alert-info" role="alert">
              {tGlobal('noData')}
            </div>
          ) : (
            <>
              <label className='block text-[var(--primary-color)] my-[16px] font-semibold'>{tManagePayments(`transaction-${transactionType}`)}</label>
              <div className="table-fixed-header max-h-[300px] overflow-y-auto">
                <table className="w-full">
                  <thead>
                    <tr>
                      <th>#</th>
                      <th>{tManagePayments('bankName')}</th>
                    </tr>
                  </thead>
                  <tbody>
                    {banks.map((bank, index) => (
                      <tr key={bank.id}>
                        <td>{index + 1}</td>
                        <td className='text-left!'>
                          <div className='flex items-center'>
                            <img src="../assets/bank/BAAC.png" alt={bank.name} className='w-[20px] h-[20px] mr-[8px]' />
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
          <button type="button" className="w-[140px] capitalize font-bold h-[30px] rounded-[20px] text-[#ffffff] px-[16px] py-[4px] text-[12px] border-[1px] border-[var(--primary-color)] mx-[4px] bg-[var(--primary-color)] hover:bg-[#375A8A] cursor-pointer  shadow-[0_1px_5px_#0003,0_2px_2px_#00000024,0_3px_1px_-2px_#0000001f]" onClick={onClose}>
            {tGlobal('close')}
          </button>
        </div>
      </div>
    </div>
  );
};

export default BanksPopupModal; 