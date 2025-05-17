import React, { useState, useMemo, useEffect } from 'react';
import { createRoot } from 'react-dom/client';
import ManualAddDepositForm from './components/ManualAddDepositForm';
import Filter from './components/Filter';
import DataGrid from './components/DataGrid';
import { useTranslation } from 'react-i18next';
import i18n from '../../../i18n/i18n-pendingDeposit';
import { useCookieLanguage } from '../../../hooks/useCookieLanguage';
import '../../../styles/common.css';
import '../../../styles/global.scss';
import { BankInfo } from '../../../services/interfaces/Transactions/BankInfo';
import { TransactionType } from '../../../common/commonFunctions';
import { transactionAPI } from '../../../services/transactionsHistoryAPI';
import {
  TransactionsHistoryParams,
  TransactionsHistoryResponse,
} from '../../../services/interfaces/Transactions/TransactionHistory';

const PendingDeposit: React.FC = () => {
  const { t } = useTranslation('pendingDeposit');
  useCookieLanguage(i18n);
  const [recentDeposits, setRecentDeposits] = useState<string>('All');
  const [cashAgentOptions, setCashAgentOptions] = useState<BankInfo[]>([]);
  const [cashAgentTo, setCashAgentTo] = useState<string>('');
  const [searchTerm, setSearchTerm] = useState<string>('');
  const [deposits, setDeposits] = useState<TransactionsHistoryResponse>({
    items: [],
    totalCount: 0,
  });
  const [showManualAddDepositForm, setShowManualAddDepositForm] = useState<boolean>(false);
  const [debouncedSearchTerm, setDebouncedSearchTerm] = useState(searchTerm);

  useEffect(() => {
    const fetchDeposits = async () => {
      const today = new Date();
      let startDate = new Date();
      
      if (recentDeposits === 'Last30Mins') {
        startDate.setMinutes(today.getMinutes() - 30);
      } else if (recentDeposits === 'Last60Mins') {
        startDate.setMinutes(today.getMinutes() - 60);
      } else {
        startDate.setDate(today.getDate() - 7);
      }

      const params: TransactionsHistoryParams = {
        dateRange: [startDate.toLocaleDateString('en-US'), today.toLocaleDateString('en-US')],
        status: 2,
        searchText: '',
        account: '',
        pageIndex: 1,
        pageSize: 100,
        transactionType: 1,
      };
      const response = await transactionAPI.getTransactions(params);
      setDeposits(response);
      console.log('response', response);
    };
    const fetchCashAgentBankInfo = async () => {
      const bankInfo = await transactionAPI.getCashAgentBankInfo(TransactionType.Deposit);
      setCashAgentOptions(bankInfo);
    };

    fetchDeposits();
    fetchCashAgentBankInfo();
  }, [recentDeposits]);

  const handleAddManualDeposit = () => {
    setShowManualAddDepositForm(true);
  };

  const handleCloseManualDepositForm = () => {
    setShowManualAddDepositForm(false);
  };

  useEffect(() => {
    const timer = setTimeout(() => {
      setDebouncedSearchTerm(searchTerm);
    }, 500);

    return () => clearTimeout(timer);
  }, [searchTerm]);

  const filteredDeposits = useMemo(() => {
    let result = [...deposits.items];
    if (cashAgentTo !== 'All') {
      result = result.filter(deposit => deposit.paymentTo.toString() === cashAgentTo);
    }

    if (debouncedSearchTerm.trim()) {
      const term = debouncedSearchTerm.toLowerCase().trim();
      result = result.filter(
        (deposit) =>
          deposit.username.toLowerCase().includes(term) ||
          deposit.transId.toString().toLowerCase().includes(term)
      );
    }

    return result;
  }, [deposits, cashAgentTo, debouncedSearchTerm]);

  return (
    <div className="min-h-screen bg-gray-50">
      <div className="mx-auto space-y-6">
        <div className="bg-white rounded-lg shadow-sm p-2">
          <h1 className="text-xl font-semibold mb-6">{t('pendingDeposit')}</h1>

          <div className="mb-6">
            <button
              onClick={handleAddManualDeposit}
              className="bg-[#1e4d8c] text-white px-8 py-2 rounded-lg text-sm font-medium hover:bg-[#173d70] transition-colors flex items-center cursor-pointer"
            >
              <span className="mr-1">+</span> {t('addManualDeposit')}
            </button>
          </div>

          <Filter
            recentDeposits={recentDeposits}
            setRecentDeposits={setRecentDeposits}
            cashAgentOptions={cashAgentOptions}
            cashAgentTo={cashAgentTo}
            setCashAgentTo={setCashAgentTo}
            searchTerm={searchTerm}
            setSearchTerm={setSearchTerm}
          />

          <DataGrid deposits={filteredDeposits} />
        </div>
      </div>

      {showManualAddDepositForm && (
        <ManualAddDepositForm
          cashAgentOptions={cashAgentOptions}
          onClose={handleCloseManualDepositForm}
        />
      )}
    </div>
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
