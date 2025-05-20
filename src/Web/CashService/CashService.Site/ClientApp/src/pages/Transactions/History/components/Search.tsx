import React from 'react';
import { SearchIcon, DownloadIcon, CopyIcon } from '../../../../components/Icons';
import { useTranslation } from 'react-i18next';

interface SearchProps {
  value: string;
  onChange: (value: string) => void;
  placeholder?: string;
}

const Search: React.FC<SearchProps> = ({ value, onChange }) => {
  const { t } = useTranslation('transactionHistory');

  const handleCopy = () => {
    // TODO: Implement copy functionality
  };

  const handleDownload = () => {
    // TODO: Implement download functionality
  };

  return (
    <div className="flex items-center w-full">
      <div className="text-sm text-gray-500 italic flex-shrink-0">
        *Up to 5000 latest records will be displayed
      </div>
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
