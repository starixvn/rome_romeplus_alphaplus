import * as React from 'react';
import { useTranslation } from 'react-i18next';
import { PaymentProvider } from '../../services/paymentsAPI';

interface FeePopupModalProps {
  provider: PaymentProvider;
  onClose: () => void;
}

const FeePopupModal: React.FC<FeePopupModalProps> = ({ provider, onClose }) => {
  const { t } = useTranslation('managePayments');

  return (
    <>
      <div className="modal fade show" style={{ display: 'block' }} tabIndex={-1}>
        <div className="modal-dialog">
          <div className="modal-content">
            <div className="modal-header">
              <h5 className="modal-title">{t('feeDetails')} - {provider.name}</h5>
              <button type="button" className="btn-close" onClick={onClose}></button>
            </div>
            <div className="modal-body">
              <div>{provider.name} {t('fees')}</div>
              <b>{t('howItWorks')}</b>
              <p>{t('feesInfo1')}</p>
              <p>{t('feesFormula')}</p>
              <p>{t('feesInfo2')}</p>
              
              <table className="table table-striped mt-3">
                <thead>
                  <tr>
                    <th>{t('transactionType')}</th>
                    <th>{t('paymentMethod')}</th>
                    <th>{t('feeRate')}</th>
                  </tr>
                </thead>
                <tbody>
                  <tr>
                    <td>{t('deposit')}</td>
                    <td>{t('bankTransfer')}</td>
                    <td>0%</td>
                  </tr>
                  <tr>
                    <td>{t('deposit')}</td>
                    <td>{t('card')}</td>
                    <td>2.9% + $0.30</td>
                  </tr>
                  <tr>
                    <td>{t('withdrawal')}</td>
                    <td>{t('bankTransfer')}</td>
                    <td>0%</td>
                  </tr>
                  <tr>
                    <td>{t('withdrawal')}</td>
                    <td>{t('card')}</td>
                    <td>2.9%</td>
                  </tr>
                </tbody>
              </table>
            </div>
            <div className="modal-footer">
              <button type="button" className="btn btn-secondary" onClick={onClose}>
                {t('close')}
              </button>
            </div>
          </div>
        </div>
      </div>
      <div className="modal-backdrop fade show"></div>
    </>
  );
};

export default FeePopupModal; 