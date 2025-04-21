import * as React from 'react';
import { useTranslation } from 'react-i18next';
import './PaymentGateway.css';
import { paymentProviderService } from '../../services/paymentsAPI';

interface SetDailyLimitPopupModalProps {
  onClose: () => void;
  onSave: (dailyLimit: number) => void;
  custId: number;
}

const SetDailyLimitPopupModal: React.FC<SetDailyLimitPopupModalProps> = ({ onClose, onSave, custId }) => {
  const { t } = useTranslation(['managePayments', 'global']);
  const [dailyLimit, setDailyLimit] = React.useState<number>(0);
  const [isLoading, setIsLoading] = React.useState<boolean>(false);
  const [error, setError] = React.useState<string>('');

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setIsLoading(true);
    setError('');

    try {
      await paymentProviderService.setDailyWithdrawalCount(custId, dailyLimit);
      onSave(dailyLimit);
      onClose();
    } catch (err) {
      setError(t('global:errorOccurred'));
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className="modal-overlay">
      <div className="modal-content">
        <div className="modal-header">
          <h5 className="modal-title">{t('managePayments:setDailyWithdrawalCount')}</h5>
          <button type="button" className="btn-close" onClick={onClose}></button>
        </div>
        <form onSubmit={handleSubmit}>
          <div className="modal-body">
            <div className="form-group mb-3">
              <div>
                <label className="form-label">{t('managePayments:setMaxDailyWithdrawalCountForYourMembers')}</label>
              </div>
              <div>
                <label htmlFor="dailyLimit" className="form-label">{t('managePayments:dailyWithdrawalCount')}</label>
              </div>
              <input
                type="number"
                className="form-control"
                id="dailyLimit"
                value={dailyLimit}
                onChange={(e) => setDailyLimit(Number(e.target.value))}
                min="0"
                required
              />
              {error && <div className="text-danger mt-2">{error}</div>}
            </div>
          </div>
          <div className="modal-footer">
            <button type="button" className="btn btn-secondary" onClick={onClose}>
              {t('global:cancel')}
            </button>
            <button type="submit" className="btn btn-primary" disabled={isLoading}>
              {isLoading ? t('global:loading') : t('global:save')}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};

export default SetDailyLimitPopupModal; 