import React, { useState, useEffect } from 'react';
import { useTranslation } from 'react-i18next';
import { transactionAPI } from '../../../../services/transactionsHistoryAPI';
import { MemberLoginName } from '../../../../services/interfaces/Transactions/MemberLoginName';
import { BankInfo } from '../../../../services/interfaces/Transactions/BankInfo';
import { CreditAmount } from '../../../../services/interfaces/Transactions/CreditAmount';
import pendingDepositAPI from '../../../../services/pendingDepositAPI';
import UploadImage from '../../Components/UploadImage';
import { AddDepositManualModel } from '../../../../services/interfaces/AddDepositManualModel';
import Loading from '../../../../components/LoadingComponent';
import { ResultMessageComponent } from '../../../../components/MessageComponents';

interface ManualAddDepositFormProps {
  cashAgentOptions: BankInfo[];
  onClose: (isSuccess: boolean) => void;
}

const ManualAddDepositForm: React.FC<ManualAddDepositFormProps> = ({ cashAgentOptions, onClose }) => {
  const { t } = useTranslation('pendingDeposit');
  const [memberLoginOptions, setMemberLoginOptions] = useState<MemberLoginName[]>([]);
  const [memberId, setMemberId] = useState<number>(0);
  const [paymentFrom, setPaymentFrom] = useState('');
  const [paymentTo, setPaymentTo] = useState(cashAgentOptions[0].bankAccountNumber);
  const [cashMemberOptions, setCashMemberOptions] = useState<BankInfo[]>([]);
  const [creditAmount, setCreditAmount] = useState<CreditAmount>({ minAmt: 0, maxAmt: 0 });
  const [depositAmount, setDepositAmount] = useState('');
  const [remarks, setRemarks] = useState('');
  const [image, setImage] = useState<File | null>(null);
  const [initialLoading, setInitialLoading] = useState(true);
  const [validationMessage, setValidationMessage] = useState<{ type: 'error' | 'success' | 'warning', message: string } | null>(null);

  console.log('imageimageimage', image);

  useEffect(() => {
    const fetchMemberLoginName = async () => {
      try {
        const memberLoginNames = await transactionAPI.getMemberLoginName();
        setMemberLoginOptions(memberLoginNames);
        setMemberId(memberLoginNames[0]?.custId);
        console.log('memberLoginNames', memberLoginNames);
      } catch (error) {
        console.error('Error fetching member login names:', error);
      }
    };
    fetchMemberLoginName();
  }, []);

  useEffect(() => {
    const fetchCashMemberBankInfo = async () => {
      if (memberId) {
        try {
          const bankInfo = await transactionAPI.getCashMemberBankInfo(memberId);
          setCashMemberOptions(bankInfo);
          if (bankInfo.length > 0) {
            setPaymentFrom(bankInfo[0].bankAccountNumber);
          }
        } catch (error) {
          console.error('Error fetching cash member bank info:', error);
        }
      }
    };
    fetchCashMemberBankInfo();
  }, [memberId]);

  useEffect(() => {
    const fetchCreditLimitAmount = async () => {
      if (memberId) {
        try {
          const creditAmount = await pendingDepositAPI.getLimitAmtDeposit(memberId);
          setCreditAmount(creditAmount);
        } catch (error) {
          console.error('Error fetching credit limit amount:', error);
        } finally {
          setInitialLoading(false);
        }
      }
    };
    fetchCreditLimitAmount();
  }, [memberId]);

  const handleMemberChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
    const newMemberId = parseInt(e.target.value);
    setMemberId(newMemberId);
  };

  const getParams = () => {
    const params : AddDepositManualModel = {
      custId: memberId,
      paymentFrom: paymentFrom,
      paymentTo: paymentTo,
      amount: parseFloat(depositAmount),
      remark: remarks,
      file: image as File,
    }
    return params;
  };

  const validateParams = (params: AddDepositManualModel): boolean => {
    if (!params.custId) {
      setValidationMessage({
        type: 'error',
        message: 'Member login name is required'
      });
      return false;
    }

    if (!params.paymentFrom) {
      setValidationMessage({
        type: 'error',
        message: 'Payment from account is required'
      });
      return false;
    }

    if (!params.paymentTo) {
      setValidationMessage({
        type: 'error',
        message: 'Payment to account is required'
      });
      return false;
    }

    if (!params.amount || params.amount <= 0) {
      setValidationMessage({
        type: 'error',
        message: 'Deposit amount must be greater than 0'
      });
      return false;
    }
    
    if (params.file && params.file.size > 1048576) {
      setValidationMessage({
        type: 'error',
        message: 'File size must be less than 1MB'
      });
      return false;
    }

    return true;
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    const params = getParams();
    
    if (!validateParams(params)) {
      return;
    }

    try {
      const response = await pendingDepositAPI.createDepositManual(params);
      if (response.isSuccessful) {
        setValidationMessage({
          type: 'success',
          message: 'Deposit request submitted successfully'
        });      
        setTimeout(() => {
          onClose(true);
        }, 3000);
      } else {
        setValidationMessage({
          type: 'error',
          message: response.message || 'Failed to submit deposit request'
        });
      }
    } catch (error) {
      setValidationMessage({
        type: 'error',
        message: 'Failed to submit deposit request'
      });
    }
  };

  return (
    <div className="fixed inset-0 bg-white flex flex-col z-50">
      {initialLoading ? (
        <div className="flex items-center justify-center h-full">
          <Loading />
        </div>
      ) : (
        <>
          <div className="bg-gray-100 p-4">
            <h2 className="text-lg font-medium">{t('manualDepositForm.title')}</h2>
          </div>

          <form id="manual-deposit-form" onSubmit={handleSubmit} className="p-6 flex-1">
            {validationMessage && (
              <ResultMessageComponent
                type={validationMessage.type}
                message={validationMessage.message}
                className="mb-4"
              />
            )}
            <div className="flex flex-row space-x-10">
              <div className="">
                <div className="mb-5 w-[900px]">
                  <h3 className="text-md font-medium text-blue-900 mb-4">
                    {t('manualDepositForm.accountInformation')}
                  </h3>

                  <div className="flex flex-col space-y-4">
                    <div className="grid grid-cols-2 gap-4">
                      <div>
                        <label
                          htmlFor="memberLoginName"
                          className="block text-sm font-medium text-gray-700 mb-1 relative"
                        >
                          {t('manualDepositForm.memberLoginName')}
                          <span className="text-red-500 absolute">*</span>
                        </label>
                        <div className="relative">
                          <select
                            id="memberLoginName"
                            value={memberId}
                            onChange={handleMemberChange}
                            className="border border-gray-300 rounded-md px-2 w-full h-[32px] max-[768px]:h-[40px] focus:outline-none focus:ring-1 focus:ring-[#214184] focus:border-[#214184] appearance-auto"
                          >
                            {memberLoginOptions.map((data, key) => (
                              <option key={`${key}-${data.custId}`} value={data.custId}>
                                {data.username}
                              </option>
                            ))}
                          </select>
                        </div>
                      </div>

                      <div>
                        <label
                          htmlFor="paymentFrom"
                          className="block text-sm font-medium text-gray-700 mb-1 relative"
                        >
                          {t('manualDepositForm.from')}
                          <span className="text-red-500 absolute">*</span>
                        </label>
                        <div className="relative">
                          <select
                            id="paymentFrom"
                            value={paymentFrom}
                            onChange={(e) => setPaymentFrom(e.target.value)}
                            className="border border-gray-300 rounded-md px-2 w-full h-[32px] max-[768px]:h-[40px] focus:outline-none focus:ring-1 focus:ring-[#214184] focus:border-[#214184] appearance-auto"
                          >
                            {cashMemberOptions.map((option, key) => (
                              <option key={`${key}-${option.bankAccountNumber}`} value={option.bankAccountNumber}>
                                {option.bankAccountNumber}
                              </option>
                            ))}
                          </select>
                        </div>
                      </div>
                    </div>

                    <div className="grid grid-cols-2 gap-4">
                      <div>
                        <label
                          htmlFor="paymentTo"
                          className="block text-sm font-medium text-gray-700 mb-1 relative"
                        >
                          {t('manualDepositForm.to')}
                          <span className="text-red-500 absolute">*</span>
                        </label>
                        <div className="relative">
                          <select
                            id="paymentTo"
                            value={paymentTo}
                            onChange={(e) => setPaymentTo(e.target.value)}
                            className="border border-gray-300 rounded-md px-2 w-full h-[32px] max-[768px]:h-[40px] focus:outline-none focus:ring-1 focus:ring-[#214184] focus:border-[#214184] appearance-auto"
                          >
                            {cashAgentOptions.map((option, key) => (
                              <option key={`${key}-${option.bankAccountNumber}`} value={option.bankAccountNumber}>
                                {option.bankAccountNumber}
                              </option>
                            ))}
                          </select>
                        </div>
                      </div>

                      <div>
                        <label
                          htmlFor="depositAmount"
                          className="block text-sm font-medium text-gray-700 mb-1 relative"
                        >
                          {t('manualDepositForm.depositAmount')}
                          <span className="text-red-500 absolute">*</span>
                          <span className="text-sm pl-3">
                            <span className="text-blue-900">
                              (Agent Available Credit:
                                <span className="text-red-500">
                                  {` ${creditAmount.minAmt} - ${creditAmount.maxAmt}`}
                                </span>
                              )
                            </span>
                          </span>
                        </label>
                        <input
                          id="depositAmount"
                          type="number"
                          value={depositAmount}
                          onChange={(e) => setDepositAmount(e.target.value)}
                          className="border border-gray-300 rounded-md px-2 w-full h-[32px] max-[768px]:h-[40px] focus:outline-none focus:ring-1 focus:ring-[#214184] focus:border-[#214184]"
                          placeholder="0"
                        />
                      </div>
                    </div>

                    <div>
                      <label htmlFor="remarks" className="block text-sm font-medium text-gray-700 mb-1">
                        {t('manualDepositForm.remarks')}
                      </label>
                      <textarea
                        id="remarks"
                        value={remarks}
                        onChange={(e) => setRemarks(e.target.value)}
                        className="border border-gray-300 rounded-md p-2 w-full focus:outline-none focus:ring-1 focus:ring-[#214184] focus:border-[#214184] min-h-[120px]"
                        placeholder={t('manualDepositForm.remarksPlaceholder')}
                      />
                    </div>
                  </div>
                </div>
              </div>

              <div className="flex flex-col items-center pl-20">
                <UploadImage
                  setImage={setImage}
                />

                <div className="flex justify-center space-x-4 mt-3 w-full">
                  <button
                    type="button"
                    onClick={() => onClose(false)}
                    className="w-[140px] h-[32px] max-[768px]:h-[40px] border border-gray-300 rounded-full text-sm font-medium text-gray-700 bg-white hover:bg-gray-50 focus:outline-none"
                  >
                    {t('manualDepositForm.cancel')}
                  </button>
                  <button
                    type="submit"
                    className="w-[140px] h-[32px] max-[768px]:h-[40px] border border-transparent rounded-full text-sm font-medium text-white bg-[#163f78] hover:bg-[#0f2d5c] focus:outline-none"
                  >
                    {t('manualDepositForm.approve')}
                  </button>
                </div>
              </div>
            </div>
          </form>
        </>
      )}
    </div>
  );
};

export default ManualAddDepositForm;
