import * as React from 'react';
import { createRoot } from 'react-dom/client';
import { useTranslation } from 'react-i18next';
import './PaymentProviderSettings.css';

const PaymentProviderSettings = () => {
  const { t } = useTranslation('managePayments');
  return (
    <div>
      <h1>{t('paymentProviderSettings')}</h1>
    </div>
  );
};

createRoot(document.getElementById('root')!).render(
  <React.StrictMode>
    <PaymentProviderSettings />
  </React.StrictMode>
); 