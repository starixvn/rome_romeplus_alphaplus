import React, { useState, useEffect } from 'react';
import { useTranslation } from 'react-i18next';
import { Bank, BankAccount, DepositMatchingMode } from '../../services/interfaces/Payments';
import { paymentProviderService } from '../../services/paymentsAPI';
import {
  hasDepositTransactionType,
  hasManualDepositTransactionType,
  hasAutoDepositTransactionType,
  hasWithdrawalTransactionType,
  hasManualWithdrawalTransactionType
} from '../../utils/bankTransactionUtils';
import { BANK_TRANSACTION_TYPES } from '../../constants/paymentConstants';
import { baseAPI } from '../../services/commonAPI';
import CustomDropdown, { DropdownOption } from '../../components/CustomDropdownComponent';
import Loading from '../../components/LoadingComponent';
import { BackButtonComponent } from '../../components/ButtonComponents';
import { ErrorMessageComponent, SuccessMessageComponent, ValidateMessageComponent } from '../../components/MessageComponents';
import MobileComponent from '../../components/MobileComponent';
import { ButtonApplyComponent, ButtonCancelComponent } from '../../components/ButtonComponents';
import TitleComponent from '../../components/TitleComponent';

interface UpdateBankAccountComponentProps {
  onClose: () => void;
  onSave: () => void;
  bankAccount: BankAccount;
}

interface FormState {
  accountNickname: string;
  accountHolderName: string;
  accountNumber: string;
  selectedBank: string;
  status: string;
  promptPay: boolean;
  mobile: string;
  countryCode: string;
  creator: string;
  bankDepositMatchingMode: DepositMatchingMode;
  bank_TransTypeList: string;
  agent_DepositMatchingMode: number;
  selectedManualDeposit: boolean;
  selectedAutoDeposit: boolean;
  selectedManualWithdrawal: boolean;
}

interface FormErrors {
  mobile: string;
  countryCode: string;
  selectedTransactionType: string;
  selectedDepositMatchingMode: string;
}

