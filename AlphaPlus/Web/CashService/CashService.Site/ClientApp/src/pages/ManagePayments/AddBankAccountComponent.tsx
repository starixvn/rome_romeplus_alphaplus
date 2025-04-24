import React, { useState } from 'react';
import { useTranslation } from 'react-i18next';
import { Bank } from '../../services/interfaces/Bank';
import { paymentProviderService } from '../../services/paymentsAPI';

interface AddBankAccountComponentProps {
  onClose: () => void;
  onSave: () => void;
  banks: Bank[];
}

const AddBankAccountComponent: React.FC<AddBankAccountComponentProps> = ({
  onClose,
  onSave,
  banks
}) => {
  const { t: tManagePayments } = useTranslation('managePayments');
  const { t: tGlobal } = useTranslation('global');
  
  const [accountNickname, setAccountNickname] = useState<string>('');
  const [accountHolderName, setAccountHolderName] = useState<string>('');
  const [accountNumber, setAccountNumber] = useState<string>('');
  const [selectedBank, setSelectedBank] = useState<string>('');
  const [status, setStatus] = useState<string>('1'); // Default to active
  const [depositType, setDepositType] = useState<string>('1');
  const [withdrawalType, setWithdrawalType] = useState<string>('1');
  const [depositMatchingMode, setDepositMatchingMode] = useState<string>('1');
  const [promptPay, setPromptPay] = useState<boolean>(false);
  const [mobile, setMobile] = useState<string>('');
  const [error, setError] = useState<string | null>(null);
  const [success, setSuccess] = useState<boolean>(false);
  const [successMessage, setSuccessMessage] = useState<string>('');
  const [loading, setLoading] = useState<boolean>(false);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError(null);
    setSuccess(false);
    setLoading(true);

    try {
      const result = await paymentProviderService.addBankAccount({
        id: 0, // Temporary ID for new bank account
        accountNickname,
        accountHolderName,
        accountNumber,
        bankId: parseInt(selectedBank),
        bankName: banks.find(b => b.id === parseInt(selectedBank))?.name || '',
        transactionTypes: '',
        status: parseInt(status),
        depositMatchingMode: parseInt(depositMatchingMode),
        promptPay,
        mobile,
        lastUpdated: new Date().toISOString(),
        lastUpdatedStr: new Date().toLocaleString()
      });

      if (result.isSuccessful) {
        setSuccess(true);
        setSuccessMessage(result.message || tManagePayments('bankAccountAdded'));
        setTimeout(() => {
          onSave();
        }, 3000);
      } else {
        setError(result.message || tManagePayments('failedToAddBankAccount'));
      }
    } catch (err) {
      setError(tManagePayments('failedToAddBankAccount'));
      console.error('Error adding bank account:', err);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="modal show d-block" tabIndex={-1}>
      <div className="modal-dialog">
        <div className="modal-content">
          <div className="modal-header">
            <h5 className="modal-title">{tManagePayments('bankManagement')}</h5>
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
                <label className="form-label">{tManagePayments('bankStatus')}</label>
                <label className="form-label">{tManagePayments('bankNote1')}</label>
                <div className="btn-group" role="group">
                  <button
                    type="button"
                    className={`btn ${status === '1' ? 'btn-success' : 'btn-outline-success'}`}
                    onClick={() => setStatus('1')}
                  >
                    {tManagePayments('active')}
                  </button>
                  <button
                    type="button"
                    className={`btn ${status === '2' ? 'btn-danger' : 'btn-outline-danger'}`}
                    onClick={() => setStatus('2')}
                  >
                    {tManagePayments('inactive')}
                  </button>
                </div>
              </div>

              <div className="mb-3">
                <label className="form-label">{tManagePayments('accountInformation')}</label>
              </div>
              <div className="mb-3">
                <label className="form-label">{tManagePayments('bank')}</label>
              </div>
              
              <div className="mb-3">
                <label className="form-label">{tManagePayments('bank')}</label>
                <select
                  className="form-select"
                  value={selectedBank}
                  onChange={(e) => setSelectedBank(e.target.value)}
                  required
                >
                  <option value="">{tGlobal('select')}</option>
                  {banks.map((bank) => (
                    <option key={bank.id} value={bank.id}>
                      {bank.name}
                    </option>
                  ))}
                </select>
              </div>

              <div className="mb-3">
                <label className="form-label">{tManagePayments('depositType')}</label>
                <div className="btn-group" role="group">
                  <button
                    type="button"
                    className={`btn ${depositType === '1' ? 'btn-primary' : 'btn-outline-primary'}`}
                    onClick={() => setDepositType('1')}
                  >
                    {tManagePayments('slipUpload')}
                  </button>
                  <button
                    type="button"
                    className={`btn ${depositType === '2' ? 'btn-primary' : 'btn-outline-primary'}`}
                    onClick={() => setDepositType('2')}
                  >
                    {tManagePayments('sms')}
                    <br />
                    {tManagePayments('autoDeposit')}
                  </button>
                </div>
              </div>

              <div className="mb-3">
                <label className="form-label">{tManagePayments('withdrawalType')}</label>
                <div className="btn-group" role="group">
                  <button
                    type="button"
                    className={`btn ${depositType === '1' ? 'btn-primary' : 'btn-outline-primary'}`}
                    onClick={() => setWithdrawalType('1')}
                  >
                    {tManagePayments('manualWithdrawal')}
                  </button>
                </div>
              </div>

              <div className="mb-3">
                <label className="form-label">{tManagePayments('depositMatchingMode')}</label>
                <div className="btn-group" role="group">
                  <button
                    type="button"
                    className={`btn ${depositMatchingMode === '1' ? 'btn-primary' : 'btn-outline-primary'}`}
                    onClick={() => setDepositMatchingMode('1')}
                  >
                    {tManagePayments('manualMatching')}
                  </button>
                  <button
                    type="button"
                    className={`btn ${depositMatchingMode === '2' ? 'btn-primary' : 'btn-outline-primary'}`}
                    onClick={() => setDepositMatchingMode('2')}
                  >
                    {tManagePayments('autoMatching')}
                  </button>
                </div>
              </div>

              <div className="mb-3">
                <label className="form-label">{tManagePayments('accountHolderName')}</label>
                <label className="text-muted">{tManagePayments('accountHolderNameNote1')}</label>
                <input
                  type="text"
                  className="form-control"
                  value={accountHolderName}
                  onChange={(e) => setAccountHolderName(e.target.value)}
                  required
                />

                <label className="text-muted">{tManagePayments('bankAccountNumber')}</label>
                <input
                  type="text"
                  className="form-control"
                  value={accountNumber}
                  onChange={(e) => setAccountNumber(e.target.value)}
                  required
                />

                <label className="text-muted">{tManagePayments('accountNickname')}</label>
                <label className="text-muted">{tManagePayments('accountNicknameNote1')}</label>
                <input
                  type="text"
                  className="form-control"
                  value={accountNickname}
                  onChange={(e) => setAccountNickname(e.target.value)}
                  required
                />
              </div>

              <div className="mb-3">
                <label className="form-label">{tManagePayments('promptPay')}</label>
                <label className="text-muted">{tManagePayments('linkMobile')}</label>
                <div className="btn-group" role="group">
                  <button
                    type="button"
                    className={`btn ${promptPay ? 'btn-success' : 'btn-outline-success'}`}
                    onClick={() => setPromptPay(true)}
                  >
                    {tManagePayments('on')}
                  </button>
                  <button
                    type="button"
                    className={`btn ${!promptPay ? 'btn-danger' : 'btn-outline-danger'}`}
                    onClick={() => setPromptPay(false)}
                  >
                    {tManagePayments('off')}
                  </button>
                </div>
              </div>

              
              <div className="mb-3">
                <label className="form-label">{tManagePayments('mobile')}</label>
                <input
                  type="text"
                  className="form-control"
                  value={mobile}
                  onChange={(e) => setMobile(e.target.value)}
                  required
                />
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

export default AddBankAccountComponent; 