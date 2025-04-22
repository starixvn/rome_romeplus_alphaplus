import React, { useState } from 'react';
import { createRoot } from 'react-dom/client';
import './PendingDepositsStyle.css'
import ManualDepositForm from './components/ManualDepositForm';

interface DepositData {
  id: number;
  username: string;
  loginName: string;
  firstName: string;
  lastName: string;
  amount: number;
  currency: string;
  from: number;
  to: number;
  referenceNo: string;
  dateTime: string;
}

const PendingDeposit: React.FC = () => {
  const [recentDeposits, setRecentDeposits] = useState<string>('All');
  const [toFilter, setToFilter] = useState<string>('All');
  const [searchTerm, setSearchTerm] = useState<string>('');
  const [deposits, setDeposits] = useState<DepositData[]>([]);
  const [showManualDepositForm, setShowManualDepositForm] = useState<boolean>(false);

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
    // Add the new deposit to the list
    const newDeposit: DepositData = {
      id: deposits.length + 1,
      username: data.memberLoginName,
      loginName: data.memberLoginName, // Using username as login name for simplicity
      firstName: "", // No firstName in the form data
      lastName: "", // No lastName in the form data
      amount: data.depositAmount,
      currency: "USD", // Default currency
      from: data.from,
      to: data.to,
      referenceNo: data.remarks,
      dateTime: new Date().toLocaleString(),
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
              className="bg-[#1e4d8c] text-white px-8 py-2 rounded-lg text-sm font-medium hover:bg-[#173d70] transition-colors flex items-center"
            >
              <span className="mr-1">+</span> Add Manual Deposit
            </button>
          </div>
          
          <div className="flex flex-wrap gap-6 mb-6">
            <div className="w-full md:w-48">
              <label className="block text-sm font-medium text-gray-700 mb-1">
                Recent Deposits
              </label>
              <div className="select-wrapper relative">
                <select
                  value={recentDeposits}
                  onChange={(e) => setRecentDeposits(e.target.value)}
                  className="appearance-none block w-full bg-white border border-gray-300 px-3 py-1.5 pr-8 rounded text-gray-900 focus:outline-none focus:ring-blue-500 focus:border-blue-500 sm:text-sm h-9"
                >
                  <option value="All">All</option>
                  <option value="Today">Today</option>
                  <option value="Yesterday">Yesterday</option>
                  <option value="Last7Days">Last 7 Days</option>
                </select>
                <div className="pointer-events-none absolute inset-y-0 right-0 flex items-center px-2 text-gray-700">
                  <svg className="h-4 w-4 fill-current" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 20 20">
                    <path d="M9.293 12.95l.707.707L15.657 8l-1.414-1.414L10 10.828 5.757 6.586 4.343 8z" />
                  </svg>
                </div>
              </div>
            </div>
            
            <div className="w-full md:w-48">
              <label className="block text-sm font-medium text-gray-700 mb-1">
                To
              </label>
              <div className="select-wrapper relative">
                <select
                  value={toFilter}
                  onChange={(e) => setToFilter(e.target.value)}
                  className="appearance-none block w-full bg-white border border-gray-300 px-3 py-1.5 pr-8 rounded text-gray-900 focus:outline-none focus:ring-blue-500 focus:border-blue-500 sm:text-sm h-9"
                >
                  <option value="All">All</option>
                  <option value="Bank1">Bank 1</option>
                  <option value="Bank2">Bank 2</option>
                  <option value="Wallet1">Wallet 1</option>
                </select>
                <div className="pointer-events-none absolute inset-y-0 right-0 flex items-center px-2 text-gray-700">
                  <svg className="h-4 w-4 fill-current" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 20 20">
                    <path d="M9.293 12.95l.707.707L15.657 8l-1.414-1.414L10 10.828 5.757 6.586 4.343 8z" />
                  </svg>
                </div>
              </div>
            </div>
            
            <div className="w-full md:w-auto ml-auto">
              <label className="block text-sm font-medium text-gray-700 mb-1 opacity-0">
                Search
              </label>
              <div className="relative">
                <div className="pointer-events-none absolute inset-y-0 left-0 flex items-center pl-3">
                  <svg className="h-5 w-5 text-gray-400" viewBox="0 0 20 20" fill="currentColor">
                    <path fillRule="evenodd" d="M9 3.5a5.5 5.5 0 100 11 5.5 5.5 0 000-11zM2 9a7 7 0 1112.452 4.391l3.328 3.329a.75.75 0 11-1.06 1.06l-3.329-3.328A7 7 0 012 9z" clipRule="evenodd" />
                  </svg>
                </div>
                <input
                  type="text"
                  value={searchTerm}
                  onChange={(e) => setSearchTerm(e.target.value)}
                  className="block w-full rounded-md border border-gray-300 py-1.5 pl-10 pr-3 text-gray-900 placeholder-gray-500 focus:border-blue-500 focus:outline-none focus:ring-blue-500 sm:text-sm h-9"
                  placeholder="Username/Reference No."
                />
              </div>
            </div>
          </div>
          
          <div className="overflow-x-auto">
            <table className="min-w-full border-collapse border border-gray-300">
              <thead>
                <tr>
                  <th scope="col" className="px-1 py-0.5 text-center text-sm font-semibold text-gray-800 border border-gray-300">#</th>
                  <th scope="col" className="px-1 py-0.5 text-center text-sm font-semibold text-gray-800 border border-gray-300">Username</th>
                  <th scope="col" className="px-1 py-0.5 text-center text-sm font-semibold text-gray-800 border border-gray-300">Login Name</th>
                  <th scope="col" className="px-1 py-0.5 text-center text-sm font-semibold text-gray-800 border border-gray-300">
                    <div>First Name</div>
                    <div>Last Name</div>
                  </th>
                  <th scope="col" className="px-1 py-0.5 text-center text-sm font-semibold text-gray-800 border border-gray-300">Amount</th>
                  <th scope="col" className="px-1 py-0.5 text-center text-sm font-semibold text-gray-800 border border-gray-300">Currency</th>
                  <th scope="col" className="px-1 py-0.5 text-center text-sm font-semibold text-gray-800 border border-gray-300">From</th>
                  <th scope="col" className="px-1 py-0.5 text-center text-sm font-semibold text-gray-800 border border-gray-300">To</th>
                  <th scope="col" className="px-1 py-0.5 text-center text-sm font-semibold text-gray-800 border border-gray-300">Reference No.</th>
                  <th scope="col" className="px-1 py-0.5 text-center text-sm font-semibold text-gray-800 border border-gray-300">Date and Time</th>
                  <th scope="col" className="px-1 py-0.5 text-center text-sm font-semibold text-gray-800 border border-gray-300">Action</th>
                </tr>
              </thead>
              <tbody>
                {deposits.length > 0 ? (
                  deposits.map((deposit) => (
                    <tr key={deposit.id}>
                      <td className="px-1 py-1 text-sm text-gray-600 border border-gray-300">{deposit.id}</td>
                      <td className="px-1 py-1 text-sm text-gray-600 border border-gray-300">{deposit.username}</td>
                      <td className="px-1 py-1 text-sm text-gray-600 border border-gray-300">{deposit.loginName}</td>
                      <td className="px-1 py-1 text-sm text-gray-600 border border-gray-300">
                        <div>{deposit.firstName}</div>
                        <div>{deposit.lastName}</div>
                      </td>
                      <td className="px-1 py-1 text-sm text-gray-600 border border-gray-300">{deposit.amount}</td>
                      <td className="px-1 py-1 text-sm text-gray-600 border border-gray-300">{deposit.currency}</td>
                      <td className="px-1 py-1 text-sm text-gray-600 border border-gray-300">{deposit.from}</td>
                      <td className="px-1 py-1 text-sm text-gray-600 border border-gray-300">{deposit.to}</td>
                      <td className="px-1 py-1 text-sm text-gray-600 border border-gray-300">{deposit.referenceNo}</td>
                      <td className="px-1 py-1 text-sm text-gray-600 border border-gray-300">{deposit.dateTime}</td>
                      <td className="px-1 py-1 text-sm text-gray-600 border border-gray-300">
                        <button className="text-blue-600 hover:text-blue-900">Edit</button>
                      </td>
                    </tr>
                  ))
                ) : (
                  <tr>
                    <td colSpan={11} className="border border-gray-300">
                      <div className="flex flex-col items-center justify-center py-16">
                        <svg width="70" height="70" viewBox="0 0 70 70" fill="none" xmlns="http://www.w3.org/2000/svg" className="mb-4 text-gray-400">
                          <g opacity="0.5">
                            <path d="M55 15H15C12.2386 15 10 17.2386 10 20V50C10 52.7614 12.2386 55 15 55H55C57.7614 55 60 52.7614 60 50V20C60 17.2386 57.7614 15 55 15Z" stroke="#9CA3AF" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round"/>
                            <circle cx="35" cy="35" r="5" stroke="#9CA3AF" strokeWidth="2" />
                            <path d="M35 35L45 45" stroke="#9CA3AF" strokeWidth="2" strokeLinecap="round" />
                            <path d="M20 26V26.01" stroke="#9CA3AF" strokeWidth="2" strokeLinecap="round" />
                            <path d="M26 26V26.01" stroke="#9CA3AF" strokeWidth="2" strokeLinecap="round" />
                            <path d="M32 26V26.01" stroke="#9CA3AF" strokeWidth="2" strokeLinecap="round" />
                          </g>
                        </svg>
                        <div className="text-sm text-gray-500">No Data to display</div>
                      </div>
                    </td>
                  </tr>
                )}
              </tbody>
            </table>
          </div>
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

// For development/preview purposes only
// Remove when integrating with the actual application
if (document.getElementById('root')) {
  createRoot(document.getElementById('root')!).render(
    <React.StrictMode>
      <PendingDeposit />
    </React.StrictMode>
  );
} 