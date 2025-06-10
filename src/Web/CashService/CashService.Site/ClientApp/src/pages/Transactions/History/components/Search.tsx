import React, { useState } from 'react';
import { useTranslation } from 'react-i18next';

import { IconCopy, IconExcel, IconSearch } from '../../../../components/Icons';
import { Alert } from '../../../../components/ui/Alert';
import { TransactionItem } from '../../../../services/interfaces/Transactions/TransactionHistory';
import {
    CurrentCurrency, displayAmount, formatDateTime, mappingStatus
} from '../../../../utils/commonFunctions';
import Tippy from '@tippyjs/react';

interface SearchProps {
  value: string;
  onChange: (value: string) => void;
  placeholder?: string;
  items: TransactionItem[];
  columnFilter: string[];
  onDownload: () => void;
}

const Search: React.FC<SearchProps> = ({ value, onChange, items, columnFilter, onDownload }) => {
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
      `${t('table.username')} & ${t('table.nickname')}`,
      ...columnFilter.map(getColumnHeader),
    ];

    const rows = items.map((item, index) => {
      const rowData: any[] = [
        (index + 1).toString(),
        `${item.username} ${item.nickname ? `(${item.nickname})` : ''}`,
      ];
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
            rowData.push(mappingStatus(item.statusId));
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

  const handleDownload = async () => {
    onDownload();
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
          className="p-[4px] bg-black/10 rounded-[3px] cursor-pointer"
          title="Download"
        >
          <IconExcel />
        </button>
        <button
          onClick={handleCopy}
          className="p-[4px] bg-black/10 rounded-[3px] cursor-pointer"
          title="Copy"
        >
          <Tippy  placement="top" theme="light" arrow={true}>
            <IconCopy className="fill-[var(--pager-color)]" />
          </Tippy>
          
        </button>

        <div className="flex">
          <div className="w-[250px] ml-auto">
            <div className="relative">
              <span className="absolute transform-[translatey(-50%)] z-[10] top-1/2 left-[8px]">
                <IconSearch />
              </span>
              <input
                type="text"
                value={value}
                onChange={(e) => onChange(e.target.value)}
                className="control-field pl-[30px]!"
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
