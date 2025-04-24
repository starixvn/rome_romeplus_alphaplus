import * as React from 'react';
import { useTranslation } from 'react-i18next';
import { Bank } from '../../services/interfaces/Bank';
import { paymentProviderService } from '../../services/paymentsAPI';
import { TRANSACTION_TYPES } from '../../constants/paymentConstants';
import './PaymentProviderSettings.css';

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
    <div className="modal-overlay">
      <div className="modal-content">
        <div className="modal-header">
          <h5 className="modal-title">{tManagePayments('listOfBanks')}</h5>
          <button type="button" className="btn-close" onClick={onClose}></button>
        </div>
        <div className="modal-body">
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
              <label>{TRANSACTION_TYPES.find(type => type.id === transactionType)?.name || transactionType}</label>
              <div className="table-responsive">
                <table className="table table-striped">
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
                        <td>{bank.name}</td>
                      </tr>
                    ))}
                  </tbody>
                </table>
              </div>
            </>
          )}
        </div>
        <div className="modal-footer">
          <button type="button" className="btn btn-secondary" onClick={onClose}>
            {tGlobal('close')}
          </button>
        </div>
      </div>
    </div>
  );
};

export default BanksPopupModal; 