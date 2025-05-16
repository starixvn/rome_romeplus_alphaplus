import React, { useState, useEffect } from 'react';
import { useTranslation } from 'react-i18next';
import { Bank } from '../../services/interfaces/Payments/Bank';
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
import CustomDropdown, { DropdownOption } from '../../components/DropdownComponent';
import Loading from '../../components/LoadingComponent';
import BackComponent from '../../components/BackComponent';
import { ResultMessageComponent } from '../../components/ResultMessageComponent';
import CountryCodeComponent from '../../components/CountryCodeComponent';

interface AddBankAccountComponentProps {
  onClose: () => void;
  onSave: () => void;
}

const AddBankAccountComponent: React.FC<AddBankAccountComponentProps> = ({
  onClose,
  onSave
}) => {
  const { t: tManagePayments } = useTranslation('managePayments');
  const { t: tGlobal } = useTranslation('global');
  
  const [accountNickname, setAccountNickname] = useState<string>('');
  const [accountHolderName, setAccountHolderName] = useState<string>('');
  const [accountNumber, setAccountNumber] = useState<string>('');
  const [selectedBank, setSelectedBank] = useState<string>('');
  const [status, setStatus] = useState<string>('1'); // Default to active
  const [depositMatchingMode, setDepositMatchingMode] = useState<number>(0);
  const [promptPay, setPromptPay] = useState<boolean>(false);
  const [mobile, setMobile] = useState<string>('');
  const [countryCode, setCountryCode] = useState<string>('');
  const [countryCodes, setCountryCodes] = useState<string[]>([]);
  const [error, setError] = useState<string | null>(null);
  const [success, setSuccess] = useState<boolean>(false);
  const [successMessage, setSuccessMessage] = useState<string>('');
  const [loading, setLoading] = useState<boolean>(false);
  const [banks, setBanks] = useState<Bank[]>([]);
  const [selectedBankObj, setSelectedBankObj] = useState<Bank | undefined>(undefined);
  const [isLoadingBanks, setIsLoadingBanks] = useState<boolean>(true);
  
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
    selectedDepositMatchingMode: ''
  });

  useEffect(() => {
    const fetchBanks = async () => {
      setIsLoadingBanks(true);
      try {
        const fetchedBanks = await paymentProviderService.getBanksByTransactionType(0);
        setBanks(fetchedBanks);
        
        // Set first bank data if available
        if (fetchedBanks.length > 0) {
          const firstBank = fetchedBanks[0];
          setSelectedBank(firstBank.id.toString());
          setSelectedBankObj(firstBank);
          
          // Set transaction types based on first bank's capabilities
          setSelectedManualDeposit(true);
          setSelectedAutoDeposit(false);
          setSelectedManualWithdrawal(false);
          
          // Set deposit matching mode based on bank's capabilities
          if (firstBank.depositMatchingMode.mappingTypes.length > 0) {
            setDepositMatchingMode(0);
          }
        }
      } catch (err) {
        console.error('Error fetching banks:', err);
        setError(tManagePayments('failedToFetchBanks'));
      } finally {
        setIsLoadingBanks(false);
      }
    };

    const fetchCountryCodes = async () => {
      try {
        const prefixes = await baseAPI.getCountryCodes();
        setCountryCodes(prefixes);
        if (prefixes.length > 0) {
          setCountryCode(prefixes[0]);
        }
      } catch (err) {
        console.error('Error fetching mobile prefixes:', err);
      }
    };

    fetchBanks();
    fetchCountryCodes();
  }, [tManagePayments]);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError(null);
    setSuccess(false);
    setLoading(true);

    // Field-level validation
    const errors = {
      accountHolderName: !accountHolderName ? tGlobal('pleaseFillField') : '',
      accountNumber: !accountNumber ? tGlobal('pleaseFillField') : '',
      selectedBank: !selectedBank ? tGlobal('pleaseFillField') : '',
      mobile: promptPay && !mobile ? tGlobal('pleaseFillField') : '',
      countryCode: promptPay && !countryCode ? tGlobal('pleaseFillField') : '',
      selectedTransactionType: !selectedManualDeposit && !selectedAutoDeposit && !selectedManualWithdrawal ? tManagePayments('pleaseSelectTransactionType') : '',
      selectedDepositMatchingMode: selectedAutoDeposit && !depositMatchingMode ? tManagePayments('pleaseSelectDepositMatchingMode') : ''
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

      const result = await paymentProviderService.addBankAccount({
        id: 0, // Temporary ID for new bank account
        accountNickname,
        accountHolderName,
        accountNumber,
        bankId: parseInt(selectedBank),
        bankName: banks.find(b => b.id === parseInt(selectedBank))?.name || '',
        status: parseInt(status),
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
          sourceTypeName: ''
        }
      });

      if (result.isSuccessful) {
        setSuccess(true);
        setSuccessMessage(result.message || tManagePayments('bankAccountAdded'));
        setTimeout(() => {
          onSave();
        }, 3000);
      } else {
        setError(result.message || tManagePayments('failedToAddBankAccount'));
      }
    } catch (err) {
      setError(tManagePayments('failedToAddBankAccount'));
      console.error('Error adding bank account:', err);
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

  // Handler for bank selection changes
  const handleBankChange = (option: DropdownOption) => {
    const newBankId = option.value.toString();
    setSelectedBank(newBankId);
    
    // Find and set the selected bank object
    const bank = banks.find(b => b.id === parseInt(newBankId));
    setSelectedBankObj(bank);
    
    // Reset transaction type selections
    setSelectedManualDeposit(true);
    setSelectedAutoDeposit(false);
    setSelectedManualWithdrawal(false);
  };

 
  return (
    <div className="p-[12px]" tabIndex={-1}>
      <div className="modal-dialog">
        <div className="modal-content max-w-[1200px]">
          <BackComponent onClose={onClose} />
          <div className="mb-[8px]">
            <h2 className="flex items-center font-bold text-[15px] ">{tManagePayments('bankManagement')}</h2>            
          </div>
          {isLoadingBanks 
            ? ( <Loading /> ) 
            : (
            <form onSubmit={handleSubmit} className='bg-[#ffffff] p-[16px] rounded-[5px] shadow-[0_1px_5px_#0003,0_2px_2px_#00000024,0_3px_1px_-2px_#0000001f]'>
              <div className="modal-body">
                {error && (<ResultMessageComponent message={error} type='error' />)}
                {success && (<ResultMessageComponent message={successMessage} type='success' />)}                
                <div className="mb-3">
                  <div className="pb-[8px] text-[16px] text-[var(--text-color-primary)] font-semibold leading-[21.6px] tracking-[0.2px]">{tManagePayments('bankStatus')}</div>
                  <p className="text-[12px] my-[8px]">{tManagePayments('bankNote1')}</p>
                  <div className="btn-group" role="group">
                    <button
                      type="button"
                      className={`text-[12px] w-[100px] px-[16px] py-[8px] cursor-pointer rounded-tl-[4px] rounded-bl-[4px] ${status === '1' ? 'bg-[var(--button-switch-toggle-background-active)] text-[var(--button-switch-toggle-background-color-active)]' : 'bg-[var(--button-switch-toggle-background)] text-[var(--button-switch-toggle-background-color)]'}`}
                      onClick={() => setStatus('1')}
                    >
                      {tGlobal('enable')}
                    </button>
                    <button
                      type="button"
                      className={`text-[12px] w-[100px] px-[16px] py-[8px] cursor-pointer rounded-tr-[4px] rounded-br-[4px] ${status === '2' ? 'bg-[var(--button-switch-toggle-background-active)] text-[var(--button-switch-toggle-background-color-active)]' : 'bg-[var(--button-switch-toggle-background)] text-[var(--button-switch-toggle-background-color)]'}`}
                      onClick={() => setStatus('2')}
                    >
                      {tGlobal('disable')}
                    </button>
                  </div>
                </div>

                <div className="mt-[8px] text-[16px] text-[var(--text-color-primary)] font-semibold leading-[21.6px] tracking-[0.2px]">{tManagePayments('accountInformation')}</div>
                
                
                <div className='mb-[16px]'>
                  <label className="block text-[12px] font-semibold text-[#333333] my-[8px]">{tManagePayments('bank')}<span className='text-[#E25757]'>*</span></label>
                  <div className='w-[470px]'>
                    <CustomDropdown
                      options={banks.map(bank => ({
                        value: bank.id,
                        label: bank.name,
                        code: bank.code
                      }))}
                      onSelect={handleBankChange}
                      disable={status === '0'}
                    />
                  </div>
                  {fieldErrors.selectedBank && (<div className="text-[#FE0000] text-[11px] mt-1">{fieldErrors.selectedBank}</div>)}
                </div>

                <div className=''>
                  <div className="pb-[8px] text-[16px] text-[var(--text-color-primary)] font-semibold leading-[21.6px]">{tManagePayments('transactionType')}<span className='text-[#E25757]'>*</span></div>
                  {fieldErrors.selectedTransactionType && (<div className="text-[#FE0000] text-[11px] mt-1">{fieldErrors.selectedTransactionType}</div> )}
                    
                  <div className='inline-block border-[1px] border-[var(--border-control)] rounded-[4px] px-[16px] pb-[16px] bg-[#f8f8f8] mb-[8px]'>
                  {hasDepositTransactionType(selectedBank, banks) && (
                    <div className="mb-3">
                      <label className="block text-[12px] font-semibold text-[#333333] my-[8px]">{tManagePayments('depositType')}</label>
                      <div className="flex gap-[10px]" role="group">
                        {hasManualDepositTransactionType(selectedBank, banks) && (
                          <button
                            type="button"
                            className={`block w-[230px] cursor-pointer h-[120px]  rounded-[4px] border-[1px] border-[var(--button-border)]  ${selectedManualDeposit ? 'text-[#ffffff] bg-[var(--button-background)]' : 'text-[var(--text-color-primary)] bg-[#ffffff]'}`}
                            onClick={toggleManualDeposit}
                          >                     
                            <span className="text-[14px] font-bold ">{tManagePayments('slipUpload')}</span>
                            <br />
                            <span className="text-[12px]">{tManagePayments('manualDeposit')}</span>
                          </button>
                        )}
                        {hasAutoDepositTransactionType(selectedBank, banks) && (
                          <button
                            type="button"
                            className={`block w-[230px] cursor-pointer h-[120px] rounded-[4px] border-[1px] border-[var(--button-border)] ${selectedAutoDeposit ? 'text-[#ffffff] bg-[var(--button-background)]' : 'text-[var(--text-color-primary)] bg-[#ffffff]'}`}
                            onClick={toggleAutoDeposit}
                          >
                            <span className="text-[14px] font-bold">{selectedBankObj?.depositMatchingMode.sourceTypeName}</span>
                            <br />
                            <span className="text-[12px]">{tManagePayments('autoDeposit')}</span>
                          </button>
                        )}
                      </div>
                    </div>
                  )}

                  {hasWithdrawalTransactionType(selectedBank, banks) && (
                    <div>
                      <label className="block text-[12px] font-semibold text-[#333333] my-[8px]">{tManagePayments('withdrawalType')}</label>
                      <div className="btn-group" role="group">
                        {hasManualWithdrawalTransactionType(selectedBank, banks) && (
                          <button
                            type="button"
                            className={`block cursor-pointer w-[230px] h-[120px] rounded-[4px] border-[1px] border-[var(--button-border)] ${selectedManualWithdrawal ? 'text-[#ffffff] bg-[var(--button-background)]' : 'text-[var(--text-color-primary)] bg-[#ffffff]'}`}
                            onClick={toggleManualWithdrawal}
                          >
                            <span className="text-[14px] font-bold">{tManagePayments('manualWithdrawal')}</span>
                          </button>
                        )}
                      </div>
                    </div>
                  )}
                  </div>
                </div>

                {(() => {
                  return selectedBank && selectedBankObj && selectedBankObj.depositMatchingMode?.mappingTypes?.length > 0 && selectedAutoDeposit && (
                    <div className="mb-3">
                      <label className="block text-[12px] font-semibold text-[#333333] my-[8px]">{tManagePayments('depositMatchingMode')}</label>
                      <div className="flex gap-[8px]" role="group">
                        {selectedBankObj.depositMatchingMode.mappingTypes.includes(1) && (   
                          <label className='flex items-center cursor-pointer capitalize' htmlFor="manualMatching1" onClick={() => setDepositMatchingMode(1)}>
                            <input className='w-[20px] h-[20px] m-[10px] cursor-pointer' id="manualMatching1" type="radio" name="depositMatchingMode" value={depositMatchingMode} checked={depositMatchingMode === 1} />
                            <span className='text-[12px] leading-[14px]'>{tManagePayments('decimal')}</span>
                          </label>
                        )}
                        {selectedBankObj.depositMatchingMode.mappingTypes.includes(2) && (                        
                          <label className='flex items-center cursor-pointer capitalize' htmlFor="manualMatching2" onClick={() => setDepositMatchingMode(2)}>
                            <input className='w-[20px] h-[20px] m-[10px] cursor-pointer' id="manualMatching2" type="radio" name="depositMatchingMode" value={depositMatchingMode} checked={depositMatchingMode === 2} />
                            <span className='text-[12px] leading-[14px]'>{tManagePayments('bankMatching')}</span>
                          </label>
                        )}
                      </div>
                      {fieldErrors.selectedDepositMatchingMode && (
                        <div className="text-[#FE0000] text-[11px] mt-1">{fieldErrors.selectedDepositMatchingMode}</div>
                      )}
                    </div>
                  );
                })()}

                <div className="flex gap-[16px]">
                  <div className='w-[230px]'>
                    <label className="block text-[12px] font-semibold text-[#333333] my-[8px]">{tManagePayments('accountHolderName')}<span className='text-[#E25757]'>*</span></label>                
                    <input
                      type="text"
                      className={`h-[32px] max-[768px]:h-[40px] w-[100%] border-[1px] rounded-[4px] px-[8px] text-[12px] focus:outline-none focus:border-[var(--button-border)] cursor-pointer ${fieldErrors.accountHolderName ? 'border-[#FE0000]' : 'border-[var(--border-control)]'}`}
                      value={accountHolderName}
                      onChange={(e) => setAccountHolderName(e.target.value)}
                      maxLength={255}
                    />
                    {fieldErrors.accountHolderName && (
                      <div className="text-[#FE0000] text-[11px] mt-1">{fieldErrors.accountHolderName}</div>
                    )}
                    <label className="block mt-[4px] text-[#828282] text-[11px] leading-[12px] font-semibold">{tManagePayments('provideFullName')}</label>
                  </div>

                  <div className='w-[230px]'>
                    <label className="block text-[12px] font-semibold text-[#333333] my-[8px]">{tManagePayments('bankAccountNumber')}<span className='text-[#E25757]'>*</span></label>
                    <input
                      type="text"
                      className={`h-[32px] max-[768px]:h-[40px] w-[100%] border-[1px] rounded-[4px] px-[8px] text-[12px] focus:outline-none focus:border-[var(--button-border)] cursor-pointer ${fieldErrors.accountNumber ? 'border-[#FE0000]' : 'border-[var(--border-control)]'}`}
                      value={accountNumber}
                      onChange={(e) => setAccountNumber(e.target.value)}
                      maxLength={50}
                    />
                    {fieldErrors.accountNumber && (
                      <div className="text-[#FE0000] text-[11px] mt-1">{fieldErrors.accountNumber}</div>
                    )}
                  </div>

                  <div className='w-[230px]'>
                    <label className="block text-[12px] font-semibold text-[#333333] my-[8px]">{tManagePayments('accountNickname')}</label>
                    <input
                      type="text"
                      className={`h-[32px] max-[768px]:h-[40px] w-[100%] border-[1px] rounded-[4px] px-[8px] text-[12px] focus:outline-none focus:border-[var(--button-border)] cursor-pointer`}
                      value={accountNickname}
                      onChange={(e) => setAccountNickname(e.target.value)}
                      maxLength={255}
                    />
                    <label className="block mt-[4px] text-[#828282] text-[11px] leading-[12px] font-semibold">{tManagePayments('accountNicknameNote1')}</label>
                  </div>
                </div>

                <div className="mb-3">
                  <div className="pb-[8px] mt-[8px] text-[16px] text-[var(--text-color-primary)] font-semibold leading-[21.6px] tracking-[0.2px]">{tManagePayments('promptPay')}</div>
                  <p className="pb-[8px] text-[12px]">{tManagePayments('linkMobile')}</p>
                  <div className="btn-group" role="group">
                    <button
                      type="button"
                      className={`text-[12px] w-[70px] px-[16px] py-[8px] cursor-pointer rounded-tl-[4px] rounded-bl-[4px] ${promptPay ? 'bg-[var(--button-switch-toggle-background-active)] text-[var(--button-switch-toggle-background-color-active)]' : 'bg-[var(--button-switch-toggle-background)] text-[var(--button-switch-toggle-background-color)]'}`}
                      onClick={() => setPromptPay(true)}
                    >
                      {tGlobal('on')}
                    </button>
                    <button
                      type="button"
                      className={`text-[12px] w-[70px] px-[16px] py-[8px] cursor-pointer rounded-tr-[4px] rounded-br-[4px] ${!promptPay ? 'bg-[var(--button-switch-toggle-background-active)] text-[var(--button-switch-toggle-background-color-active)]' : 'bg-[var(--button-switch-toggle-background)] text-[var(--button-switch-toggle-background-color)]'}`}
                      onClick={() => setPromptPay(false)}
                    >
                      {tGlobal('off')}
                    </button>
                  </div>
                </div>

                {promptPay && (
                  <CountryCodeComponent
                    label={tManagePayments('mobile')}
                    countryCode={countryCode}
                    mobile={mobile}
                    countryCodes={countryCodes}
                    fieldErrors={fieldErrors}
                    onCountryCodeChange={(e) => setCountryCode(e.target.value)}
                    onMobileChange={(e) => setMobile(e.target.value)}
                    placeholder={tManagePayments('enterMobileNumber')}
                  />
                )}
              </div>
              <div className="flex justify-end py-[8px] mt-[8px]">            
                <button
                  type="button"
                  className="w-[140px] h-[30px] rounded-[20px] text-[var(--button-text-cancel)] px-[16px] py-[4px] text-[12px] border-[1px] border-[var(--button-border)]  bg-[var(--button-background-cancel)] hover:bg-[var(--button-background-cancel-hover)] cursor-pointer ml-[8px]"
                  onClick={onClose}
                  disabled={loading}
                >
                  {tGlobal('cancel')}
                </button>
                <button
                  type="submit"
                  className="w-[140px] h-[30px] rounded-[20px] text-[var(--button-text)] px-[16px] py-[4px] text-[12px] border-[1px] border-[var(--button-border)] ml-[8px] bg-[var(--button-background)] hover:bg-[var(--button-background-hover)] hover:border-[var(--button-background-hover)] cursor-pointer  shadow-[0_1px_5px_#0003,0_2px_2px_#00000024,0_3px_1px_-2px_#0000001f]"
                  disabled={loading}
                >
                  {loading ? tGlobal('saving') : tGlobal('save')}
                </button>
              </div>
            </form>
          )}
        </div>
      </div>
    </div>
  );
};

export default AddBankAccountComponent; 