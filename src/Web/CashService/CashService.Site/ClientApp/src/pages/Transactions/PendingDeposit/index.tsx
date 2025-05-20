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
import { ButtonOutline } from '../../../components/ButtonComponents';

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
  const [isLoading, setIsLoading] = useState<boolean>(false);

  const calculateDateRange = (): [string, string] => {
    const today = new Date();
    let startDate = new Date();

    switch (recentDeposits) {
      case 'Last30Mins':
        startDate.setMinutes(today.getMinutes() - 30);
        break;
      case 'Last60Mins':
        startDate.setMinutes(today.getMinutes() - 60);
        break;
      default:
        return ['', ''];
    }
    return [startDate.toISOString().split('T')[0], today.toISOString().split('T')[0]];
  };

  const fetchDeposits = async () => {
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
        transactionType: 1,
      };
      const depositsResponse = await transactionAPI.getTransactions(params);
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

  useEffect(() => {
    const timer = setTimeout(() => {
      setDebouncedSearchTerm(searchTerm);
    }, 500);

    return () => clearTimeout(timer);
  }, [searchTerm]);

  const filteredDeposits = useMemo(() => {
    let result = [...deposits.items];
    if (cashAgentTo !== '') {
      console.log('cashAgentTo', cashAgentTo);
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
          <ButtonOutline 
            text={`+ ${t('addManualDeposit')}`}
            onClick={handleAddManualDeposit}
            className="min-w-[183px] [&>*]:min-w-[183px] [&>*]:inline-block" 
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
          <div className='p-[4px]'>
            <DataGrid deposits={filteredDeposits} />
         
          </div>
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
