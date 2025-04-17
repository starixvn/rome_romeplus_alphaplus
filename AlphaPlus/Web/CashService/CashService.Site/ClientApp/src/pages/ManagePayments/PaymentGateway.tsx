import * as React from 'react';
import { useTranslation } from 'react-i18next';
import './PaymentGateway.css';

interface PaymentGatewayProps {
  // Add props here as needed
}

const PaymentGateway: React.FC<PaymentGatewayProps> = () => {
  const { t } = useTranslation('managePayments');

  return (
    <div className="payment-gateway-container">
      <div className="payment-gateway-form">
        <div className="form-group mb-3">
          <label className="form-label">{t('setDailyWidrawalCount')}</label>
        </div>
          <div className="form-group mb-3">
          <label className="form-label">{t('setDailyWidrawalCountForYourMember')}</label>
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
              {/* Table rows will be populated dynamically */}
            </tbody>
          </table>
        </div>
      </div>
    </div>
  );
};

export default PaymentGateway; 