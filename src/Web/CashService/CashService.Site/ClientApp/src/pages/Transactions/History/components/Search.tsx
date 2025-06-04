import React, { useState } from 'react';
import { SearchIcon, DownloadIcon, CopyIcon } from '../../../../components/Icons';
import { useTranslation } from 'react-i18next';
import { TransactionItem } from '../../../../services/interfaces/Transactions/TransactionHistory';
import {
  displayAmount,
  mappingStatus,
  formatDateTime,
  CurrentCurrency,
} from '../../../../common/commonFunctions';
import { Alert } from '../../../../components/ui/Alert';

interface SearchProps {
  value: string;
  onChange: (value: string) => void;
  placeholder?: string;
  items: TransactionItem[];
  columnFilter: string[];
}

const Search: React.FC<SearchProps> = ({ value, onChange, items, columnFilter }) => {
  const { t } = useTranslation('transactionHistory');
  const [showAlert, setShowAlert] = useState(false);

  const getColumnHeader = (columnName: string): string => {
    switch (columnName) {
      case 'name':
        return t('table.name');
      case 'amount':
        return `${t('table.amount')} (${CurrentCurrency})`;
      case 'from':
        return t('table.from');
      case 'to':
        return t('table.to');
      case 'status':
        return t('table.status');
      case 'type':
        return t('table.type');
      case 'referenceNo':
        return t('table.referenceNo');
      case 'dateAndTime':
        return t('table.dateAndTime');
      default:
        return columnName;
    }
  };

  const handleCopy = () => {
    const headers = [
      t('table.no'),
      `${t('table.username')}/${t('table.nickname')}`,
      ...columnFilter.map(getColumnHeader),
    ];

    const rows = items.map((item, index) => {
      const rowData: string[] = [(index + 1).toString(), `${item.username} / ${item.nickname}`];
      columnFilter.forEach((col) => {
        switch (col) {
          case 'name':
            rowData.push(`${item.firstName} ${item.lastName}`);
            break;
          case 'amount':
            rowData.push(displayAmount(item.amount));
            break;
          case 'from':
            rowData.push(item.paymentFrom ? item.paymentFrom : '-');
            break;
          case 'to':
            rowData.push(item.paymentTo ? item.paymentTo : '-');
            break;
          case 'status':
            rowData.push(mappingStatus(item.statusId).toUpperCase());
            break;
          case 'type':
            rowData.push(item.transactionType?.toString() || '-');
            break;
          case 'referenceNo':
            rowData.push(item.transId.toString());
            break;
          case 'dateAndTime':
            rowData.push(formatDateTime(item.createdTime));
            break;
          default:
            rowData.push('');
        }
      });
      return rowData.join('\t');
    });

    const finalText = [headers.join('\t'), ...rows].join('\n');
    navigator.clipboard
      .writeText(finalText)
      .then(() => {
        setShowAlert(true);
        setTimeout(() => {
          setShowAlert(false);
        }, 1000);
      })
      .catch((err) => {
        console.error('Failed to copy text: ', err);
      });
  };

  const handleDownload = () => {
    // TODO: Implement download functionality
  };

  return (
    <div className="flex items-center w-full">
      <div className="text-sm text-gray-500 italic flex-shrink-0">* {t('filter.maxRecords')}</div>
      {showAlert && (
        <Alert
          style={{ position: 'absolute', marginLeft: '550px' }}
          message={t('messages.copiedToClipboard')}
          showIcon={false}
        />
      )}
      <div className="flex items-center space-x-2 ml-auto">
        <button
          onClick={handleDownload}
          className="p-1.5 hover:bg-gray-100 rounded-lg transition-colors cursor-pointer relative bottom-[7px]"
          title="Download"
        >
          <DownloadIcon />
        </button>
        <button
          onClick={handleCopy}
          className="p-1.5 hover:bg-gray-100 rounded-lg transition-colors cursor-pointer relative bottom-[7px]"
          title="Copy"
        >
          <CopyIcon />
        </button>

        <div className="flex mb-[16px]">
          <div className="w-[250px] ml-auto">
            <div className="relative">
              <span className="absolute top-[8px] left-[8px] z-[10] max-[768px]:top-[12px]">
                <SearchIcon />
              </span>
              <input
                type="text"
                value={value}
                onChange={(e) => onChange(e.target.value)}
                className="h-[32px] max-[768px]:h-[40px] w-[100%] pl-[30px] bg-[#ffffff] border-[1px] border-[#0000003D] rounded-[4px] px-[8px] focus:outline-none focus:border-[var(--button-border)] shadow-[0_0_6px_rgba(0,0,0,0.16)]"
                placeholder={t('filter.usernamePlaceholder')}
              />
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Search;
