import React, { useState, useRef } from 'react';
import { Domain } from '../../../services/interfaces/MemberSites';
import { QRCodeCanvas } from 'qrcode.react';
import { useTranslation } from 'react-i18next';
import { QR_CODE_SIZE } from '../../../constants';
import './DomainCard.scss';
import { IconCopied, IconCopy } from '../../../components/Icons';
interface DomainCardProps {
  domain: Domain;
  index: number;
}

const DomainCard: React.FC<DomainCardProps> = ({ domain,  index }) => {
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
    <div className={`bg-[var(--domain-card-color1)] justify-between rounded-[5px] flex-1 text-[var(--domain-card-color2)] flex flex-nowrap p-[16px] max-w-[450px]`}>
      <div className="flex justify-between items-center  ">
        <div className="">
          <p className="font-bold text-[110%]">{t('registrationLinks.domain')} {index + 1}</p>
          <p>{domain.url}</p>
        </div>        
      </div>
      <div className="flex items-center ">
        <button 
            onClick={(e) => handleCopy(e)}
            className="cursor-pointer p-1.5 rounded-[4px] hover:bg-[var(--button-icon-hover)] transition-colors"
            title="Copy URL"
          >
            {copied ? (
              <IconCopied className="h-5 w-5" />
            ) : (
              <IconCopy className="h-5 w-5" />
            )}
          </button>
          <button 
          disabled={domain.status === 'blocked'}
          onClick={(e) => downloadQRCode(e)}
          className={`cursor-pointer whitespace-nowrap text-[var(--domain-card-color1)] font-bold bg-[var(--domain-card-color2)] rounded-[5px] p-[6px]  ${
            domain.status === 'active'
              ? 'hover:opacity-80'
              : 'opacity-50 cursor-not-allowed'
          }`}
        >
          {t('registrationLinks.downloadQRCode')}
        </button>
      </div>
      
      
      {/* Hidden QR Code for download */}
      <div ref={qrRef} className="hidden">
        <QRCodeCanvas value={domain.url} size={QR_CODE_SIZE} />
      </div>
    </div>
  );
};

export default DomainCard; 