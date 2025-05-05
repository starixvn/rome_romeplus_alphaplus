import React, { useState, useEffect } from 'react';
import { useTranslation } from 'react-i18next';
import { Bank } from '../../services/interfaces/Bank';
import { MobilePrefix } from '../../services/interfaces/MobilePrefix';
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
  const [depositMatchingMode, setDepositMatchingMode] = useState<string>('1');
  const [promptPay, setPromptPay] = useState<boolean>(false);
  const [mobile, setMobile] = useState<string>('');
  const [mobilePrefix, setMobilePrefix] = useState<string>('');
  const [mobilePrefixes, setMobilePrefixes] = useState<MobilePrefix[]>([]);
  const [error, setError] = useState<string | null>(null);
  const [success, setSuccess] = useState<boolean>(false);
  const [successMessage, setSuccessMessage] = useState<string>('');
  const [loading, setLoading] = useState<boolean>(false);
  const [banks, setBanks] = useState<Bank[]>([]);
  
  // Add state variables for selected transaction types
  const [selectedManualDeposit, setSelectedManualDeposit] = useState<boolean>(true);
  const [selectedAutoDeposit, setSelectedAutoDeposit] = useState<boolean>(false);
  const [selectedManualWithdrawal, setSelectedManualWithdrawal] = useState<boolean>(false);

  useEffect(() => {
    const fetchBanks = async () => {
      try {
        const fetchedBanks = await paymentProviderService.getBanksByTransactionType(0);
        setBanks(fetchedBanks);
        
        // Set first bank data if available
        if (fetchedBanks.length > 0) {
          const firstBank = fetchedBanks[0];
          setSelectedBank(firstBank.id.toString());
          
          // Set transaction types based on first bank's capabilities
          setSelectedManualDeposit(true);
          setSelectedAutoDeposit(false);
          setSelectedManualWithdrawal(false);
          
          // Set deposit matching mode based on bank's capabilities
          if (firstBank.depositMatchingMode >= 1) {
            setDepositMatchingMode('1');
          }
        }
      } catch (err) {
        console.error('Error fetching banks:', err);
        setError(tManagePayments('failedToFetchBanks'));
      }
    };

    const fetchMobilePrefixes = async () => {
      try {
        const prefixes = await paymentProviderService.getMobilePrefixes();
        setMobilePrefixes(prefixes);
        if (prefixes.length > 0) {
          setMobilePrefix(prefixes[0].prefix);
        }
      } catch (err) {
        console.error('Error fetching mobile prefixes:', err);
      }
    };

    fetchBanks();
    fetchMobilePrefixes();
  }, [tManagePayments]);

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

      const result = await paymentProviderService.addBankAccount({
        id: 0, // Temporary ID for new bank account
        accountNickname,
        accountHolderName,
        accountNumber,
        bankId: parseInt(selectedBank),
        bankName: banks.find(b => b.id === parseInt(selectedBank))?.name || '',
        transactionTypes: transactionTypesString,
        status: parseInt(status),
        depositMatchingMode: parseInt(depositMatchingMode),
        promptPay,
        mobile: mobile,
        mobilePrefix: mobilePrefix,
        lastUpdated: new Date().toISOString(),
        lastUpdatedStr: new Date().toLocaleString()
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
  const handleBankChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
    const newBankId = e.target.value;
    setSelectedBank(newBankId);
    
    // Reset transaction type selections
    setSelectedManualDeposit(true);
    setSelectedAutoDeposit(false);
    setSelectedManualWithdrawal(false);
  };

  return (
    <div className="p-[12px]" tabIndex={-1}>
      <div className="modal-dialog">
        <div className="modal-content max-w-[1200px]">
          <div className="mb-[8px]">
            <h2 className="flex items-center font-bold text-[15px] ">{tManagePayments('bankManagement')}</h2>            
          </div>
          <form onSubmit={handleSubmit} className='bg-[#ffffff] p-[16px] rounded-[5px] shadow-[0_1px_5px_#0003,0_2px_2px_#00000024,0_3px_1px_-2px_#0000001f]'>
            <div className="modal-body">
              {error && (
                <div className="mb-[8px] font-bold text-[12px] text-[#333] bg-[#FFF2F2] border-[1px] border-[#FE0000] rounded-[10px] pl-[36px] pr-[12px] py-[8px] relative block capitalize" role="alert">
                  <IconError className='w-[20px] h-[20px] fill-[#fe0000] absolute top-[8px] left-[10px]' />{error}
                </div>
              )}
              {success && (                
                <div className="mb-[8px] font-bold text-[12px] text-[#333] bg-[#E2FFF2] border-[1px] border-[#5cb774] rounded-[10px] pl-[36px] pr-[12px] py-[8px] relative block capitalize" role="alert"><IconCheck className='w-[20px] h-[20px] fill-[#5CB774] absolute top-[8px] left-[10px]' />{successMessage}</div>
              )}
              
              <div className="mb-3">
                <div className="pb-[8px] text-[16px] text-[#163f78] font-semibold leading-[21.6px] tracking-[0.2px]">{tManagePayments('bankStatus')}</div>
                <p className="text-[12px] my-[8px]">{tManagePayments('bankNote1')}</p>
                <div className="btn-group" role="group">
                  <button
                    type="button"
                    className={`text-[12px] w-[100px] px-[16px] py-[8px] cursor-pointer ${status === '1' ? 'bg-[#163f78] text-[#ffffff]' : 'bg-[#eeeeee] text-[#163f78]'}`}
                    onClick={() => setStatus('1')}
                  >
                    {tGlobal('enable')}
                  </button>
                  <button
                    type="button"
                    className={`text-[12px] text-[#163f78] w-[100px] px-[16px] py-[8px] cursor-pointer ${status === '2' ? 'bg-[#163f78] text-[#ffffff]' : 'bg-[#eeeeee] text-[#163f78]'}`}
                    onClick={() => setStatus('2')}
                  >
                    {tGlobal('disable')}
                  </button>
                </div>
              </div>

              <div className="mt-[8px] text-[16px] text-[#163f78] font-semibold leading-[21.6px] tracking-[0.2px]">{tManagePayments('accountInformation')}</div>
              <div>
                <label className="block text-[12px] font-semibold text-[#333333] my-[8px]">{tManagePayments('bank')}</label>
                <select
                  className="h-[35px] w-[470px] border-[1px] border-[#0000003D] rounded-[4px] px-[8px] text-[12px] focus:outline-none focus:border-[#163f78] cursor-pointer"
                  value={selectedBank}
                  onChange={handleBankChange}
                  required
                >
                  {banks.map((bank) => (
                    <option key={bank.id} value={bank.id}>
                      {bank.name}
                    </option>
                  ))}
                </select>
              </div>

              {hasDepositTransactionType(selectedBank, banks) && (
                <div className="mb-3">
                  <label className="block text-[12px] font-semibold text-[#333333] my-[8px]">{tManagePayments('depositType')}</label>
                  <div className="flex gap-[10px]" role="group">
                    {hasManualDepositTransactionType(selectedBank, banks) && (
                      <button
                        type="button"
                        className={`block w-[230px] cursor-pointer h-[120px]  rounded-[4px] border-[1px] border-[#163f78]  ${selectedManualDeposit ? 'text-[#ffffff] bg-[#163f78]' : 'text-[#163f78] bg-[#ffffff]'}`}
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
                        className={`block w-[230px] cursor-pointer h-[120px] rounded-[4px] border-[1px] border-[#163f78] ${selectedAutoDeposit ? 'text-[#ffffff] bg-[#163f78]' : 'text-[#163f78] bg-[#ffffff]'}`}
                        onClick={toggleAutoDeposit}
                      >
                        <span className="text-[14px] font-bold">{tManagePayments('sms')}</span>
                        <br />
                        <span className="text-[12px]">{tManagePayments('autoDeposit')}</span>
                      </button>
                    )}
                  </div>
                </div>
              )}

              {hasWithdrawalTransactionType(selectedBank, banks) && (
                <div className="mb-3">
                  <label className="block text-[12px] font-semibold text-[#333333] my-[8px]">{tManagePayments('withdrawalType')}</label>
                  <div className="btn-group" role="group">
                    {hasManualWithdrawalTransactionType(selectedBank, banks) && (
                      <button
                        type="button"
                        className={`block cursor-pointer w-[230px] h-[120px] rounded-[4px] border-[1px] border-[#163f78] ${selectedManualWithdrawal ? 'text-[#ffffff] bg-[#163f78]' : 'text-[#163f78] bg-[#ffffff]'}`}
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
                return selectedBank && selectedBankObj && selectedBankObj.depositMatchingMode >= 1 && (
                  <div className="mb-3">
                    <label className="block text-[12px] font-semibold text-[#333333] my-[8px]">{tManagePayments('depositMatchingMode')}</label>
                    <div className="flex gap-[8px]" role="group">
                      <label className='flex items-center cursor-pointer capitalize' htmlFor="manualMatching1" onClick={() => setDepositMatchingMode('1')}>
                        <input className='w-[20px] h-[20px] m-[10px] cursor-pointer' id="manualMatching1" type="radio" name="depositMatchingMode" value={depositMatchingMode} checked={depositMatchingMode === '1'} />
                        <span className='text-[12px] leading-[14px]'>{tManagePayments('manualMatching')}</span>
                      </label>
                      {selectedBankObj.depositMatchingMode >= 2 && (                        
                        <label className='flex items-center cursor-pointer capitalize' htmlFor="manualMatching2" onClick={() => setDepositMatchingMode('2')}>
                          <input className='w-[20px] h-[20px] m-[10px] cursor-pointer' id="manualMatching2" type="radio" name="depositMatchingMode" value={depositMatchingMode} checked={depositMatchingMode === '2'} />
                          <span className='text-[12px] leading-[14px]'>{tManagePayments('autoMatching')}</span>
                        </label>
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
                    className="h-[35px] w-[100%] border-[1px] border-[#0000003D] rounded-[4px] px-[8px] text-[12px] focus:outline-none focus:border-[#163f78] cursor-pointer"
                    value={accountHolderName}
                    onChange={(e) => setAccountHolderName(e.target.value)}
                    required
                  />
                  <label className=" block mt-[4px] text-[#828282] text-[11px] leading-[12px] font-semibold">{tManagePayments('provideFullName')}</label>
                </div>

                <div className='w-[230px]'>
                  <label className="block text-[12px] font-semibold text-[#333333] my-[8px]">{tManagePayments('bankAccountNumber')}</label>
                  <input
                    type="text"
                    className="h-[35px] w-[100%] border-[1px] border-[#0000003D] rounded-[4px] px-[8px] text-[12px] focus:outline-none focus:border-[#163f78] cursor-pointer"
                    value={accountNumber}
                    onChange={(e) => setAccountNumber(e.target.value)}
                    required
                  />
                </div>

                <div className='w-[230px]'> 
                  <label className="block text-[12px] font-semibold text-[#333333] my-[8px]">{tManagePayments('accountNickname')}</label>
                  
                  <input
                    type="text"
                    className="h-[35px] w-[100%] border-[1px] border-[#0000003D] rounded-[4px] px-[8px] text-[12px] focus:outline-none focus:border-[#163f78] cursor-pointer"
                    value={accountNickname}
                    onChange={(e) => setAccountNickname(e.target.value)}
                    required
                  />
                  <label className=" block mt-[4px] text-[#828282] text-[11px] leading-[12px] font-semibold">{tManagePayments('accountNicknameNote1')}</label>
                </div>
              </div>

              <div className="mb-3">
                <div className="pb-[8px] mt-[8px] text-[16px] text-[#163f78] font-semibold leading-[21.6px] tracking-[0.2px]">{tManagePayments('promptPay')}</div>
                <p className="pb-[8px] text-[12px]">{tManagePayments('linkMobile')}</p>
                <div className="btn-group" role="group">
                  <button
                    type="button"
                    className={`text-[12px] text-[#163f78] w-[70px] px-[16px] py-[8px] cursor-pointer  ${promptPay ? 'bg-[#163f78] text-[#ffffff]' : 'bg-[#eeeeee] text-[#163f78]'}`}
                    onClick={() => setPromptPay(true)}
                  >
                    {tGlobal('on')}
                  </button>
                  <button
                    type="button"
                    className={`text-[12px] text-[#163f78] w-[70px] px-[16px] py-[8px] cursor-pointer text-[#163f78] ${!promptPay ? 'bg-[#163f78] text-[#ffffff]' : 'bg-[#eeeeee] text-[#163f78]'}`}
                    onClick={() => setPromptPay(false)}
                  >
                    {tGlobal('off')}
                  </button>
                </div>
              </div>

              
              <div className="mb-3">
                <label className="block text-[12px] font-semibold text-[#333333] my-[8px]">{tManagePayments('mobile')}</label>
                <div className="flex items-center h-[35px] w-[470px] border-[1px] border-[#0000003D] rounded-[4px] px-[8px] text-[12px] focus:outline-none focus:border-[#163f78] cursor-pointer">
                  <select
                    className="focus:outline-none focus:border-[#163f78] cursor-pointer"
                    value={mobilePrefix}
                    onChange={(e) => setMobilePrefix(e.target.value)}
                    style={{ maxWidth: '120px' }}
                  >
                    {mobilePrefixes.map((prefix) => (
                      <option key={prefix.id} value={prefix.prefix}>
                        {prefix.prefix}
                      </option>
                    ))}
                  </select>
                  <input
                    type="text"
                    className="focus:outline-none focus:border-[#163f78] w-[100%] h-[100%]"
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
                className="w-[140px] h-[30px] rounded-[20px] text-[#163f78] px-[16px] py-[4px] text-[12px] border-[1px] border-[#163f78]  bg-[#ffffff] hover:bg-[#EAEDF2] cursor-pointer "
                onClick={onClose}
                disabled={loading}
              >
                {tGlobal('cancel')}
              </button>
              <button
                type="submit"
                className="w-[140px] h-[30px] rounded-[20px] text-[#ffffff] px-[16px] py-[4px] text-[12px] border-[1px] border-[#163f78] ml-[8px] bg-[#163f78] hover:bg-[#375A8A] cursor-pointer  shadow-[0_1px_5px_#0003,0_2px_2px_#00000024,0_3px_1px_-2px_#0000001f]"
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

export default AddBankAccountComponent; 