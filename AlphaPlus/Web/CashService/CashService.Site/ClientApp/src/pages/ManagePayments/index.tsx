import * as React from 'react';
import { createRoot } from 'react-dom/client';
import { useTranslation } from 'react-i18next';
import '../../i18n/i18n';
import './ManagePayments.css';

function ManagePayments() {
  const { t } = useTranslation('managePayments');

  return (
    <div className="container">
      <div className="d-flex justify-content-between align-items-center mb-4">
        <div className="d-flex align-items-center">
          <h1 className="mb-0 me-2 inline-elm">{t('managePayments')}</h1>
          <i className="bi bi-info-circle fs-4 ico-info inline-elm" title={t('info')}></i>
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