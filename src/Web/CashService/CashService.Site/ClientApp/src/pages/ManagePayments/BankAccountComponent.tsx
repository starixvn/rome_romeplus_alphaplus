import 'react-confirm-alert/src/react-confirm-alert.css';

import React, { useEffect, useRef, useState } from 'react';
import { confirmAlert } from 'react-confirm-alert';
import { useTranslation } from 'react-i18next';

import BankIcon from '../../components/BankComponents';
import {
  ButtonTabActiveComponent,
  ButtonTabInactiveComponent,
  ButtonApplyComponent,
  ButtonCancelComponent,
  ButtonComponent,
} from '../../components/Buttons/index';
import CustomDropdown from '../../components/CustomDropdownComponent';
import { IconArrowDown, IconSearch } from '../../components/Icons';
import Loading from '../../components/LoadingComponent';
import { MessageComponent } from '../../components/Message';
import { StatusComponent } from '../../components/Status';
import { TitleComponent } from '../../components/Title';
import { BANK_STATUS_OPTIONS } from '../../constants/paymentConstants';
import { Bank, BankAccount } from '../../services/interfaces/Payments';
import { paymentProviderService } from '../../services/paymentsAPI';
import AddBankAccountComponent from './AddBankAccountComponent';
import SetTransactionLimitModal from './SetTransactionLimitModal';
import UpdateBankAccountComponent from './UpdateBankAccountComponent';
import { createErrorResultModel } from '../../services/interfaces/ResultModel';
import NoRecordFound from '../../components/NoRecordFoundComponent';
import { ButtonIconComponent } from '../../components/Buttons/ButtonIconComponent';

interface BankAccountComponentProps {
  setActiveTab: React.Dispatch<React.SetStateAction<string>>;
  isCashServiceEnabled: boolean;
}

