import { useState, useRef, useEffect } from 'react';
import { IconCopy, IconExcel, IconSearch } from '../../../../components/Icons';
import Tippy from '@tippyjs/react';
import { useTranslation } from 'react-i18next';

interface SearchProps {
  search: string;
  setSearch: (val: string) => void;
  onDownload: () => void;
}

export default function Search({ search, setSearch, onDownload }: SearchProps) {
  const { t: tGlobal } = useTranslation('global');
  const [textCopy, setTextCopy] = useState(tGlobal('copy'));
  const [isSearchActive, setIsSearchActive] = useState(false);
  const searchRef = useRef<HTMLDivElement>(null);

  const handleDownload = async () => {
    onDownload();
  };
  
  const handleCopy = () => {
    navigator.clipboard.writeText(search);
    setTextCopy(tGlobal('copied'));
  };

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
          <li className='uppercase relative inline-block after:content-["❯"] after:px-[8px] after:text-[#333] '>
            <a className='text-[#045ace] cursor-pointer'>Promotion Report</a>
          </li>
          <li className='text-[#9d1c1c] uppercase relative inline-block'>
            <span className=''>New Year Sale</span>
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
        <div ref={searchRef} className={`flex box-search relative ${isSearchActive ? 'active' : ''}`}>
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
