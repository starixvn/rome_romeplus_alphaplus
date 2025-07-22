import React, { useState } from 'react';
import { useTranslation } from 'react-i18next';

import { IconSearch } from '../../../../components/Icons';
import { PromotionTransaction } from '../../../../services/interfaces/Promotion';
import { displayAmount, formatDateTime } from '../../../../utils/commonFunctions';
import Tippy from '@tippyjs/react';
import { ButtonIconComponent } from '../../../../components/Buttons/ButtonIconComponent';
import { ButtonComponent } from '../../../../components/Buttons';

interface SearchProps {
  value: string;
  onChange: (value: string) => void;
  placeholder?: string;
  items: PromotionTransaction[];
  selectedItems: PromotionTransaction[];
  onDownload: () => void;
  onReject: () => void;
  onApprove: () => void;
}

const Search: React.FC<SearchProps> = ({
  value,
  onChange,
  items,
  selectedItems,
  onDownload,
  onReject,
  onApprove,
}) => {
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
      <div className="flex items-center gap-[8px]">
        <ButtonComponent
          variant="apply"
          onClick={onApprove}
          disabled={selectedItems.length === 0}
          text={t('search.approve') + ' (' + selectedItems.length + ')'}
        />
        <ButtonComponent
          variant="reject"
          onClick={onReject}
          disabled={selectedItems.length === 0}
          text={t('search.reject') + ' (' + selectedItems.length + ')'}
        />
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
