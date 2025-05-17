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
import Loading from '../../../components/LoadingComponent';
import TitleComponent from '../../../components/TitleComponent';
import { ButtonComponent } from '../../../components/ButtonComponents';

const PendingDeposit: React.FC = () => {
  const { t } = useTranslation('pendingDeposit');
  useCookieLanguage(i18n);
  const [recentDeposits, setRecentDeposits] = useState<string>('All');
  const [cashAgentOptions, setCashAgentOptions] = useState<BankInfo[]>([]);
  const [cashAgentTo, setCashAgentTo] = useState<string>('All');
  const [searchTerm, setSearchTerm] = useState<string>('');
  const [deposits, setDeposits] = useState<TransactionsHistoryResponse>({
    items: [],
    totalCount: 0,
  });
  const [showManualAddDepositForm, setShowManualAddDepositForm] = useState<boolean>(false);
  const [debouncedSearchTerm, setDebouncedSearchTerm] = useState(searchTerm);
  const [isLoading, setIsLoading] = useState<boolean>(false);

  useEffect(() => {
    const fetchData = async () => {
      setIsLoading(true);
      try {
        const [depositsResponse, bankInfo] = await Promise.all([
          (async () => {
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
            return await transactionAPI.getTransactions(params);
          })(),
          transactionAPI.getCashAgentBankInfo(TransactionType.Deposit)
        ]);

        setDeposits(depositsResponse);
        setCashAgentOptions(bankInfo);
      } catch (error) {
        console.error('Error fetching data:', error);
      } finally {
        setIsLoading(false);
      }
    };

    fetchData();
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

  console.log('filteredDeposits', filteredDeposits);
  console.log('deposits.items', deposits.items);

  return (
    <div className="min-h-screen bg-gray-50">
      <div className="mx-auto space-y-6">
        <div className="bg-white rounded-lg shadow-sm p-2">
          <TitleComponent title={t('pendingDeposit')} />          
          <div className="mb-[8px]">
            <ButtonComponent
              type="cancel"
              style="medium"
              text={`+${t('addManualDeposit')}`}
              onClick={handleAddManualDeposit}
              className="w-[183px] shadow-[0_1px_5px_#0003,0_2px_2px_#00000024,0_3px_1px_-2px_#0000001f]"
            />
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

          {isLoading ? (
            <div className="flex justify-center items-center py-8">
              <Loading />
            </div>
          ) : (
            <DataGrid deposits={filteredDeposits} />
          )}
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
