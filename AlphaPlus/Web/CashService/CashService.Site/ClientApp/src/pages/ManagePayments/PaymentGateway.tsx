import * as React from 'react';
import { useTranslation } from 'react-i18next';
import { paymentProviderService } from '../../services/paymentsAPI';
import { PaymentProvider } from '../../services/interfaces/PaymentProvider';
import FeePopupModal from './FeePopupModal';
import './PaymentGateway.css';

interface PaymentGatewayProps {
  // Add props here as needed
}

const PaymentGateway: React.FC<PaymentGatewayProps> = () => {
  const { t } = useTranslation('managePayments');
  const [providers, setProviders] = React.useState<PaymentProvider[]>([]);
  const [loading, setLoading] = React.useState<boolean>(true);
  const [error, setError] = React.useState<string | null>(null);
  const [selectedProvider, setSelectedProvider] = React.useState<PaymentProvider | null>(null);
  const [showFeePopup, setShowFeePopup] = React.useState<boolean>(false);

  React.useEffect(() => {
    const fetchProviders = async () => {
      try {
        setLoading(true);
        const data = await paymentProviderService.getAllProviders();
        setProviders(data);
        setError(null);
      } catch (err) {
        console.error('Failed to fetch payment providers:', err);
        setError('Failed to load payment providers. Please try again later.');
      } finally {
        setLoading(false);
      }
    };

    fetchProviders();
  }, []);

  const getStatusText = (status: number) => {
    return status === 1 ? t('active') : t('inactive');
  };

  const handleFeeClick = (provider: PaymentProvider) => {
    setSelectedProvider(provider);
    setShowFeePopup(true);
  };

  const handleClosePopup = () => {
    setShowFeePopup(false);
    setSelectedProvider(null);
  };

  return (
    <div className="payment-gateway-container">
      <div className="payment-gateway-form">
        <div className="form-group mb-3">
          <label className="form-label">{t('setDailyWithdrawalCount')}</label>
        </div>
        <div className="form-group mb-3">
          <label className="form-label">{t('setDailyWithdrawalCountForYourMember')}</label>
        </div>
        <div className="form-group mb-3">
          <button type="button" className="btn btn-primary">
            {t('setLimit')}
          </button>
        </div>
        <hr className="separator" />
        <div className="form-group mb-3">
          <label className="form-label">{t('managePaymentProviders')}</label>
        </div>
        
        {loading ? (
          <div className="text-center p-4">
            <div className="spinner-border text-primary" role="status">
              <span className="visually-hidden">Loading...</span>
            </div>
            <p className="mt-2">{t('loading')}</p>
          </div>
        ) : error ? (
          <div className="alert alert-danger" role="alert">
            {error}
          </div>
        ) : (
          <div className="table-responsive">
            <table className="table table-striped table-hover">
              <thead>
                <tr>
                  <th scope="col">#</th>
                  <th scope="col">{t('action')}</th>
                  <th scope="col">{t('provider')}</th>
                  <th scope="col">{t('fee')}</th>
                  <th scope="col">{t('providerStatus')}</th>
                  <th scope="col">{t('lastUpdated')}</th>
                </tr>
              </thead>
              <tbody>
                {providers.length > 0 ? (
                  providers.map((provider, index) => (
                    <tr key={provider.id}>
                      <td>{index + 1}</td>
                      <td>
                        <button className="btn btn-sm btn-outline-primary me-1">
                          {t('edit')}
                        </button>
                      </td>
                      <td>{provider.name}</td>
                      <td>
                        <button 
                          className="btn btn-sm btn-outline-primary me-1"
                          onClick={() => handleFeeClick(provider)}
                        >
                          {t('fee')}
                        </button>
                      </td>
                      <td>
                        <span className={`badge ${provider.status === 1 ? 'bg-success' : 'bg-danger'}`}>
                          {getStatusText(provider.status)}
                        </span>
                      </td>
                      <td>{provider.lastUpdatedStr}</td>
                    </tr>
                  ))
                ) : (
                  <tr>
                    <td colSpan={6} className="text-center">
                      {t('noProvidersFound')}
                    </td>
                  </tr>
                )}
              </tbody>
            </table>
          </div>
        )}

        {/* Fee Popup Modal */}
        {showFeePopup && selectedProvider && (
          <FeePopupModal
            provider={selectedProvider}
            onClose={handleClosePopup}
          />
        )}
      </div>
    </div>
  );
};

export default PaymentGateway; 