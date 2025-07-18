import React, { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';

import BankIcon from '../../components/BankComponents';
import {
  ButtonBackComponent,
  ButtonComponent,
  DisableButtonComponent,
  EnableButtonComponent,
} from '../../components/Buttons/index';
import CustomDropdown, { DropdownOption } from '../../components/CustomDropdownComponent';
import { LoadingFullComponent } from '../../components/LoadingComponent';
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
import { ensureIntegerInput } from '../../utils/inputHandlers';

interface AddBankAccountComponentProps {
  onClose: () => void;
  onSave: () => void;
  isCashServiceEnabled: boolean;
}

const AddBankAccountComponent: React.FC<AddBankAccountComponentProps> = ({ onClose, onSave, isCashServiceEnabled }) => {
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
      accountHolderName: !accountHolderName.trim() ? tGlobal('pleaseFillField') : '',
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

  // Toggle handlers for transaction type buttons
  const toggleManualDeposit = () => {
    var isValid = (!selectedManualDeposit) || selectedAutoDeposit || selectedManualWithdrawal;
    setSelectedManualDeposit(!selectedManualDeposit);

    setFieldErrors({
      ...fieldErrors,
      selectedTransactionType: !isValid ? tManagePayments('pleaseSelectTransactionType') : '',
    });
  };

  const toggleAutoDeposit = () => {
    var isValid = selectedManualDeposit || !selectedAutoDeposit || selectedManualWithdrawal;
    setSelectedAutoDeposit(!selectedAutoDeposit);
    setFieldErrors({
      ...fieldErrors,
      selectedTransactionType: !isValid ? tManagePayments('pleaseSelectTransactionType') : '',
    });
  };

  const toggleManualWithdrawal = () => {
    var isValid = selectedManualDeposit || selectedAutoDeposit || !selectedManualWithdrawal;
    setSelectedManualWithdrawal(!selectedManualWithdrawal);
    setFieldErrors({
      ...fieldErrors,
      selectedTransactionType: !isValid ? tManagePayments('pleaseSelectTransactionType') : '',
    });
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
    var value = e.target.value;
    setAccountHolderName(value);
    setFieldErrors((prev) => ({
      ...prev,
      accountHolderName: value.trim() ? '' : tGlobal('pleaseFillField'),
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
      <div className="modal-content max-w-[1200px]">
        <div className="flex items-center justify-flex-start mb-[8px] py-[8px] border-[var(--border-control)] border-b-[1px] [&>:nth-child(2)]:border-0 [&>:nth-child(2)]:mb-[0] [&>:nth-child(1)]:mb-[0]">
          <ButtonBackComponent onClick={onClose} />
          <TitleComponent title={tManagePayments('bankManagement')} />
        </div>
        {isLoadingBanks ? (<LoadingFullComponent />) : (
          <form
            onSubmit={handleSubmit}
            className="pb-[16px]"
          >
            <div className="modal-body">
              <ErrorMessageComponent className='mb-[16px] max-[768px]:mb-[0]' message={error} />
              <SuccessMessageComponent className='mb-[16px] max-[768px]:mb-[0]' message={successMessage} />

              <MediumTitleComponent text={tManagePayments('bankStatus')} />
              <p className=" my-[8px]">{tManagePayments('bankNote1')}</p>
              <div className="mb-[16px] inline-block border-[1px] border-[var(--border-control)] rounded-[5px]" role="group">
                <EnableButtonComponent type="button" isActive={status === 1} onClick={() => setStatus(1)} />
                <DisableButtonComponent type="button" isActive={status === 0} onClick={() => setStatus(0)} />
              </div>
              <MediumTitleComponent text={tManagePayments('accountInformation')} />
              <div className="mb-[16px]">
                <LabelTitleComponent text={tManagePayments('bank')} required={true} />
                <div className="w-[470px]">
                  <CustomDropdown
                    options={banks.map((bank) => ({
                      value: bank.id,
                      label: tGlobal(bank.code),
                      icon: <BankIcon code={bank.code} />,
                    }))}
                    onSelect={onBankChange}
                    initialValue={selectedBank}
                  />
                </div>
                <ValidateMessageComponent message={fieldErrors.selectedBank} />
              </div>
              <div>
                <MediumTitleComponent
                  className="mb-[8px]"
                  text={tManagePayments('transactionType')}
                  required={true}
                />
                <div className="inline-block border-[1px] border-[var(--border-control)] rounded-[4px] px-[10px] pt-[5px] pb-[10px] bg-[#f8f8f8] dark:bg-[#2b2b2b] mb-[8px]">
                  {hasDepositTransactionType(selectedBank, banks) && (
                    <div className="mb-3">
                      <LabelTitleComponent text={tManagePayments('depositType')} />
                      <div className="flex gap-[10px]" role="group">
                        {hasManualDepositTransactionType(selectedBank, banks) && (
                          <button
                            type="button"
                            className={`transaction-box ${selectedManualDeposit ? 'select' : ''}`}
                            onClick={toggleManualDeposit}
                          >
                            <span className='check-box'></span>
                            <span className="text-[110%] font-bold ">
                              {tManagePayments('slipUpload')}
                            </span>
                            <br />
                            <span className="">{tManagePayments('manualDeposit')}</span>
                          </button>
                        )}
                        {hasAutoDepositTransactionType(selectedBank, banks) && (
                          <button
                            type="button"
                            className={`transaction-box ${selectedAutoDeposit ? 'select' : ''}`}
                            onClick={toggleAutoDeposit}
                          >
                            <span className='check-box'></span>
                            <span className="text-[110%] font-bold">
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
                            className={`transaction-box ${selectedManualWithdrawal ? 'select' : ''}`}
                            onClick={toggleManualWithdrawal}
                          >
                            <span className='check-box'></span>
                            <span className="text-[110%] font-bold">
                              {tManagePayments('manualWithdrawal')}
                            </span>
                          </button>
                        )}
                      </div>
                    </div>
                  )}
                </div>
                <ValidateMessageComponent message={fieldErrors.selectedTransactionType} />
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
                      <ValidateMessageComponent message={fieldErrors.selectedDepositMatchingMode} />
                    </div>
                  )
                );
              })()}

              <div className="flex flex-nowrap gap-[16px] mb-[16px] max-[768px]:justify-between max-[768px]:gap-[0] max-[768px]:flex-wrap w-full">
                <div className="w-[240px] pt-[4px] max-[768px]:w-[48%]">
                  <LabelTitleComponent text={tManagePayments('accountHolderName')} required={true} />
                  <input
                    type="text"
                    className={`control-field ${fieldErrors.accountHolderName ? 'error' : ''}`}
                    value={accountHolderName}
                    onChange={onAccountHolderNameChange}
                    maxLength={255}
                  />
                  <ValidateMessageComponent message={fieldErrors.accountHolderName} />
                  <label className="block mt-[4px] text-[#828282] text-[90%]">
                    {tManagePayments('provideFullName')}
                  </label>
                </div>

                <div className="w-[240px] pt-[4px] max-[768px]:w-[48%]">
                  <LabelTitleComponent text={tManagePayments('bankAccountNumber')} required={true} />
                  <input
                    type="text"
                    className={`control-field ${fieldErrors.accountNumber ? 'error' : ''}`}
                    value={accountNumber}
                    onChange={onAccountNumberChange}
                    maxLength={100}
                  />
                  <ValidateMessageComponent message={fieldErrors.accountNumber} />
                  <label className="block mt-[4px] text-[#828282] text-[90%]">
                    {tGlobal('maxLength').replace('{0}', '100')}
                  </label>
                </div>

                <div className="w-[240px] pt-[4px] max-[768px]:w-[48%]">
                  <LabelTitleComponent text={tManagePayments('accountNickname')} />
                  <input
                    type="text"
                    className={`control-field`}
                    value={accountNickname}
                    onChange={(e) => setAccountNickname(e.target.value)}
                    maxLength={255}
                  />
                  <label className="block mt-[4px] text-[#828282] text-[90%]">
                    {tManagePayments('accountNicknameNote1')}
                  </label>
                </div>
              </div>

              <MediumTitleComponent text={tManagePayments('promptPay')} />
              <p className="pb-[8px] ">{tManagePayments('linkMobile')}</p>
              <div className="mb-[10px] inline-block border-[1px] border-[var(--border-control)] rounded-[5px]" role="group">
                <EnableButtonComponent text={tGlobal('on')} type="button" isActive={promptPay} onClick={() => setPromptPay(true)} />
                <DisableButtonComponent text={tGlobal('off')} type="button" isActive={!promptPay} onClick={() => setPromptPay(false)} />
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
              <ButtonComponent variant="cancel" text={tGlobal('cancel')}  onClick={onClose} type='button' />
              <ButtonComponent
                variant="apply"
                text={tGlobal(loading ? 'saving' : 'save')}
                type='submit'
                onClick={() => handleSubmit}
                disabled={!isCashServiceEnabled || loading || isBlockAction}
                title={isCashServiceEnabled ? '' : tGlobal('cashServiceDisabled')}
              />
            </div>
          </form>
        )}
      </div>     
    </div>
  );
};

export default AddBankAccountComponent;
