import React, { useState } from 'react';
import { useTranslation } from 'react-i18next';

import { IconCopy, IconExcel, IconSearch } from '../../../../components/Icons';
import { PromotionTransaction } from '../../../../services/interfaces/Promotion';
import { displayAmount, formatDateTime } from '../../../../utils/commonFunctions';
import Tippy from '@tippyjs/react';

interface SearchProps {
  value: string;
  onChange: (value: string) => void;
  placeholder?: string;
  items: PromotionTransaction[];
  onDownload: () => void;
}

const Search: React.FC<SearchProps> = ({ value, onChange, items, onDownload }) => {
  const { t } = useTranslation('promotion');
  const [textCopy, setTextCopy] = useState(t('messages.copy'));

  const handleCopy = () => {
    const headers = [
      t('table.no'),
      `${t('table.username')} & ${t('table.nickname')}`,
      t('table.name'),
      t('table.title'),
      t('table.promotionType'),
      t('table.amount'),
      t('table.status'),
      t('table.dateRange'),
    ];

    const rows = items.map((item, index) => {
      const rowData = [
        (index + 1).toString(),
        `${item.username} ${item.nickname ? `(${item.nickname})` : ''}`,
        `${item.firstName} ${item.lastName}`,
        item.title,
        item.promotionTypeName,
        `${displayAmount(item.amount)} ${item.currency}`,
        item.status,
        `${formatDateTime(item.fromDate)} - ${formatDateTime(item.toDate)}`,
      ];
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
      <div className="flex items-center gap-[8px]">
        <Tippy
          content={t('messages.exportToExcel')}
          placement="top"
          theme="light"
          arrow={true}
          hideOnClick={false}
        >
          <button
            onClick={handleDownload}
            className="p-[4px] bg-black/10 rounded-[3px] cursor-pointer"
          >
            <IconExcel />
          </button>
        </Tippy>
        <Tippy content={textCopy} placement="top" theme="light" arrow={true} hideOnClick={false}>
          <button onClick={handleCopy} className="p-[4px] bg-black/10 rounded-[3px] cursor-pointer">
            <IconCopy />
          </button>
        </Tippy>

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
                placeholder={t('filter.searchPlaceholder')}
              />
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Search;
