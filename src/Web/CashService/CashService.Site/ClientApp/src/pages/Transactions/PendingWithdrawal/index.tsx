import React, { useState } from 'react';
import { createRoot } from 'react-dom/client';
import './PendingWithdrawalsStyle.css';
import ManualWithdrawalForm from './components/ManualWithdrawalForm.tsx';
import Filter from './components/Filter.tsx';
import DataGrid from './components/DataGrid.tsx';
import { WithdrawalAccountInfo } from '../../../services/interfaces/WithdrawalAccountInfo';
import { pendingWithdrawalService } from '../../../services/pendingWithdrawalAPI';
import { useTranslation } from 'react-i18next';
import '../../../i18n/i18n';

const PendingWithdrawal: React.FC = () => {
  const { t } = useTranslation('pendingWithdrawal');
  const [recentWithdrawals, setRecentWithdrawals] = useState<string>('All');
  const [toFilter, setToFilter] = useState<string>('All');
  const [searchTerm, setSearchTerm] = useState<string>('');
  const [withdrawals, setWithdrawals] = useState<WithdrawalAccountInfo[]>([]);
  const [showManualWithdrawalForm, setShowManualWithdrawalForm] = useState<boolean>(false);

  React.useEffect(() => {
    const fetchWithdrawals = async () => {
      const withdrawals = await pendingWithdrawalService.getBankWithdrawalAccounts();
      setWithdrawals(withdrawals);
    };
    fetchWithdrawals();
  }, []);

  const handleAddManualWithdrawal = () => {
    setShowManualWithdrawalForm(true);
  };

  const handleCloseManualWithdrawalForm = () => {
    setShowManualWithdrawalForm(false);
  };

  const handleSubmitManualWithdrawal = (data: {
    memberLoginName: string;
    from: number;
    to: number;
    withdrawalAmount: number;
    remarks: string;
    image?: File;
  }) => {

    const newWithdrawal: WithdrawalAccountInfo = {
      id: withdrawals.length + 1,
      username: data.memberLoginName,
      loginName: data.memberLoginName,
      firstName: '',
      lastName: '',
      amount: data.withdrawalAmount,
      currency: 'USD',
      from: data.from,
      to: data.to,
      referenceNo: data.remarks,
      dateAndTime: new Date(),
    };

    setWithdrawals([...withdrawals, newWithdrawal]);
    setShowManualWithdrawalForm(false);
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

      {showManualWithdrawalForm && (
        <ManualWithdrawalForm
          onClose={handleCloseManualWithdrawalForm}
          onSubmit={handleSubmitManualWithdrawal}
        />
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