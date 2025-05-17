import React, { useState } from 'react';
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
  TransactionItem,
} from '../../../services/interfaces/Transactions/TransactionHistory.ts';
import { transactionAPI } from '../../../services/transactionsHistoryAPI.ts';
import { TransactionType } from '../../../common/commonFunctions.tsx';

const PendingWithdrawal: React.FC = () => {
  const { t } = useTranslation('pendingWithdrawal');
  useCookieLanguage(i18n);
  const [recentWithdrawals, setRecentWithdrawals] = useState<string>('All');
  const [toFilter, setToFilter] = useState<string>('All');
  const [searchTerm, setSearchTerm] = useState<string>('');
  const [withdrawals, setWithdrawals] = useState<TransactionItem[]>([]);
  const [showManualAddWithdrawalForm, setShowManualAddWithdrawalForm] = useState<boolean>(false);

  React.useEffect(() => {
    const fetchWithdrawals = async () => {
      try {
        const [fromDate, toDate] = calculateDateRange();
        const params: TransactionsHistoryParams = {
          dateRange: [fromDate, toDate],
          status: 2,
          searchText: searchTerm,
          account: toFilter !== 'All' ? toFilter : '',
          pageIndex: 1,
          pageSize: 100,
          transactionType: TransactionType.Withdrawal,
        };
        const response = await transactionAPI.getTransactions(params);
        setWithdrawals(response.items);
      } catch (error) {
        console.error('Error fetching withdrawals:', error);
      }
    };
    fetchWithdrawals();
  }, [recentWithdrawals, toFilter, searchTerm]);

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

  const handleAddManualWithdrawal = () => {
    setShowManualAddWithdrawalForm(true);
  };

  const handleCloseManualWithdrawalForm = () => {
    setShowManualAddWithdrawalForm(false);
  };

  return (
    <div className="min-h-screen bg-gray-50">
      <div className="mx-auto space-y-6">
        <div className="bg-white rounded-lg shadow-sm p-2">
          <h1 className="text-xl font-semibold mb-6">{t('pendingWithdrawal')}</h1>

          <div className="mb-6">
            <button
              onClick={handleAddManualWithdrawal}
              className="bg-[#1e4d8c] text-white px-8 py-2 rounded-lg text-sm font-medium hover:bg-[#173d70] transition-colors flex items-center cursor-pointer"
            >
              <span className="mr-1">+</span> {t('addManualWithdrawal')}
            </button>
          </div>

          <Filter
            recentWithdrawals={recentWithdrawals}
            setRecentWithdrawals={setRecentWithdrawals}
            toFilter={toFilter}
            setToFilter={setToFilter}
            searchTerm={searchTerm}
            setSearchTerm={setSearchTerm}
          />

          <DataGrid withdrawals={withdrawals} />
        </div>
      </div>

      {showManualAddWithdrawalForm && (
        <ManualAddWithdrawalForm onClose={handleCloseManualWithdrawalForm} />
      )}
    </div>
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
