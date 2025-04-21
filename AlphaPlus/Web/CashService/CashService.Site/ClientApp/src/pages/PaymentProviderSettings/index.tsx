import * as React from 'react';
import { createRoot } from 'react-dom/client';
import { useTranslation } from 'react-i18next';
import '../../i18n/i18n';
import './PaymentProviderSettings.css';

// Extend Window interface to include providerName
declare global {
  interface Window {
    providerName: string;
  }
}

function PaymentProviderSettings() {
  const { t: tGlobal } = useTranslation('global');
  const { t: tManagePayments } = useTranslation('managePayments');
  const [isEnabled, setIsEnabled] = React.useState(false);

  const handleToggle = (event: React.ChangeEvent<HTMLInputElement>) => {
    setIsEnabled(event.target.checked);
  };

  return (
    <div>
      <h1>{window.providerName}</h1>
      <label>{tManagePayments('setUpPaymentMethodsForMembers')}</label>
      <label>{tGlobal('enable')}</label>/<label>{tGlobal('disable')}</label>
      <div>
        <button>{tManagePayments('addPaymentMethod')}</button>
        <input
          type="checkbox"
          id="enableToggle"
          checked={isEnabled}
          onChange={handleToggle}
          className="toggle-checkbox"
        />
      </div>
    </div>
  );
};

createRoot(document.getElementById('root')!).render(
  <React.StrictMode>
    <PaymentProviderSettings />
  </React.StrictMode>
); 