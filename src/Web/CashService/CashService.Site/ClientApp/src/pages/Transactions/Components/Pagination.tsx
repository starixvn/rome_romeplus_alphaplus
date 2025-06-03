import React, { useState, useEffect } from 'react';
import { PAGE_SIZE_OPTIONS } from '../../../constants/paymentConstants';
import { useTranslation } from 'react-i18next';

interface PaginationProps {
  currentPage: number;
  pageSize: number;
  totalItems: number;
  onPageChange: (page: number) => void;
  onPageSizeChange: (pageSize: number) => void;
}

const Pagination: React.FC<PaginationProps> = ({
  currentPage,
  pageSize,
  totalItems,
  onPageChange,
  onPageSizeChange,
}) => {
  const { t } = useTranslation('global');
  const [inputPage, setInputPage] = useState(currentPage.toString());
  const totalPages = pageSize > 0 ? Math.ceil(totalItems / pageSize) : 1;

  useEffect(() => {
    setInputPage(currentPage.toString());
  }, [currentPage]);

  const handlePageInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    if (e.target.value === '' || /^\d+$/.test(e.target.value)) {
      setInputPage(e.target.value);
    }
  };

  const handlePageInputKeyDown = (e: React.KeyboardEvent<HTMLInputElement>) => {
    if (e.key === 'Enter') {
      const newPage = Number(inputPage);
      if (newPage > 0 && newPage <= totalPages) {
        onPageChange(newPage);
      } else {
        setInputPage(currentPage.toString());
      }
    }
  };

  const startItem = (currentPage - 1) * pageSize + 1;
  const endItem = Math.min(currentPage * pageSize, totalItems);

  return (
    <div className="flex items-center gap-2 py-2 px-4 bg-white text-sm">
      <div className="flex items-center gap-2">
        <span>{t('pageSize')}</span>
        <select
          value={pageSize}
          onChange={(e) => onPageSizeChange(Number(e.target.value))}
          className="h-[24px] px-1 w-[60px] border border-gray-300 rounded outline-none"
        >
          {PAGE_SIZE_OPTIONS.map((size) => (
            <option key={size} value={size}>
              {size}
            </option>
          ))}
        </select>
      </div>

      <div className="flex items-center h-[24px] space-x-1">
        <span
          onClick={() => currentPage !== 1 && onPageChange(1)}
          className={`text-black text-xl h-[24px] flex items-center translate-y-[-1px] cursor-pointer select-none ${
            currentPage === 1 ? 'opacity-40' : 'hover:opacity-70'
          }`}
          style={{ transform: 'scaleX(-1)' }}
        >
          »
        </span>

        <span
          onClick={() => currentPage !== 1 && onPageChange(currentPage - 1)}
          className={`text-black text-xl h-[24px] flex items-center translate-y-[-1px] cursor-pointer select-none ${
            currentPage === 1 ? 'opacity-40' : 'hover:opacity-70'
          }`}
          style={{ transform: 'scaleX(-1)' }}
        >
          ›
        </span>

        <div className="border-r border-[#ccc] h-[14px] mx-1 my-auto"></div>
        <span className="h-[24px] flex items-center">{t('page')}</span>
        <div className="pl-1">
          <input
            type="text"
            value={inputPage}
            onChange={handlePageInputChange}
            onKeyDown={handlePageInputKeyDown}
            onBlur={() => setInputPage(currentPage.toString())}
            className="border border-gray-300 rounded px-1 h-[24px] w-[40px] text-center mx-1 outline-none"
          />
        </div>

        <div className="border-r border-[#ccc] h-[14px] mx-1 my-auto"></div>

        <span
          onClick={() => currentPage !== totalPages && onPageChange(currentPage + 1)}
          className={`text-black text-xl h-[24px] flex items-center translate-y-[-1px] cursor-pointer select-none ${
            currentPage === totalPages ? 'opacity-40' : 'hover:opacity-70'
          }`}
        >
          ›
        </span>

        <span
          onClick={() => currentPage !== totalPages && onPageChange(totalPages)}
          className={`text-black text-xl h-[24px] flex items-center translate-y-[-1px] cursor-pointer select-none ${
            currentPage === totalPages ? 'opacity-40' : 'hover:opacity-70'
          }`}
        >
          »
        </span>
      </div>

      <div className="ml-auto">
        {endItem > 0 &&
          t('display') +
            ' ' +
            startItem +
            ' ' +
            t('to') +
            ' ' +
            endItem +
            ' ' +
            t('of') +
            ' ' +
            totalItems +
            ' ' +
            t('items')}
      </div>
    </div>
  );
};

export default Pagination;
