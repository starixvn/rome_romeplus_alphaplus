import React, { useState, useEffect, useRef } from 'react';
import { useTranslation } from 'react-i18next';
import { Bank, BankAccount } from '../../services/interfaces/Payments';
import { paymentProviderService } from '../../services/paymentsAPI';
import { ENABLED_STATUS_OPTIONS } from '../../constants/paymentConstants';
import SetTransactionLimitModal from './SetTransactionLimitModal';
import AddBankAccountComponent from './AddBankAccountComponent';
import UpdateBankAccountComponent from './UpdateBankAccountComponent';
import TitleComponent from '../../components/TitleComponent';
import { IconDelete,  IconPen, IconSearch, IconSync } from '../../components/Icons';
import CustomDropdown from '../../components/DropdownComponent';
import Loading from '../../components/LoadingComponent';
import { ResultMessageComponent } from '../../components/MessageComponents';

import { confirmAlert } from 'react-confirm-alert';
import 'react-confirm-alert/src/react-confirm-alert.css';
import { ButtonComponent } from '../../components/ButtonComponents';

interface BankAccountComponentProps {
  setActiveTab: React.Dispatch<React.SetStateAction<string>>;
  activeTab: string;
}

const BankAccountComponent: React.FC<BankAccountComponentProps> = ({ setActiveTab, activeTab }) => {
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
  }, []);

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

  const handleDeleteBankAccount = async (account: BankAccount) => {
    confirmAlert({
      title: tManagePayments('deleteBankAccount'),
      message: tManagePayments('confirmDeleteBankAccount').replace('{{bankAccount}}', account.accountNumber || ''),
      buttons: [
        {
          label: tGlobal('confirm'),
          onClick: async () => {
            try {
              setDeleting(account.id);
              setError(null);
              setSuccessMessage(null);
              const result = await paymentProviderService.deleteBankAccount(account.id);
              if (result.isSuccessful) {
                setSuccessMessage(tManagePayments('bankAccountDeletedSuccessfully'));
                // Clear success message and refresh list again after 3 seconds
                setTimeout(() => {
                  setSuccessMessage(null);
                  fetchBankAccounts();
                }, 3000);
              } else {
                setError(result.message || tManagePayments('failedToDeleteBankAccount'));
              }
            } catch (err) {
              setError(tManagePayments('failedToDeleteBankAccount'));
              console.error('Error deleting bank account:', err);
            } finally {
              setDeleting(null);
            }
          }
        },
        {
          label: tGlobal('cancel'),
          onClick: () => {}
        }
      ]
    });
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
      const type = transactionTypes.length > 0 ? transactionTypes.find(t => t.id === typeId) : '';
      return type ? type.name : tManagePayments(`transactionType-${typeId}`);
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
    <>
      {showAddBankAccountModal ? (
        <AddBankAccountComponent
          onClose={handleCloseAddBankAccountModal}
          onSave={handleSaveAddBankAccount}
        />
      ) : showUpdateBankAccountModal && selectedBankAccount ? (
        <UpdateBankAccountComponent
          onClose={handleCloseUpdateBankAccountModal}
          onSave={handleSaveUpdateBankAccount}
          bankAccount={selectedBankAccount}
        />
      ) : (
        <>
         <div className="p-[12px] h-[calc(100%-250px)]">
          <TitleComponent title={tManagePayments('managePayments')} Tippy >
            <div>
              <div>
                <i className="bi bi-info-circle fs-4 ico-info inline-elm"></i> 
                {tManagePayments('managePayments')}
              </div>
              <div>• {tManagePayments('managePaymentsTip1')}</div>
              <div>• {tManagePayments('managePaymentsTip2')}</div>
            </div>
          </TitleComponent>
          
          <div className="card h-[calc(100%-40px)] flex flex-col">
            <div className="card-header">
              <ul className="flex">
                <li className="mr-[8px]">
                  <button
                      className={`font-bold text-[12px] tracking-[0.12px] leading-[36px] pr-[16px] pl-[16px] rounded-t-[4px] ${activeTab === 'paymentGateway' ? ' bg-[var(--button-background)] text-[#ffffff] cursor-pointer' : 'bg-[#dbe2ef] text-[var(--text-color-primary)] cursor-pointer'}`}
                      onClick={() => setActiveTab('paymentGateway')}
                    >
                    {tManagePayments('paymentGateway')}
                  </button>
                </li>
                <li>
                  <button
                      className={`font-bold text-[12px] tracking-[0.12px] leading-[36px] pr-[16px] pl-[16px] rounded-t-[4px] ${activeTab === 'bankAccount' ? ' bg-[var(--button-background)] text-[#ffffff] cursor-pointer' : 'bg-[#dbe2ef] text-[var(--text-color-primary)] cursor-pointer'}`}
                      onClick={() => setActiveTab('bankAccount')}
                    >
                    {tManagePayments('bankAccount')}
                  </button>
                </li>
              </ul>
            </div>
            <div className="card-body h-full">
              <div className=" h-full flex flex-col">
                <div className="bg-[#ffffff] px-[14px] py-[8px]">
                <div className='pt-[14px] bg-[#ffffff]'>                   
                  {error && (<ResultMessageComponent type="error" message={error} />)}
                  {successMessage && (<ResultMessageComponent type="success" message={successMessage} />)}
                </div>
                  <div className="pb-[8px] mt-[8px] text-[16px] text-[var(--text-color-primary)] font-semibold leading-[21.6px] tracking-[0.2px] ">{tManagePayments('manageBankAccountAndLimits')}</div>
                  <div className="text-[12px] leading-[14px]">{tManagePayments('addBankAccountAndLimit')}</div>
                  <div className="my-[8px] flex gap-[8px]">                    
                    <ButtonComponent className='w-[183px]' type="apply" style="medium" text={`+${tManagePayments('addBankAccount')}`} onClick={handleAddBankAccount} />                    
                    <ButtonComponent className='shadow-[0_1px_5px_#0003,0_2px_2px_#00000024,0_3px_1px_-2px_#0000001f] w-[183px]' type="cancel" style="medium" text={tManagePayments('setLimit')} onClick={handleSetLimit} />
                  </div>
                </div>
                {loading && (<Loading />)}              
                {!loading && !error && banks.length > 0 && (                 
                  <>                    
                    <div className="flex px-[14px] bg-[#ffffff]">
                      <div className="w-[230px] pr-[14px]">
                        <div className="form-group mb-[8px]">
                          <label htmlFor="bankSelect" className="block text-[12px] font-semibold text-[#333333] my-[8px]">{tManagePayments('bank')}</label>                           
                          
                          <CustomDropdown                           
                            options={(() => {                              
                              return banks.map(bank => ({
                                value: bank.id,
                                label: bank.id === 0 ? tGlobal('all') : bank.name,
                                code: bank.code.toLocaleUpperCase()
                              }));
                            })()}
                            onSelect={(option) => handleBankChange({ target: { value: option.value.toString() } } as React.ChangeEvent<HTMLSelectElement>)}
                          />
                        </div>
                      </div>

                      <div className="w-[230px] pr-[14px]">
                        <div className="form-group mb-[8px]">
                          <label htmlFor="transactionTypeSelect" className="block text-[12px] font-semibold text-[#333333] my-[8px]">{tManagePayments('transactionType')}</label>
                          <select
                            id="transactionTypeSelect"
                            className="h-[32px] max-[768px]:h-[40px] w-[100%] border-[1px] border-[var(--border-control)] rounded-[4px] px-[8px] text-[12px] focus:outline-none focus:border-[var(--button-border)] cursor-pointer"
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

                      <div className="w-[230px] pr-[14px]">
                        <div className="form-group mb-[8px]">
                          <label htmlFor="statusSelect" className="block text-[12px] font-semibold text-[#333333] my-[8px]">{tManagePayments('status')}</label>
                          <select
                            id="statusSelect"
                            className="h-[32px] max-[768px]:h-[40px] w-[100%] border-[1px] border-[var(--border-control)] rounded-[4px] px-[8px] text-[12px] focus:outline-none focus:border-[var(--button-border)] cursor-pointer"
                            value={selectedStatus}
                            onChange={handleStatusChange}
                          >
                            {ENABLED_STATUS_OPTIONS.map((status) => (
                              <option key={status.id} value={status.id}>
                                {tManagePayments(`enable-status-${status.id}`)}
                              </option>
                            ))}
                          </select>
                        </div>
                      </div>
                    </div>
                    <div className="flex px-[14px] pb-[16px] bg-[#ffffff]">
                      <div className="w-[250px] ml-auto">
                        <div className="relative">
                          <span className='absolute top-[10px] left-[5px] z-[10]'>
                          <IconSearch />
                          </span>
                          <input
                            type="text"
                            id="searchInput"
                            className="h-[32px] max-[768px]:h-[40px] w-[100%] pl-[30px] border-[1px] border-[#0000003D] rounded-[4px] px-[8px] text-[12px] focus:outline-none focus:border-[var(--button-border)] shadow-[0_0_6px_rgba(0,0,0,0.16)]"
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
                  <div className="flex-1 h-full">
                  <div className="table-fixed-header table-fixed-body bg-[#ffffff] h-full">
                    <table className="text-[12px] max-[600px]:text-[17px] w-full whitespace-nowrap border-collapse border border-[#0000001f] border-t-[0px]  border-l-[0px] text-[#333333] shadow-[0_1px_5px_#0003,0_2px_2px_#00000024,0_3px_1px_-2px_#0000001f]">
                      <thead>
                        <tr>
                          <th className="col-no bg-[#ffffff]">#</th>
                          <th className="col-action bg-[#ffffff]">{tGlobal('action')}</th>
                          <th>{tManagePayments('accountNickname')}</th>
                          <th>{tManagePayments('bank')}</th>
                          <th>{tManagePayments('accountHolderName')}</th>
                          <th>{tManagePayments('bankAccountNumber')}</th>
                          <th>{tManagePayments('transactionType')}</th>
                          <th>{tManagePayments('status')}</th>
                          <th>{tManagePayments('lastUpdated')}</th>
                        </tr>
                      </thead>
                      <tbody className='font-[500]'>
                        {bankAccounts.map((account, index) => (
                          <tr key={account.id}>
                            <td className="col-no bg-[#ffffff]">{index + 1}</td> 
                            <td className="col-action bg-[#ffffff]">
                              <div className="flex">
                                <button 
                                  className="cursor-pointer p-[4px] rounded-[4px] bg-[#ffffff] hover:bg-[#EAEDF2] text-[var(--text-color-primary)]"
                                  onClick={() => handleEditBankAccount(account.id)}
                                  title={tGlobal('edit')}
                                >                            
                                  <IconPen />
                                </button>
                                <button 
                                  className="cursor-pointer p-[4px] rounded-[4px] bg-[#ffffff] hover:bg-[#EAEDF2] text-[var(--text-color-primary)]"
                                  onClick={() => handleDeleteBankAccount(account)}
                                  title={tGlobal('delete')}
                                  disabled={deleting === account.id}
                                >
                                  {deleting === account.id ? (
                                      <IconSync className='w-[16px] h-[16px] fill-[var(--primary-color)] animate-spin duration-2000 ease-linear animate-infinite' />
                                  ) : (
                                      <IconDelete />
                                  )}
                                </button>
                              </div>
                            </td>
                            <td className='text-left'>{account.accountNickname}</td>
                            <td className='text-left'>
                              <div className='flex items-center relative pl-[25px]'>
                                <img src={`../assets/bank/${account.code}.png`} alt={account.bankName} className='w-[20px] h-[20px] absolute top-[0] left-[0]' />
                                {account.bankName}
                              </div>
                              </td>
                            <td className='text-left'>{account.accountHolderName}</td>
                            <td>{account.accountNumber}</td>
                            <td>{getTransactionTypeNames(account.transactionTypes)}</td>
                            <td>
                              <span className={`rounded-[20px] px-[10px] py-[3px] ${account.status === 1 ? 'bg-[#D4E7D9]' : 'bg-[#FBD8D8]'}`}>
                                {account.status === 1 ? tGlobal('enabled') : tGlobal('disabled')}
                              </span>
                            </td>
                            <td>{account.lastUpdatedStr}</td>
                          </tr>
                        ))} 
                        
                      </tbody>
                    </table>
                  </div>
                  </div>
                )}

                {!loading && !error && banks.length > 0 && bankAccounts.length === 0 && (
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
                </div>
              </div>
            </div>
          </div>
        </>
      )}
    </>
  );
};

export default BankAccountComponent; 