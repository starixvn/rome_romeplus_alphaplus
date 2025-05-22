import React, { useState, useMemo, useEffect } from 'react';
import { createRoot } from 'react-dom/client';
import ManualAddWithdrawalForm from './components/ManualAddWithdrawalForm.tsx';
import Filter from './components/Filter.tsx';
import DataGrid from './components/DataGrid.tsx';
import { useTranslation } from 'react-i18next';
import i18n from '../../../i18n/i18n-pendingWithdrawal';
import { useCookieLanguage } from '../../../hooks/useCookieLanguage';
import '../../../styles/common.css';
import '../../../styles/global.scss';
import {
  TransactionsHistoryParams,
  TransactionsHistoryResponse,
} from '../../../services/interfaces/Transactions/TransactionHistory.ts';
import { transactionAPI } from '../../../services/transactionsHistoryAPI.ts';
import { TransactionType } from '../../../common/commonFunctions.tsx';
import Loading from '../../../components/LoadingComponent';
import TitleComponent from '../../../components/TitleComponent';
import { ButtonOutlineComponent } from '../../../components/ButtonComponents';

const PendingWithdrawal: React.FC = () => {
  const { t } = useTranslation('pendingWithdrawal');
  useCookieLanguage(i18n);
  const [recentWithdrawals, setRecentWithdrawals] = useState<string>('All');
  const [cashAgentTo, setCashAgentTo] = useState<string>('');
  const [searchTerm, setSearchTerm] = useState<string>('');
  const [withdrawals, setWithdrawals] = useState<TransactionsHistoryResponse>({
    items: [],
    totalCount: 0,
  });
  const [showManualAddWithdrawalForm, setShowManualAddWithdrawalForm] = useState<boolean>(false);
  const [debouncedSearchTerm, setDebouncedSearchTerm] = useState(searchTerm);
  const [isLoading, setIsLoading] = useState<boolean>(false);

  const calculateDateRange = (): [string, string] => {
    const today = new Date();
    let startDate = new Date();

    switch (recentWithdrawals) {
      case 'Today':
        startDate.setHours(0, 0, 0, 0);
        break;
      case 'Yesterday':
        startDate.setDate(today.getDate() - 1);
        startDate.setHours(0, 0, 0, 0);
        break;
      case 'Last7Days':
        startDate.setDate(today.getDate() - 7);
        startDate.setHours(0, 0, 0, 0);
        break;
      default:
        return ['', ''];
    }
    return [startDate.toISOString().split('T')[0], today.toISOString().split('T')[0]];
  };

  const fetchWithdrawals = async () => {
    setIsLoading(true);
    try {
      const [fromDate, toDate] = calculateDateRange();
      const params: TransactionsHistoryParams = {
        dateRange: [fromDate, toDate],
        status: 2,
        searchText: '',
        account: '',
        pageIndex: 1,
        pageSize: 100,
        transactionType: TransactionType.Withdrawal,
      };
      const withdrawalsResponse = await transactionAPI.getTransactions(params);
      setWithdrawals(withdrawalsResponse);
    } catch (error) {
      console.error('Error fetching withdrawals:', error);
    } finally {
      setIsLoading(false);
    }
  };

  useEffect(() => {
    fetchWithdrawals();
  }, [recentWithdrawals]);

  const handleAddManualWithdrawal = () => {
    setShowManualAddWithdrawalForm(true);
  };

  const handleCloseManualWithdrawalForm = (isSuccess: boolean) => {
    setShowManualAddWithdrawalForm(false);
    if (isSuccess) {
      fetchWithdrawals();
    }
  };

  useEffect(() => {
    const timer = setTimeout(() => {
      setDebouncedSearchTerm(searchTerm);
    }, 500);

    return () => clearTimeout(timer);
  }, [searchTerm]);

  const filteredWithdrawals = useMemo(() => {
    let result = [...withdrawals.items];
    if (cashAgentTo !== '') {
      result = result.filter((withdrawal) => withdrawal.paymentTo.toString() === cashAgentTo);
    }

    if (debouncedSearchTerm.trim()) {
      const term = debouncedSearchTerm.toLowerCase().trim();
      result = result.filter(
        (withdrawal) =>
          withdrawal.username.toLowerCase().includes(term) ||
          withdrawal.transId.toString().toLowerCase().includes(term)
      );
    }

    return result;
  }, [withdrawals, cashAgentTo, debouncedSearchTerm]);

  return (
    <>
      {showManualAddWithdrawalForm ? (
        <ManualAddWithdrawalForm onClose={() => handleCloseManualWithdrawalForm(false)} />
      ) : (
        <div className="py-[12px] flex h-full flex-col">
          <TitleComponent title={t('pendingWithdrawal')} />
          <div className="mb-[8px]">
            <ButtonOutlineComponent
              text={`+${t('addManualWithdrawal')}`}
              onClick={handleAddManualWithdrawal}
              className="min-w-[183px] [&>*]:min-w-[183px] [&>*]:inline-block"
            />
          </div>

          <Filter
            recentWithdrawals={recentWithdrawals}
            setRecentWithdrawals={setRecentWithdrawals}
            toFilter={cashAgentTo}
            setToFilter={setCashAgentTo}
            searchTerm={searchTerm}
            setSearchTerm={setSearchTerm}
          />

          {isLoading ? (
            <div className="flex justify-center items-center py-8">
              <Loading />
            </div>
          ) : (
            <DataGrid withdrawals={filteredWithdrawals} />
          )}
        </div>
      )}
    </>
  );
};

export default PendingWithdrawal;

if (document.getElementById('root')) {
  createRoot(document.getElementById('root')!).render(
    <React.StrictMode>
      <PendingWithdrawal />
    </React.StrictMode>
  );
}
