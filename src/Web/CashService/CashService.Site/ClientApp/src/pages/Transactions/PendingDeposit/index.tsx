import React, { useState, useMemo, useEffect } from 'react';
import { createRoot } from 'react-dom/client';
import { useDebouncedCallback } from '../../../hooks/useDebouncedCallback';
import ManualAddDepositForm from './components/ManualAddDepositForm';
import Filter from './components/Filter';
import DataGrid from './components/DataGrid';
import { useTranslation } from 'react-i18next';
import i18n from '../../../i18n/i18n-pendingDeposit';
import { useCookieLanguage } from '../../../hooks/useCookieLanguage';
import '../../../styles/common.css';
import '../../../styles/global.scss';
import { BankInfo } from '../../../services/interfaces/Transactions/BankInfo';
import { calculateDateRange, TransactionType } from '../../../utils/commonFunctions';
import { transactionAPI } from '../../../services/transactionsHistoryAPI';
import {
  TransactionsHistoryParams,
  TransactionsHistoryResponse,
} from '../../../services/interfaces/Transactions/TransactionHistory';
import TitleComponent from '../../../components/TitleComponents';
import { ButtonOutlineComponent } from '../../../components/ButtonComponents';
import { LoadingFullComponent } from '../../../components/LoadingComponent';

const PendingDeposit: React.FC = () => {
  const { t } = useTranslation('pendingDeposit');
  useCookieLanguage(i18n);
  const [recentDeposits, setRecentDeposits] = useState<string>('All');
  const [cashAgentOptions, setCashAgentOptions] = useState<BankInfo[]>([]);
  const [uniquePaymentToOptions, setUniquePaymentToOptions] = useState<string[]>([]);
  const [cashAgentTo, setCashAgentTo] = useState<string>('');
  const [searchTerm, setSearchTerm] = useState<string>('');
  const [deposits, setDeposits] = useState<TransactionsHistoryResponse>({
    items: [],
    totalCount: 0,
  });
  const [showManualAddDepositForm, setShowManualAddDepositForm] = useState<boolean>(false);
  const [isLoading, setIsLoading] = useState<boolean>(false);

  const fetchDeposits = async () => {
    setIsLoading(true);
    try {
      const [fromDate, toDate] = calculateDateRange(recentDeposits);
      const params: TransactionsHistoryParams = {
        dateRange: [fromDate, toDate],
        status: 2,
        searchText: '',
        account: '',
        pageIndex: 1,
        pageSize: 100,
        transactionType: 1,
      };
      const depositsResponse = await transactionAPI.getTransactions(params);
      setUniquePaymentToOptions(filterPaymentTo(depositsResponse));
      setDeposits(depositsResponse);
    } catch (error) {
      console.error('Error fetching deposits:', error);
    } finally {
      setIsLoading(false);
    }
  };

  const filterPaymentTo = (deposits: TransactionsHistoryResponse | null) => {
    if (!deposits || !deposits.items) {
      return [];
    }
    const uniquePaymentToOptions = [...new Set(deposits.items.map((deposit) => deposit.paymentTo))];
    return uniquePaymentToOptions;
  };

  useEffect(() => {
    fetchDeposits();
  }, [recentDeposits]);

  useEffect(() => {
    const fetchBankInfo = async () => {
      try {
        const bankInfo = await transactionAPI.getCashAgentBankInfo(TransactionType.Deposit);
        setCashAgentOptions(bankInfo);
      } catch (error) {
        console.error('Error fetching bank info:', error);
      }
    };
    fetchBankInfo();
  }, []);

  const handleAddManualDeposit = () => {
    setShowManualAddDepositForm(true);
  };

  const handleCloseManualDepositForm = (isSuccess: boolean) => {
    setShowManualAddDepositForm(false);
    if (isSuccess) {
      fetchDeposits();
    }
  };

  const debouncedSetSearchTerm = useDebouncedCallback((value: string) => {
    setSearchTerm(value);
  }, 500);

  const handleSearchChange = (value: string) => {
    setSearchTerm(value);
    debouncedSetSearchTerm(value);
  };

  const filteredDeposits = useMemo(() => {
    if (!cashAgentTo && !searchTerm.trim()) {
      return deposits?.items || [];
    }

    let result = deposits?.items || [];
    if (cashAgentTo) {
      result = result.filter((deposit) => deposit.paymentTo.toString() === cashAgentTo);
    }

    if (searchTerm.trim()) {
      const term = searchTerm.toLowerCase().trim();
      result = result.filter(
        (deposit) =>
          deposit.username.toLowerCase().includes(term) ||
          deposit.transId.toString().toLowerCase().includes(term)
      );
    }

    return result;
  }, [deposits, cashAgentTo, searchTerm]);

  return (
    <>
      {showManualAddDepositForm ? (
        <ManualAddDepositForm
          cashAgentOptions={cashAgentOptions}
          onClose={() => handleCloseManualDepositForm(false)}
        />
      ) : (
        <div className=" py-[12px] flex h-full flex-col">
          <TitleComponent title={t('pendingDeposit')} />
          <div className="mb-[8px]">
            <ButtonOutlineComponent
              text={`+ ${t('addManualDeposit')}`}
              onClick={handleAddManualDeposit}
              className="min-w-[183px] [&>*]:min-w-[183px] [&>*]:inline-block"
            />
          </div>

          <Filter
            recentDeposits={recentDeposits}
            setRecentDeposits={setRecentDeposits}
            uniquePaymentToOptions={uniquePaymentToOptions}
            setCashAgentTo={setCashAgentTo}
            searchTerm={searchTerm}
            setSearchTerm={handleSearchChange}
          />

          {isLoading ? (
            
              <LoadingFullComponent />
             
          ) : (
            <DataGrid deposits={filteredDeposits}/>
          )}
        </div>
      )}
    </>
  );
};

export default PendingDeposit;

if (document.getElementById('root')) {
  createRoot(document.getElementById('root')!).render(
    <React.StrictMode>
      <PendingDeposit />
    </React.StrictMode>
  );
}
