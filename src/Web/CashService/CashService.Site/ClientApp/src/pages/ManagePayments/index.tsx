import * as React from 'react';
import { createRoot } from 'react-dom/client';

import PaymentGatewayComponent from './PaymentGatewayComponent';
import BankAccountComponent from './BankAccountComponent';
import i18n from '../../i18n/i18n-managePayments';
import { useCookieLanguage } from '../../hooks/useCookieLanguage';
import '../../styles/common.css';
import '../../styles/global.scss';

function ManagePayments() {
  useCookieLanguage(i18n);
  const [activeTab, setActiveTab] = React.useState('paymentGateway');

  return (
    <>
      {activeTab === 'paymentGateway' && (
        <div className="payment-gateway-content">
          <PaymentGatewayComponent setActiveTab={setActiveTab} activeTab={activeTab} />
        </div>
      )}
      {activeTab === 'bankAccount' && (
        <div className="bank-account-content h-full">
          <BankAccountComponent setActiveTab={setActiveTab} activeTab={activeTab} />
        </div>
      )}
    </>
  );
}

createRoot(document.getElementById('root')!).render(
  <React.StrictMode>
    <ManagePayments />
  </React.StrictMode>
); 