import React, { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';

import BankIcon from '../../components/BankComponents';
import {
  BackButtonComponent,
  ButtonApplyComponent,
  ButtonCancelComponent,
  ButtonOffComponent,
  ButtonOnComponent,
  DisableButtonComponent,
  EnableButtonComponent,
} from '../../components/Buttons/index';
import CustomDropdown, { DropdownOption } from '../../components/CustomDropdownComponent';
import Loading from '../../components/LoadingComponent';
import {
  ErrorMessageComponent,
  SuccessMessageComponent,
  ValidateMessageComponent,
} from '../../components/Message';
import MobileComponent from '../../components/MobileComponent';
import {
  TitleComponent,
  LabelTitleComponent,
  MediumTitleComponent,
} from '../../components/Title';
import { BANK_TRANSACTION_TYPES, bankStatusConstant } from '../../constants/paymentConstants';
import { baseAPI } from '../../services/commonAPI';
import {
  Bank,
  BankAccount,
  createDefaultBankAccount,
  DepositMatchingMode,
} from '../../services/interfaces/Payments';
import { paymentProviderService } from '../../services/paymentsAPI';
import {
  hasAutoDepositTransactionType,
  hasDepositTransactionType,
  hasManualDepositTransactionType,
  hasManualWithdrawalTransactionType,
  hasWithdrawalTransactionType,
} from '../../utils/bankTransactionUtils';
import { isNumberIncludes } from '../../utils/sharedUtils';
import { createErrorResultModel } from '../../services/interfaces/ResultModel';
import { StatusCompletedComponent, StatusDisabledComponent } from '../../components/StatusComponents';

interface UpdateBankAccountComponentProps {
  onClose: () => void;
  onSave: () => void;
  bankAccount: BankAccount; 
  isCashServiceEnabled: boolean;
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
  bankAccount,
  isCashServiceEnabled,
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
  const [suggestedCountryCode, setSuggestedCountryCode] = useState<string>('');
  const [fetchedBankAccount, setFetchedBankAccount] = useState<BankAccount | null>(null);

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
    selectedManualWithdrawal: false,
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
    selectedManualWithdrawal: false,
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
    selectedDepositMatchingMode: '',
  });

  const validateFormState = () => {
    const errors: FormErrors = {
      mobile: '',
      countryCode: '',
      selectedTransactionType: '',
      selectedDepositMatchingMode: '',
    };

    // Validate transaction type selection
    if (
      !formState.selectedManualDeposit &&
      !formState.selectedAutoDeposit &&
      !formState.selectedManualWithdrawal
    ) {
      errors.selectedTransactionType = tManagePayments('pleaseSelectTransactionType');
    }

    // Validate deposit matching mode for auto deposit
    if (formState.selectedAutoDeposit && !formState.agent_DepositMatchingMode) {
      errors.selectedDepositMatchingMode = tManagePayments('pleaseSelectDepositMatchingMode');
    }

    // Validate mobile and country code for PromptPay
    if (formState.promptPay) {
      if (!mobile) {
        errors.mobile = tGlobal('pleaseFillField');
      }
      if (!countryCode) {
        errors.countryCode = tGlobal('pleaseFillField');
      }
    }

    setFieldErrors(errors);
    const hasError = Object.values(errors).some(Boolean);
    return !hasError;
  };

  const fetchBankAccount = async () => {
    setLoading(true);
    const response = await paymentProviderService.getBankAccountById(bankAccount.id);
    if (response.error) {
      setError(response.error.message);
      setLoading(false);
      return;
    }

    const fetchedBankAccount = response.data || createDefaultBankAccount();
    setFetchedBankAccount(fetchedBankAccount);

    if (fetchedBankAccount) {
      var c, m;
      if (fetchedBankAccount.promptPay) {
        const fm = fetchedBankAccount.mobile;
        c = fm.includes('-') ? fm.split('-')[0] : '';
        m = fm.includes('-') ? fm.split('-')[1] : fm;
        setCountryCode(c);
        setMobile(m);
      } else {
        c = suggestedCountryCode;
        m = '';
        setCountryCode(c);
        setMobile(m);
      }

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
        selectedManualDeposit: isNumberIncludes(fetchedBankAccount.agent_TransTypeList, BANK_TRANSACTION_TYPES[1].id),
        selectedAutoDeposit: isNumberIncludes(fetchedBankAccount.agent_TransTypeList, BANK_TRANSACTION_TYPES[2].id),
        selectedManualWithdrawal: isNumberIncludes(fetchedBankAccount.agent_TransTypeList, BANK_TRANSACTION_TYPES[3].id),
        creator: fetchedBankAccount.creator,
        mobile: m,
        countryCode: c,
      };

      setFormState(newFormState);
      setOrgFormState(newFormState);
    }
    setLoading(false);
  };

  const fetchBanks = async () => {
    const response = await paymentProviderService.getBanksByTransactionType(0);
    if (response.error) {
      setError(response.error.message);
      return;
    }

    const fetchedBanks = response.data || [];
    setBanks(fetchedBanks);
  };

  const fetchCountryCodes = async () => {
    const response = await baseAPI.getCountryCodes();
    if (response.error) {
      setError(response.error.message);
      return;
    }

    const countryCodes = response.data || [];
    if (countryCodes.length == 0) {
      return;
    }

    setSuggestedCountryCode(countryCodes[0]);
  };

  useEffect(() => {
    Promise.all([fetchBanks(), fetchCountryCodes(), fetchBankAccount()]).finally(() => {
      setInitialLoading(false);
    });
  }, []);

  if (initialLoading) {
    return <Loading />;
  }

  const onSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError(null);
    setLoading(true);

    if (!hasFormChanged()) {
      setError(tGlobal('thereNoChangeToUpdate'));
      setLoading(false);
      window.scrollTo({ top: 0, behavior: 'smooth' });
      return;
    }

    const isValid = validateFormState();
    if (!isValid) {
      setLoading(false);
      return;
    }

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
    const response = await paymentProviderService.updateBankAccount({
      id: bankAccount.id,
      accountNickname: formState.accountNickname,
      accountHolderName: formState.accountHolderName,
      accountNumber: formState.accountNumber,
      bankId: parseInt(formState.selectedBank),
      bankName: banks.find((b) => b.id === parseInt(formState.selectedBank))?.name || '',
      status: parseInt(formState.status),
      promptPay: formState.promptPay,
      mobile: formState.promptPay ? mobile : '',
      countryCode: formState.promptPay ? countryCode : '',
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

    if (response.error) {
      setError(response.error.message);
      setLoading(false);
      return;
    }
    const result = response.data || createErrorResultModel(1, tGlobal('errorOccurred'));

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
    setLoading(false);
    
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

  const onBankChange = (option: DropdownOption) => {
    setFormState({ ...formState, selectedBank: option.value.toString() });
  };

  return (
    <div tabIndex={-1}>
       
        <div className="modal-content max-w-[1200px]">
          <div className="flex items-center justify-flex-start mb-[8px] py-[8px] border-[var(--border-control)] border-b-[1px] [&>:nth-child(2)]:border-0 [&>:nth-child(2)]:mb-[0] [&>:nth-child(1)]:mb-[0]">
            <BackButtonComponent onClick={onClose} />
            <TitleComponent title={tManagePayments('bankManagement')} />
          </div>
          <div className={`bg-[#ffffff] dark:bg-[#2b2b2b] px-[20px] py-[10px] rounded-[5px] max-w-[722px] shadow-[0_1px_5px_#0003,0_2px_2px_#00000024,0_3px_1px_-2px_#0000001f] mb-[15px] border-l-[12px] ${fetchedBankAccount?.status === 1 ? ' border-[#5cb774]' : 'border-[#af4646]'}`} >
            <div className="flex gap-[30px]">
              <div className="text-center">                 
                <MediumTitleComponent text={fetchedBankAccount?.accountNickname || ''} />
                <div>                
                  {fetchedBankAccount?.status === bankStatusConstant.enabled ? <StatusCompletedComponent text={tGlobal('enabled')} /> : <StatusDisabledComponent />}
                </div>
              </div>
              <div>
                <LabelTitleComponent className="my-[0]! font-semibold!" text={tManagePayments('createdBy')} />
                <label>{formState.creator}</label>
              </div>
              <div>
                <LabelTitleComponent className="my-[0]! font-semibold!" text={tManagePayments('lastUpdated')} />
                <label>{bankAccount.lastUpdatedStr}</label>
              </div>
            </div>             
          </div>
          <form onSubmit={onSubmit}>
            <div className="modal-body">
              <ErrorMessageComponent className="mb-[10px] max-[768px]:mb-[0]" message={error} />
              <SuccessMessageComponent className="mb-[10px] max-[768px]:mb-[0]" message={successMessage} />
              <div className="mb-[16px]">
                <MediumTitleComponent text={tManagePayments('bankStatus')} />
                <p className="pb-[8px] ">{tManagePayments('bankNote1')}</p>
                <div className="inline-block border-[1px] border-[var(--border-control)] rounded-[5px]" role="group">
                  <EnableButtonComponent
                    isActive={formState.status === '1'}
                    type="button"
                    onClick={() => setFormState({ ...formState, status: '1' })}
                  />
                  <DisableButtonComponent
                    isActive={formState.status === '0'}
                    type="button"
                    onClick={() => setFormState({ ...formState, status: '0' })}
                  />
                </div>
              </div>
              <MediumTitleComponent text={tManagePayments('accountInformation')} />
              <div className="mb-[16px]">
                <LabelTitleComponent text={tManagePayments('bank')} />
                <div className="w-[470px]">
                  <CustomDropdown
                    options={banks.map((bank) => ({
                      value: bank.id,
                      label: tGlobal(bank.code),
                      icon: <BankIcon code={bank.code} />,
                    }))}
                    onSelect={onBankChange}
                    disable
                    initialValue={parseInt(formState.selectedBank)}
                  />
                </div>
              </div>
              <MediumTitleComponent
                className="mb-[8px]"
                text={tManagePayments('transactionType')}
                required={true}
              />
              <div className="inline-block border-[1px] border-[var(--border-control)] rounded-[4px] px-[10px] pt-[5px] pb-[10px] bg-[#f8f8f8] dark:bg-[#2b2b2b] mb-[8px]">
                {hasDepositTransactionType(formState.selectedBank, banks) && (
                  <div className="mb-[8px]">
                    <LabelTitleComponent text={tManagePayments('depositType')} />
                    <div className="flex gap-[10px]" role="group">
                      {hasManualDepositTransactionType(formState.selectedBank, banks) && (
                        <button
                          type="button"
                          className={`transaction-box ${formState.selectedManualDeposit ? 'select' : ''}`}
                          onClick={() => {
                            toggleManualDeposit();
                          }}
                        >
                          <span className='check-box'></span>
                          <span className="text-[14px] font-semibold ">
                            {tManagePayments('slipUpload')}
                          </span>
                          <br />
                          <span className="">{tManagePayments('manualDeposit')}</span>
                        </button>
                      )}
                      {hasAutoDepositTransactionType(formState.selectedBank, banks) && (
                        <button
                          type="button"
                          className={`transaction-box ${formState.selectedAutoDeposit ? 'select' : ''}`}
                          onClick={() => {
                            toggleAutoDeposit();
                          }}
                        >
                          <span className='check-box'></span>
                          <span className="text-[14px] font-semibold">
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
                    <LabelTitleComponent text={tManagePayments('withdrawalType')} />
                    <div className="btn-group" role="group">
                      {hasManualWithdrawalTransactionType(formState.selectedBank, banks) && (
                        <button
                          type="button"
                          className={`transaction-box ${formState.selectedManualWithdrawal ? 'select' : ''}`}
                          onClick={toggleManualWithdrawal}
                        >
                          <span className='check-box'></span>
                          <span className="text-[14px] font-semibold">
                            {tManagePayments('manualWithdrawal')}
                          </span>
                        </button>
                      )}
                    </div>
                  </div>
                )}
              </div>
              <ValidateMessageComponent message={fieldErrors.selectedTransactionType} />

              {(() => {
                const selectedBankObj = banks.find(
                  (b) => b.id === parseInt(formState.selectedBank)
                );
                const mappingTypes = selectedBankObj?.depositMatchingMode?.mappingTypes || [];

                return (
                  selectedBankObj &&
                  mappingTypes.length > 0 &&
                  formState.selectedAutoDeposit && (
                    <div className="mb-[8px]">
                      <LabelTitleComponent text={tManagePayments('depositMatchingMode')} />
                      <div className="flex gap-[10px]" role="group">
                        {mappingTypes.includes(1) && (
                          <label
                            className="flex items-center cursor-pointer capitalize mr-[8px]"
                            htmlFor="manualMatching1"
                            onClick={() =>
                              setFormState({ ...formState, agent_DepositMatchingMode: 1 })
                            }
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
                        )}
                        {mappingTypes.includes(2) && (
                          <label
                            className="flex items-center cursor-pointer capitalize"
                            htmlFor="manualMatching2"
                            onClick={() =>
                              setFormState({ ...formState, agent_DepositMatchingMode: 2 })
                            }
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
                        )}
                      </div>
                      <ValidateMessageComponent message={fieldErrors.selectedDepositMatchingMode} />
                    </div>
                  )
                );
              })()}

              <div className="flex flex-nowrap mb-[16px] gap-[16px] max-[768px]:justify-between max-[768px]:gap-[0] max-[768px]:flex-wrap w-full">
                <div className="w-[240px] pt-[4px] max-[768px]:w-[48%]">
                  <LabelTitleComponent text={tManagePayments('accountHolderName')} required={true} />
                  <input
                    type="text"
                    className="control-field"
                    value={formState.accountHolderName}
                    readOnly
                    maxLength={255}
                  />
                  <label className=" block mt-[4px] text-[#828282] text-[90%]">
                    {tManagePayments('provideFullName')}
                  </label>
                </div>

                <div className="w-[240px] pt-[4px] max-[768px]:w-[48%]">
                  <LabelTitleComponent text={tManagePayments('bankAccountNumber')} required={true} />
                  <input
                    type="text"
                    className="control-field"
                    value={formState.accountNumber}
                    readOnly
                    maxLength={100}
                  />
                  <label className="block mt-[4px] text-[#828282] text-[90%]">
                    {tGlobal('maxLength').replace('{0}', '100')}
                  </label>
                </div>

                <div className="w-[240px] pt-[4px] max-[768px]:w-[48%]">
                  <LabelTitleComponent text={tManagePayments('accountNickname')} />
                  <input
                    type="text"
                    className="control-field"
                    value={formState.accountNickname}
                    onChange={(e) => {
                      setFormState({ ...formState, accountNickname: e.target.value });
                    }}
                    maxLength={255}
                  />
                  <label className="block mt-[4px] text-[#828282] text-[90%]">
                    {tManagePayments('accountNicknameNote1')}
                  </label>
                </div>
              </div>

              <div className="mb-[8px]">
                <MediumTitleComponent text={tManagePayments('promptPay')} />
                <div className="font-semibold mb-[8px]">
                  {tManagePayments('linkMobile')}
                </div>
                <div className="mb-[8px] inline-block border-[1px] border-[var(--border-control)] rounded-[5px]" role="group">
                  <ButtonOnComponent
                    type="button"
                    isActive={formState.promptPay}
                    onClick={() =>  {
                      if(fetchedBankAccount?.promptPay) { 
                        const fm = fetchedBankAccount.mobile;
                        const c = fm.includes('-') ? fm.split('-')[0] : '';
                        const m = fm.includes('-') ? fm.split('-')[1] : fm;
                        setCountryCode(c);
                        setMobile(m);
                        setFormState({ ...formState, promptPay: true, countryCode: c, mobile: m });
                      } else {
                        setCountryCode(suggestedCountryCode);
                        setMobile('');
                        setFormState({ ...formState, promptPay: true, countryCode: suggestedCountryCode, mobile: '' });
                      }
                    }}
                  />
                  <ButtonOffComponent
                    type="button"
                    isActive={!formState.promptPay}
                    onClick={() => setFormState({ ...formState, promptPay: false })}
                  />
                </div>
              </div>

              <MobileComponent
                isShow={formState.promptPay}
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
            </div>
            <div className="flex justify-start py-[8px] mt-[8px] gap-[8px]">
              <ButtonCancelComponent text={tGlobal('cancel')} onClick={onClose} />
              <ButtonApplyComponent
                text={loading ? tGlobal('saving') : tGlobal('save')}
                type='submit'
                onClick={() => onSubmit({ preventDefault: () => {} } as React.FormEvent)}
                disabled={!isCashServiceEnabled || loading || isBlockAction}
                title={isCashServiceEnabled ? '' : tGlobal('cashServiceDisabled')}
              />
            </div>
          </form>
        </div>
  
    </div>
  );
};

export default UpdateBankAccountComponent;
