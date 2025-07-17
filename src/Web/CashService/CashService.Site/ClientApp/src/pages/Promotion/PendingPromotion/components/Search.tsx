import React, { useState } from 'react';
import { IconCopy, IconExcel, IconSearch } from '../../../../components/Icons';
import Tippy from '@tippyjs/react';
import { useTranslation } from 'react-i18next';

interface SearchProps {
  search: string;
  setSearch: (val: string) => void;
  onDownload: () => void;
}

const Search: React.FC<SearchProps> = ({ search, setSearch, onDownload }) => {
  const { t: tGlobal } = useTranslation('global');
  const [textCopy, setTextCopy] = useState(tGlobal('copy'));
  const handleDownload = async () => {
    onDownload();
  };
  const handleCopy = () => {
    navigator.clipboard.writeText(search);
    setTextCopy(tGlobal('copied'));
  };
  return (
    <div className="flex items-center w-full justify-between">
      <div className=""></div>
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

        <div className="flex">
          <div className="w-[250px] ml-auto">
            <div className="relative">
              <span className="absolute transform-[translatey(-50%)] z-[10] top-1/2 left-[8px]">
                <IconSearch />
              </span>
              <input
                type="text"
                value={search}
                onChange={(e) => setSearch(e.target.value)}
                className="control-field pl-[30px]!"
                placeholder="Username/First & Last Name"
              />
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Search;
