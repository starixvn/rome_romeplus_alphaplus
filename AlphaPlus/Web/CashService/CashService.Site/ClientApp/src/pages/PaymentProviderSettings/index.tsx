import * as React from 'react';
import { createRoot } from 'react-dom/client';
import { useTranslation } from 'react-i18next';
import '../../i18n/i18n';
import './PaymentProviderSettings.css';
import { PAYMENT_METHODS, STATUS_OPTIONS, TRANSACTION_TYPES } from '../../constants/paymentConstants';
import FeePopupModal from '../ManagePayments/FeePopupModal';
import { PaymentProvider } from '../../services/interfaces/PaymentProvider';
import { paymentProviderService } from '../../services/paymentsAPI';

// Extend Window interface to include providerName
declare global {
  interface Window {
    provider: any;
  }
}

function PaymentProviderSettings() {
  const { t: tGlobal } = useTranslation('global');
  const { t: tManagePayments } = useTranslation('managePayments');
  const [showFeePopup, setShowFeePopup] = React.useState(false);
  const [providerSettings, setProviderSettings] = React.useState<any[]>([]);
  const [originalSettings, setOriginalSettings] = React.useState<any[]>([]);
  const [loading, setLoading] = React.useState(true);
  const [error, setError] = React.useState<string | null>(null);
  
  // State for dropdowns
  const [selectedPaymentMethod, setSelectedPaymentMethod] = React.useState('');
  const [selectedStatus, setSelectedStatus] = React.useState('');
  const [selectedTransactionType, setSelectedTransactionType] = React.useState('');

  // Fetch provider settings when component mounts or when dropdowns change
  React.useEffect(() => {
    const fetchProviderSettings = async () => {
      try {
        setLoading(true);
        const method = selectedPaymentMethod ? parseInt(selectedPaymentMethod) : 0;
        const status = selectedStatus ? parseInt(selectedStatus) : 0;
        const transactionType = selectedTransactionType ? parseInt(selectedTransactionType) : 0;
        
        const settings = await paymentProviderService.getProviderSettings(
          window.provider.Id,
          method,
          status,
          transactionType
        );
        setProviderSettings(settings);
        setOriginalSettings(settings);
        setError(null);
      } catch (err) {
        console.error('Failed to fetch provider settings:', err);
        setError('Failed to load provider settings. Please try again later.');
      } finally {
        setLoading(false);
      }
    };

    fetchProviderSettings();
  }, [selectedPaymentMethod, selectedStatus, selectedTransactionType]);

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

  // Handler for minimum amount change
  const handleMinAmountChange = (index: number, value: string) => {
    const updatedSettings = [...providerSettings];
    updatedSettings[index] = {
      ...updatedSettings[index],
      minAmount: value
    };
    setProviderSettings(updatedSettings);
  };

  // Handler for maximum amount change
  const handleMaxAmountChange = (index: number, value: string) => {
    const updatedSettings = [...providerSettings];
    updatedSettings[index] = {
      ...updatedSettings[index],
      maxAmount: value
    };
    setProviderSettings(updatedSettings);
  };

  // Handler for toggle change
  const handleSettingToggle = (index: number) => {
    const updatedSettings = [...providerSettings];
    updatedSettings[index] = {
      ...updatedSettings[index],
      isEnabled: !updatedSettings[index].isEnabled
    };
    setProviderSettings(updatedSettings);
  };

  // Handler for reset button
  const handleReset = () => {
    setProviderSettings([...originalSettings]);
  };

  const getPaymentMethodName = (id: number) => {
    return PAYMENT_METHODS.find(method => method.id === id)?.name || id;
  };

  const getStatusName = (id: number) => {
    return STATUS_OPTIONS.find(status => status.id === id)?.name || id;
  };

  const getTransactionTypeName = (id: number) => {
    return TRANSACTION_TYPES.find(type => type.id === id)?.name || id;
  };

  return (
    <div className="payment-provider-settings">
      <div className="settings-header">
        <h2>{window.provider.Name}</h2>
      </div>
      
      {loading ? (
        <div className="loading-container">
          <p>{tGlobal('loading')}</p>
        </div>
      ) : error ? (
        <div className="error-container">
          <p className="error-message">{error}</p>
        </div>
      ) : (
        <>
          <div className="form-group">
            <label className="form-label">{tManagePayments('setUpPaymentMethodsForMembers')}</label>
            <label className="toggle-label">
              {tGlobal('enable')}/{tGlobal('disable')}
            </label>
            <div className="toggle-container">
              <input
                type="checkbox"
                id="enableToggle"
                checked={window.provider.Status}
                className="toggle-checkbox"
                disabled={true}
              />
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
            <button className="btn btn-secondary me-2" onClick={handleViewFees}>{tManagePayments('viewProviderFees', {providerName: window.provider.Name})}</button>
          </div>
          <div className="settings-table-container">
            <table className="table table-striped">
              <thead>
                <tr>
                  <th>{tManagePayments('paymentMethod')}</th>
                  <th>{tManagePayments('status')}</th>
                  <th>{tManagePayments('transactionType')}</th>
                  <th>{tManagePayments('minimum')}</th>
                  <th>{tManagePayments('maximum')}</th>
                  <th>{tGlobal('enable')}/{tGlobal('disable')}</th>
                  <th>{tManagePayments('bank')}</th>
                </tr>
              </thead>
              <tbody>
                {providerSettings.map((setting, index) => (
                  <tr key={index}>
                    <td>{getPaymentMethodName(setting.method)}</td>
                    <td>{getStatusName(setting.status)}</td>
                    <td>{getTransactionTypeName(setting.transactionType)}</td>
                    <td>
                      <input 
                        type="text" 
                        className="form-control form-control-sm" 
                        value={setting.minAmount || ''} 
                        onChange={(e) => handleMinAmountChange(index, e.target.value)}
                        placeholder={tManagePayments('minimum')}
                      />
                    </td>
                    <td>
                      <input 
                        type="text" 
                        className="form-control form-control-sm" 
                        value={setting.maxAmount || ''} 
                        onChange={(e) => handleMaxAmountChange(index, e.target.value)}
                        placeholder={tManagePayments('maximum')}
                      />
                    </td>
                    <td>
                      <div className="toggle-container">
                        <input
                          type="checkbox"
                          id={`toggle-${index}`}
                          checked={setting.isEnabled || false}
                          onChange={() => handleSettingToggle(index)}
                          className="toggle-checkbox"
                        />
                      </div>
                    </td>
                    <td>
                      {setting.bankRequired ? (
                        <button 
                          className="btn btn-sm btn-info"
                          onClick={() => {/* Handle view banks */}}
                        >
                          {tManagePayments('listOfBanks')}
                        </button>
                      ) : tManagePayments('noBankRequired')}
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
            <div className="settings-footer">
              <button className="btn btn-warning me-2" onClick={handleReset}>{tGlobal('reset')}</button>
              <button className="btn btn-success">{tGlobal('update')}</button>
            </div>
         </div>

        </>
      )}

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