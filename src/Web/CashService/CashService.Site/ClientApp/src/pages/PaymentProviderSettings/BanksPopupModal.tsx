import * as React from 'react';
import { useTranslation } from 'react-i18next';
import { Bank } from '../../services/interfaces/Bank';
import Loading from '../../components/LoadingComponent';
import BankIcon from '../../components/BankComponents';
import { MessageComponent } from '../../components/Message';
import PopupModalComponent from '../../components/PopupModalComponent';
import { TableComponent } from '../../components/TableComponents';
import { baseAPI } from '../../services/commonAPI';

interface BanksPopupModalProps {
  onClose: () => void;
  transactionType: number;
  providerId: number;
}

const BanksPopupModal: React.FC<BanksPopupModalProps> = ({ onClose, transactionType, providerId }) => {
  const { t: tGlobal } = useTranslation('global');
  const { t: tPaymentProviderSettings } = useTranslation('paymentProviderSettings');
  const [banks, setBanks] = React.useState<Bank[]>([]);
  const [loading, setLoading] = React.useState(true);
  const [error, setError] = React.useState<string | null>(null);

  React.useEffect(() => {
    const fetchBanks = async () => {
      setLoading(true);
      const response = await baseAPI.getProviderBanks(providerId);

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
    <PopupModalComponent title={tPaymentProviderSettings('listOfBanks')} onClose={onClose}>
        {loading ? ( <Loading /> ) 
            : error ? (<MessageComponent message={error} type='error' />) 
            : (
            <>
              <p className='block text-[var(--text-label-color)] my-[16px] font-semibold'>{tPaymentProviderSettings(`transaction-${transactionType}`)}</p>
              <div className="table-fixed-header max-h-[309px] overflow-y-auto">
                <TableComponent 
                  numOfColumns={2}
                  resourcesTranslation={tPaymentProviderSettings}
                  headerRows={[['#', 'bankName']]}
                  headerStyle="z-10"
                  bodyRows={banks?.map((bank, index) => {
                    return {
                      key: bank.id.toString(),
                      className: "",
                      columns: [
                        (<td>{index + 1}</td>),
                        (<td className='text-left!'>
                          <div className='flex items-center relative pl-[25px]'>
                            <span className='absolute top-[-2px] left-[0]'><BankIcon code={bank.code} /></span>
                            {tGlobal(bank.code)}
                          </div>
                        </td>)
                      ]
                    };
                  })}
                />
              </div>
            </>
          )}
    </PopupModalComponent>
  );
};

export default BanksPopupModal; 