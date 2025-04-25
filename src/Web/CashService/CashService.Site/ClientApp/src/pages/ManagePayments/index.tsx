import * as React from 'react';
import { createRoot } from 'react-dom/client';
import { useTranslation } from 'react-i18next';
import Tippy from '@tippyjs/react';
import 'tippy.js/dist/tippy.css'; 
import '../../i18n/i18n';
import './ManagePayments.css';
import './ManagePayments.module.scss';
import PaymentGateway from './PaymentGateway';
import BankAccount from './BankAccount';
import { IconInfo } from '../../components/Icons';

function ManagePayments() {
  const { t } = useTranslation('managePayments');
  const [activeTab, setActiveTab] = React.useState('bankAccount');

  return (
    <div className="p-[12px]">
      <div className="mb-[8px]">
        <h1 className="flex items-center font-bold text-[15px]">
          {t('managePayments')}
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
            <span><IconInfo /></span>
          </Tippy>
        </h1>
      </div>

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