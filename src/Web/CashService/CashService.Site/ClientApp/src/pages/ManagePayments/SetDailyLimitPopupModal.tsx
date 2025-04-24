import * as React from 'react';
import { useTranslation } from 'react-i18next';
import './PaymentGateway.css';
import { paymentProviderService } from '../../services/paymentsAPI';

interface SetDailyLimitPopupModalProps {
  onClose: () => void;
  onSave: (dailyLimit: number) => void;
}

const SetDailyLimitPopupModal: React.FC<SetDailyLimitPopupModalProps> = ({ onClose, onSave }) => {
  const { t } = useTranslation(['managePayments', 'global']);
  const [dailyLimit, setDailyLimit] = React.useState<number>(0);
  const [isLoading, setIsLoading] = React.useState<boolean>(false);
  const [error, setError] = React.useState<string>('');
  const [successMessage, setSuccessMessage] = React.useState<string>('');

  React.useEffect(() => {
    const loadDailyWithdrawalCount = async () => {
      try {
        const count = await paymentProviderService.getDailyWithdrawalCount();
        setDailyLimit(count);
      } catch (err) {
        setError(t('global:errorOccurred'));
      }
    };

    loadDailyWithdrawalCount();
  }, [t]);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setIsLoading(true);
    setError('');
    setSuccessMessage('');

    try {
      const response = await paymentProviderService.setDailyWithdrawalCount(dailyLimit);
      if (response.isSuccessful) {
        setSuccessMessage(t('managePayments:setLimitSuccess'));
        onSave(dailyLimit);
        setTimeout(() => {
          onClose();
        }, 3000);
      } else {
        setError(response.message);
      }
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
                type="text"
                className="form-control"
                id="dailyLimit"
                value={dailyLimit}
                onChange={(e) => setDailyLimit(Number(e.target.value))}
                min="0"
                required
              />
              {error && <div className="text-danger mt-2">{error}</div>}
              {successMessage && <div className="text-success mt-2">{successMessage}</div>}
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