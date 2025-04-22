import React, { useState } from 'react';
import { Domain } from '../../../types/memberSite';

interface DomainCardProps {
  domain: Domain;
  index: number;
  className?: string;
  onDownloadQRCode?: () => void;
}

const DomainCard: React.FC<DomainCardProps> = ({ domain, className = '', index }) => {
  const [copied, setCopied] = useState(false);

  const handleCopy = async () => {
    try {
      await navigator.clipboard.writeText(domain.url);
      console.log("copied",copied);
      setCopied(true);
      setTimeout(() => setCopied(false), 2000);
    } catch (err) {
      console.error('Failed to copy text: ', err);
    }
  };

  return (
    <div className={`bg-[#1e4d8c] rounded-lg p-4 flex items-center justify-between ${className}`}>
      <div className="flex items-center flex-1 min-w-0">
        <div className="flex-1 min-w-0">
          <p className="text-white text-sm font-bold">Domain {index + 1}</p>
          <p className="text-white text-sm truncate">{domain.url}</p>
        </div>
        <button 
          onClick={handleCopy}
          className="ml-2 p-1.5 text-white hover:bg-[#2a5da0] rounded transition-colors"
          title="Copy URL"
        >
          <svg xmlns="http://www.w3.org/2000/svg" className="h-5 w-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M8 16H6a2 2 0 01-2-2V6a2 2 0 012-2h8a2 2 0 012 2v2m-6 12h8a2 2 0 002-2v-8a2 2 0 00-2-2h-8a2 2 0 00-2 2v8a2 2 0 002 2z" />
          </svg>
        </button>
      </div>
      <button 
        disabled={domain.status === 'blocked'}
        className={`bg-white text-[#1e4d8c] px-4 py-2 rounded text-sm font-medium whitespace-nowrap ${
          domain.status === 'active'
            ? 'hover:bg-gray-50'
            : 'opacity-50 cursor-not-allowed'
        }`}
      >
        Download QR Code
      </button>
    </div>
  );
};

export default DomainCard; 