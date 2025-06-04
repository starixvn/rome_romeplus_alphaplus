import * as React from 'react';
import { useTranslation } from 'react-i18next';
import { Bank } from '../../services/interfaces/Payments/Bank';
import { paymentProviderService } from '../../services/paymentsAPI';
import Loading from '../../components/LoadingComponent';
import BankIcon from '../../components/BankComponents';
import { ErrorMessageComponent } from '../../components/MessageComponents';
import PopupModalComponent from '../../components/PopupModalComponent';
import { TableComponent } from '../../components/TableComponents';

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
    <PopupModalComponent title={tPaymentProviderSettings('listOfBanks')} onClose={onClose}>
        {loading ? ( <Loading /> ) 
            : error ? (<ErrorMessageComponent message={error} />) 
            : (
            <>
              <p className='block text-[var(--text-color-primary)] my-[16px] font-semibold'>{tPaymentProviderSettings(`transaction-${transactionType}`)}</p>
              <div className="table-fixed-header max-h-[300px] overflow-y-auto">
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