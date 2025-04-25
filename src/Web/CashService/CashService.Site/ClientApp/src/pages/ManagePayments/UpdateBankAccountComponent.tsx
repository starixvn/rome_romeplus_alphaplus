import React, { useState, useEffect } from 'react';
import { useTranslation } from 'react-i18next';
import { Bank } from '../../services/interfaces/Bank';
import { BankAccount } from '../../services/interfaces/BankAccount';
import { MobilePrefix } from '../../services/interfaces/MobilePrefix';
import { paymentProviderService } from '../../services/paymentsAPI';
import {
  hasDepositTransactionType,
  hasManualDepositTransactionType,
  hasAutoDepositTransactionType,
  hasWithdrawalTransactionType,
  hasManualWithdrawalTransactionType
} from '../../utils/bankTransactionUtils';
import { BANK_TRANSACTION_TYPES } from '../../constants/paymentConstants';

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
  const [selectedBank, setSelectedBank] = useState<string>(bankAccount.bankId.toString());
  const [status, setStatus] = useState<string>(bankAccount.status.toString());
  const [depositMatchingMode, setDepositMatchingMode] = useState<string>(bankAccount.depositMatchingMode.toString());
  const [promptPay, setPromptPay] = useState<boolean>(bankAccount.promptPay);
  const [mobile, setMobile] = useState<string>(bankAccount.mobile);
  const [mobilePrefix, setMobilePrefix] = useState<string>(bankAccount.mobilePrefix || '');
  const [mobilePrefixes, setMobilePrefixes] = useState<MobilePrefix[]>([]);
  const [error, setError] = useState<string | null>(null);
  const [success, setSuccess] = useState<boolean>(false);
  const [successMessage, setSuccessMessage] = useState<string>('');
  const [loading, setLoading] = useState<boolean>(false);

  // Add state variables for selected transaction types
  const [selectedManualDeposit, setSelectedManualDeposit] = useState<boolean>(false);
  const [selectedAutoDeposit, setSelectedAutoDeposit] = useState<boolean>(false);
  const [selectedManualWithdrawal, setSelectedManualWithdrawal] = useState<boolean>(false);

  useEffect(() => {
    const fetchBankAccount = async () => {
      try {
        setLoading(true);
        const fetchedBankAccount = await paymentProviderService.getBankAccountById(bankAccount.id);
        if (fetchedBankAccount) {
          setAccountNickname(fetchedBankAccount.accountNickname);
          setStatus(fetchedBankAccount.status.toString());
          setDepositMatchingMode(fetchedBankAccount.depositMatchingMode.toString());
          setPromptPay(fetchedBankAccount.promptPay);
          setSelectedBank(fetchedBankAccount.bankId.toString());
          
          // Set transaction types based on bank account's capabilities
          const transactionTypes = fetchedBankAccount.transactionTypes.split(',');
          setSelectedManualDeposit(transactionTypes.includes(BANK_TRANSACTION_TYPES[1].id.toString()));
          setSelectedAutoDeposit(transactionTypes.includes(BANK_TRANSACTION_TYPES[2].id.toString()));
          setSelectedManualWithdrawal(transactionTypes.includes(BANK_TRANSACTION_TYPES[3].id.toString()));
        }
      } catch (err) {
        setError(tManagePayments('failedToLoadBankAccount'));
        console.error('Error fetching bank account:', err);
      } finally {
        setLoading(false);
      }
    };

    const fetchMobilePrefixes = async () => {
      try {
        const prefixes = await paymentProviderService.getMobilePrefixes();
        setMobilePrefixes(prefixes);
      } catch (err) {
        console.error('Error fetching mobile prefixes:', err);
      }
    };

    fetchBankAccount();
    fetchMobilePrefixes();
  }, [bankAccount.id, tManagePayments]);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError(null);
    setSuccess(false);
    setLoading(true);

    try {
      // Combine selected transaction types into a comma-separated string
      const selectedTransactionTypes: string[] = [];
      
      if (selectedManualDeposit) {
        selectedTransactionTypes.push(BANK_TRANSACTION_TYPES[1].id.toString());
      }
      
      if (selectedAutoDeposit) {
        selectedTransactionTypes.push(BANK_TRANSACTION_TYPES[2].id.toString());
      }
      
      if (selectedManualWithdrawal) {
        selectedTransactionTypes.push(BANK_TRANSACTION_TYPES[3].id.toString());
      }
      
      const transactionTypesString = selectedTransactionTypes.join(',');

      const result = await paymentProviderService.updateBankAccount({
        id: bankAccount.id,
        accountNickname,
        accountHolderName,
        accountNumber,
        bankId: parseInt(selectedBank),
        bankName: banks.find(b => b.id === parseInt(selectedBank))?.name || '',
        transactionTypes: transactionTypesString,
        status: parseInt(status),
        depositMatchingMode: parseInt(depositMatchingMode),
        promptPay,
        mobile,
        mobilePrefix,
        lastUpdated: new Date().toISOString(),
        lastUpdatedStr: new Date().toLocaleString()
      });

      if (result.isSuccessful) {
        setSuccess(true);
        setSuccessMessage(result.message || tManagePayments('bankAccountUpdatedSuccessfully'));
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

  // Toggle handlers for transaction type buttons
  const toggleManualDeposit = () => {
    setSelectedManualDeposit(!selectedManualDeposit);
  };

  const toggleAutoDeposit = () => {
    setSelectedAutoDeposit(!selectedAutoDeposit);
  };

  const toggleManualWithdrawal = () => {
    setSelectedManualWithdrawal(!selectedManualWithdrawal);
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
                    {tGlobal('enable')}
                  </button>
                  <button
                    type="button"
                    className={`btn ${status === '2' ? 'btn-danger' : 'btn-outline-danger'}`}
                    onClick={() => setStatus('2')}
                  >
                    {tGlobal('disable')}
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
                  {banks.map((bank) => (
                    <option key={bank.id} value={bank.id} selected={bank.id === parseInt(selectedBank)}>
                      {bank.name}
                    </option>
                  ))}
                </select>
              </div>

              {hasDepositTransactionType(selectedBank, banks) && (
                <div className="mb-3">
                  <label className="form-label">{tManagePayments('depositType')}</label>
                  <div className="btn-group" role="group">
                    {hasManualDepositTransactionType(selectedBank, banks) && (
                      <button
                        type="button"
                        className={`btn ${selectedManualDeposit ? 'btn-primary' : 'btn-slip-upload'}`}
                        onClick={toggleManualDeposit}
                      >
                        {tManagePayments('slipUpload')}
                        <br />
                        {tManagePayments('manualDeposit')}
                      </button>
                    )}
                    {hasAutoDepositTransactionType(selectedBank, banks) && (
                      <button
                        type="button"
                        className={`btn ${selectedAutoDeposit ? 'btn-primary' : 'btn-auto-deposit'}`}
                        onClick={toggleAutoDeposit}
                      >
                        {tManagePayments('sms')}
                        <br />
                        {tManagePayments('autoDeposit')}
                      </button>
                    )}
                  </div>
                </div>
              )}

              {hasWithdrawalTransactionType(selectedBank, banks) && (
                <div className="mb-3">
                  <label className="form-label">{tManagePayments('withdrawalType')}</label>
                  <div className="btn-group" role="group">
                    {hasManualWithdrawalTransactionType(selectedBank, banks) && (
                      <button
                        type="button"
                        className={`btn ${selectedManualWithdrawal ? 'btn-primary' : 'btn-manual-withdrawal'}`}
                        onClick={toggleManualWithdrawal}
                      >
                        {tManagePayments('manualWithdrawal')}
                      </button>
                    )}
                  </div>
                </div>
              )}

              {(() => {
                const selectedBankObj = banks.find(b => b.id === parseInt(selectedBank));
                return selectedBank && selectedBankObj && selectedBankObj.depositMatchingMode >= 1 && (
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
                      {selectedBankObj.depositMatchingMode >= 2 && (
                        <button
                          type="button"
                          className={`btn ${depositMatchingMode === '2' ? 'btn-primary' : 'btn-outline-primary'}`}
                          onClick={() => setDepositMatchingMode('2')}
                        >
                          {tManagePayments('autoMatching')}
                        </button>
                      )}
                    </div>
                  </div>
                );
              })()}

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
                <div className="input-group">
                  <select
                    className="form-select"
                    value={mobilePrefix}
                    onChange={(e) => setMobilePrefix(e.target.value)}
                    style={{ maxWidth: '120px' }}
                  >
                    {mobilePrefixes.map((prefix) => (
                      <option key={prefix.id} value={prefix.prefix}>
                        {prefix.prefix}
                      </option>
                    ))}
                  </select>
                  <input
                    type="text"
                    className="form-control"
                    value={mobile}
                    onChange={(e) => setMobile(e.target.value)}
                    placeholder={tManagePayments('enterMobileNumber')}
                    required
                  />
                </div>
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