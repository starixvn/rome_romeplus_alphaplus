import React, { useState, useRef } from 'react';
import { Domain } from '../../../types/memberSite';
import { QRCodeCanvas } from 'qrcode.react';
import { useTranslation } from 'react-i18next';

interface DomainCardProps {
  domain: Domain;
  index: number;
  className?: string;
}

const DomainCard: React.FC<DomainCardProps> = ({ domain, className = '', index }) => {
  const { t } = useTranslation('memberSite');
  const [copied, setCopied] = useState(false);
  const qrRef = useRef<HTMLDivElement>(null);

  const handleCopy = async (e: any) => {
    e.preventDefault();
    try {
      await navigator.clipboard.writeText(domain.url);
      setCopied(true);
      setTimeout(() => setCopied(false), 700);
    } catch (err) {
      console.error('Failed to copy text: ', err);
    }
  };

  const downloadQRCode = (e: any) => {
    e.preventDefault();
    const canvas = qrRef.current?.querySelector("canvas");
    if (!canvas) return;

    const url = canvas.toDataURL("image/png");
    const link = document.createElement("a");
    link.href = url;
    link.download = "qrcode.png";
    link.click();
  };


  return (
    <div className={`bg-[#1e4d8c] rounded-lg p-4 flex items-center justify-between ${className}`}>
      <div className="flex items-center flex-1 min-w-0">
        <div className="flex-1 min-w-0">
          <p className="text-white text-sm font-bold">{t('registrationLinks.domain')} {index + 1}</p>
          <p className="text-white text-sm truncate">{domain.url}</p>
        </div>
        <button 
          onClick={(e) => handleCopy(e)}
          className="ml-2 p-1.5 text-white hover:bg-[#2a5da0] rounded transition-colors"
          title="Copy URL"
        >
          {copied ? (
            <svg xmlns="http://www.w3.org/2000/svg" className="h-5 w-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
              <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M5 13l4 4L19 7" />
            </svg>
          ) : (
            <svg xmlns="http://www.w3.org/2000/svg" className="h-5 w-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
              <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M8 16H6a2 2 0 01-2-2V6a2 2 0 012-2h8a2 2 0 012 2v2m-6 12h8a2 2 0 002-2v-8a2 2 0 00-2-2h-8a2 2 0 00-2 2v8a2 2 0 002 2z" />
            </svg>
          )}
        </button>
      </div>
      <button 
        disabled={domain.status === 'blocked'}
        onClick={(e) => downloadQRCode(e)}
        className={`bg-white text-[#1e4d8c] px-4 py-2 rounded text-sm font-medium whitespace-nowrap ${
          domain.status === 'active'
            ? 'hover:bg-gray-50'
            : 'opacity-50 cursor-not-allowed'
        }`}
      >
        {t('registrationLinks.downloadQRCode')}
      </button>
      
      {/* Hidden QR Code for download */}
      <div ref={qrRef} className="hidden">
        <QRCodeCanvas value={domain.url} size={256} />
      </div>
    </div>
  );
};

export default DomainCard; 