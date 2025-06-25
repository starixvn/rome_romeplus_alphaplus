import React, { useState, useMemo, useEffect } from 'react';
import { createRoot } from 'react-dom/client';
import { useDebouncedCallback } from '../../../hooks/useDebouncedCallback';
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
import { calculateDateRange, TransactionType } from '../../../utils/commonFunctions.tsx';
import { LoadingFullComponent } from '../../../components/LoadingComponent';
import TitleComponent from '../../../components/TitleComponents.tsx';
import { ButtonFullComponent } from '../../../components/ButtonComponents';

const PendingWithdrawal: React.FC = () => {
  const { t } = useTranslation('pendingWithdrawal');
  useCookieLanguage(i18n);
  const [recentWithdrawals, setRecentWithdrawals] = useState<string>('All');
  const [searchTerm, setSearchTerm] = useState<string>('');
  const [withdrawals, setWithdrawals] = useState<TransactionsHistoryResponse>({
    items: [],
    totalCount: 0,
  });
  const [showManualAddWithdrawalForm, setShowManualAddWithdrawalForm] = useState<boolean>(false);
  const [isLoading, setIsLoading] = useState<boolean>(false);

  const fetchWithdrawals = async () => {
    setIsLoading(true);
    try {
      const [fromDate, toDate] = calculateDateRange(recentWithdrawals);
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

  const debouncedSetSearchTerm = useDebouncedCallback((value: string) => {
    setSearchTerm(value);
  }, 500);

  const handleSearchChange = (value: string) => {
    setSearchTerm(value);
    debouncedSetSearchTerm(value);
  };

  const filteredWithdrawals = useMemo(() => {
    if (!searchTerm.trim()) {
      return withdrawals?.items || [];
    }

    let result = withdrawals?.items || [];

    if (searchTerm.trim()) {
      const term = searchTerm.toLowerCase().trim();
      result = result.filter(
        (withdrawal) =>
          withdrawal.username.toLowerCase().includes(term) ||
          withdrawal.transId.toString().toLowerCase().includes(term)
      );
    }

    return result;
  }, [withdrawals, searchTerm]);

  return (
    <>
      {showManualAddWithdrawalForm ? (
        <ManualAddWithdrawalForm onClose={() => handleCloseManualWithdrawalForm(false)} />
      ) : (
        <div className="flex h-full flex-col">
          <TitleComponent title={t('pendingWithdrawal')} />
          <div className="mb-[8px]">
            <ButtonFullComponent
              text={`${t('addManualWithdrawal')}`}
              onClick={handleAddManualWithdrawal}
              className="min-w-[183px] [&>*]:min-w-[183px] [&>*]:inline-block"
            />
          </div>

          <Filter
            recentWithdrawals={recentWithdrawals}
            setRecentWithdrawals={setRecentWithdrawals}
            searchTerm={searchTerm}
            setSearchTerm={handleSearchChange}
          />

          {isLoading ? (
            <div className="flex items-center justify-center h-full">
              <LoadingFullComponent />
            </div>
          ) : (
            //console.log(filteredWithdrawals),
            <DataGrid withdrawals={filteredWithdrawals} reloadGrid={fetchWithdrawals} />
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
