import * as React from 'react';
import { useTranslation } from 'react-i18next';
import { PaymentProvider } from '../../services/interfaces/PaymentProvider';
import { PaymentProviderFee } from '../../services/interfaces/PaymentProviderFee';
import { paymentProviderService } from '../../services/paymentsAPI';

interface FeePopupModalProps {
  provider: PaymentProvider;
  onClose: () => void;
}

const FeePopupModal: React.FC<FeePopupModalProps> = ({ provider, onClose }) => {
  const { t } = useTranslation('managePayments');
  const [isLoading, setIsLoading] = React.useState(true);
  const [feeData, setFeeData] = React.useState<PaymentProviderFee[]>([]);

  React.useEffect(() => {
    const loadFeeData = async () => {
      try {
        setIsLoading(true);
        const fees = await paymentProviderService.getProviderFees(provider.id);
        setFeeData(fees);
      } catch (error) {
        console.error('Error loading fee data:', error);
      } finally {
        setIsLoading(false);
      }
    };

    loadFeeData();
  }, [provider.id]);

  return (
    <>
      <div className="modal fade show" style={{ display: 'block' }} tabIndex={-1}>
        <div className="modal-dialog">
          <div className="modal-content">
            <div className="modal-header">
              <h5 className="modal-title">{t('feeDetails')} - {provider.name}</h5>
              <button type="button" className="btn-close" onClick={onClose}></button>
            </div>
            <div className="modal-body">
              {isLoading ? (
                <div className="text-center">
                  <div className="spinner-border" role="status">
                    <span className="visually-hidden">{t('loading')}</span>
                  </div>
                </div>
              ) : (
                <>
                  <div>{provider.name} {t('fees')}</div>
                  <b>{t('howItWorks')}</b>
                  <p>{t('feesInfo1')}</p>
                  <p>{t('feesFormula')}</p>
                  <p>{t('feesInfo2')}</p>
                  
                  <table className="table table-striped mt-3">
                    <thead>
                      <tr>
                        <th>{t('transactionType')}</th>
                        <th>{t('paymentMethod')}</th>
                        <th>{t('feeRate')}</th>
                      </tr>
                    </thead>
                    <tbody>
                      {feeData.length > 0 ? (
                        feeData.map((fee, index) => (
                          <tr key={index}>
                            <td>{fee.transactionType}</td>
                            <td>{fee.method}</td>
                            <td>{(fee.rate * 100).toFixed(2)}%</td>
                          </tr>
                        ))
                      ) : (
                        <tr>
                          <td colSpan={3} className="text-center">{t('noData')}</td>
                        </tr>
                      )}
                    </tbody>
                  </table>
                </>
              )}
            </div>
            <div className="modal-footer">
              <button type="button" className="btn btn-secondary" onClick={onClose}>
                {t('close')}
              </button>
            </div>
          </div>
        </div>
      </div>
      <div className="modal-backdrop fade show"></div>
    </>
  );
};

export default FeePopupModal; 