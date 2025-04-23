import React from 'react';

interface TransactionItem {
  id: string;
  username: string;
  firstName: string;
  lastName: string;
  amount: number;
  currency: string;
  from: string;
  to: string;
  status: 'COMPLETED' | 'REJECTED' | 'PROVIDER_REJECTED' | 'EXPIRED';
  transactionType: string;
  referenceNumber: string;
  dateTime: string;
}

interface TableProps {
  items: TransactionItem[];
  isLoading: boolean;
}

const Table: React.FC<TableProps> = ({ items, isLoading }) => {
  if (isLoading) {
    return (
      <div className="flex justify-center items-center h-64">
        <div className="loading-spinner"></div>
      </div>
    );
  }

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
              Status
            </th>
            <th
              scope="col"
              className="px-1 py-0.5 text-center text-sm font-semibold text-gray-800 border border-gray-300"
            >
              Transaction type
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
          {items.length > 0 ? (
            items.map((item, index) => (
              <tr key={item.id}>
                <td className="px-1 py-1 text-sm text-gray-600 border border-gray-300 text-center">
                  {index + 1}
                </td>
                <td className="px-1 py-1 text-sm text-gray-600 border border-gray-300">
                  {item.username}
                </td>
                <td className="px-1 py-1 text-sm text-gray-600 border border-gray-300">
                  <div>{item.firstName}</div>
                  <div>{item.lastName}</div>
                </td>
                <td className="px-1 py-1 text-sm text-gray-600 border border-gray-300 text-right">
                  {item.amount.toFixed(2)}
                </td>
                <td className="px-1 py-1 text-sm text-gray-600 border border-gray-300">
                  {item.currency}
                </td>
                <td className="px-1 py-1 text-sm text-gray-600 border border-gray-300">
                  {item.from}
                </td>
                <td className="px-1 py-1 text-sm text-gray-600 border border-gray-300">
                  {item.to}
                </td>
                <td className="px-1 py-1 text-sm border border-gray-300">
                  <span className={`status-badge ${item.status.toLowerCase()}`}>{item.status}</span>
                </td>
                <td className="px-1 py-1 text-sm text-gray-600 border border-gray-300">
                  {item.transactionType}
                </td>
                <td className="px-1 py-1 text-sm text-gray-600 border border-gray-300">
                  {item.referenceNumber}
                </td>
                <td className="px-1 py-1 text-sm text-gray-600 border border-gray-300">
                  {item.dateTime}
                </td>
                <td className="px-1 py-1 text-sm text-gray-600 border border-gray-300">
                  <button className="action-button">View</button>
                </td>
              </tr>
            ))
          ) : (
            <tr>
              <td colSpan={12} className="border border-gray-300">
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
                  <p className="text-gray-500 text-sm">No records found</p>
                </div>
              </td>
            </tr>
          )}
        </tbody>
      </table>
    </div>
  );
};

export default Table;
