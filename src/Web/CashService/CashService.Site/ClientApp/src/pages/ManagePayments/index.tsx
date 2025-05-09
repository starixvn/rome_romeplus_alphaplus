import * as React from 'react';
import { createRoot } from 'react-dom/client';
// import 'tippy.js/dist/tippy.css'; 
import './common.css';
// import '../../styles/global.scss';
import PaymentGateway from './PaymentGateway';
import BankAccount from './BankAccount';
import i18n from '../../i18n/i18n-managePayments';
import { useCookieLanguage } from '../../hooks/useCookieLanguage';

function ManagePayments() {
  useCookieLanguage(i18n);
  const [activeTab, setActiveTab] = React.useState('paymentGateway');

  return (
    <>
      {activeTab === 'paymentGateway' && (
        <div className="payment-gateway-content">
          <PaymentGateway setActiveTab={setActiveTab} activeTab={activeTab} />
        </div>
      )}
      {activeTab === 'bankAccount' && (
        <div className="bank-account-content h-[calc(100%-24px)]">
          <BankAccount setActiveTab={setActiveTab} activeTab={activeTab} />
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