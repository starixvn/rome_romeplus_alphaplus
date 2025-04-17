import * as React from 'react';
import { createRoot } from 'react-dom/client';
import { useTranslation } from 'react-i18next';
import Tippy from '@tippyjs/react';
import 'tippy.js/dist/tippy.css'; 
import '../../i18n/i18n';
import './ManagePayments.css';
import PaymentGateway from './PaymentGateway';

function ManagePayments() {
  const { t } = useTranslation('managePayments');
  const [activeTab, setActiveTab] = React.useState('paymentGateway');

  return (
    <div className="container">
      <div className="d-flex justify-content-between align-items-center mb-4">
        <div className="d-flex align-items-center">
          <h1 className="mb-0 me-2 inline-elm">{t('managePayments')}</h1>
          <Tippy
            content={
              <div>
                <div>
                  <i className="bi bi-info-circle fs-4 ico-info inline-elm"></i> 
                  {t('managePayments')}
                </div>
                <div>• {t('managePaymentsTip1')}</div>
                <div>• {t('managePaymentsTip2')}</div>
              </div>
            }
            placement="top"
            theme="light"
            arrow={true}
          >
            <i className="bi bi-info-circle fs-4 ico-info inline-elm"></i>
          </Tippy>
        </div>
      </div>

      <div className="card">
        <div className="card-header">
          <ul className="nav nav-tabs card-header-tabs">
            <li className="nav-item">
              <button
                className={`nav-link ${activeTab === 'paymentGateway' ? 'active' : ''}`}
                onClick={() => setActiveTab('paymentGateway')}
              >
                Payment Gateway
              </button>
            </li>
            <li className="nav-item">
              <button
                className={`nav-link ${activeTab === 'bankAccount' ? 'active' : ''}`}
                onClick={() => setActiveTab('bankAccount')}
              >
                Bank Account
              </button>
            </li>
          </ul>
        </div>
        <div className="card-body">
          {activeTab === 'paymentGateway' && (
            <div className="payment-gateway-content">
              <PaymentGateway />
            </div>
          )}
          {activeTab === 'bankAccount' && (
            <div className="bank-account-content">
              {/* Bank Account content will go here */}
              <p>Bank Account content</p>
            </div>
          )}
        </div>
      </div>
    </div>
  );
}

createRoot(document.getElementById('root')!).render(
  <React.StrictMode>
    <ManagePayments />
  </React.StrictMode>
); 