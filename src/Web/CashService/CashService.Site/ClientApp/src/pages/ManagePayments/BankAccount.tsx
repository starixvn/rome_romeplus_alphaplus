import React, { useState, useEffect, useRef } from 'react';
import { useTranslation } from 'react-i18next';
import { Bank } from '../../services/interfaces/Bank';
import type { BankAccount } from '../../services/interfaces/BankAccount';
import { paymentProviderService } from '../../services/paymentsAPI';
import { TRANSACTION_TYPES, ENABLED_STATUS_OPTIONS } from '../../constants/paymentConstants';
import SetTransactionLimitModal from './SetTransactionLimitModal';
import AddBankAccountComponent from './AddBankAccountComponent';
import UpdateBankAccountComponent from './UpdateBankAccountComponent';
import styles from './PaymentGateway.module.scss';

const BankAccount: React.FC = () => {
  const { t: tManagePayments } = useTranslation('managePayments');
  const { t: tGlobal } = useTranslation('global');
  const [banks, setBanks] = useState<Bank[]>([]);
  const [bankAccounts, setBankAccounts] = useState<BankAccount[]>([]);
  const [selectedBank, setSelectedBank] = useState<string>('0');
  const [selectedTransactionType, setSelectedTransactionType] = useState<string>('0');
  const [selectedStatus, setSelectedStatus] = useState<string>('0');
  const [searchInput, setSearchInput] = useState<string>('');
  const [searchText, setSearchText] = useState<string>('');
  const [error, setError] = useState<string | null>(null);
  const [successMessage, setSuccessMessage] = useState<string | null>(null);
  const [loading, setLoading] = useState(true);
  const [deleting, setDeleting] = useState<number | null>(null);
  const [showSetLimitModal, setShowSetLimitModal] = useState(false);
  const [showAddBankAccountModal, setShowAddBankAccountModal] = useState(false);
  const [showUpdateBankAccountModal, setShowUpdateBankAccountModal] = useState(false);
  const [selectedBankAccount, setSelectedBankAccount] = useState<BankAccount | null>(null);
  const [transactionTypes, setTransactionTypes] = useState<any[]>([]);
  const searchInputRef = useRef<HTMLInputElement>(null);

  const fetchTransactionTypes = async () => {
    try {
      setLoading(true);
      const fetchedTransactionTypes = await paymentProviderService.getTransactionTypes();
      setTransactionTypes(fetchedTransactionTypes);
      setError(null);
    } catch (err) {
      setError('Failed to load transaction types. Please try again later.');
      console.error('Error fetching transaction types:', err);
    } finally {
      setLoading(false);
    }
  };

  const fetchBankAccounts = async () => {
    try {
      const fetchedBankAccounts = await paymentProviderService.getBankAccounts(
        parseInt(selectedBank) ,
        parseInt(selectedTransactionType),
        parseInt(selectedStatus),
        searchText
      );
      setBankAccounts(fetchedBankAccounts);
      setError(null);
      
      // Set focus on the search input after loading
      if (searchInputRef.current) {
        searchInputRef.current.focus();
      }
    } catch (err) {
      setError('Failed to load bank accounts. Please try again later.');
      console.error('Error fetching bank accounts:', err);
    }
  };

  useEffect(() => {
    const fetchBanks = async () => {
      try {
        setLoading(true);
        const fetchedBanks = await paymentProviderService.getBanksByCust();
        setBanks(fetchedBanks);
        setError(null);

        // Call fetchBankAccounts without setting loading state
        fetchBankAccounts();
      } catch (err) {
        setError('Failed to load banks. Please try again later.');
        console.error('Error fetching banks:', err);
      } finally {
        setLoading(false);
      }
    };

    fetchTransactionTypes();
    fetchBanks();
  }, [selectedBank]);

  useEffect(() => {
    if (banks.length > 0) {
      fetchBankAccounts();
    }
  }, [selectedBank, selectedTransactionType, selectedStatus, searchText]);

  const handleAddBankAccount = () => {
    setShowAddBankAccountModal(true);
  };

  const handleSetLimit = () => {
    setShowSetLimitModal(true);
  };

  const handleCloseSetLimitModal = () => {
    setShowSetLimitModal(false);
  };

  const handleSaveSetLimit = () => {
    setShowSetLimitModal(false);
  };

  const handleEditBankAccount = (accountId: number) => {
    const account = bankAccounts.find(acc => acc.id === accountId);
    if (account) {
      setSelectedBankAccount(account);
      setShowUpdateBankAccountModal(true);
    }
  };

  const handleCloseUpdateBankAccountModal = () => {
    setShowUpdateBankAccountModal(false);
    setSelectedBankAccount(null);
  };

  const handleSaveUpdateBankAccount = () => {
    setShowUpdateBankAccountModal(false);
    setSelectedBankAccount(null);
    fetchBankAccounts();
  };

  const handleDeleteBankAccount = async (accountId: number) => {
    try {
      setDeleting(accountId);
      setError(null);
      setSuccessMessage(null);
      const result = await paymentProviderService.deleteBankAccount(accountId);
      if (result.isSuccessful) {
        setSuccessMessage(tManagePayments('bankAccountDeletedSuccessfully'));

        // Add a 3-second delay before refreshing the list
        await new Promise(resolve => setTimeout(resolve, 3000));
        // Refresh the bank accounts list after successful deletion
        await fetchBankAccounts();
      } else {
        setError(result.message || tManagePayments('failedToDeleteBankAccount'));
      }
    } catch (err) {
      setError(tManagePayments('failedToDeleteBankAccount'));
      console.error('Error deleting bank account:', err);
    } finally {
      setDeleting(null);
    }
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

  const handleSearchChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    setSearchInput(event.target.value);
    setSearchText(event.target.value);
  };

  const getTransactionTypeNames = (types: string) => {
    if (!types) return '';
    
    // Split the comma-separated type IDs
    const typeIds = types.split(',').map(id => parseInt(id.trim()));
    
    // Map each type ID to its name
    const typeNames = typeIds.map(typeId => {
      const type = transactionTypes.length > 0 
        ? transactionTypes.find(t => t.id === typeId)
        : TRANSACTION_TYPES.find(t => t.id === typeId);
      return type ? type.name : typeId.toString();
    });
    
    return typeNames.join(', ');
  };

  const handleCloseAddBankAccountModal = () => {
    setShowAddBankAccountModal(false);
  };

  const handleSaveAddBankAccount = () => {
    setShowAddBankAccountModal(false);
    fetchBankAccounts();
  };

  return (
    <div className="bg-[#ffffff] max-w-[1000px]">
      {showAddBankAccountModal ? (
        <AddBankAccountComponent
          onClose={handleCloseAddBankAccountModal}
          onSave={handleSaveAddBankAccount}
          banks={banks}
        />
      ) : showUpdateBankAccountModal && selectedBankAccount ? (
        <UpdateBankAccountComponent
          onClose={handleCloseUpdateBankAccountModal}
          onSave={handleSaveUpdateBankAccount}
          banks={banks}
          bankAccount={selectedBankAccount}
        />
      ) : (
        <>
          <div className="px-[14px] py-[8px]">
            <div className="pb-[8px] mt-[8px] text-[16px] text-[#163f78] font-semibold leading-[21.6px] tracking-[0.2px] ">{tManagePayments('manageBankAccountAndLimits')}</div>
            <div className="text-[12px] leading-[14px]">{tManagePayments('addBankAccountAndLimit')}</div>
            <div className="my-[8px]">
              <button 
                className="w-[183px] h-[30px] rounded-[5px] text-[#ffffff] px-[16px] py-[4px] text-[12px] font-bold mx-[4px] bg-[#163f78] hover:bg-[#375A8A] cursor-pointer  shadow-[0_1px_5px_#0003,0_2px_2px_#00000024,0_3px_1px_-2px_#0000001f]"
                onClick={handleAddBankAccount}
              >
              +{tManagePayments('addBankAccount')}
              </button>
              <button 
                className="w-[183px] h-[30px] rounded-[5px] text-[#163f78] px-[16px] py-[4px] text-[12px] font-bold border-[1px] border-[#163f78] mx-[4px] bg-[#ffffff] hover:bg-[#EAEDF2] cursor-pointer shadow-[0_1px_5px_#0003,0_2px_2px_#00000024,0_3px_1px_-2px_#0000001f]"
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

          {successMessage && (
            <div className="alert alert-success" role="alert">
              {successMessage}
            </div>
          )}

          {!loading && !error && banks.length === 0 && (
            <div className="alert alert-info" role="alert">
              {tGlobal('noData')}
            </div>
          )}

          {!loading && !error && banks.length > 0 && (
            <>
              <div className="row">
                <div className="col-md-4">
                  <div className="form-group mb-3">
                    <label htmlFor="bankSelect" className="form-label">{tManagePayments('bank')}</label>
                    <select
                      id="bankSelect"
                      className="form-select"
                      value={selectedBank}
                      onChange={handleBankChange}
                    >
                      <option key="0" value="0">{tGlobal('all')}</option>
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
                      <option key="0" value="0">{tGlobal('all')}</option>
                      {transactionTypes.length && (
                        transactionTypes.map((type) => (
                          <option key={type.id} value={type.id}>
                            {tManagePayments(`transactionType-${type.id}`)}
                          </option>
                        ))
                      )}
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
                      {ENABLED_STATUS_OPTIONS.map((status) => (
                        <option key={status.id} value={status.id}>
                          {tManagePayments(`status-${status.id}`)}
                        </option>
                      ))}
                    </select>
                  </div>
                </div>
              </div>
              <div className="row mb-3">
                <div className="col-md-12">
                  <div className="form-group">
                    <input
                      type="text"
                      id="searchInput"
                      className="form-control"
                      placeholder={tManagePayments('searchAccountNumberOrName')}
                      value={searchInput}
                      onChange={handleSearchChange}
                      ref={searchInputRef}
                    />
                  </div>
                </div>
              </div>
            </>
          )}

          {!loading && !error && bankAccounts.length > 0 && (
            <div className="table-responsive mt-4">
              <table className="table table-striped table-hover">
                <thead>
                  <tr>
                    <th>{tGlobal('action')}</th>
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
                            disabled={deleting === account.id}
                          >
                            {deleting === account.id ? (
                              <>
                                {tGlobal('deleting')}
                                <span className="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span>
                              </>
                            ) : (
                              <>
                                {tGlobal('delete')}
                                <i className="bi bi-trash"></i>
                              </>
                            )}
                          </button>
                        </div>
                      </td>
                      <td>{account.accountNickname}</td>
                      <td>{account.bankName}</td>
                      <td>{account.accountHolderName}</td>
                      <td>{account.accountNumber}</td>
                      <td>{getTransactionTypeNames(account.transactionTypes)}</td>
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

          {showSetLimitModal && (
            <SetTransactionLimitModal
              onClose={handleCloseSetLimitModal}
              onSave={handleSaveSetLimit}
            />
          )}
        </>
      )}
    </div>
  );
};

export default BankAccount; 