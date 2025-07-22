import React, { useState, useEffect } from 'react';
import { PAGE_SIZE_OPTIONS } from '../../../constants/paymentConstants';
import { useTranslation } from 'react-i18next';
import './Pagination.scss';
import { IconArrowDown, IconFirst, IconLast, IconNext, IconPrev } from '../../../components/Icons';
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
  const pager = Math.ceil(totalItems / pageSize);
 
  return (
    <div className="flex items-center py-[6px] px-[10px] border border-[var(--pager-border)] border-t-0 mb-[2px] shadow-[inset_0px_1px_0px_0px_var(--pager-border)]">
      <div className="mr-[6px] relative">    
        <select
          value={pageSize}
          onChange={(e) => onPageSizeChange(Number(e.target.value))}
          className="h-[24px] min-w-[70px] control-field"
        >
          {PAGE_SIZE_OPTIONS.map((size) => (
            <option key={size} value={size}>
              {size}
            </option>
          ))}
        </select>
        <IconArrowDown className="w-[12px] h-[12px] absolute right-[3px] top-[7px] fill-[#333] dark:fill-[#ededed]  " />
      </div>

      <div className="flex items-center h-[24px] space-x-1">
        <span
          onClick={() => currentPage !== 1 && totalPages !== 0 && onPageChange(1)}
          className={`rounded-[3px] p-[2px] mx-[3px]  ${
            currentPage === 1 || totalPages === 0 ? 'cursor-not-allowed bg-[var(--pager-button-background1)] [&>svg]:fill-[var(--pager-button-disable-color)]' : 'cursor-pointer bg-[var(--pager-button-background2)] hover:shadow-[var(--pager-button-shadow)]'
          }`}          
        >
          <IconFirst />
        </span>

        <span
          onClick={() => currentPage !== 1 && totalPages !== 0 && onPageChange(currentPage - 1)}
          className={`rounded-[3px] p-[2px] mx-[3px]  ${
            currentPage === 1 || totalPages === 0 ? 'cursor-not-allowed bg-[var(--pager-button-background1)] [&>svg]:fill-[var(--pager-button-disable-color)]' : 'cursor-pointer bg-[var(--pager-button-background2)] hover:shadow-[var(--pager-button-shadow)]'
          }`}         
        >
          <IconPrev  />
        </span>        
        <span className="h-[24px] flex items-center">{t('page')}</span>
        <div className="pl-1">
          <input
            type="text"
            value={inputPage}
            onChange={handlePageInputChange}
            onKeyDown={handlePageInputKeyDown}
            onBlur={() => setInputPage(currentPage.toString())}
            className="control-field max-w-[38px]"
          />
        </div>       
        <div className="flex items-center">
          <span className="mx-[4px]">{t('of')}</span >
          {pager}
        </div>
        <span
          onClick={() =>
            currentPage !== totalPages && totalPages !== 0 && onPageChange(currentPage + 1)
          }
          className={`rounded-[3px] p-[2px] mx-[3px]  ${
            currentPage === totalPages || totalPages === 0 ? 'cursor-not-allowed bg-[var(--pager-button-background1)] [&>svg]:fill-[var(--pager-button-disable-color)]' : 'cursor-pointer bg-[var(--pager-button-background2)] hover:shadow-[var(--pager-button-shadow)]'
          }`}
        >
          <IconNext />
        </span>

        <span
          onClick={() => currentPage !== totalPages && totalPages !== 0 && onPageChange(totalPages)}
            className={`rounded-[3px] p-[2px] mx-[3px]  ${
            currentPage === totalPages || totalPages === 0 ? 'cursor-not-allowed bg-[var(--pager-button-background1)] [&>svg]:fill-[var(--pager-button-disable-color)]' : 'cursor-pointer bg-[var(--pager-button-background2)] hover:shadow-[var(--pager-button-shadow)]'
          }`}
        >
          <IconLast />
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
