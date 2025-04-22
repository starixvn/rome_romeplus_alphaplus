import * as React from 'react';
import { useTranslation } from 'react-i18next';
import { paymentProviderService } from '../../services/paymentsAPI';
import { PaymentProvider } from '../../services/interfaces/PaymentProvider';
import FeePopupModal from './FeePopupModal';
import DailyLimitPopupModal from './SetDailyLimitPopupModal';
import './PaymentGateway.css';

// Extend Window interface to include custId
declare global {
  interface Window {
    custId: number;
  }
}

interface PaymentGatewayProps {
  // Add props here as needed
}

const PaymentGateway: React.FC<PaymentGatewayProps> = () => {
  const { t: tGlobal } = useTranslation('global');
  const { t: tManagePayments } = useTranslation('managePayments');
  const [providers, setProviders] = React.useState<PaymentProvider[]>([]);
  const [loading, setLoading] = React.useState<boolean>(true);
  const [error, setError] = React.useState<string | null>(null);
  const [selectedProvider, setSelectedProvider] = React.useState<PaymentProvider | null>(null);
  const [showFeePopup, setShowFeePopup] = React.useState<boolean>(false);
  const [showDailyLimitPopup, setShowDailyLimitPopup] = React.useState<boolean>(false);

  React.useEffect(() => {
    const fetchProviders = async () => {
      try {
        setLoading(true);
        const data = await paymentProviderService.getProviders();
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
    return status === 1 ? tManagePayments('active') : tManagePayments('inactive');
  };

  const handleFeeClick = (provider: PaymentProvider) => {
    setSelectedProvider(provider);
    setShowFeePopup(true);
  };

  const handleClosePopup = () => {
    setShowFeePopup(false);
    setSelectedProvider(null);
  };

  const handleSetLimitClick = () => {
    setShowDailyLimitPopup(true);
  };

  const handleCloseDailyLimitPopup = () => {
    setShowDailyLimitPopup(false);
  };

  const handleSaveDailyLimit = () => {};

  const handleEditClick = (provider: PaymentProvider) => {
    location.href = '../Payments/PaymentProviderSettings?id=' + provider.id;
  };

  return (
    <div className="payment-gateway-container">
      <div className="payment-gateway-form">
        <div className="form-group mb-3">
          <label className="form-label">{tManagePayments('setDailyWithdrawalCount')}</label>
        </div>
        <div className="form-group mb-3">
          <label className="form-label">{tManagePayments('setDailyWithdrawalCountForYourMembers')}</label>
        </div>
        <div className="form-group mb-3">
          <button 
            type="button" 
            className="btn btn-primary"
            onClick={() => handleSetLimitClick()} // Default values, should be updated with actual values
          >
            {tManagePayments('setLimit')}
          </button>
        </div>
        <hr className="separator" />
        <div className="form-group mb-3">
          <label className="form-label">{tManagePayments('managePaymentProviders')}</label>
        </div>
        
        {loading ? (
          <div className="text-center p-4">
            <div className="spinner-border text-primary" role="status">
              <span className="visually-hidden">{tGlobal('loading')}</span>
            </div>
            <p className="mt-2">{tManagePayments('loading')}</p>
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
                  <th scope="col">{tGlobal('action')}</th>
                  <th scope="col">{tManagePayments('provider')}</th>
                  <th scope="col">{tManagePayments('fee')}</th>
                  <th scope="col">{tManagePayments('providerStatus')}</th>
                  <th scope="col">{tManagePayments('lastUpdated')}</th>
                </tr>
              </thead>
              <tbody>
                {providers.length > 0 ? (
                  providers.map((provider, index) => (
                    <tr key={provider.id}>
                      <td>{index + 1}</td>
                      <td>
                        <button 
                          className="btn btn-sm btn-outline-primary me-1"
                          onClick={() => handleEditClick(provider)}
                        >
                          {tGlobal('edit')}
                        </button>
                      </td>
                      <td>{provider.name}</td>
                      <td>
                        <button 
                          className="btn btn-sm btn-outline-primary me-1"
                          onClick={() => handleFeeClick(provider)}
                        >
                          {tManagePayments('fee')}
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
                      {tGlobal('noData')}
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

        {/* Daily Limit Popup Modal */}
        {showDailyLimitPopup && (
          <DailyLimitPopupModal
            onClose={handleCloseDailyLimitPopup}
            onSave={handleSaveDailyLimit}
            custId={window.custId}
          />
        )}
      </div>
    </div>
  );
};

export default PaymentGateway; 