const UpdateBankAccountComponent: React.FC<UpdateBankAccountComponentProps> = ({
  onClose,
  onSave,
  bankAccount
}) => {
  const { t: tManagePayments } = useTranslation('managePayments');
  const { t: tGlobal } = useTranslation('global');
  
  const [error, setError] = useState<string | null>(null);
  const [successMessage, setSuccessMessage] = useState<string>('');
  const [loading, setLoading] = useState<boolean>(false);
  const [initialLoading, setInitialLoading] = useState<boolean>(true);
  const [banks, setBanks] = useState<Bank[]>([]);
  const [countryCode, setCountryCode] = useState<string>('');
  const [mobile, setMobile] = useState<string>('');
  const [isBlockAction, setIsBlockAction] = useState<boolean>(false);

  const [formState, setFormState] = useState<FormState>({
    accountNickname: bankAccount.accountNickname,
    accountHolderName: bankAccount.accountHolderName,
    accountNumber: bankAccount.accountNumber,
    selectedBank: bankAccount.bankId.toString(),
    status: bankAccount.status.toString(),
    promptPay: bankAccount.promptPay,
    mobile: bankAccount.mobile,
    countryCode: bankAccount.countryCode || '',
    creator: bankAccount.creator || '',
    bankDepositMatchingMode: bankAccount.bankDepositMatchingMode,
    bank_TransTypeList: bankAccount.bank_TransTypeList,
    agent_DepositMatchingMode: bankAccount.agent_DepositMatchingMode,
    selectedManualDeposit: false,
    selectedAutoDeposit: false,
    selectedManualWithdrawal: false
  });

  const [orgFormState, setOrgFormState] = useState<FormState>({
    accountNickname: bankAccount.accountNickname,
    accountHolderName: bankAccount.accountHolderName,
    accountNumber: bankAccount.accountNumber,
    selectedBank: bankAccount.bankId.toString(),
    status: bankAccount.status.toString(),
    promptPay: bankAccount.promptPay,
    mobile: bankAccount.mobile,
    countryCode: bankAccount.countryCode || '',
    creator: bankAccount.creator || '',
    bankDepositMatchingMode: bankAccount.bankDepositMatchingMode,
    bank_TransTypeList: bankAccount.bank_TransTypeList,
    agent_DepositMatchingMode: bankAccount.agent_DepositMatchingMode,
    selectedManualDeposit: false,
    selectedAutoDeposit: false,
    selectedManualWithdrawal: false
  });

  const hasFormChanged = (): boolean => {
    return (
      formState.accountNickname !== orgFormState.accountNickname ||
      formState.status !== orgFormState.status ||
      formState.promptPay !== orgFormState.promptPay ||
      formState.mobile !== orgFormState.mobile ||
      formState.countryCode !== orgFormState.countryCode ||
      formState.bankDepositMatchingMode !== orgFormState.bankDepositMatchingMode ||
      formState.bank_TransTypeList !== orgFormState.bank_TransTypeList ||
      formState.agent_DepositMatchingMode !== orgFormState.agent_DepositMatchingMode ||
      formState.selectedManualDeposit !== orgFormState.selectedManualDeposit ||
      formState.selectedAutoDeposit !== orgFormState.selectedAutoDeposit ||
      formState.selectedManualWithdrawal !== orgFormState.selectedManualWithdrawal
    );
  };

  const [fieldErrors, setFieldErrors] = useState<FormErrors>({
    mobile: '',
    countryCode: '',
    selectedTransactionType: '',
    selectedDepositMatchingMode: ''
  });

  const fetchBankAccount = async () => {
    try {
      setLoading(true);
      const fetchedBankAccount = await paymentProviderService.getBankAccountById(bankAccount.id);
      if (fetchedBankAccount) {
        const newFormState = {
          accountNickname: fetchedBankAccount.accountNickname,
          accountHolderName: fetchedBankAccount.accountHolderName,
          accountNumber: fetchedBankAccount.accountNumber,
          status: fetchedBankAccount.status.toString(),
          promptPay: fetchedBankAccount.promptPay,
          selectedBank: fetchedBankAccount.bankId.toString(),
          bankDepositMatchingMode: fetchedBankAccount.bankDepositMatchingMode,
          bank_TransTypeList: fetchedBankAccount.bank_TransTypeList,
          agent_DepositMatchingMode: fetchedBankAccount.agent_DepositMatchingMode,
          selectedManualDeposit: fetchedBankAccount.agent_TransTypeList.split(',').includes(BANK_TRANSACTION_TYPES[1].id.toString()),
          selectedAutoDeposit: fetchedBankAccount.agent_TransTypeList.split(',').includes(BANK_TRANSACTION_TYPES[2].id.toString()),
          selectedManualWithdrawal: fetchedBankAccount.agent_TransTypeList.split(',').includes(BANK_TRANSACTION_TYPES[3].id.toString()),
          creator: fetchedBankAccount.creator,

          mobile: mobile,
          countryCode: countryCode
        };
        setFormState(newFormState);
        setOrgFormState(newFormState);

        if(fetchedBankAccount.promptPay){
          var fm = fetchedBankAccount.mobile;
          var c = fm.includes('-') ? fm.split('-')[0] : '';
          var m = fm.includes('-') ? fm.split('-')[1] : fm;
          setCountryCode(c);
          setMobile(m);
          setFormState(prev => ({ ...prev, countryCode: c, mobile: m }));
          setOrgFormState(prev => ({ ...prev, countryCode: c, mobile: m }));
        }
      }
    } catch (err) {
      setError(tManagePayments('failedToLoadBankAccount'));
      console.error('Error fetching bank account:', err);
    } finally {
      setLoading(false);
    }
  };
  const fetchBanks = async () => {
    try {
      const fetchedBanks = await paymentProviderService.getBanksByTransactionType(0);
      setBanks(fetchedBanks);
    } catch (err) {
      console.error('Error fetching banks:', err);
      setError(tManagePayments('failedToFetchBanks'));
    }
  };

  const fetchCountryCodes = async () => {
    try {
      const countryCodes = await baseAPI.getCountryCodes();
      if (!formState.countryCode && countryCodes.length > 0) {
        setCountryCode(countryCodes[0]);
        setMobile('');

        setFormState(prev => ({ ...prev, countryCode: countryCodes[0] }));
        setOrgFormState(prev => ({ ...prev, countryCode: countryCodes[0] }));
      }
    } catch (err) {
      console.error('Error fetching mobile prefixes:', err);
    }
  };

  useEffect(() => {
    Promise.all([fetchBanks(), fetchBankAccount(), fetchCountryCodes()]).finally(() => {
      setInitialLoading(false);
    });
  }, [bankAccount.id, tManagePayments]);

  if (initialLoading) {
    return <Loading />;
  }

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError(null);
    setLoading(true);

    if (!hasFormChanged()) {
      setError(tGlobal('thereNoChangeToUpdate'));
      setLoading(false);
      return;
    }

    // Field-level validation
    const errors = {
      mobile: formState.promptPay && !mobile ? tGlobal('pleaseFillField') : '',
      countryCode: formState.promptPay && !countryCode ? tGlobal('pleaseFillField') : '',
      selectedTransactionType:
        !formState.selectedManualDeposit && !formState.selectedAutoDeposit && !formState.selectedManualWithdrawal
          ? tManagePayments('pleaseSelectTransactionType')
          : '',
      selectedDepositMatchingMode:
        formState.selectedAutoDeposit && !formState.agent_DepositMatchingMode
          ? tManagePayments('pleaseSelectDepositMatchingMode')
          : '',
    };
    setFieldErrors(errors);
    const hasError = Object.values(errors).some(Boolean);
    if (hasError) {
      setLoading(false);
      return;
    }

    try {
      // Combine selected transaction types into a comma-separated string
      const selectedTransactionTypes: string[] = [];

      if (formState.selectedManualDeposit) {
        selectedTransactionTypes.push(BANK_TRANSACTION_TYPES[1].id.toString());
      }

      if (formState.selectedAutoDeposit) {
        selectedTransactionTypes.push(BANK_TRANSACTION_TYPES[2].id.toString());
      }

      if (formState.selectedManualWithdrawal) {
        selectedTransactionTypes.push(BANK_TRANSACTION_TYPES[3].id.toString());
      }

      const transactionTypesString = selectedTransactionTypes.join(',');

      const result = await paymentProviderService.updateBankAccount({
        id: bankAccount.id,
        accountNickname: formState.accountNickname,
        accountHolderName: formState.accountHolderName,
        accountNumber: formState.accountNumber,
        bankId: parseInt(formState.selectedBank),
        bankName: banks.find((b) => b.id === parseInt(formState.selectedBank))?.name || '',
        status: parseInt(formState.status),
        promptPay: formState.promptPay,
        mobile: mobile,
        countryCode: countryCode,
        lastUpdated: new Date().toISOString(),
        lastUpdatedStr: new Date().toLocaleString(),
        code: '',
        creator: formState.creator,
        transactionTypes: '',
        depositMatchingMode: 0,
        bank_TransTypeList: formState.bank_TransTypeList,
        agent_TransTypeList: transactionTypesString,
        agent_DepositMatchingMode: formState.agent_DepositMatchingMode,
        bankDepositMatchingMode: formState.bankDepositMatchingMode,
      });

      if (result.isSuccessful) {
        setIsBlockAction(true);
        window.scrollTo({ top: 0, behavior: 'smooth' });
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
    setFormState({ ...formState, selectedManualDeposit: !formState.selectedManualDeposit });
  };

  const toggleAutoDeposit = () => {
    setFormState({ ...formState, selectedAutoDeposit: !formState.selectedAutoDeposit });
  };

  const toggleManualWithdrawal = () => {
    setFormState({ ...formState, selectedManualWithdrawal: !formState.selectedManualWithdrawal });
  };

  const handleBankChange = (option: DropdownOption) => {
    setFormState({ ...formState, selectedBank: option.value.toString() });
  };

  return (
    <div tabIndex={-1}>
      <BackButtonComponent onClick={onClose} />      
      <div className="modal-dialog">
        <div className="modal-content max-w-[1200px]">
          <div className="mb-[8px]">           
            <TitleComponent  title={tManagePayments('bankManagement')} />
          </div>
          <div
            className={`bg-[#ffffff] p-[16px] rounded-[5px] shadow-[0_1px_5px_#0003,0_2px_2px_#00000024,0_3px_1px_-2px_#0000001f] mb-[20px] ${bankAccount.status === 1 ? 'border-l-[12px] border-[#5cb774]' : 'bg-[#D4E7D9]'}`}
          >
            <div className="flex mb-[12px]">
              <label className="font-semibold text-[16px] max-[768px]:text-[20px] mr-[5px] text-[var(--text-color-primary)]">
                {bankAccount.accountNickname}
              </label>
              <div>
                <span
                  className={`font-semibold rounded-[20px] px-[10px] py-[3px] uppercase ${bankAccount.status === 1 ? 'bg-[#D4E7D9]' : 'bg-[#FBD8D8]'}`}
                >
                  {bankAccount.status === 1 ? tGlobal('enabled') : tGlobal('disabled')}
                </span>
              </div>
            </div>

            <div className="flex text-[#333333] font-semibold">
              <div className="flex flex-col mr-[32px]">
                <label>{tManagePayments('createdBy')}</label>
                <label>{formState.creator}</label>
              </div>
              <div className="flex flex-col">
                <label>{tManagePayments('lastUpdated')}</label>
                <label>{bankAccount.lastUpdatedStr}</label>
              </div>
            </div>
          </div>

          <form
            onSubmit={handleSubmit}
            className="bg-[#ffffff] p-[16px] rounded-[5px] shadow-[0_1px_5px_#0003,0_2px_2px_#00000024,0_3px_1px_-2px_#0000001f]"
          >
            <div className="modal-body">
              <ErrorMessageComponent message={error} />
              <SuccessMessageComponent message={successMessage} />

              <div className="mb-[8px]">
                <div className="pb-[8px] text-[16px] max-[768px]:text-[20px] text-[var(--text-color-primary)] font-semibold ">
                  {tManagePayments('bankStatus')}
                </div>
                <p className="pb-[8px] ">{tManagePayments('bankNote1')}</p>
                <div className="btn-group" role="group">
                  <button
                    type="button"
                    className={` w-[100px] px-[16px] py-[8px] cursor-pointer rounded-tl-[4px] rounded-bl-[4px] ${formState.status === '1' ? 'bg-[var(--button-switch-toggle-background-active)] text-[var(--button-switch-toggle-background-color-active)]' : 'bg-[var(--button-switch-toggle-background)] text-[var(--button-switch-toggle-background-color)]'}`}
                    onClick={() => setFormState({ ...formState, status: '1' })}
                  >
                    {tGlobal('enable')}
                  </button>
                  <button
                    type="button"
                    className={` w-[100px] px-[16px] py-[8px] cursor-pointer rounded-tr-[4px] rounded-br-[4px] ${formState.status === '0' ? 'bg-[var(--button-switch-toggle-background-active)] text-[var(--button-switch-toggle-background-color-active)]' : 'bg-[var(--button-switch-toggle-background)] text-[var(--button-switch-toggle-background-color)]'}`}
                    onClick={() => setFormState({ ...formState, status: '0' })}
                  >
                    {tGlobal('disable')}
                  </button>
                </div>
              </div>

              <div className="mt-[8px] text-[16px] max-[768px]:text-[20px] text-[var(--text-color-primary)] font-semibold ">
                {tManagePayments('accountInformation')}
              </div>
              <div className="mb-[16px]">
                <label className="block  font-semibold text-[#333333] my-[8px]">
                  {tManagePayments('bank')}
                </label>
                <div className="w-[470px]">
                  <CustomDropdown
                    options={banks.map((bank) => ({
                      value: bank.id,
                      label: bank.name,
                      code: bank.code,
                    }))}
                    onSelect={handleBankChange}
                    disable
                    initialValue={parseInt(formState.selectedBank)}
                  />
                </div>
              </div>

              <div className="pb-[8px] text-[16px] text-[var(--text-color-primary)] font-semibold ">
                {tManagePayments('transactionType')}
                <span className="text-[#E25757]">*</span>
              </div>
              <ValidateMessageComponent message={fieldErrors.selectedTransactionType} />
              <div className="inline-block border-[1px] border-[var(--border-control)] rounded-[4px] px-[16px] pb-[16px] bg-[#f8f8f8] mb-[8px]">
                {hasDepositTransactionType(formState.selectedBank, banks) && (
                  <div className="mb-[8px]">
                    <label className="block  font-semibold text-[#333333] my-[8px]">
                      {tManagePayments('depositType')}
                    </label>
                    <div className="flex gap-[10px]" role="group">
                      {hasManualDepositTransactionType(formState.selectedBank, banks) && (
                        <button
                          type="button"
                          className={`block w-[230px] cursor-pointer h-[120px]  rounded-[4px] border-[1px] border-[var(--button-border)]  ${formState.selectedManualDeposit ? 'text-[#ffffff] bg-[var(--button-background)]' : 'text-[var(--text-color-primary)] bg-[#ffffff]'}`}
                          onClick={() => {
                            toggleManualDeposit();
                          }}
                        >
                          <span className="text-[14px] font-bold ">
                            {tManagePayments('slipUpload')}
                          </span>
                          <br />
                          <span className="">{tManagePayments('manualDeposit')}</span>
                        </button>
                      )}
                      {hasAutoDepositTransactionType(formState.selectedBank, banks) && (
                        <button
                          type="button"
                          className={`block w-[230px] cursor-pointer h-[120px] rounded-[4px] border-[1px] border-[var(--button-border)] ${formState.selectedAutoDeposit ? 'text-[#ffffff] bg-[var(--button-background)]' : 'text-[var(--text-color-primary)] bg-[#ffffff]'}`}
                          onClick={() => {
                            toggleAutoDeposit();
                          }}
                        >
                          <span className="text-[14px] font-bold">
                            {formState.bankDepositMatchingMode?.sourceTypeName}
                          </span>
                          <br />
                          <span className="">{tManagePayments('autoDeposit')}</span>
                        </button>
                      )}
                    </div>
                  </div>
                )}

                {hasWithdrawalTransactionType(formState.selectedBank, banks) && (
                  <div className="mb-[8px]">
                    <label className="block  font-semibold text-[#333333] my-[8px]">
                      {tManagePayments('withdrawalType')}
                    </label>
                    <div className="btn-group" role="group">
                      {hasManualWithdrawalTransactionType(formState.selectedBank, banks) && (
                        <button
                          type="button"
                          className={`block cursor-pointer w-[230px] h-[120px] rounded-[4px] border-[1px] border-[var(--button-border)] ${formState.selectedManualWithdrawal ? 'text-[#ffffff] bg-[var(--button-background)]' : 'text-[var(--text-color-primary)] bg-[#ffffff]'}`}
                          onClick={toggleManualWithdrawal}
                        >
                          <span className="text-[14px] font-bold">
                            {tManagePayments('manualWithdrawal')}
                          </span>
                        </button>
                      )}
                    </div>
                  </div>
                )}
              </div>

              {(() => {
                const selectedBankObj = banks.find((b) => b.id === parseInt(formState.selectedBank));
                const mappingTypes = selectedBankObj?.depositMatchingMode?.mappingTypes || [];

                return (
                  selectedBankObj &&
                  mappingTypes.length > 0 &&
                  formState.selectedAutoDeposit && (
                    <div className="mb-[8px]">
                      <label className="block  font-semibold text-[#333333] my-[8px]">
                        {tManagePayments('depositMatchingMode')}
                      </label>
                      <div className="flex" role="group">
                        {mappingTypes.includes(1) && (
                          <>
                            <label
                              className="flex items-center cursor-pointer capitalize mr-[8px]"
                              htmlFor="manualMatching1"
                              onClick={() => setFormState({ ...formState, agent_DepositMatchingMode: 1 })}
                            >
                              <input
                                className="w-[20px] h-[20px] m-[10px] cursor-pointer accent-[var(--primary-color)]"
                                id="manualMatching1"
                                type="radio"
                                name="depositMatchingMode"
                                value={formState.agent_DepositMatchingMode}
                                checked={formState.agent_DepositMatchingMode == 1}
                              />
                              <span className=" leading-[14px] text-[#333333]">
                                {tManagePayments('decimal')}
                              </span>
                            </label>
                          </>
                        )}
                        {mappingTypes.includes(2) && (
                          <>
                            <label
                              className="flex items-center cursor-pointer capitalize"
                              htmlFor="manualMatching2"
                              onClick={() => setFormState({ ...formState, agent_DepositMatchingMode: 2 })}
                            >
                              <input
                                className="w-[20px] h-[20px] m-[10px] cursor-pointer accent-[var(--primary-color)]"
                                id="manualMatching2"
                                type="radio"
                                name="depositMatchingMode"
                                value={formState.agent_DepositMatchingMode}
                                checked={formState.agent_DepositMatchingMode == 2}
                              />
                              <span className=" leading-[14px] text-[#333333]">
                                {tManagePayments('bankMatching')}
                              </span>
                            </label>
                          </>
                        )}
                      </div>
                      <ValidateMessageComponent message={fieldErrors.selectedDepositMatchingMode} />
                    </div>
                  )
                );
              })()}

              <div className="flex flex-nowrap gap-[16px] max-[768px]:justify-between max-[768px]:gap-[0] max-[768px]:flex-wrap w-full">
                <div className="w-[230px] pt-[4px] max-[768px]:w-[48%]">
                  <label className="block  font-semibold text-[#333333] my-[8px]">
                    {tManagePayments('accountHolderName')}
                    <span className="text-[#E25757]">*</span>
                  </label>
                  <input
                    type="text"
                    className="h-[32px] max-[768px]:h-[40px] w-[100%] border-[1px] border-[var(--border-control)] rounded-[4px] px-[8px]  focus:outline-none focus:border-[var(--button-border)] cursor-pointer read-only:bg-[#f4f4f4] read-only:cursor-not-allowed read-only:focus:border-[#0000003D]"
                    value={formState.accountHolderName}
                    readOnly
                    maxLength={255}
                  />
                  <label className=" block mt-[4px] text-[#828282] text-[11px] max-[768px]:text-[15px] font-semibold">
                    {tManagePayments('provideFullName')}
                  </label>
                </div>

                <div className="w-[230px] pt-[4px] max-[768px]:w-[48%]">
                  <label className="block  font-semibold text-[#333333] my-[8px]">
                    {tManagePayments('bankAccountNumber')}
                    <span className="text-[#E25757]">*</span>
                  </label>
                  <input
                    type="text"
                    className="h-[32px] max-[768px]:h-[40px] w-[100%] border-[1px] border-[var(--border-control)] rounded-[4px] px-[8px]  focus:outline-none focus:border-[var(--button-border)] cursor-pointer read-only:bg-[#f4f4f4] read-only:cursor-not-allowed read-only:focus:border-[#0000003D]"
                    value={formState.accountNumber}
                    readOnly
                    maxLength={100}
                  />
                  <label className="block mt-[4px] text-[#828282] text-[11px] max-[768px]:text-[15px] font-semibold">{tGlobal('maxLength').replace('{0}', '100')}</label>
                </div>

                <div className="w-[230px] pt-[4px] max-[768px]:w-[48%]">
                  <label className="block  font-semibold text-[#333333] my-[8px]">
                    {tManagePayments('accountNickname')}
                  </label>

                  <input
                    type="text"
                    className={`h-[32px] max-[768px]:h-[40px] w-[100%] border-[1px] border-[var(--border-control)] rounded-[4px] px-[8px]  focus:outline-none focus:border-[var(--button-border)] cursor-pointer `}
                    value={formState.accountNickname}
                    onChange={(e) => {
                      setFormState({ ...formState, accountNickname: e.target.value });
                    }}
                    maxLength={255}
                  />
                  <label className="block mt-[4px] text-[#828282] text-[11px] max-[768px]:text-[15px] font-semibold">
                    {tManagePayments('accountNicknameNote1')}
                  </label>
                </div>
              </div>

              <div className="mb-[8px]">
                <div className="pb-[8px] mt-[8px] text-[16px] max-[768px]:text-[20px] text-[var(--text-color-primary)] font-semibold ">
                  {tManagePayments('promptPay')}
                </div>
                <div className="block  font-semibold text-[#333333] mb-[8px]">
                  {tManagePayments('linkMobile')}
                </div>
                <div className="btn-group" role="group">
                  <button
                    type="button"
                    className={` w-[70px] px-[16px] py-[8px] cursor-pointer rounded-tl-[4px] rounded-bl-[4px] ${formState.promptPay ? 'bg-[var(--button-switch-toggle-background-active)] text-[var(--button-switch-toggle-background-color-active)]' : 'bg-[var(--button-switch-toggle-background)] text-[var(--button-switch-toggle-background-color)]'}`}
                    onClick={() => setFormState({ ...formState, promptPay: true })}
                  >
                    {tGlobal('on')}
                  </button>
                  <button
                    type="button"
                    className={` w-[70px] px-[16px] py-[8px] cursor-pointer rounded-tr-[4px] rounded-br-[4px] ${!formState.promptPay ? 'bg-[var(--button-switch-toggle-background-active)] text-[var(--button-switch-toggle-background-color-active)]' : 'bg-[var(--button-switch-toggle-background)] text-[var(--button-switch-toggle-background-color)]'}`}
                    onClick={() => setFormState({ ...formState, promptPay: false })}
                  >
                    {tGlobal('off')}
                  </button>
                </div>
              </div>

              {formState.promptPay && (
                <MobileComponent
                  label={tManagePayments('mobile')}
                  countryCode={countryCode}
                  mobile={mobile}
                  fieldErrors={fieldErrors}
                  onCountryCodeChange={(e) => {
                    setFormState({ ...formState, countryCode: e.target.value });
                    setCountryCode(e.target.value);
                  }}
                  onMobileChange={(e) => {
                    setFormState({ ...formState, mobile: e.target.value });
                    setMobile(e.target.value);
                  }}
                  placeholder={tManagePayments('enterMobileNumber')}
                />
              )}
            </div>
            <div className="flex justify-start py-[8px] mt-[8px] gap-[8px]">              
              <ButtonCancelComponent text={tGlobal('cancel')} onClick={onClose}  />
              <ButtonApplyComponent text={loading ? tGlobal('saving') : tGlobal('save')} onClick={() => handleSubmit({ preventDefault: () => {} } as React.FormEvent)} disabled={loading || isBlockAction} />
            </div>
          </form>
        </div>
      </div>
    </div>
  );
};

export default UpdateBankAccountComponent;
