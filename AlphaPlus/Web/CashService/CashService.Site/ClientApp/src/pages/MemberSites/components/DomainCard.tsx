import React from 'react';
import { Domain } from '../../../services/memberSitesAPI';

interface DomainCardProps {
  domain: Domain;
}

const DomainCard: React.FC<DomainCardProps> = ({ domain }) => {
  return (
    <div className="bg-[#1e4d8c] text-white p-4 rounded-lg flex justify-between items-center">
      <div className="space-y-1">
        <h3 className="font-medium">{domain.url}</h3>
        {domain.qrCode && (
          <img src={domain.qrCode} alt="QR Code" className="w-24 h-24" />
        )}
      </div>
      <button className="bg-white text-[#1e4d8c] px-4 py-2 rounded text-sm hover:bg-gray-100 transition-colors">
        Download QR Code
      </button>
    </div>
  );
};

export default DomainCard; 