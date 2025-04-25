import React from 'react';
import { SearchIcon, DownloadIcon, CopyIcon } from '../../../../components/Icons';
import { useTranslation } from 'react-i18next';
import '../../../../i18n/i18n';

interface SearchProps {
  value: string;
  onChange: (value: string) => void;
  placeholder?: string;
}

const Search: React.FC<SearchProps> = ({ value, onChange, placeholder }) => {
  const { t } = useTranslation('transactionHistory');

  const handleCopy = () => {
    // TODO: Implement copy functionality
  };

  const handleDownload = () => {
    // TODO: Implement download functionality
  };

  return (
    <div className="flex items-center w-full mb-2">
      <div className="text-sm text-gray-500 italic flex-shrink-0">
        *Up to 1000 latest records will be displayed
      </div>
      <div className="flex items-center space-x-2 ml-auto">
        <button
          onClick={handleDownload}
          className="p-1.5 hover:bg-gray-100 rounded-lg transition-colors"
          title="Download"
        >
          <DownloadIcon />
        </button>
        <button
          onClick={handleCopy}
          className="p-1.5 hover:bg-gray-100 rounded-lg transition-colors"
          title="Copy"
        >
          <CopyIcon />
        </button>
        <div className="relative">
          <div className="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
            <SearchIcon className="h-5 w-5 text-gray-400" />
          </div>
          <input
            type="text"
            value={value}
            onChange={(e) => onChange(e.target.value)}
            className="block w-full pl-10 pr-3 py-2 border border-gray-300 rounded-md leading-5 bg-white placeholder-gray-500 focus:outline-none focus:placeholder-gray-400 focus:ring-1 focus:ring-blue-500 focus:border-blue-500 sm:text-sm"
            placeholder={placeholder || t('filter.usernamePlaceholder')}
          />
        </div>
      </div>
    </div>
  );
};

export default Search;
