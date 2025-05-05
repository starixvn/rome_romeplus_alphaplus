import * as React from 'react';
import { createRoot } from 'react-dom/client';
import { useTranslation } from 'react-i18next';
import 'tippy.js/dist/tippy.css'; 
import './ManagePayments.css';
import PaymentGateway from './PaymentGateway';
import BankAccount from './BankAccount';
import { useCookieLanguage } from '../../hooks/useCookieLanguage';
import TitleCommon from '../../components/ui/TitleCommon';

function ManagePayments() {
  const { t } = useTranslation('managePayments');
  useCookieLanguage();
  const [activeTab, setActiveTab] = React.useState('paymentGateway');

  return (
    <div className="p-[12px]">
      <TitleCommon title={t('managePayments')} />

      <div className="card">
        <div className="card-header">
          <ul className="flex">
            <li className="mr-[8px]">
              <button
                  className={`font-bold text-[12px] tracking-[0.12px] leading-[36px] pr-[16px] pl-[16px] rounded-t-[4px] ${activeTab === 'paymentGateway' ? ' bg-[#163f78] text-[#ffffff] cursor-pointer' : 'bg-[#dbe2ef] text-[#163f78] cursor-pointer'}`}
                  onClick={() => setActiveTab('paymentGateway')}
                >
                {t('paymentGateway')}
              </button>
            </li>
            <li>
              <button
                  className={`font-bold text-[12px] tracking-[0.12px] leading-[36px] pr-[16px] pl-[16px] rounded-t-[4px] ${activeTab === 'bankAccount' ? ' bg-[#163f78] text-[#ffffff] cursor-pointer' : 'bg-[#dbe2ef] text-[#163f78] cursor-pointer'}`}
                  onClick={() => setActiveTab('bankAccount')}
                >
                {t('bankAccount')}
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
              <BankAccount />
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