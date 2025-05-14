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
import { IconCheck, IconError } from '../../components/Icons';
import { baseAPI } from '../../services/commonAPI';
import CustomDropdown, { DropdownOption } from '../../components/ui/customDropdown';
import Loading from '../../components/ui/Loading';
import BackCommon from '../../components/ui/BackComponents';

interface UpdateBankAccountComponentProps {
  onClose: () => void;
  onSave: () => void;
  bankAccount: BankAccount;
}

const UpdateBankAccountComponent: React.FC<UpdateBankAccountComponentProps> = ({
  onClose,
  onSave,
  bankAccount
}) => {
  const { t: tManagePayments } = useTranslation('managePayments');
  const { t: tGlobal } = useTranslation('global');
  
  const [accountNickname, setAccountNickname] = useState<string>(bankAccount.accountNickname);
  const [accountHolderName] = useState<string>(bankAccount.accountHolderName);
  const [accountNumber] = useState<string>(bankAccount.accountNumber);
  const [selectedBank, setSelectedBank] = useState<string>(bankAccount.bankId.toString());
  const [status, setStatus] = useState<string>(bankAccount.status.toString());
  const [promptPay, setPromptPay] = useState<boolean>(bankAccount.promptPay);
  const [mobile, setMobile] = useState<string>(bankAccount.mobile);
  const [countryCode, setCountryCode] = useState<string>(bankAccount.countryCode || '');
  const [creator, setCreator] = useState<string>(bankAccount.creator || '');
  const [countryCodes, setCountryCodes] = useState<string[]>([]);
  const [error, setError] = useState<string | null>(null);
  const [success, setSuccess] = useState<boolean>(false);
  const [successMessage, setSuccessMessage] = useState<string>('');
  const [loading, setLoading] = useState<boolean>(false);
  const [initialLoading, setInitialLoading] = useState<boolean>(true);
  const [banks, setBanks] = useState<Bank[]>([]);

  const [bankDepositMatchingMode, setBankDepositMatchingMode] = useState<DepositMatchingMode>(bankAccount.bankDepositMatchingMode);
  const [bank_TransTypeList, setBank_TransTypeList] = useState<string>(bankAccount.bank_TransTypeList);
  const [agent_DepositMatchingMode, setAgent_DepositMatchingMode] = useState<number>(bankAccount.agent_DepositMatchingMode);

  // Add state variables for selected transaction types
  const [selectedManualDeposit, setSelectedManualDeposit] = useState<boolean>(false);
  const [selectedAutoDeposit, setSelectedAutoDeposit] = useState<boolean>(false);
  const [selectedManualWithdrawal, setSelectedManualWithdrawal] = useState<boolean>(false);

  const [fieldErrors, setFieldErrors] = useState({
    mobile: '',
    countryCode: '',
    selectedDepositType: '',
    selectedDepositMatchingMode: ''
  });

  useEffect(() => {
    const fetchBanks = async () => {
      try {
        const fetchedBanks = await paymentProviderService.getBanksByTransactionType(0);
        setBanks(fetchedBanks);
      } catch (err) {
        console.error('Error fetching banks:', err);
        setError(tManagePayments('failedToFetchBanks'));
      }
    };

    const fetchBankAccount = async () => {
      try {
        setLoading(true);
        const fetchedBankAccount = await paymentProviderService.getBankAccountById(bankAccount.id);
        if (fetchedBankAccount) {
          setAccountNickname(fetchedBankAccount.accountNickname);
          setStatus(fetchedBankAccount.status.toString());
          setPromptPay(fetchedBankAccount.promptPay);
          setSelectedBank(fetchedBankAccount.bankId.toString());
          
          setBankDepositMatchingMode(fetchedBankAccount.bankDepositMatchingMode);
          setBank_TransTypeList(fetchedBankAccount.bank_TransTypeList);
          setAgent_DepositMatchingMode(fetchedBankAccount.agent_DepositMatchingMode);

          var transTypeList = fetchedBankAccount.agent_TransTypeList.split(',');
          setSelectedManualDeposit(transTypeList.includes(BANK_TRANSACTION_TYPES[1].id.toString()));
          setSelectedAutoDeposit(transTypeList.includes(BANK_TRANSACTION_TYPES[2].id.toString()));
          setSelectedManualWithdrawal(transTypeList.includes(BANK_TRANSACTION_TYPES[3].id.toString()));

          setCreator(fetchedBankAccount.creator);

          var m = fetchedBankAccount.mobile;
          var countryCode = m.includes('-') ?  fetchedBankAccount.mobile.split('-')[0] : '';
          var mobile = m.includes('-') ? fetchedBankAccount.mobile.split('-')[1] : m;
          setMobile(mobile);
          setCountryCode(countryCode);
        }
      } catch (err) {
        setError(tManagePayments('failedToLoadBankAccount'));
        console.error('Error fetching bank account:', err);
      } finally {
        setLoading(false);
      }
    };

    const fetchCountryCodes = async () => {
      try {
        const prefixes = await baseAPI.getCountryCodes();
        setCountryCodes(prefixes);
      } catch (err) {
        console.error('Error fetching mobile prefixes:', err);
      }
    };

    Promise.all([fetchBanks(), fetchBankAccount(), fetchCountryCodes()]).finally(() => {
      setInitialLoading(false);
    });
  }, [bankAccount.id, tManagePayments]);

  if (initialLoading) {
    return <Loading  />;
  }

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError(null);
    setSuccess(false);
    setLoading(true);

    // Field-level validation
    const errors = {
      mobile: !mobile ? tGlobal('pleaseFillField') : '',
      countryCode: !countryCode ? tGlobal('pleaseFillField') : '',
      selectedDepositType: !selectedManualDeposit && !selectedAutoDeposit ? tManagePayments('pleaseSelectDepositType') : '',
      selectedDepositMatchingMode: selectedAutoDeposit && !agent_DepositMatchingMode ? tManagePayments('pleaseSelectDepositMatchingMode') : ''
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

      const result = await paymentProviderService.updateBankAccount({
        id: bankAccount.id,
        accountNickname,
        accountHolderName,
        accountNumber,
        bankId: parseInt(selectedBank),
        bankName: banks.find(b => b.id === parseInt(selectedBank))?.name || '',
        status: parseInt(status),
        promptPay,
        mobile,
        countryCode,
        lastUpdated: new Date().toISOString(),
        lastUpdatedStr: new Date().toLocaleString(),
        code: '',
        creator: '',
        transactionTypes: '',
        depositMatchingMode: 0,
        bank_TransTypeList: bank_TransTypeList,
        agent_TransTypeList: transactionTypesString,
        agent_DepositMatchingMode: agent_DepositMatchingMode,
        bankDepositMatchingMode: bankDepositMatchingMode
      });

      if (result.isSuccessful) {
        window.scrollTo({ top: 0, behavior: 'smooth' });
        setSuccess(true);
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
    setSelectedManualDeposit(!selectedManualDeposit);
  };

  const toggleAutoDeposit = () => {
    setSelectedAutoDeposit(!selectedAutoDeposit);
  };

  const toggleManualWithdrawal = () => {
    setSelectedManualWithdrawal(!selectedManualWithdrawal);
  };

  const handleBankChange = (option: DropdownOption) => {
    setSelectedBank(option.value.toString());
  };

  return (
    <div className="p-[12px]" tabIndex={-1}>
      <BackCommon onClose={onClose} />
      <div className="modal-dialog">
        <div className="modal-content max-w-[1200px]">
          <div className="mb-[8px]">
            <h2 className="flex items-center font-bold text-[15px] text-[#333333]">{tManagePayments('bankManagement')}</h2>
          </div>        
          <div className={ `bg-[#ffffff] p-[16px] rounded-[5px] shadow-[0_1px_5px_#0003,0_2px_2px_#00000024,0_3px_1px_-2px_#0000001f] mb-[20px] ${bankAccount.status === 1 ? 'border-l-[12px] border-[#5cb774]' : 'bg-[#D4E7D9]'}`}>
            <div className='flex mb-[12px]'>
              <label className='font-semibold text-[16px] mr-[5px] text-[var(--text-color-primary)]'>{bankAccount.accountNickname}</label>
              <div>
                <span className={`font-semibold text-[12px]  rounded-[20px] px-[10px] py-[3px] uppercase ${bankAccount.status === 1 ? 'bg-[#D4E7D9]' : 'bg-[#FBD8D8]'}`}>
                                {bankAccount.status === 1 ? tGlobal('enabled') : tGlobal('disabled')}
                </span>
              </div>
            </div>             
             
            <div className='flex text-[12px] text-[#333333] font-semibold'>
              <div className='flex flex-col mr-[32px]'>
                <label>{tManagePayments('createdBy')}</label>
                <label>{creator}</label>
              </div>
              <div className='flex flex-col'>
                <label>{tManagePayments('lastUpdated')}</label>
                <label>{bankAccount.lastUpdatedStr}</label>                
              </div>
            </div>   
          </div>


          <form onSubmit={handleSubmit} className='bg-[#ffffff] p-[16px] rounded-[5px] shadow-[0_1px_5px_#0003,0_2px_2px_#00000024,0_3px_1px_-2px_#0000001f]'>
            <div className="modal-body">
              {error && (
                <div className="mt-[16px] font-bold text-[12px] text-[#333] bg-[#FFF2F2] border-[1px] border-[#FE0000] rounded-[10px] pl-[36px] pr-[12px] py-[8px] relative block capitalize"><IconError className='w-[20px] h-[20px] fill-[#fe0000] absolute top-[8px] left-[10px]' />{error}</div>
              )}
              {success && (
                <div className="mt-[16px] font-bold text-[12px] text-[#333] bg-[#E2FFF2] border-[1px] border-[#5cb774] rounded-[10px] pl-[36px] pr-[12px] py-[8px] relative block capitalize"><IconCheck className='w-[20px] h-[20px] fill-[#5CB774] absolute top-[8px] left-[10px]' />{successMessage}</div>
              )}
              
              <div className="mb-[8px]">
                <div className='pb-[8px] mt-[8px] text-[16px] text-[var(--text-color-primary)] font-semibold leading-[21.6px] tracking-[0.2px]' >{tManagePayments('bankStatus')}</div>
                <p className="pb-[8px] text-[12px]">{tManagePayments('bankNote1')}</p>
                <div className="btn-group" role="group">
                  <button
                    type="button"
                    className={`text-[12px] w-[100px] px-[16px] py-[8px] cursor-pointer rounded-tl-[4px] rounded-bl-[4px] ${status === '1' ? 'bg-[var(--button-switch-toggle-background-active)] text-[var(--button-switch-toggle-background-color-active)]' : 'bg-[var(--button-switch-toggle-background)] text-[var(--button-switch-toggle-background-color)]'}`}                    onClick={() => setStatus('1')}
                  >
                    {tGlobal('enable')}
                  </button>
                  <button
                    type="button"
                    className={`text-[12px] w-[100px] px-[16px] py-[8px] cursor-pointer rounded-tr-[4px] rounded-br-[4px] ${status === '0' ? 'bg-[var(--button-switch-toggle-background-active)] text-[var(--button-switch-toggle-background-color-active)]' : 'bg-[var(--button-switch-toggle-background)] text-[var(--button-switch-toggle-background-color)]'}`}
                    onClick={() => setStatus('0')}
                  >
                    {tGlobal('disable')}
                  </button>
                </div>
              </div>

              <div className="mt-[8px] text-[16px] text-[var(--text-color-primary)] font-semibold leading-[21.6px] tracking-[0.2px]">{tManagePayments('accountInformation')}</div>
              <div className="mb-[8px]">
                <label className="block text-[12px] font-semibold text-[#333333] my-[8px]">{tManagePayments('bank')}</label>
                 
                <div className='w-[470px]'>
                  <CustomDropdown
                    options={banks.map(bank => ({
                      value: bank.id,
                      label: bank.name,
                      code: bank.code
                    }))}
                    onSelect={handleBankChange}
                    disable={status === '1'}
                    initialValue={parseInt(selectedBank)}
                  />
                </div>
              </div>

              {hasDepositTransactionType(selectedBank, banks) && (
                <div className="mb-[8px]">
                  <label className="block text-[12px] font-semibold text-[#333333] my-[8px]">{tManagePayments('depositType')}<span className='text-[#E25757]'>*</span></label>
                  <div className="flex gap-[10px]" role="group">
                    {hasManualDepositTransactionType(selectedBank, banks) && (
                      <button
                      type="button"
                      className={`block w-[230px] cursor-pointer h-[120px]  rounded-[4px] border-[1px] border-[var(--button-border)]  ${selectedManualDeposit ? 'text-[#ffffff] bg-[var(--button-background)]' : 'text-[var(--text-color-primary)] bg-[#ffffff]'}`}
                      onClick={() => {
                        toggleManualDeposit();
                      }}
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
                      onClick={() => {
                        toggleAutoDeposit();
                      }}
                    >
                      <span className="text-[14px] font-bold">{bankDepositMatchingMode?.sourceTypeName}</span>
                      <br />
                      <span className="text-[12px]">{tManagePayments('autoDeposit')}</span>
                    </button>
                    )}
                  </div>
                  {fieldErrors.selectedDepositType && (
                    <div className="text-[#FE0000] text-[11px] mt-1">{fieldErrors.selectedDepositType}</div>
                  )}
                </div>
              )}

              {hasWithdrawalTransactionType(selectedBank, banks) && (
                <div className="mb-[8px]">
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

              {(() => {
                const selectedBankObj = banks.find(b => b.id === parseInt(selectedBank));
                return selectedBank && selectedBankObj && selectedBankObj.depositMatchingMode?.mappingTypes?.length > 0 && selectedAutoDeposit && (
                  <div className="mb-[8px]">
                    <label className="block text-[12px] font-semibold text-[#333333] my-[8px]">{tManagePayments('depositMatchingMode')}</label>
                    <div className="flex" role="group">
                      {selectedBankObj.depositMatchingMode.mappingTypes.includes(1) && (
                        <>                       
                          <label className='flex items-center cursor-pointer capitalize mr-[8px]' htmlFor="manualMatching1" onClick={() => setAgent_DepositMatchingMode(1)}>
                            <input className='w-[20px] h-[20px] m-[10px] cursor-pointer accent-[var(--primary-color)]' id="manualMatching1" type="radio" name="depositMatchingMode" value={agent_DepositMatchingMode} checked={agent_DepositMatchingMode == 1} />
                            <span className='text-[12px] leading-[14px] text-[#333333]'>{tManagePayments('decimal')}</span>
                          </label> 
                        </>
                      )}
                      {selectedBankObj.depositMatchingMode.mappingTypes.includes(2) && (
                        <>                        
                          <label className='flex items-center cursor-pointer capitalize' htmlFor="manualMatching2" onClick={() => setAgent_DepositMatchingMode(2)}>
                            <input className='w-[20px] h-[20px] m-[10px] cursor-pointer accent-[var(--primary-color)]' id="manualMatching2" type="radio" name="depositMatchingMode" value={agent_DepositMatchingMode} checked={agent_DepositMatchingMode == 2} />
                            <span className='text-[12px] leading-[14px] text-[#333333]'>{tManagePayments('bankMatching')}</span>
                          </label>
                        </>
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
                  <label className="block text-[12px] font-semibold text-[#333333] my-[8px]">{tManagePayments('accountHolderName')}<span className="text-[#E25757]">*</span></label>
                  <input
                    type="text"
                    className="h-[32px] max-[768px]:h-[40px] w-[100%] border-[1px] border-[var(--border-control)] rounded-[4px] px-[8px] text-[12px] focus:outline-none focus:border-[var(--button-border)] cursor-pointer read-only:bg-[#f4f4f4] read-only:cursor-not-allowed read-only:focus:border-[#0000003D]"
                    value={accountHolderName}
                    readOnly
                  />
                  <label className=" block mt-[4px] text-[#828282] text-[11px] leading-[12px] font-semibold">{tManagePayments('provideFullName')}</label>
                </div>

                <div className='w-[230px]'>
                <label className="block text-[12px] font-semibold text-[#333333] my-[8px]">{tManagePayments('bankAccountNumber')}<span className="text-[#E25757]">*</span></label>
                <input
                  type="text"
                  className="h-[32px] max-[768px]:h-[40px] w-[100%] border-[1px] border-[var(--border-control)] rounded-[4px] px-[8px] text-[12px] focus:outline-none focus:border-[var(--button-border)] cursor-pointer read-only:bg-[#f4f4f4] read-only:cursor-not-allowed read-only:focus:border-[#0000003D]"
                  value={accountNumber}
                  readOnly
                />
                </div>

                <div className='w-[230px]'>
                <label className="block text-[12px] font-semibold text-[#333333] my-[8px]">{tManagePayments('accountNickname')}<span className="text-[#E25757]">*</span></label>
                
                <input
                  type="text"
                  className={`h-[32px] max-[768px]:h-[40px] w-[100%] border-[1px] border-[var(--border-control)] rounded-[4px] px-[8px] text-[12px] focus:outline-none focus:border-[var(--button-border)] cursor-pointer `}
                  value={accountNickname}
                  onChange={(e) => {
                    setAccountNickname(e.target.value);
                  }}
                />
                <label className="block mt-[4px] text-[#828282] text-[11px] leading-[12px] font-semibold">{tManagePayments('accountNicknameNote1')}</label>
                </div>
              </div>

              <div className="mb-[8px]">
                <div className="pb-[8px] mt-[8px] text-[16px] text-[var(--text-color-primary)] font-semibold leading-[21.6px] tracking-[0.2px]">{tManagePayments('promptPay')}</div>
                <div className="block text-[12px] font-semibold text-[#333333] mb-[8px]">{tManagePayments('linkMobile')}</div>
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

              <div className="mb-[8px]">
                <label className="block text-[12px] font-semibold text-[#333333] my-[8px]">{tManagePayments('mobile')}<span className='text-[#E25757]'>*</span></label>
                <div className="flex items-center h-[32px] max-[768px]:h-[40px] w-[470px] border-[1px] border-[#0000003D] rounded-[4px] px-[8px] text-[12px] focus:outline-none focus:border-[var(--button-border)] cursor-pointer">
                  <select
                    className={`focus:outline-none focus:border-[var(--button-border)] h-[100%] cursor-pointer mr-[5px] ${fieldErrors.countryCode ? 'border-[#FE0000]' : ''}`}
                    value={countryCode}
                    onChange={(e) => {
                      setCountryCode(e.target.value);
                    }}
                    style={{ maxWidth: '120px' }}
                  >
                    {countryCodes.map((code) => (
                      <option key={code} value={code} selected={code === countryCode}>
                        +{code}
                      </option>
                    ))}                
                  </select>
                  <input
                    type="text"
                    className={`focus:outline-none w-[100%] h-[100%] pl-[10px] border-l-[1px] ${fieldErrors.mobile ? 'border-[#FE0000]' : 'border-[#0000003D]'}`}
                    value={mobile}
                    onChange={(e) => {
                      setMobile(e.target.value);
                    }}
                    placeholder={tManagePayments('enterMobileNumber')}
                  />
                </div>
                {(fieldErrors.mobile || fieldErrors.countryCode) && (
                  <div className="text-[#FE0000] text-[11px] mt-1">{fieldErrors.mobile || fieldErrors.countryCode}</div>
                )}
              </div>
            </div>
            <div className="flex justify-end py-[8px] mt-[8px]">
              <button
                type="button"
                className="w-[140px] h-[30px] rounded-[20px] text-[var(--button-text-cancel)] px-[16px] py-[4px] text-[12px] border-[1px] border-[var(--button-border)]  bg-[var(--button-background-cancel)] hover:bg-[var(--button-background-cancel-hover)] cursor-pointer"
                onClick={onClose}
                disabled={loading}
              >
                {tGlobal('cancel')}
              </button>
              <button
                type="submit"
                className="w-[140px] h-[30px] rounded-[20px] text-[var(--button-text)] px-[16px] py-[4px] text-[12px] border-[1px] border-[var(--button-border)] ml-[8px] bg-[var(--button-background)] hover:bg-[var(--button-background-hover)] hover:border-[var(--button-border-hover)] cursor-pointer shadow-[0_1px_5px_#0003,0_2px_2px_#00000024,0_3px_1px_-2px_#0000001f]"
                disabled={loading}
              >
                {loading ? tGlobal('saving') : tGlobal('save')}
              </button>
            </div>
          </form>
        </div>
      </div>
    </div>
  );
};

export default UpdateBankAccountComponent; 