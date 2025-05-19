import React from 'react';
import { useTranslation } from 'react-i18next';

interface NoRecordFoundProps {
  colSpan: number;
  message?: string;
}

const NoRecordFound: React.FC<NoRecordFoundProps> = ({ colSpan, message }) => {
  const { t } = useTranslation('common');

  return (
    <tr>
      <td colSpan={colSpan} className="p-3 text-center border border-[#dee2e6]">
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
              <path d="M35 35L45 45" stroke="#9CA3AF" strokeWidth="2" strokeLinecap="round" />
              <path d="M20 26V26.01" stroke="#9CA3AF" strokeWidth="2" strokeLinecap="round" />
              <path d="M26 26V26.01" stroke="#9CA3AF" strokeWidth="2" strokeLinecap="round" />
              <path d="M32 26V26.01" stroke="#9CA3AF" strokeWidth="2" strokeLinecap="round" />
            </g>
          </svg>
          <div className="text-sm text-gray-500">{message || t('table.noRecordsFound')}</div>
        </div>
      </td>
    </tr>
  );
};

export default NoRecordFound;
