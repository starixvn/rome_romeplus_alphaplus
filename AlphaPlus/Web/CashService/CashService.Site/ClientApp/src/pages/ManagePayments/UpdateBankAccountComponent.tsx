import React, { useState } from 'react';
import { useTranslation } from 'react-i18next';
import { Bank } from '../../services/interfaces/Bank';
import { BankAccount } from '../../services/interfaces/BankAccount';
import { paymentProviderService } from '../../services/paymentsAPI';

interface UpdateBankAccountComponentProps {
  onClose: () => void;
  onSave: () => void;
  banks: Bank[];
  bankAccount: BankAccount;
}

const UpdateBankAccountComponent: React.FC<UpdateBankAccountComponentProps> = ({
  onClose,
  onSave,
  banks,
  bankAccount
}) => {
  const { t: tManagePayments } = useTranslation('managePayments');
  const { t: tGlobal } = useTranslation('global');
  
  const [accountNickname, setAccountNickname] = useState<string>(bankAccount.accountNickname);
  const [accountHolderName] = useState<string>(bankAccount.accountHolderName);
  const [accountNumber] = useState<string>(bankAccount.accountNumber);
  const [selectedBank] = useState<string>(bankAccount.bank.id.toString());
  const [status, setStatus] = useState<string>(bankAccount.status.toString());
  const [depositType, setDepositType] = useState<string>(bankAccount.depositType.toString());
  const [withdrawalType, setWithdrawalType] = useState<string>(bankAccount.withdrawalType.toString());
  const [depositMatchingMode, setDepositMatchingMode] = useState<string>(bankAccount.depositMatchingMode.toString());
  const [promptPay, setPromptPay] = useState<boolean>(bankAccount.promptPay);
  const [mobile] = useState<string>(bankAccount.mobile);
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
      const result = await paymentProviderService.updateBankAccount({
        id: bankAccount.id,
        accountNickname,
        accountHolderName,
        accountNumber,
        bank: {
          id: parseInt(selectedBank),
          name: banks.find(b => b.id === parseInt(selectedBank))?.name || ''
        },
        transactionType: bankAccount.transactionType,
        status: parseInt(status),
        depositType: parseInt(depositType),
        withdrawalType: parseInt(withdrawalType),
        depositMatchingMode: parseInt(depositMatchingMode),
        promptPay,
        mobile,
        lastUpdated: new Date().toISOString(),
        lastUpdatedStr: new Date().toLocaleString()
      });

      if (result.isSuccessful) {
        setSuccess(true);
        setSuccessMessage(result.message || tManagePayments('bankAccountUpdated'));
        setTimeout(() => {
          onSave();
        }, 3000);
      } else {
        setError(result.message || tManagePayments('failedToUpdateBankAccount'));
      }
    } catch (err) {
      setError(tManagePayments('failedToUpdateBankAccount'));
      console.error('Error updating bank account:', err);
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
                <select
                  className="form-select"
                  value={selectedBank}
                  disabled
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
                    className={`btn ${withdrawalType === '1' ? 'btn-primary' : 'btn-outline-primary'}`}
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
                <input
                  type="text"
                  className="form-control"
                  value={accountHolderName}
                  readOnly
                  required
                />

                <label className="text-muted">{tManagePayments('bankAccountNumber')}</label>
                <input
                  type="text"
                  className="form-control"
                  value={accountNumber}
                  readOnly
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
                  readOnly
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

export default UpdateBankAccountComponent; 