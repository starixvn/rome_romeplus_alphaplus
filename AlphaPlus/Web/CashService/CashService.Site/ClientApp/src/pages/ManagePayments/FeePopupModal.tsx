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
              {/* Add your fee details content here */}
              <p>{t('feeDetailsContent')}</p>
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