import React, { useState } from 'react';
import { createRoot } from 'react-dom/client';
import ManualAddWithdrawalForm from './components/ManualAddWithdrawalForm.tsx';
import Filter from './components/Filter.tsx';
import DataGrid from './components/DataGrid.tsx';
import {
  AddManualWithdrawalAccountInfo,
  WithdrawalAccountInfo,
} from '../../../services/interfaces/WithdrawalAccountInfo';
import { getBankWithdrawalAccounts } from '../../../services/pendingWithdrawalAPI';
import { useTranslation } from 'react-i18next';
import i18n from '../../../i18n/i18n-pendingWithdrawal';
import { useCookieLanguage } from '../../../hooks/useCookieLanguage';
import '../../../styles/common.css';
import '../../../styles/global.scss';

const PendingWithdrawal: React.FC = () => {
  const { t } = useTranslation('pendingWithdrawal');
  useCookieLanguage(i18n);
  const [recentWithdrawals, setRecentWithdrawals] = useState<string>('All');
  const [toFilter, setToFilter] = useState<string>('All');
  const [searchTerm, setSearchTerm] = useState<string>('');
  const [withdrawals, setWithdrawals] = useState<WithdrawalAccountInfo[]>([]);
  const [showManualAddWithdrawalForm, setShowManualAddWithdrawalForm] = useState<boolean>(false);

  React.useEffect(() => {
    const fetchWithdrawals = async () => {
      const withdrawals = await getBankWithdrawalAccounts();
      setWithdrawals(withdrawals);
    };
    fetchWithdrawals();
  }, []);

  const handleAddManualWithdrawal = () => {
    setShowManualAddWithdrawalForm(true);
  };

  const handleCloseManualWithdrawalForm = () => {
    setShowManualAddWithdrawalForm(false);
  };

  const handleSubmitManualWithdrawal = (data: {
    custId: number;
    from: string;
    to: string;
    withdrawalAmount: number;
    remarks: string;
    image?: File;
  }) => {
    const manualWithdrawalData: AddManualWithdrawalAccountInfo = {
      custId: data.custId,
      from: data.from,
      to: data.to,
      amount: data.withdrawalAmount,
      remarks: data.remarks,
      image: data.image,
      providerId: isNaN(parseInt(data.from)) ? 0 : parseInt(data.from),
    };
    console.log(manualWithdrawalData);
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
        <ManualAddWithdrawalForm
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
