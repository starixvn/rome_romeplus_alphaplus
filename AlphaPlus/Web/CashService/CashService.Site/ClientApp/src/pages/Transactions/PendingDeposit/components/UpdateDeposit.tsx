import React, { useState, useEffect } from 'react';
import { DepositAccountInfo } from '../../../../services/interfaces/DepositAccountInfo';
import { useTranslation } from 'react-i18next';

interface UpdateDepositProps {
  deposit: DepositAccountInfo;
  onClose: () => void;
  onSave: (updatedDeposit: DepositAccountInfo) => void;
}

const UpdateDeposit: React.FC<UpdateDepositProps> = ({ 
  deposit, 
  onClose, 
  onSave 
}) => {
  const { t } = useTranslation('pendingDeposit');
  const [username, setUsername] = useState('');
  const [loginName, setLoginName] = useState('');
  const [firstName, setFirstName] = useState('');
  const [lastName, setLastName] = useState('');
  const [amount, setAmount] = useState('');
  const [currency, setCurrency] = useState('USD');
  const [from, setFrom] = useState('');
  const [to, setTo] = useState('');
  const [referenceNo, setReferenceNo] = useState('');
  const [dateTime, setDateTime] = useState('');

  useEffect(() => {
    if (deposit) {
      setUsername(deposit.username);
      setLoginName(deposit.loginName);
      setFirstName(deposit.firstName);
      setLastName(deposit.lastName);
      setAmount(deposit.amount.toString());
      setCurrency(deposit.currency);
      setFrom(deposit.from.toString());
      setTo(deposit.to.toString());
      setReferenceNo(deposit.referenceNo);
      
      const date = typeof deposit.dateAndTime === 'string' 
        ? new Date(deposit.dateAndTime)
        : deposit.dateAndTime;
        
      const formattedDate = date.toISOString().slice(0, 16);
      setDateTime(formattedDate);
    }
  }, [deposit]);

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    
    const updatedDeposit: DepositAccountInfo = {
      ...deposit,
      username,
      loginName,
      firstName,
      lastName,
      amount: parseFloat(amount),
      currency,
      from: parseInt(from),
      to: parseInt(to),
      referenceNo,
      dateAndTime: new Date(dateTime)
    };
    
    onSave(updatedDeposit);
  };

  return (
    <div className="fixed inset-0 bg-white flex flex-col z-50">
      <div className="bg-gray-100 p-4">
        <h2 className="text-lg font-medium">{t('updateDeposit.title')}</h2>
      </div>
      
      <form id='update-deposit-form' onSubmit={handleSubmit} className="p-6 flex-1 w-1/2">
        <div className="grid grid-cols-2 gap-6">
          <div>
            <label htmlFor="username" className="block text-sm font-medium text-gray-700 mb-1">
              {t('dataGrid.member')}
            </label>
            <input
              id="username"
              type="text"
              value={username}
              onChange={(e) => setUsername(e.target.value)}
              className="block w-full rounded-md border border-gray-300 py-2 px-3 text-gray-900 focus:border-blue-500 focus:outline-none focus:ring-blue-500 sm:text-sm"
              required
            />
          </div>
          
          <div>
            <label htmlFor="loginName" className="block text-sm font-medium text-gray-700 mb-1">
              Login Name
            </label>
            <input
              id="loginName"
              type="text"
              value={loginName}
              onChange={(e) => setLoginName(e.target.value)}
              className="block w-full rounded-md border border-gray-300 py-2 px-3 text-gray-900 focus:border-blue-500 focus:outline-none focus:ring-blue-500 sm:text-sm"
              required
            />
          </div>
          
          <div>
            <label htmlFor="firstName" className="block text-sm font-medium text-gray-700 mb-1">
              First Name
            </label>
            <input
              id="firstName"
              type="text"
              value={firstName}
              onChange={(e) => setFirstName(e.target.value)}
              className="block w-full rounded-md border border-gray-300 py-2 px-3 text-gray-900 focus:border-blue-500 focus:outline-none focus:ring-blue-500 sm:text-sm"
              required
            />
          </div>
          
          <div>
            <label htmlFor="lastName" className="block text-sm font-medium text-gray-700 mb-1">
              Last Name
            </label>
            <input
              id="lastName"
              type="text"
              value={lastName}
              onChange={(e) => setLastName(e.target.value)}
              className="block w-full rounded-md border border-gray-300 py-2 px-3 text-gray-900 focus:border-blue-500 focus:outline-none focus:ring-blue-500 sm:text-sm"
              required
            />
          </div>
          
          <div>
            <label htmlFor="amount" className="block text-sm font-medium text-gray-700 mb-1">
              {t('dataGrid.amount')}
            </label>
            <input
              id="amount"
              type="number"
              step="0.01"
              value={amount}
              onChange={(e) => setAmount(e.target.value)}
              className="block w-full rounded-md border border-gray-300 py-2 px-3 text-gray-900 focus:border-blue-500 focus:outline-none focus:ring-blue-500 sm:text-sm"
              required
            />
          </div>
          
          <div>
            <label htmlFor="currency" className="block text-sm font-medium text-gray-700 mb-1">
              Currency
            </label>
            <select
              id="currency"
              value={currency}
              onChange={(e) => setCurrency(e.target.value)}
              className="block w-full rounded-md border border-gray-300 py-2 px-3 text-gray-900 focus:border-blue-500 focus:outline-none focus:ring-blue-500 sm:text-sm"
              required
            >
              <option value="USD">USD</option>
              <option value="EUR">EUR</option>
              <option value="GBP">GBP</option>
            </select>
          </div>
          
          <div>
            <label htmlFor="from" className="block text-sm font-medium text-gray-700 mb-1">
              {t('dataGrid.from')}
            </label>
            <input
              id="from"
              type="text"
              value={from}
              onChange={(e) => setFrom(e.target.value)}
              className="block w-full rounded-md border border-gray-300 py-2 px-3 text-gray-900 focus:border-blue-500 focus:outline-none focus:ring-blue-500 sm:text-sm"
              required
            />
          </div>
          
          <div>
            <label htmlFor="to" className="block text-sm font-medium text-gray-700 mb-1">
              {t('dataGrid.to')}
            </label>
            <input
              id="to"
              type="text"
              value={to}
              onChange={(e) => setTo(e.target.value)}
              className="block w-full rounded-md border border-gray-300 py-2 px-3 text-gray-900 focus:border-blue-500 focus:outline-none focus:ring-blue-500 sm:text-sm"
              required
            />
          </div>
          
          <div>
            <label htmlFor="referenceNo" className="block text-sm font-medium text-gray-700 mb-1">
              {t('dataGrid.referenceNo')}
            </label>
            <input
              id="referenceNo"
              type="text"
              value={referenceNo}
              onChange={(e) => setReferenceNo(e.target.value)}
              className="block w-full rounded-md border border-gray-300 py-2 px-3 text-gray-900 focus:border-blue-500 focus:outline-none focus:ring-blue-500 sm:text-sm"
              required
            />
          </div>
          
          <div>
            <label htmlFor="dateTime" className="block text-sm font-medium text-gray-700 mb-1">
              {t('dataGrid.dateAndTime')}
            </label>
            <input
              id="dateTime"
              type="datetime-local"
              value={dateTime}
              onChange={(e) => setDateTime(e.target.value)}
              className="block w-full rounded-md border border-gray-300 py-2 px-3 text-gray-900 focus:border-blue-500 focus:outline-none focus:ring-blue-500 sm:text-sm"
              required
            />
          </div>
        </div>
        
        <div className="mt-8 flex justify-end space-x-3">
          <button
            type="button"
            onClick={onClose}
            className="px-6 py-2 border border-gray-300 rounded-md shadow-sm text-sm font-medium text-gray-700 bg-white hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500"
          >
            {t('manualDepositForm.cancel')}
          </button>
          <button
            type="submit"
            className="bg-[#1e4d8c] text-white px-6 py-2 rounded-md text-sm font-medium hover:bg-[#173d70] transition-colors"
          >
            {t('updateDeposit.approveDeposit')}
          </button>
        </div>
      </form>
    </div>
  );
};

export default UpdateDeposit; 