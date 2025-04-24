import React, { useState, useEffect } from 'react';
import { useTranslation } from 'react-i18next';
import { paymentProviderService } from '../../services/paymentsAPI';

interface SetTransactionLimitModalProps {
  onClose: () => void;
  onSave: () => void;
}

const SetTransactionLimitModal: React.FC<SetTransactionLimitModalProps> = ({
  onClose,
  onSave
}) => {
  const { t: tManagePayments } = useTranslation('managePayments');
  const { t: tGlobal } = useTranslation('global');
  
  const [minDeposit, setMinDeposit] = useState<string>('');
  const [maxDeposit, setMaxDeposit] = useState<string>('');
  const [minWithdrawal, setMinWithdrawal] = useState<string>('');
  const [maxWithdrawal, setMaxWithdrawal] = useState<string>('');
  const [dailyWithdrawalCount, setDailyWithdrawalCount] = useState<string>('');
  const [error, setError] = useState<string | null>(null);
  const [success, setSuccess] = useState<boolean>(false);
  const [successMessage, setSuccessMessage] = useState<string>('');
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    const fetchTransactionLimit = async () => {
      try {
        const limit = await paymentProviderService.getTransactionLimit();
        setMinDeposit(limit.minPerDepositTransaction.toString());
        setMaxDeposit(limit.maxPerDepositTransaction.toString());
        setMinWithdrawal(limit.minPerWithdrawalTransaction.toString());
        setMaxWithdrawal(limit.maxPerWithdrawalTransaction.toString());
        setDailyWithdrawalCount(limit.dailyWithdrawalCount.toString());
      } catch (err) {
        setError(tManagePayments('failedToFetchLimit'));
        console.error('Error fetching transaction limit:', err);
      }
    };

    fetchTransactionLimit();
  }, [tManagePayments]);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError(null);
    setSuccess(false);
    setLoading(true);

    try {
      const result = await paymentProviderService.setTransactionLimit({
        dailyWithdrawalCount: parseInt(dailyWithdrawalCount) || 0,
        minPerDepositTransaction: parseFloat(minDeposit) || 0,
        maxPerDepositTransaction: parseFloat(maxDeposit) || 0,
        minPerWithdrawalTransaction: parseFloat(minWithdrawal) || 0,
        maxPerWithdrawalTransaction: parseFloat(maxWithdrawal) || 0
      });

      if (result.isSuccessful) {
        setSuccess(true);
        setSuccessMessage(tManagePayments('transactionLimitUpdated'));
        setTimeout(() => {
          onSave();
        }, 3000);
      } else {
        setError(result.message || tManagePayments('failedToSetLimit'));
      }
    } catch (err) {
      setError(tManagePayments('failedToSetLimit'));
      console.error('Error setting transaction limit:', err);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="modal show d-block" tabIndex={-1}>
      <div className="modal-dialog">
        <div className="modal-content">
          <div className="modal-header">
            <h5 className="modal-title">{tManagePayments('transactionLimitPreferences')}</h5>
            <button
              type="button"
              className="btn-close"
              onClick={onClose}
              disabled={loading}
            />
          </div>
          <form onSubmit={handleSubmit}>
            <div className="modal-body">
              {error && (
                <div className="alert alert-danger" role="alert">
                  {error}
                </div>
              )}
              {success && (
                <div className="alert alert-success" role="alert">
                  {successMessage}
                </div>
              )}
              <div className="mb-3">
                <label className="form-label">{tManagePayments('customizeTransactionLimitPreferences')}</label>
              </div>
              <div className="mb-3">
                <label className="form-label">{tManagePayments('deposit')}</label>
              </div>
              <div className="mb-3">
                <label className="form-label">{tManagePayments('minPerTransaction')}</label>
                <input
                  type="text"
                  className="form-control"
                  value={minDeposit}
                  onChange={(e) => setMinDeposit(e.target.value)}
                  min="0"
                  step="0.01"
                  required
                />
              </div>
              <div className="mb-3">
                <label className="form-label">{tManagePayments('maxPerTransaction')}</label>
                <input
                  type="text"
                  className="form-control"
                  value={maxDeposit}
                  onChange={(e) => setMaxDeposit(e.target.value)}
                  min="0"
                  step="0.01"
                  required
                />
              </div>
              <div className="mb-3">
                <label className="form-label">{tManagePayments('withdrawal')}</label>
              </div>
              <div className="mb-3">
                <label className="form-label">{tManagePayments('minPerTransaction')}</label>
                <input
                  type="text"
                  className="form-control"
                  value={minWithdrawal}
                  onChange={(e) => setMinWithdrawal(e.target.value)}
                  min="0"
                  step="0.01"
                  required
                />
              </div>
              <div className="mb-3">
                <label className="form-label">{tManagePayments('maxPerTransaction')}</label>
                <input
                  type="text"
                  className="form-control"
                  value={maxWithdrawal}
                  onChange={(e) => setMaxWithdrawal(e.target.value)}
                  min="0"
                  step="0.01"
                  required
                />
              </div>
              <div className="mb-3">
                <label className="form-label">{tManagePayments('dailyWithdrawalCount')}</label>
                <input
                  type="text"
                  className="form-control"
                  value={dailyWithdrawalCount}
                  onChange={(e) => setDailyWithdrawalCount(e.target.value)}
                  min="0"
                  required
                />
              </div>
              <div className="mb-3">
                <label className="form-label">{tManagePayments('dailyWithdrawalCountTip')}</label>
              </div>
            </div>
            <div className="modal-footer">
              <button
                type="button"
                className="btn btn-secondary"
                onClick={onClose}
                disabled={loading}
              >
                {tGlobal('cancel')}
              </button>
              <button
                type="submit"
                className="btn btn-primary"
                disabled={loading}
              >
                {loading ? tGlobal('saving') : tGlobal('save')}
              </button>
            </div>
          </form>
        </div>
      </div>
    </div>
  );
};

export default SetTransactionLimitModal; 