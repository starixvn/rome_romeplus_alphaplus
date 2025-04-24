import React from 'react';
import { DepositAccountInfo } from '../../../../services/interfaces/DepositAccountInfo';

interface DataGridProps {
  deposits: DepositAccountInfo[];
}

const DataGrid: React.FC<DataGridProps> = ({ deposits }) => {
  return (
    <div className="overflow-x-auto">
      <table className="min-w-full border-collapse border border-gray-300">
        <thead>
          <tr>
            <th
              scope="col"
              className="px-1 py-0.5 text-center text-sm font-semibold text-gray-800 border border-gray-300"
            >
              #
            </th>
            <th
              scope="col"
              className="px-1 py-0.5 text-center text-sm font-semibold text-gray-800 border border-gray-300"
            >
              Username
            </th>
            <th
              scope="col"
              className="px-1 py-0.5 text-center text-sm font-semibold text-gray-800 border border-gray-300"
            >
              Login Name
            </th>
            <th
              scope="col"
              className="px-1 py-0.5 text-center text-sm font-semibold text-gray-800 border border-gray-300"
            >
              <div>First Name</div>
              <div>Last Name</div>
            </th>
            <th
              scope="col"
              className="px-1 py-0.5 text-center text-sm font-semibold text-gray-800 border border-gray-300"
            >
              Amount
            </th>
            <th
              scope="col"
              className="px-1 py-0.5 text-center text-sm font-semibold text-gray-800 border border-gray-300"
            >
              Currency
            </th>
            <th
              scope="col"
              className="px-1 py-0.5 text-center text-sm font-semibold text-gray-800 border border-gray-300"
            >
              From
            </th>
            <th
              scope="col"
              className="px-1 py-0.5 text-center text-sm font-semibold text-gray-800 border border-gray-300"
            >
              To
            </th>
            <th
              scope="col"
              className="px-1 py-0.5 text-center text-sm font-semibold text-gray-800 border border-gray-300"
            >
              Reference No.
            </th>
            <th
              scope="col"
              className="px-1 py-0.5 text-center text-sm font-semibold text-gray-800 border border-gray-300"
            >
              Date and Time
            </th>
            <th
              scope="col"
              className="px-1 py-0.5 text-center text-sm font-semibold text-gray-800 border border-gray-300"
            >
              Action
            </th>
          </tr>
        </thead>
        <tbody>
          {deposits.length > 0 ? (
            deposits.map((deposit) => (
              <tr key={deposit.id}>
                <td className="px-1 py-1 text-sm text-gray-600 border border-gray-300">
                  {deposit.id}
                </td>
                <td className="px-1 py-1 text-sm text-gray-600 border border-gray-300">
                  {deposit.username}
                </td>
                <td className="px-1 py-1 text-sm text-gray-600 border border-gray-300">
                  {deposit.loginName}
                </td>
                <td className="px-1 py-1 text-sm text-gray-600 border border-gray-300">
                  <div>{deposit.firstName}</div>
                  <div>{deposit.lastName}</div>
                </td>
                <td className="px-1 py-1 text-sm text-gray-600 border border-gray-300">
                  {deposit.amount}
                </td>
                <td className="px-1 py-1 text-sm text-gray-600 border border-gray-300">
                  {deposit.currency}
                </td>
                <td className="px-1 py-1 text-sm text-gray-600 border border-gray-300">
                  {deposit.from}
                </td>
                <td className="px-1 py-1 text-sm text-gray-600 border border-gray-300">
                  {deposit.to}
                </td>
                <td className="px-1 py-1 text-sm text-gray-600 border border-gray-300">
                  {deposit.referenceNo}
                </td>
                <td className="px-1 py-1 text-sm text-gray-600 border border-gray-300">
                  {deposit.dateAndTime.toLocaleString()}
                </td>
                <td className="px-1 py-1 text-sm text-gray-600 border border-gray-300">
                  <button className="text-blue-600 hover:text-blue-900">Edit</button>
                </td>
              </tr>
            ))
          ) : (
            <tr>
              <td colSpan={11} className="border border-gray-300">
                <div className="flex flex-col items-center justify-center py-16">
                  <svg
                    width="70"
                    height="70"
                    viewBox="0 0 70 70"
                    fill="none"
                    xmlns="http://www.w3.org/2000/svg"
                    className="mb-4 text-gray-400"
                  >
                    <g opacity="0.5">
                      <path
                        d="M55 15H15C12.2386 15 10 17.2386 10 20V50C10 52.7614 12.2386 55 15 55H55C57.7614 55 60 52.7614 60 50V20C60 17.2386 57.7614 15 55 15Z"
                        stroke="#9CA3AF"
                        strokeWidth="2"
                        strokeLinecap="round"
                        strokeLinejoin="round"
                      />
                      <circle cx="35" cy="35" r="5" stroke="#9CA3AF" strokeWidth="2" />
                      <path
                        d="M35 35L45 45"
                        stroke="#9CA3AF"
                        strokeWidth="2"
                        strokeLinecap="round"
                      />
                      <path
                        d="M20 26V26.01"
                        stroke="#9CA3AF"
                        strokeWidth="2"
                        strokeLinecap="round"
                      />
                      <path
                        d="M26 26V26.01"
                        stroke="#9CA3AF"
                        strokeWidth="2"
                        strokeLinecap="round"
                      />
                      <path
                        d="M32 26V26.01"
                        stroke="#9CA3AF"
                        strokeWidth="2"
                        strokeLinecap="round"
                      />
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
  );
};

export default DataGrid; 