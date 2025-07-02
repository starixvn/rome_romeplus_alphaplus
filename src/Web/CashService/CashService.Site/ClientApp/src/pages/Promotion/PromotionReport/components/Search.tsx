import { useState, useRef, useEffect } from 'react';
import Tippy from '@tippyjs/react';
import { useTranslation } from 'react-i18next';
import { IconCopy, IconExcel, IconSearch } from '../../../../components/Icons';
import { copyHtmlToClipboard } from '../../../../utils/sharedUtils';
import useExportExcel from '../../../../hooks/useExportExcel';

interface SearchProps {
  promotionTitle: string;
  onClickBreadcrumb: () => void;
  tableRef: React.RefObject<HTMLTableElement | null>;
  search: string;
  setSearch: (val: string) => void;
  onDownload: () => void;
}

export default function Search({
  promotionTitle,
  onClickBreadcrumb,
  tableRef,
  search,
  setSearch,
  onDownload,
}: SearchProps) {
  const { t: tGlobal } = useTranslation('global');
  const [textCopy, setTextCopy] = useState(tGlobal('copy'));
  const [isSearchActive, setIsSearchActive] = useState(false);
  const searchRef = useRef<HTMLDivElement>(null);
  const downloadExcel = useExportExcel('ExportDetailsReport');

  async function handleDownload() {
    await downloadExcel();
    onDownload();
  }

  async function handleCopy() {
    await copyHtmlToClipboard(tableRef.current, () => {
      setTextCopy(tGlobal('copied'));
      setTimeout(() => setTextCopy(tGlobal('copy')), 3000);
    });
  }

  const handleSearchClick = () => {
    setIsSearchActive(!isSearchActive);
  };

  useEffect(() => {
    const handleClickOutside = (event: MouseEvent) => {
      if (searchRef.current && !searchRef.current.contains(event.target as Node)) {
        setIsSearchActive(false);
      }
    };

    document.addEventListener('mousedown', handleClickOutside);
    return () => {
      document.removeEventListener('mousedown', handleClickOutside);
    };
  }, []);

  return (
    <div className="flex items-center w-full justify-between">
      <div className="font-semibold mt-[1px]">
        <ul>
          <li className='uppercase relative inline-block after:content-["❯"] after:px-[8px] after:text-[#333]  dark:after:text-[#fff]'>
            <a
              href="#"
              className="text-[#045ace] dark:text-[#41a4ff] cursor-pointer"
              onClick={onClickBreadcrumb}
            >
              Promotion Report
            </a>
          </li>
          <li className="text-[#9d1c1c] dark:text-[#fff] uppercase relative inline-block">
            <span>{promotionTitle}</span>
          </li>
        </ul>
      </div>
      <div className="flex items-center gap-[8px]">
        <Tippy
          content={tGlobal('exportToExcel')}
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
        <div
          ref={searchRef}
          className={`flex box-search relative ${isSearchActive ? 'active' : ''}`}
        >
          <div className="w-[250px] max-[768px]:w-[28px] ml-auto">
            <div
              className="hidden max-[768px]:block bg-black/10 rounded-[3px] p-[6px] cursor-pointer"
              onClick={handleSearchClick}
            >
              <IconSearch />
            </div>
            <div className="relative max-[768px]:hidden child-search">
              <span className="absolute transform-[translatey(-50%)] z-[10] top-1/2 left-[8px] cursor-pointer">
                <IconSearch />
              </span>
              <input
                type="text"
                value={search}
                onChange={(e) => setSearch(e.target.value)}
                className="control-field pl-[30px]! max-[768px]:pl-[8px]!"
                placeholder="Promotion Title"
              />
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
