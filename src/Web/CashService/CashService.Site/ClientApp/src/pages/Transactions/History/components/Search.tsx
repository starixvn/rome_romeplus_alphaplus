import React, { useState } from 'react';
import { useTranslation } from 'react-i18next';

import { TransactionItem } from '../../../../services/interfaces/Transactions/TransactionHistory';
import { displayAmount, formatDateTime, mappingStatus } from '../../../../utils/commonFunctions';
import Tippy from '@tippyjs/react';
 
import { IconSearch } from '../../../../components/Icons';
import { ButtonIconComponent } from '../../../../components/Buttons/ButtonIconComponent';

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
  const { t: tGlobal } = useTranslation('global');
  const [textCopy, setTextCopy] = useState(t('messages.copy'));

  const getColumnHeader = (columnName: string): string => {
    switch (columnName) {
      case 'name':
        return t('table.name');
      case 'amount':
        return t('table.amount');
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

  const getAccountName = (account: string) => {
    if (!account || account === '-') return '-';
    const [bankCode, accountNumber] = account.split('-');
    return bankCode && !accountNumber ? bankCode : accountNumber.toString();
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
            rowData.push(getAccountName(item.paymentFrom));
            break;
          case 'to':
            rowData.push(getAccountName(item.paymentTo));
            break;
          case 'status':
            rowData.push(tGlobal(mappingStatus(item.statusId).props.text));
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
        setTextCopy(t('messages.copied'));
      })
      .catch((err) => {
        console.error('Failed to copy text: ', err);
      });
  };

  const handleDownload = async () => {
    onDownload();
  };

  return (
    <div className="flex items-center w-full justify-between">
      <div className="italic ">
        <span className="text-[#ff0101] dark:text-[#e54748]">*</span> {t('filter.maxRecords')}
      </div>
      <div className="flex items-center">
        <Tippy
          content={t('messages.exportToExcel')}
          placement="top"
          theme="light"
          arrow={true}
          hideOnClick={false}
        >
          <span><ButtonIconComponent style='Excel' onClick={handleDownload} /></span>
        </Tippy>
        <Tippy content={textCopy} placement="top" theme="light" arrow={true} hideOnClick={false}>
          <span><ButtonIconComponent style='Copy' onClick={handleCopy} /></span>
        </Tippy>

        <div className="flex ml-[4px]">
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
