import React, { useState } from 'react';

interface ManualDepositFormProps {
  onClose: () => void;
  onSubmit: (data: {
    username: string;
    firstName: string;
    lastName: string;
    amount: string;
    currency: string;
    from: string;
    to: string;
    referenceNo: string;
  }) => void;
}

const ManualDepositForm: React.FC<ManualDepositFormProps> = ({ onClose, onSubmit }) => {
  const [username, setUsername] = useState('');
  const [firstName, setFirstName] = useState('');
  const [lastName, setLastName] = useState('');
  const [amount, setAmount] = useState('');
  const [currency, setCurrency] = useState('THB');
  const [from, setFrom] = useState('');
  const [to, setTo] = useState('');
  const [referenceNo, setReferenceNo] = useState('');

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    onSubmit({
      username,
      firstName,
      lastName,
      amount,
      currency,
      from,
      to,
      referenceNo
    });
  };

  return (
    <div className="fixed inset-0 bg-gray-600 bg-opacity-50 flex items-center justify-center z-50">
      <div className="bg-white rounded-lg shadow-xl max-w-2xl w-full p-6 max-h-[90vh] overflow-y-auto">
        <div className="flex justify-between items-center mb-6">
          <h2 className="text-lg font-medium">Add Manual Deposit</h2>
          <button 
            onClick={onClose}
            className="text-gray-400 hover:text-gray-500"
          >
            <svg className="h-6 w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor">
              <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M6 18L18 6M6 6l12 12" />
            </svg>
          </button>
        </div>

        <form onSubmit={handleSubmit} className="space-y-6">
          <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
            <div className="space-y-2">
              <label htmlFor="username" className="block text-sm font-medium text-gray-700">
                Username *
              </label>
              <input
                id="username"
                type="text"
                required
                value={username}
                onChange={(e) => setUsername(e.target.value)}
                className="block w-full rounded-md border border-gray-300 py-2 px-3 text-gray-900 focus:border-blue-500 focus:outline-none focus:ring-blue-500 sm:text-sm"
              />
            </div>

            <div className="space-y-2">
              <label htmlFor="firstName" className="block text-sm font-medium text-gray-700">
                First Name *
              </label>
              <input
                id="firstName"
                type="text"
                required
                value={firstName}
                onChange={(e) => setFirstName(e.target.value)}
                className="block w-full rounded-md border border-gray-300 py-2 px-3 text-gray-900 focus:border-blue-500 focus:outline-none focus:ring-blue-500 sm:text-sm"
              />
            </div>

            <div className="space-y-2">
              <label htmlFor="lastName" className="block text-sm font-medium text-gray-700">
                Last Name *
              </label>
              <input
                id="lastName"
                type="text"
                required
                value={lastName}
                onChange={(e) => setLastName(e.target.value)}
                className="block w-full rounded-md border border-gray-300 py-2 px-3 text-gray-900 focus:border-blue-500 focus:outline-none focus:ring-blue-500 sm:text-sm"
              />
            </div>

            <div className="space-y-2">
              <label htmlFor="amount" className="block text-sm font-medium text-gray-700">
                Amount *
              </label>
              <input
                id="amount"
                type="number"
                required
                value={amount}
                onChange={(e) => setAmount(e.target.value)}
                className="block w-full rounded-md border border-gray-300 py-2 px-3 text-gray-900 focus:border-blue-500 focus:outline-none focus:ring-blue-500 sm:text-sm"
              />
            </div>

            <div className="space-y-2">
              <label htmlFor="currency" className="block text-sm font-medium text-gray-700">
                Currency *
              </label>
              <select
                id="currency"
                required
                value={currency}
                onChange={(e) => setCurrency(e.target.value)}
                className="block w-full rounded-md border border-gray-300 py-2 px-3 text-gray-900 focus:border-blue-500 focus:outline-none focus:ring-blue-500 sm:text-sm"
              >
                <option value="THB">THB</option>
                <option value="USD">USD</option>
                <option value="EUR">EUR</option>
              </select>
            </div>

            <div className="space-y-2">
              <label htmlFor="from" className="block text-sm font-medium text-gray-700">
                From *
              </label>
              <input
                id="from"
                type="text"
                required
                value={from}
                onChange={(e) => setFrom(e.target.value)}
                className="block w-full rounded-md border border-gray-300 py-2 px-3 text-gray-900 focus:border-blue-500 focus:outline-none focus:ring-blue-500 sm:text-sm"
              />
            </div>

            <div className="space-y-2">
              <label htmlFor="to" className="block text-sm font-medium text-gray-700">
                To *
              </label>
              <input
                id="to"
                type="text"
                required
                value={to}
                onChange={(e) => setTo(e.target.value)}
                className="block w-full rounded-md border border-gray-300 py-2 px-3 text-gray-900 focus:border-blue-500 focus:outline-none focus:ring-blue-500 sm:text-sm"
              />
            </div>

            <div className="space-y-2">
              <label htmlFor="referenceNo" className="block text-sm font-medium text-gray-700">
                Reference Number *
              </label>
              <input
                id="referenceNo"
                type="text"
                required
                value={referenceNo}
                onChange={(e) => setReferenceNo(e.target.value)}
                className="block w-full rounded-md border border-gray-300 py-2 px-3 text-gray-900 focus:border-blue-500 focus:outline-none focus:ring-blue-500 sm:text-sm"
              />
            </div>
          </div>

          <div className="flex justify-end space-x-3 pt-4">
            <button
              type="button"
              onClick={onClose}
              className="px-4 py-2 border border-gray-300 rounded-md shadow-sm text-sm font-medium text-gray-700 bg-white hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500"
            >
              Cancel
            </button>
            <button
              type="submit"
              className="bg-[#1e4d8c] text-white px-6 py-2 rounded-lg text-sm font-medium hover:bg-[#173d70] transition-colors"
            >
              Submit
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};

export default ManualDepositForm; 