import React, { useState, useEffect } from 'react';
import { useTranslation } from 'react-i18next';
import { Bank } from '../../services/interfaces/Bank';
import type { BankAccount } from '../../services/interfaces/BankAccount';
import { paymentProviderService } from '../../services/paymentsAPI';
import { TRANSACTION_TYPES, STATUS_OPTIONS } from '../../constants/paymentConstants';

const BankAccount: React.FC = () => {
  const { t: tManagePayments } = useTranslation('managePayments');
  const { t: tGlobal } = useTranslation('global');
  const [banks, setBanks] = useState<Bank[]>([]);
  const [bankAccounts, setBankAccounts] = useState<BankAccount[]>([]);
  const [selectedBank, setSelectedBank] = useState<string>('');
  const [selectedTransactionType, setSelectedTransactionType] = useState<string>('');
  const [selectedStatus, setSelectedStatus] = useState<string>('');
  const [error, setError] = useState<string | null>(null);
  const [loading, setLoading] = useState(true);

  const fetchBankAccounts = async () => {
    try {
      setLoading(true);
      const fetchedBankAccounts = await paymentProviderService.getBankAccounts(
        1, // TODO: Replace with actual customer ID
        parseInt(selectedTransactionType),
        selectedBank ? parseInt(selectedBank) : undefined,
        undefined, // accountNumber
        undefined, // accountHolderName
        selectedStatus ? parseInt(selectedStatus) : undefined
      );
      setBankAccounts(fetchedBankAccounts);
      setError(null);
    } catch (err) {
      setError('Failed to load bank accounts. Please try again later.');
      console.error('Error fetching bank accounts:', err);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    const fetchBanks = async () => {
      try {
        setLoading(true);
        const fetchedBanks = await paymentProviderService.getAllBanks();
        setBanks(fetchedBanks);
        setError(null);
      } catch (err) {
        setError('Failed to load banks. Please try again later.');
        console.error('Error fetching banks:', err);
      } finally {
        setLoading(false);
      }
    };

    fetchBanks();
  }, []);

  useEffect(() => {
    if (banks.length > 0) {
      fetchBankAccounts();
    }
  }, [selectedBank, selectedTransactionType, selectedStatus]);

  const handleAddBankAccount = () => {
    // TODO: Implement add bank account functionality
    console.log('Add bank account clicked');
  };

  const handleSetLimit = () => {
    // TODO: Implement set limit functionality
    console.log('Set limit clicked');
  };

  const handleEditBankAccount = (accountId: number) => {
    // TODO: Implement edit bank account functionality
    console.log('Edit bank account clicked for account:', accountId);
  };

  const handleDeleteBankAccount = (accountId: number) => {
    // TODO: Implement delete bank account functionality
    console.log('Delete bank account clicked for account:', accountId);
  };

  const handleBankChange = (event: React.ChangeEvent<HTMLSelectElement>) => {
    setSelectedBank(event.target.value);
  };

  const handleTransactionTypeChange = (event: React.ChangeEvent<HTMLSelectElement>) => {
    setSelectedTransactionType(event.target.value);
  };

  const handleStatusChange = (event: React.ChangeEvent<HTMLSelectElement>) => {
    setSelectedStatus(event.target.value);
  };

  const getTransactionTypeName = (typeId: string) => {
    const type = TRANSACTION_TYPES.find(t => t.id.toString() === typeId);
    return type ? type.name : typeId;
  };

  return (
    <div className="bank-account-container">
      <div className="d-flex justify-content-between align-items-center mb-4">
        <h2 className="h4 mb-0">{tManagePayments('manageBankAccountAndLimits')}</h2>
        <label>{tManagePayments('addBankAccountAndLimit')}</label>
        <div className="d-flex gap-2">
          <button 
            className="btn btn-primary"
            onClick={handleAddBankAccount}
          >
            {tManagePayments('addBankAccount')}
          </button>
          <button 
            className="btn btn-secondary"
            onClick={handleSetLimit}
          >
            {tManagePayments('setLimit')}
          </button>
        </div>
      </div>

      {loading && (
        <div className="text-center">
          <div className="spinner-border" role="status">
            <span className="visually-hidden">{tGlobal('loading')}</span>
          </div>
        </div>
      )}

      {error && (
        <div className="alert alert-danger" role="alert">
          {error}
        </div>
      )}

      {!loading && !error && banks.length === 0 && (
        <div className="alert alert-info" role="alert">
          {tGlobal('noData')}
        </div>
      )}

      {!loading && !error && banks.length > 0 && (
        <div className="row">
          <div className="col-md-4">
            <div className="form-group mb-3">
              <label htmlFor="bankSelect" className="form-label">{tManagePayments('bankName')}</label>
              <select
                id="bankSelect"
                className="form-select"
                value={selectedBank}
                onChange={handleBankChange}
              >
                <option value="">{tGlobal('select')}</option>
                {banks.map((bank) => (
                  <option key={bank.id} value={bank.id}>
                    {bank.name}
                  </option>
                ))}
              </select>
            </div>
          </div>

          <div className="col-md-4">
            <div className="form-group mb-3">
              <label htmlFor="transactionTypeSelect" className="form-label">{tManagePayments('transactionType')}</label>
              <select
                id="transactionTypeSelect"
                className="form-select"
                value={selectedTransactionType}
                onChange={handleTransactionTypeChange}
              >
                <option value="">{tGlobal('select')}</option>
                {TRANSACTION_TYPES.map((type) => (
                  <option key={type.id} value={type.id}>
                    {type.name}
                  </option>
                ))}
              </select>
            </div>
          </div>

          <div className="col-md-4">
            <div className="form-group mb-3">
              <label htmlFor="statusSelect" className="form-label">{tManagePayments('status')}</label>
              <select
                id="statusSelect"
                className="form-select"
                value={selectedStatus}
                onChange={handleStatusChange}
              >
                <option value="">{tGlobal('select')}</option>
                {STATUS_OPTIONS.map((status) => (
                  <option key={status.id} value={status.id}>
                    {status.name}
                  </option>
                ))}
              </select>
            </div>
          </div>
        </div>
      )}

      {!loading && !error && bankAccounts.length > 0 && (
        <div className="table-responsive mt-4">
          <table className="table table-striped table-hover">
            <thead>
              <tr>
                <th>{tManagePayments('action')}</th>
                <th>{tManagePayments('accountNickname')}</th>
                <th>{tManagePayments('bank')}</th>
                <th>{tManagePayments('accountHolderName')}</th>
                <th>{tManagePayments('bankAccountNumber')}</th>
                <th>{tManagePayments('transactionType')}</th>
                <th>{tManagePayments('status')}</th>
                <th>{tManagePayments('lastUpdated')}</th>
              </tr>
            </thead>
            <tbody>
              {bankAccounts.map((account) => (
                <tr key={account.id}>
                  <td>
                    <div className="d-flex gap-2">
                      <button 
                        className="btn btn-sm btn-outline-primary"
                        onClick={() => handleEditBankAccount(account.id)}
                        title={tManagePayments('edit')}
                      >
                        {tGlobal('edit')}
                        <i className="bi bi-pencil"></i>
                      </button>
                      <button 
                        className="btn btn-sm btn-outline-danger"
                        onClick={() => handleDeleteBankAccount(account.id)}
                        title={tManagePayments('delete')}
                      >
                        {tGlobal('delete')}
                        <i className="bi bi-trash"></i>
                      </button>
                    </div>
                  </td>
                  <td>{account.accountNickname}</td>
                  <td>{account.bank.name}</td>
                  <td>{account.accountHolderName}</td>
                  <td>{account.accountNumber}</td>
                  <td>{getTransactionTypeName(account.transactionType)}</td>
                  <td>
                    <span className={`badge ${account.status === 1 ? 'bg-success' : 'bg-danger'}`}>
                      {account.status === 1 ? tManagePayments('active') : tManagePayments('inactive')}
                    </span>
                  </td>
                  <td>{account.lastUpdatedStr}</td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}

      {!loading && !error && bankAccounts.length === 0 && (
        <div className="alert alert-info mt-4" role="alert">
          {tGlobal('noData')}
        </div>
      )}
    </div>
  );
};

export default BankAccount; 