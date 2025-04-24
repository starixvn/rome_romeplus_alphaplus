import React, { useState } from 'react';
import { createRoot } from 'react-dom/client';
import './PendingDepositsStyle.css';
import ManualDepositForm from './components/ManualDepositForm';
import Filter from './components/Filter';
import DataGrid from './components/DataGrid';
import { DepositAccountInfo } from '../../../services/interfaces/DepositAccountInfo';
import { pendingDepositService } from '../../../services/pendingDepositAPI';

const PendingDeposit: React.FC = () => {
  const [recentDeposits, setRecentDeposits] = useState<string>('All');
  const [toFilter, setToFilter] = useState<string>('All');
  const [searchTerm, setSearchTerm] = useState<string>('');
  const [deposits, setDeposits] = useState<DepositAccountInfo[]>([]);
  const [showManualDepositForm, setShowManualDepositForm] = useState<boolean>(false);

  React.useEffect(() => {
    const fetchDeposits = async () => {
      const deposits = await pendingDepositService.getMemberBankAccounts();
      setDeposits(deposits);
    };
    fetchDeposits();
  }, []);

  const handleAddManualDeposit = () => {
    setShowManualDepositForm(true);
  };

  const handleCloseManualDepositForm = () => {
    setShowManualDepositForm(false);
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
    };

    setDeposits([...deposits, newDeposit]);
    setShowManualDepositForm(false);
  };

  return (
    <div className="min-h-screen bg-gray-50">
      <div className="mx-auto space-y-6">
        <div className="bg-white rounded-lg shadow-sm p-2">
          <h1 className="text-xl font-semibold mb-6">Pending Deposit</h1>

          <div className="mb-6">
            <button
              onClick={handleAddManualDeposit}
              className="bg-[#1e4d8c] text-white px-8 py-2 rounded-lg text-sm font-medium hover:bg-[#173d70] transition-colors flex items-center cursor-pointer"
            >
              <span className="mr-1">+</span> Add Manual Deposit
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

          <DataGrid deposits={deposits} />
        </div>
      </div>

      {showManualDepositForm && (
        <ManualDepositForm
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
