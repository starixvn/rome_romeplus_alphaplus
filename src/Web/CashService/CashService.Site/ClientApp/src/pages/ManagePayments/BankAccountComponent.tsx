import 'react-confirm-alert/src/react-confirm-alert.css';

import React, { useEffect, useRef, useState } from 'react';
import { confirmAlert } from 'react-confirm-alert';
import { useTranslation } from 'react-i18next';

import BankIcon from '../../components/BankComponents';
import {
  ButtonFullComponent,
  ButtonOutlineComponent,
  ButtonTabActiveComponent,
  ButtonTabInactiveComponent,
} from '../../components/ButtonComponents';
import CustomDropdown from '../../components/CustomDropdownComponent';
import { IconDelete, IconInfo, IconPen, IconSearch, IconSync } from '../../components/Icons';
import Loading from '../../components/LoadingComponent';
import { ErrorMessageComponent, SuccessMessageComponent } from '../../components/MessageComponents';
import { StatusDisabledComponent, StatusEnabledComponent } from '../../components/StatusComponents';
import TitleComponent, {
  LabelTitleComponent,
  MediumTitleComponent,
} from '../../components/TitleComponents';
import { BANK_STATUS_OPTIONS } from '../../constants/paymentConstants';
import { Bank, BankAccount } from '../../services/interfaces/Payments';
import { paymentProviderService } from '../../services/paymentsAPI';
import AddBankAccountComponent from './AddBankAccountComponent';
import SetTransactionLimitModal from './SetTransactionLimitModal';
import UpdateBankAccountComponent from './UpdateBankAccountComponent';

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
  const [selectedStatus, setSelectedStatus] = useState<string>('2');
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
    setLoading(true);
    const response = await paymentProviderService.getTransactionTypes();
    if (response.error) {
      setError(response.error.message);
      setLoading(false);
      return;
    }

    const fetchedTransactionTypes = response.data || [];
    setTransactionTypes(fetchedTransactionTypes);
    setError(null);
    setLoading(false);
  };

  const fetchBankAccounts = async () => {
    const response = await paymentProviderService.getBankAccounts(
      parseInt(selectedBank),
      parseInt(selectedTransactionType),
      parseInt(selectedStatus),
      searchText
    );

    if (response.error) {
      setError(response.error.message);
      setLoading(false);
      return;
    }

    const fetchedBankAccounts = response.data || [];
    setBankAccounts(fetchedBankAccounts);
    setError(null);

    if (searchInputRef.current) {
      searchInputRef.current.focus();
    }
  };

  const fetchBanks = async () => {
    setLoading(true);
    const response = await paymentProviderService.getBanksByCust();

    if (response.error) {
      setError(response.error.message);
      setLoading(false);
      return;
    }

    const fetchedBanks = response.data || [];
    fetchedBanks.unshift({
      id: 0,
      name: tGlobal('all'),
      code: '',
      transactionTypes: '',
      depositMatchingMode: {
        sourceType: '',
        mappingTypes: [],
        sourceTypeName: '',
      },
    });

    setBanks(fetchedBanks);
    setSelectedBank('0');
    setError(null);

    // Call fetchBankAccounts without setting loading state
    fetchBankAccounts();
    setLoading(false);
  };

  useEffect(() => {
    fetchTransactionTypes();
    fetchBanks();
  }, []);

  useEffect(() => {
    if (banks.length > 0) {
      fetchBankAccounts();
    }
  }, [selectedBank, selectedTransactionType, selectedStatus, searchText]);

  const onAddBankAccount = () => {
    setShowAddBankAccountModal(true);
  };
  const onSetLimit = () => {
    setShowSetLimitModal(true);
  };
  const onCloseSetLimitModal = () => {
    setShowSetLimitModal(false);
  };
  const onSaveSetLimit = () => {
    setShowSetLimitModal(false);
  };

  const onEditBankAccount = (accountId: number) => {
    const account = bankAccounts.find((acc) => acc.id === accountId);
    if (account) {
      setSelectedBankAccount(account);
      setShowUpdateBankAccountModal(true);
    }
  };

  const onCloseUpdateBankAccountModal = () => {
    setShowUpdateBankAccountModal(false);
    setSelectedBankAccount(null);
    fetchBankAccounts();
  };

  const onSaveUpdateBankAccount = () => {
    setShowUpdateBankAccountModal(false);
    setSelectedBankAccount(null);
    fetchBanks();
    fetchBankAccounts();
  };

  const onDeleteBankAccount = async (account: BankAccount) => {
    confirmAlert({
      title: tManagePayments('deleteBankAccount'),
      message: tManagePayments('confirmDeleteBankAccount').replace(
        '{{bankAccount}}',
        account.accountNumber || ''
      ),
      buttons: [
        {
          label: tGlobal('confirm'),
          onClick: async () => {
            setDeleting(account.id);
            setError(null);
            setSuccessMessage(null);
            const response = await paymentProviderService.deleteBankAccount(account.id);

            if (response.error) {
              setError(response.error.message);
              setDeleting(null);
              return;
            }

            const result = response.data ?? {
              isSuccessful: false,
              code: 1,
              message: tGlobal('errorOccurred'),
            };
            if (result.isSuccessful) {
              setSuccessMessage(tManagePayments('bankAccountDeletedSuccessfully'));
              // Clear success message and refresh list again after 3 seconds
              setTimeout(() => {
                setSuccessMessage(null);
                fetchBankAccounts();
              }, 3000);
            } else {
              setError(result.message ?? tManagePayments('failedToDeleteBankAccount'));
            }

            setDeleting(null);
          },
        },
        {
          label: tGlobal('cancel'),
          onClick: () => {},
        },
      ],
    });
  };

  const onBankChange = (event: React.ChangeEvent<HTMLSelectElement>) => {
    setSelectedBank(event.target.value);
  };

  const onTransactionTypeChange = (event: React.ChangeEvent<HTMLSelectElement>) => {
    setSelectedTransactionType(event.target.value);
  };

  const onStatusChange = (event: React.ChangeEvent<HTMLSelectElement>) => {
    setSelectedStatus(event.target.value);
  };

  const onSearchChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    setSearchInput(event.target.value);
    setSearchText(event.target.value);
  };

  const getTransactionTypeNames = (types: string) => {
    if (!types) return '';

    // Split the comma-separated type IDs
    const typeIds = types.split(',').map((id) => parseInt(id.trim()));

    // Map each type ID to its name
    const typeNames = typeIds.map((typeId) => {
      return tManagePayments(`transactionType-${typeId}`);
    });

    return typeNames.join(', ');
  };

  const onCloseAddBankAccountModal = () => {
    setShowAddBankAccountModal(false);
  };

  const onSaveAddBankAccount = () => {
    setShowAddBankAccountModal(false);
    fetchBanks();
    fetchBankAccounts();
  };

  return (
    <>
      {showAddBankAccountModal ? (
        <AddBankAccountComponent
          onClose={onCloseAddBankAccountModal}
          onSave={onSaveAddBankAccount}
        />
      ) : showUpdateBankAccountModal && selectedBankAccount ? (
        <UpdateBankAccountComponent
          onClose={onCloseUpdateBankAccountModal}
          onSave={onSaveUpdateBankAccount}
          bankAccount={selectedBankAccount}
        />
      ) : (
        <>
          <div className="h-full flex flex-col">
            <TitleComponent title={tManagePayments('managePayments')} showTippy={true}>
              <div>
                <div>
                  <IconInfo className="w-[18px] h-[18px] fill-[#ffffff] inline-block vertical-middle mr-[4px] relative top-[-2px]" />
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
                  <ButtonTabActiveComponent
                text={tManagePayments('paymentGateway')}
                onClick={() => setActiveTab('paymentGateway')}
              />
                  </li>
                  <li>
                  {activeTab === 'bankAccount' && (
                <ButtonTabInactiveComponent text={tManagePayments('bankAccount')} />
              )}
                  </li>
                </ul>
              </div>
              <div className="card-body h-full">
                <div className=" h-full flex flex-col">
                  <div className="bg-[#ffffff] px-[14px] py-[8px]">
                    <ErrorMessageComponent message={error} />
                    <SuccessMessageComponent message={successMessage} />
                    <MediumTitleComponent
                      className="my-[8px]"
                      text={tManagePayments('manageBankAccountAndLimits')}
                    />
                    <div className="">{tManagePayments('addBankAccountAndLimit')}</div>
                    <div className="my-[8px] flex gap-[8px]">
                      <ButtonFullComponent
                        className="w-[183px] [&>*]:w-[100%]"
                        text={`+${tManagePayments('addBankAccount')}`}
                        onClick={onAddBankAccount}
                      />
                      <ButtonOutlineComponent
                        className="w-[183px] [&>*]:w-[100%]"
                        text={tManagePayments('setLimit')}
                        onClick={onSetLimit}
                      />
                    </div>
                  </div>
                  {loading && <Loading />}
                  {!loading && !error && banks.length > 0 && (
                    <>
                      <div className="flex px-[14px] bg-[#ffffff] flex-nowrap gap-[16px] max-[768px]:justify-between max-[768px]:gap-[0] max-[768px]:flex-wrap w-full">
                        <div className="w-[230px] pt-[4px] max-[768px]:w-[48%] mb-[8px]">
                          <LabelTitleComponent text={tManagePayments('bank')} />
                          <CustomDropdown
                            options={(() => {
                              return banks.map((bank) => ({
                                value: bank.id,
                                label: bank.id === 0 ? tGlobal('all') : bank.name,
                                icon: <BankIcon code={bank.code} />,
                              }));
                            })()}
                            initialValue={selectedBank}
                            onSelect={(option) =>
                              onBankChange({
                                target: { value: option.value.toString() },
                              } as React.ChangeEvent<HTMLSelectElement>)
                            }
                          />
                        </div>

                        <div className="w-[230px] pt-[4px] max-[768px]:w-[48%] mb-[8px]">
                          <LabelTitleComponent text={tManagePayments('transactionType')} />
                          <select
                            id="transactionTypeSelect"
                            className="h-[32px] max-[768px]:h-[40px] w-[100%] border-[1px] border-[var(--border-control)] rounded-[4px] px-[8px] focus:outline-none focus:border-[var(--button-border)] cursor-pointer"
                            value={selectedTransactionType}
                            onChange={onTransactionTypeChange}
                          >
                            <option key="0" value="0">
                              {tGlobal('all')}
                            </option>
                            {transactionTypes.length &&
                              transactionTypes.map((type) => (
                                <option key={type.id} value={type.id}>
                                  {tManagePayments(`transactionType-${type.id}`)}
                                </option>
                              ))}
                          </select>
                        </div>

                        <div className="w-[230px] pt-[4px] max-[768px]:w-[48%] mb-[8px] ">
                          <LabelTitleComponent text={tManagePayments('status')} />
                          <select
                            id="statusSelect"
                            className="h-[32px] max-[768px]:h-[40px] w-[100%] border-[1px] border-[var(--border-control)] rounded-[4px] px-[8px] focus:outline-none focus:border-[var(--button-border)] cursor-pointer"
                            value={selectedStatus}
                            onChange={onStatusChange}
                          >
                            {BANK_STATUS_OPTIONS.map((status) => (
                              <option key={status.id} value={status.id}>
                                {tManagePayments(`bank-status-${status.id}`)}
                              </option>
                            ))}
                          </select>
                        </div>
                      </div>
                      <div className="flex px-[14px] pb-[16px] w-full bg-[#ffffff]">
                        <div className="w-[250px] ml-auto ">
                          <div className="relative">
                            <span className="absolute top-[8px] left-[8px] z-[10] max-[768px]:top-[12px]">
                              <IconSearch />
                            </span>
                            <input
                              type="text"
                              id="searchInput"
                              className="h-[32px] max-[768px]:h-[40px] w-[100%] pl-[30px] border-[1px] border-[#0000003D] rounded-[4px] px-[8px] focus:outline-none focus:border-[var(--button-border)] shadow-[0_0_6px_rgba(0,0,0,0.16)]"
                              placeholder={tManagePayments('searchAccountNumberOrName')}
                              value={searchInput}
                              onChange={onSearchChange}
                              ref={searchInputRef}
                            />
                          </div>
                        </div>
                      </div>
                    </>
                  )}

                  {!loading && !error && bankAccounts.length > 0 && (
                    
                      <div className="table-fixed-header table-fixed-body flex-1">
                        <table className="w-full whitespace-nowrap border-collapse  ">
                          <thead>
                            <tr>
                              <th className="col-no z-10 bg-[var(--table-header-background)]">#</th>
                              <th className="col-action z-10 bg-[var(--table-header-background)]">{tGlobal('action')}</th>
                              <th>{tManagePayments('accountNickname')}</th>
                              <th>{tManagePayments('bank')}</th>
                              <th>{tManagePayments('accountHolderName')}</th>
                              <th>{tManagePayments('bankAccountNumber')}</th>
                              <th>{tManagePayments('transactionType')}</th>
                              <th>{tManagePayments('status')}</th>
                              <th>{tManagePayments('lastUpdated')}</th>
                            </tr>
                          </thead>
                          <tbody className="font-[500]">
                          {bankAccounts.map((account, index) => (
                              <tr key={account.id}>
                                <td className="col-no bg-[#ffffff]">{index + 1}</td>
                                <td className="col-action bg-[#ffffff]">
                                  <div className="flex justify-center">
                                    <button
                                      className="cursor-pointer p-[4px] rounded-[4px] bg-[#ffffff] hover:bg-[#EAEDF2] text-[var(--text-color-primary)]"
                                      onClick={() => onEditBankAccount(account.id)}
                                      title={tGlobal('edit')}
                                    >
                                      <IconPen />
                                    </button>
                                    <button
                                      className="cursor-pointer p-[4px] rounded-[4px] bg-[#ffffff] hover:bg-[#EAEDF2] text-[var(--text-color-primary)]"
                                      onClick={() => onDeleteBankAccount(account)}
                                      title={tGlobal('delete')}
                                      disabled={deleting === account.id}
                                    >
                                      {deleting === account.id ? (
                                        <IconSync className="w-[16px] h-[16px] fill-[var(--primary-color)] animate-spin duration-2000 ease-linear animate-infinite" />
                                      ) : (
                                        <IconDelete />
                                      )}
                                    </button>
                                  </div>
                                </td>
                                <td className="text-left">{account.accountNickname}</td>
                                <td className="text-left">
                                  <div className="flex items-center relative pl-[25px]">
                                    <span className="absolute top-[-2px] left-[0]">
                                      <BankIcon code={account.code} />
                                    </span>
                                    {account.bankName}
                                  </div>
                                </td>
                                <td className="text-left">{account.accountHolderName}</td>
                                <td>{account.accountNumber}</td>
                                <td>{getTransactionTypeNames(account.transactionTypes)}</td>
                                <td>
                                  {account.status === 1 ? (
                                    <StatusEnabledComponent />
                                  ) : (
                                    <StatusDisabledComponent />
                                  )}
                                </td>
                                <td>{account.lastUpdatedStr}</td>
                              </tr>
                            ))}
                            
                          </tbody>
                        </table>
                      </div>
                  
                  )}

                  {!loading && !error && banks.length > 0 && bankAccounts.length === 0 && (
                    <div className="alert alert-info mt-4" role="alert">
                      {tGlobal('noData')}
                    </div>
                  )}

                  {showSetLimitModal && (
                    <SetTransactionLimitModal
                      onClose={onCloseSetLimitModal}
                      onSave={onSaveSetLimit}
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
