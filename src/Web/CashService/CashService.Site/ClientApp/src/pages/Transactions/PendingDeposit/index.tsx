import React, { useState, useMemo, useEffect } from 'react';
import { createRoot } from 'react-dom/client';
import { useDebouncedCallback } from '../../../hooks/useDebouncedCallback';
import ManualAddDepositForm from './components/ManualAddDepositForm';
import Filter, { TimeFilterOptions } from './components/Filter';
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
import { TitleComponent } from '../../../components/Title';
import { ButtonComponent } from '../../../components/Buttons/index';
import { LoadingFullComponent } from '../../../components/LoadingComponent';

const useDeposits = (recentDeposits: string) => {
  const [deposits, setDeposits] = useState<TransactionsHistoryResponse>({
    items: [],
    totalCount: 0,
  });
  const [uniquePaymentToOptions, setUniquePaymentToOptions] = useState<string[]>([]);
  const [isLoading, setIsLoading] = useState<boolean>(false);

  const filterPaymentTo = (deposits: TransactionsHistoryResponse | null): string[] => {
    if (!deposits?.items) return [];
    return [...new Set(deposits.items.map((deposit) => deposit.paymentTo))];
  };

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

  useEffect(() => {
    fetchDeposits();
  }, [recentDeposits]);

  return { deposits, uniquePaymentToOptions, isLoading, fetchDeposits };
};

const useBankInfo = () => {
  const [cashAgentOptions, setCashAgentOptions] = useState<BankInfo[]>([]);

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

  return { cashAgentOptions };
};

const useSearch = () => {
  const [searchTerm, setSearchTerm] = useState<string>('');

  const debouncedSetSearchTerm = useDebouncedCallback((value: string) => {
    setSearchTerm(value);
  }, 500);

  const handleSearchChange = (value: string) => {
    setSearchTerm(value);
    debouncedSetSearchTerm(value);
  };

  return { searchTerm, setSearchTerm, handleSearchChange };
};

const PendingDeposit: React.FC = () => {
  const { t } = useTranslation('pendingDeposit');
  useCookieLanguage(i18n);

  const [showManualAddDepositForm, setShowManualAddDepositForm] = useState<boolean>(false);
  const [recentDeposits, setRecentDeposits] = useState<TimeFilterOptions>(TimeFilterOptions.All);
  const [cashAgentTo, setCashAgentTo] = useState<string>('');
  const [isCashServiceEnabled, setIsCashServiceEnabled] = useState(false);

  useEffect(() => {
    const isCashServiceEnabled = document
      .getElementById('IsCashServiceEnabled')
      ?.getAttribute('value');
    setIsCashServiceEnabled(isCashServiceEnabled === '1');
  }, []);

  const { deposits, uniquePaymentToOptions, isLoading, fetchDeposits } =
    useDeposits(recentDeposits);
  const { cashAgentOptions } = useBankInfo();
  const { searchTerm, handleSearchChange } = useSearch();

  const handleAddManualDeposit = () => setShowManualAddDepositForm(true);

  const handleCloseManualDepositForm = () => {
    setShowManualAddDepositForm(false);
    fetchDeposits();
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
          onClose={handleCloseManualDepositForm}
          isCashServiceEnabled={isCashServiceEnabled}
        />
      ) : (
        <div className="flex h-full flex-col">
          <TitleComponent type="main" text={t('pendingDeposit')} />
          <div className="mb-[8px]">
            <ButtonComponent
              variant="full"
              text={`${t('addManualDeposit')}`}
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
            <div className="flex items-center justify-center h-full">
              <LoadingFullComponent />
            </div>
          ) : (
            <DataGrid
              deposits={filteredDeposits}
              reloadGrid={fetchDeposits}
              isCashServiceEnabled={isCashServiceEnabled}
            />
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
