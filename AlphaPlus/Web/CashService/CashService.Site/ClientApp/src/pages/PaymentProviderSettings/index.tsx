import * as React from 'react';
import { createRoot } from 'react-dom/client';
import { useTranslation } from 'react-i18next';
import '../../i18n/i18n';
import './PaymentProviderSettings.css';
import { PAYMENT_METHODS, STATUS_OPTIONS, TRANSACTION_TYPES } from '../../constants/paymentConstants';
import FeePopupModal from '../ManagePayments/FeePopupModal';

// Extend Window interface to include providerName
declare global {
  interface Window {
    provider: any;
  }
}

function PaymentProviderSettings() {
  const { t: tGlobal } = useTranslation('global');
  const { t: tManagePayments } = useTranslation('managePayments');
  const [isEnabled, setIsEnabled] = React.useState(false);
  const [showFeePopup, setShowFeePopup] = React.useState(false);
  
  // State for dropdowns
  const [selectedPaymentMethod, setSelectedPaymentMethod] = React.useState('');
  const [selectedStatus, setSelectedStatus] = React.useState('');
  const [selectedTransactionType, setSelectedTransactionType] = React.useState('');

  const handleToggle = (event: React.ChangeEvent<HTMLInputElement>) => {
    setIsEnabled(event.target.checked);
  };

  const handlePaymentMethodChange = (event: React.ChangeEvent<HTMLSelectElement>) => {
    setSelectedPaymentMethod(event.target.value);
  };

  const handleStatusChange = (event: React.ChangeEvent<HTMLSelectElement>) => {
    setSelectedStatus(event.target.value);
  };

  const handleTransactionTypeChange = (event: React.ChangeEvent<HTMLSelectElement>) => {
    setSelectedTransactionType(event.target.value);
  };

  const handleViewFees = () => {
    setShowFeePopup(true);
  };

  const handleCloseFeePopup = () => {
    setShowFeePopup(false);
  };

  return (
    <div className="payment-provider-settings">
      <div className="settings-header">
        <h2>{window.provider.Name}</h2>
      </div>
      
      <div className="form-group">
        <label className="form-label">{tManagePayments('setUpPaymentMethodsForMembers')}</label>
        <div className="toggle-container">
          <input
            type="checkbox"
            id="enableToggle"
            checked={window.provider.Status}
            onChange={handleToggle}
            className="toggle-checkbox"
          />
          <label className="toggle-label">
            {isEnabled ? tGlobal('enable') : tGlobal('disable')}
          </label>
        </div>
      </div>

      <div className="form-group">
        <label className="form-label">{tManagePayments('paymentMethod')}</label>
        <select 
          className="form-select" 
          value={selectedPaymentMethod}
          onChange={handlePaymentMethodChange}
        >
          <option value="">{tGlobal('select')}</option>
          {PAYMENT_METHODS.map(method => (
            <option key={method.id} value={method.id}>{method.name}</option>
          ))}
        </select>
      </div>

      <div className="form-group">
        <label className="form-label">{tGlobal('status')}</label>
        <select 
          className="form-select" 
          value={selectedStatus}
          onChange={handleStatusChange}
        >
          <option value="">{tGlobal('select')}</option>
          {STATUS_OPTIONS.map(status => (
            <option key={status.id} value={status.id}>{status.name}</option>
          ))}
        </select>
      </div>

      <div className="form-group">
        <label className="form-label">{tManagePayments('transactionType')}</label>
        <select 
          className="form-select" 
          value={selectedTransactionType}
          onChange={handleTransactionTypeChange}
        >
          <option value="">{tGlobal('select')}</option>
          {TRANSACTION_TYPES.map(type => (
            <option key={type.id} value={type.id}>{type.name}</option>
          ))}
        </select>
      </div>

      <div className="settings-footer">
        <button className="btn btn-primary me-2">{tManagePayments('addPaymentMethod')}</button>
        <button className="btn btn-secondary" onClick={handleViewFees}>{tManagePayments('viewProviderFees', {providerName: window.provider.Name})}</button>
      </div>

      {/* Fee Popup Modal */}
      {showFeePopup && (
        <FeePopupModal
          provider={{id: window.provider.Id, name: window.provider.Name} as PaymentProvider}
          onClose={handleCloseFeePopup}
        />
      )}
    </div>
  );
};

createRoot(document.getElementById('root')!).render(
  <React.StrictMode>
    <PaymentProviderSettings />
  </React.StrictMode>
); 