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
} from '../../components/ButtonComponents';
import CustomDropdown, { DropdownOption } from '../../components/CustomDropdownComponent';
import { LoadingFullComponent } from '../../components/LoadingComponent';
import {
  ErrorMessageComponent,
  SuccessMessageComponent,
  ValidateMessageComponent,
} from '../../components/MessageComponents';
import MobileComponent from '../../components/MobileComponent';
import TitleComponent, {
  LabelTitleComponent,
  MediumTitleComponent,
} from '../../components/TitleComponents';
import { BANK_TRANSACTION_TYPES } from '../../constants/paymentConstants';
import { baseAPI } from '../../services/commonAPI';
import { Bank } from '../../services/interfaces/Payments/Bank';
import { paymentProviderService } from '../../services/paymentsAPI';
import {
  hasAutoDepositTransactionType,
  hasDepositTransactionType,
  hasManualDepositTransactionType,
  hasManualWithdrawalTransactionType,
  hasWithdrawalTransactionType,
} from '../../utils/bankTransactionUtils';
import { ensureIntegerInput, ensureNameInput } from '../../utils/inputHandlers';

interface AddBankAccountComponentProps {
  onClose: () => void;
  onSave: () => void;
}

const AddBankAccountComponent: React.FC<AddBankAccountComponentProps> = ({ onClose, onSave }) => {
  const { t: tManagePayments } = useTranslation('managePayments');
  const { t: tGlobal } = useTranslation('global');

  const [accountNickname, setAccountNickname] = useState<string>('');
  const [accountHolderName, setAccountHolderName] = useState<string>('');
  const [accountNumber, setAccountNumber] = useState<string>('');
  const [selectedBank, setSelectedBank] = useState<string>('');
  const [status, setStatus] = useState(1); // Default to active
  const [depositMatchingMode, setDepositMatchingMode] = useState<number>(0);
  const [promptPay, setPromptPay] = useState<boolean>(true);
  const [mobile, setMobile] = useState<string>('');
  const [countryCode, setCountryCode] = useState<string>('');
  const [error, setError] = useState<string | null>(null);
  const [successMessage, setSuccessMessage] = useState<string>('');
  const [loading, setLoading] = useState<boolean>(false);
  const [banks, setBanks] = useState<Bank[]>([]);
  const [selectedBankObj, setSelectedBankObj] = useState<Bank | undefined>(undefined);
  const [isLoadingBanks, setIsLoadingBanks] = useState<boolean>(true);
  const [isBlockAction, setIsBlockAction] = useState<boolean>(false);

  // Add state variables for selected transaction types
  const [selectedManualDeposit, setSelectedManualDeposit] = useState<boolean>(true);
  const [selectedAutoDeposit, setSelectedAutoDeposit] = useState<boolean>(false);
  const [selectedManualWithdrawal, setSelectedManualWithdrawal] = useState<boolean>(false);

  // Add field-level validation state
  const [fieldErrors, setFieldErrors] = useState({
    accountHolderName: '',
    accountNumber: '',
    selectedBank: '',
    mobile: '',
    countryCode: '',
    selectedTransactionType: '',
    selectedDepositMatchingMode: '',
  });

  useEffect(() => {
    const fetchBanks = async () => {
      setIsLoadingBanks(true);

      const response = await paymentProviderService.getBanksByTransactionType(0);

      if (response.error) {
        setError(response.error.message);
        setIsLoadingBanks(false);
        return;
      }

      const fetchedBanks = response.data || [];
      setBanks(fetchedBanks);
      if (fetchedBanks.length > 0) {
        const firstBank = fetchedBanks[0];
        setSelectedBank(firstBank.id.toString());
        setSelectedBankObj(firstBank);

        // Set transaction types based on first bank's capabilities
        setSelectedManualDeposit(true);
        setSelectedAutoDeposit(false);
        setSelectedManualWithdrawal(false);

        // Set deposit matching mode based on bank's capabilities
        if (firstBank.depositMatchingMode?.mappingTypes?.length > 0) {
          setDepositMatchingMode(0);
        }
      }

      setIsLoadingBanks(false);
    };

    const fetchCountryCodes = async () => {
      const response = await baseAPI.getCountryCodes();
      if (response.error) {
        setError(response.error.message);
        return;
      }

      const prefixes = response.data ?? [];
      setCountryCode(prefixes[0]);
    };

    fetchBanks();
    fetchCountryCodes();
  }, [tManagePayments]);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError(null);
    setLoading(true);

    // Field-level validation
    const errors = {
      accountHolderName: !accountHolderName ? tGlobal('pleaseFillField') : '',
      accountNumber: !accountNumber ? tGlobal('pleaseFillField') : '',
      selectedBank: !selectedBank ? tGlobal('pleaseFillField') : '',
      mobile: promptPay && !mobile ? tGlobal('pleaseFillField') : '',
      countryCode: promptPay && !countryCode ? tGlobal('pleaseFillField') : '',
      selectedTransactionType:
        !selectedManualDeposit && !selectedAutoDeposit && !selectedManualWithdrawal
          ? tManagePayments('pleaseSelectTransactionType')
          : '',
      selectedDepositMatchingMode:
        selectedAutoDeposit && !depositMatchingMode
          ? tManagePayments('pleaseSelectDepositMatchingMode')
          : '',
    };
    setFieldErrors(errors);
    const hasError = Object.values(errors).some(Boolean);
    if (hasError) {
      setLoading(false);
      return;
    }

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

    const response = await paymentProviderService.addBankAccount({
      id: 0, // Temporary ID for new bank account
      accountNickname,
      accountHolderName,
      accountNumber,
      bankId: parseInt(selectedBank),
      bankName: banks.find((b) => b.id === parseInt(selectedBank))?.name ?? '',
      status: status,
      promptPay,
      mobile: mobile,
      countryCode: countryCode,
      lastUpdated: new Date().toISOString(),
      lastUpdatedStr: new Date().toLocaleString(),
      code: '',
      creator: '',
      transactionTypes: '',
      depositMatchingMode: 0,
      bank_TransTypeList: '',
      agent_TransTypeList: transactionTypesString,
      agent_DepositMatchingMode: depositMatchingMode,
      bankDepositMatchingMode: {
        sourceType: 'BANK',
        mappingTypes: [depositMatchingMode],
        sourceTypeName: '',
      },
    });

    if (response.error) {
      setError(response.error.message);
      setLoading(false);
      return;
    }

    const result = response.data || {};
    if (result.isSuccessful) {
      setIsBlockAction(true);
      setSuccessMessage(result.message || tManagePayments('bankAccountAdded'));
      setTimeout(() => {
        onSave();
      }, 3000);
    } else {
      setError(result.message || tManagePayments('failedToAddBankAccount'));
    }

    setLoading(false);
  };

  const updateSelectTransactionTypeError = () => {
    setFieldErrors({
      ...fieldErrors,
      selectedTransactionType:
        !selectedManualDeposit && !selectedAutoDeposit && !selectedManualWithdrawal
          ? tManagePayments('pleaseSelectTransactionType')
          : '',
    });
  };
  // Toggle handlers for transaction type buttons
  const toggleManualDeposit = () => {
    setSelectedManualDeposit(!selectedManualDeposit);
    updateSelectTransactionTypeError();
  };

  const toggleAutoDeposit = () => {
    setSelectedAutoDeposit(!selectedAutoDeposit);
    updateSelectTransactionTypeError();
  };

  const toggleManualWithdrawal = () => {
    setSelectedManualWithdrawal(!selectedManualWithdrawal);
    updateSelectTransactionTypeError();
  };

  // Handler for bank selection changes
  const onBankChange = (option: DropdownOption) => {
    const newBankId = option.value.toString();
    setSelectedBank(newBankId);

    // Find and set the selected bank object
    const bank = banks.find((b) => b.id === parseInt(newBankId));
    setSelectedBankObj(bank);

    // Reset transaction type selections
    setSelectedManualDeposit(true);
    setSelectedAutoDeposit(false);
    setSelectedManualWithdrawal(false);
  };

  const onAccountHolderNameChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const {
      target: { value },
    } = ensureNameInput(e);
    setAccountHolderName(value);
    setFieldErrors((prev) => ({
      ...prev,
      accountHolderName: value ? '' : tGlobal('pleaseFillField'),
    }));
  };

  const onAccountNumberChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const {
      target: { value },
    } = ensureIntegerInput(e);
    setAccountNumber(value);
    setFieldErrors((prev) => ({
      ...prev,
      accountNumber: value ? '' : tGlobal('pleaseFillField'),
    }));
  };

  const onCountryCodeChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const value = e.target.value;
    setCountryCode(value);
    setFieldErrors((prev) => ({
      ...prev,
      countryCode: value ? '' : tGlobal('pleaseFillField'),
    }));
  };

  const onMobileChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const value = e.target.value;
    setMobile(value);
    setFieldErrors((prev) => ({
      ...prev,
      mobile: value ? '' : tGlobal('pleaseFillField'),
    }));
  };

  return (
    <div tabIndex={-1}>
      <div className="modal-dialog">
        <div className="modal-content max-w-[1200px]">
          <div className="flex items-center justify-flex-start mb-[16px] pt-[8px] border-[#cfcfcf] border-dotted border-b-[2px]">
            <BackButtonComponent onClick={onClose} />
            <TitleComponent title={tManagePayments('bankManagement')} />
          </div>
          {isLoadingBanks ? (
            <LoadingFullComponent />
          ) : (
            <form
              onSubmit={handleSubmit}
              className="bg-[#ffffff] p-[16px] rounded-[5px] shadow-[0_1px_5px_#0003,0_2px_2px_#00000024,0_3px_1px_-2px_#0000001f]"
            >
              <div className="modal-body">
                <ErrorMessageComponent message={error} />
                <SuccessMessageComponent message={successMessage} />

                <MediumTitleComponent text={tManagePayments('bankStatus')} />
                <p className=" my-[8px]">{tManagePayments('bankNote1')}</p>
                <div className="mb-[16px]" role="group">
                  <EnableButtonComponent isActive={status === 1} onClick={() => setStatus(1)} />
                  <DisableButtonComponent isActive={status === 0} onClick={() => setStatus(0)} />
                </div>

                <MediumTitleComponent text={tManagePayments('accountInformation')} />
                <div className="mb-[16px]">
                  <LabelTitleComponent text={tManagePayments('bank')} required={true} />
                  <div className="w-[470px]">
                    <CustomDropdown
                      options={banks.map((bank) => ({
                        value: bank.id,
                        label: bank.name,
                        icon: <BankIcon code={bank.code} />,
                      }))}
                      onSelect={onBankChange}
                      disable={status === 0}
                      initialValue={selectedBank}
                    />
                  </div>
                  <ValidateMessageComponent message={fieldErrors.selectedBank} />
                </div>

                <div className="">
                  <MediumTitleComponent
                    className="mb-[8px]"
                    text={tManagePayments('transactionType')}
                    required={true}
                  />
                  <ValidateMessageComponent message={fieldErrors.selectedTransactionType} />
                  <div className="inline-block border-[1px] border-[var(--border-control)] rounded-[4px] px-[16px] pb-[16px] bg-[#f8f8f8] mb-[8px]">
                    {hasDepositTransactionType(selectedBank, banks) && (
                      <div className="mb-3">
                        <LabelTitleComponent text={tManagePayments('depositType')} />
                        <div className="flex gap-[10px]" role="group">
                          {hasManualDepositTransactionType(selectedBank, banks) && (
                            <button
                              type="button"
                              className={`block w-[230px] cursor-pointer h-[120px]  rounded-[4px] border-[1px] border-[var(--button-border)]  ${selectedManualDeposit ? 'text-[#ffffff] bg-[var(--button-background)]' : 'text-[var(--text-color-primary)] bg-[#ffffff]'}`}
                              onClick={toggleManualDeposit}
                            >
                              <span className="text-[14px] font-bold ">
                                {tManagePayments('slipUpload')}
                              </span>
                              <br />
                              <span className="">{tManagePayments('manualDeposit')}</span>
                            </button>
                          )}
                          {hasAutoDepositTransactionType(selectedBank, banks) && (
                            <button
                              type="button"
                              className={`block w-[230px] cursor-pointer h-[120px] rounded-[4px] border-[1px] border-[var(--button-border)] ${selectedAutoDeposit ? 'text-[#ffffff] bg-[var(--button-background)]' : 'text-[var(--text-color-primary)] bg-[#ffffff]'}`}
                              onClick={toggleAutoDeposit}
                            >
                              <span className="text-[14px] font-bold">
                                {selectedBankObj?.depositMatchingMode.sourceTypeName}
                              </span>
                              <br />
                              <span className="">{tManagePayments('autoDeposit')}</span>
                            </button>
                          )}
                        </div>
                      </div>
                    )}

                    {hasWithdrawalTransactionType(selectedBank, banks) && (
                      <div>
                        <LabelTitleComponent text={tManagePayments('withdrawalType')} />
                        <div className="btn-group" role="group">
                          {hasManualWithdrawalTransactionType(selectedBank, banks) && (
                            <button
                              type="button"
                              className={`block cursor-pointer w-[230px] h-[120px] rounded-[4px] border-[1px] border-[var(--button-border)] ${selectedManualWithdrawal ? 'text-[#ffffff] bg-[var(--button-background)]' : 'text-[var(--text-color-primary)] bg-[#ffffff]'}`}
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
                </div>

                {(() => {
                  const mappingTypes = selectedBankObj?.depositMatchingMode?.mappingTypes || [];
                  return (
                    selectedBank &&
                    selectedBankObj &&
                    mappingTypes.length > 0 &&
                    selectedAutoDeposit && (
                      <div className="mb-3">
                        <LabelTitleComponent text={tManagePayments('depositMatchingMode')} />
                        <div className="flex gap-[8px]" role="group">
                          {mappingTypes.includes(1) && (
                            <label
                              className="flex items-center cursor-pointer capitalize"
                              htmlFor="manualMatching1"
                              onClick={() => setDepositMatchingMode(1)}
                            >
                              <input
                                className="w-[20px] h-[20px] m-[10px] cursor-pointer"
                                id="manualMatching1"
                                type="radio"
                                name="depositMatchingMode"
                                value={depositMatchingMode}
                                checked={depositMatchingMode === 1}
                              />
                              <span className=" leading-[14px]">{tManagePayments('decimal')}</span>
                            </label>
                          )}
                          {mappingTypes.includes(2) && (
                            <label
                              className="flex items-center cursor-pointer capitalize"
                              htmlFor="manualMatching2"
                              onClick={() => setDepositMatchingMode(2)}
                            >
                              <input
                                className="w-[20px] h-[20px] m-[10px] cursor-pointer"
                                id="manualMatching2"
                                type="radio"
                                name="depositMatchingMode"
                                value={depositMatchingMode}
                                checked={depositMatchingMode === 2}
                              />
                              <span className=" leading-[14px]">
                                {tManagePayments('bankMatching')}
                              </span>
                            </label>
                          )}
                        </div>
                        <ValidateMessageComponent
                          message={fieldErrors.selectedDepositMatchingMode}
                        />
                      </div>
                    )
                  );
                })()}

                <div className="flex flex-nowrap gap-[16px] mb-[16px] max-[768px]:justify-between max-[768px]:gap-[0] max-[768px]:flex-wrap w-full">
                  <div className="w-[230px] pt-[4px] max-[768px]:w-[48%]">
                    <LabelTitleComponent
                      text={tManagePayments('accountHolderName')}
                      required={true}
                    />
                    <input
                      type="text"
                      className={`h-[32px] max-[768px]:h-[40px] w-[100%] border-[1px] rounded-[4px] px-[8px]  focus:outline-none focus:border-[var(--button-border)] cursor-pointer ${fieldErrors.accountHolderName ? 'border-[#FE0000]' : 'border-[var(--border-control)]'}`}
                      value={accountHolderName}
                      onChange={onAccountHolderNameChange}
                      maxLength={255}
                    />
                    <ValidateMessageComponent message={fieldErrors.accountHolderName} />
                    <label className="block mt-[4px] text-[#828282] text-[11px] max-[768px]:text-[15px] font-semibold">
                      {tManagePayments('provideFullName')}
                    </label>
                  </div>

                  <div className="w-[230px] pt-[4px] max-[768px]:w-[48%]">
                    <LabelTitleComponent
                      text={tManagePayments('bankAccountNumber')}
                      required={true}
                    />
                    <input
                      type="text"
                      className={`h-[32px] max-[768px]:h-[40px] w-[100%] border-[1px] rounded-[4px] px-[8px]  focus:outline-none focus:border-[var(--button-border)] cursor-pointer ${fieldErrors.accountNumber ? 'border-[#FE0000]' : 'border-[var(--border-control)]'}`}
                      value={accountNumber}
                      onChange={onAccountNumberChange}
                      maxLength={100}
                    />
                    <ValidateMessageComponent message={fieldErrors.accountNumber} />
                    <label className="block mt-[4px] text-[#828282] text-[11px] max-[768px]:text-[15px] font-semibold">
                      {tGlobal('maxLength').replace('{0}', '100')}
                    </label>
                  </div>

                  <div className="w-[230px] pt-[4px] max-[768px]:w-[48%]">
                    <LabelTitleComponent text={tManagePayments('accountNickname')} />
                    <input
                      type="text"
                      className={`h-[32px] max-[768px]:h-[40px] w-[100%] border-[1px] border-[var(--border-control)] rounded-[4px] px-[8px]  focus:outline-none focus:border-[var(--button-border)] cursor-pointer`}
                      value={accountNickname}
                      onChange={(e) => setAccountNickname(e.target.value)}
                      maxLength={255}
                    />
                    <label className="block mt-[4px] text-[#828282] text-[11px] max-[768px]:text-[15px] font-semibold">
                      {tManagePayments('accountNicknameNote1')}
                    </label>
                  </div>
                </div>

                <MediumTitleComponent text={tManagePayments('promptPay')} />
                <p className="pb-[8px] ">{tManagePayments('linkMobile')}</p>
                <div className="btn-group mb-[16px]" role="group">
                  <ButtonOnComponent isActive={promptPay} onClick={() => setPromptPay(true)} />
                  <ButtonOffComponent isActive={!promptPay} onClick={() => setPromptPay(false)} />
                </div>

                <MobileComponent
                  isShow={promptPay}
                  label={tManagePayments('mobile')}
                  countryCode={countryCode}
                  mobile={mobile}
                  fieldErrors={fieldErrors}
                  onCountryCodeChange={onCountryCodeChange}
                  onMobileChange={onMobileChange}
                  placeholder={tManagePayments('enterMobileNumber')}
                />
              </div>
              <div className="flex justify-start py-[8px] mt-[8px] gap-[8px]">
                <ButtonCancelComponent text={tGlobal('cancel')} onClick={onClose} />
                <ButtonApplyComponent
                  text={tGlobal(loading ? 'saving' : 'save')}
                  onClick={() => handleSubmit}
                  disabled={loading || isBlockAction}
                />
              </div>
            </form>
          )}
        </div>
      </div>
    </div>
  );
};

export default AddBankAccountComponent;