const BankAccountComponent: React.FC<BankAccountComponentProps> = ({
  setActiveTab,
  isCashServiceEnabled,
}) => {
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
  const [isSearchActive, setIsSearchActive] = useState(false);
  const searchRef = useRef<HTMLDivElement>(null);

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

  const handleSearchClick = () => {
    setIsSearchActive(!isSearchActive);
  };
  useEffect(() => {
    const handleClickOutside = (event: MouseEvent) => {
      if (searchRef.current && !searchRef.current.contains(event.target as Node)) {
        setIsSearchActive(false);
      }
    };

    document.addEventListener('mousedown', handleClickOutside);
    return () => {
      document.removeEventListener('mousedown', handleClickOutside);
    };
  }, []);

  const handleDeleteBankAccount = async (account: BankAccount) => {
    setDeleting(account.id);
    setError(null);
    setSuccessMessage(null);
    const response = await paymentProviderService.deleteBankAccount(account.id);

    if (response.error) {
      setError(response.error.message);
      setDeleting(null);
      return;
    }

    const result = response.data ?? createErrorResultModel(1, tGlobal('errorOccurred'));
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
  };

  const renderConfirmUI = (
    { onClose: closeConfirm }: { onClose: () => void },
    account: BankAccount,
    isCashServiceEnabled: boolean
  ) => (
    <div className="bg-[#00000080] fixed inset-0 flex items-start justify-center z-50">
      <div className="px-[20px] pb-[10px] bg-[var(--modal-background)] border-[1px] border-[var(--modal-border-color)] shadow-[var(--modal-box-shadow)] mt-[100px] max-w-[400px] w-full max-[768px]:max-w-[90%] rounded-[3px]">
        <div className="relative mb-[10px] border-b-[1px] border-[var(--border-color)] py-[11px]">
          <TitleComponent type="medium" text={tManagePayments('deleteBankAccount')} />
          <ButtonIconComponent style="Close" onClick={closeConfirm} />
        </div>
        <p className="mb-[20px]">
          {tManagePayments('confirmDeleteBankAccount').replace(
            '{{bankAccount}}',
            account.accountNumber || ''
          )}
        </p>
        <div className="flex justify-center gap-[8px]">
          <ButtonCancelComponent onClick={closeConfirm} text={tGlobal('cancel')} />
          <ButtonApplyComponent
            onClick={() => {
              closeConfirm();
              handleDeleteBankAccount(account);
            }}
            text={tGlobal('confirm')}
            disabled={!isCashServiceEnabled}
            title={isCashServiceEnabled ? '' : tGlobal('cashServiceDisabled')}
          />
        </div>
      </div>
    </div>
  );

  const onDeleteBankAccount = async (account: BankAccount) => {
    confirmAlert({
      customUI: ({ onClose }) => renderConfirmUI({ onClose }, account, isCashServiceEnabled),
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

  useEffect(() => {
    setTimeout(function () {
      top?.indexManager && top.indexManager.initFeedbackFunction(33);
    }, 1000);
  }, []);

  return (
    <>
      {showAddBankAccountModal ? (
        <AddBankAccountComponent
          onClose={onCloseAddBankAccountModal}
          onSave={onSaveAddBankAccount}
          isCashServiceEnabled={isCashServiceEnabled}
        />
      ) : showUpdateBankAccountModal && selectedBankAccount ? (
        <UpdateBankAccountComponent
          onClose={onCloseUpdateBankAccountModal}
          onSave={onSaveUpdateBankAccount}
          bankAccount={selectedBankAccount}
          isCashServiceEnabled={isCashServiceEnabled}
        />
      ) : (
        <>
          <div className="h-full flex flex-col">
            <TitleComponent type="main" text={tManagePayments('managePayments')} showTippy={true}>
              <div>
                <div>• {tManagePayments('managePaymentsTip1')}</div>
                <div>• {tManagePayments('managePaymentsTip2')}</div>
              </div>
            </TitleComponent>
            <ul className="flex mt-[-12px] border-b border-[var(--border-color)]">
              <li className="mr-[8px]">
                <ButtonTabInactiveComponent
                  text={tManagePayments('paymentGateway')}
                  onClick={() => setActiveTab('paymentGateway')}
                />
              </li>
              <li>
                <ButtonTabActiveComponent text={tManagePayments('bankAccount')} />
              </li>
            </ul>
            <div className="pt-[8px]">
              <MessageComponent message={error} type="error" className="mb-[10px]" />
              <MessageComponent message={successMessage} type="success" className="mb-[10px]" />
            </div>
            {loading && <Loading />}
            {!loading && banks && banks.length > 0 && (
              <>
                <div className="flex flex-wrap w-full">
                  <div className="w-[160px] mb-[12px] mr-[12px]">
                    <TitleComponent type="label" text={tManagePayments('bank')} />
                    <CustomDropdown
                      className="w-[330px]"
                      options={(() => {
                        return banks.map((bank) => ({
                          value: bank.id,
                          label: bank.id === 0 ? tGlobal('all') : tGlobal(bank.code),
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

                  <div className="w-[160px] mb-[12px] mr-[12px]">
                    <TitleComponent type="label" text={tManagePayments('transactionType')} />
                    <div className="relative">
                      <select
                        id="transactionTypeSelect"
                        className="control-field"
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
                      <IconArrowDown className="w-[12px] h-[12px] absolute right-[3px] top-[7px] fill-[#333] dark:fill-[#ededed]  " />
                    </div>
                  </div>

                  <div className="w-[160px] mb-[12px] mr-[12px] ">
                    <TitleComponent type="label" text={tManagePayments('status')} />
                    <div className="relative">
                      <select
                        id="statusSelect"
                        className="control-field"
                        value={selectedStatus}
                        onChange={onStatusChange}
                      >
                        {BANK_STATUS_OPTIONS.map((status) => (
                          <option key={status.id} value={status.id}>
                            {tManagePayments(`bank-status-${status.id}`)}
                          </option>
                        ))}
                      </select>
                      <IconArrowDown className="w-[12px] h-[12px] absolute right-[3px] top-[7px] fill-[#333] dark:fill-[#ededed]  " />
                    </div>
                  </div>
                </div>
              </>
            )}
            <div className="w-full border border-[var(--table-border-color)] border-b-0 p-[6px] flex">
              <div className="flex gap-[8px]">
                <ButtonComponent
                  variant="full"
                  className="w-[183px] [&>*]:w-[100%]"
                  text={`${tManagePayments('addBankAccount')}`}
                  onClick={onAddBankAccount}
                />
                <ButtonComponent
                  variant="outline"
                  className="w-[183px] [&>*]:w-[100%]"
                  text={tManagePayments('setLimit')}
                  onClick={onSetLimit}
                />
              </div>
              <div
                ref={searchRef}
                className={`box-search relative w-[250px] max-[768px]:w-[28px] ml-auto ${isSearchActive ? 'active' : ''}`}
              >
                <div
                  className="hidden max-[768px]:block bg-black/10 rounded-[3px] p-[6px] cursor-pointer"
                  onClick={handleSearchClick}
                >
                  <IconSearch />
                </div>
                <div className="relative max-[768px]:hidden child-search">
                  <span className="absolute transform-[translatey(-50%)] z-[10] top-1/2 left-[8px]">
                    <IconSearch />
                  </span>
                  <input
                    type="text"
                    id="searchInput"
                    className="control-field pl-[30px]!"
                    placeholder={tManagePayments('searchAccountNumberOrName')}
                    value={searchInput}
                    onChange={onSearchChange}
                    ref={searchInputRef}
                  />
                </div>
              </div>
            </div>
            <div className="table table-fixed-body">
              <table className="w-full whitespace-nowrap   ">
                <thead>
                  <tr>
                    <th className="bg-[var(--table-header-background)] left-0 min-w-[32px] sticky z-21">
                      #
                    </th>
                    <th className="col-shadown bg-[var(--table-header-background)] left-[32px] min-w-[82px] sticky z-21">
                      {tGlobal('action')}
                    </th>
                    <th>{tManagePayments('bank')}</th>
                    <th>{tManagePayments('transactionType')}</th>
                    <th>{tManagePayments('accountHolderName')}</th>
                    <th>{tManagePayments('bankAccountNumber')}</th>
                    <th>{tManagePayments('accountNickname')}</th>
                    <th>{tManagePayments('status')}</th>
                  </tr>
                </thead>
                {!loading && bankAccounts && bankAccounts.length > 0 && (
                  <tbody className="font-[500]">
                    {bankAccounts.map((account, index) => (
                      <tr key={account.id}>
                        <td className="bg-[var(--table-background)] left-0 min-w-[32px] sticky z-11">
                          {index + 1}
                        </td>
                        <td className="col-shadown bg-[var(--table-background)] left-[32px] min-w-[82px] sticky z-11">
                          <div className="flex justify-center">
                            <ButtonIconComponent
                              style="Edit"
                              disabled={deleting === account.id}
                              onClick={() => onEditBankAccount(account.id)}
                            />
                            <ButtonIconComponent
                              style="Delete"
                              onClick={() => onDeleteBankAccount(account)}
                            />
                          </div>
                        </td>
                        <td className="text-left">
                          <div className="flex items-center relative pl-[25px]">
                            <span className="absolute top-[-2px] left-[0]">
                              <BankIcon code={account.code} />
                            </span>
                            {tGlobal(account.code)}
                          </div>
                        </td>
                        <td className="text-left">
                          {getTransactionTypeNames(account.transactionTypes)}
                        </td>
                        <td className="text-left">{account.accountHolderName}</td>
                        <td className="text-left">{account.accountNumber}</td>
                        <td className="text-left">{account.accountNickname}</td>
                        <td>
                          <StatusComponent
                            type={account.status === 1 ? 'completed' : 'disabled'}
                            text={account.status === 1 ? tGlobal('enabled') : tGlobal('disabled')}
                          />
                        </td>
                      </tr>
                    ))}
                  </tbody>
                )}
                {!loading && bankAccounts && bankAccounts.length === 0 && (
                  <tbody>
                    <NoRecordFound colSpan={8} />
                  </tbody>
                )}
              </table>
            </div>

            {showSetLimitModal && (
              <SetTransactionLimitModal
                onClose={onCloseSetLimitModal}
                onSave={onSaveSetLimit}
                isCashServiceEnabled={isCashServiceEnabled}
              />
            )}
          </div>
        </>
      )}
    </>
  );
};

export default BankAccountComponent;
