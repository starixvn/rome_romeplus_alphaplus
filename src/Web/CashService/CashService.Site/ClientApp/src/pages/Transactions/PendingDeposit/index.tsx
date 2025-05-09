import React, { useState, useMemo, useEffect } from 'react';
import { createRoot } from 'react-dom/client';
import './PendingDepositsStyle.css';
import ManualAddDepositForm from './components/ManualAddDepositForm';
import Filter from './components/Filter';
import DataGrid from './components/DataGrid';
import { DepositAccountInfo } from '../../../services/interfaces/DepositAccountInfo';
import { pendingDepositService } from '../../../services/pendingDepositAPI';
import { useTranslation } from 'react-i18next';


const PendingDeposit: React.FC = () => {
  const { t } = useTranslation('pendingDeposit');
  const [recentDeposits, setRecentDeposits] = useState<string>('All');
  const [toFilter, setToFilter] = useState<string>('All');
  const [searchTerm, setSearchTerm] = useState<string>('');
  const [deposits, setDeposits] = useState<DepositAccountInfo[]>([]);
  const [showManualAddDepositForm, setShowManualAddDepositForm] = useState<boolean>(false);
  const [debouncedSearchTerm, setDebouncedSearchTerm] = useState(searchTerm);

  useEffect(() => {
    const fetchDeposits = async () => {
      const deposits = await pendingDepositService.getMemberBankAccounts();
      setDeposits(deposits);
    };
    fetchDeposits();
  }, []);

  const handleAddManualDeposit = () => {
    setShowManualAddDepositForm(true);
  };

  const handleCloseManualDepositForm = () => {
    setShowManualAddDepositForm(false);
  };

  const handleSubmitManualDeposit = (data: {
    memberLoginName: string;
    from: number;
    to: number;
    depositAmount: number;
    remarks: string;
    image?: File;
  }) => {

    const newDeposit: DepositAccountInfo = {
      id: deposits.length + 1,
      username: data.memberLoginName,
      loginName: data.memberLoginName,
      firstName: '',
      lastName: '',
      amount: data.depositAmount,
      currency: 'USD',
      from: data.from,
      to: data.to,
      referenceNo: data.remarks,
      dateAndTime: new Date(),  
      dateAndTimeStr: new Date().toLocaleString(),
    };

    setDeposits([...deposits, newDeposit]);
    setShowManualAddDepositForm(false);
  };

  useEffect(() => {
    const timer = setTimeout(() => {
      setDebouncedSearchTerm(searchTerm);
    }, 500);
    
    return () => clearTimeout(timer);
  }, [searchTerm]);
  
  const filteredDeposits = useMemo(() => {
    let result = [...deposits];
    if (recentDeposits === 'Last30Mins') {
      const thirtyMinsAgo = new Date();
      thirtyMinsAgo.setMinutes(thirtyMinsAgo.getMinutes() - 30);
      result = result.filter(deposit => {
        const depositDate = new Date(deposit.dateAndTime);
        return depositDate > thirtyMinsAgo;
      });
    } else if (recentDeposits === 'Last60Mins') {
      const sixtyMinsAgo = new Date();
      sixtyMinsAgo.setMinutes(sixtyMinsAgo.getMinutes() - 60);
      result = result.filter(deposit => {
        const depositDate = new Date(deposit.dateAndTime);
        return depositDate > sixtyMinsAgo;
      });
    }

    if (toFilter !== 'All') {
      result = result.filter(deposit => deposit.to.toString() === toFilter);
    }
    
    if (debouncedSearchTerm.trim()) {
      const term = debouncedSearchTerm.toLowerCase().trim();
      result = result.filter(deposit => 
        deposit.username.toLowerCase().includes(term) || 
        deposit.referenceNo.toLowerCase().includes(term)
      );
    }
    
    return result;
  }, [deposits, recentDeposits, toFilter, debouncedSearchTerm]);

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
            toFilter={toFilter}
            setToFilter={setToFilter}
            searchTerm={searchTerm}
            setSearchTerm={setSearchTerm}
          />

          <DataGrid deposits={filteredDeposits} />
        </div>
      </div>

      {showManualAddDepositForm && (
        <ManualAddDepositForm
          onClose={handleCloseManualDepositForm}
          onSubmit={handleSubmitManualDeposit}
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
