import * as React from 'react';
import { createRoot } from 'react-dom/client';
import { useTranslation } from 'react-i18next';
import LanguageSelector from '../../components/LanguageSelector';
import '../../i18n/i18n';
import './ManagePayments.css';

function ManagePayments() {
  const { t } = useTranslation('managePayments');

  return (
    <div className="container">
      <div className="d-flex justify-content-between align-items-center mb-4">
        <h1>{t('managePayments')}</h1>
        <LanguageSelector />
      </div>
      
      <p className="mb-4">{t('description')}</p>
      
      <div className="row mb-4">
        <div className="col-md-3">
          <button className="btn btn-primary w-100">
            {t('actions.add')}
          </button>
        </div>
        <div className="col-md-9">
          <div className="input-group">
            <input 
              type="text" 
              className="form-control" 
              placeholder={t('filters.search')} 
            />
            <button className="btn btn-outline-secondary" type="button">
              {t('filters.search')}
            </button>
          </div>
        </div>
      </div>
      
      <div className="card">
        <div className="card-body">
          <div className="table-responsive">
            <table className="table table-striped">
              <thead>
                <tr>
                  <th>{t('table.id')}</th>
                  <th>{t('table.date')}</th>
                  <th>{t('table.amount')}</th>
                  <th>{t('table.status')}</th>
                  <th>{t('table.description')}</th>
                  <th>{t('table.actions')}</th>
                </tr>
              </thead>
              <tbody>
                <tr>
                  <td colSpan={6} className="text-center">
                    {t('messages.noPayments')}
                  </td>
                </tr>
              </tbody>
            </table>
          </div>
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