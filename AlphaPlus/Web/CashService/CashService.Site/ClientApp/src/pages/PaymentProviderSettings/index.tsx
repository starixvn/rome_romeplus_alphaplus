import * as React from 'react';
import { createRoot } from 'react-dom/client';
import { useTranslation } from 'react-i18next';
import '../../i18n/i18n';
import './PaymentProviderSettings.css';
import { STATUS_OPTIONS, TRANSACTION_TYPES } from '../../constants/paymentConstants';
import ProviderFeesPopupModal from '../ManagePayments/ProviderFeesPopupModal';
import AddPaymentMethodModal from './AddPaymentMethodModal';
import BanksPopupModal from './BanksPopupModal';
import { PaymentProvider } from '../../services/interfaces/PaymentProvider';
import { paymentProviderService } from '../../services/paymentsAPI';

// Extend Window interface to include providerName
declare global {
  interface Window {
    providerSettingsViewModel: any;
  }
}

function PaymentProviderSettings() {
  const { t: tGlobal } = useTranslation('global');
  const { t: tManagePayments } = useTranslation('managePayments');
  const [showFeePopup, setShowFeePopup] = React.useState(false);
  const [showAddPaymentMethod, setShowAddPaymentMethod] = React.useState(false);
  const [showBanksPopup, setShowBanksPopup] = React.useState(false);
  const [selectedSetting, setSelectedSetting] = React.useState<any>(null);
  const [providerSettings, setProviderSettings] = React.useState<any[]>([]);
  const [originalSettings, setOriginalSettings] = React.useState<any[]>([]);
  const [loading, setLoading] = React.useState(true);
  const [error, setError] = React.useState<string | null>(null);
  const [updating, setUpdating] = React.useState(false);
  const [updateMessage, setUpdateMessage] = React.useState<{type: 'success' | 'error', text: string} | null>(null);
  const [providerStatus, setProviderStatus] = React.useState(window.providerSettingsViewModel.Provider.Status);
  
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
          window.providerSettingsViewModel.Provider.Id,
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

  const handleAddPaymentMethod = () => {
    setShowAddPaymentMethod(true);
  };

  const handleCloseAddPaymentMethod = () => {
    setShowAddPaymentMethod(false);
  };

  const handlePaymentMethodAdded = () => {};

  // Handler for viewing banks
  const handleViewBanks = (setting: any) => {
    setSelectedSetting(setting);
    setShowBanksPopup(true);
  };

  const handleCloseBanksPopup = () => {
    setShowBanksPopup(false);
    setSelectedSetting(null);
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
    setSelectedPaymentMethod('');
    setSelectedStatus('');
    setSelectedTransactionType('');
    setProviderStatus(window.providerSettingsViewModel.Provider.Status);
    setUpdateMessage(null);
  };

  // Handler for update button
  const handleUpdate = async () => {
    try {
      setUpdating(true);
      setUpdateMessage(null);
      
      var provider = {
        id: window.providerSettingsViewModel.Provider.Id,
        name: window.providerSettingsViewModel.Provider.Name,
        status: providerStatus ? 1 : 0,
        providerSettings: providerSettings
      };

      // Call the API to update provider
      const result = await paymentProviderService.updateProvider(provider);
      
      if (result.isSuccessful) {
        setUpdateMessage({
          type: 'success',
          text: tGlobal('updateSuccess')
        });
        // Update original settings to match current settings after successful update
        setOriginalSettings([...providerSettings]);
      } else {
        setUpdateMessage({
          type: 'error',
          text: result.message || tGlobal('updateFailed')
        });
      }
    } catch (err) {
      console.error('Failed to update provider:', err);
      setUpdateMessage({
        type: 'error',
        text: tGlobal('updateFailed')
      });
    } finally {
      setUpdating(false);
    }
  };

  // Handler for provider status toggle
  const handleProviderStatusToggle = () => {
    setProviderStatus(!providerStatus);
    // You might want to add API call here to update the provider status on the server
  };

  // const getPaymentMethodName = (id: number) => {
    // return window.providerSettingsViewModel.PaymentMethods.find((method: any) => method.Id === id)?.Name || id;
  // };

  const getStatusName = (id: number) => {
    return STATUS_OPTIONS.find(status => status.id === id)?.name || id;
  };

  const getTransactionTypeName = (id: number) => {
    return TRANSACTION_TYPES.find(type => type.id === id)?.name || id;
  };

  return (
    <div className="payment-provider-settings">
      <div className="settings-header">
        <h2>{window.providerSettingsViewModel.Provider.Name}</h2>
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
            <label className="toggle-label" htmlFor='enableToggle'>
              {tGlobal('enable')}/{tGlobal('disable')}
            </label>
            <div className="settings-footer">
              <button className="btn btn-primary me-2" onClick={handleAddPaymentMethod}>{tManagePayments('addPaymentMethod')}</button>
            </div>
            <div className="toggle-container">
              <input
                type="checkbox"
                id="enableToggle"
                checked={providerStatus}
                onChange={handleProviderStatusToggle}
                className="toggle-checkbox"
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
              {window.providerSettingsViewModel.PaymentMethods.map((method: any) => (
                <option key={method.Id} value={method.Id}>{method.Name}</option>
              ))}
            </select>
          </div>

          <div className="form-group">
            <label className="form-label">{tManagePayments('status')}</label>
            <select 
              className="form-select" 
              value={selectedStatus}
              onChange={handleStatusChange}
            >
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
              {TRANSACTION_TYPES.map(type => (
                <option key={type.id} value={type.id}>{type.name}</option>
              ))}
            </select>
          </div>

          <div className="settings-footer">
            <button className="btn btn-secondary me-2" onClick={handleViewFees}>{tManagePayments('viewProviderFees', {providerName: window.providerSettingsViewModel.Provider.Name})}</button>
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
                    <td>{setting.methodName}</td>
                    <td>{getStatusName(setting.status)}</td>
                    <td>{getTransactionTypeName(setting.transactionType)}</td>
                    <td>
                      <input 
                        type="text" 
                        className="form-control form-control-sm" 
                        value={setting.minAmount.toLocaleString()} 
                        onChange={(e) => handleMinAmountChange(index, e.target.value)}
                        placeholder={tManagePayments('minimum')}
                      />
                      <label className="form-label">{tManagePayments('minAmount')} {setting.minAmountTrans.toLocaleString()}</label>
                    </td>
                    <td>
                      <input 
                        type="text" 
                        className="form-control form-control-sm" 
                        value={setting.maxAmount.toLocaleString()} 
                        onChange={(e) => handleMaxAmountChange(index, e.target.value)}
                        placeholder={tManagePayments('maximum')}
                      />
                      <label className="form-label">{tManagePayments('maxAmount')} {setting.maxAmountTrans.toLocaleString()}</label>
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
                          onClick={() => handleViewBanks(setting)}
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
              <button 
                className="btn btn-success" 
                onClick={handleUpdate}
                disabled={updating}
              >
                {updating ? tGlobal('updating') : tGlobal('update')}
              </button>
            </div>
            
            {updateMessage && (
              <div className={`alert alert-${updateMessage.type === 'success' ? 'success' : 'danger'} mt-3`}>
                {updateMessage.text}
              </div>
            )}
          </div>

        </>
      )}

      {/* Fee Popup Modal */}
      {showFeePopup && (
        <ProviderFeesPopupModal
          provider={{id: window.providerSettingsViewModel.Provider.Id, name: window.providerSettingsViewModel.Provider.Name} as PaymentProvider}
          onClose={handleCloseFeePopup}
        />
      )}

      {/* Add Payment Method Modal */}
      {showAddPaymentMethod && (
        <AddPaymentMethodModal
          providerId={window.providerSettingsViewModel.Provider.Id}
          onClose={handleCloseAddPaymentMethod}
          onSave={handlePaymentMethodAdded}
        />
      )}

      {/* Banks Popup Modal */}
      {showBanksPopup && selectedSetting && (
        <BanksPopupModal
          onClose={handleCloseBanksPopup} 
          transactionType={selectedSetting.transactionType}
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