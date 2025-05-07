import React, { useState, useEffect } from 'react';
import { useTranslation } from 'react-i18next';
import { Bank } from '../../services/interfaces/Bank';
import { BankAccount } from '../../services/interfaces/BankAccount';
import { DepositMatchingMode } from '../../services/interfaces/DepositMatchingMode';
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
import { baseAPI } from '../../services/baseAPI';

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
  const [banks, setBanks] = useState<Bank[]>([]);

  const [bankDepositMatchingMode, setBankDepositMatchingMode] = useState<DepositMatchingMode>(bankAccount.bankDepositMatchingMode);
  const [bank_TransTypeList, setBank_TransTypeList] = useState<string>(bankAccount.bank_TransTypeList);
  const [agent_DepositMatchingMode, setAgent_DepositMatchingMode] = useState<number>(bankAccount.agent_DepositMatchingMode);

  // Add state variables for selected transaction types
  const [selectedManualDeposit, setSelectedManualDeposit] = useState<boolean>(false);
  const [selectedAutoDeposit, setSelectedAutoDeposit] = useState<boolean>(false);
  const [selectedManualWithdrawal, setSelectedManualWithdrawal] = useState<boolean>(false);

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

          var countryCode = fetchedBankAccount.mobile.split('-')[0];
          var mobile = fetchedBankAccount.mobile.split('-')[1];
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

    fetchBanks();
    fetchBankAccount();
    fetchCountryCodes();
  }, [bankAccount.id, tManagePayments]);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError(null);
    setSuccess(false);
    setLoading(true);

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

  return (
    <div className="p-[12px]" tabIndex={-1}>
      <div className="modal-dialog">
        <div className="modal-content max-w-[1200px]">
          <div className="mb-[8px]">
            <h2 className="flex items-center font-bold text-[15px] ">{tManagePayments('bankManagement')}</h2>
            
          </div>
          <div>
            <label>{bankAccount.accountNickname}</label>
            <label>{tManagePayments(`status-${bankAccount.status}`)}</label>
            <label>{tManagePayments('createdBy')}</label>
            <label>{tManagePayments('lastUpdated')}</label>
            <label>{creator}</label>
            <label>{bankAccount.lastUpdatedStr}</label>
          </div>
          <form onSubmit={handleSubmit} className='bg-[#ffffff] p-[16px] rounded-[5px] shadow-[0_1px_5px_#0003,0_2px_2px_#00000024,0_3px_1px_-2px_#0000001f]'>
            <div className="modal-body">
              {error && (
                <div className="mt-[16px] font-bold text-[12px] text-[#333] bg-[#FFF2F2] border-[1px] border-[#FE0000] rounded-[10px] pl-[36px] pr-[12px] py-[8px] relative block capitalize"><IconError className='w-[20px] h-[20px] fill-[#fe0000] absolute top-[8px] left-[10px]' />{error}</div>
              )}
              {success && (
                <div className="mt-[16px] font-bold text-[12px] text-[#333] bg-[#E2FFF2] border-[1px] border-[#5cb774] rounded-[10px] pl-[36px] pr-[12px] py-[8px] relative block capitalize"><IconCheck className='w-[20px] h-[20px] fill-[#5CB774] absolute top-[8px] left-[10px]' />{successMessage}</div>
              )}
              
              <div className="mb-3">
                <div className='pb-[8px] mt-[8px] text-[16px] text-[var(--primary-color)] font-semibold leading-[21.6px] tracking-[0.2px]' >{tManagePayments('bankStatus')}</div>
                <p className="pb-[8px] text-[12px]">{tManagePayments('bankNote1')}</p>
                <div className="btn-group" role="group">
                  <button
                    type="button"
                    className={`text-[12px] w-[100px] px-[16px] py-[8px] cursor-pointer  ${status === '1' ? 'bg-[var(--primary-color)] text-[#ffffff]' : 'bg-[#eeeeee] text-[var(--primary-color)]'}`}                    onClick={() => setStatus('1')}
                  >
                    {tGlobal('enable')}
                  </button>
                  <button
                    type="button"
                    className={`text-[12px] w-[100px] px-[16px] py-[8px] cursor-pointer  ${status === '0' ? 'bg-[var(--primary-color)] text-[#ffffff]' : 'bg-[#eeeeee] text-[var(--primary-color)]'}`}
                    onClick={() => setStatus('0')}
                  >
                    {tGlobal('disable')}
                  </button>
                </div>
              </div>

              <div className="pb-[8px] mt-[8px] text-[16px] text-[var(--primary-color)] font-semibold leading-[21.6px] tracking-[0.2px]">{tManagePayments('accountInformation')}</div>

              <div className="mb-3">
                <label className="block text-[12px] font-semibold text-[#333333] my-[8px]">{tManagePayments('bank')}</label>
                <select
                  className="h-[35px] w-[470px] border-[1px] border-[#0000003D] rounded-[4px] px-[8px] text-[12px] focus:outline-none focus:border-[var(--primary-color)] cursor-pointer disabled:bg-[#f4f4f4] disabled:cursor-not-allowed"
                  value={selectedBank}
                  disabled
                  required
                >
                  {banks.map((bank) => (
                    <option key={bank.id} value={bank.id} selected={bank.id === parseInt(selectedBank)}>
                      {bank.name}
                    </option>
                  ))}
                </select>
              </div>

              {hasDepositTransactionType(selectedBank, banks) && (
                <div className="mb-3">
                  <label className="form-label">{tManagePayments('depositType')}</label>
                  <div className="btn-group" role="group">
                    {hasManualDepositTransactionType(selectedBank, banks) && (
                      <button
                        type="button"
                        className={`btn ${selectedManualDeposit ? 'btn-primary' : 'btn-slip-upload'}`}
                        onClick={toggleManualDeposit}
                      >
                        {tManagePayments('slipUpload')}
                        <br />
                        {tManagePayments('manualDeposit')}
                      </button>
                    )}
                    {hasAutoDepositTransactionType(selectedBank, banks) && (
                      <button
                        type="button"
                        className={`btn ${selectedAutoDeposit ? 'btn-primary' : 'btn-auto-deposit'}`}
                        onClick={toggleAutoDeposit}
                      >
                        {tManagePayments('sms')}
                        <br />
                        {tManagePayments('autoDeposit')}
                      </button>
                    )}
                  </div>
                </div>
              )}

              {hasWithdrawalTransactionType(selectedBank, banks) && (
                <div className="mb-3">
                  <label className="form-label">{tManagePayments('withdrawalType')}</label>
                  <div className="btn-group" role="group">
                    {hasManualWithdrawalTransactionType(selectedBank, banks) && (
                      <button
                        type="button"
                        className={`btn ${selectedManualWithdrawal ? 'btn-primary' : 'btn-manual-withdrawal'}`}
                        onClick={toggleManualWithdrawal}
                      >
                        {tManagePayments('manualWithdrawal')}
                      </button>
                    )}
                  </div>
                </div>
              )}

              {(() => {
                const selectedBankObj = banks.find(b => b.id === parseInt(selectedBank));
                return selectedBank && selectedBankObj && selectedBankObj.depositMatchingMode >= 1 && (
                  <div className="mb-3">
                    <label className="form-label">{tManagePayments('depositMatchingMode')}</label>
                    <div className="btn-group" role="group">
                      <button
                        type="button"
                        className={`btn ${agent_DepositMatchingMode === 1 ? 'btn-primary' : 'btn-outline-primary'}`}
                        onClick={() => setAgent_DepositMatchingMode(1)}
                      >
                        {tManagePayments('manualMatching')}
                      </button>
                      {selectedBankObj.depositMatchingMode >= 2 && (
                        <button
                          type="button"
                          className={`btn ${agent_DepositMatchingMode === 2 ? 'btn-primary' : 'btn-outline-primary'}`}
                          onClick={() => setAgent_DepositMatchingMode(2)}
                        >
                          {tManagePayments('autoMatching')}
                        </button>
                      )}
                    </div>
                  </div>
                );
              })()}

              <div className="flex gap-[16px]">
                <div className='w-[230px]'>
                  <label className="block text-[12px] font-semibold text-[#333333] my-[8px]">{tManagePayments('accountHolderName')}</label>
                  <input
                    type="text"
                    className="h-[35px] w-[100%] border-[1px] border-[#0000003D] rounded-[4px] px-[8px] text-[12px] focus:outline-none focus:border-[var(--primary-color)] cursor-pointer read-only:bg-[#f4f4f4] read-only:cursor-not-allowed"
                    value={accountHolderName}
                    readOnly
                    required
                  />
                  <label className=" block mt-[4px] text-[#828282] text-[11px] leading-[12px] font-semibold">{tManagePayments('provideFullName')}</label>
                </div>

                <div className='w-[230px]'>
                <label className="block text-[12px] font-semibold text-[#333333] my-[8px]">{tManagePayments('bankAccountNumber')}</label>
                <input
                  type="text"
                  className="h-[35px] w-[100%] border-[1px] border-[#0000003D] rounded-[4px] px-[8px] text-[12px] focus:outline-none focus:border-[var(--primary-color)] cursor-pointer read-only:bg-[#f4f4f4] read-only:cursor-not-allowed"
                  value={accountNumber}
                  readOnly
                  required
                />
                </div>

                <div className='w-[230px]'>
                <label className="block text-[12px] font-semibold text-[#333333] my-[8px]">{tManagePayments('accountNickname')}</label>
                
                <input
                  type="text"
                  className="h-[35px] w-[100%] border-[1px] border-[#0000003D] rounded-[4px] px-[8px] text-[12px] focus:outline-none focus:border-[var(--primary-color)] cursor-pointer"
                  value={accountNickname}
                  onChange={(e) => setAccountNickname(e.target.value)}
                  required
                />
                <label className=" block mt-[4px] text-[#828282] text-[11px] leading-[12px] font-semibold">{tManagePayments('accountNicknameNote1')}</label>
                </div>
              </div>

              <div className="mb-3">
                <div className="pb-[8px] mt-[8px] text-[16px] text-[var(--primary-color)] font-semibold leading-[21.6px] tracking-[0.2px]">{tManagePayments('promptPay')}</div>
                <div className="pb-[8px] text-[12px]">{tManagePayments('linkMobile')}</div>
                <div className="btn-group" role="group">
                  <button
                    type="button"
                    className={`text-[12px] text-[var(--primary-color)] w-[70px] px-[16px] py-[8px] cursor-pointer ${promptPay ? 'bg-[var(--primary-color)] text-[#ffffff]' : 'bg-[#eeeeee] text-[var(--primary-color)]'}`}
                    onClick={() => setPromptPay(true)}
                  >
                    {tGlobal('on')}
                  </button>
                  <button
                    type="button"
                    className={`text-[12px] text-[var(--primary-color)] w-[70px] px-[16px] py-[8px] cursor-pointer ${!promptPay ? 'bg-[var(--primary-color)] text-[#ffffff]' : 'bg-[#eeeeee] text-[var(--primary-color)]'}`}
                    onClick={() => setPromptPay(false)}
                  >
                    {tGlobal('off')}
                  </button>
                </div>
              </div>

              <div className="mb-3">
                <label className="form-label">{tManagePayments('mobile')}</label>
                <div className="flex items-center h-[35px] w-[470px] border-[1px] border-[#0000003D] rounded-[4px] px-[8px] text-[12px] focus:outline-none focus:border-[var(--primary-color)] cursor-pointer">
                  <select
                    className="focus:outline-none focus:border-[var(--primary-color)] cursor-pointer"
                    value={countryCode}
                    onChange={(e) => setCountryCode(e.target.value)}
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
                    className="focus:outline-none focus:border-[var(--primary-color)] w-[100%] h-[100%]"
                    value={mobile}
                    onChange={(e) => setMobile(e.target.value)}
                    placeholder={tManagePayments('enterMobileNumber')}
                    required
                  />
                </div>
              </div>
            </div>
            <div className="flex justify-end py-[8px] mt-[8px]">
              <button
                type="button"
                className="w-[140px] h-[30px] rounded-[20px] text-[var(--primary-color)] px-[16px] py-[4px] text-[12px] border-[1px] border-[var(--primary-color)]  bg-[#ffffff] hover:bg-[#EAEDF2] cursor-pointer"
                onClick={onClose}
                disabled={loading}
              >
                {tGlobal('cancel')}
              </button>
              <button
                type="submit"
                className="w-[140px] h-[30px] rounded-[20px] text-[#ffffff] px-[16px] py-[4px] text-[12px] border-[1px] border-[var(--primary-color)] ml-[8px] bg-[var(--primary-color)] hover:bg-[#375A8A] cursor-pointer  shadow-[0_1px_5px_#0003,0_2px_2px_#00000024,0_3px_1px_-2px_#0000001f]"
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