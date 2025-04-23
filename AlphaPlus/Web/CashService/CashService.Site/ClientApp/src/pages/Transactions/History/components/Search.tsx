import React from 'react';
import { SearchIcon, DownloadIcon, CopyIcon } from '../../../../components/Icons';

interface SearchProps {
  value: string;
  onChange: (value: string) => void;
  placeholder?: string;
}

const Search: React.FC<SearchProps> = ({
  value,
  onChange,
  placeholder = 'Username/Reference No.',
}) => {
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
            <SearchIcon />
          </div>
          <input
            type="text"
            value={value}
            onChange={(e) => onChange(e.target.value)}
            placeholder={placeholder}
            className="pl-10 pr-4 py-1.5 w-[240px] border border-gray-300 rounded-lg focus:outline-none focus:ring-1 focus:ring-blue-500 focus:border-blue-500 text-sm"
          />
        </div>
      </div>
    </div>
  );
};

export default Search;
